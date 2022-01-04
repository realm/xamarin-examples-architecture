using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SharedGroceriesWebService.Models;
using SharedGroceriesWebService.Services;

namespace SharedGroceriesWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<UserInfo>> GetAllUsers()
            => DataService.GetAllUsers();

        [HttpGet("authenticate")]
        public ActionResult<UserInfo> Authenticate(string username, string password)
            => DataService.Authenticate(username, password);
    }
}
