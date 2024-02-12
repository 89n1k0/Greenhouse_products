using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace Greenhouse_products
{
    /// <summary>
    /// Логика взаимодействия для AddEditDeleteProducts.xaml
    /// </summary>
    public partial class AddEditDeleteProducts : Window
    {
        public greenhouse_productsEntities db;
        private byte[] _image = null;
        public int id;
        public AddEditDeleteProducts()
        {
            InitializeComponent();
            save.IsEnabled = false;
            foreach (var d in db.Каталог)
            {
                Categ.Items.Add(d.Наименование);
            }
        }
        public void ListViewLoad()
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                var categories = db.Продукция.ToList();

                Categ.ItemsSource = categories;
            }
        }
        private void ChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string path;
            if ((bool)openFileDialog.ShowDialog())
            {
                path = openFileDialog.FileName;
                _image = System.IO.File.ReadAllBytes(path);
            }
            MemoryStream ms = new MemoryStream(_image);
            image.Source = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                if (name.Text == null || price.Text == null || weidth.Text == null || count.Text == null || catalog.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    Продукция продукция = new Продукция();
                    продукция.Изображение = _image;
                    продукция.Наименование = name.Text;
                    продукция.Цена = Convert.ToDecimal(price.Text);
                    продукция.Вес = Convert.ToInt32(weidth.Text);
                    продукция.Каталог = catalog.SelectedIndex;
                    db.Продукция.Add(продукция);
                    db.SaveChanges();
                }
                ListViewLoad();
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            save.IsEnabled = true;
            if (Categ.SelectedIndex >= 0)
            {
                using (greenhouse_productsEntities db = new greenhouse_productsEntities())
                {
                    var item = Categ.SelectedItem as Продукция;
                    id = item.Номер;
                    Продукция продукция = db.Продукция.Where(x => x.Номер == id).FirstOrDefault();
                    MemoryStream ms = new MemoryStream(продукция.Изображение);
                    image.Source = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    name.Text = продукция.Наименование;
                    price.Text = продукция.Цена.ToString();
                    weidth.Text = продукция.Вес.ToString();
                    catalog.SelectedIndex = (int)продукция.Каталог;
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни один элемент");
            }
            ListViewLoad();
        }
        public static void RemoveProduct(int id)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                Продукция meal = db.Продукция.Where(x => x.Номер == id).FirstOrDefault();
                db.Продукция.Remove(meal);
                db.SaveChanges();
            }
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (Categ.SelectedIndex >= 0)
            {
                var result = MessageBox.Show("Вы точно хотите удалить этот товар?", "Удалить", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var item = Categ.SelectedItem as Продукция;
                    int id = item.Номер;
                    RemoveProduct(id);
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни один элемент");
            }
            ListViewLoad();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            using (greenhouse_productsEntities db = new greenhouse_productsEntities())
            {
                if (name.Text == null || price.Text == null || weidth.Text == null || count.Text == null || catalog.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля");
                }
                else
                {
                    Продукция продукция = db.Продукция.Where(x => x.Номер == id).FirstOrDefault();
                    продукция.Изображение = _image;
                    продукция.Наименование = name.Text;
                    продукция.Цена = Convert.ToDecimal(price.Text);
                    продукция.Вес = Convert.ToInt32(weidth.Text);
                    продукция.Каталог = catalog.SelectedIndex;
                    db.SaveChanges();
                }
                ListViewLoad();
                save.IsEnabled = false;
            }
        }
    }
}
