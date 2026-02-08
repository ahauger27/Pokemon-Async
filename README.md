This is a console app that will asynchroonously access the PokeAPI and give the user a few options.

Users can: 
1. List the names of all pokemon
2. Display details about a specific Pokemon, searching by name
3. Exit program

HOW TO RUN 
With a stable internet connection, run Program.cs in the terminal using `dotnet run`. When not connected to the internet, the program currently does not run as intended. 

UPDATE 2/7/26
- The program now accesses all unique Pokemon from 1 - 1025 (no variants)
- The user choices now accounted for inside an enum UserChoices, and are now validated
- The total time it takes to load the Pokedex is now monitored and printed to the console

TODO
- When the Pokedex fails to load. As of now, program will continue to run and display the OPTIONS MENU, but will not be able to 