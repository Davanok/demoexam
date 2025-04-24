using System.Windows;
using demoexam.Entities;

namespace demoexam;

public partial class AuthWindow : Window
{
    public required Database Database;
    public User? User { private set; get; }
    public AuthWindow()
    {
        InitializeComponent();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e) { Close(); }

    private void OnContinueButtonClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(UserNameField.Text))
        {
            MessageBox.Show("username text field cannot be empty");
            return;
        }

        var user = Database.GetUser(UserNameField.Text);
        if (user == null) {
            MessageBox.Show("user with that username doesn't found");
            return;
        }
        User = user;
        Close();
    }
}