using System;
using System.Diagnostics;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Shopping
{
    public class Shopping
    {
        public readonly IWebDriver Driver;
        private int _sectionCounter;
        private int _itemCounter;
        public StringBuilder Url { get; }
        private readonly WebDriverWait _wait;

        public Shopping(DriverOptions options)
        {
            Driver = new RemoteWebDriver(new Uri(@"http://localhost:4444"), options);
            var getAppUrl = new Process
            {
                StartInfo =
                {
                    FileName = "docker",
                    Arguments = "exec shopping_app_1 hostname -i",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            getAppUrl.Start();
            Url = new StringBuilder(@"http://" + getAppUrl.StandardOutput.ReadLine() + @":3000");
            Driver.Navigate().GoToUrl(Url.ToString());
            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
        }

        public void AddItemFromOptions(string name, string sectionName)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add items")));
            element.Click();
            element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
            element.Click();
            element.SendKeys(name);
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[2].Click();
            var dropdownItems =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
            foreach (var section in dropdownItems)
            {
                if (section.Text == sectionName)
                {
                    section.Click();
                }
            }

            buttons[3].Click();
            _itemCounter++;
            element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
            element.SendKeys(Keys.Escape);
        }

        public void AddItemFromSections(string name, string sectionName)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add items")));
            element.Click();
            var inputFields =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            inputFields[_sectionCounter + 1].Click();
            inputFields[_sectionCounter + 1].SendKeys(name);
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[_sectionCounter + 2].Click();
            var dropdownItems =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
            foreach (var section in dropdownItems)
            {
                if (section.Text == sectionName)
                {
                    section.Click();
                }
            }

            buttons[_sectionCounter + 3].Click();
            _itemCounter++;
            element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
            element.SendKeys(Keys.Escape);
        }

        public void AddItemFromShoppingList(string name, string sectionName)
        {
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[_itemCounter + 1].Click();
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
            element.Click();
            element.SendKeys(name);
            buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[_itemCounter + 2].Click();
            var dropdownItems =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
            foreach (var section in dropdownItems)
            {
                if (section.Text == sectionName)
                {
                    section.Click();
                }
            }

            buttons[_itemCounter + 3].Click();
            _itemCounter++;
            element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
            element.SendKeys(Keys.Escape);
        }

        public void AddFirstItemFromShoppingList(string name, string sectionName)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
            element.Click();
            element.SendKeys(name);
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[1].Click();
            var dropdownItems =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
            foreach (var section in dropdownItems)
            {
                if (section.Text == sectionName)
                {
                    section.Click();
                }
            }

            buttons[2].Click();
            _itemCounter++;
        }

        public void AddSection(string name)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            if (_sectionCounter == 0)
            {
                _wait.Until(e =>
                    e.FindElement(By.ClassName("input")).GetAttribute("placeholder") == "New section name");
            }

            var inputFields =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            inputFields[_sectionCounter].Click();
            inputFields[_sectionCounter].SendKeys(name);
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[_sectionCounter + 1].Click();
            _sectionCounter++;
        }

        public void RemoveAllItems()
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Options")));
            element.Click();
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[1].Click();
            buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[3].Click();
            _itemCounter = 0;
        }

        public void RemoveItem(string name)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            var items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[items.Count + 3].Click();
            _wait.Until(ExpectedConditions.StalenessOf(buttons[items.Count + 3]));
            buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[items.Count + 3].Click();
            _itemCounter--;
        }

        public void CrossOutItem(string name)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            var checks =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("check")));
            var items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    checks[i].Click();
                }
            }
        }

        public void EditItemName(string name, string finalName)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            var items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
            element.Click();
            var actionProvider = new Actions(Driver);
            var keydown = actionProvider.KeyDown(Keys.Control).SendKeys("a").Build();
            keydown.Perform();
            element.SendKeys(Keys.Backspace);
            element.SendKeys(finalName);
            element.SendKeys(Keys.Escape);
        }

        public void EditItemSection(string name, string sectionName)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            var items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[items.Count + 2].Click();
            var dropdownItems =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
            foreach (var section in dropdownItems)
            {
                if (section.Text == sectionName)
                {
                    section.Click();
                }
            }

            buttons[items.Count + 2].SendKeys(Keys.Escape);
        }

        public void EditSectionName(string name, string finalName)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            var inputFields =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            foreach (var section in inputFields)
            {
                if (section.GetAttribute("value") != name) continue;
                section.Click();
                var actionProvider = new Actions(Driver);
                var keydown = actionProvider.KeyDown(Keys.Control).SendKeys("a").Build();
                keydown.Perform();
                section.SendKeys(Keys.Backspace);
                section.SendKeys(finalName);
            }
        }

        public void RemoveSection(string name)
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            var inputFields =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            var buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            for (var i = 0; i < inputFields.Count; i++)
            {
                if (inputFields[i].GetAttribute("value") == name)
                {
                    buttons[i + 1].Click();
                }
            }

            _sectionCounter--;
        }
    }
}
