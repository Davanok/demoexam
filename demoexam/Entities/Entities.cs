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
    public string MeasurementUnit { get; set; } = "";
    public int Count { get; set; }
    public float MaxSale { get; set; }
    public float Sale { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public int ManufacturerId { get; set; }
    public string? Image { get; set; }
    public string Description { get; set; } = "";
    public float Price { get; set; }

    public string? Validate()
    {
        if (string.IsNullOrEmpty(Article)) return "Артикул не может быть пустым";
        if (string.IsNullOrEmpty(Name)) return "Название не может быть пустым";
        if (!Categories.Select(c => c.Id).Contains(CategoryId)) return $"Категории с Id {CategoryId} не существует";
        if (!Suppliers.Select(c => c.Id).Contains(SupplierId)) return $"Поставщика с Id {SupplierId} не существует";
        if (!Manufacturers.Select(c => c.Id).Contains(ManufacturerId)) return $"Производителя с Id {ManufacturerId} не существует";
        if (Count < 0) return "Количество не может быть отрицательным";
        if (Price < 0) return "Цена не может быть отрицательной";
        if (Sale < 0) return "Скидка не может быть отрицательной";
        if (MaxSale < 0) return "Максимальная скидка не может быть отрицательной";
        if (Sale > MaxSale) return "Скидка не может быть больше максимальной скидки";
        return null;
    }

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
    public required string PasswordHash { get; init; }
    public required Role Role;
}