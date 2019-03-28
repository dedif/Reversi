using System.Collections.Generic;
using System.Linq;
using API.DAL;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Application Programmable Interface controller for game objects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        /// <summary>
        /// Data access layer for games. Handles the database.
        /// Instance is given via dependency injection
        /// Object is an interface to be mockable
        /// </summary>
        private IGameDal _gameDal;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameDal">Data access layer for games. Parsed via dependency injection.</param>
        public GameController(IGameDal gameDal) => _gameDal = gameDal;

        /// <summary>
        /// Method called upon HTTP GET api/game/games
        /// </summary>
        /// <returns>All games that can currently be played</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetAvailableGames() => _gameDal.GetAvailableGames().ToList();

        /// <summary>
        /// Method called upon HTTP POST api/game/{id}.
        /// Call it when you want to get one certain game
        /// </summary>
        /// <param name="id">The ID of the game that has been requested</param>
        /// <returns>The game with the corresponding ID</returns>
        [HttpGet("{id}")]
        public ActionResult<Game> GetGame(int id) => _gameDal.GetGame(id);

        /// <summary>
        /// Method called upon POST api/game
        /// Create a new game
        /// </summary>
        /// <param name="blackPlayer">
        /// The black player begins.
        /// This player is also the player that sent the request to create a new game.
        /// </param>
        /// <returns>Return true if the method succeeded</returns>
        [HttpPost]
        public ActionResult<bool> CreateGame([FromBody] Player blackPlayer) =>
            _gameDal.CreateGame(blackPlayer, "x");
    }
}