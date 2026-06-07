using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для WindowAuth.xaml
    /// </summary>
    public partial class WindowAuth : Window
    {
        private readonly ShoesShopContext _context;

        public string Login { get; set; }
        public string Password { get; set; }
        public WindowAuth()
        {
            InitializeComponent();
            _context = new ShoesShopContext();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = _context.Users.Any(x => x.Login == Login && x.Password == Password);

            if (result)
            {
                var user = _context.Users.First(x => x.Login == Login);
                var mainWindow = new MainWindow(user);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
                
        }

        private void GuestLogin_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(null);
            mainWindow.Show();
            this.Close();
        }
    }
}
