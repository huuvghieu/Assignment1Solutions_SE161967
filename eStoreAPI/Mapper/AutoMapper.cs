using AutoMapper;
using BusinessObject.Models;
using Repository.DTO.Response;
using Repository.DTO.Resquest;

namespace eStoreAPI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            #region Category
            CreateMap<Category, CategoryResponse>().ReverseMap();
            #endregion

            #region Product
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            #endregion

            #region Member
            CreateMap<Member, MemberReponse>().ReverseMap();
            CreateMap<CreateMemberRequest, Member>();
            CreateMap<UpdateMemberRequest, Member>();
            #endregion

            #region Order
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<CreateOrderRequest, Order>();
            #endregion

            #region OrderDetail
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();
            CreateMap<OrderDetailRequest, OrderDetail>();
            #endregion
        }
    }
}
