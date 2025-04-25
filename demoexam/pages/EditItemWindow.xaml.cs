using System.Globalization;
using System.Windows;
using System.Windows.Controls.Ribbon.Primitives;
using demoexam.Entities;
using static System.Int32;

namespace demoexam.pages;

public partial class EditItemWindow : Window
{
    private readonly Database _database;
    private readonly EditableItem _item;
    
    public EditItemWindow(string? itemArticle, Database database)
    {
        InitializeComponent();
        _database = database;
        
        ItemArticle.IsEnabled = itemArticle == null;
        DeleteButton.Visibility = itemArticle != null ? Visibility.Visible : Visibility.Collapsed;

        _item = itemArticle == null ? GetEmptyItem() : _database.GetItemForEdit(itemArticle) ?? GetEmptyItem();
        SetView();
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
        ItemMeasurementUnit.Text = _item.MeasurementUnit;
        ItemCount.Text = _item.Count.ToString();
        ItemMaxSale.Text = _item.MaxSale.ToString(CultureInfo.CurrentCulture);
        ItemSale.Text = _item.Sale.ToString(CultureInfo.CurrentCulture);
        ItemDescription.Text = _item.Description;
        ItemPrice.Text = _item.Price.ToString(CultureInfo.CurrentCulture);
        
        ItemCategory.ItemsSource = _item.Categories;
        ItemCategory.SelectedIndex = _item.Categories.FindIndex(c => c.Id == _item.CategoryId);
        ItemSupplier.ItemsSource = _item.Suppliers;
        ItemSupplier.SelectedIndex = _item.Suppliers.FindIndex(c => c.Id == _item.SupplierId);
        ItemManufacturer.ItemsSource = _item.Manufacturers;
        ItemManufacturer.SelectedIndex = _item.Manufacturers.FindIndex(c => c.Id == _item.ManufacturerId);
    }

    private void DeleteButtonClick(object sender, RoutedEventArgs e)
    {
        _database.DeleteItem(_item.Article);
        Close();
    }

    private string? ReadItem()
    {
        _item.Article = ItemArticle.Text;
        _item.Name = ItemName.Text;
        _item.MeasurementUnit = ItemMeasurementUnit.Text;
        
        if (int.TryParse(ItemCount.Text, out var count)) _item.Count = count;
        else return "Количество";
        if (float.TryParse(ItemMaxSale.Text, out var maxSale)) _item.MaxSale = maxSale;
        else return "Максимальная скидка";
        if (float.TryParse(ItemSale.Text, out var sale)) _item.Sale = sale;
        else return "Скидка";

        if (ItemCategory.SelectedItem == null) return "Категория";
        _item.CategoryId = (ItemCategory.SelectedItem as Category)!.Id;
        if (ItemSupplier.SelectedItem == null) return "Поставщик";
        _item.SupplierId = (ItemSupplier.SelectedItem as Supplier)!.Id;
        if (ItemManufacturer.SelectedItem == null) return "Производитель";
        _item.ManufacturerId = (ItemManufacturer.SelectedItem as Manufacturer)!.Id;

        _item.Description = ItemDescription.Text;

        if (float.TryParse(ItemPrice.Text, out var price)) _item.Price = price;
        else return "Цена";
        
        return null;
    }

    private void SaveButtonClick(object sender, RoutedEventArgs e)
    {
        var fieldError = ReadItem();
        if (fieldError != null)
        {
            MessageBox.Show($"Неправильно введено {fieldError}");
            return;
        }
        var response = _item.Validate();
        if (response != null)
        {
            MessageBox.Show($"Товар не соответствует критериям ({response})");
            return;
        }
        _database.SaveItem(_item);
        Close();
    }
}