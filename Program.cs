using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Dynamic;
using System.Reflection;

/* TODO
-ADD DATETIME USAGE TO TRACK HOW LONG IT TAKES TO BUILD THE POKEDEX;
-ADD DATETIME TO STATE AT WHAT TIME POKEMON DETAILS ARE RETRIEVED;
-INCORPORATE ENUM
*/

class Program
{
    private static Pokedex? pokedex;
    
    public static async Task Main()
    {
        await LoadPokedex(); 
        Console.WriteLine($"{Environment.NewLine}WELCOME TO THE POKEDEX CONSOLE APP");
        
        while (true)
        {
            Console.WriteLine($"{Environment.NewLine}OPTIONS MENU:{Environment.NewLine}1. LIST ALL POKEMON{Environment.NewLine}2. VIEW POKEMON DETAILS{Environment.NewLine}3. EXIT PROGRAM");
            Console.Write("CHOOSE AN OPTION: ");
            
            try
            {
                string? choiceId = Console.ReadLine();
                bool exists;
                UserChoices choice = (UserChoices)int.Parse(choiceId);
                exists = Enum.IsDefined(typeof(UserChoices), choice);
                if (exists == false)
                {
                    Console.WriteLine($"{Environment.NewLine}{choice} is not a valid option.");
                    continue;
                }
                switch (choice)
                {
                    case UserChoices.ListAllPokemon:
                        ListAllPokemon();
                        break;
                    case UserChoices.ViewPokemonDetails:
                        await DisplayPokemonDetails();
                        break;
                    case UserChoices.ExitProgram:
                        Console.WriteLine($"{Environment.NewLine}Quitting program...");
                        break;
                }
                if (choice == UserChoices.ExitProgram) break;
            }
            catch (FormatException)
            {
                Console.WriteLine($"{Environment.NewLine}Please input a valid number");
                continue;
            }
            
        }
    }
    
    public enum UserChoices
    {
        ListAllPokemon = 1,
        ViewPokemonDetails = 2,
        ExitProgram = 3
    }

    public static async Task LoadPokedex()
    {
        Console.WriteLine($"{Environment.NewLine}Loading Pokedex...");
        TimeOnly timeStart = TimeOnly.FromDateTime(DateTime.Now);

        string URL = "https://pokeapi.co/api/v2/pokemon?&limit=1025";
        try
        {
            using HttpClient client = new HttpClient();
            string pokemonJson = await client.GetStringAsync(URL);
            pokedex = JsonSerializer.Deserialize<Pokedex>(pokemonJson);
            
            TimeSpan timeElapsed = TimeOnly.FromDateTime(DateTime.Now) - timeStart;
            Console.WriteLine($"Pokedex loaded in {timeElapsed.Milliseconds} ms");
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
                Console.WriteLine($"#{++i:0000}: {pokemon?.Name?.ToUpper()}");
            }
        }
    }

    public static async Task DisplayPokemonDetails()
    {
        Console.Write($"Enter Pokemon Name: ");
        string? name = Console.ReadLine()?.ToLower();

        Console.WriteLine($"Loading details...");

        // Found this LINQ query when googling how to search a list
        Pokemon? pokemon = pokedex?.PokemonList?.FirstOrDefault(p => p.Name == name);

        try
        {
            using HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(pokemon?.Url);
            PokemonDetails? details = new();
            details = JsonSerializer.Deserialize<PokemonDetails?>(json);

            if (details == null) return;

            Console.WriteLine($"{Environment.NewLine}Name: {pokemon?.Name?.ToUpper()}"); 
            Console.WriteLine($"ID: #{details.Id}");
            Console.WriteLine($"Height: {details.Height}");
            Console.WriteLine($"Weight: {details.Weight}");
            if (details.Types != null)
            {
                List<string> types = new List<string> { };
                foreach(var type in details.Types)
                {
                    string? typeName = type?.Type?.Name?.ToUpper();
                    if (typeName != null)
                    {
                        types.Add(typeName);
                    }
                }
                if (types.Count < 2)
                {
                    Console.WriteLine($"Type: {types[0]}");
                }
                else
                {
                    Console.WriteLine($"Types: {string.Join(", ", types)}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Pokemon details could not be loaded.");
            Console.WriteLine(e.Message);
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