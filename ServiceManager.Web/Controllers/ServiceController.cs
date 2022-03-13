using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceManager.Core.Interfaces;
using ServiceManager.Data.Entities;
using ServiceManager.Infrastructure.Models;
using ServiceManager.Web.ViewModels;
using static ServiceManager.Web.Controllers.Common.Enum;
using System.Web;

namespace ServiceManager.Web.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IGenRepo<Service, int> _genRepoService;
        private readonly ILogger<ServiceController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _Configuration;
        // private string[] permittedExtensions = { ".jpg", ".png" };      
        private string[] permittedExtensions = { ".xls", ".xlsx"};

        public ServiceController(IGenRepo<Service, int> genRepo, ILogger<ServiceController> logger,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IWebHostEnvironment webHostEnvironment,
            IConfiguration _Configuration)
        {
            _genRepoService = genRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            this._Configuration=_Configuration;
        }
        public async Task<IActionResult> ServiceMgt()
        {
        //    var services = (IEnumerable<Service>)null;
        //    var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
            //Service sv = new Service();
            if (TempData["SearchService"] != null || TempData["DisabledServices"] != null)
            {
                if(TempData["SearchService"] != null)
                {
                    var nservices = JsonConvert.DeserializeObject<IEnumerable<ServiceViewModel>>((string)TempData["SearchService"]);
                    //ViewData["SearchService"] = JsonConvert.DeserializeObject<IEnumerable<ServiceViewModel>>((string)TempData["SearchService"]);

                    // var nservices = ViewData["SearchService"] as ServiceManager.Web.ViewModels.ServiceViewModel;// BiodataTest.Models.Skills;
                    TempData["SearchService"] = null;
                    return View(nservices);

                }
                else
                {
                    var nservices = JsonConvert.DeserializeObject<IEnumerable<ServiceViewModel>>((string)TempData["DisabledServices"]);
                    //ViewData["SearchService"] = JsonConvert.DeserializeObject<IEnumerable<ServiceViewModel>>((string)TempData["SearchService"]);

                    // var nservices = ViewData["SearchService"] as ServiceManager.Web.ViewModels.ServiceViewModel;// BiodataTest.Models.Skills;
                    TempData["SearchService"] = null;
                    return View(nservices);
                }
                
               
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
                    return PartialView("_CreateService", svm);
                }
                else
                {
                    //logg error 
                    //display error message
                    Alert("Something went wrong, service not created", NotificationType.error);
                    return PartialView("_CreateService", svm);
                    //return View("_CreateService", svm);
                    
                }


            }
            else
            {
                return PartialView("_CreateService", svm);
                //return View("_CreateService", svm);
                //return RedirectToPage("_CreateService", svm);
            }
            //return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(platformItems));
            //return View(svm);// create partial View

        }
        //UploadExcelService

        //EditService
     [HttpGet]
        public async Task<IActionResult> UploadExcelService()
        {

            ExcelUploadViewModel eXc = new ExcelUploadViewModel();
            
            return PartialView("_UploadExcel", eXc);
        }
        [HttpPost]
        public async Task<IActionResult> UploadExcelService(ExcelUploadViewModel CVM, IFormCollection collection, IFormFile file)
        {
            //upload to directory
            //send file path to queue for upoad
            //Queue should listen and upload
            //ExcelData

            // CVM.CVfile =
            //
            var file2 = collection.Files[0];

            if (CVM.CVfile == null)
            {
                return PartialView("_UploadExcel", CVM);
            }
            long size = CVM.CVfile.Length;//.Sum(f => f.Length);



            var ext = Path.GetExtension(CVM.CVfile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return PartialView("_UploadExcel", CVM);
            }
            string uniqueFileName = await UploadedFile(CVM);
            string conStringExcel = this._Configuration.GetConnectionString("ExcelConString");
            //Send path and file name to Q for backgroud processing
            //BackgroundJob.Enqueue<_genRepoService>(x => x.Send());
            // CreateMap<ExcelUploadViewModel, ExcelUploadModel>().ReverseMap();
            var mapp = _mapper.Map<ExcelUploadModel>(CVM);
            try
            {//JsonConvert.SerializeObject(mapp)

                BackgroundJob.Enqueue(() => _genRepoService.UploadExcell(uniqueFileName, ext));
                Alert("Upload is going on at background, kindly refresh page to see progress", NotificationType.success);
            }
            catch (Exception ex)
            {

                
            }

            return RedirectToAction("ServiceMgt");
        }

        private async Task<string> UploadedFile(ExcelUploadViewModel model)
        {
            string uniqueFileName = null;
            string filePath=null;

            if (model.CVfile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ExcelData");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CVfile.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    // model.CVfile.CopyTo(fileStream);
                    await model.CVfile.CopyToAsync(fileStream);
                }
            }
           // return uniqueFileName;
            return filePath;
        }
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
                    //return PartialView("_CreateService", svm);
                    return Redirect("/");
                    //return RedirectToAction("ServiceMgt", services);
                    //return RedirectToActionPermanent("_CreateService", svm);
                }


            }
            else
            {

                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(x => x.ErrorMessage)).ToList();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("Error: ", error);
                }
                return PartialView("_CreateService", svm);
                //return RedirectToActionPermanent("_CreateService", svm);
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
        //ViewDisabledServices
        public async Task<IActionResult> ViewDisabledServices()
        {
            var services = await _genRepoService.GetAllDisabled();
            var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
            TempData["DisabledServices"] = JsonConvert.SerializeObject(mapp);
            // return View(services);
            return RedirectToAction("ServiceMgt");


            //convert to viewmodel

        }
        //ExportServices
        //public void Task<IActionResult> ExportServices()
        //{
        //    //var services = await _genRepoService.GetAllDisabled();
        //    //var mapp = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
        //    //TempData["DisabledServices"] = JsonConvert.SerializeObject(mapp);
        //    //// return View(services);
        //    //return RedirectToAction("ServiceMgt");


        //    //convert to viewmodel

        //    DataTable dt = new DataTable("Grid");
        //    dt.Columns.AddRange(new DataColumn[4] { new DataColumn("CustomerId"),
        //                                new DataColumn("ContactName"),
        //                                new DataColumn("City"),
        //                                new DataColumn("Country") });

        //    var customers = from customer in this.Context.Customers.Take(10)
        //                    select customer;

        //    foreach (var customer in customers)
        //    {
        //        dt.Rows.Add(customer.CustomerID, customer.ContactName, customer.City, customer.Country);
        //    }

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
        //        }
        //    }

        //}
    }
}
