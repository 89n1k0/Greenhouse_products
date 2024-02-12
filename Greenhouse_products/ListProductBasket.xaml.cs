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
    /// Логика взаимодействия для ListProductBasket.xaml
    /// </summary>
    public partial class ListProductBasket : Window
    {
        public greenhouse_productsEntities db;
        private List<Продуция_заказ> _basket = new List<Продуция_заказ>();
        public ListProductBasket(int idBasket)
        {
            InitializeComponent();
            ListProducts.Items.Clear();

            _basket = db.Продуция_заказ.Where(x => x.Заказ == idBasket).ToList();
            ListProducts.ItemsSource = _basket;
            Заказ заказ = db.Заказ.Where(x => x.Номер == idBasket).FirstOrDefault();
            number.Content = idBasket;
            status.Content = db.Статус.Where(x => x.Номер == заказ.Статус).FirstOrDefault();
        }
    }
}
