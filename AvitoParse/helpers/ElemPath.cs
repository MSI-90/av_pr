using OpenQA.Selenium;

namespace AvitoParse.helpers
{
  public static class ElemPath
  {
    // Выбор  региона для поиска
    public static readonly By searchRegion0 = By.XPath("//div[@class='main-link-wrapper-FaUIo']//span[@class='main-link-firstLine-ffKwQ']");
    public static readonly By searchRegion = By.XPath("//div[@class='index-rightWrapper-su9bk']//span[@class='buyer-pages-mfe-location-nev1ty']");
    // Сам input в форме ввода региона
    public static readonly By regionInput = By.XPath("//input[@class='styles-module-searchInput-ndCjY']");
    // Элемент "выбора" региона из выпадающего списка при клике на иконку очистки
    public static readonly By regionInputClear = By.XPath("//span[@class='styles-module-crossIcon__size_m-LFGmM']");
    // Элемент клнтейнер (выпадающий списко) который появляется после установки текста региона
    public static readonly By regionAfterSendText = By.XPath("//div[@class='styles-module-dropdownScrollWrapper-q51_9']");
    // Кнопка показать более 1 тыс. объявлений
    public static readonly By showAnnouncement = By.XPath("//button[@data-marker='popup-location/save-button']");

    // Поле ввода для поиска
    public static readonly By searchInput = By.XPath("//input[@class='styles-module-input-Z0mvi']");

    // Контейнер с результатами поиска
    public static readonly By searchContainer = By.XPath("//div[@class='index-content-FRUkN']");

    #region Карточка товара в списке результатов поиска
    // Элемент объявления в результатах поиска
    public static readonly By itemSelector = By.XPath("//div[@data-marker='item']");
    // Ссылка на объявление в элементе объявления
    public static readonly By itemUrl = By.XPath(".//div[1]//div[1]//a");
    // Заголовок объявления в элементе объявления
    public static readonly By itemTitle = By.XPath(".//div[1]//div[1]//div[2]//div[2]//div[1]//div[1]//div[1]//h2//a");
    // Цена объявления в элементе объявления
    public static readonly By itemPrice = By.XPath(".//div[1]//div[1]//div[2]//div[2]//div[2]//span//div[1]//div[1]//p//strong//span");
    #endregion



  }
}
