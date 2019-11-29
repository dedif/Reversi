using System.Collections.Generic;
using System.Linq;
using API.DAL;
using API.Models;
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

        private IPlayerDal _playerDal;
        private IPlayerGameDal _playerGameDal;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameDal">Data access layer for games. Parsed via dependency injection.</param>
        /// <param name="playerDal"></param>
        public GameController(IGameDal gameDal, IPlayerDal playerDal, IPlayerGameDal playerGameDal)
        {
            _gameDal = gameDal;
            _playerDal = playerDal;
            _playerGameDal = playerGameDal;
        }

        /// <summary>
        /// Method called upon HTTP GET api/game/games
        /// </summary>
        /// <returns>All games that can currently be played</returns>
        [HttpGet("games")]
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
        /// Method called upon POST api/game/create
        /// Create a new game
        /// </summary>
        /// <param name="description">Description of the game</param>
        /// <returns>Return true if the method succeeded</returns>
        [HttpPost("create")]
        public ActionResult<bool> CreateGame([FromBody] string description) =>
            _gameDal.CreateGame(_playerDal.GetUserFromClaims(HttpContext), description);

        [HttpPost("join")]
        public ActionResult<bool> JoinGame([FromBody] int gameId)
        {
            return _playerGameDal.JoinGame(_playerDal.GetUserFromClaims(HttpContext), gameId);
        }
    }
}