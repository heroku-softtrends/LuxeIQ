using CsvHelper;
using CsvHelper.Configuration;
using LuxeIQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LuxeIQ.ViewModels;
using LuxeIQ.Repositories;
using LuxeIQ.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace LuxeIQ.Pages
{
    public class manufacturersModel : PageModel
    {
        private readonly ILogger<manufacturersModel> _logger;
        private readonly IManufacturersRepository _manufacturersRepository;

        public manufacturersModel(ILogger<manufacturersModel> logger, IManufacturersRepository manufacturersRepository)
        {
            _manufacturersRepository = manufacturersRepository;
            _logger = logger;
        }
        public IList<Manufacturers> manufacturers { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    manufacturers = await _manufacturersRepository.GetAll();
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
                            List<ManufacturerImport> lManufacturers = new List<ManufacturerImport>();
                            using (var reader = new StreamReader(fileInput.OpenReadStream()))
                            {
                                var csvConfig = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
                                csvConfig.IgnoreReferences = true;
                                csvConfig.DetectColumnCountChanges = true;
                                csvConfig.IgnoreBlankLines = true;
                                using (var csv = new CsvReader(reader, csvConfig))
                                {
                                    lManufacturers = csv.GetRecords<ManufacturerImport>().ToList();
                                }
                                StringBuilder sb = new StringBuilder();
                                ManufacturerImport mi = new ManufacturerImport();
                                foreach (var field in mi.GetType().GetProperties())
                                {
                                    TableColumns lcol = new TableColumns();
                                    lcol.name = field.Name;
                                    lcol.fieldType = "text";
                                    ltableColumns.Add(lcol);
                                }
                            }
                            
                            // List<Manufacturers> listManufacturer = lManufacturers.ToListModel();
                            if (lManufacturers != null && lManufacturers.Count > 0)
                            {
                                var i = 0;
                                do
                                {
                                    var pmanufacturer = lManufacturers.AsEnumerable().Skip(i).Take(LuxeIQ.Common.Constants.maxLoopCountForImport);
                                    await Task.Run(async () =>
                                    {
                                        IList<Dictionary<string, object>> ldic = new List<Dictionary<string, object>>();
                                        ldic = pmanufacturer.Select(x => x.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(x, null))).ToList();
                                        await ServiceRepository.BulkInsert(ldic, ltableColumns, "public", "Manufacturers");

                                        i = i + LuxeIQ.Common.Constants.maxLoopCountForImport;
                                    });
                                } while (i < lManufacturers.Count);
                            }
                            else
                            {
                                TempData["msg"] = "<script type=\"text/javascript\">alert('No manufacturer records available in the import format','Error');</script>";
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

            return RedirectToPage("./manufacturers");
        }
    }
}