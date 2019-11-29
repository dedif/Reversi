namespace API.Models
{
    /// <summary>
    /// Viewmodel parsed when logging in
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The avatar. Optional when email address is given
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// The email address. Optional when avatar is given
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// The pass phrase. Not yet encrypted.
        /// </summary>
        public string PassPhrase { get; set; }
    }
}