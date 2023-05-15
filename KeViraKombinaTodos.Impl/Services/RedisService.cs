using KeViraKombinaTodos.Core;
using KeViraKombinaTodos.Core.Services;
using StackExchange.Redis;
using System;

namespace KeViraKombinaTodos.Impl.Services
{
    [Component]
    public class RedisService : IRedisService
    {

        #region Private Read-Only Fields

        private IDatabase _db;

        #endregion

        private void ConfigureRedis()
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379, allowAdmin=true, ConnectTimeout = 30000, connectRetry = 5, syncTimeout = 30000, abortConnect = false");
                _db = redis.GetDatabase();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        #region Public Constructors

        public RedisService()
        {
            ConfigureRedis();
        }

        #endregion

        #region Members
        public bool IsKeyExists(string key)
        {
            if (_db.KeyExists(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetStrings(string key, string value)
        {
            _db.StringSet(key, value);
        }

        public string GetStrings(string key)
        {
            var result = string.Empty;

            try
            {                 
                result = _db.StringGet(key, CommandFlags.DemandMaster);               
            }
            catch (Exception)
            {
            }

            return result;
        }

        public long Increment(string key)
        {
            return _db.HashIncrement(key, 1);
        }

        public long Decrement(string key)
        {
            return _db.HashDecrement(key, 1);
        }

        #endregion
    }
}