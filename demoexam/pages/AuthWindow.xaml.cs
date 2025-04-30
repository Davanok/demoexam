using System.Security.Cryptography;
using System.Text;
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

    private static byte[] GetHash(string inputString)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(inputString));
    }
    private static string GetHashString(string inputString)
    {
        var sb = new StringBuilder();
        foreach (var b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
    private void OnContinueButtonClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(UserNameField.Text))
        {
            MessageBox.Show("username text field cannot be empty");
            return;
        }
        if (string.IsNullOrEmpty(PasswordField.Text))
        {
            MessageBox.Show("password text field cannot be empty");
            return;
        }
        
        var user = _database.GetUser(UserNameField.Text);
        if (user == null) {
            MessageBox.Show("user with that username doesn't found");
            return;
        }
        if (!user.PasswordHash.Equals(GetHashString(PasswordField.Text), StringComparison.CurrentCultureIgnoreCase))
        {
            MessageBox.Show("password is incorrect");
            return;
        }
        User = user;
        Close();
    }
}