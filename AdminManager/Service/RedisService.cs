using _Network;
using AdminShared;
using AdminManager.Common;
using GDT;
using StackExchange.Redis;
using GameType = AdminManager.Common.GameType;

namespace AdminManager.Service
{
    public interface IRedisService
    {
        void Req_Test(int num, string msg);
        Task<int> GetMasterServerIdAsync();

        Task SendAdminMailReloadAsync();
        Task SendAccountAdminMailReloadCommandAsync(List<long> targets);
        Task SendCharacterAdminMailReloadCommandAsync(List<long> targets);

        void ReqNotifyMessage(List<NotifyMessage> notifyMessages);
        void ReqAirdropEvent(GameType gameType, int eventNo, int amount);

        void ReqContentsStatus();

        Game2Admin.Stub Stub { get; }

        void ReqContentSwitch(GameModeValue typeId, bool isLock);
    }

    public class RedisService : IRedisService
    {
        private ConnectionMultiplexer _redisConnection;
        private IDatabase _database;
        private ISubscriber _subscriber;

        private Admin2Game.Proxy _proxy;
        private Game2Admin.Stub _stub;

        public Game2Admin.Stub Stub => _stub;


        public RedisService(IConfiguration configuration)
        {
            var configString = configuration.GetConnectionString("Redis");
            Init(configString);
        }

        private void Init(string connStr)
        {
            ConfigurationOptions opt = ConfigurationOptions.Parse(connStr);
            opt.ClientName = "AdminTool";

            _redisConnection = ConnectionMultiplexer.Connect(opt);
            _database = _redisConnection.GetDatabase();
            _subscriber = _redisConnection.GetSubscriber();

            _proxy = new(_subscriber);
            _proxy.SetDefaultChannel("Admin2Game");

            _stub = new();

            _subscriber.Subscribe(
                new RedisChannel("Game2Admin", RedisChannel.PatternMode.Literal),
                (channel, message) =>
                {
                    try
                    {
                        byte[] buffer = message;
                        _Packet packet = new(ref buffer);
                        _stub.OnDispatch(packet);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });

            _stub.Res_Test = (param) =>
            {
                Console.WriteLine($"{param} {param.num1} {param.str1}");
                return true;
            };
        }

        public async Task<int> GetMasterServerIdAsync()
        {
            var serverId = await _database.StringGetAsync("MasterServerId");
            return int.Parse(serverId);
        }

        // all account
        public void Req_Test(int num, string msg)
        {
            _proxy.Req_Test(num, msg);
        }

        public async Task SendAdminMailReloadAsync()
        {
            _proxy.Send_AdminAllMail_Reload();
            await Task.CompletedTask;
        }

        // target account of character
        public async Task SendAccountAdminMailReloadCommandAsync(List<long> targets)
        {
            _proxy.Send_AdminTargetMail_A_Reload(targets);
            await Task.CompletedTask;
        }

        // target character
        public async Task SendCharacterAdminMailReloadCommandAsync(List<long> targets)
        {
            _proxy.Send_AdminTargetMail_C_Reload(targets);
            await Task.CompletedTask;
        }

        public void ReqNotifyMessage(List<NotifyMessage> notifyMessages) => _proxy.Req_NotifyMessage(notifyMessages);

        public void ReqAirdropEvent(GameType gameType, int eventNo, int amount) => _proxy.Req_Airdrop(gameType.ToEventId(), eventNo, amount);

        public void ReqContentsStatus() => _proxy.Req_ContentsLock();

        public void ReqContentSwitch(GameModeValue typeId, bool isLock) => _proxy.Req_ContentLockSwitch(Convert.ToByte(typeId), Convert.ToByte(isLock));
    }
}
