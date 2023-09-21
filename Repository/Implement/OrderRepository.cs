using AutoMapper;
using BusinessObject.Models;
using DataAccess;
using Repository.DTO.Response;
using Repository.DTO.Resquest;
using Repository.Exceptions;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implement
{
    public class OrderRepository : IOrderRepository
    {
        private IMapper _mapper;

        public OrderRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> GetOrders()
        {
            try
            {
                var orders = await OrderDAO.Instance.GetOrders();
                List<OrderResponse> result = new List<OrderResponse> ();
                foreach (var order in orders)
                {
                    var orderDetail = _mapper.Map<List<OrderDetailResponse>>(order.OrderDetails);
                    var memberResult = _mapper.Map<Member, MemberReponse>(order.Member);
                    var orderResult = new OrderResponse()
                    {
                        OrderId = order.OrderId,
                        MemberId = order.MemberId,
                        Freight = order.Freight,
                        Member = memberResult,
                        OrderDate = order.OrderDate,
                        OrderDetails = orderDetail,
                        RequiredDate = order.RequiredDate,
                        ShippedDate = order.ShippedDate,
                    };
                    result.Add(orderResult);
                }
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get all products failed!", ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderResponse> GetOrderById(int id)
        {
            try
            {
                var order = OrderDAO.Instance.GetAll().Where(x => x.OrderId == id).SingleOrDefault();
                if(order == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found order with id", id.ToString());
                }

                var orderDetail = _mapper.Map<List<OrderDetailResponse>>(order.OrderDetails);
                var memberResult = _mapper.Map<Member, MemberReponse>(order.Member);
                var orderResult = new OrderResponse()
                {
                    OrderId = order.OrderId,
                    MemberId = order.MemberId,
                    Freight = order.Freight,
                    Member = memberResult,
                    OrderDate = order.OrderDate,
                    OrderDetails = orderDetail,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                };

                return orderResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {
            try
            {

                Order order = new Order();
                _mapper.Map<CreateOrderRequest, Order>(request, order);

                #region Order
                order.OrderDate = DateTime.Now;
                order.ShippedDate = DateTime.Now.AddDays(5);
                Random random = new Random();
                order.OrderId = random.Next(1000, 10000);
                order.Freight = random.Next(10000, 100000);
                //order.Member = MemberDAO.Instance.GetAll().Where(x => x.MemberId == order.MemberId).SingleOrDefault();
                //check id
                var checkOrder = OrderDAO.Instance.GetAll().Where(x => x.OrderId == order.OrderId)
                                                               .FirstOrDefault();
                while(checkOrder != null)
                {
                    order.OrderId = random.Next(1000, 10000);
                    checkOrder = OrderDAO.Instance.GetAll().Where(x => x.OrderId == order.OrderId)
                                                               .FirstOrDefault();
                }
                #endregion

                #region OrderDetail
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var orderDetailRequest in request.OrderDetails)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    _mapper.Map<OrderDetailRequest, OrderDetail>(orderDetailRequest, orderDetail);
                    orderDetail.OrderId = order.OrderId;

                    var product = ProductDAO.Instance.GetAll().Where(x => x.ProductId == orderDetail.ProductId).SingleOrDefault();
                    orderDetail.UnitPrice = product.UnitPrice;
                    orderDetails.Add(orderDetail);
                    //update quatity of product

                    product.UnitsInStock = product.UnitsInStock - orderDetail.Quantity;
                    await ProductDAO.Instance.UpdateProduct(product);
                    order.OrderDetails = orderDetails;
                }

                await OrderDAO.Instance.InsertOrder(order);

                var orderDetailResult = _mapper.Map<List<OrderDetailResponse>>(order.OrderDetails);
                var memberResult = _mapper.Map<Member, MemberReponse>(order.Member);

                var orderResult = new OrderResponse()
                {
                    OrderId = order.OrderId,
                    MemberId = order.MemberId,
                    Freight = order.Freight,
                    Member = memberResult,
                    OrderDate = order.OrderDate,
                    OrderDetails = orderDetailResult,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                };
                #endregion

                return orderResult;
            }
            catch (CrudException ex)
            {
                throw new CrudException(ex.StatusCode, ex.Message, ex?.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
