using System;
using System.Linq;
using API.DAL;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages
{
    public class CreateGameModel : PageModel
    {
        /// <summary>
        /// Data Access Layer for games
        /// </summary>
        private IGameDal _gameDal;

        private IPlayerDal _playerDal;

        /// <summary>
        /// Constructor to dependency inject the game data access layer
        /// </summary>
        /// <param name="gameDal">The dependency injected game data access layer</param>
        /// <param name="playerDal">The dependency injected player data access layer</param>
        public CreateGameModel(IGameDal gameDal, IPlayerDal playerDal)
        {
            _gameDal = gameDal;
            _playerDal = playerDal;
        }

        /// <summary>
        /// The game to be made
        /// </summary>
        [BindProperty] public Game Game { get; set; }

        /// <summary>
        /// When the page is opened
        /// </summary>
        public void OnGet()
        {

        }

        public IActionResult OnPost([Bind("Description")] Game game)
        {
            // The creator of the game is the host and therefor the black player
            var x = HttpContext.User;
            foreach (var userClaim in HttpContext.User.Claims)
            {
                var y = userClaim;
                var z = userClaim.Type.Split("/").Last();
//                foreach (var VARIABLE in Model.GetType())
//                {
//                    
//                }
                Console.WriteLine(userClaim);
            }

            game.BlackPlayer = new Player
            {
                Avatar = HttpContext.User.Identity.Name,
                CurrentlyPlayingGame = game
            };
            
            Console.WriteLine(game.Description);
            _gameDal.CreateGame(game.BlackPlayer, game.Description);
            return Redirect("~/index.html");
        }
    }
}