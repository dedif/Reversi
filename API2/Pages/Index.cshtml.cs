using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Norgerman.Cryptography.Scrypt;

namespace API2.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            Console.WriteLine(HashPassPhrase("test", MakeSalt()));
        }

        /// <summary>
        /// Make a salt for the hash
        /// </summary>
        /// <returns>A randomly generated salt that is 16 bytes long</returns>
        private byte[] MakeSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Hash an unhashed pass phrase
        /// </summary>
        /// <param name="passPhrase">The unhashed pass phrase</param>
        /// <param name="salt">
        /// The salt to be added.
        /// When making a new user, it will be randomly generated
        /// When logging in for an existing user, it will be taken from a salt database
        /// </param>
        /// <returns>The SCrypt hashed pass phrase</returns>
        public string HashPassPhrase(string passPhrase, byte[] salt)
        {
            var password = Encoding.UTF8.GetBytes(passPhrase);
            var hashed = Convert.ToBase64String(ScryptUtil.Scrypt(
                password: password,
                salt: salt,
                // N = CPU/memory cost parameter
                N: 262144,
                // r = block size parameter (8 is common, so we do that too)
                r: 8,
                // p = parallelisation parameter, let's do 1 because it is recommended by CryptSharp
                p: 1,
                // dkLen = intended output length,
                // we do 256 / 8 = 32
                // because Microsoft has that number too
                // in their PBKDF2 tutorial using the built in ASP.NET cryptography tool
                dkLen: 256 / 8));
            return hashed;
        }
    }
}
