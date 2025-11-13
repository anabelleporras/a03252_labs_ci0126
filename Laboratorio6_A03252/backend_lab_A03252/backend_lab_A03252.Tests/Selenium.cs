using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend_lab_A03252.Tests
{
  public class Selenium
  {
    IWebDriver _driver;
    WebDriverWait _wait;

    [SetUp]
    public void Setup()
    {
      _driver = new ChromeDriver();
      _driver.Manage().Window.Maximize();
      _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
    }

    [Test]
    public void Enter_To_List_Of_Countries_Test()
    {
      //Arrange
      var URL = "http://localhost:8080/";

      //Act
      _driver.Navigate().GoToUrl(URL);

      //Assert
      Assert.That(_driver, Is.Not.Null);
    }

    [Test]
    public void create_New_Country_Test()
    {
      //Arrange
      var URL = "http://localhost:8080/";

      //Act
      _driver.Navigate().GoToUrl(URL);

      var createButton = _driver.FindElement(By.CssSelector(".btn-outline-secondary"));
      createButton.Click();

      var countryNameInput = _driver.FindElement(By.Id("name"));
      countryNameInput.SendKeys("TestCountry");

      var ddlContinent = new SelectElement(_driver.FindElement(By.Id("continente")));
      ddlContinent.SelectByText("Asia");

      var languageInput = _driver.FindElement(By.Id("idioma"));
      languageInput.SendKeys("TestLanguage");

      var saveButton = _driver.FindElement(By.CssSelector(".btn"));
      saveButton.Click();

      _wait.Until(d =>
      {
        var current = d.Url.TrimEnd('/');
        var expected = URL.TrimEnd('/');
        return String.Equals(current, expected, StringComparison.OrdinalIgnoreCase);
      });

      _wait.Until(d => d.FindElement(By.TagName("table")));

      //Assert
      var createdCountry = _wait.Until(d =>
        d.FindElement(By.XPath("//table//td[contains(., 'TestCountry')]"))
      );

      Assert.That(createdCountry, Is.Not.Null);
    }

    [Test]
    public void create_New_Country_Validate_Name_Test()
    {
      //Arrange
      var URL = "http://localhost:8080/";
      _driver.Navigate().GoToUrl(URL);

      //Act
      var createButton = _driver.FindElement(By.CssSelector(".btn-outline-secondary"));
      createButton.Click();

      var nameInput = _driver.FindElement(By.Id("name"));

      var saveButton = _driver.FindElement(By.CssSelector(".btn"));
      saveButton.Click();

      //Assert
      IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
      bool nameValid = (bool)js.ExecuteScript("return document.getElementById('name').checkValidity();");

      Assert.That(nameValid, Is.False);
    }

    [Test]
    public void create_New_Country_Validate_Language_Test()
    {
      //Arrange
      var URL = "http://localhost:8080/";
      _driver.Navigate().GoToUrl(URL);

      //Act
      var createButton = _driver.FindElement(By.CssSelector(".btn-outline-secondary"));
      createButton.Click();

      var countryNameInput = _driver.FindElement(By.Id("name"));
      countryNameInput.SendKeys("TestCountry");

      var ddlContinent = new SelectElement(_driver.FindElement(By.Id("continente")));
      ddlContinent.SelectByText("Asia");

      var languageInput = _driver.FindElement(By.Id("idioma"));

      var saveButton = _driver.FindElement(By.CssSelector(".btn"));
      saveButton.Click();

      //Assert
      IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
      bool languageValid = (bool)js.ExecuteScript("return document.getElementById('idioma').checkValidity();");

      Assert.That(languageValid, Is.False);
    }
  }
}
