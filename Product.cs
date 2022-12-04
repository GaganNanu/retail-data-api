using System.ComponentModel.DataAnnotations.Schema;

namespace retail_data_api.Models;

public class Product
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int PRODUCT_NUM { get; set; }

    public string DEPARTMENT { get; set; }

    public string COMMODITY { get; set; }

    public string BRAND_TY { get; set; }

    public string NATURAL_ORGANIC_FLAG { get; set; }

}