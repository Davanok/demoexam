using System;
using System.Collections.Generic;
using demoexam.Entities;

namespace demoexam;
using MySql.Data.MySqlClient;

public class Database
{
    private const string Query = """
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
    private readonly MySqlConnection _connection = new (
        "server=localhost; user=root; database=shop; port=3306; password=database"
    );
    
    public List<FullItem> GetItems() 
    {
        List<FullItem> items = [];
        _connection.Open();
        try 
        {
            MySqlCommand command = new (Query, _connection);
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
}