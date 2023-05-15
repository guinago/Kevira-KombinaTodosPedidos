using System;

namespace KeViraKombinaTodos.Core.Services
{
    public interface IRedisService {

        #region Methods
        bool IsKeyExists(string key);
        void SetStrings(string key, string value);
        string GetStrings(string key);        
        long Increment(string key);
        long Decrement(string key);

        #endregion
    }
}