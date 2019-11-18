using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToolkit.Helpers
{
    class LoginHelper
    {
        public static string Login(string email, string password, string recoveryEmail)
        {
            return email + ":" + password + ":" + recoveryEmail;
        }
    }
}
