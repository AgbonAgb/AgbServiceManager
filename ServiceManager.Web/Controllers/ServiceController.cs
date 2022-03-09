using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceManager.Core.Interfaces;
using ServiceManager.Data.Entities;
using ServiceManager.Web.ViewModels;
using static ServiceManager.Web.Controllers.Common.Enum;

namespace ServiceManager.Web.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IGenRepo<Service, int> _genRepoService;
        private readonly ILogger<ServiceController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ServiceController(IGenRepo<Service, int> genRepo, ILogger<ServiceController> logger,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _genRepoService = genRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<IActionResult> ServiceMgt()
        {
        //    var services = (IEnumerable<Service>)null;
        //    var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
            //Service sv = new Service();
            if (TempData["SearchService"] != null)
            {
                var nservices= JsonConvert.DeserializeObject<IEnumerable<ServiceViewModel>>((string)TempData["SearchService"]);
                //ViewData["SearchService"] = JsonConvert.DeserializeObject<IEnumerable<ServiceViewModel>>((string)TempData["SearchService"]);

              // var nservices = ViewData["SearchService"] as ServiceManager.Web.ViewModels.ServiceViewModel;// BiodataTest.Models.Skills;
                TempData["SearchService"] = null;
                return View(nservices);
            }
            else
            {
                var services = await _genRepoService.GetAll();
                var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
                // return View(services);
                return View(mapp);
            }


            
            //convert to viewmodel
            
        }

        //CreateService
        [HttpGet]
        public async Task<IActionResult> CreateService()
        {
            ServiceViewModel svm = new ServiceViewModel();

           
               // TempData["SearchService"] = JsonConvert.SerializeObject(mapp);

            //return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(platformItems));
            //return View(svm);// create partial View
            return PartialView("_CreateService", svm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateService([FromForm] ServiceViewModel svm)
        {
            TempData["SearchService"] = null;
            if (ModelState.IsValid)
            {//var username = HttpContext.User.Identity.Name;

                svm.SetupBy = "Godwin Agbon";
                svm.ContactStaff = "Godwin Agbon";

                var mapp = _mapper.Map<Service>(svm);

                bool rtn = await _genRepoService.Create(mapp);
                if (rtn)
                {
                    Alert("Service Created successfully", NotificationType.success);
                    var services = await _genRepoService.GetAll();
                    //display success messgae

                    //convert to viewmodel
                    // var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
                    return RedirectToAction("ServiceMgt", services);
                }
                else
                {
                    //logg error 
                    //display error message
                    Alert("Something went wrong, service not created", NotificationType.error);
                    return PartialView("_CreateService", svm);
                }


            }
            else
            {
                return PartialView("_CreateService", svm);
            }
            //return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(platformItems));
            //return View(svm);// create partial View

        }

        //EditService
        [HttpGet]
        public async Task<IActionResult> EditService(int Id)
        {
            TempData["SearchService"] = null;
            ServiceViewModel svm = new ServiceViewModel();
            var itm = await _genRepoService.GetById(Id);
            var mapp = _mapper.Map<ServiceViewModel>(itm);
            //return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(platformItems));
            //return View(svm);// create partial View
            return PartialView("_EditService", mapp);
        }

        [HttpPost]
        public async Task<IActionResult> EditService([FromForm] ServiceViewModel svm)
        {
            TempData["SearchService"] = null;

            if (ModelState.IsValid)
            {//
                var username = HttpContext.User.Identity.Name;
                svm.SetupBy = "Godwin Oliha";
                svm.ContactStaff = "Godwin Oliha";

                var mapp = _mapper.Map<Service>(svm);

                bool rtn = await _genRepoService.Update(mapp);
                if (rtn)
                {
                    Alert("Service Updated successfully", NotificationType.success);
                    var services = await _genRepoService.GetAll();
                    //display success messgae

                    //convert to viewmodel
                    // var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
                    return RedirectToAction("ServiceMgt", services);
                }
                else
                {
                    //logg error 
                    //display error message
                    Alert("Something went wrong, service not created", NotificationType.error);
                    return PartialView("_CreateService", svm);
                }


            }
            else
            {
                return PartialView("_CreateService", svm);
            }
            //return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(platformItems));
            //return View(svm);// create partial View

        }
        //ServiceSearch
        [HttpGet]
        public async Task<IActionResult> ServiceSearch(string SearchString)
        {
            var services = (IEnumerable<Service>)null;
            var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);

            if (!string.IsNullOrEmpty(SearchString))
            {


                services = await _genRepoService.SearchItem(SearchString);
             


                mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);

                TempData["SearchService"] = JsonConvert.SerializeObject(mapp);
            }
            else
            {
                TempData["SearchService"] = null;
            }
            //return RedirectToAction("ServiceMgt", mapp);
            return RedirectToAction("ServiceMgt");


        }
    }
}
