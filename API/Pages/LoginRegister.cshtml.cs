using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using API.DAL;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages
{
    /// <summary>
    /// The page to login and register a user
    /// </summary>
    public class LoginRegisterModel : PageModel
    {
        /// <summary>
        /// Mock-able Data Access Layer for players
        /// </summary>
        private IPlayerDal _playerDal;

        /// <summary>
        /// Constructor: init the player Data Access Layer
        /// </summary>
        /// <param name="apiDal">The Data Access Layer to be dependency injected</param>
        public LoginRegisterModel(IPlayerDal apiDal) => _playerDal = apiDal;

        /// <summary>
        /// The player model whose properties are bound to the register form
        /// </summary>
        [BindProperty]
        public Player Player { get; set; }

        /// <summary>
        /// The error message that will appear when something goes wrong upon registering
        /// </summary>
        public string RegisterErrorMessage { get; set; }

        /// <summary>
        /// The avatar property that's bound to the login form avatar field
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// The email address property that's bound to the login form email address field
        /// </summary>
        [DisplayName("E-mailadres")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The pass phrase property that's bound to the login form pass phrase field
        /// </summary>
        [DisplayName("Wachtzin")]
        public string PassPhrase { get; set; }

        /// <summary>
        /// The error message that will appear when something goes wrong upon logging in
        /// </summary>
        [ViewData]
        public string LoginErrorMessage { get; set; }

        /// <summary>
        /// When opening this page: instantiate the player object
        /// </summary>
        public void OnGet() => Player = new Player();

        /// <summary>
        /// When the user clicks on the Register button
        /// </summary>
        /// <returns>A redirect to this page so the user can login</returns>
        public ActionResult OnPostRegister()
        {
            _playerDal.AddPlayer(Player);
            return Page();
        }

        /// <summary>
        /// When the user clicks on the login button
        /// </summary>
        /// <param name="avatar">The avatar the user might have entered</param>
        /// <param name="emailAddress">The email address the user might have entered</param>
        /// <param name="passPhrase">The pass phrase the user entered</param>
        /// <returns>A return to the index.html page</returns>
        public async Task<IActionResult> OnPostLogin(string avatar, string emailAddress, string passPhrase)
        {
            Console.WriteLine(avatar);
            Player = await _playerDal.GetPlayer(HttpContext, avatar, emailAddress, passPhrase);
            if (Player == null)
            {
                Console.WriteLine("Niet ingelogd");
                LoginErrorMessage = "Gebruiker niet gevonden";
                return Page();
            }
            else
            {
                Console.WriteLine("Ingelogd");
                return RedirectToPage("./Game");
            }
        }
    }
}