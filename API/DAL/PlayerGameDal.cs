using System.Data.SqlClient;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.DAL
{
    public class PlayerGameDal : IPlayerGameDal
    {
        public ActionResult<bool> JoinGame(Player player, int gameId)
        {
            int result;
            using (var sqlConnection = new SqlConnection(Connection.ConnectionString))
            {
                const string query =
                    "UPDATE Game" +
                    "SET WhitePlayerAvatar = @WhitePlayerAvatar" +
                    "WHERE Id = @Id;" +
                    "UPDATE Player" +
                    "SET CurrentlyPlayingGameId = @Id" +
                    "WHERE Avatar = @WhitePlayerAvatar";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@WhitePlayerAvatar", player.Avatar);
                sqlCommand.Parameters.AddWithValue("@Id", gameId);

                sqlConnection.Open();
                result = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            return result == 1;
        }
    }
}
