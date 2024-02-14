using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Greenhouse_products
{
    /// <summary>
    /// Овощи
    /// </summary>
    public partial class Vegetables : Window
    {
        public bool isLoggedIn = ((App)Application.Current).IsLoggedIn;
        public int CurrentUser = ((App)Application.Current).CurrentUser;
        public bool isAdmin = ((App)Application.Current).isAdmin;
        public int id;

        private greenhouse_productsEntities _context = new greenhouse_productsEntities();
        private List<Каталог> _category = new List<Каталог>();
        private List<Продукция> _products = new List<Продукция>();
        public ListView ListCateg;
        public ListView ListProduct;
        public Vegetables()
        {
            InitializeComponent();
            popup.IsOpen = false;
            if (isAdmin == false)
            {
                order.Visibility = Visibility.Collapsed;
                add.Visibility = Visibility.Collapsed;
            }
            else
            {
                order.Visibility = Visibility.Visible;
                add.Visibility = Visibility.Visible;
            }
            EllipseBasketCount();

            ListProducts.Items.Clear();
            _products = _context.Продукция.Where(x => x.Каталог < 13).ToList();
            ListProducts.ItemsSource = _products;

            ListCatalog.Items.Clear();
            _category = _context.Каталог.Where(x => x.Номер_продукции == 1).ToList();
            ListCatalog.ItemsSource = _category;
            ListCateg = ListCatalog;
        }

        public void EllipseBasketCount()
        {
            basket.Visibility = Visibility.Collapsed;
            if (CurrentUser != 0)
            {
                Заказ заказ = _context.Заказ.Where(x => x.Пользователь == CurrentUser && x.Дата_оформления == null).FirstOrDefault();
                if (заказ != null)
                {
                    if (заказ.Статус != 1)
                    {
                        basket.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        int продуция_Заказ = _context.Продуция_заказ.Where(x => x.Заказ == заказ.Номер).Count();
                        count.Text = продуция_Заказ.ToString();
                        basket.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    basket.Visibility = Visibility.Collapsed;
                }
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
            ((App)Application.Current).IsLoggedIn = false;
            ((App)Application.Current).isAdmin = false;
            ((App)Application.Current).CurrentUser = 0;
            popup.IsOpen = false;
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Hide();
        }

        private void private_acc_Click(object sender, RoutedEventArgs e)
        {
            if (isLoggedIn)
            {
                popup.IsOpen = false;
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
                _products = _context.Продукция.Where(x => x.Каталог == id).ToList();
                ListProducts.ItemsSource = _products;

            }
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            if (isLoggedIn)
            {
                var button = sender as Button;
                if (button != null)
                {
                    var product = button.DataContext as Продукция;
                    if (product != null)
                    {
                        id = product.Номер;


                        using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                        {

                            Заказ заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser && x.Дата_оформления == null).FirstOrDefault();
                            if (заказ !=null)
                            {
                                if (заказ.Статус == 1)
                                {
                                    Продуция_заказ продуция_Заказ = new Продуция_заказ();
                                    продуция_Заказ.Заказ = заказ.Номер;
                                    продуция_Заказ.Продукция = id;
                                    продуция_Заказ.Количество = 1;
                                    продуция_Заказ.Сумма = db.Продукция.Where(x => x.Номер == id).FirstOrDefault().Цена;
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

                                    Продуция_заказ продуция_Заказ = new Продуция_заказ();
                                    продуция_Заказ.Заказ = заказ.Номер;
                                    продуция_Заказ.Продукция = id;
                                    продуция_Заказ.Количество = 1;
                                    продуция_Заказ.Сумма = db.Продукция.Where(x => x.Номер == id).FirstOrDefault().Цена;
                                    db.Продуция_заказ.Add(продуция_Заказ);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                заказ = new Заказ();
                                заказ.Пользователь = CurrentUser;
                                заказ.Статус = 1;
                                db.Заказ.Add(заказ);
                                db.SaveChanges();

                                Продуция_заказ продуция_Заказ = new Продуция_заказ();
                                продуция_Заказ.Заказ = заказ.Номер;
                                продуция_Заказ.Продукция = id;
                                продуция_Заказ.Количество = 1;
                                продуция_Заказ.Сумма = db.Продукция.Where(x => x.Номер == id).FirstOrDefault().Цена;
                                db.Продуция_заказ.Add(продуция_Заказ);
                                db.SaveChanges();
                            }
                            EllipseBasketCount();
                        }
                    }
                }
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

        private void find_Click(object sender, RoutedEventArgs e)
        {
            string searchText = find.Text;
            if (searchText != "")
            {
                using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                {
                    var query = from data in db.Продукция
                                where data.Наименование.Contains(searchText) && data.Каталог < 13
                                select data;
                    ListProducts.ItemsSource = query.ToList();
                }
            }
            else
            {
                using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                {
                    var query = from data in db.Продукция
                                where data.Каталог < 13
                                select data;
                    ListProducts.ItemsSource = query.ToList();
                }
            }
        }

        private void add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEditDeleteProducts addEditDeleteProducts = new AddEditDeleteProducts();
            addEditDeleteProducts.Show();
            this.Hide();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedValue;
            if (selectedItem == reverse)
            {
                _products = _context.Продукция.Where(x => x.Каталог < 13).OrderByDescending(item => item.Наименование).ToList();
                ListProducts.ItemsSource = _products;
            }
            else if (selectedItem == sort)
            {
                _products = _context.Продукция.Where(x => x.Каталог < 13).OrderBy(item => item.Наименование).ToList();
                ListProducts.ItemsSource = _products;
            }
        }
        private void basket_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isLoggedIn)
            {
                if (basket.IsVisible)
                {
                    Basket basket = new Basket();
                    basket.Show();
                    this.Hide();
                }
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

        private void order_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Change_order_status status = new Change_order_status();
            status.Show();
            this.Hide();
        }
    }
}
