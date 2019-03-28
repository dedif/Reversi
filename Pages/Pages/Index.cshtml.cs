using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pages.ApiDal;
using Pages.Models;

namespace Pages.Pages
{
    public class IndexModel : PageModel
    {
        private IApiDal _apiDal;

        public IndexModel(IApiDal apiDal) => _apiDal = apiDal;

        [BindProperty]
        public PlayerDto PlayerDto { get; set; }

        public string RegisterErrorMessage { get; set; }

        public string Avatar { get; set; }

        [DisplayName("E-mailadres")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DisplayName("Wachtzin")]
        public string PassPhrase { get; set; }

        public string LoginErrorMessage { get; set; }

        public void OnGet() => PlayerDto = new PlayerDto();

        public ActionResult OnPostRegister()
        {
            _apiDal.Create(PlayerDto, "api/player/register");
            return Page();
        }

        public async Task<IActionResult> OnPostLogin(string avatar, string emailAddress, string passPhrase)
        {
            Console.WriteLine(avatar);
            PlayerDto = await _apiDal.GetPlayer(avatar, emailAddress, passPhrase);
            if (PlayerDto == null)
            {
                LoginErrorMessage = "Gebruiker niet gevonden";
            }
            return RedirectToPage("./Game");
        }
    }
}