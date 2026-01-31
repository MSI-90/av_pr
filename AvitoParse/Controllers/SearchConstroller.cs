using AvitoParse.Contracts;
using AvitoParse.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AvitoParse.Controllers
{
  [Route("api/search")]
  [ApiController]
  public class SearchConstroller : ControllerBase
  {
    private readonly ISearchService _searchService;
    public SearchConstroller(ISearchService searchService)
    {
      _searchService = searchService;
    }

    [HttpGet]
    public IActionResult GetQuery([FromBody] SearchDTO searchDto)
    {
      var search = _searchService.GetSearchResults(searchDto);
      return Ok(search);
    }
  }
}
