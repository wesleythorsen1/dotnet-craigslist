using System;
using System.Threading.Tasks;

namespace Craigslist.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var request = new CraigslistRequest("seattle", "see", "hhh");
            request.SearchText = "loft";

            var uri = request.Uri;
            var uirStr = uri.ToString();
        }
    }
}
