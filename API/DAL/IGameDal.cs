using System.Collections.Generic;
using API.Models;

namespace API.DAL
{
    /// <summary>
    /// Interface of the Game Data Access Layer to be able to mock the interface
    /// </summary>
    public interface IGameDal
    {
        /// <summary>
        /// Get the games that you can join
        /// </summary>
        /// <returns>The games that you can join</returns>
        IEnumerable<Game> GetAvailableGames();

        /// <summary>
        /// Get a certain game, declared with the ID
        /// </summary>
        /// <param name="id">The ID of the game that you want to get</param>
        /// <returns>The game that you want to get</returns>
        Game GetGame(int id);

        /// <summary>
        /// Create a game
        /// </summary>
        /// <param name="blackPlayer">The black player: the player that requested to make a new game</param>
        /// <param name="description">A description of the game</param>
        /// <returns>If the game was created successfully</returns>
        bool CreateGame(Player blackPlayer, string description);
    }
}
