using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using API.Models;

namespace Pages.Models
{
    public class PlayerDto
    {
        [Required(ErrorMessage = "Dit veld is vereist")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Dit veld is vereist")]
        [DisplayName("E-mailadres")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Dit veld is vereist")]
        [DisplayName("Wachtzin")]
        [MinLength(15, ErrorMessage = "Wachtzin moet minimaal 15 tekens lang zijn")]
        [MaxLength(255, ErrorMessage = "Wachtzin mag maximaal 255 tekens lang zijn")]
        [RegularExpression(@"(?!.*(.+)\1{2}.*)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9\\s])^.*$", ErrorMessage =
            "Je wachtzin moet bestaan uit hoofdletters, kleine letters, cijfers, en speciale tekens en mag geen herhalende reeksen bevatten")]
        public string PassPhrase { get; set; }

        public Game CurrentlyPlayingGame { get; set; }

        public string UserRole { get; set; } = "Admin";
    }
}