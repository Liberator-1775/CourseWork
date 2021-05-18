using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CourseWork
{
    class Program
    {
        private static void Main()
        {
            var shopping = new Shopping();
            shopping.Driver.Manage().Window.Maximize();
            shopping.AddSection("Test section 1");
            shopping.Driver.Navigate().GoToUrl(shopping.Url.ToString());
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
        public readonly IWebDriver Driver;
        private int _sectionCounter;
        private int _itemCounter;
        public StringBuilder Url { get; }
        private WebDriverWait _wait;

        public Shopping()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            Driver = new RemoteWebDriver(new Uri(@"http://localhost:4444"), chromeOptions);
            Process getAppUrl = new Process();
            getAppUrl.StartInfo.FileName = "docker";
            getAppUrl.StartInfo.Arguments = "exec shopping_app_1 hostname -i";
            getAppUrl.StartInfo.UseShellExecute = false;
            getAppUrl.StartInfo.RedirectStandardOutput = true;
            getAppUrl.Start();
            Url = new StringBuilder(@"http://" + getAppUrl.StandardOutput.ReadToEnd() + @":3000");
            Driver.Navigate().GoToUrl(Url.ToString());
            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
        }

        public void AddItem(string name, string sectionName)
        {
            IWebElement element;
            if (Driver.Url == Url + @"/options")
            {
                element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add items")));
                element.Click();
                element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
                element.Click();
                element.SendKeys(name);
                ReadOnlyCollection<IWebElement> buttons =
                    _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                buttons[2].Click();
                ReadOnlyCollection<IWebElement> dropdownItems =
                    _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
                for (int i = 0; i < dropdownItems.Count; i++)
                {
                    if (dropdownItems[i].Text == sectionName)
                    {
                        dropdownItems[i].Click();
                    }
                }

                buttons[3].Click();
                _itemCounter++;
                element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
                element.SendKeys(Keys.Escape);
            }
            else if (Driver.Url == Url + @"/sections")
            {
                element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Add items")));
                element.Click();
                ReadOnlyCollection<IWebElement> inputFields =
                    _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
                inputFields[_sectionCounter + 1].Click();
                inputFields[_sectionCounter + 1].SendKeys(name);
                ReadOnlyCollection<IWebElement> buttons =
                    _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                buttons[_sectionCounter + 2].Click();
                ReadOnlyCollection<IWebElement> dropdownItems =
                    _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
                for (int i = 0; i < dropdownItems.Count; i++)
                {
                    if (dropdownItems[i].Text == sectionName)
                    {
                        dropdownItems[i].Click();
                    }
                }

                buttons[_sectionCounter + 3].Click();
                _itemCounter++;
                element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
                element.SendKeys(Keys.Escape);
            }
            else // Url == @"http://172.20.0.X:3000/"
            {
                if (_itemCounter > 0)
                {
                    ReadOnlyCollection<IWebElement> buttons =
                        _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                    buttons[_itemCounter + 1].Click();
                    element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
                    element.Click();
                    element.SendKeys(name);
                    buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                    buttons[_itemCounter + 2].Click();
                    ReadOnlyCollection<IWebElement> dropdownItems =
                        _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
                    for (int i = 0; i < dropdownItems.Count; i++)
                    {
                        if (dropdownItems[i].Text == sectionName)
                        {
                            dropdownItems[i].Click();
                        }
                    }

                    buttons[_itemCounter + 3].Click();
                    _itemCounter++;
                    element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("button")));
                    element.SendKeys(Keys.Escape);
                }
                else
                {
                    element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
                    element.Click();
                    element.SendKeys(name);
                    ReadOnlyCollection<IWebElement> buttons =
                        _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
                    buttons[1].Click();
                    ReadOnlyCollection<IWebElement> dropdownItems =
                        _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
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
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            if (_sectionCounter == 0)
            {
                _wait.Until(e => e.FindElement(By.ClassName("input")).GetAttribute("placeholder") == "New section name");
            }

            ReadOnlyCollection<IWebElement> inputFields =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            inputFields[_sectionCounter].Click();
            inputFields[_sectionCounter].SendKeys(name);
            ReadOnlyCollection<IWebElement> buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[_sectionCounter + 1].Click();
            _sectionCounter++;
        }

        public void RemoveAllItems()
        {
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Options")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[1].Click();
            buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[3].Click();
            _itemCounter = 0;
        }

        public void RemoveItem(string name)
        {
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            ReadOnlyCollection<IWebElement> items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (int i = 0; i < items.Count; i++)
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
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> checks =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("check")));
            ReadOnlyCollection<IWebElement> items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
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
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            ReadOnlyCollection<IWebElement> items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("input")));
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
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Shopping list")));
            element.Click();
            ReadOnlyCollection<IWebElement> buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            ReadOnlyCollection<IWebElement> items =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("is-size-4")));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Text == name)
                {
                    buttons[i + 1].Click();
                }
            }

            buttons = _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            buttons[items.Count + 2].Click();
            ReadOnlyCollection<IWebElement> dropdownItems =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("dropdown-item")));
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
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            ReadOnlyCollection<IWebElement> inputFields =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
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
            IWebElement element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Edit sections")));
            element.Click();
            ReadOnlyCollection<IWebElement> inputFields =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("input")));
            ReadOnlyCollection<IWebElement> buttons =
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("button")));
            for (int i = 0; i < inputFields.Count; i++)
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