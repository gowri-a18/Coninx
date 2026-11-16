using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoApi
{
    public class UserAuthentication : IDisposable
    {
        public string ValidateUser(string username, string password)
        {
            string Name = username == "sap" ? "Valid" : "InValid";
            string Pass = password == "Sap@12345" ? "Valid" : "InValid";

            if (Name == "Valid" && Pass == "Valid")
                return "true";
            else
                return "false";
        }
        public void Dispose()
        {
            //Dispose();  
        }
    }
}