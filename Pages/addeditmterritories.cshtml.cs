using Google.Apis.Bigquery.v2.Data;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.Services;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LuxeIQ.Pages
{
    public class addeditmterritoriesModel : PageModel
    {
        private readonly ILogger<addeditmterritoriesModel> _logger;
        private readonly IManufacturersTerritoryRepository _manufacturersTerritoryRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        public addeditmterritoriesModel(ILogger<addeditmterritoriesModel> logger, IManufacturersTerritoryRepository manufacturersTerritoryRepository, IManufacturersRepository manufacturersRepository)
        {
            _manufacturersTerritoryRepository = manufacturersTerritoryRepository;
            _manufacturersRepository = manufacturersRepository;
            _logger = logger;

        }
        public async Task<IActionResult> OnGet(Int64 id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
            {
                return Page();
            }
            else
            {
                return RedirectToPage("./Index");
            }

        }

        [BindProperty]
        public ManufacturerTerritories mterritories { get; set; } = default!;

        [BindProperty]
        public IList<Manufacturers> manufacturers { get; set; } = default!;

        [BindProperty]
        public string action { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostView(Int64 id, string type)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    action = type;
                    Console.WriteLine(id);
                    if (id > 0)
                    {
                        manufacturers = await _manufacturersRepository.GetAll();
                        mterritories = await _manufacturersTerritoryRepository.Find(id);
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


    }
}
