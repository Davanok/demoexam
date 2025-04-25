using demoexam.Entities;

namespace demoexam;

using MySql.Data.MySqlClient;

public class Database
{
    private const string GetFullItemsQuery = """
                                             select items.article       as article,
                                                    items.name          as name,
                                                    categories.name     as category,
                                                    items.description   as description,
                                                    items.price         as price,
                                                    existing_items.sale as sale,
                                                    items.image         as image
                                             from items
                                                      RIGHT JOIN categories ON category_id = categories.id
                                                      RIGHT JOIN existing_items ON existing_items.article = items.article
                                             """;

    private const string GetUserQuery = """
                                        SELECT 
                                            u.user_id   AS user_id, 
                                            u.username  AS username,
                                            u.role_id   AS role_id,
                                            r.role_name AS role_name
                                        FROM users AS u
                                        INNER JOIN roles AS r 
                                            ON r.role_id = u.role_id
                                        WHERE u.username = @username
                                        LIMIT 1;
                                        """;

    private readonly MySqlConnection _connection = new(
        "server=localhost; user=root; database=shop; port=3306; password=database"
    );

    public List<UiItem> GetItems()
    {
        List<UiItem> items = [];
        _connection.Open();
        try
        {
            MySqlCommand command = new(GetFullItemsQuery, _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var image = reader["image"] as string;
                image = string.IsNullOrWhiteSpace(image) ? null : image;
                UiItem item = new()
                {
                    Article = reader.GetString("article"),
                    Name = reader.GetString("name"),
                    Category = reader.GetString("category"),
                    Description = reader.GetString("description"),
                    Price = reader.GetFloat("price"),
                    Sale = reader.GetFloat("sale"),
                    Image = image,
                };
                items.Add(item);
            }

            reader.Close();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _connection.Close();
        }

        return items;
    }

    public User? GetUser(string name)
    {
        User? result = null;
        _connection.Open();
        try
        {
            MySqlCommand command = new(GetUserQuery, _connection);
            command.Parameters.AddWithValue("@username", name);
            var reader = command.ExecuteReader();
            if (reader.Read())
                result = new User
                {
                    UserId = reader.GetInt32("user_id"),
                    UserName = reader.GetString("username"),
                    Role = new Role
                    {
                        RoleId = reader.GetInt32("role_id"),
                        Name = reader.GetString("role_name")
                    }
                };
            reader.Close();
        }
        finally
        {
            _connection.Close();
        }

        return result;
    }

    private List<Category> GetCategories(MySqlConnection connection)
    {
        List<Category> categories = [];
        MySqlCommand command = new("SELECT id, name FROM categories", connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            categories.Add(new Category
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name")
            });
        }

        reader.Close();
        return categories;
    }

    private List<Supplier> GetSuppliers(MySqlConnection connection)
    {
        List<Supplier> suppliers = [];
        MySqlCommand command = new("SELECT id, name FROM suppliers", connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            suppliers.Add(new Supplier
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name")
            });
        }

        reader.Close();
        return suppliers;
    }

    private List<Manufacturer> GetManufacturers(MySqlConnection connection)
    {
        List<Manufacturer> manufacturers = [];
        MySqlCommand command = new("SELECT id, name FROM manufacturers", connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            manufacturers.Add(new Manufacturer
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name")
            });
        }

        reader.Close();
        return manufacturers;
    }

    public void GetLists(
        out List<Category> categories,
        out List<Supplier> suppliers,
        out List<Manufacturer> manufacturers
    )
    {
        _connection.Open();
        try
        {
            categories = GetCategories(_connection);
            suppliers = GetSuppliers(_connection);
            manufacturers = GetManufacturers(_connection);
        }
        finally
        {
            _connection.Close();
        }
    }

    private const string GetEditableItemQuery = """
                                                SELECT i.`article` as article,
                                                    i.name as name,
                                                    i.measurement_unit as measurement_unit,
                                                    i.`category_id` as category_id,
                                                    i.`max_sale` as max_sale,
                                                    i.`supplier_id` as supplier_id,
                                                    i.`manufacturer_id` as manufacturer_id,
                                                    i.`image` as image,
                                                    i.`description` as description,
                                                    i.`price` as price,
                                                    e.`count` as count,
                                                    e.`sale` as sale
                                                FROM `items` i
                                                    RIGHT JOIN `existing_items` e ON i.`article` = e.`article`
                                                WHERE i.`article` = @article
                                                """;

    public EditableItem? GetItemForEdit(string article)
    {
        EditableItem? result = null;
        _connection.Open();
        try
        {
            var categories = GetCategories(_connection);
            var suppliers = GetSuppliers(_connection);
            var manufacturers = GetManufacturers(_connection);

            MySqlCommand command = new(GetEditableItemQuery, _connection);
            command.Parameters.AddWithValue("@article", article);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var image = reader["image"] as string;
                image = string.IsNullOrWhiteSpace(image) ? null : image;
                result = new EditableItem
                {
                    Article = reader.GetString("article"),
                    Name = reader.GetString("name"),
                    MeasurementUnit = reader.GetString("measurement_unit"),
                    Count = reader.GetInt32("count"),
                    MaxSale = reader.GetInt32("max_sale"),
                    Sale = reader.GetInt32("sale"),
                    CategoryId = reader.GetInt32("category_id"),
                    SupplierId = reader.GetInt32("supplier_id"),
                    ManufacturerId = reader.GetInt32("manufacturer_id"),
                    Image = image,
                    Description = reader.GetString("description"),
                    Price = reader.GetFloat("price"),

                    Categories = categories,
                    Suppliers = suppliers,
                    Manufacturers = manufacturers
                };
            }

            reader.Close();
        }
        finally
        {
            _connection.Close();
        }

        return result;
    }

    public void DeleteItem(string article)
    {
        _connection.Open();
        try
        {
            MySqlCommand command = new("DELETE FROM items WHERE article = @article", _connection);
            command.Parameters.AddWithValue("@article", article);
            command.ExecuteNonQuery();
        }
        finally
        {
            _connection.Close();
        }
    }

    private const string SaveItemQuery = """
                                   REPLACE INTO items (
                                           article, 
                                           name,
                                           measurement_unit,
                                           price,
                                           max_sale, 
                                           description, 
                                           manufacturer_id,
                                           supplier_id, 
                                           category_id, 
                                           image
                                           ) 
                                   VALUES (
                                           @article,
                                           @name,
                                           @measurement_unit,
                                           @price,
                                           @max_sale,
                                           @description,
                                           @manufacturer_id,
                                           @supplier_id,
                                           @category_id,
                                           @image
                                   )
                                   """;
    private const string SaveExistingItemQuery = """
                                   REPLACE INTO existing_items (article, sale, count) 
                                   VALUES (@article, @sale, @count)
                                   """;

    public void SaveItem(EditableItem item)
    {
        _connection.Open();
        try
        {
            MySqlCommand itemCommand = new(SaveItemQuery, _connection);
            itemCommand.Parameters.AddWithValue("@article", item.Article);
            itemCommand.Parameters.AddWithValue("@name", item.Name);
            itemCommand.Parameters.AddWithValue("@measurement_unit", item.MeasurementUnit);
            itemCommand.Parameters.AddWithValue("@price", item.Price);
            itemCommand.Parameters.AddWithValue("@max_sale", item.MaxSale);
            itemCommand.Parameters.AddWithValue("@description", item.Description);
            itemCommand.Parameters.AddWithValue("@manufacturer_id", item.ManufacturerId);
            itemCommand.Parameters.AddWithValue("@supplier_id", item.SupplierId);
            itemCommand.Parameters.AddWithValue("@category_id", item.CategoryId);
            itemCommand.Parameters.AddWithValue("@image", item.Image);
            itemCommand.ExecuteNonQuery();
            
            MySqlCommand existingItemCommand = new(SaveExistingItemQuery, _connection);
            existingItemCommand.Parameters.AddWithValue("@article", item.Article);
            existingItemCommand.Parameters.AddWithValue("@sale", item.Sale);
            existingItemCommand.Parameters.AddWithValue("@count", item.Count);
            existingItemCommand.ExecuteNonQuery();
        }
        finally
        {
            _connection.Close();
        }
    }
}