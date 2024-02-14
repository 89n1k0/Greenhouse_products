using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greenhouse_products
{
    public class OrderViewModel
    {
        public int Номер { get; set; }
        public string Адрес { get; set; }
        public decimal Сумма { get; set; }
        public DateTime Дата_оформления { get; set; }
        public string Наименование_статуса { get; set; }
    }
}
