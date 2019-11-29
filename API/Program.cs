using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    /// <summary>
    /// The initially run file
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The initially run method. It builds and runs a web host
        /// </summary>
        /// <param name="args">The parameters given upon running the project</param>
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        /// <summary>
        /// Create a web host builder based on the <see cref="T:API.Startup" /> class
        /// </summary>
        /// <param name="args">The parameters given upon running the project</param>
        /// <returns>A web host builder using the <see cref="T:API.Startup" /> class</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
