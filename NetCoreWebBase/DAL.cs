using NetCoreWebBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase
{
    public class DAL
    {
        DapperHelper.DapperHelper DapperHelper = new DapperHelper.DapperHelper();

        public User GetUserByLogin(string userName, string password)
        {
            string sql = "select * from users where username=@username and password=@password";

            var user= DapperHelper.QueryFirstOrDefault<User>(sql, new { userName, password });
            if (user == null)
            {
                //引用类型返回null
                return default;
            }

            return user;
        }
    }
}
