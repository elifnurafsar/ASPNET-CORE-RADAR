using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Controllers
{
    public class UserLogoutController : Controller
    {
        private IConfiguration _configuration;
        public UserLogoutController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Logout()
        {
            //Change IsLoggedIn to 0 in AccountSessions table
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "UPDATE AccountSessions SET IsLoggedIn = 0 WHERE IsLoggedIn = 1;";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    sqlcomm.ExecuteNonQuery();
                }
            }
            
            //remove the records of the currently logged in user from the global currentUserInfo array
            for(int i=0; i<3; i++)
            {
                Models.CurrentUser.currentUser.CurrentUserInfo[i] = null;
            }

            return RedirectToAction("Login", "UserLogin");
        }
    }
}
