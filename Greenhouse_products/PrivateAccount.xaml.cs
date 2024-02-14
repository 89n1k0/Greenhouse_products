using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;

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
        public byte[] _image = null;
        public PrivateAccount()
        {
            InitializeComponent();
            popup.IsOpen = false;
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

            if (isAdmin == false)
            {
                add.Visibility = Visibility.Collapsed;
            }
            else
            {
                add.Visibility = Visibility.Visible;
            }
            ListBasket.Items.Clear();

            var productsWithCounts = _context.Заказ
    .Join(
        _context.Статус,
        product => product.Статус,
        order => order.Номер,
        (product, order) => new BasketViewModel
        {
            Номер = product.Номер,
            Дата_создания = product.Дата_оформления,
            Сумма = (decimal)product.Сумма,
            Статус = order.Наименование,
            Пользователь = (int)product.Пользователь
        }).Where(x => x.Пользователь ==CurrentUser)
    .ToList();
            ListBasket.ItemsSource = productsWithCounts;
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                Пользователь пользователь = db.Пользователь.Where(x => x.Номер == CurrentUser).FirstOrDefault();
                email.Text = пользователь.Почта;
                pass.Text = пользователь.Пароль;

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

        private void save_Click(object sender, RoutedEventArgs e)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                if (email.Text != "" && pass.Text != "")
                {
                    Пользователь пользователь = db.Пользователь.Where(x => x.Номер == CurrentUser).FirstOrDefault();
                    пользователь.Почта = email.Text;
                    пользователь.Пароль = pass.Text;
                    db.SaveChanges();
                    MessageBox.Show("данные изменены");
                }
                else
                {
                    MessageBox.Show("Заполните все поля");
                }

            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var product = item.DataContext as BasketViewModel;
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
    }
}
