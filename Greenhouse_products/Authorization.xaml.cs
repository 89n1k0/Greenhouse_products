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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public bool isLoggedIn = ((App)Application.Current).IsLoggedIn;
        public int CurrentUser = ((App)Application.Current).CurrentUser;
        public bool isAdmin = ((App)Application.Current).isAdmin;
        public Authorization()
        {
            InitializeComponent();
        }

        private void reg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (email.Text != null && pass.Password != null)
            {
                using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                {
                    Пользователь пользователь = db.Пользователь.Where(x => x.Почта == email.Text && x.Пароль == pass.Password).First();
                    if (пользователь != null)
                    {
                        CurrentUser = пользователь.Номер;
                        isLoggedIn = true;
                        if (пользователь.Роль == 1)
                        {
                            isAdmin = true;
                        }
                        else
                        {
                            isAdmin = false;
                        }
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Данный пользователь отсутствует");
                    }
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
    }
}
