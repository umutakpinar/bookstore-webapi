using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public abstract record BookDtoForManipulation
{
    [Required(ErrorMessage = "Price is required.")]
    [MinLength(2, ErrorMessage = "Title should be 2 characters at least.")]
    [MaxLength(50, ErrorMessage = "Title must consist of at maximum 50 characters.")]
    public String Title { get; init; }
    [Required(ErrorMessage = "Price is required.")]
    [Range(10,1000)]
    public decimal Price { get; init; }
}