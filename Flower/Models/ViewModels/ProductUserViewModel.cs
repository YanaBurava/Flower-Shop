using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flower.Models.ViewModels
{
    public class ProductUserViewModel
    {
        public ProductUserViewModel()
        {
            ProductList=new List<Product>();
        }
        public AppUser ApplicationUser { get; set; }
        public IList<Product> ProductList { get; set; }
    }
}
