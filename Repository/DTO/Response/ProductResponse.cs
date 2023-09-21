﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO.Response
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string? ProductName { get; set; }
        public string? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        
    }
}
