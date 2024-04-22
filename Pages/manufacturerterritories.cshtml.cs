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
    public class manufacturerterritoriesModel : PageModel
    {
        private readonly ILogger<manufacturerterritoriesModel> _logger;
        private readonly IManufacturersTerritoryRepository _manufacturersTerritoryRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        public manufacturerterritoriesModel(ILogger<manufacturerterritoriesModel> logger, IManufacturersTerritoryRepository manufacturersTerritoryRepository, IManufacturersRepository manufacturersRepository)
        {
            _manufacturersTerritoryRepository = manufacturersTerritoryRepository;
            _manufacturersRepository = manufacturersRepository;
            _logger = logger;
        }
        public IList<Manufacturers> manufacturers { get; set; } = default!;
        public IList<ManufacturerTerritories> manufacturersTerritory { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    //manufacturers = await _manufacturersRepository.GetAll();
                    manufacturersTerritory = await _manufacturersTerritoryRepository.GetAllByManufacturerId(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
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
        public void OnPost()
        {
            Console.WriteLine("calling post");
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
                            List<ManufacturerTerritoryImport> lManufacturerTerritory = new List<ManufacturerTerritoryImport>();
                            using (var reader = new StreamReader(fileInput.OpenReadStream()))
                            {
                                var csvConfig = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
                                csvConfig.IgnoreReferences = true;
                                csvConfig.DetectColumnCountChanges = true;
                                csvConfig.IgnoreBlankLines = true;
                                using (var csv = new CsvReader(reader, csvConfig))
                                {
                                    lManufacturerTerritory = csv.GetRecords<ManufacturerTerritoryImport>().ToList();
                                }
                                TableColumns lcol = new TableColumns();
                                lcol.name = "manufacturerId";
                                lcol.fieldType = "bigint";
                                lcol.isRequired = true;
                                lcol.defaultValue = HttpContext.Session.GetString("ManufacturerId");
                                ltableColumns.Add(lcol);
                                ManufacturerTerritoryImport mti = new ManufacturerTerritoryImport();
                                foreach (var field in mti.GetType().GetProperties())
                                {
                                    lcol = new TableColumns();
                                    lcol.name = field.Name;
                                    lcol.fieldType = "text";
                                    ltableColumns.Add(lcol);
                                }
                            }
                            
                            if (lManufacturerTerritory != null && lManufacturerTerritory.Count > 0)
                            {
                                var i = 0;
                                do
                                {
                                    var pmanufacturerTerritories = lManufacturerTerritory.AsEnumerable().Skip(i).Take(LuxeIQ.Common.Constants.maxLoopCountForImport);
                                    await Task.Run(async () =>
                                    {
                                        IList<Dictionary<string, object>> ldic = new List<Dictionary<string, object>>();
                                        ldic = pmanufacturerTerritories.Select(x => x.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(x, null))).ToList();

                                        await ServiceRepository.BulkInsert(ldic, ltableColumns, "public", "ManufacturerTerritories");
                                        i = i + LuxeIQ.Common.Constants.maxLoopCountForImport;
                                    });
                                } while (i < lManufacturerTerritory.Count);
                            }
                            else
                            {
                                TempData["msg"] = "<script type=\"text/javascript\">alert('No manufacturer territory records available in the import format','Error');</script>";
                            }
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script type=\"text/javascript\">alert('Please attach a file to import manufacturer','Error');</script>";
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
             
            return RedirectToPage("./manufacturerterritories");
        }
    }
}