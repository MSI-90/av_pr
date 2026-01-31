using AvitoParse.Shared;

namespace AvitoParse.Contracts
{
  public interface ISearchService
  {
    IEnumerable<string> GetSearchResults(SearchDTO? query);
  }
}
