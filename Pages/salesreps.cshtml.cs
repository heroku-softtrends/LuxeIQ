using CsvHelper;
using CsvHelper.Configuration;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LuxeIQ.Pages
{
    public class salesrepsModel : PageModel
    {
        private readonly ILogger<salesrepsModel> _logger;
        private readonly IUsersRepository _userRepository;
        public salesrepsModel(ILogger<salesrepsModel> logger, IUsersRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;

        }
        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
            {
                users = await _userRepository.GetAllSalesReps(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
            }
            else
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public IActionResult OnPostDelete(Int64 id)
        {
            try
            {
                if (id > 0)
                {
                    _userRepository.Remove(id);
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToPage("./users");

        }
        public IList<UserViewModel> users { get; set; } = default!;

    }
}
