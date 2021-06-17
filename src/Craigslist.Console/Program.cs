using System;
using System.Threading.Tasks;

namespace Craigslist.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = new CraigslistHousingRentalRequest("seattle", "see");
            request.SearchText = "loft + view";

            var uri = request.Uri;
            var uirStr = uri.ToString();
        }
    }
}
