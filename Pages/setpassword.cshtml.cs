using CsvHelper;
using CsvHelper.Configuration;
using LuxeIQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LuxeIQ.ViewModels;
using LuxeIQ.Repositories;
using LuxeIQ.Extensions;
using System.Reflection.Metadata;
using LuxeIQ.Common;
using System;
using LuxeIQ.Services;

namespace LuxeIQ.Pages
{
    public class setpasswordModel : PageModel
    {
        private readonly ILogger<setpasswordModel> _logger;
        private readonly IUsersRepository _userRepository;
        private readonly IManufacturersTerritoryRepository _manufacturersTerritoryRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly IWholesalerShowroomRepository _wholesalerShowroomRepository;
        public setpasswordModel(ILogger<setpasswordModel> logger, IWholesalerShowroomRepository wholesalerShowroomRepository, IWholesalerRepository wholesalerRepository, IUsersRepository userRepository, IManufacturersTerritoryRepository manufacturersTerritoryRepository, IManufacturersRepository manufacturersRepository)
        {
            _userRepository = userRepository;
            _manufacturersTerritoryRepository = manufacturersTerritoryRepository;
            _manufacturersRepository = manufacturersRepository;
            _wholesalerRepository = wholesalerRepository;
            _wholesalerShowroomRepository = wholesalerShowroomRepository;
            _logger = logger;

        }


        [BindProperty]
        public string action { get; set; } = string.Empty;
        [BindProperty]
        public Boolean buttonStatus { get; set; } = false;
        [BindProperty]
        public Boolean usertypeDisable { get; set; } = false;

        [BindProperty]
        public UserViewModel user { get; set; } = default!;

        [BindProperty]
        public IList<Manufacturers> manufacturers { get; set; } = default!;
        [BindProperty]
        public IList<ManufacturerTerritories> manufacturersTerritory { get; set; } = default!;
        [BindProperty]
        public IList<Wholesalers> wholesalers { get; set; } = default!;
        [BindProperty]
        public IList<WholesalerShowrooms> wholesalershowroom { get; set; } = default!;

        [BindProperty]
        public IList<US_State> usState { get; set; } = default!;
        public async Task<IActionResult> OnGet(string id)
        {
           // if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
            //{
                UserViewModel luser = new UserViewModel();
                if (!string.IsNullOrEmpty(id))
                {
                    Int64 userId;
                    Int64.TryParse(Utilities.DecryptText(Utilities.ToBase64Decoding(id)), out userId);
                    if (userId > 0)
                    {
                        Users tuser = await _userRepository.Find(userId);
                        if (tuser != null)
                        {
                            luser = tuser.ToUserViewModel();
                        }
                    }
                    else
                    {
                        return RedirectToPage("./Index");
                    }
                }
                user = luser;
            //}
            //else
            //{
            //    return RedirectToPage("./Index");
            //}

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
               // if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    if (user != null)
                    {
                        ModelState.Remove("action");
                        ModelState.Remove("user.ManufacturerName");
                        ModelState.Remove("user.SalesRepAgencyName");
                    }
                    if (ModelState.IsValid)
                    {
                        Users _user = user.ToModel();
                        _user.userId = user.userId;
                        // _user.userId = Guid.NewGuid().ToString();
                        Users uu = _userRepository.FindByEmailId(user.email);

                        if (uu != null)
                        {
                            await _userRepository.UpdatePassword(_user);
                            if (_user.userType == "M")
                            {
                                return RedirectToPage("./Index");
                            }
                            else
                            {
                                return RedirectToPage("./thankyou");
                            }
                        }
                    }
                }
                //else
                //{
                //    return RedirectToPage("./Index");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
            return RedirectToPage("./Index");
        }

    }
}