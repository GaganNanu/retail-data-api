using System.ComponentModel.DataAnnotations.Schema;

namespace retail_data_api.Models;

public class Transaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int HSHD_NUM { get; set; }

    public int BASKET_NUM { get; set; }
    public DateTime PURCHASE_ { get; set; }
    public int PRODUCT_NUM { get; set; }
    public double SPEND { get; set; }
    public int UNITS { get; set; }
    public string STORE_R { get; set; }
    public int WEEK_NUM { get; set; }
    public int YEAR { get; set; }
}