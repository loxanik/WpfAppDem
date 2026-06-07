using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ShoesShopContext _context;

        public Good? SelectedGood { get; set; }
        public User? CurrentUser { get; set; }
        public List<Good> Goods { get; set; }
        public MainWindow(User? user)
        {
            InitializeComponent();

            CurrentUser = user;

            if (CurrentUser?.RoleId == 1)
            {
                EditButton.Visibility = Visibility.Visible;
                ButtonDelete.Visibility = Visibility.Visible;
            }

            _context = new();

            Goods = _context.Goods
                .Include(g => g.Supplier)
                .Include(g => g.Unit)
                .Include(g => g.Manufactorer)
                .Include(g => g.Category)
                .ToList();

            DataContext = this;
        }

        private void CardBorder_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = sender as Border;

            var good = border.DataContext as Good;

            if (good != null)
            {

                if (good.Discount > 0)
                {
                    Run newPrice = border.FindName("NewPrice") as Run;
                    Run price = border.FindName("Price") as Run;

                    var oldPrice = Convert.ToDouble(good.Price);
                    var currentPrice = Convert.ToDouble(good.Price);

                    oldPrice = currentPrice / (1 - (Convert.ToDouble(good.Discount) / 100));

                    price.Text = Math.Round(oldPrice, 2).ToString("N2");
                    price.Foreground = Brushes.Red;
                    price.TextDecorations = System.Windows.TextDecorations.Strikethrough;

                    newPrice.Text = good.Price.ToString("N2");
                }

                var converter = new BrushConverter();

                if (good.Discount >= 15)
                    border.Background = (Brush)converter.ConvertFromString("#2E8B57");
                else
                    border.Background = Brushes.LightGray;

                if (good.Amount == 0)
                    border.Background = Brushes.AliceBlue;
            }

            Image imgEl = border.FindName("GoodImage") as Image;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, good.PhotoPath);


            if (imgEl != null)
            {
                if (File.Exists(fullPath))
                {
                    imgEl.Source = new BitmapImage(new Uri(fullPath, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    string defaultImg = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "picture.png");
                    imgEl.Source = new BitmapImage(new Uri(defaultImg, UriKind.RelativeOrAbsolute));

                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGood == null)
                MessageBox.Show("Выберите товар!", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);

            var editWindow = new WindowEdit(SelectedGood, _context);
            editWindow.ShowDialog();
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = null;
            var auth = new WindowAuth();
            auth.Show();
            this.Close();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGood == null)
                return;

            var result = MessageBox.Show("Вы действительно хотите удалить товар?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _context.Goods.Remove(SelectedGood);
                _context.SaveChanges();
            }
        }
    }
}