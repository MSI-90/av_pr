using System.ComponentModel.DataAnnotations;

namespace AvitoParse.Shared
{
  public record SearchDTO()
  {
    [Required(ErrorMessage = "Необходимо указать предмет поиска")]
    public string? Query {  get; init; }

    public string? Region { get; init; }
  }
}
