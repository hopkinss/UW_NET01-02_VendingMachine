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
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.AddACanOf, 2);
            Assert.AreEqual(vm.CanRack[SodaFlavor.Lemon],2);
        }

        [TestMethod]
        public void AddRemoveSoda()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.AddACanOf, 3);
            vm.ManageInventory(RackAction.RemoveACanOf, SodaFlavor.Lemon, 1);
            Assert.AreEqual(vm.CanRack[SodaFlavor.Lemon], 2);
            vm.ManageInventory(RackAction.AddACanOf, SodaFlavor.Lemon, 1);
            Assert.AreEqual(vm.CanRack[SodaFlavor.Lemon], 3);
        }

        [TestMethod]
        public void RemoveAndFillCanRack()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.AddACanOf, 3);

            foreach (SodaFlavor soda in SodaFlavor.GetValues(typeof(SodaFlavor)))
            {
                vm.EmptyCanRackOf(soda);
                Assert.AreEqual(vm.CanRack[soda], 0);
            }

            vm.FillTheCanRack();
            foreach (SodaFlavor soda in SodaFlavor.GetValues(typeof(SodaFlavor)))
            {
                Assert.AreEqual(vm.CanRack[soda], vm.MaxInventory);
            }

        }

        [TestMethod]
        public void AddOverMaxInventory()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.AddACanOf, 3);
            Assert.ThrowsException<Exception>(() => vm.ManageInventory(RackAction.AddACanOf,SodaFlavor.Lemon,1));
        }

        [TestMethod]
        public void NoInventoryAvailable()
        {
            var vm = new VendingMachine(50, SodaFlavor.Lemon, RackAction.AddACanOf, 1);
            Assert.ThrowsException<Exception>(() => vm.ManageInventory(RackAction.RemoveACanOf, SodaFlavor.Lemon, 2));
        }

        // tests from Exercise01  -
        [TestMethod]
        public void CostOfSodaTest()
        {
            var vm = new VendingMachine(55);

            Assert.AreEqual(vm.PurchasePrice, 55);
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
