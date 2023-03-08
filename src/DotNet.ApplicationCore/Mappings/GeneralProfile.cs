using AutoMapper;
using DotNet.ApplicationCore.DTOs;
using DotNet.ApplicationCore.Entities;

namespace DotNet.ApplicationCore.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            //CreateMap<CreateProductRequest, Product>()
            //    .ForMember(dest =>
            //        dest.ProductId,
            //        opt => opt.Ignore()
            //    )                
            //    .ForMember(dest =>
            //        dest.CreatedAt,
            //        opt => opt.Ignore()
            //    )
            //    .ForMember(dest =>
            //        dest.UpdatedAt,
            //        opt => opt.Ignore()
            //    );

           // CreateMap<VMDivision, Divisions>();
        }
    }
}