//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Greenhouse_products
{
    using System;
    using System.Collections.Generic;
    
    public partial class Продукция
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Продукция()
        {
            this.Продуция_заказ = new HashSet<Продуция_заказ>();
        }
    
        public int Номер { get; set; }
        public byte[] Изображение { get; set; }
        public string Наименование { get; set; }
        public int Вес { get; set; }
        public decimal Цена { get; set; }
        public Nullable<int> Общее_количество_склад { get; set; }
        public Nullable<int> Каталог { get; set; }
    
        public virtual Каталог Каталог1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Продуция_заказ> Продуция_заказ { get; set; }
    }
}
