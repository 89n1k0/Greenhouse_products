using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        public bool isLoggedIn = ((App)Application.Current).IsLoggedIn;
        public int CurrentUser = ((App)Application.Current).CurrentUser;
        public bool isAdmin = ((App)Application.Current).isAdmin;
        public int all_count = 1;
        public greenhouse_productsEntities db = new greenhouse_productsEntities();

        public Basket()
        {
            InitializeComponent();
            card.IsEnabled = false;

            ListViewItem listViewItem = (ListViewItem)ListProducts.ItemContainerGenerator.ContainerFromIndex(107);
            TextBox counts = (TextBox)listViewItem.FindName("counts");
            counts.Text = all_count.ToString();
        }

        private void drop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void arrange_Click(object sender, RoutedEventArgs e)
        {

        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            if (all_count == 1)
            {
                MessageBox.Show("Данное количество минимально");
            }
            else
            {
                all_count--;
                count.Text = all_count.ToString();
                total_sum.Content = Convert.ToInt32(изделия.Цена) * all_count;
            }
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            if (all_count == изделия.Количество)
            {
                MessageBox.Show("Данное количество максимально");
            }
            else
            {
                all_count++;
                count.Text = all_count.ToString();
                total_sum.Content = Convert.ToInt32(изделия.Цена) * all_count;
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
