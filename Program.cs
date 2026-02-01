using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Dynamic;
using System.Reflection;


class Program
{
    private static HttpClient client = new HttpClient();
    private static Pokedex? pokedex;
    
    public static async Task Main()
    {
        Console.WriteLine($"{Environment.NewLine}WELCOME TO THE POKEDEX CONSOLE APP");
        await LoadPokedex(); 
        Console.WriteLine
        (@"OPTIONS MENU:
        1. LIST ALL POKEMON
        2. VIEW POKEMON DETAILS (WIP)
        3. EXIT PROGRAM");
        Console.Write("CHOOSE AN OPTION: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                ListAllPokemon();
                break;
            case "2":
                await DisplayPokemonDetails();
                break;
            case "3":
                break;
        }
        Console.WriteLine("GOODBYE");
        Console.Read();
        
    }

    public static async Task LoadPokedex()
    {
        Console.WriteLine("Loading Pokedex...");
        string URL = "https://pokeapi.co/api/v2/pokemon?offset=0&limit=151";
        try
        {
            string pokemonJson = await client.GetStringAsync(URL);
            pokedex = JsonSerializer.Deserialize<Pokedex>(pokemonJson);
            Console.WriteLine("Pokedex loaded");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to load Pokedex");
            Console.WriteLine(e.Message);
        }
    }

    public static void ListAllPokemon()
    {
        if (pokedex != null && pokedex.PokemonList != null)
        {
            Console.WriteLine($"{Environment.NewLine}POKEMON LIST:");
            int i = 0;
            foreach(var pokemon in pokedex.PokemonList)
            {
                Console.WriteLine($"#{++i:000}: {pokemon.Name.ToUpper()}");
            }
        }
    }

    public static async Task DisplayPokemonDetails()
    {
        Console.Write("Enter Pokemon Name:");
        string? name = Console.ReadLine()?.ToLower();

        var pokemon = pokedex?.PokemonList?.FirstOrDefault(p => p.Name == name); 
        
        
        
        
        
        
        
        
        
        
        )
        
        if (string.IsNullOrEmpty(pokemon.Url)) return;

        string json = await client.GetStringAsync(pokemon.Url);
        var details = JsonSerializer.Deserialize<PokemonDetails>(json);

        if (details == null) return;

        Console.WriteLine($"{Environment.NewLine}Details for {pokemon.Name}"); 
        Console.WriteLine($"ID: {details.Id}");
        Console.WriteLine($"Height: {details.Height}");
        Console.WriteLine($"Weight: {details.Weight}");
        
        if (details.Types != null)
        {
            foreach(var type in details.Types)
            {
                Console.WriteLine($"- {type.Type?.Name}");
            }
        }
    }
}

class Pokedex
{
    [JsonPropertyName("results")]
    public List<Pokemon>? PokemonList { get; set; }
}

class Pokemon
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

class PokemonDetails
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("types")]
    public List<TypeSlot>? Types { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    public class TypeSlot
    {
        [JsonPropertyName("slot")]
        public int Slot { get; set; }
        [JsonPropertyName("type")]
        public PokemonType? Type { get; set; }
    }
    public class PokemonType
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}