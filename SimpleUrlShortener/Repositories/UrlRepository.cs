using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleUrlShortener.Repositories
{
    public class UrlRepository
    {
        private readonly IDatabase _conn;
        private readonly IServer _server;
        public UrlRepository()
        {
            ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect("localhost:6379");
            _server = muxer.GetServer("localhost", 6379);
            _conn = muxer.GetDatabase();
        }

        public void Insert(string key, string url)
        {
            _conn.StringSet(key, url);
        }

        public string Get(string key)
        {
            return _conn.StringGet(key);
        }

        public List<string> GetAllKeys()
        {
            var result = new List<string>();

            foreach(var key in _server.Keys())
            {
                result.Add(key);
            }

            return result;
        }
    }
}
