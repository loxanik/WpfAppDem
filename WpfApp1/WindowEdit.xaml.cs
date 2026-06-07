using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : Window
    {
        private readonly ShoesShopContext _context;

        public Good CurrentGood { get; set; }
        public List<Unit> Units { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Manufactorer> Manufactorers { get; set; }
        public List<Category> Categories { get; set; }
        public WindowEdit(Good? selectedGood, ShoesShopContext context)
        {
            InitializeComponent();
            _context = context;

            Units = _context.Units.ToList();
            Suppliers = _context.Suppliers.ToList();
            Manufactorers = _context.Manufactorers.ToList();
            Categories = _context.Categories.ToList();

            CurrentGood = selectedGood ?? new Good();

            DataContext = this;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentGood.Id == 0)
            {
                _context.Goods.Add(CurrentGood);
            }

            _context.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
