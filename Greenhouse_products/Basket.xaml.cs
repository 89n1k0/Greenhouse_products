﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
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
            EllipseBasketCount();
            ListViewLoad();
            popup.IsOpen = false;
            if (isAdmin == false)
            {
                add.Visibility = Visibility.Collapsed;
            }
            else
            {
                add.Visibility = Visibility.Visible;
            }

            card.IsEnabled = false;
        }


        public void EllipseBasketCount()
        {
            basket.Visibility = Visibility.Collapsed;
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

        /// <summary>
        /// Заполнение корзины выбранными товарами
        /// </summary>
        public void ListViewLoad()
        {
            заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser && x.Дата_оформления == null).FirstOrDefault();
            id = заказ.Номер;
            total_sum.Content = db.Заказ.Where(x => x.Номер == id).FirstOrDefault().Сумма.ToString();

            var productsWithCounts = db.Продукция
    .Join(
        db.Продуция_заказ,
        product => product.Номер,
        order => order.Продукция,
        (product, order) => new ProductViewModel
        {
            Номер = order.Номер,
            Наименование = product.Наименование,
            Изображение = product.Изображение,
            Сумма = (decimal)order.Сумма,
            Количество = (int)order.Количество,
            Продукция = (int)order.Продукция,
            Заказ = (int)order.Заказ
        }).Where(x => x.Заказ == id)
    .ToList();
            ListProducts.ItemsSource = productsWithCounts;
        }
        /// <summary>
        /// Оформление заказа
        /// </summary>
        private void arrange_Click(object sender, RoutedEventArgs e)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                Заказ заказ = db.Заказ.Where(x => x.Пользователь == CurrentUser && x.Дата_оформления == null).FirstOrDefault();
                if (заказ.Статус == 1)
                {
                    if (adress.Text != "")
                    {
                        if ((bool)cash.IsChecked)
                        {
                            заказ.Статус = 2;
                            заказ.Адрес = adress.Text;
                            заказ.Дата_оформления = DateTime.Now;
                            db.SaveChanges();
                            MessageBox.Show("Заказ оформлен");
                            PrivateAccount privateAccount = new PrivateAccount();
                            privateAccount.Show();
                            this.Hide();
                            EllipseBasketCount();
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

        private void toggleButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button != null)
            {
                var product = button.DataContext as ProductViewModel;
                if (product != null)
                {
                    int id = product.Номер;
                    Продуция_заказ продуция_Заказ = db.Продуция_заказ.Where(x => x.Номер == id).FirstOrDefault();
                    Заказ заказ =db.Заказ.Where(x => x.Номер == id).FirstOrDefault();
                    заказ.Сумма -= db.Продукция.Where(x => x.Номер == продуция_Заказ.Продукция).FirstOrDefault().Цена;
                    total_sum.Content = заказ.Сумма.ToString();
                    db.Продуция_заказ.Remove(продуция_Заказ);
                    db.SaveChanges();
                    ListViewLoad();
                }
            }
        }
    }
}
