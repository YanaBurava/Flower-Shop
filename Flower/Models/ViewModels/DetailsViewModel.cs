using System;
using System.Collections.Generic;

namespace Flower.Models.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Product = new Product();
        }

        public Product Product { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
