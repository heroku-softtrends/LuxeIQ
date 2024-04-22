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
using Microsoft.AspNetCore.Http;

namespace LuxeIQ.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUsersRepository _userRepository;

        public IndexModel(ILogger<IndexModel> logger, IUsersRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        [BindProperty]
        public string username { get; set; } = default!;
        [BindProperty]
        public string password { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            HttpContext.Session.Remove("LUXEIQ_LOGIN_USER");
            HttpContext.Session.Clear();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    if (username == Constants.username && password == Constants.password)
                    {
                        if (username == "admin")
                        {
                            HttpContext.Session.SetString("LUXEIQ_LOGIN_USER", username);
                            Constants.LUXEIQ_LOGIN_USER = username;
                        }
                        return RedirectToPage("./manufacturers");
                    }
                    else
                    {
                        Users loginuser = _userRepository.Login(username, password);
                        if (loginuser != null)
                        {
                            HttpContext.Session.SetString("LUXEIQ_LOGIN_USER", username);
                            if (loginuser.userType == "M")
                            {
                                Constants.LUXEIQ_LOGIN_USER = username;
                                Constants.LUXEIQ_LOGIN_USER_TYPE = loginuser.userType;
                                Constants.LUXEIQ_LOGIN_USER_ID = loginuser.ManufacturerId;

                                HttpContext.Session.SetString("ManufacturerId", loginuser.ManufacturerId.ToString());
                            }
                            return RedirectToPage("./manufacturerterritories");
                        }
                        else
                        {
                            username = "";
                            password = "";
                            TempData["msg"] = "<script type=\"text/javascript\">alert('Invalid username or password');</script>";
                            //return RedirectToPage("./index");
                        }
                    }
                }
                else
                {
                    username = "";
                    password = "";
                    TempData["msg"] = "<script type=\"text/javascript\">alert('Please enter your username or password');</script>";
                    //return RedirectToPage("./index");
                }
            }
            catch (Exception ex)
            {

            }
            return Page();

        }
    }
}