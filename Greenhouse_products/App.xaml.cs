using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Greenhouse_products
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public int CurrentUser = 0;
        public bool IsLoggedIn = false;
        public bool isAdmin = false;
    }
}
