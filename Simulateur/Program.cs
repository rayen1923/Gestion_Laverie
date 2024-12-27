using GestionLaverie.Entites;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using Machine = GestionLaverie.Entites.Machine;

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
                Console.WriteLine("1. Retrieve all proprietors .");
                Console.WriteLine("2. Display all proprietors .");
                Console.WriteLine("3. Toggle machine state.");
                Console.WriteLine("0. Exit.");
                Console.Write("Choose number: ");

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
                        Console.WriteLine("Invalid choice.");
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
                    Console.WriteLine("Proprietors have been successfully retrieved.");
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
                Console.WriteLine("No proprietors available.");
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
            Console.WriteLine("Available Proprietors:");
            foreach (var _proprietor in proprietorsList)
            {
                Console.WriteLine($"    {_proprietor.Id}/ {_proprietor.Name}");
            }

            Propriétaire proprietor = null;

            while (proprietor == null)
            {
                Console.Write("Enter the ID of the proprietor: ");
                if (int.TryParse(Console.ReadLine(), out int proprietorId))
                {
                    proprietor = FindProprietorByid(proprietorId);
                    if (proprietor == null)
                    {
                        Console.WriteLine("Proprietor not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer ID.");
                }
            }

            Console.WriteLine($"Laundries: ");
            foreach (var laverie in proprietor.Laveries)
            {
                Console.WriteLine($"    {laverie.Id}/ Name: {laverie.Name}");
            }

            Laverie laundry = null;

            while (laundry == null)
            {
                Console.Write("Enter the ID of the Laundry: ");
                if (int.TryParse(Console.ReadLine(), out int laundryId))
                {
                    laundry = FindLaundryByid(laundryId, proprietor);
                    if (laundry == null)
                    {
                        Console.WriteLine("Laundry not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer ID.");
                }
            }

            Console.WriteLine($"Machines: ");
            foreach (var machine in laundry.Machines)
            {
                Console.WriteLine($"    {machine.Id}/ Marque: {machine.Marque}");
            }

            Machine machineE = null;

            while (machineE == null)
            {
                Console.Write("Enter the ID of the Machine: ");
                if (int.TryParse(Console.ReadLine(), out int machineId))
                {
                    machineE = FindMachineByid(machineId, laundry);
                    if (machineE == null)
                    {
                        Console.WriteLine("Machine not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer ID.");
                }
            }

            Console.WriteLine($"Cycles: ");
            foreach (var cycle in machineE.Cycles)
            {
                Console.WriteLine($"    {cycle.Id}/ Cout: {cycle.Cout} / Duration: {cycle.Duration}");
            }

            Cycle cycleE = null;

            while (cycleE == null)
            {
                Console.Write("Enter the ID of the Cycle: ");
                if (int.TryParse(Console.ReadLine(), out int cycleId))
                {
                    cycleE = FindCycleByid(cycleId, machineE);
                    if (cycleE == null)
                    {
                        Console.WriteLine("Cycle not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer ID.");
                }
            }
            Console.WriteLine("Starting the cycle...");
            await MonitorCycleCompletion(machineE, cycleE);
        }

        private static async Task MonitorCycleCompletion(Machine machine, Cycle cycle)
        {
            using HttpClient client = new HttpClient();
            string apiUrl = $"{ApiConfig.BaseUrl}/Machine/toggleMachineEtat/{machine.Id}/{cycle.Id}";

            try
            {
                Console.WriteLine($"Starting cycle {cycle.Id} for Machine {machine.Id}.");
                HttpResponseMessage initialResponse = await client.PutAsync(apiUrl, null);

                Console.WriteLine($"Cycle {cycle.Id} will complete in {cycle.Duration.TotalSeconds} seconds...");
                await Task.Delay(cycle.Duration);

                Console.WriteLine($"Cycle {cycle.Id} for Machine {machine.Id} completed.");
                HttpResponseMessage finalResponse = await client.PutAsync(apiUrl, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating state: {ex.Message}");
            }
        }

        static Cycle FindCycleByid(int cycleId, Machine machineE)
        {
            foreach (var cycle in machineE.Cycles)
            {
                if (cycle.Id == cycleId)
                {
                    return cycle;
                }
            }
            return null;
        }

        static Machine FindMachineByid(int machineId, Laverie laundry)
        {
            foreach (var machine in laundry.Machines)
            {
                if (machine.Id == machineId)
                {
                    return machine;
                }
            }
            return null;
        }

        static Laverie FindLaundryByid(int laundryId, Propriétaire proprietor)
        {
            foreach (var laverie in proprietor.Laveries)
            {
                if (laverie.Id == laundryId)
                {
                    return laverie;
                }
            }
            return null;
        }

        static Propriétaire FindProprietorByid(int proprietorId)
        {
            foreach (var proprietor in proprietorsList)
            {
                if (proprietor.Id == proprietorId)
                {
                    return proprietor;
                }
            }
            return null;
        }
    }
}
