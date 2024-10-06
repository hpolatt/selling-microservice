using System;
using StackExchange.Redis;

namespace BasketService.Api.Extensions;

public static class RedisRegistration
{

    public static ConnectionMultiplexer ConfigureRedis(this IServiceProvider services, IConfiguration configuration)
    {
        var redisConfig = ConfigurationOptions.Parse(configuration["RedisConfig:ConnectionString"], true);
        redisConfig.ResolveDns = true;
        
        return ConnectionMultiplexer.Connect(redisConfig);
    }


}
