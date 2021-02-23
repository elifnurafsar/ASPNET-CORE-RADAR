using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using ASPNETAOP.Models;
using ASPNETAOP.Aspect;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    [Guid("18020B1D-DB0B-4600-9443-8ACA5C6CF4FE")]
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [IsAuthenticated]
        public IActionResult Profile()
        {
            ViewData["message"] = "User name: " + Models.CurrentUser.currentUser.getUsername() + "\r\n Mail: " + Models.CurrentUser.currentUser.getUsermail();
            return View();
        }

        [HttpPost]
        public IActionResult Profile(UserLogin ur)
        {
            ViewData["message"] = "User name: " + Models.CurrentUser.currentUser.CurrentUserInfo[1] + "\r\n Mail: " + Models.CurrentUser.currentUser.CurrentUserInfo[2];
            return View(ur);
        }
    }
}
