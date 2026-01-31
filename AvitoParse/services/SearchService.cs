using AvitoParse.configs;
using AvitoParse.Contracts;
using AvitoParse.Models.Exceptions;
using AvitoParse.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AvitoParse.services
{
  // TODO: нет проверки на открытие диалога смены региона поиска, нужна ли она?
  public sealed class SearchService : ISearchService
  {
    Config Config { get; set; }
    IWebDriver? _driver;
    string _defaultSearchValue = "Все регионы";
    public SearchService()
    {
      Config = new Config();
      _driver = Config.DriverInit();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchDto"></param>
    /// <returns></returns>
    public IEnumerable<string> GetSearchResults(SearchDTO? searchDto)
    {
      if (string.IsNullOrWhiteSpace(searchDto?.Query))
        return Enumerable.Empty<string>();

      if (_driver == null) 
        return Enumerable.Empty<string>();

      try
      {
        _driver?.FindElement(helpers.ElemPath.searchInput).SendKeys(searchDto?.Query + Keys.Enter);

        var wait = new WebDriverWait(_driver!, TimeSpan.FromSeconds(2));

        Thread.Sleep(1500);

        ChangeSearchLocation(searchDto?.Region ?? _defaultSearchValue);

        //searchItems = _driver?.FindElements(helpers.ElemPath.itemSelector)!;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Неполадка {ex.Message}");
      }
      finally
      {
        //_driver?.Quit();
      }

      return Enumerable.Empty<string>();
    }

    //TODO: пересмотреть далее параметр на параметр DTO модели.
    //TODO: устновлен Thread.Sleep(2000), в связи с тем, что с 4 версии прекращена поддержка Expected Conditions для .NET, посмотреть иные варианты
    /// <summary>
    /// Смена региона поиска
    /// </summary>
    public void ChangeSearchLocation(string region) 
    {
      if (_driver is null) return;
      
      try
      {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        {
          PollingInterval = TimeSpan.FromMilliseconds(500)
        };

        var changeRegionElement = _driver?.FindElement(helpers.ElemPath.searchRegion) ?? _driver?.FindElement(helpers.ElemPath.searchRegion0);
        if (changeRegionElement is null)
          throw new ChangeRegionElementException();

        changeRegionElement.Click();

        var searchInput = _driver?.FindElement(helpers.ElemPath.regionInput);
        searchInput?.Click();
        while (!string.IsNullOrEmpty(searchInput?.GetAttribute("value")))
        {
          searchInput?.SendKeys(Keys.Backspace);
        }
        
        //установить текст в input и прожать Enter
        searchInput?.SendKeys(region + Keys.Enter);

        Thread.Sleep(2000);

        //Выпадающий список, отображающийся после ввода и подтверждения текста
        var regionAfterSendText = _driver?.FindElement(helpers.ElemPath.regionAfterSendText);

        // Элементы выпадающего списка представленные как Button-обертка
        var buttonItems = regionAfterSendText?.FindElements(By.XPath(".//button"));

        // Найти полное совпадение строки
        // TODO: пересмотреть на неполное совпадение строк
        foreach (var buttonItem in buttonItems)
        {
          var spanItems = buttonItem.FindElements(By.XPath(".//span"));
          var isBreaked = CheckStrictEntry(region, buttonItems) || CheckPartialEntry(region, buttonItems);

          if (isBreaked)
            break;
        }

        // Прожать кнопку поиска всех объявлений по региону
        // TODO: понаблюдать за сбоями в прожатии элемента
        _driver?.FindElement(helpers.ElemPath.showAnnouncement).Click();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Неполадка {ex.Message}");
      }
      finally
      {
        //_driver?.Quit();
      }
    }

    public bool CheckStrictEntry(string region, ReadOnlyCollection<IWebElement> spanItems)
    {
      if (spanItems is null) 
        return false;

      foreach (var spanItem in spanItems)
      {
        var text = spanItem.Text;
        if (text.ToLower().Equals(region?.ToLower()))
        {
          spanItem.Click();
          return true;
        }
      }

      return false;
    }

    public bool CheckPartialEntry(string region, ReadOnlyCollection<IWebElement> spanItems)
    {
      if (spanItems is null)
        return false;

      foreach (var spanItem in spanItems)
      {
        var text = spanItem.Text;
        if (text.Contains(region!, StringComparison.OrdinalIgnoreCase))
        {
          spanItem.Click();
          return true;
        }
      }

      return false;
    }

    //~SearchService()
    //{
    //  if (_driver is not null)
    //  {
    //    _driver?.Quit();
    //  }
    //} 
  }
}
