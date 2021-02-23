using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using Microsoft.Extensions.Configuration;

namespace ASPNETAOP.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IConfiguration _configuration;
        public UserRegistrationController(IConfiguration Configuration) { _configuration = Configuration; }

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserRegister ur)
        {
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "insert into AccountInfo(Username, Usermail, Userpassword) values ('" + ur.Username + "', '" + ur.Usermail + "', '" + ur.Userpassword + "' )";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                    ViewData["Message"] = "New User "+ur.Username+ " is saved successfully";
                }
            }

            return View(ur);
        }
    }
}
