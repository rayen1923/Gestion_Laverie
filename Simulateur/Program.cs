using GestionLaverie.Entites;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Simulateur
{
    public class Program
    {
        private static List<Propriétaire> proprietorsList = new List<Propriétaire>();

        public static async Task Main(string[] args)
        {
            bool continueRunning = true;

            while (continueRunning)
            {
                Console.WriteLine("Available tasks:");
                Console.WriteLine("1. Retrieve all proprietors with their details.");
                Console.WriteLine("2. Display all proprietors with their laundries, machines, and cycles.");
                Console.WriteLine("3. Toggle machine state.");
                Console.WriteLine("0. Exit.");
                Console.Write("Please choose an option (by number): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await FetchProprietorsWithDetails();
                        break;
                    case "2":
                        DisplayProprietorsWithDetails();
                        break;
                    case "3":
                        await ToggleMachineState();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static async Task FetchProprietorsWithDetails()
        {
            try
            {
                using HttpClient client = new HttpClient();

                string apiUrl = $"{ApiConfig.BaseUrl}/Configuration/proprietaires";

                Console.WriteLine("Fetching data from the API...");
                List<Propriétaire> proprietors = await client.GetFromJsonAsync<List<Propriétaire>>(apiUrl);

                if (proprietors == null || proprietors.Count == 0)
                {
                    Console.WriteLine("No proprietors found.");
                }
                else
                {
                    proprietorsList = proprietors;
                    Console.WriteLine("Proprietors have been successfully retrieved and stored.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during the API call: {ex.Message}");
            }
        }

        static void DisplayProprietorsWithDetails()
        {
            if (proprietorsList == null || proprietorsList.Count == 0)
            {
                Console.WriteLine("No proprietors available. Please fetch data first.");
                return;
            }

            Console.WriteLine("List of proprietors with their details:");
            foreach (var proprietor in proprietorsList)
            {
                Console.WriteLine($"Proprietor: {proprietor.Name} (ID: {proprietor.Id})");
                foreach (var laverie in proprietor.Laveries)
                {
                    Console.WriteLine($"  Laundry: {laverie.Name} (Address: {laverie.Adress})");
                    foreach (var machine in laverie.Machines)
                    {
                        Console.WriteLine($"    Machine: {machine.Id} {machine.Marque} {machine.Modele} (State: {machine.Etat})");
                        foreach (var cycle in machine.Cycles)
                        {
                            Console.WriteLine($"      Cycle: {cycle.Cout} for {cycle.Duration.TotalMinutes} minutes");
                        }
                    }
                }
            }
        }

        static async Task ToggleMachineState()
        {
            Console.Write("Enter the ID of the machine to toggle state: ");
            int machineId = int.Parse(Console.ReadLine());

            var machine = FindMachineById(machineId);

            if (machine != null)
            {
                machine.Etat = (machine.Etat == 1) ? 0 : 1;

                Console.WriteLine($"Machine ID {machineId} state toggled to: {machine.Etat}");
            }
            else
            {
                Console.WriteLine("Machine not found.");
            }
        }

        static Machine FindMachineById(int machineId)
        {
            foreach (var proprietor in proprietorsList)
            {
                foreach (var laverie in proprietor.Laveries)
                {
                    foreach (var machine in laverie.Machines)
                    {
                        if (machine.Id == machineId)
                        {
                            return machine;
                        }
                    }
                }
            }
            return null;
        }
    }
}
