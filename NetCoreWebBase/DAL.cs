using NetCoreWebBase.DapperHelper;
using NetCoreWebBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase
{
    public class DAL
    {
        public IDapperHelper _dapperHelper;

        public DAL(IDapperHelper dapperHelper)
        {
            _dapperHelper = dapperHelper;
        }

        public User GetUserByLogin(string userName, string password)
        {
            string sql = "select * from users where username=@username and password=@password";

            var user = _dapperHelper.QueryFirstOrDefault<User>(sql, new { userName, password });
            if (user == null)
            {
                //引用类型返回null
                return default;
            }

            return user;
        }
    }
}
