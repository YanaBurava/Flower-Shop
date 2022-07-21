using System;
using System.Collections.Generic;

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
