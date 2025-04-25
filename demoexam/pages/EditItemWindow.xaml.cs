using System.Globalization;
using System.Windows;
using System.Windows.Controls.Ribbon.Primitives;
using demoexam.Entities;

namespace demoexam.pages;

public partial class EditItemWindow : Window
{
    private readonly Database _database;
    private readonly EditableItem _item;
    
    public EditItemWindow(string? itemArticle, Database database)
    {
        InitializeComponent();
        _database = database;
        
        ItemArticle.IsEnabled = itemArticle != null;
        DeleteButton.Visibility = itemArticle != null ? Visibility.Visible : Visibility.Collapsed;

        _item = itemArticle == null ? GetEmptyItem() : _database.GetItemForEdit(itemArticle) ?? GetEmptyItem();
        return;

        EditableItem GetEmptyItem()
        {
            database.GetLists(out var categories, out var suppliers, out var manufacturers);
            return new EditableItem
            {
                Categories = categories,
                Suppliers = suppliers,
                Manufacturers = manufacturers
            };
        }
    }
    private void SetView()
    {
        ItemArticle.Text = _item.Article;
        ItemName.Text = _item.Name;
        ItemCount.Text = _item.Count.ToString();
        ItemMaxSale.Text = _item.MaxSale.ToString(CultureInfo.CurrentCulture);
        ItemSale.Text = _item.Sale.ToString(CultureInfo.CurrentCulture);
        ItemCategory.ItemsSource = _item.Categories;
        ItemSupplier.ItemsSource = _item.Suppliers;
        ItemManufacturer.ItemsSource = _item.Manufacturers;
        ItemDescription.Text = _item.Description;
        ItemPrice.Text = _item.Price.ToString(CultureInfo.CurrentCulture);
    }

    private void DeleteButtonClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void SaveButtonClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}