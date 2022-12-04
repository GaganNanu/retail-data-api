using System.ComponentModel.DataAnnotations.Schema;

namespace retail_data_api.Models;

public class Joined
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int HSHD_NUM { get; set; }
    public string L { get; set; }
    public string AGE_RANGE { get; set; }
    public string MARITAL { get; set; }
    public string INCOME_RANGE { get; set; }
    public string HOMEOWNER { get; set; }
    public string HSHD_COMPOSITION { get; set; }
    public string HH_SIZE { get; set; }
    public string CHILDREN { get; set; }
    public int BASKET_NUM { get; set; }
    public DateTime PURCHASE_ { get; set; }
    public int PRODUCT_NUM { get; set; }
    public double SPEND { get; set; }
    public int UNITS { get; set; }
    public string STORE_R { get; set; }
    public int WEEK_NUM { get; set; }
    public int YEAR { get; set; }
    public string DEPARTMENT { get; set; }
    public string COMMODITY { get; set; }
    public string BRAND_TY { get; set; }
    public string NATURAL_ORGANIC_FLAG { get; set; }

}