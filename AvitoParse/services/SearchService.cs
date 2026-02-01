using AvitoParse.configs;
using AvitoParse.Contracts;
using AvitoParse.Models.Exceptions;
using AvitoParse.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public List<CardProductOutputDTO> GetSearchResults(SearchDTO? searchDto)
    {
      if (string.IsNullOrWhiteSpace(searchDto?.Query))
        return new List<CardProductOutputDTO>();

      if (_driver == null) 
        return new List<CardProductOutputDTO>();

      IReadOnlyCollection<IWebElement> searchItems;
      var productsInfo = new List<CardProductOutputDTO>();
      try
      {
        _driver?.FindElement(helpers.ElemPath.searchInput).SendKeys(searchDto?.Query + Keys.Enter);

        var wait = new WebDriverWait(_driver!, TimeSpan.FromSeconds(2));

        Thread.Sleep(3000);

        ChangeSearchLocation(searchDto?.Region ?? _defaultSearchValue);

        searchItems = _driver?.FindElements(helpers.ElemPath.itemSelector)!;
        productsInfo = GetCardProducts(searchItems);
      }
      catch
      {
        throw new Exception("Произошла ошибка, ответсвенные уже знают о ней. Попробуйте повторить запрос через 1 час.");
      }
      finally
      {
        //_driver?.Quit();
      }

      return productsInfo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchItems">Коллекция IWebElement</param>
    /// <returns>Коллекция типа CardProductOutputDTO - список информации о карточках товаров</returns>
    static List<CardProductOutputDTO> GetCardProducts(IReadOnlyCollection<IWebElement> searchItems)
    {
      var productsInfo = new List<CardProductOutputDTO>();
      foreach (var item in searchItems)
      {
        var itemUrl = item.FindElement(helpers.ElemPath.itemUrl).GetAttribute("href");
        var itemTitle = item.FindElement(helpers.ElemPath.itemTitle).Text;
        var itemPrice = item.FindElement(helpers.ElemPath.itemPrice).Text;

        productsInfo.Add(new CardProductOutputDTO(itemUrl, itemTitle, itemPrice));
      }
      return productsInfo;
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

        var changeRegionElement = _driver?.FindElements(helpers.ElemPath.searchRegion)
          .FirstOrDefault() ?? _driver?.FindElements(helpers.ElemPath.searchRegion0).FirstOrDefault();

        if (changeRegionElement is null)
          throw new ChangeRegionElementException();

        changeRegionElement.Click();

        var searchInput = ShowAnnouncement();

        searchInput?.Click();
        while (!string.IsNullOrEmpty(searchInput?.GetAttribute("value")))
        {
          searchInput?.SendKeys(Keys.Backspace);
        }
        
        //установить текст в input и прожать Enter
        searchInput?.SendKeys(region + Keys.Enter);

        Thread.Sleep(3000);

        //Выпадающий список, отображающийся после ввода и подтверждения текста
        var regionAfterSendText = _driver?.FindElement(helpers.ElemPath.regionAfterSendText);

        // Элементы выпадающего списка представленные как Button-обертка
        var buttonItems = regionAfterSendText?.FindElements(By.XPath(".//button"));

        // Найти полное или частичное совпадение строки
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

    IWebElement? ShowAnnouncement()
    {
      IReadOnlyCollection<IWebElement>? searchInputs = _driver?.FindElements(helpers.ElemPath.regionInput);
      IWebElement? searchInput = null;
      if (searchInputs?.Count > 1)
      {
        searchInput = searchInputs
          .Where(input => string.Equals(
            input.GetAttribute("data-marker"), "popup-location/region/search-input", StringComparison.OrdinalIgnoreCase))
          .FirstOrDefault();
      }
      else
        searchInput = searchInputs?.FirstOrDefault();

      return searchInput;
    }

    static bool CheckStrictEntry(string region, ReadOnlyCollection<IWebElement> spanItems)
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

    static bool CheckPartialEntry(string region, ReadOnlyCollection<IWebElement> spanItems)
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
  }
}
