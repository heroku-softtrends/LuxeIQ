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
    public class thankyouModel : PageModel
    {
        private readonly ILogger<thankyouModel> _logger;
        private readonly IUsersRepository _userRepository;
        private readonly IManufacturersTerritoryRepository _manufacturersTerritoryRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly IWholesalerShowroomRepository _wholesalerShowroomRepository;
        public thankyouModel(ILogger<thankyouModel> logger, IWholesalerShowroomRepository wholesalerShowroomRepository, IWholesalerRepository wholesalerRepository, IUsersRepository userRepository, IManufacturersTerritoryRepository manufacturersTerritoryRepository, IManufacturersRepository manufacturersRepository)
        {
            _userRepository = userRepository;
            _manufacturersTerritoryRepository = manufacturersTerritoryRepository;
            _manufacturersRepository = manufacturersRepository;
            _wholesalerRepository = wholesalerRepository;
            _wholesalerShowroomRepository = wholesalerShowroomRepository;
            _logger = logger;

        }


        public async Task<IActionResult> OnGet(string id)
        {

            return Page();

        }


    }
}