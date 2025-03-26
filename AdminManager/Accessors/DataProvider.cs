using MySqlConnector;
using System.Data;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Linq;

namespace AdminManager.Accessors
{
    public abstract class Provider
    {
        protected readonly IConfiguration _configuration;

        public Provider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }

    public class DapperProvider : Provider
    {
        public enum DB
        {
            Admin,
            Game,
            User
        }

        private readonly Dictionary<DB, string> _connStrings = new();

        public DapperProvider(IConfiguration configuration) : base(configuration)
        {
            _connStrings[DB.Admin] = _configuration.GetConnectionString("AdminDB");
            _connStrings[DB.Game]  = _configuration.GetConnectionString("GameDB");
            _connStrings[DB.User]  = _configuration.GetConnectionString("UserDB");
        }

        public IDbConnection Open(DB db)
        {
            return !_connStrings.TryGetValue(db, out var connString) ? null : new MySqlConnection(connString);
        }
    }

    public class MongoProvider : Provider
    {
        private readonly string _connString;
        private const string _dbName = "user";
        private readonly string _userName;
        private readonly string _password;

        public MongoProvider(IConfiguration configuration) : base(configuration)
        {
            try
            {
                _connString = _configuration.GetConnectionString("MongoDB");

                var section = _configuration.GetSection("Auth");
                _userName = section["MongoUsr"];
                _password = section["MongoPwd"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IMongoDatabase Open()
        {
            var settings = MongoClientSettings.FromConnectionString(_connString);
            settings.LinqProvider = LinqProvider.V3;
            settings.UseTls = false;

            MongoIdentity identity = new MongoInternalIdentity(_dbName, _userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(_password);
            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            var client = new MongoClient(settings);
            return client.GetDatabase(_dbName);
        }
    }

    public class GDTProvider : Provider
    {
        private readonly string _filePath = default!;

        public GDTProvider(IConfiguration configuration) : base(configuration)
        {
            _filePath = _configuration["GameDataTable"];
        }

        public CommonGDTManager Open()
        {
            if (!File.Exists(_filePath))
                return null;

            var gdtManager = new CommonGDTManager();

            try
            {
                var bytes = File.ReadAllBytes(_filePath);
                gdtManager.Init(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return gdtManager;
        }
    }
}