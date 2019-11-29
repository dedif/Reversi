using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;

namespace API.DAL
{
    /// <summary>
    /// Data Access Layer for players and other users
    /// Implementable to be mock-able
    /// </summary>
    public interface IPlayerDal
    {
        /// <summary>
        /// Add a player to the database
        /// </summary>
        /// <param name="player">The player to be added</param>
        /// <returns>
        /// True if the player can be added, false if the player cannot because the username is already claimed
        /// </returns>
        bool AddPlayer(Player player);

        /// <summary>
        /// Login a player with given credentials
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="avatar">
        /// The avatar of the logged in player.
        /// Can be null or empty if the email address is given
        /// </param>
        /// <param name="emailAddress">
        /// The email address of the logged in player.
        /// Can be null or empty if the avatar is given
        /// </param>
        /// <param name="passPhrase">
        /// The pass phrase of the user.
        /// Like a password, but longer.
        /// The password is not hashed at this point
        /// </param>
        /// <returns>The player if all credentials are valid. Else null.</returns>
        Player GetPlayer(HttpContext httpContext, string avatar, string emailAddress, string passPhrase);

        /// <summary>
        /// Sign out of the application
        /// </summary>
        /// <param name="httpContext">
        /// The context (<see cref="T:Microsoft.AspNetCore.Http.HttpContext" />) that carries the info about the HTTP request,
        /// the <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions"/> class
        /// needs a HttpContext for its extension method
        /// </param>
        void Logout(HttpContext httpContext);

        IEnumerable<Claim> GetUserClaims(Player player);

        Player GetUserFromClaims(HttpContext httpContext);
    }
}