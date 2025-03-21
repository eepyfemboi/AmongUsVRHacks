using System.Net.Http;
using System.Threading.Tasks;
using AmongUsHacks.Main;

namespace AmongUsHacks.Utils
{
    public static class Networking
    {
        private static readonly HttpClient client = new();

        public static async Task<string> GetStringAsync(string url)
        {
            return await client.GetStringAsync(url);
        }
    }
}
