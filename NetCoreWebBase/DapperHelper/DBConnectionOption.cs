using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase.DapperHelper
{
    public class DBConnectionOption
    {
        public string WriteConnection { get; set; }

        /// <summary>
        ///80%的数据库操作是查询，此属性用于存放多个查询从库的连接字符串
        /// </summary>
        public List<string> ReadConnectionList { get; set; }

        /// 多个查询数据库实例的负载均衡策略<summary>
        /// 
        /// </summary>
        public Strategy Strategy { get; set; }
    }

    public enum DBExcuteOption
    {
        Write,
        Read
    }

    public enum Strategy
    {
        Polling,
        Random
    }
}
