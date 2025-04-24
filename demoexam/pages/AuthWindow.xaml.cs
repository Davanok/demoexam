using System.Windows;
using demoexam.Entities;

namespace demoexam.pages;

public partial class AuthWindow : Window
{
    private readonly Database _database;
    public User? User { private set; get; }
    public AuthWindow(Database database)
    {
        InitializeComponent();
        _database = database;
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e) { Close(); }

    private void OnContinueButtonClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(UserNameField.Text))
        {
            MessageBox.Show("username text field cannot be empty");
            return;
        }

        var user = _database.GetUser(UserNameField.Text);
        if (user == null) {
            MessageBox.Show("user with that username doesn't found");
            return;
        }
        User = user;
        Close();
    }
}