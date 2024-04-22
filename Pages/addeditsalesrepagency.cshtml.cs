using LuxeIQ.Models;
using LuxeIQ.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LuxeIQ.Pages
{
    public class addeditsalesrepagencyModel : PageModel
    {
        private readonly ILogger<addeditsalesrepagencyModel> _logger;
        private readonly ISalesRepAgencyRepository _salesRepAgencyRepository;
        private readonly IManufacturersTerritoryRepository _manufacturersTerritoryRepository;
        public addeditsalesrepagencyModel(ILogger<addeditsalesrepagencyModel> logger, ISalesRepAgencyRepository salesRepAgencyRepository, IManufacturersTerritoryRepository manufacturersTerritoryRepository)
        {
            _salesRepAgencyRepository = salesRepAgencyRepository;
            _manufacturersTerritoryRepository = manufacturersTerritoryRepository;
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
        public SalesRepAgency salesRepAgency { get; set; } = default!;

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
                        salesRepAgency = await _salesRepAgencyRepository.Find(id);
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