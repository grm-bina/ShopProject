using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Models
{
    public class ShoppingCartViewModel
    {
        private int _discount;
        public ShoppingCartViewModel(int discount)
        {
            _discount = discount;
        }

        public IEnumerable<Product> Products { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Total Price")]
        public decimal? TotalPrice
        {
            get
            {
                if (Products != null)
                {
                    decimal? temp = Products.Sum(i => i.Price);
                    if (temp != null)
                        temp -= temp * _discount / 100;
                    return temp;

                }
                else
                    return null;
            }
        }
    }
}
