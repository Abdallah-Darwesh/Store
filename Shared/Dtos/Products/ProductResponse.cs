﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Products
{
    public class ProductResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public string Brand { get; set; }

        public string Type { get; set; }

    }
}
