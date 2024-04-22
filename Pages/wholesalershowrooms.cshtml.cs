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
    public class wholesalershowroomsModel : PageModel
    {
        private readonly ILogger<wholesalershowroomsModel> _logger;
        private readonly IWholesalerShowroomRepository _wholesalerShowroomRepository;
        private readonly IWholesalerHQRepository _wholesalerHQRepository;
        private readonly IWholesalerRepository _wholesalerRepository;

        public wholesalershowroomsModel(ILogger<wholesalershowroomsModel> logger, IWholesalerShowroomRepository wholesalerShowroomRepository, IWholesalerRepository wholesalerRepository, IWholesalerHQRepository wholesalerHQRepository)
        {
            _wholesalerShowroomRepository = wholesalerShowroomRepository;
            _wholesalerRepository = wholesalerRepository;
            _wholesalerHQRepository = wholesalerHQRepository;
            _logger = logger;
        }
        public IList<WholesalerHQ> wholesalerHQ { get; set; } = default!;

        public IList<WholesalerShowrooms> showrooms { get; set; } = default!;
        public IList<Wholesalers> wholesaler { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    wholesaler = await _wholesalerRepository.GetAll();
                    showrooms = await _wholesalerShowroomRepository.GetAllShowroomByManufactuerId(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                   // wholesalerHQ = await _wholesalerHQRepository.FindbyWholesaler(wholesaler[0].wholesalerId);
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
        public async Task<IActionResult> OnGetWholesalerHQ(Int64 wholesalerId)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    if (wholesalerId > 0)
                    {
                        var wholesalerHQRepo = await _wholesalerHQRepository.FindbyWholesaler(wholesalerId);
                        return new JsonResult(wholesalerHQRepo);

                    }
                    else
                    {
                        return new JsonResult(null);
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
                return new JsonResult(null);
            }

        }
        public async Task<IActionResult> OnPostImport(IFormFile fileInput, int ddlWholesaler = 0, int ddlWholesalerHQ = 0)
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
                            List<WholesalerShowroomImport> lShowrooms = new List<WholesalerShowroomImport>();
                            using (var reader = new StreamReader(fileInput.OpenReadStream()))
                            {
                                var csvConfig = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
                                csvConfig.IgnoreReferences = true;
                                csvConfig.DetectColumnCountChanges = true;
                                csvConfig.IgnoreBlankLines = true;
                                using (var csv = new CsvReader(reader, csvConfig))
                                {
                                    lShowrooms = csv.GetRecords<WholesalerShowroomImport>().ToList();
                                }
                                TableColumns lcol = new TableColumns();
                                lcol.name = "wholesalerId";
                                lcol.fieldType = "bigint";
                                lcol.isRequired = true;
                                lcol.defaultValue = ddlWholesaler.ToString();
                                ltableColumns.Add(lcol);

                                WholesalerShowroomImport wsi = new WholesalerShowroomImport();
                                foreach (var field in wsi.GetType().GetProperties())
                                {
                                    lcol = new TableColumns();
                                    lcol.name = field.Name;
                                    lcol.fieldType = "text";
                                    ltableColumns.Add(lcol);
                                }
                            }
 
                            if (lShowrooms != null && lShowrooms.Count > 0)
                            {
                                var i = 0;
                                do
                                {
                                    var pwholesalerShowroom = lShowrooms.AsEnumerable().Skip(i).Take(LuxeIQ.Common.Constants.maxLoopCountForImport);
                                    await Task.Run(async () =>
                                    {
                                        IList<Dictionary<string, object>> ldic = new List<Dictionary<string, object>>();
                                        ldic = pwholesalerShowroom.Select(x => x.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(x, null))).ToList();

                                        await ServiceRepository.BulkInsert(ldic, ltableColumns, "public", "WholesalerShowrooms");

                                        i = i + LuxeIQ.Common.Constants.maxLoopCountForImport;
                                    });
                                } while (i < lShowrooms.Count);
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

            return RedirectToPage("./wholesalershowrooms");
        }
    }
}