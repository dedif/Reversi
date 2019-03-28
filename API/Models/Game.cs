using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    /// <summary>
    /// Game model
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The ID of the game
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Who plays the white checkers, he goes second. He is the guest of the game
        /// </summary>
        public Player WhitePlayer { get; set; }

        /// <summary>
        /// Who plays the black checkers, he goes first. He is the host of the game
        /// </summary>
        [Required]
        [DisplayName("Zwarte speler")]
        public Player BlackPlayer { get; set; }

        /// <summary>
        /// Description of the game.
        /// Can be max 1000 chars long to avoid extremely long names taking up lots of storage.
        /// </summary>
        [Required]
        [MaxLength(1000, ErrorMessage = "Beschrijving mag maximaal 1000 tekens lang zijn")]
        [DisplayName("Beschrijving")]
        public string Description { get; set; }
    }
}