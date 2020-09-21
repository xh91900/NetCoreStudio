using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using NetCoreWebBase.Entities;
using Dapper.Contrib.Extensions;

namespace NetCoreWebBase.DapperHelper
{
    public interface IDapperHelper : IDisposable
    {
        public T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction dbTransaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : BaseEntity;

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction dbTransaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : BaseEntity;

        public int Execute(string sql, object param = null, IDbTransaction dbTransaction = null, int? commandTimeout = null);

        public T Get<T>(int id) where T : BaseEntity;

        public IEnumerable<T> GetAll<T>(int id) where T : BaseEntity;

        public long Insert<T>(T t) where T : BaseEntity;

        public bool Update<T>(T t) where T : BaseEntity;

        public bool Delete<T>(T t) where T : BaseEntity;
        
    }

    public class DapperHelper: IDapperHelper
    {
        //优化到ConnectionOptions
        //static IDbConnection dbConnection = new SqlConnection();

        //public string ConnectionString = ConnectionOptions.ConnectionString;

        //public DapperHelper()
        //{
        //    if(string.IsNullOrEmpty(dbConnection.ConnectionString))
        //    {
        //        dbConnection.ConnectionString = ConnectionString;
        //    }
        //}

        public ICustomConnectionFactory _customConnectionFactory = null;

        public IDbConnection dbConnection = null;

        public DapperHelper(ICustomConnectionFactory customConnectionFactory)
        {
            _customConnectionFactory = customConnectionFactory;
            dbConnection = _customConnectionFactory.GetConnection(DBExcuteOption.Read);
        }

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="dbTransaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql,object param=null,IDbTransaction dbTransaction=null,int? commandTimeout=null,CommandType? commandType=null) where T: BaseEntity
        {
            //使用事务要在外面把链接先打开，因为创建事务需要把链接打开后才能创建
            ConnectionOptions.DbConnection.Open();
            using (dbTransaction= ConnectionOptions.DbConnection.BeginTransaction())
            {
                var result= ConnectionOptions.DbConnection.QueryFirstOrDefault<T>(sql, param, dbTransaction, commandTimeout, commandType);
                ConnectionOptions.DbConnection.Close();
                return result;
            }
        }

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="dbTransaction"></param>
        /// <param name="buffered">缓冲/缓存</param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction dbTransaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : BaseEntity
        {
            return _customConnectionFactory.GetConnection(DBExcuteOption.Read).Query<T>(sql, param, dbTransaction, buffered, commandTimeout, commandType);
            //return ConnectionOptions.DbConnection.Query<T>(sql, param, dbTransaction,buffered, commandTimeout, commandType);
        }

        public int Execute(string sql, object param = null, IDbTransaction dbTransaction = null, int? commandTimeout = null)
        {
            return ConnectionOptions.DbConnection.Execute(sql, param, dbTransaction, commandTimeout);
        }


        public T Get<T>(int id) where T : BaseEntity
        {
            //通过dapper的官方扩展Dapper.Contrib实现单表查询。
            return ConnectionOptions.DbConnection.Get<T>(id);
        }

        public IEnumerable<T> GetAll<T>(int id) where T : BaseEntity
        {
            //通过dapper的官方扩展Dapper.Contrib实现单表查询。
            return ConnectionOptions.DbConnection.GetAll<T>();
        }

        public long Insert<T>(T t) where T : BaseEntity
        {
            //通过dapper的官方扩展Dapper.Contrib实现单表查询。
            return ConnectionOptions.DbConnection.Insert<T>(t);
        }

        public bool Update<T>(T t) where T : BaseEntity
        {
            //通过dapper的官方扩展Dapper.Contrib实现单表查询。
            return ConnectionOptions.DbConnection.Update<T>(t);
        }

        public bool Delete<T>(T t) where T : BaseEntity
        {
            //通过dapper的官方扩展Dapper.Contrib实现单表查询。
            return ConnectionOptions.DbConnection.Delete<T>(t);
        }

        /// <summary>
        /// 这里可以释放链接
        /// 实现了IDisposable接口线程销毁的时候触发
        /// </summary>
        public void Dispose()
        {
            if (dbConnection != null)
                dbConnection.Dispose();
        }
    }
}
