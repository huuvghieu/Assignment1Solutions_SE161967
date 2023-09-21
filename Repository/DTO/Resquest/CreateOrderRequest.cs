using Repository.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO.Resquest
{
    public class CreateOrderRequest
    {
        public int? MemberId { get; set; }

        public DateTime? RequiredDate { get; set; }
        public virtual ICollection<OrderDetailRequest> OrderDetails { get; set; }
    }

    public class OrderDetailRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
