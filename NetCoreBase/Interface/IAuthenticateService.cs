using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.Interface
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(string Username,string Password, out string token);
    }
}
