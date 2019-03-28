using System.Collections.Generic;
using System.Data.SqlClient;
using API.Models;

namespace API.DAL
{
    /// <inheritdoc />
    /// <summary>
    /// Data Access Layer for games.
    /// Implements IGameDal so that this class can be mocked.
    /// </summary>
    public class GameDal : IGameDal
    {
        /// <inheritdoc />
        /// <summary>
        /// Dummy method to return an empty IEnumerable (an array) of empty games.
        /// </summary>
        /// <returns>Empty dummy array of empty games</returns>
        public IEnumerable<Game> GetAvailableGames()
        {
            var games = new List<Game>();
            using (var sqlConnection = new SqlConnection(Connection.ConnectionString))
            {
                const string query = "SELECT Id, BlackPlayerAvatar, Description " +
                                     "FROM Game " +
                                     "WHERE WhitePlayerAvatar IS NULL";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                var sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    games.Add(new Game
                    {
                        Id = int.Parse(sqlDataReader["Id"].ToString()),
                        BlackPlayer = new Player { Avatar = sqlDataReader["BlackPlayerAvatar"].ToString() },
                        Description = sqlDataReader["Description"].ToString()
                    });
                }
            }

            return games;
        }

        /// <inheritdoc />
        /// <summary>
        /// Dummy method to return an empty game
        /// </summary>
        /// <param name="id">An empty dummy game</param>
        /// <returns></returns>
        public Game GetGame(int id) => new Game();

        /// <inheritdoc />
        /// <summary>
        /// Dummy method to create a game
        /// </summary>
        /// <param name="blackPlayer">The black player: the player that requested to make a new game</param>
        /// <param name="description">A description of the game</param>
        /// <returns>If the game was created successfully (for now, always, because it's a dummy method)</returns>
        public bool CreateGame(Player blackPlayer, string description)
        {
            int result;
            using (var sqlConnection = new SqlConnection(Connection.ConnectionString))
            {
                const string query =
                    "INSERT INTO Game (BlackPlayerAvatar, Description) " +
                    "VALUES (@BlackPlayerAvatar, @Description)";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@BlackPlayerAvatar", blackPlayer.Avatar);
                sqlCommand.Parameters.AddWithValue("@Description", description);

                sqlConnection.Open();
                result = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            return result == 1;
        }
    }
}