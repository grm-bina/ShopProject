﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductCreateViewModel
    {
        public Product Product {get;set;}
        public PicturesViewModel Files { get; set;  }
    }
}
