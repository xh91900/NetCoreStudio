using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase.Entities
{
    /// <summary>
    /// 这里只能叫User不能叫Users，因为dapper会自动添加s
    /// 或者添加特性指定表名
    /// </summary>
    [Table("Users")]
    public class User: BaseEntity
    {
        [Key]
        public string UserNo { get; set; }

        public string UserName { get; set; }

        [Computed]//更新的时候不更新
        public int UserLevel { get; set; }

        [Write(false)]//禁止往这个字段写值
        public string Password { get; set; }
    }
}
