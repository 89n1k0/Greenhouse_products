using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Greenhouse_products
{
    /// <summary>
    /// Авторизация
    /// </summary>
    public partial class Authorization : Window
    {
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
            if (email.Text != "" && pass.Password != "")
            {
                using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                {
                    Пользователь пользователь = db.Пользователь.Where(x => x.Почта == email.Text && x.Пароль == pass.Password).FirstOrDefault();
                    if (пользователь != null)
                    {
                        ((App)Application.Current).CurrentUser = пользователь.Номер;
                        ((App)Application.Current).IsLoggedIn = true;
                        if (пользователь.Роль == 2)
                        {
                            ((App)Application.Current).isAdmin = true;
                        }
                        else
                        {
                            ((App)Application.Current).isAdmin = false;
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
