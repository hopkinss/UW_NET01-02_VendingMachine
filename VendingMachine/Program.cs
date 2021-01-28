using System;
using Microsoft.Extensions.Configuration;

namespace VendingMachine
{
    class Program
    {
        // Hopkins, Shawn, hopkinss
        // Exercise 02 - Vending Machine
        // shawn.hopkins1@gmail.com
        static void Main(string[] args)
        {
            var config = Settings().Build();

            // Get the default values for project from configuration file
            var vendingMachine = new VendingMachine(int.Parse(config["Price"]),
                                                    numberOfCans:int.Parse(config["MaxNumberOfCans"]));

            // show the contents of the vending machine in debug window
            vendingMachine.ShowVendingMachineStatus();

            Console.WriteLine("Welcome to the .NET Soda vending machine.\n");
            bool isVending = true;

            while (isVending)
            {
                // Select a soda flavor
                SodaFlavor soda = default;
                while (soda == default)
                {                    
                    Console.WriteLine("Enter a flavor of soda from the menu:");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    foreach (var v in vendingMachine.AvailableSodaFlavors())
                    {
                        Console.WriteLine($"\t{v}");
                    }
                    Console.ForegroundColor = ConsoleColor.White;

                    soda = Utility.ParseEnum<SodaFlavor>(Console.ReadLine());
                    if (soda == default)
                        Console.WriteLine($"You must enter a flavor available in the menu!\n\n");
                }

                // pay for the soda
                while (!vendingMachine.IsAmountSufficient())
                {
                    Console.Write($"Please insert {vendingMachine.Price} cents (enter 'q' to quit): ");
                    try
                    {
                        vendingMachine.GetEnteredAmount(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(ApplicationException))
                            Environment.Exit(0);
                        else
                            Console.WriteLine($"Sorry, {ex.Message}\n");
                    }
                    Console.Write($"You have inserted {vendingMachine.AmountSpent} cents\n");

                    if (!vendingMachine.IsAmountSufficient())
                        Console.Write($"Please deposit {vendingMachine.Balance()} cents more to purchase the soda: ");
                }

                // update the inventory
                try
                {
                    vendingMachine.ManageInventory(RackAction.Remove, soda, 1);
                    var msg = vendingMachine.Balance() < 0 ? $"and {Math.Abs(vendingMachine.Balance())} cents change" : string.Empty;
                    Console.WriteLine($"Thanks. Here is your soda {msg}\n\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Sorry, {ex.Message}\n");
                    if (ex.Message.Contains("empty"))
                    {
                        Console.Write($"Would you like to refill the {soda} soda in the vending machine? (y/n): ");
                        var resp = Console.ReadKey();

                        // Restock the selected soda
                        if (resp.Key == ConsoleKey.Y)
                        {
                            vendingMachine.ManageInventory(RackAction.Add, soda, 3);
                            Console.WriteLine($"\nRestocked the {soda} soda...\n\n");
                        }
                        else
                        {
                            Console.Clear();
                        }
                    }
                }

                // display contents of vending machine to debug
                vendingMachine.ShowVendingMachineStatus();
            }
        }

        public static IConfigurationBuilder Settings()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            return configBuilder;
        }
    }
}
