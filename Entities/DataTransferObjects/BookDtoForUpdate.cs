namespace Entities.DataTransferObjects;

// dto objeleri
// readonly
// immutable
// LINQ
// Ref type
// ctor
public record BookDtoForUpdate
{
    public int Id { get; set; }
    public String Title { get; set; }
    public decimal Price { get; set; }
}