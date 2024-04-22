using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LuxeIQ.Pages
{
    public class manufactureradminModel : PageModel
    {
        private readonly ILogger<manufactureradminModel> _logger;
        private readonly IUsersRepository _userRepository;
        public manufactureradminModel(ILogger<manufactureradminModel> logger, IUsersRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;

        }
        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
            {
                users = await _userRepository.GetAllManufacturingAdmins();
                //users = users.Where(m => m.userType == "M").ToList();
                return Page();
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }

        public IActionResult OnPostDelete(Int64 id)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    if (id > 0)
                    {
                        _userRepository.Remove(id);
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
            return RedirectToPage("./users");

        }
        public IList<UserViewModel> users { get; set; } = default!;

    }
}
