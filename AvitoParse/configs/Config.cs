using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AvitoParse.configs
{
  public class Config
  {
    IWebDriver? _driver;
    public IWebDriver? Driver 
    {
      get { return _driver; } 
    }
    readonly string? _url = "https://www.avito.ru/";
    private int _waitForLoading = 10000;
    public IWebDriver? DriverInit()
    {
      try
      {
        var options = new ChromeOptions();
        //options.AddArgument("--ignore-certificate-errors");
        //options.AddArgument("--headless");
        //options.AddArgument("--disable-web-security");
        //options.AddArgument("--disable-dev-shm-usage");
        //options.AddArgument("--window-size=1920,1080");
        //options.AddArgument("--no-sandbox");
        //options.AddArgument("--no-proxy-server");  // Отключает все прокси
        //options.AddArgument("--proxy-server='direct://'");
        //options.AddArgument("--proxy-bypass-list=*");

        _driver = new ChromeDriver(options);
        _driver.Navigate().GoToUrl(_url!);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(_waitForLoading);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Неполадка {0}", ex.Message);
      }
      return _driver;
    }
  }
}
