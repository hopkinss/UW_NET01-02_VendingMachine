using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace VendingMachine.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddSodaConstructor()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.Add, 2);
            Assert.AreEqual(vm.Inventory[SodaFlavor.Lemon],2);
        }

        [TestMethod]
        public void AddRemoveSoda()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.Add, 3);
            vm.ManageInventory(RackAction.Remove, SodaFlavor.Lemon, 1);
            Assert.AreEqual(vm.Inventory[SodaFlavor.Lemon], 2);
            vm.ManageInventory(RackAction.Add, SodaFlavor.Lemon, 1);
            Assert.AreEqual(vm.Inventory[SodaFlavor.Lemon], 3);
        }

        [TestMethod]
        public void AddOverMaxInventory()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.Add, 3);
            Assert.ThrowsException<Exception>(() => vm.ManageInventory(RackAction.Add,SodaFlavor.Lemon,1));
        }

        [TestMethod]
        public void NoInventoryAvailable()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.Add, 1);
            Assert.ThrowsException<Exception>(() => vm.ManageInventory(RackAction.Remove, SodaFlavor.Lemon, 2));
        }

        // tests from Exercise01  -
        [TestMethod]
        public void CostOfSodaTest()
        {
            var vm = new VendingMachine(55);

            Assert.AreEqual(vm.Price, 55);
        }

        [TestMethod]
        public void AmountSpentTest()
        {
            var vm = new VendingMachine(5);
            int spent = 0;
            for (int i = 1; i <= 5; i++)
            {
                vm.GetEnteredAmount(i.ToString());
                spent += i;
                Assert.AreEqual(spent, vm.AmountSpent);
            }
        }

        [TestMethod]
        public void GetEnteredAmountNullTest()
        {
            var vm = new VendingMachine(44);
            Assert.ThrowsException<ArgumentNullException>(() => vm.GetEnteredAmount(null));
        }

        [TestMethod]
        public void GetEnteredAmountInvalidTest()
        {
            var vm = new VendingMachine(44);
            Assert.ThrowsException<ArgumentException>(() => vm.GetEnteredAmount("four"));
        }

        [TestMethod]
        public void GetEnteredAmountTest()
        {
            var vm = new VendingMachine(44);
            vm.GetEnteredAmount("10");
            Assert.AreEqual(vm.AmountSpent, 10);
            vm.GetEnteredAmount("10");
            Assert.AreEqual(vm.AmountSpent, 20);
        }

        [TestMethod]
        public void IsAmountSufficientTest()
        {
            var vm = new VendingMachine(50);
            vm.GetEnteredAmount("30");

            Assert.IsFalse(vm.IsAmountSufficient());

            vm.GetEnteredAmount("20");
            Assert.IsTrue(vm.IsAmountSufficient());
        }
    }
}
