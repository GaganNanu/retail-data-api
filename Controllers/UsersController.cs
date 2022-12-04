using Microsoft.AspNetCore.Mvc;
using retail_data_api.Models;

namespace retail_data_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<UsersController> _logger;
    RetailContext db;
    public UsersController(ILogger<UsersController> logger, RetailContext db)
    {
        _logger = logger;
        this.db = db;
    }

    [HttpPost]
    [Route("signup")]
    public ActionResult SignUp(User user)
    {
        try
        {
            if (db.Users.Any(u => u.UserName == user.UserName || u.Email == user.Email))
            {
                return BadRequest("User already Exists. Please Sign In");
            }
            this.db.Add<User>(user);
            this.db.SaveChanges();
            return Ok("true");
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPost]
    [Route("signin")]
    public ActionResult SignIn(SignIn user)
    {
        if(user == null)
            return BadRequest();
        try
        {
            var userFromDb = db.Users.First(u => u.UserName == user.UserName && u.Password == user.Password);
            if (userFromDb != null)
            {
                return Ok(userFromDb.Email);
            }
            else
            {
                return Unauthorized("Invalid Credentials");
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}

