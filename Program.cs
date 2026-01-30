using System.Text.Json;
using System.Net.Http;

// Json "results" is a list

class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Hello");
        Console.WriteLine("Hello");
        await FetchPokemonData();
        Console.WriteLine("Hello");

    }

    public static async Task FetchPokemonData()
    {
        string URL = "https://pokeapi.co/api/v2/pokemon?offset=0&limit=151";

        using HttpClient client = new HttpClient();

        string jsonResponse = await client.GetStringAsync(URL);

        Console.WriteLine(jsonResponse);
    }
}




