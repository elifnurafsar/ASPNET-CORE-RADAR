using Microsoft.Data.SqlClient;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAOP.Models;
using ASPNETAOP.Controllers;

namespace ASPNETAOP.Aspect
{
   
    [PSerializable]
    public sealed class IsAuthenticatedAttribute : OnMethodBoundaryAspect
    {

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (Models.CurrentUser.currentUser.CurrentUserInfo[0] == null) throw new UserNotLoggedInException();
        }
    }


    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException() {

        }

        public UserNotLoggedInException(String message) : base(message) {
            
        }
    }

}
