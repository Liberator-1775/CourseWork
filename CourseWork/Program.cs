using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CourseWork
{
    class Program
    {
        private static void Main()
        {
            var shopping = new Shopping {Driver = new ChromeDriver("../../../../Drivers/")};
            shopping.Driver.Navigate().GoToUrl(@"http://localhost:3000");
            shopping.Driver.Manage().Window.Maximize();
            shopping.AddSection("Test section 1");
            shopping.Driver.Navigate().GoToUrl(@"http://localhost:3000");
            shopping.AddItem("Test item 1", "Test section 1");
            shopping.AddItem("Test item 2", "Test section 1");
            shopping.RemoveItem("Test item 1");
            shopping.CrossOutItem("Test item 2");
            shopping.EditItemName("Test item 2", "2nd test item");
            shopping.AddSection("Test section 2");
            shopping.EditItemSection("2nd test item", "Test section 2");
            shopping.EditSectionName("Test section 2", "2nd test section");
            shopping.RemoveSection("2nd test section");
            shopping.RemoveAllItems();
            shopping.Driver.Quit();
        }
    }

    public class Shopping
    {
        public IWebDriver Driver { get; init; }
        private int _sectionCounter = 0;
        private int _itemCounter = 0;

        public void AddItem(string name, string sectionName)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element;
            if (Driver.Url == @"http://localhost:3000/options")
            {
                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add items")));
                element.Click();
                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
                element.Click();
                element.SendKeys(name);
                ReadOnlyCollection<IWebElement> buttons =
                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                buttons[2].Click();
                ReadOnlyCollection<IWebElement> dropdownItems =
                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
                for (int i = 0; i < dropdownItems.Count; i++)
                {
                    if (dropdownItems[i].Text == sectionName)
                    {
                        dropdownItems[i].Click();
                    }
                }

                buttons[3].Click();
                _itemCounter++;
                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
                element.SendKeys(Keys.Escape);
            }
            else if (Driver.Url == @"http://localhost:3000/sections")
            {
                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add items")));
                element.Click();
                ReadOnlyCollection<IWebElement> inputFields =
                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
                inputFields[_sectionCounter + 1].Click();
                inputFields[_sectionCounter + 1].SendKeys(name);
                ReadOnlyCollection<IWebElement> buttons =
                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                buttons[_sectionCounter + 2].Click();
                ReadOnlyCollection<IWebElement> dropdownItems =
                    wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
                for (int i = 0; i < dropdownItems.Count; i++)
                {
                    if (dropdownItems[i].Text == sectionName)
                    {
                        dropdownItems[i].Click();
                    }
                }

                buttons[_sectionCounter + 3].Click();
                _itemCounter++;
                element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
                element.SendKeys(Keys.Escape);
            }
            else // Url == @"http://localhost:3000"
            {
                if (_itemCounter > 0)
                {
                    ReadOnlyCollection<IWebElement> buttons =
                        wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                    buttons[_itemCounter + 1].Click();
                    element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
                    element.Click();
                    element.SendKeys(name);
                    buttons = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                    buttons[_itemCounter + 2].Click();
                    ReadOnlyCollection<IWebElement> dropdownItems =
                        wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
                    for (int i = 0; i < dropdownItems.Count; i++)
                    {
                        if (dropdownItems[i].Text == sectionName)
                        {
                            dropdownItems[i].Click();
                        }
                    }

                    buttons[_itemCounter + 3].Click();
                    _itemCounter++;
                    element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
                    element.SendKeys(Keys.Escape);
                }
                else
                {
                    element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
                    element.Click();
                    element.SendKeys(name);
                    ReadOnlyCollection<IWebElement> buttons =
                        wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                    buttons[1].Click();
                    ReadOnlyCollection<IWebElement> dropdownItems =
                        wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
                    for (int i = 0; i < dropdownItems.Count; i++)
                    {
                        if (dropdownItems[i].Text == sectionName)
                        {
                            dropdownItems[i].Click();
                        }
                    }

                    buttons[2].Click();
                    _itemCounter++;
                }
            }
        }

        public void AddSection(string name)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            if (_sectionCounter == 0)
            {
                wait.Until(e => e.FindElement(By.ClassName("input")).GetAttribute("placeholder") == "New section name");
            }
            ReadOnlyCollection<IWebElement> inputFields =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            inputFields[_sectionCounter].Click();
            inputFields[_sectionCounter].SendKeys(name);
            ReadOnlyCollection<IWebElement> buttons =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[_sectionCounter + 1].Click();
            _sectionCounter++;
        }

        public void RemoveAllItems()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Options")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[1].Click();
            buttons = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[3].Click();
            _itemCounter = 0;
        }

        public void RemoveItem(string name)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            ReadOnlyCollection<IWebElement> items =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            buttons = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[items.Count + 3].Click();
            wait.Until(ExpectedConditions.StalenessOf(buttons[items.Count + 3]));
            buttons = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[items.Count + 3].Click();
            _itemCounter--;
        }

        public void CrossOutItem(string name)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> checks =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("check")));
            ReadOnlyCollection<IWebElement> items =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    checks[i].Click();
                }
            }
        }

        public void EditItemName(string name, string finalName)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            ReadOnlyCollection<IWebElement> items =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            element = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
            element.Click();
            Actions actionProvider = new Actions(Driver);
            IAction keydown = actionProvider.KeyDown(Keys.Control).SendKeys("a").Build();
            keydown.Perform();
            element.SendKeys(Keys.Backspace);
            element.SendKeys(finalName);
            element.SendKeys(Keys.Escape);
        }

        public void EditItemSection(string name, string sectionName)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            ReadOnlyCollection<IWebElement> items =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            buttons = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[items.Count + 2].Click();
            ReadOnlyCollection<IWebElement> dropdownItems =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
            for (int i = 0; i < dropdownItems.Count; i++)
            {
                if (dropdownItems[i].Text == sectionName)
                {
                    dropdownItems[i].Click();
                }
            }

            buttons[items.Count + 2].SendKeys(Keys.Escape);
        }

        public void EditSectionName(string name, string finalName)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            ReadOnlyCollection<IWebElement> inputFields =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            for (int i = 0; i < inputFields.Count; i++)
            {
                if (inputFields[i].GetAttribute("value") == name)
                {
                    inputFields[i].Click();
                    Actions actionProvider = new Actions(Driver);
                    IAction keydown = actionProvider.KeyDown(Keys.Control).SendKeys("a").Build();
                    keydown.Perform();
                    inputFields[i].SendKeys(Keys.Backspace);
                    inputFields[i].SendKeys(finalName);
                }
            }
        }

        public void RemoveSection(string name)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            ReadOnlyCollection<IWebElement> inputFields =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            ReadOnlyCollection<IWebElement> buttons =
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            for (int i = 0; i < inputFields.Count; i++)
            {
                if (inputFields[i].GetAttribute("value") == name)
                {
                    buttons[i + 1].Click();
                }
            }
        }
    }
}