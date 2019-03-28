using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using API.DAL;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages
{
    /// <summary>
    /// CRUD Read page for games
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class GameModel : PageModel
    {
        /// <summary>
        /// Data Access Layer for games
        /// </summary>
        private IGameDal _gameDal;

        /// <summary>
        /// Constructor to dependency inject the game data access layer
        /// </summary>
        /// <param name="gameDal">The dependency injected game data access layer</param>
        public GameModel(IGameDal gameDal) => _gameDal = gameDal;

        /// <summary>
        /// The list of games
        /// </summary>
        public IList<Game> AvailableGames { get; set; }

        /// <summary>
        /// When the game page is opened
        /// </summary>
        public void OnGet() => AvailableGames = _gameDal.GetAvailableGames().ToList();

        /// <summary>
        /// When the user clics on "Join Game"
        /// </summary>
        /// <param name="id">The game ID</param>
        /// <returns>A return to the index.html in the wwwroot</returns>
        public IActionResult OnGetJoin(int id)
        {
            var x = ClaimsPrincipal.Current;
            Console.WriteLine(x.Claims);
            Console.WriteLine(x.Identities);
            return Redirect("~/index.html");
        }
    }
}