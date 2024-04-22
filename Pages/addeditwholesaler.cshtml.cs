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
    public class addeditwholesalerModel : PageModel
    {
        private readonly ILogger<addeditwholesalerModel> _logger;
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly IWholesalerShowroomRepository _wholesalerShowroomRepository;
        public addeditwholesalerModel(ILogger<addeditwholesalerModel> logger, IWholesalerShowroomRepository wholesalerShowroomRepository, IWholesalerRepository wholesalerRepository)
        {
            _wholesalerRepository = wholesalerRepository;
            _wholesalerShowroomRepository = wholesalerShowroomRepository;
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
        public Wholesalers wholesaler { get; set; } = default!;

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
                        wholesaler = await _wholesalerRepository.Find(id);
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
