# Redis-Pub-Sub-Lab

## Redis

Start Redis locally with Docker:

	docker run --rm -it -p 6379:6379 redis

## [What is ConnectionMultiplexer](https://www.thecodebuzz.com/redis-dependency-injection-connectionmultiplexer-redis-cache-netcore-csharp/)

- ConnectionMultiplexer StackExchange’s principal object has multipurpose usage like accessing the Redis database and letting you perform read, write, update or delete operations, provide pub/sub features, etc.
- It is a thread-safe and ready-to-use application. All of the following examples will presume you have a ConnectionMultiplexer instance saved for later usage.
- The ConnectionMultiplexer is designed to be shared and reused between callers.
- Per operation, you should not establish a ConnectionMultiplexer.
- It recommends sharing and reusing the ConnectionMultiplexer object. In the below example, we are creating a singleton instance and using it for all request processing.

## Links

- [Redis Pub/Sub](https://redis.io/docs/manual/pubsub/)
- [Creating a Very Simple Console Chat App using C# and Redis Pub/Sub](https://www.codeproject.com/Articles/1222027/Creating-a-Very-Simple-Console-Chat-App-using-Csha)
- [Redis-Dependency Injection of the ConnectionMultiplexer – Best Practices](https://www.thecodebuzz.com/redis-dependency-injection-connectionmultiplexer-redis-cache-netcore-csharp/)
- [Publish and Subscribe messages with Redis – C# .NET Core](https://www.thecodebuzz.com/redis-publish-subscribe-messages-redis-net-core-csharp/)