using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Hangfire.Storage;
using LuxeIQ.Common;
using LuxeIQ.Extensions;
using LuxeIQ.Models;
using LuxeIQ.Repositories;
using LuxeIQ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace LuxeIQ.Pages
{
    public class productsModel : PageModel
    {
        private readonly ILogger<productsModel> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public productsModel(ILogger<productsModel> logger, IProductRepository productRepository, IManufacturersRepository manufacturersRepository, IHostingEnvironment hostingEnvironment)
        {
            _productRepository = productRepository;
            _manufacturersRepository = manufacturersRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        public IList<Manufacturers> manufacturers { get; set; } = default!;
        public IList<IDictionary<string, object>> products { get; set; } = default!;
        public List<string> productkeys { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    Manufacturers lmanufacturer = await _manufacturersRepository.Find(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                    if (lmanufacturer != null)
                    {
                        var ltableName = Utilities.RemoveSpecialChars(lmanufacturer.businessName.ToLower()).Replace(" ", "_") + "_product";


                        if (!string.IsNullOrEmpty(ltableName) && ServiceRepository.isTableExist("products", ltableName) > 0)
                        {
                            products = ServiceRepository.GetRecordsByFilter(null, ltableName, "products", 1, 1);
                            if (products != null && products.Count > 0)
                                productkeys = products[0].Keys.ToList();
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
                products = null;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetProductDetails()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LUXEIQ_LOGIN_USER")))
                {
                    var draw = HttpContext.Request.Query["sEcho"].FirstOrDefault();
                    var start = HttpContext.Request.Query["iDisplayStart"].FirstOrDefault();
                    var length = HttpContext.Request.Query["iDisplayLength"].FirstOrDefault();
                    var sortColumn = HttpContext.Request.Query["iSortCol_0"].FirstOrDefault();
                    var sortColumnDir = HttpContext.Request.Query["sSortDir_0"].FirstOrDefault();

                    var searchValue = HttpContext.Request.Query["sSearch"].FirstOrDefault();

                    //Paging Size (10,20,50,100)
                    int pageSize = length != null ? Convert.ToInt32(length) : 10;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;

                    // Getting all Customer data
                    //var dbTableData = getHDCTREASPF();
                    Manufacturers lmanufacturer = await _manufacturersRepository.Find(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                    if (lmanufacturer != null)
                    {
                        var ltableName = Utilities.RemoveSpecialChars(lmanufacturer.businessName.ToLower()).Replace(" ", "_") + "_product";
                        if (!string.IsNullOrEmpty(ltableName) && ServiceRepository.isTableExist("products", ltableName) > 0)
                        {
                            IList<IDictionary<string, object>> lproducts = ServiceRepository.GetRecordsByFilter(null, ltableName, "products", 1, 1);
                            if (lproducts != null && lproducts.Count > 0)
                            {
                                int i = 0;
                                List<TableColumns> lcols = new List<TableColumns>();
                                foreach (var key in lproducts[0].Keys)
                                {
                                    if (i < 5)
                                    {
                                        TableColumns lcol = new TableColumns();
                                        lcol.name = key;
                                        lcol.fieldType = "text";
                                        lcols.Add(lcol);
                                        i++;
                                    }
                                }
                                //if (lcols != null && lcols.Count == 5)
                                //{
                                //    TableColumns lcol = new TableColumns();
                                //    lcol.name = "SFT_POSITION_ID";
                                //    lcol.fieldType = "text";
                                //    lcols.Add(lcol);
                                //    i++;
                                //}
                                products = ServiceRepository.GetRecordsByFilterWithSortAndSearch(lcols, ltableName, "products", skip + 1, pageSize, 0, sortColumn, sortColumnDir, searchValue);
                                recordsTotal = ServiceRepository.GetRecordsByFilterCount(lcols, ltableName, "products", searchValue);
                            }
                        }
                    }

                    var data = products != null ? products.ToList() : null;
                    //Returning Json Data
                    JsonSerializerSettings jsSettings = new JsonSerializerSettings();
                    jsSettings.ContractResolver = new DefaultContractResolver();
                    return new JsonResult(new { sEcho = draw, iTotalDisplayRecords = recordsTotal, iTotalRecords = recordsTotal, aaData = data }, jsSettings);

                }
                else
                {
                    return new JsonResult(new { });
                }
            }
            catch (Exception ex)
            {
                products = null;
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
                            List<dynamic> lProduct = new List<dynamic>();
                            string[] headerRow = null;
                            string lArticleNumber = string.Empty;
                            using (var reader = new StreamReader(fileInput.OpenReadStream()))
                            {
                                var csvConfig = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
                                csvConfig.IgnoreReferences = true;
                                csvConfig.DetectColumnCountChanges = true;
                                csvConfig.PrepareHeaderForMatch = args => args.Header.Replace(" ", "");
                                csvConfig.IgnoreBlankLines = true;
                                csvConfig.HasHeaderRecord = true;
                                csvConfig.BadDataFound = null;

                                using (var csv = new CsvReader(reader, csvConfig))
                                {
                                    csv.Read();
                                    csv.ReadHeader();
                                    headerRow = csv.HeaderRecord;
                                    lProduct = csv.GetRecords<dynamic>().ToList();
                                    int tableCreated = 0;
                                    if (headerRow != null && headerRow.Count() > 0)
                                    {
                                        List<TableColumns> ltableColumns = new List<TableColumns>();
                                        foreach (var col in headerRow)
                                        {
                                            TableColumns lcol = new TableColumns();
                                            lcol.name = col.Replace(" ", "");
                                            lcol.fieldType = "text";
                                            if (lcol.name.ToLower() == "articlenumber")
                                            {
                                                lArticleNumber = lcol.name;
                                                lcol.isPrimaryKey = true;
                                            }
                                            ltableColumns.Add(lcol);
                                        }
                                        if (ltableColumns.Count > 0)
                                        {
                                            Manufacturers lmanufacturer = await _manufacturersRepository.Find(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                                            if (lmanufacturer != null)
                                            {
                                                var ltableName = Utilities.RemoveSpecialChars(lmanufacturer.businessName.ToLower()).Replace(" ", "_") + "_product";

                                                var istableExist = ServiceRepository.isTableExist("products", ltableName);

                                                if (istableExist > 0)
                                                {
                                                    List<DatabaseTableColumns> ldatabaseTableColumns = ServiceRepository.GetPGDatabaseTableColumns(ltableName, "products");
                                                    if ((ldatabaseTableColumns.Count() > ltableColumns.Count()) || (ldatabaseTableColumns.Count() < ltableColumns.Count()))
                                                    {
                                                        TempData["msg"] = "<script type=\"text/javascript\">alert('Invalid import file.Column count mismatch error','Error');</script>";
                                                        return RedirectToPage("./products");
                                                    }
                                                    foreach (TableColumns lcol in ltableColumns)
                                                    {
                                                        if (ldatabaseTableColumns.Where(p => p.name == lcol.name).Count() == 0)
                                                        {
                                                            TempData["msg"] = "<script type=\"text/javascript\">alert('Invalid import file.Column not available','Error');</script>";
                                                            return RedirectToPage("./products");
                                                        }
                                                    }
                                                }
                                                Products lproduct = await _productRepository.FindByManufacturingId(Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId")));
                                                if (lproduct == null)
                                                {
                                                    lproduct = new Products();
                                                    lproduct.manufacturerId = Convert.ToInt64(HttpContext.Session.GetString("ManufacturerId"));
                                                    List<ProductAttribute> lproductAttribute = new List<ProductAttribute>();
                                                    if (ltableColumns != null && ltableColumns.Count > 0)
                                                    {
                                                        List<ProductAttribute> productAttributes = null;
                                                        string contentRootPath = _hostingEnvironment.ContentRootPath;
                                                        var jsonData = System.IO.File.ReadAllText(contentRootPath + "/wwwroot/attributes/" + ltableName.Replace("_product", "") + "_attributes.json");
                                                        if (!string.IsNullOrEmpty(jsonData))
                                                        {
                                                            productAttributes = JsonConvert.DeserializeObject<List<ProductAttribute>>(jsonData);
                                                        }
                                                        foreach (TableColumns lcol in ltableColumns)
                                                        {
                                                            ProductAttribute lattr = new ProductAttribute();
                                                            lattr.field_name = lcol.name;
                                                            lattr.include_in_attribute = true;
                                                            if (productAttributes != null && productAttributes.Count() > 0)
                                                            {
                                                                var ss = productAttributes.Where(p => p.field_name.Replace(" ", "").ToLower().Trim() == lcol.name.Replace(" ", "").ToLower().Trim()).FirstOrDefault();
                                                                lattr.datatype = ss.datatype.ToUpper();
                                                            }
                                                            else
                                                            {
                                                                lattr.datatype = DataType.TEXT.ToString();
                                                            }

                                                            lproductAttribute.Add(lattr);
                                                        }
                                                    }
                                                    lproduct.productAttributes = JsonConvert.SerializeObject(lproductAttribute);
                                                    lproduct.tableName = ltableName;
                                                    await _productRepository.Add(lproduct);
                                                }

                                                if (istableExist == 0)
                                                {
                                                    tableCreated = ServiceRepository.CreatePostgresTable(ltableColumns, ltableName);
                                                }
                                                else
                                                {
                                                    List<DatabaseTableColumns> ldatabaseTableColumns = ServiceRepository.GetPGDatabaseTableColumns(ltableName, "products");
                                                    tableCreated = 1;
                                                }
                                                if (tableCreated > 0)
                                                {
                                                    //var query = lProduct.GroupBy(item => item.lArticleNumber).Select(group => group.Last()).OrderBy(item => item.lArticleNumber).Select(a => a).ToList();
                                                    if (lProduct != null && lProduct.Count > 0)
                                                    {
                                                        var i = 0;
                                                        do
                                                        {
                                                            var pProduct = lProduct.AsEnumerable().Skip(i).Take(LuxeIQ.Common.Constants.maxLoopCountForImport);
                                                            await Task.Run(async () =>
                                                            {
                                                                await ServiceRepository.BulkInsertForProducts(pProduct.Cast<IDictionary<string, object>>().ToList(), ltableColumns, "products", ltableName);

                                                                i = i + LuxeIQ.Common.Constants.maxLoopCountForImport;
                                                            });
                                                        } while (i < lProduct.Count);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script type=\"text/javascript\">alert('Please attach a file to import products','Error');</script>";
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
            return RedirectToPage("./products");
        }
    }
}