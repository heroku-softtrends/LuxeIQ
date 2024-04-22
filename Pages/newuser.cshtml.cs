using EnumsNET;
using Google.Apis.Bigquery.v2.Data;
using LuxeIQ.Common;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.Services;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using YamlDotNet.Core.Tokens;

namespace LuxeIQ.Pages
{
    public class newuserModel : PageModel
    {
        private readonly ILogger<newuserModel> _logger;
        private readonly IUsersRepository _userRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly ISalesRepAgencyRepository _salesRepAgencyRepository;
        public newuserModel(ILogger<newuserModel> logger, ISalesRepAgencyRepository salesRepAgencyRepository, IWholesalerRepository wholesalerRepository, IUsersRepository userRepository, IManufacturersRepository manufacturersRepository)
        {
            _userRepository = userRepository;
            _manufacturersRepository = manufacturersRepository;
            _wholesalerRepository = wholesalerRepository;
            _salesRepAgencyRepository = salesRepAgencyRepository;
            _logger = logger;

        }
        public async Task<IActionResult> OnGet(Int64 id, string type = "", string userType = "")
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
            {

                UserViewModel luser = new UserViewModel();
                luser.country = "USA";
                usState = Utilities.GetUS_States();
                if (id > 0)
                {
                    Users tuser = await _userRepository.Find(id);
                    luser = tuser.ToUserViewModel();
                    if (luser.userType == "M")
                    {
                        usertypeName = ((UserType)UserType.M).AsString(EnumFormat.Description);
                        manufacturers = await _manufacturersRepository.GetAll();
                    }
                    else if (luser.userType == "W")
                    {
                        wholesalers = await _wholesalerRepository.GetAll();
                    }
                    else if (luser.userType == "SA")// && luser.ManufacturerId.HasValue && luser.ManufacturerId.Value > 0)
                    {
                        usertypeName = ((UserType)UserType.SA).AsString(EnumFormat.Description);
                        salesRepAgencies = await _salesRepAgencyRepository.GetAll();
                    }
                    else if (luser.userType == "SH" && luser.WholesalerId.HasValue && luser.WholesalerId.Value > 0)
                    {
                        wholesalers = await _wholesalerRepository.GetAll();
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                    {
                        return RedirectToPage("./Index");
                    }
                    else if (userType == "M")
                    {
                        usertypeName = ((UserType)UserType.M).AsString(EnumFormat.Description);
                        luser.userType = "M";
                        manufacturers = await _manufacturersRepository.GetAll();
                    }
                    else if (userType == "SA")
                    {
                        usertypeName = ((UserType)UserType.SA).AsString(EnumFormat.Description);
                        salesRepAgencies = await _salesRepAgencyRepository.GetAll();
                        luser.userType = "SA";

                    }
                }
                if (type == "Add" || type == "Edit")
                {
                    buttonStatus = true;
                }
                else
                {
                    buttonStatus = false;
                }
                HttpContext.Session.SetString("ACTION_TYPE", type);
                action = type;
                user = luser;
                return Page();
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }

        [BindProperty]
        public string usertypeName { get; set; } = string.Empty;

        [BindProperty]
        public string action { get; set; } = string.Empty;
        [BindProperty]
        public Boolean buttonStatus { get; set; } = false;

        [BindProperty]
        public UserViewModel user { get; set; } = default!;

        [BindProperty]
        public IList<Manufacturers> manufacturers { get; set; } = default!;
        [BindProperty]
        public IList<SalesRepAgency> salesRepAgencies { get; set; } = default!;
        [BindProperty]
        public IList<ManufacturerTerritories> manufacturersTerritory { get; set; } = default!;
        [BindProperty]
        public IList<Wholesalers> wholesalers { get; set; } = default!;
        [BindProperty]
        public IList<WholesalerShowrooms> wholesalershowroom { get; set; } = default!;

        [BindProperty]
        public IList<US_State> usState { get; set; } = default!;

        public ActionResult OnEdit(int id)
        {
            Console.WriteLine(id);
            return null;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    ModelState.Remove("usertypeName");
                    ModelState.Remove("action");
                    ModelState.Remove("user.password");
                    ModelState.Remove("user.confirmpassword");
                    ModelState.Remove("user.ManufacturerName");
                    ModelState.Remove("user.SalesRepAgencyName");
                    if (ModelState.IsValid)
                    {
                        Users _user = user.ToModel();
                        Users uu = _userRepository.FindByEmailId(user.email);
                        if (uu == null)
                        {
                            if (_user.userType == "SA")
                            {
                                _user.ManufacturerId = Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId"));
                                salesRepAgencies = await _salesRepAgencyRepository.GetAll();
                            }
                            _user.activated = "false";
                            _userRepository.Add(_user);
                            EmailServices.SendAlertForUserCreateMessage(_user.name, _user.email, _user.userId);
                        }
                        else if (uu.userId > 0 && HttpContext.Session.GetString("ACTION_TYPE").ToString() == "Edit")
                        {
                            if (_user.userType == "SA")
                            {
                                _user.ManufacturerId = uu.ManufacturerId;
                            }
                            _user.userId = uu.userId;
                            _user.activated = uu.activated;
                            _userRepository.Add(_user);
                        }
                        else
                        {
                            usState = Utilities.GetUS_States();
                            if (_user.userType == "M")
                            {
                                manufacturers = await _manufacturersRepository.GetAll();
                            }
                            else if (_user.userType == "SA")
                            {
                                salesRepAgencies = await _salesRepAgencyRepository.GetAll();
                            }
                            else if (_user.userType == "W")
                            {
                                wholesalers = await _wholesalerRepository.GetAll();
                            }
                            TempData["msg"] = "<script type=\"text/javascript\">alert('This user already exist','Error');</script>";
                            return Page();
                        }
                    }
                }
                else
                {
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
            if (HttpContext.Session.GetString("LUXEIQ_LOGIN_USER") == "admin")
            {
                return RedirectToPage("./manufactureradmin");
            }
            if (Constants.LUXEIQ_LOGIN_USER_TYPE == "M")
            {
                return RedirectToPage("./salesreps");
            }
            return RedirectToPage("./users");
        }


        public async Task<IActionResult> OnGetManufacturerTerritory(Int64 manufacturerId)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    if (manufacturerId > 0)
                    {
                        // var lmanufacturerterritory=await _manufacturersTerritoryRepository.GetAllByManufacturerId(manufacturerId);
                        return new JsonResult(null);
                    }
                    else
                    {
                        return new JsonResult(null);
                    }
                }
                else
                {
                    return new JsonResult(null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                return new JsonResult(null);
            }
        }
        public async Task<IActionResult> OnGetShowroomLocation(Int64 wholesalerId)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {

                    if (wholesalerId > 0)
                    {
                        //  var lwholesalerShowroom = await _wholesalerShowroomRepository.GetAllByWholesalerId(wholesalerId);
                        return new JsonResult(null);

                    }
                    else
                    {
                        return new JsonResult(null);
                    }
                }
                else
                {
                    return new JsonResult(null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                return new JsonResult(null);
            }

        }
        public async Task<IActionResult> OnGetUserType(string userType)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {

                    if (!string.IsNullOrEmpty(userType))
                    {
                        if (userType == "M" || userType == "SA")
                        {
                            var lmanufacturers = await _manufacturersRepository.GetAll();
                            return new JsonResult(lmanufacturers);
                        }
                        else if (userType == "W" || userType == "SH")
                        {
                            var lwholesaler = await _wholesalerRepository.GetAll();
                            return new JsonResult(lwholesaler);
                        }
                        else
                        {
                            return new JsonResult(null);
                        }
                    }
                    else
                    {
                        return new JsonResult(null);
                    }
                }
                else
                {
                    return new JsonResult(null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                return new JsonResult(null);
            }
        }
    }
}
