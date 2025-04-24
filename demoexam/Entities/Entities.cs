namespace demoexam.Entities;

public class Categories
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
public class Manufacturers
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
public class Suppliers
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
public class Items
{
    public required string Article { get; init; }
    public required string Name { get; init; }
    public required string Unit { get; init; }
    public required double Price { get; init; }
    public required double MaxSale { get; init; }
    public required string Description { get; init; }
    public required int ManufacturerId { get; init; }
    public required int SupplierId { get; init; }
    public required int CategoryId { get; init; }
    public required string? Image { get; init; }
}
public class ExistingItems
{
    public required int Id { get; init; }
    public required string Article { get; init; }
    public required double Sale { get; init; }
    public required int Amount { get; init; }
}

public class FullItem 
{
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required string Description { get; init; }
    public required float Price { get; init; }
    public required float Sale { get; init; }
    public required string? Image { get; set; }
    
    public override string ToString()
    {
        return $"FullItem({Name}, {Category}, {Description}, {Price}, {Sale}, {Image})";
    }
}

public class Role
{
    public required int RoleId { get; init; }
    public required string Name { get; init; }
}

public class User
{
    public required int UserId { get; init; }
    public required string UserName { get; init; }
    public required Role Role;
}