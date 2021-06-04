using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using ShoppingClass;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ChromeTest()
        {
            var options = new ChromeOptions();
            Test(options);
        }

        [TestMethod]
        public void FirefoxTest()
        {
            var options = new FirefoxOptions();
            Test(options);
        }

        [TestMethod]
        public void EdgeTest()
        {
            var options = new EdgeOptions();
            Test(options);
        }

        private static void Test(DriverOptions options)
        {
            var shopping = new Shopping(options);
            try
            {
                shopping.Driver.Manage().Window.Maximize();
                shopping.AddSection("Test section 1");
                shopping.Driver.Navigate().GoToUrl(shopping.Url.ToString());
                shopping.AddFirstItemFromShoppingList("Test item 1", "Test section 1");
                shopping.AddItemFromShoppingList("Test item 2", "Test section 1");
                shopping.Driver.Navigate().GoToUrl(shopping.Url + @"/options");
                shopping.AddItemFromOptions("Test item 3", "Test section 1");
                shopping.Driver.Navigate().GoToUrl(shopping.Url + @"/sections");
                shopping.AddItemFromSections("Test item 4", "Test section 1");
                shopping.RemoveItem("Test item 1");
                shopping.CrossOutItem("Test item 2");
                shopping.EditItemName("Test item 2", "2nd test item");
                shopping.AddSection("Test section 2");
                shopping.EditItemSection("2nd test item", "Test section 2");
                shopping.EditSectionName("Test section 2", "2nd test section");
                shopping.RemoveSection("Test section 1");
                shopping.RemoveAllItems();
                shopping.Driver.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                shopping.Driver.Quit();
            }
        }
    }
}