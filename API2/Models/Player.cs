using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    /// <summary>
    /// The player model
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The avatar of the player
        /// Maximum length is 255 to be able to fit in db
        /// </summary>
        [Required(ErrorMessage = "Dit veld is vereist")]
        [MaxLength(255, ErrorMessage = "Dit veld mag niet meer dan 255 tekens bevatten")]
        public string Avatar { get; set; }

        /// <summary>
        /// The email address of the player.
        /// <see cref="T:System.ComponentModel.DataAnnotations.EmailAddressAttribute"/>
        /// verifies if it's really an email address.
        /// Maximum length is 255 to be able to fit in db
        /// </summary>
        [Required(ErrorMessage = "Dit veld is vereist")]
        [EmailAddress]
        [MaxLength(255, ErrorMessage = "Dit veld mag niet meer dan 255 tekens bevatten")]
        [DisplayName("E-mailadres")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The pass phrase of the player
        /// Minimum and maximum length,
        /// containment of capitals, small letters, numbers and special characters
        /// and abundance of repeating parts
        /// are verified.
        /// </summary>
        [DisplayName("Wachtzin")]
        [MinLength(15, ErrorMessage = "Wachtzin moet minimaal 15 tekens lang zijn")]
        [MaxLength(255, ErrorMessage = "Wachtzin mag maximaal 255 tekens lang zijn")]
        [RegularExpression(@"(?!.*(.+)\1{2}.*)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9\\s])^.*$", ErrorMessage =
            "Je wachtzin moet bestaan uit hoofdletters, kleine letters, cijfers, en speciale tekens en mag geen herhalende reeksen bevatten")]
        public string PassPhrase { get; set; }

        /// <summary>
        /// The salt of the player's hash
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// The game the user currently plays. Doesn't have to be set.
        /// </summary>
        public Game CurrentlyPlayingGame { get; set; }

        /// <summary>
        /// The role of the user
        /// </summary>
        public string UserRole { get; set; } = "Admin";//TODO: Change to not being an admin by default
    }
}