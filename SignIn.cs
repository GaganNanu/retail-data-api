using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace retail_data_api.Models;

public class SignIn
{
    public string UserName { get; set; }
    public string Password { get; set; }
}