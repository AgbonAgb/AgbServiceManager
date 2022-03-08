using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceManager.Core.Interfaces;
using ServiceManager.Data.Entities;
using ServiceManager.Web.ViewModels;

namespace ServiceManager.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IGenRepo<Service,int> _genRepoService;
        private readonly ILogger<ServiceController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //  private readonly IMapper _mapper;
        public ServiceController(IGenRepo<Service, int> genRepo, ILogger<ServiceController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _genRepoService = genRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor; 
            //_mapper=mapper;
        }
        public async Task <IActionResult> ServiceMgt()
        { 
            //Service sv = new Service();
            var services= await _genRepoService.GetAll();
            //convert to viewmodel
           // var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
            return View(services);
            //return View(mapp);
        }

        //CreateService
        [HttpGet]
        public async Task<IActionResult> CreateService()
        {
            ServiceViewModel svm = new ServiceViewModel();
           

            //return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(platformItems));
            //return View(svm);// create partial View
            return PartialView("_CreateService", svm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateService([Bind()] ServiceViewModel svm)
        {
            
            if(ModelState.IsValid)
            {//var username = HttpContext.User.Identity.Name;
                svm.SetupBy = "Godwin Agbon";


                var services = await _genRepoService.GetAll();
                //convert to viewmodel
                // var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
                return View(services);
            }
            else
            {
                return PartialView("_CreateService", svm);
            }
            //return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(platformItems));
            //return View(svm);// create partial View
            
        }
    }
}
