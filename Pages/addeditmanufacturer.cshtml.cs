using Google.Apis.Bigquery.v2.Data;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.Services;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IdentityModel.Claims;

namespace LuxeIQ.Pages
{
    public class addeditmanufacturerModel : PageModel
    {
        private readonly ILogger<addeditmanufacturerModel> _logger;
        private readonly IManufacturersTerritoryRepository _manufacturersTerritoryRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly IWholesalerShowroomRepository _wholesalerShowroomRepository;
        public addeditmanufacturerModel(ILogger<addeditmanufacturerModel> logger, IWholesalerShowroomRepository wholesalerShowroomRepository, IWholesalerRepository wholesalerRepository, IManufacturersTerritoryRepository manufacturersTerritoryRepository, IManufacturersRepository manufacturersRepository)
        {
            _manufacturersTerritoryRepository = manufacturersTerritoryRepository;
            _manufacturersRepository = manufacturersRepository;
            _wholesalerRepository = wholesalerRepository;
            _wholesalerShowroomRepository = wholesalerShowroomRepository;
            _logger = logger;

        }
        public async Task<IActionResult> OnGet(Int64 id, string type)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {


                    if (type == "Add" || type == "Edit")
                    {
                        buttonStatus = true;
                    }
                    else
                    {
                        buttonStatus = false;
                    }
                    action = type;
                    Console.WriteLine(id);
                    if (id > 0)
                    {
                        manufacturer = await _manufacturersRepository.Find(id);
                    }
                }
                else
                {
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {

            }
            return Page();
        }

        [BindProperty]
        public Manufacturers manufacturer { get; set; } = default!;
        [BindProperty]
        public string action { get; set; } = string.Empty;
        [BindProperty]
        public Boolean buttonStatus { get; set; } = false;
        //public async Task<IActionResult> OnPostView(Int64 id, string type)
        //{
        //    try
        //    {
        //        if (type == "Edit")
        //        {
        //            buttonStatus = true;
        //        }
        //        else
        //        {
        //            buttonStatus = false;
        //        }
        //        action = type;
        //        Console.WriteLine(id);
        //        if (id > 0)
        //        {
        //            manufacturer = await _manufacturersRepository.Find(id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Page();
        //}
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    ModelState.Remove("action");
                    if (ModelState.IsValid)
                    {
                        _manufacturersRepository.Add(manufacturer);
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

            return RedirectToPage("./manufacturers");
        }

    }
}
