using System.Windows;
using demoexam.Entities;

namespace demoexam.pages;

public partial class EditItemWindow : Window
{
    private readonly Database _database;
    public EditItemWindow(string? itemArticle, Database database)
    {
        InitializeComponent();
        _database = database;
    }
}