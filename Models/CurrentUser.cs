using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class CurrentUser
    {


        public String[] CurrentUserInfo = new String[3];
        //0 - id 
        //1 - username
        //2 - usermail

        public static readonly CurrentUser currentUser = new CurrentUser();

        public CurrentUser(String[] CurrentUserInfo)
        {
            this.CurrentUserInfo = CurrentUserInfo;
        }

        public CurrentUser()
        {

        }



        public String getUsername(){return CurrentUserInfo[1];}
        public String getUsermail(){return CurrentUserInfo[2];}
    }
}
