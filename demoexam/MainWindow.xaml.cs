using System.Windows;
using System.Windows.Controls;
using demoexam.Entities;

namespace demoexam;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private class ComboBoxItem
    {
        public required int Id { get; init; }
        public required string Label { get; init; }
    }

    private readonly Database _database = new();

    private List<FullItem> _items = [];

    private void SetUser(User? user)
    {
        _user = user;
        if (user == null)
        {
            AuthButton.Content = "Войти";
            UserNameTextBlock.Text = "";
            
            AddItemButton.Visibility = Visibility.Collapsed;
            EditItemButton.Visibility = Visibility.Collapsed;
        }
        else
        {
            AuthButton.Content = "Выйти";
            UserNameTextBlock.Text = user.UserName;

            if (user.Role.Name == "Администратор")
            {
                AddItemButton.Visibility = Visibility.Visible;
                EditItemButton.Visibility = Visibility.Visible;
            }
            else
            {
                AddItemButton.Visibility = Visibility.Collapsed;
                EditItemButton.Visibility = Visibility.Collapsed;
            }
        }
    }
    private User? _user;

    private void LoadItems()
    {
        var imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources");
        var defaultImagePath = System.IO.Path.Combine(imagesFolder, "picture.png");
        _items = _database.GetItems();
        foreach (var item in _items)
        {
            item.Image = item.Image == null ? defaultImagePath : System.IO.Path.Combine(imagesFolder, item.Image);
        }
        AddAllItems();
    }

    private void AddAllItems()
    {
        ListBox.Items.Clear();
        foreach (var item in _items) ListBox.Items.Add(item);
        
        FoundItemsTextBlock.Text = $"Found {ListBox.Items.Count} items";
    }
    private void FilterItems(Func<FullItem, bool> filter)
    {
        ListBox.Items.Clear();
        foreach (var item in _items.Where(filter)) ListBox.Items.Add(item);
        
        FoundItemsTextBlock.Text = $"Found {ListBox.Items.Count} items";
    }

    private void OnSaleSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ComboBox.SelectedItem is not ComboBoxItem selectedItem || selectedItem.Id == 1)
            AddAllItems();
        else FilterItems( item => 
            selectedItem.Id == 2 && item.Sale is >= 0 and <= 24 || 
            selectedItem.Id == 3 && item.Sale is >= 25 and <= 49 ||
            selectedItem.Id == 4 && item.Sale is >= 50 and <= 74 ||
            selectedItem.Id == 5 && item.Sale is >= 75 and <= 100
            );
    }

    public MainWindow()
    {
        InitializeComponent();
        LoadItems();
        ComboBox.ItemsSource = new ComboBoxItem[]
        {
            new() { Id = 1, Label = "Все диапазоны" },
            new() { Id = 2, Label = "0 - 24%" },
            new() { Id = 3, Label = "25 - 49" },
            new() { Id = 4, Label = "50 - 74%" },
            new() { Id = 5, Label = "75 - 100%" },
        };
        ComboBox.SelectedIndex = 0;
    }

    private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        ListBox.Items.Clear();
        var text = TextBox.Text;
        if (string.IsNullOrEmpty(text)) AddAllItems();
        else FilterItems(item => item.Name.StartsWith(text, StringComparison.InvariantCultureIgnoreCase));
    }

    private void AuthButtonClick(object sender, RoutedEventArgs e)
    {
        if (_user != null)
        {
            SetUser(null);
            return;
        }

        AuthWindow window = new() { Database = _database };
        window.ShowDialog();
        if (window.User == null) return;
        
        SetUser(window.User);
    }

    private void AddItemButtonClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void EditItemButtonClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}