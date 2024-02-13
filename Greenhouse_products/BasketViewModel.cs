using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greenhouse_products
{
    public class BasketViewModel
    {
        public int Номер { get; set; }
        public Nullable<System.DateTime> Дата_создания { get; set; }
        public decimal Сумма { get; set; }
        public string Статус { get; set; }
    }
}
