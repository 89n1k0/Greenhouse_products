using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
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
    /// Логика работы Корзины
    /// </summary>
    public partial class Basket : Window
    {
        public bool isLoggedIn = ((App)Application.Current).IsLoggedIn;
        public int CurrentUser = ((App)Application.Current).CurrentUser;
        public bool isAdmin = ((App)Application.Current).isAdmin;
        public Заказ заказ;
        public int id;
        private List<Продуция_заказ> _products = new List<Продуция_заказ>();
        public greenhouse_productsEntities db = new greenhouse_productsEntities();
        public Basket()
        {
            InitializeComponent();
            /// <summary>
            /// Доступность корзины
            /// </summary>
            if (CurrentUser != 0)
            {
                заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser).OrderByDescending(x => x.Дата_создания).FirstOrDefault();
                if (заказ.Статус != 1)
                {
                    basket.Visibility = Visibility.Collapsed;
                }
                else
                {
                    int продуция_Заказ = db.Продуция_заказ.Where(x => x.Заказ == заказ.Номер).Count();
                    count.Text = продуция_Заказ.ToString();
                    basket.Visibility = Visibility;
                }
            }
            else
            {
                basket.Visibility = Visibility.Collapsed;
            }
            /// <summary>
            /// Заполнение корзины выбранными товарами
            /// </summary>
            заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser).OrderByDescending(x => x.Дата_создания).FirstOrDefault();
            id = заказ.Номер;
            ListProducts.Items.Clear();
            _products = db.Продуция_заказ.Where(x => x.Заказ == заказ.Номер).ToList();
            ListProducts.ItemsSource = _products;

            /// <summary>
            /// Указание количества того или иного продукта
            /// </summary>
            card.IsEnabled = false;
            ListViewItem listViewItem = (ListViewItem)ListProducts.ItemContainerGenerator.ContainerFromIndex(107);
            TextBox counts = (TextBox)listViewItem.FindName("counts");
            counts.Text = all_count.ToString();
        }
        /// <summary>
        /// Удаление продукта из корзины
        /// </summary>
        private void drop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var product = item.Content as Продуция_заказ;
                int id = product.Номер;
                Продуция_заказ продуция_Заказ = db.Продуция_заказ.Where(x => x.Номер == id).FirstOrDefault();
                db.Продуция_заказ.Remove(продуция_Заказ);
                ListProducts.Items.Clear();
                _products = db.Продуция_заказ.Where(x => x.Заказ == заказ.Номер).ToList();
                ListProducts.ItemsSource = _products;

            }
        }
        /// <summary>
        /// Оформление заказа
        /// </summary>
        private void arrange_Click(object sender, RoutedEventArgs e)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                Заказ заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser).FirstOrDefault();
                if (заказ.Статус == 1)
                {
                    if (adress.Text != "")
                    {
                        if ((bool)cash.IsChecked)
                        {
                            заказ.Статус = 2;
                            заказ.Адрес = adress.Text;
                            заказ.Дата_создания = DateTime.Now;
                            db.SaveChanges();
                        }
                        else
                        {
                            MessageBox.Show("Выберите способ оплаты");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Заполните поле адреса");
                    }
                }
                else
                {
                    MessageBox.Show("Корзина пуста");
                }
            }
        }
        /// <summary>
        /// Уменьшение количества продукта
        /// </summary>
        private void minus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var product = item.Content as Продуция_заказ;
                var номер_продукции = product.Продукция;
                int количество = (int)db.Продуция_заказ.Where(x => x.Продукция == номер_продукции && x.Заказ == id).FirstOrDefault().Количество;
                количество -= количество;
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Увеличение количества продукта
        /// </summary>
        private void plus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var product = item.Content as Продуция_заказ;
                var номер_продукции = product.Продукция;
                int количество = (int)db.Продуция_заказ.Where(x => x.Продукция == номер_продукции && x.Заказ == id).FirstOrDefault().Количество;
                количество += количество;
                db.SaveChanges();
            }
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

        private void add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddEditDeleteProducts addEditDeleteProducts = new AddEditDeleteProducts();
            addEditDeleteProducts.Show();
            this.Hide();
        }
    }
}
