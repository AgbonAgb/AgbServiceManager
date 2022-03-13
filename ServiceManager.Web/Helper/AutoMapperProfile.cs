using AutoMapper;
using ServiceManager.Data.Entities;
using ServiceManager.Infrastructure.Models;
using ServiceManager.Web.ViewModels;

namespace ServiceManager.Web.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Source and Destination
            CreateMap<Service, ServiceViewModel>().ReverseMap();
            CreateMap<ExcelUploadViewModel, ExcelUploadModel>().ReverseMap();
        }
    }
}
