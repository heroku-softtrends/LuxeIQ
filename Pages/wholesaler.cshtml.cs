using CsvHelper;
using CsvHelper.Configuration;
using LuxeIQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LuxeIQ.ViewModels;
using LuxeIQ.Repositories;
using LuxeIQ.Extensions;
using System.Reflection;

namespace LuxeIQ.Pages
{
    public class wholesalerModel : PageModel
    {
        private readonly ILogger<wholesalerModel> _logger;
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly IManufacturersRepository _manufacturersRepository;

        public wholesalerModel(ILogger<wholesalerModel> logger, IWholesalerRepository wholesalerRepository, IManufacturersRepository manufacturersRepository)
        {
            _wholesalerRepository = wholesalerRepository;
            _manufacturersRepository = manufacturersRepository;
            _logger = logger;
        }
        public IList<Manufacturers> manufacturers { get; set; } = default!;
        public IList<Wholesalers> wholesaler { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                   // manufacturers = await _manufacturersRepository.GetAll();
                    wholesaler = await _wholesalerRepository.GetAllbyManufacturer(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
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

        public async Task<IActionResult> OnPostImport(IFormFile fileInput)
        {
            Console.WriteLine("calling import");
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    if (fileInput == null || fileInput.Length == 0)
                    {
                        TempData["msg"] = "<script type=\"text/javascript\">alert('File is not selected','Error');</script>";
                    }
                    else if (fileInput != null && !string.IsNullOrEmpty(fileInput.FileName.Trim()))
                    {
                        if (fileInput.FileName.Remove(0, fileInput.FileName.LastIndexOf(".") + 1) != "csv")
                        {
                            TempData["msg"] = "<script type=\"text/javascript\">alert('Invalid csv file for Session upload','Error');</script>";
                        }
                        else
                        {
                            List<TableColumns> ltableColumns = new List<TableColumns>();
                           
                            List<WholesalerImport> lWholesaler = new List<WholesalerImport>();
                            using (var reader = new StreamReader(fileInput.OpenReadStream()))
                            {
                                var csvConfig = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
                                csvConfig.IgnoreReferences = true;
                                csvConfig.DetectColumnCountChanges = true;
                                csvConfig.IgnoreBlankLines = true;
                                using (var csv = new CsvReader(reader, csvConfig))
                                {
                                    lWholesaler = csv.GetRecords<WholesalerImport>().ToList();
                                }

                                TableColumns lcol = new TableColumns();
                                lcol.name = "manufacturerId";
                                lcol.fieldType = "bigint";
                                lcol.isRequired = true;
                                lcol.defaultValue = HttpContext.Session.GetString("ManufacturerId");
                                ltableColumns.Add(lcol);

                                WholesalerImport wsi = new WholesalerImport();
                                foreach (var field in wsi.GetType().GetProperties())
                                {
                                    lcol = new TableColumns();
                                    lcol.name = field.Name;
                                    lcol.fieldType = "text";
                                    ltableColumns.Add(lcol);
                                }
                            }
                             
                            if (lWholesaler != null && lWholesaler.Count > 0)
                            {
                                var i = 0;
                                do
                                {
                                    var pwholesaler = lWholesaler.AsEnumerable().Skip(i).Take(LuxeIQ.Common.Constants.maxLoopCountForImport);
                                    await Task.Run(async () =>
                                    {
                                        IList<Dictionary<string, object>> ldic = new List<Dictionary<string, object>>();
                                        ldic = pwholesaler.Select(x => x.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(x, null))).ToList();

                                        await ServiceRepository.BulkInsert(ldic, ltableColumns, "public", "Wholesalers");

                                        i = i + LuxeIQ.Common.Constants.maxLoopCountForImport;
                                    });
                                } while (i < lWholesaler.Count);
                            }
                            else
                            {
                                TempData["msg"] = "<script type=\"text/javascript\">alert('No wholesaler records available in the import format','Error');</script>";
                            }
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script type=\"text/javascript\">alert('Please attach a file to import','Error');</script>";
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
                TempData["msg"] = "<script type=\"text/javascript\">alert('Invalid import file.Please check the header string.');</script>";
            }

            return RedirectToPage("./wholesaler");
        }
    }
}
