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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void autho_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Hide();
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            if (email.Text != null && pass.Password != null)
            {
                using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                {
                    Пользователь пользователь = db.Пользователь.Where(x => x.Почта == email.Text && x.Пароль == pass.Password).FirstOrDefault();
                    if (пользователь != null)
                    {
                        MessageBox.Show("Данный пользователь уже существует");
                    }
                    else
                    {
                        if (email.Text.Contains("@") && email.Text.Contains("."))
                        {
                            if (pass.Password.Count() > 6) {
                                пользователь = new Пользователь();
                                пользователь.Почта = email.Text;
                                пользователь.Пароль = pass.Password;
                                пользователь.Роль = 1;
                                db.Пользователь.Add(пользователь);
                                db.SaveChanges();
                                Authorization authorization = new Authorization();
                                authorization.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Пароль должен содержать более 6 символов");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Электронная почта не прошла проверку");
                        }
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
