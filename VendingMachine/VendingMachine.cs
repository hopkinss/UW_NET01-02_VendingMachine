using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VendingMachine
{
    public enum SodaFlavor { Regular = 1, Orange=2, Lemon=3 }
    public enum RackAction { Add = 1, Remove=2 }

    public class VendingMachine
    {
        private int price;                             // Cost of soda
        private int amountSpent;                       // Current amount the user has inserted
        private int maxInventory;                      // Max number of cans of soda per flavor
        private Dictionary<SodaFlavor, int> inventory; // Inventory of soda availble in the vending machine
        

        // Creates an instance of a full vending machine
        public VendingMachine(int initalPrice            
                            , SodaFlavor flavor = default
                            , RackAction action = RackAction.Add
                            , int numberOfCans = 3)
        {
            price = initalPrice;
            maxInventory = numberOfCans;
            inventory = new Dictionary<SodaFlavor, int>();
            ManageInventory(action,flavor,numberOfCans);
        }

        public int Price
        {
            get => price;
            set => price = value;
        }

        public int MaxInventory
        {
            get { return maxInventory; }
            set { maxInventory = value; }
        }


        public int AmountSpent
        {
            get => amountSpent;
            set
            {
                amountSpent += value;
            }
        }

        public Dictionary<SodaFlavor, int> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }

        public void ManageInventory(RackAction action,SodaFlavor flavor,int numberOfCans)
        {
            var flavors = new List<SodaFlavor>();
            flavors = flavor == default ? new List<SodaFlavor>() { SodaFlavor.Lemon, SodaFlavor.Orange, SodaFlavor.Regular } :
                                new List<SodaFlavor> { flavor };

            foreach(var f in flavors)
            {
                for (int i = 1; i <= numberOfCans; i++)
                {
                    if (inventory.ContainsKey(f))
                    {
                        if (action == RackAction.Add)
                        {
                            if (!IsFull(f))
                                inventory[f] += 1;
                            else
                                throw new Exception($"{f} rack is full");
                        }
                        else
                        {
                            if (!IsEmpty(f))
                                inventory[f] -= 1;
                            else
                                throw new Exception($"{f} is empty");
                        }
                    }
                    else
                    {
                        if (action == RackAction.Add)
                        {
                            inventory.Add(f, i);
                        }
                        else
                        {
                            throw new Exception($"There is no inventory of {f} soda");
                        }                        
                    }
                }
            }           
        }

        public void GetEnteredAmount(string response)
        {

            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var isInt = int.TryParse(response, out int amountSpent);

            if (!isInt)
            {
                if (response.ToLower() == "q")
                    throw new ApplicationException();
                else
                    throw new ArgumentException($"Argument '{response}' is not an integer\n");
            }

            if (isInt)
                this.amountSpent += amountSpent;

        }
        public bool IsAmountSufficient()
        {
            return this.amountSpent >= this.price;
        }

        public int Balance()
        {
            return this.price - this.amountSpent;
        }

        private bool IsFull(SodaFlavor flavor)
        {
            if (this.inventory.TryGetValue(flavor, out int stock))
            {
                return stock >= this.maxInventory ? true : false;
            }
            else
                return false;
        }

        private bool IsEmpty(SodaFlavor flavor)
        {
            if (this.inventory.TryGetValue(flavor, out int stock))
            {
                return stock > 0 ? false : true;
            }
            else
                return true;
        }

        // Available soda flavors
        public IEnumerable<string> AvailableSodaFlavors()
        {
            return this.inventory.Select(x => x.Key.ToString());
        }

        // Write the state of vending machcine to debug location
        public void ShowVendingMachineStatus()
        {
            foreach(var item in this.inventory)
            {
                Debug.WriteLine($"There are {item.Value} cans of {item.Key} soda available at a cost of {this.price} cents");
            }
        }

        public static IConfigurationBuilder Settings()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            return configBuilder;
        }
    }
}
