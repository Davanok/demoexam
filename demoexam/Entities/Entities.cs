namespace demoexam.Entities;

public class Categories
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}
public class Manufacturers
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}
public class Suppliers
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}
public class Items
{
    public required string Article { get; set; }
    public required string Name { get; set; }
    public required string Unit { get; set; }
    public required double Price { get; set; }
    public required double MaxSale { get; set; }
    public required string Description { get; set; }
    public required int ManufacturerId { get; set; }
    public required int SupplierId { get; set; }
    public required int CategoryId { get; set; }
    public required string? Image { get; set; }
}
public class ExistingItems
{
    public required int Id { get; set; }
    public required string Article { get; set; }
    public required double Sale { get; set; }
    public required int Amount { get; set; }
}

public class FullItem 
{
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string Description { get; set; }
    public required float Price { get; set; }
    public required float Sale { get; set; }
    public required string? Image { get; set; }
    
    public override string ToString()
    {
        return $"FullItem({Name}, {Category}, {Description}, {Price}, {Sale}, {Image})";
    }
}