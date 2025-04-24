namespace demoexam.Entities;

public class Category
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public class Manufacturer
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public class Supplier
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

public class UiItem
{
    public required string Article { get; init; }
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

public class EditableItem
{
    public string Article { get; set; } = "";
    public string Name { get; set; } = "";
    public int Count { get; set; } = 0;
    public float MaxSale { get; set; } = 0;
    public float Sale { get; set; } = 0;
    public int CategoryId { get; set; } = -1;
    public int SupplierId { get; set; } = -1;
    public int ManufacturerId { get; set; } = -1;
    public string? Image { get; set; } = null;
    public string Description { get; set; } = "";
    public float Price { get; set; } = 0;

    bool Validate() => !(
        string.IsNullOrEmpty(Article) || 
        string.IsNullOrEmpty(Name) || 
        Categories.Select(c => c.Id).Contains(CategoryId) || 
        Count < 0 ||
        MaxSale < 0 || 
        Sale < 0 || 
        Sale > MaxSale || 
        Suppliers.Select(s => s.Id).Contains(SupplierId) || 
        Manufacturers.Select(m => m.Id).Contains(ManufacturerId)
    );

    public required List<Category> Categories { get; init; }
    public required List<Supplier> Suppliers { get; init; }
    public required List<Manufacturer> Manufacturers { get; init; }
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