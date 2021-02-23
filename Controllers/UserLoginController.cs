using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using ASPNETAOP.Aspect;
using ASPNETAOP;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace ASPNETAOP.Controllers
{
    public class UserLoginController : Controller
    {
        private IConfiguration _configuration;
        public UserLoginController(IConfiguration Configuration) { _configuration = Configuration; }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        //Used to log user activies 
        //Session managment has been moved to ASPNETAOP-WebServer solution, which can be accessed with SendRequest method
        public void SaveCookie(UserLogin ur)
        {
            String connection = _configuration.GetConnectionString("localDatabase");
            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                DateTime thisDay = DateTime.Now;
                //  30/3/2020 12:00 AM
                //0 - Logged Out & 1 - Logged in
                string sqlQuerySession = "insert into AccountSessions(Usermail, LoginDate, IsLoggedIn) values ('" + ur.Usermail + "', '" + thisDay.ToString("g") + "', 1 )";
                using (SqlCommand sqlcommCookie = new SqlCommand(sqlQuerySession, sqlconn))
                {
                    sqlconn.Open();
                    sqlcommCookie.ExecuteNonQuery();
                }
            }
        }

        public void SendRequest(String[] ur)
        {
            HttpClient client = new HttpClient();

            Console.WriteLine("In UserLoginCOntroller -- session id is :" + (HttpContext.Session.Id));

            SessionList.listObject.Pair.Add(new Pair(HttpContext.Session.Id, SessionList.listObject.count));

            PostJsonHttpClient("https://localhost:44316/api/SessionItems", client, ur);
        }

        private static async Task PostJsonHttpClient(string uri, HttpClient httpClient, String[] userInfo)
        {
            var postUser = new SessionItem { Id = SessionList.listObject.count++,  UserID = Int32.Parse(userInfo[0]), Username = userInfo[1], Usermail = userInfo[2], Roleid = Int32.Parse(userInfo[3]) };

            var postResponse = await httpClient.PostAsJsonAsync(uri, postUser);

            postResponse.EnsureSuccessStatusCode();
        }

        //When user is redirected to login page, user's info is 
        //1. stored in CurrentUser array (in ASPNETAOP project)
        //2. sent to UserSession (in ASPNETAOP-Session) to be stored in DatabaseDb 
        //3. saved as a cookie (in ASPNETAOP) in AccountDb
        [HttpPost]
        public IActionResult Login(UserLogin ur)
        {

            String connection = _configuration.GetConnectionString("localDatabase");

            using (SqlConnection sqlconn = new SqlConnection(connection))
            {
                string sqlquery = "select AI.Userpassword, AI.UserID, AI.Username, UR.Roleid  from AccountInfo AI, UserRoles UR where AI.UserID = UR.UserID AND AI.Usermail = '" + ur.Usermail + "' ";
                using (SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn))
                {
                    sqlconn.Open();
                    SqlDataReader reader = sqlcomm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0).Equals(ur.Userpassword)){
                                ViewData["Message"] = "Welcome: " + ur.Usermail;

                                //Hold current user's info in ASPNETAOP project 
                                String userID = reader.GetInt32(1).ToString();    //UserID;
                                String username = reader.GetString(2);    //Username;
                                String usermail = ur.Usermail;
                                String roleID = reader.GetInt32(3).ToString();

                                Models.CurrentUser.currentUser.CurrentUserInfo[0] = userID;
                                Models.CurrentUser.currentUser.CurrentUserInfo[1] = username;
                                Models.CurrentUser.currentUser.CurrentUserInfo[2] = usermail;

                                reader.Close();
                                sqlconn.Close();

                                //Store user's session as a cookie in AccountDb
                                SaveCookie(ur);

                                //Send the user information to ASPNETAOP-WebServer for session
                                String[] UserLoggedIn = {userID, username, usermail, roleID};
                                SendRequest(UserLoggedIn);

                                ViewData["Message"] = "Successfully logged in";
                                reader.Close();

                                return RedirectToAction("Profile","UserProfile", new { ur });
                            }
                            else
                            {
                                ViewData["Message"] = "Incorrect password";
                            }
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "No user with this email address has been found";
                        reader.Close();

                        return RedirectToAction("Create", "UserRegistration");
                    }
                    reader.Close();
                }

            }

            return View(ur);
        }
    }
}
