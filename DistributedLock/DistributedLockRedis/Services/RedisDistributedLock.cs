using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace DistributedLockRedis.Services
{
    public class RedisDistributedLock
    {

        private readonly IDatabase _redisDatabase;
        private readonly string _lockKey;
        private readonly string _lockValue;
        private readonly TimeSpan _expiry;

        public RedisDistributedLock(IDatabase redisDatabase, string lockKey, string lockValue, TimeSpan expiry)
        {
            _redisDatabase = redisDatabase;
            _lockKey = lockKey;
            _lockValue = lockValue;
            _expiry = expiry;
        }


        public async Task<bool> AcquireLockAsync()
        {
            // 尝试设置锁，仅当键不存在时才成功
            //使用 Redis 的 SET key value NX EX 命令尝试获取锁。NX 表示仅在键不存在时设置，`EX 设置键的过期时间。
            return await _redisDatabase.StringSetAsync(_lockKey, _lockValue, _expiry, When.NotExists);
        }


        public async Task ReleaseLockAsync()
        {
            // 使用 Lua 脚本确保只有锁的持有者才能释放锁

            //Lua 脚本在 Redis 中是原子执行的，即 Redis 会在脚本执行期间阻塞其他命令，确保脚本中涉及的所有操作（如 GET 和 DEL）是一次性完成的。
            var luaScript = @"
            if redis.call('GET', KEYS[1]) == ARGV[1] then
                return redis.call('DEL', KEYS[1])
            else
                return 0
            end";

            await _redisDatabase.ScriptEvaluateAsync(luaScript, new RedisKey[] { _lockKey }, new RedisValue[] { _lockValue });
        }




    }



}

