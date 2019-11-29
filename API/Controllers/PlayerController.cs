using API.DAL;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Application Programmable Interface controller for player objects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        /// <summary>
        /// Data access layer for players. Handles the database.
        /// Instance is given via dependency injection
        /// Object is an interface to be mockable
        /// </summary>
        private readonly IPlayerDal _playerDal;

        public PlayerController(IPlayerDal playerDal) => _playerDal = playerDal;

        // POST api/player/isLoggedIn
        [HttpGet("isLoggedIn")]
        public bool IsLoggedIn() => Request.Cookies.ContainsKey(".AspNetCore.Cookies");

        // POST api/player/getLoggedInPlayer
        [HttpGet("getLoggedInPlayer")]
        public ActionResult<Player> GetLoggedInPlayer() => _playerDal.GetUserFromClaims(HttpContext);

        // POST api/player/login
        [HttpPost("login")]
        public bool Login(LoginViewModel loginViewModel)
        {
            var player = _playerDal.GetPlayer(HttpContext, loginViewModel.Avatar, loginViewModel.EmailAddress, loginViewModel.PassPhrase);
            return player != null;
        }

        // POST api/player/register
        [HttpPost("register")]
        public ActionResult<bool> Register([FromBody] Player player)
        {
            var modelIsValid = TryValidateModel(player);
            var playerIsAdded = _playerDal.AddPlayer(player);
            return modelIsValid && playerIsAdded;
        }

        [HttpPost("logout")]
        public void Logout() => _playerDal.Logout(HttpContext);
    }
}