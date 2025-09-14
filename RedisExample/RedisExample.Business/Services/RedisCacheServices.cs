using RedisExample.Business.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisExample.Business.Services
{
    public class RedisCacheServices : IRedisCacheServices
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabaseAsync _databaseAsync;

        public RedisCacheServices(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _databaseAsync = _connectionMultiplexer.GetDatabase();
        }

        public async Task ClearAsync(string key)
        {
            await _databaseAsync.KeyDeleteAsync(key);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _databaseAsync.StringGetAsync(key);
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            return await _databaseAsync.StringSetAsync(key, value);
        }

        public void ClearAll()
        {
            var redisEndPoints = _connectionMultiplexer.GetEndPoints();
            foreach (var redisEndPoint in redisEndPoints)
            {
                var redisServer = _connectionMultiplexer.GetServer(redisEndPoint);
                redisServer.FlushAllDatabases();
            }
        }
    }
}
