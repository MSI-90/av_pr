using AvitoParse.Shared;

namespace AvitoParse.Contracts
{
  public interface ISearchService
  {
    List<CardProductOutputDTO> GetSearchResults(SearchDTO? query);
    void ChangeSearchLocation(string region);
  }
}
