using AutoMapper;
using ran_product_management_net.Database.Mongodb.Models;
using ran_product_management_net.Database.Postgresql.Models;
using ran_product_management_net.Models.DTOs.Request;
using ran_product_management_net.Models.DTOs.Response;
using ran_product_management_net.Models.Integration;

namespace ran_product_management_net.Models.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateProductCategoryReq, ProductCategory>();
        CreateMap<ProductCategory, ProductCategoryResp>();
            // .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd")));
        CreateMap<UpdateProductCategoryReq, UpdateProductCategoryReq>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<ListProductDto, Product>();
        CreateMap<GetProductDto, Product>();
        CreateMap<CreateProductReq, ProductInventory>();
        CreateMap<CreateProductReq, ProductDetail>();
        CreateMap<ProductInventory, ProductInventoryResp>();
        CreateMap<ProductDetail, ProductDetailResp>();
    }
}