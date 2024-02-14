using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;
using System.Data.Entity.Validation;

namespace Greenhouse_products
{
    /// <summary>
    /// Просмотр заказов и изменение статуса
    /// </summary>
    public partial class Change_order_status : Window
    {
        public bool isLoggedIn = ((App)Application.Current).IsLoggedIn;
        public int CurrentUser = ((App)Application.Current).CurrentUser;
        public bool isAdmin = ((App)Application.Current).isAdmin;
        public greenhouse_productsEntities db = new greenhouse_productsEntities();
        public int id;
        public Change_order_status()
        {
            InitializeComponent();
            save.IsEnabled = false;
            ListViewLoad();
            if (CurrentUser != 0)
            {
                Заказ заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser && x.Дата_оформления == null).FirstOrDefault();
                if (заказ != null)
                {
                    if (заказ.Статус != 1)
                    {
                        basket.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        int продуция_Заказ = db.Продуция_заказ.Where(x => x.Заказ == заказ.Номер).Count();
                        counts.Text = продуция_Заказ.ToString();
                        basket.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    basket.Visibility = Visibility.Collapsed;
                }
            }
            foreach (var d in db.Статус)
            {
                status.Items.Add(d.Наименование);
            }
        }
        public void ListViewLoad()
        {
            var productsWithCounts = db.Заказ
    .Join(
        db.Статус,
        product => product.Статус,
        order => order.Номер,
        (product, order) => new OrderViewModel
        {
            Номер = (int)product.Номер,
            Адрес = product.Адрес,
            Сумма = (decimal)product.Сумма,
            Дата_оформления = (DateTime)product.Дата_оформления,
            Наименование_статуса = order.Наименование
        })
    .ToList();
            ListOrders.ItemsSource = productsWithCounts;
        }
        private void add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEditDeleteProducts addEditDeleteProducts = new AddEditDeleteProducts();
            addEditDeleteProducts.Show();
            this.Hide();
        }

        private void about_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void fruits_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Fruits fruits = new Fruits();
            fruits.Show();
            this.Hide();
        }

        private void vegetables_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vegetables vegetables = new Vegetables();
            vegetables.Show();
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

        private void get_Click(object sender, RoutedEventArgs e)
        {
            save.IsEnabled = true;
            if (ListOrders.SelectedIndex >= 0)
            {
                using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                {
                    var item = ListOrders.SelectedItem as OrderViewModel;
                    id = item.Номер;
                    string name_status = item.Наименование_статуса;
                    Статус статус = db.Статус.Where(x => x.Наименование == name_status).FirstOrDefault();
                    status.SelectedIndex = (int)статус.Номер - 1;
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни один элемент");
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                if (status.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    Заказ заказ = db.Заказ.FirstOrDefault(x => x.Номер == id);
                    заказ.Статус = status.SelectedIndex + 1;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                MessageBox.Show("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                }
                ListViewLoad();
                save.IsEnabled = false;
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
