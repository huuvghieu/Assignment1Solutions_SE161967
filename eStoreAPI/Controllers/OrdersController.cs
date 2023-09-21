using Microsoft.AspNetCore.Mvc;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using Repository.Interface;

namespace eStoreAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        
        public OrdersController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
        {
            var rs = await _orderRepo.GetOrders();
            return Ok(rs);
        }

        [HttpGet("id")]
        public async Task<ActionResult<OrderResponse>> GetOrderById([FromQuery] int id)
        {
            var rs = await _orderRepo.GetOrderById(id);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var rs = await _orderRepo.CreateOrder(request);
            return Ok(rs);
        }
    }
}
