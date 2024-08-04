using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

// dto objeleri
// readonly
// immutable
// LINQ
// Ref type
// ctor
public record BookDtoForUpdate : BookDtoForManipulation
{
    [Required]
    public int Id { get; init; }
}