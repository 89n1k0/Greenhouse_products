using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greenhouse_products
{
    public class ProductViewModel
    {
        public int Номер { get; set; }
        public string Наименование { get; set; }
        public byte[] Изображение { get; set; }
        public decimal Сумма { get; set; }
        public int Количество { get; set; }
        public int Продукция { get; set; }
    }
}
