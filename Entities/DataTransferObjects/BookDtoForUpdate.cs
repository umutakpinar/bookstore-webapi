namespace Entities.DataTransferObjects;

// dto objeleri
// readonly
// immutable
// LINQ
// Ref type
// ctor
public record BookDtoForUpdate
{
    public int Id { get; init; }
    public String Title { get; init; }
    public decimal Price { get; init; }
}