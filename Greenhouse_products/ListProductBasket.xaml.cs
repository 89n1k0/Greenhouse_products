using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Greenhouse_products
{
    /// <summary>
    /// Список продуктов заказа
    /// </summary>
    public partial class ListProductBasket : Window
    {
        public greenhouse_productsEntities db = new greenhouse_productsEntities();
        private List<Продуция_заказ> _basket = new List<Продуция_заказ>();
        public ListProductBasket(int idBasket)
        {
            InitializeComponent();
            ListProducts.Items.Clear();

            var productsWithCounts = db.Продукция
    .Join(
        db.Продуция_заказ,
        product => product.Номер,
        order => order.Продукция,
        (product, order) => new ProductViewModel
        {
            Номер = (int)order.Заказ,
            Наименование = product.Наименование,
            Изображение = product.Изображение,
            Сумма = (decimal)order.Сумма,
        }).Where(item => item.Номер == idBasket)
    .ToList();
            ListProducts.ItemsSource = productsWithCounts;
            Заказ заказ = db.Заказ.Where(x => x.Номер == idBasket).FirstOrDefault();
            number.Content = idBasket.ToString();
            status.Content = db.Статус.Where(x => x.Номер == заказ.Статус).FirstOrDefault().Наименование.ToString() ;
            sum.Content = заказ.Сумма.ToString();
        }
    }
}
