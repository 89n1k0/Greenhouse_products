using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Greenhouse_products
{
    /// <summary>
    /// Логика взаимодействия для Vegetables.xaml
    /// </summary>
    public partial class Vegetables : Window
    {
        public bool isLoggedIn = ((App)Application.Current).IsLoggedIn;
        public int CurrentUser = ((App)Application.Current).CurrentUser;
        public bool isAdmin = ((App)Application.Current).isAdmin;
        public decimal sum = ((App)Application.Current).sum;
        public int id;

        private greenhouse_productsEntities _context = new greenhouse_productsEntities();
        private List<Каталог> _category = new List<Каталог>();
        private List<Продукция> _products = new List<Продукция>();
        public ListView ListCateg;
        public ListView ListProduct;
        public Vegetables()
        {
            if (isAdmin == false)
            {
                add.Visibility = Visibility.Collapsed;
            }

            InitializeComponent();
            ListProducts.Items.Clear();
            _products = _context.Продукция.Where(x => x.Каталог < 13).ToList();
            ListProducts.ItemsSource = _products;

            ListCatalog.Items.Clear();
            _category = _context.Каталог.Where(x => x.Номер_продукции == 1).ToList();
            ListCatalog.ItemsSource = _category;
            ListCateg = ListCatalog;

            if (account_image.Source == null)
            {
                Uri resourceUri = new Uri("./images/user.png", UriKind.Relative);
                ImageSource imageSource = new BitmapImage(resourceUri);
                account_image.Source = imageSource;
            }
        }

        private void vegetables_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vegetables vegetables = new Vegetables();
            vegetables.Show();
            this.Hide();
        }

        private void fruits_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Fruits fruits = new Fruits();
            fruits.Show();
            this.Hide();
        }

        private void about_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void out_Click(object sender, RoutedEventArgs e)
        {
            isLoggedIn = false;
            CurrentUser = 0;
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Hide();
        }

        private void private_acc_Click(object sender, RoutedEventArgs e)
        {
            if (isLoggedIn)
            {
                PrivateAccount privateAccount = new PrivateAccount();
                privateAccount.Show();
                this.Hide();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы не авторизованы", "Авторизоваться", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Authorization authorization = new Authorization();
                    authorization.Show();
                    this.Hide();
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var product = item.Content as Каталог;
                int id = product.Номер;
                ListProducts.Items.Clear();
                _products = _context.Продукция.Where(x => x.Каталог == id).ToList();
                ListProducts.ItemsSource = _products;

            }
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var product = item.Content as Продукция;
                id = product.Номер;
                decimal price = product.Цена;
                sum += price;

            }
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {

                Заказ заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser).FirstOrDefault();
                if (заказ.Статус == 1)
                {
                    Продуция_заказ продуция_Заказ = new Продуция_заказ();
                    продуция_Заказ.Заказ = заказ.Номер;
                    продуция_Заказ.Продукция = id;
                    db.Продуция_заказ.Add(продуция_Заказ);
                    db.SaveChanges();
                }
                else
                {
                    заказ = new Заказ();
                    заказ.Пользователь = CurrentUser;
                    заказ.Статус = 1;
                    db.Заказ.Add(заказ);
                    db.SaveChanges();
                }
            }
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            string searchText = find.Text;
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                var query = from data in db.Продукция
                            where data.Наименование.Contains(searchText)
                            select data;
                ListProduct.ItemsSource = query.ToList();
            }
        }

        private void reverse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _products = _context.Продукция.Where(x => x.Каталог > 13).OrderBy(item => item.Наименование).ToList();
            ListProducts.ItemsSource = _products;
        }

        private void sort_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _products = _context.Продукция.Where(x => x.Каталог > 13).OrderByDescending(item => item.Наименование).ToList();
            ListProducts.ItemsSource = _products;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            decimal sliderValue = (decimal)slider.Value;
            _products = _context.Продукция.Where(x => x.Каталог > 13 && x.Цена == sliderValue).ToList();
            ListProducts.ItemsSource = _products;
        }

        private void add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
