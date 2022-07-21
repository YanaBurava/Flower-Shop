using System;
using System.Collections.Generic;

namespace Flower.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories  { get; set; }
    }
}
