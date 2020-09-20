using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase.DapperHelper
{
    public interface ICustomConnectionFactory
    {
        public IDbConnection GetConnection(DBExcuteOption dBExcuteOption);
    }

    public class CustomConnectionFactory: ICustomConnectionFactory
    {

        public DBConnectionOption _dBConnectionOption = null;

        public IDbConnection _dbConnection = null;

        //负载均衡的数量
        private static int _iSeed = 0;
        private static bool _isSet = true;
        private static readonly object locker = new object();

        public CustomConnectionFactory(IOptions<DBConnectionOption> dBConnectionOption, IDbConnection dbConnection)
        {
            _dBConnectionOption = dBConnectionOption.Value;
            _dbConnection = dbConnection;

            if (_isSet)
            {
                lock (locker)
                {
                    if (_isSet)
                    {
                        _iSeed = _dBConnectionOption.ReadConnectionList.Count;
                        _isSet = false;
                    }
                }
            }
        }

        public IDbConnection GetConnection(DBExcuteOption dBExcuteOption)
        {
            switch (dBExcuteOption)
            {
                case DBExcuteOption.Write: _dbConnection.ConnectionString = _dBConnectionOption.WriteConnection; break;
                case DBExcuteOption.Read: _dbConnection.ConnectionString = this.QueryStrategy(); break;
                default: break;
            }

            return _dbConnection;
        }


        private string QueryStrategy()
        {
            switch (_dBConnectionOption.Strategy)
            {
                case Strategy.Polling: return this.Polling();
                case Strategy.Random: return this.Random();
                default: throw new Exception("分库查询策略不存在");
            }
        }

        private string Random()
        {
            int Count = _dBConnectionOption.ReadConnectionList.Count;
            int index = new Random().Next(0, Count);
            return _dBConnectionOption.ReadConnectionList[index];
        }

        private string Polling()
        {
            return this._dBConnectionOption.ReadConnectionList[_iSeed++ % this._dBConnectionOption.ReadConnectionList.Count];
        }
    }
}
