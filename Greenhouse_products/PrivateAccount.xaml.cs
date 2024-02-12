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
    /// Логика взаимодействия для PrivateAccount.xaml
    /// </summary>
    public partial class PrivateAccount : Window
    {
        public bool isLoggedIn = ((App)Application.Current).IsLoggedIn;
        public int CurrentUser = ((App)Application.Current).CurrentUser;
        public bool isAdmin = ((App)Application.Current).isAdmin;
        private greenhouse_productsEntities _context = new greenhouse_productsEntities();
        private List<Заказ> _basket = new List<Заказ>();
        public PrivateAccount()
        {
            InitializeComponent();
            if (CurrentUser != 0)
            {
                Заказ заказ = _context.Заказ.Where(x => x.Пользователь == CurrentUser).OrderByDescending(x => x.Дата_создания).FirstOrDefault();
                if (заказ.Статус != 1)
                {
                    basket.Visibility = Visibility.Collapsed;
                }
                else
                {
                    int продуция_Заказ = _context.Продуция_заказ.Where(x => x.Заказ == заказ.Номер).Count();
                    count.Text = продуция_Заказ.ToString();
                    basket.Visibility = Visibility;
                }
            }
            else
            {
                basket.Visibility = Visibility.Collapsed;
            }

            if (isAdmin == false)
            {
                add.Visibility = Visibility.Collapsed;
            }
            ListBasket.Items.Clear();

            _basket = _context.Заказ.Where(x => x.Пользователь == CurrentUser).ToList();
            ListBasket.ItemsSource = _basket;
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                Пользователь пользователь = db.Пользователь.Where(x => x.Номер == CurrentUser).FirstOrDefault();
                email.Text = пользователь.Почта;
                pass.Text = пользователь.Пароль;

            }
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

        private void save_Click(object sender, RoutedEventArgs e)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                Пользователь пользователь = db.Пользователь.Where(x => x.Номер == CurrentUser).FirstOrDefault();
                пользователь.Почта = email.Text;
                пользователь.Пароль = pass.Text;
                db.SaveChanges();

            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var product = item.Content as Заказ;
                int id = product.Номер;
                ListProductBasket listProductBasket = new ListProductBasket(id);
                listProductBasket.Show();

            }
        }

        private void add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEditDeleteProducts addEditDeleteProducts = new AddEditDeleteProducts();
            addEditDeleteProducts.Show();
            this.Hide();
        }
    }
}
