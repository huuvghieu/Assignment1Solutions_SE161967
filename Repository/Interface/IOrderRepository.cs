using Repository.DTO.Response;
using Repository.DTO.Resquest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<OrderResponse>> GetOrders();

        public Task<OrderResponse> GetOrderById(int id);

        public Task<OrderResponse> CreateOrder(CreateOrderRequest request);
    }
}
