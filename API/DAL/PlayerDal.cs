using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Norgerman.Cryptography.Scrypt;

namespace API.DAL
{
    /// <inheritdoc />
    /// <summary>
    /// Data Access Layer for players and other users
    /// </summary>
    public class PlayerDal : IPlayerDal
    {
        /// <summary>
        /// Process salt bytes to a database processable salt data string
        /// </summary>
        /// <param name="byteArray">The original salt bytes that the application can validate against</param>
        /// <returns>The object that goes to the SQL Data Reader salt array item</returns>
        public string ByteArrayToString(byte[] byteArray) =>
            //new string(byteArray.Select(b => (char) b).ToArray());
            BitConverter.ToString(byteArray);

        /// <summary>
        /// Process the raw VARCHAR(255) salt data to the original salt bytes
        /// </summary>
        /// <param name="rawDbData">The object that comes from the SQL Data Reader salt array item</param>
        /// <returns>The original salt bytes that the application can validate against</returns>
        //public byte[] StringToByteArray(string rawDbData) => rawDbData.Select(c => (byte)c).ToArray();
        public byte[] StringToByteArray(string rawDbData)
        {
            var strArray = rawDbData.Split('-');
            var byteArray = new byte[strArray.Length];
            for (var i = 0; i < strArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(strArray[i], 16);
            }

            return byteArray;
        }

        /// <inheritdoc />
        public bool AddPlayer(Player player)
        {
            player.Salt = MakeSalt();
            player.PassPhrase = HashPassPhrase(player.PassPhrase, player.Salt);

            // Add the player to the DB
            int result;
            using (var sqlConnection = new SqlConnection(Connection.ConnectionString))
            {
                const string query =
                    "INSERT INTO Player (Avatar, EmailAddress, PassPhrase, Salt, UserRole) " +
                    "VALUES (@Avatar, @EmailAddress, @PassPhrase, @Salt, @UserRole)";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Avatar", player.Avatar);
                sqlCommand.Parameters.AddWithValue("@EmailAddress", player.EmailAddress);
                sqlCommand.Parameters.AddWithValue("@PassPhrase", player.PassPhrase);
                sqlCommand.Parameters.AddWithValue("@Salt", ByteArrayToString(player.Salt));
                sqlCommand.Parameters.AddWithValue("@UserRole", player.UserRole);

                sqlConnection.Open();
                result = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            return result == 1;
        }

        /// <inheritdoc />
        public async Task<Player> GetPlayer(HttpContext httpContext, string avatar, string emailAddress,
            string passPhrase)
        {
            // Look for the player with the given credentials, return null when there is none
            var player = FindPlayerInDatabase(avatar, emailAddress);
            if (player == null)
            {
                return null;
            }

            // Get user's salt
            var salt = player.Salt;

            // Return the player if the password is OK, return null otherwise
            var hashedPassPhrase = HashPassPhrase(passPhrase, salt);
            if (hashedPassPhrase.Equals(player.PassPhrase))
            {
                // add ClaimsIdentity
                var identity = new ClaimsIdentity(GetUserClaims(player),
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return player;
            }
            else
            {
                Console.WriteLine("password doesn't match");
                return null;
            }
        }

        /// <summary>
        /// Get the Claims Based Access Control claims for this user
        /// </summary>
        /// <param name="player">The user to get claims from</param>
        /// <returns>A list of claims: name, email, roles</returns>
        public IEnumerable<Claim> GetUserClaims(Player player)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, player.Avatar),
                new Claim(ClaimTypes.Name, player.Avatar),
                new Claim(ClaimTypes.Email, player.EmailAddress)
            };
            claims.AddRange(GetUserRoleClaims(player));
            return claims;
        }

        public Player GetUserFromClaims(HttpContext httpContext)
        {
            var claims = httpContext.User.Claims;
            var claimsArray = claims as Claim[] ?? claims.ToArray();
            return new Player
            {
                Avatar = claimsArray.Where(c => c.Type.Equals(ClaimTypes.Name)).Select(c => c.Value).Single(),
                EmailAddress = claimsArray.Where(c => c.Type.Equals(ClaimTypes.Email)).Select(c => c.Value).Single(),
                UserRole = claimsArray.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(c => c.Value).Single()
            };
        }

        /// <summary>
        /// Get the Claims Based Access Control role claims from this user
        /// </summary>
        /// <param name="player">The user to get role claims from</param>
        /// <returns>A list of user role claims: the avatar and the role</returns>
        private IEnumerable<Claim> GetUserRoleClaims(Player player)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, player.Avatar),
                new Claim(ClaimTypes.Role, player.UserRole)
            };
        }

        /// <summary>
        /// Look for the database (which is currently just a dummy list) record with the given avatar or email address
        /// </summary>
        /// <param name="avatar">The nickname the user chose. Can be null or empty when there is an email address</param>
        /// <param name="emailAddress">The email address. Can be null or empty when there is an avatar</param>
        /// <returns>
        /// The player matching the given avatar or email address.
        /// No matching result = null
        /// Neither avatar nor email address given = also null
        /// </returns>
        private Player FindPlayerInDatabase(string avatar, string emailAddress)
        {
            if (avatar != null && !avatar.Equals(string.Empty))
            {
                return FindPlayerWithAvatar(avatar);
            }
            else if (emailAddress != null && !emailAddress.Equals(string.Empty))
            {
                return FindPlayerWithEmailAddress(emailAddress);
            }
            // No avatar or email address given = null
            else
            {
                return null;
            }
        }

        /// <summary>
        /// If the user logs in with email address, get the player from the database with that address
        /// </summary>
        /// <param name="emailAddress">The email address the user wants to login with</param>
        /// <returns>
        /// The player whose email address equals to the given email address.
        /// If there is none (the user entered a wrong email address), return null
        /// </returns>
        private Player FindPlayerWithEmailAddress(string emailAddress)
        {
            var player = new Player();
            using (var sqlConnection = new SqlConnection(Connection.ConnectionString))
            {
                const string query = "SELECT Avatar, PassPhrase, Salt, CurrentlyPlayingGameId, UserRole " +
                                     "FROM Player " +
                                     "WHERE EmailAddress = @EmailAddress";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EmailAddress", emailAddress);
                sqlConnection.Open();
                var sqlDataReader = sqlCommand.ExecuteReader();

                if (!sqlDataReader.Read()) return player;

                player.Avatar = sqlDataReader["Avatar"].ToString();
                player.EmailAddress = emailAddress;
                player.PassPhrase = sqlDataReader["PassPhrase"].ToString();
                player.Salt = StringToByteArray(sqlDataReader["Salt"].ToString());
                player.CurrentlyPlayingGame = new Game(); // TODO: implement getting the game from db
                player.UserRole = sqlDataReader["UserRole"].ToString();
                sqlConnection.Close();
            }

            return player;
        }

        /// <summary>
        /// If the user logs in with avatar, get the player from the database with that avatar
        /// </summary>
        /// <param name="avatar">The avatar the user wants to login with</param>
        /// <returns>
        /// The player whose avatar equals to the given avatar.
        /// If there is none (the user entered a wrong avatar), return null
        /// </returns>
        private Player FindPlayerWithAvatar(string avatar)
        {
            var player = new Player();
            using (var sqlConnection = new SqlConnection(Connection.ConnectionString))
            {
                const string query = "SELECT EmailAddress, PassPhrase, Salt, CurrentlyPlayingGameId, UserRole " +
                                     "FROM Player " +
                                     "WHERE Avatar = @Avatar";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Avatar", avatar);
                sqlConnection.Open();
                var sqlDataReader = sqlCommand.ExecuteReader();

                if (!sqlDataReader.Read()) return player;

                player.Avatar = avatar;
                player.EmailAddress = sqlDataReader["EmailAddress"].ToString();
                player.PassPhrase = sqlDataReader["PassPhrase"].ToString();
                player.Salt = StringToByteArray(sqlDataReader["Salt"].ToString());
                player.CurrentlyPlayingGame = new Game(); // TODO: implement getting the game from db
                player.UserRole = sqlDataReader["UserRole"].ToString();
                sqlConnection.Close();
            }

            return player;
        }

        /// <inheritdoc />
        public async void Logout(HttpContext httpContext) => await httpContext.SignOutAsync();

        /// <summary>
        /// Make a salt for the hash
        /// </summary>
        /// <returns>A randomly generated salt that is 16 bytes long</returns>
        private byte[] MakeSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Hash an unhashed pass phrase
        /// </summary>
        /// <param name="passPhrase">The unhashed pass phrase</param>
        /// <param name="salt">
        /// The salt to be added.
        /// When making a new user, it will be randomly generated
        /// When logging in for an existing user, it will be taken from a salt database
        /// </param>
        /// <returns>The SCrypt hashed pass phrase</returns>
        public string HashPassPhrase(string passPhrase, byte[] salt)
        {
            var password = Encoding.UTF8.GetBytes(passPhrase);
            var hashed = Convert.ToBase64String(ScryptUtil.Scrypt(
                password: password,
                salt: salt,
                // N = CPU/memory cost parameter
                N: 262144,
                // r = block size parameter (8 is common, so we do that too)
                r: 8,
                // p = parallelisation parameter, let's do 1 because it is recommended by CryptSharp
                p: 1,
                // dkLen = intended output length,
                // we do 256 / 8 = 32
                // because Microsoft has that number too
                // in their PBKDF2 tutorial using the built in ASP.NET cryptography tool
                dkLen: 256 / 8));
            return hashed;
        }
    }
}