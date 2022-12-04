using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using retail_data_api.Models;
using EFCore.BulkExtensions;
using System.Text.RegularExpressions;

namespace retail_data_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RetailDataController : ControllerBase
{

    private readonly ILogger<RetailDataController> _logger;
    RetailContext db;
    public RetailDataController(ILogger<RetailDataController> logger, RetailContext db)
    {
        _logger = logger;
        this.db = db;
    }

    [HttpGet]
    public List<Joined> Get()
    {
        return this.db.Joined.ToList();
    }


    [HttpGet("{HSHD_NUM}")]
    public ActionResult<List<Joined>> Get(int HSHD_NUM)
    {
        try
        {
            return this.db.Joined.Where(j => j.HSHD_NUM == HSHD_NUM).ToList();
        }
        catch(Exception ex) 
        {
            return StatusCode(500, ex);
        }
    }


    [HttpPost("upload")]
    public async Task<ActionResult> UploadProducts()
    {
        try
        {
            var files = Request.Form.Files;
            var productsFile = files.First(f => f.Name == "products");
            var householdsFile = files.First(f => f.Name == "households");
            var transactionsFile = files.First(f => f.Name == "transactions");
            IEnumerable<Product> products = Enumerable.Empty<Product>();
            IEnumerable<Transaction> transactions = Enumerable.Empty<Transaction>(); ;
            IEnumerable<Household> households = Enumerable.Empty<Household>(); ;
            Dictionary<string, Type> typeMap = new Dictionary<string, Type>()
        {
            { "transactions",typeof(Transaction)},
            { "products", typeof(Product)},
            { "households",typeof(Household)}
        };
            Dictionary<string, HashSet<string>> columnMap = new Dictionary<string, HashSet<string>>()
        {
            { "products", new HashSet<string> { "PRODUCT_NUM", "DEPARTMENT", "COMMODITY", "BRAND_TY", "NATURAL_ORGANIC_FLAG" }},
            { "transactions",new HashSet<string> { "HSHD_NUM", "BASKET_NUM", "PURCHASE_", "PRODUCT_NUM", "SPEND", "UNITS", "STORE_R", "WEEK_NUM", "YEAR"}},
            { "households",new HashSet<string> {"HSHD_NUM", "L", "AGE_RANGE", "MARITAL", "INCOME_RANGE", "HOMEOWNER", "HSHD_COMPOSITION", "HH_SIZE", "CHILDREN"}},
        };
            List<string> bad = new List<string>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                PrepareHeaderForMatch = header => Regex.Replace(header.Header, @"\s", string.Empty),
                BadDataFound = context =>
                {
                    bad.Add(context.RawRecord);
                }
            };
            // Validation for column name
            foreach (var file in files)
            {
                var data = new MemoryStream();
                await file.CopyToAsync(data);
                data.Position = 0;
                using (TextReader reader = new StreamReader(data))
                using (var csvReader = new CsvReader(reader, config))
                {
                    // csvReader.Context.RegisterClassMap<ParticipantMap>();
                    if (file.Name == "products")
                        products = csvReader.GetRecords<Product>().ToList();
                    else if (file.Name == "households")
                        households = csvReader.GetRecords<Household>().ToList();
                    else if (file.Name == "transactions")
                        transactions = csvReader.GetRecords<Transaction>().ToList();

                    // if(csvReader.HeaderRecord == null || csvReader.HeaderRecord.Count() != columnMap[file.Name].Count || !(csvReader.HeaderRecord.All(h => columnMap[file.Name].Contains(h))))
                    //     return BadRequest($"Incorrect column names for {file.Name} file");
                    // await db.AddRangeAsync(records);
                    // db.SaveChanges();
                    // return Ok(true);
                }
            }
            var joinedTransactions = transactions.Join(products, t => t.PRODUCT_NUM, p => p.PRODUCT_NUM, (t, p) => new
            {
                HSHD_NUM = t.HSHD_NUM,
                BASKET_NUM = t.BASKET_NUM,
                PURCHASE_ = t.PURCHASE_,
                PRODUCT_NUM = t.PRODUCT_NUM,
                SPEND = t.SPEND,
                UNITS = t.UNITS,
                STORE_R = t.STORE_R,
                WEEK_NUM = t.WEEK_NUM,
                YEAR = t.YEAR,
                DEPARTMENT = p.DEPARTMENT,
                COMMODITY = p.COMMODITY,
                BRAND_TY = p.BRAND_TY,
                NATURAL_ORGANIC_FLAG = p.NATURAL_ORGANIC_FLAG,
            });
            var joined = households.Join(joinedTransactions, h => h.HSHD_NUM, t => t.HSHD_NUM, (h, t) => new Joined
            {
                HSHD_NUM = t.HSHD_NUM,
                BASKET_NUM = t.BASKET_NUM,
                PURCHASE_ = t.PURCHASE_,
                PRODUCT_NUM = t.PRODUCT_NUM,
                SPEND = t.SPEND,
                UNITS = t.UNITS,
                STORE_R = t.STORE_R,
                WEEK_NUM = t.WEEK_NUM,
                YEAR = t.YEAR,
                DEPARTMENT = t.DEPARTMENT,
                COMMODITY = t.COMMODITY,
                BRAND_TY = t.BRAND_TY,
                NATURAL_ORGANIC_FLAG = t.NATURAL_ORGANIC_FLAG,
                AGE_RANGE = h.AGE_RANGE,
                CHILDREN = h.CHILDREN,
                HH_SIZE = h.HH_SIZE,
                HOMEOWNER = h.HOMEOWNER,
                HSHD_COMPOSITION = h.HSHD_COMPOSITION,
                INCOME_RANGE = h.INCOME_RANGE,
                L = h.L,
                MARITAL = h.MARITAL,
            });
            using (var transaction = db.Database.BeginTransaction())
            {
                await db.BulkInsertAsync<Joined>(joined.ToList());
                transaction.Commit();
            }
            return Ok(true);
        }
        catch(Exception ex) 
        {
            return StatusCode(500, ex);
        }   
    }
}