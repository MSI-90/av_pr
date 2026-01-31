using OpenQA.Selenium;

namespace AvitoParse.helpers
{
  public static class ElemPath
  {
    //Выбор  региона для поиска
    public static readonly By searchRegion = By.XPath("//div[@class='index-rightWrapper-su9bk']//span[@class='buyer-pages-mfe-location-nev1ty']");
    // Сам input в форме ввода региона
    public static readonly By regionInput = By.XPath("//input[@class='styles-module-searchInput-ndCjY']");
    // Элемент "выбора" региона из выпадающего списка при клике на иконку очистки
    public static readonly By regionInputClear = By.XPath("//span[@class='styles-module-crossIcon__size_m-LFGmM']");
    // Элемент клнтейнер (выпадающий списко) который появляется после установки текста региона
    public static readonly By regionAfterSendText = By.XPath("//div[@class='styles-module-dropdownScrollWrapper-q51_9']");
    // Кнопка показать более 1 тыс. объявлений
    public static readonly By showAnnouncement = By.XPath("//button[@data-marker='popup-location/save-button']");

    //Поле ввода для поиска
    public static readonly By searchInput = By.XPath("//input[@class='styles-module-input-Z0mvi']");

    //Контейнер с результатами поиска
    public static readonly By searchContainer = By.XPath("//div[@class='index-content-FRUkN']");

    //Элемент объявления в результатах поиска
    public static readonly By itemSelector = By.XPath("//div[@data-marker='item']");


  }
}
