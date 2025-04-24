using demoexam.Entities;

namespace demoexam;
using MySql.Data.MySqlClient;

public class Database
{
    private const string GetFullItemsQuery = """
                                 select items.name          as name,
                                        categories.name     as category,
                                        items.description   as description,
                                        items.price         as price,
                                        existing_items.sale as sale,
                                        items.image         as image
                                 from items
                                          RIGHT JOIN categories ON category = categories.id
                                          RIGHT JOIN existing_items ON existing_items.item_id = article
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
    private readonly MySqlConnection _connection = new (
        "server=localhost; user=root; database=shop; port=3306; password=database"
    );
    
    public List<FullItem> GetItems() 
    {
        List<FullItem> items = [];
        _connection.Open();
        try 
        {
            MySqlCommand command = new (GetFullItemsQuery, _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                FullItem item = new()
                {
                    Name = reader.GetString("name"),
                    Category = reader.GetString("category"),
                    Description = reader.GetString("description"),
                    Price = reader.GetFloat("price"),
                    Sale = reader.GetFloat("sale"),
                    Image = string.IsNullOrWhiteSpace(reader["image"].ToString()) ? null : reader.GetString("image"),
                };
                items.Add(item);
            }
            reader.Close();
        } catch (MySqlException ex) {
            Console.WriteLine(ex.Message);
        } finally {
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
            MySqlCommand command = new (GetUserQuery, _connection); 
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
}