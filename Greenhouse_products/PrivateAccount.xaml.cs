﻿using System;
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
        public PrivateAccount()
        {
            InitializeComponent();
            if (account_image.Source == null)
            {
                Uri resourceUri = new Uri("./images/user.png", UriKind.Relative);
                ImageSource imageSource = new BitmapImage(resourceUri);
                account_image.Source = imageSource;
            }
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (popup.IsOpen == false)
                {
                    Point position = Mouse.GetPosition(this);
                    popup.HorizontalOffset = position.X;
                    popup.VerticalOffset = position.Y;
                    popup.IsOpen = true;
                }
                else
                {
                    popup.IsOpen = false;
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
    }
}
