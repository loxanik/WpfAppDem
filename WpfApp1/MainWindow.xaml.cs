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
        public List<Good> GoodsFiltered { get; set; }
        public MainWindow(User? user)
        {
            InitializeComponent();

            CurrentUser = user;

            if (CurrentUser?.RoleId == 1)
            {
                EditButton.Visibility = Visibility.Visible;
                ButtonDelete.Visibility = Visibility.Visible;
                ButtonAdd.Visibility = Visibility.Visible;
            }

            if (CurrentUser?.RoleId < 3)
            {
                SearchTextBox.Visibility = Visibility.Visible;
                SearchButton.Visibility = Visibility.Visible;
                ResetSearchButton.Visibility = Visibility.Visible;
                SortComboBox.Visibility = Visibility.Visible;
            }

            _context = new();

            Goods = _context.Goods
                .Include(g => g.Supplier)
                .Include(g => g.Unit)
                .Include(g => g.Manufactorer)
                .Include(g => g.Category)
                .ToList();
            
            GoodsFiltered = Goods;

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

                    price.Text = $"{Math.Round(oldPrice, 2).ToString("N2")} руб.";
                    price.Foreground = Brushes.Red;
                    price.TextDecorations = TextDecorations.Strikethrough;

                    newPrice.Text = $"{good.Price.ToString("N2")} руб.";
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

            if (good.PhotoPath != null)
            {
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            GoodsFiltered = Goods.Where(g => g.Description.ToLower().Contains(searchText)).ToList();

            GoodsListView.ItemsSource = GoodsFiltered;

            DataContext = this;
        }

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GoodsListView.ItemsSource = Goods;
            GoodsFiltered = Goods;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;

            if (GoodsListView == null) return;
        

            if (combo.SelectedIndex == 1)
            {
                GoodsListView.ItemsSource = GoodsFiltered.OrderBy(g => g.Price);
            }
            else if (combo.SelectedIndex == 2)
            {
                GoodsListView.ItemsSource = GoodsFiltered.OrderByDescending(g => g.Price);
            } 
            else if (combo.SelectedIndex == 0)
            {
                GoodsListView.ItemsSource = Goods;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new WindowEdit(null, _context);
            editWindow.ShowDialog();
        }
    }
}