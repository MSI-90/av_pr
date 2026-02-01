using AvitoParse.Shared;

namespace AvitoParse.Contracts
{
  public interface ISearchService
  {
    IEnumerable<CardProductOutputDTO> GetSearchResults(SearchDTO? query);
    void ChangeSearchLocation(string region);
  }
}
