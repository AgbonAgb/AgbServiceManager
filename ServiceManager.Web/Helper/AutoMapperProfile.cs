using AutoMapper;
using ServiceManager.Data.Entities;
using ServiceManager.Web.ViewModels;

namespace ServiceManager.Web.Helper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Service, ServiceViewModel>().ReverseMap();
        }
    }
}
