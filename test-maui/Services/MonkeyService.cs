using System.Net.Http.Json;
using System.Threading.Tasks;
using test_maui.Model;

namespace test_maui.Services
{
    public class MonkeyService
    {
        HttpClient httpClient;

        public MonkeyService()
        {
            httpClient = new();
        }

        List<Monkey> monkeys = new();
        public async Task<List<Monkey>> GetMonkeys()
        {
            if (monkeys?.Count >0)            
                return monkeys;
            var url = "https://montemagno.com/monkeys.json";

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                monkeys = await response.Content.ReadFromJsonAsync<List<Monkey>>();
            }
            return monkeys;
        }
    }
}
