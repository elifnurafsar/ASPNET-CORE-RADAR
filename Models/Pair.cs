using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class Pair
    {
        private String sessionID;
        private int requestID;

        public Pair(String sessionID, int requestID)
        {
            this.sessionID = sessionID;
            this.requestID = requestID;
        }

        public String getSessionID()
        {
            return this.sessionID;
        }

        public int getRequestID()
        {
            return this.requestID;
        }
    }
}
