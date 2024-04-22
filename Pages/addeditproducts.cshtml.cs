using Google.Apis.Bigquery.v2.Data;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.Services;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LuxeIQ.Pages
{
    public class addeditproductsModel : PageModel
    {
        private readonly ILogger<addeditproductsModel> _logger;
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IProductRepository _productRepository;
        public addeditproductsModel(ILogger<addeditproductsModel> logger, IProductRepository productRepository, IManufacturersRepository manufacturersRepository)
        {
            _manufacturersRepository = manufacturersRepository;
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<IActionResult> OnGet(string id, string type)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    action = type;
                    Products product = await _productRepository.FindByManufacturingId(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                    if (product != null)
                    {
                        if (!string.IsNullOrEmpty(product.productAttributes))
                        {
                            productattributes = JsonConvert.DeserializeObject<IList<ProductAttribute>>(product.productAttributes);
                        }
                        if (!string.IsNullOrEmpty(product.tableName))
                        {
                            products = ServiceRepository.GetRecordsByFilter(null, product.tableName, "products", 0, 0, id).FirstOrDefault();
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

            }
            return Page();

        }

        [BindProperty]
        public string action { get; set; } = string.Empty;
        public IDictionary<string, object> products { get; set; } = default!;

        public IList<ProductAttribute> productattributes { get; set; } = default!;


    }
}
