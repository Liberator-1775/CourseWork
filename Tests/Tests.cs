using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            ChromeOptions options = new ChromeOptions();
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

        [TestMethod]
        public void FirefoxTest()
        {
            FirefoxOptions options = new FirefoxOptions();
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

        [TestMethod]
        public void EdgeTest()
        {
            EdgeOptions options = new EdgeOptions();
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