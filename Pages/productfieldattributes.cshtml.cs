using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using LuxeIQ.Common;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.Services;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data.Common;
using System.Text;

namespace LuxeIQ.Pages
{
    public class productfieldattributesModel : PageModel
    {
        private readonly ILogger<productfieldattributesModel> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        public productfieldattributesModel(ILogger<productfieldattributesModel> logger, IProductRepository productRepository, IManufacturersRepository manufacturersRepository)
        {
            _productRepository = productRepository;
            _manufacturersRepository = manufacturersRepository;
            _logger = logger;
        }

        public Products product { get; set; } = default!;

        [BindProperty]
        public IList<ProductAttribute> productattributes { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    product = await _productRepository.FindByManufacturingId(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                    if (product != null)
                    {
                        productattributes = !string.IsNullOrEmpty(product.productAttributes) ? JsonConvert.DeserializeObject<List<ProductAttribute>>(product.productAttributes) : null;
                    }
                    return Page();
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
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    if (productattributes != null)
                    {
                        Products lproduct = await _productRepository.FindByManufacturingId(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                        if (lproduct != null)
                        {
                            string productAttributes = JsonConvert.SerializeObject(productattributes);
                            await _productRepository.UpdateProductAttribute(productAttributes, lproduct.productId);
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
            return Page();
        }

        public void OnPostGetAjax(string id)
        {
            Console.WriteLine("Hello " + id);
        }
        public IActionResult OnPostUpdateAttributes()
        {
            try
            {
                return new JsonResult(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                return new JsonResult(null);
            }


        }
        public IActionResult OnGetUpdateAttributes(string attributes)
        {
            try
            {
                if (!string.IsNullOrEmpty(attributes))
                {
                    //_manufacturersRepository.Update

                    return new JsonResult(null);
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