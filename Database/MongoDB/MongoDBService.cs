using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using ran_product_management_net.Database.Mongodb.Models;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

namespace ran_product_management_net.Database.Mongodb;

public class MongoDBService
{
    private readonly IConfiguration _conf;
    private readonly IMongoDatabase _database;

    public MongoDBService(IConfiguration conf)
    {
        _conf = conf;

        var connectionString = _conf.GetConnectionString("MongoDBConn");
        var mongoUrl = MongoUrl.Create(connectionString);

        var mongoClient = new MongoClient(mongoUrl);
        _database = mongoClient.GetDatabase(_conf.GetConnectionString("MongoDBDatabase"));

        // Register serializers
        BsonClassMap.RegisterClassMap<ProductDetailBase>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true); // Mark as root for polymorphism
        });
        BsonClassMap.RegisterClassMap<Smartphone>(cm => 
        {
            cm.AutoMap();
            cm.SetDiscriminator("Smartphone");
        });
        BsonClassMap.RegisterClassMap<Fashion>(cm => 
        {
            cm.AutoMap();
            cm.SetDiscriminator("Fashion");
        }); 
        BsonClassMap.RegisterClassMap<Electronic>(cm => {
            cm.AutoMap();
            cm.SetDiscriminator("Electronic");
        });
    }

    public IMongoDatabase? Database => _database;
}
