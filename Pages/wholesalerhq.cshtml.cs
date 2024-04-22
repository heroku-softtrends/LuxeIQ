using CsvHelper;
using CsvHelper.Configuration;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace LuxeIQ.Pages
{
    public class wholesalerhqModel : PageModel
    {
        private readonly ILogger<wholesalerhqModel> _logger;
        private readonly IWholesalerHQRepository _wholesalerHQRepository;
        private readonly IWholesalerRepository _wholesalerRepository;
        public wholesalerhqModel(ILogger<wholesalerhqModel> logger, IWholesalerHQRepository wholesalerHQRepository, IWholesalerRepository wholesalerRepository)
        {
            _wholesalerHQRepository = wholesalerHQRepository;
            _wholesalerRepository = wholesalerRepository;
            _logger = logger;
        }
        public IList<Wholesalers> wholesaler { get; set; } = default!;
        public IList<WholesalerHQ> wholesalerHQ { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    wholesaler = await _wholesalerRepository.GetAll();
                    wholesalerHQ = await _wholesalerHQRepository.GetAllWholesalerHQsByManufactuerId(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
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
        public async Task<IActionResult> OnPostImport(IFormFile fileInput, int ddlWholesaler = 0)
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
                            List<WholesalerHQImport> lWholesalerHQImport = new List<WholesalerHQImport>();
                            using (var reader = new StreamReader(fileInput.OpenReadStream()))
                            {
                                var csvConfig = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
                                csvConfig.IgnoreReferences = true;
                                csvConfig.DetectColumnCountChanges = true;
                                csvConfig.IgnoreBlankLines = true;
                                using (var csv = new CsvReader(reader, csvConfig))
                                {
                                    lWholesalerHQImport = csv.GetRecords<WholesalerHQImport>().ToList();
                                }

                                TableColumns lcol = new TableColumns();
                                lcol.name = "wholesalerId";
                                lcol.fieldType = "bigint";
                                lcol.isRequired = true;
                                lcol.defaultValue = ddlWholesaler.ToString();
                                ltableColumns.Add(lcol);

                                WholesalerHQImport whqi = new WholesalerHQImport();
                                foreach (var field in whqi.GetType().GetProperties())
                                {
                                    lcol = new TableColumns();
                                    lcol.name = field.Name;
                                    lcol.fieldType = "text";
                                    ltableColumns.Add(lcol);
                                }
                            }
                           
                            if (lWholesalerHQImport != null && lWholesalerHQImport.Count > 0)
                            {
                                var i = 0;
                                do
                                {
                                    var pwholesalerHQ = lWholesalerHQImport.AsEnumerable().Skip(i).Take(LuxeIQ.Common.Constants.maxLoopCountForImport);
                                   
                                    await Task.Run(async () =>
                                    {
                                        IList<Dictionary<string, object>> ldic = new List<Dictionary<string, object>>();
                                        ldic = pwholesalerHQ.Select(x => x.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(x, null))).ToList();

                                        await ServiceRepository.BulkInsert(ldic, ltableColumns, "public", "WholesalerHQ");

                                        i = i + LuxeIQ.Common.Constants.maxLoopCountForImport;
                                    });
                                } while (i < lWholesalerHQImport.Count);
                            }
                            else
                            {
                                TempData["msg"] = "<script type=\"text/javascript\">alert('No wholesaler HQ records available in the import format','Error');</script>";
                            }
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script type=\"text/javascript\">alert('Please attach a file to import wholesaler HQ','Error');</script>";
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
            return RedirectToPage("./wholesalerhq");
        }
    }
}