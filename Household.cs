using System.ComponentModel.DataAnnotations.Schema;

namespace retail_data_api.Models;

public class Household
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
}