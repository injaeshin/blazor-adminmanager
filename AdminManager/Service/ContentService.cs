using GDT;

namespace AdminManager.Service
{
    public class ContentService
    {
        private readonly IRedisService _redisService;
        private readonly Dictionary<GameModeValue, bool> _contentsLock = new();

        public event Action OnResponseContentsLock;
        public event Action<bool> OnResponseContentLockSwitch;

        public ContentService(IServiceProvider serviceProvider)
        {
            _redisService = serviceProvider.GetRequiredService<IRedisService>();

            Init();
        }

        private void Init()
        {
            _redisService.Stub.Res_ContentsLock = (res) =>
            {
                foreach (var content in res.contents)
                {
                    if (!Enum.IsDefined(typeof(GameModeValue), content.TypeId))
                        continue;
                    
                    var typeId = (GameModeValue)content.TypeId;
                    _contentsLock[typeId] = (content.IsLock == 1);
                }

                OnResponseContentsLock?.Invoke();
                
                return true;
            };

            _redisService.Stub.Res_ContentLockSwitch = (res) =>
            {
                if (!Enum.IsDefined(typeof(GameModeValue), res.typeId))
                    return true;

                if (!_contentsLock.ContainsKey((GameModeValue)res.typeId))
                    return true;

                var typeId = (GameModeValue)res.typeId;
                _contentsLock[typeId] = (res.isOpen == 0);

                OnResponseContentLockSwitch?.Invoke(_contentsLock[typeId]);

                return true;
            };
        }

        public Task<bool> GetContentLock(GameModeValue typeId)
        {
            if (_contentsLock.TryGetValue(typeId, out var isLock))
                return Task.FromResult(isLock);

            return Task.FromResult(false);
        }

        public Task<Dictionary<GameModeValue, bool>> GetContentsLock()
        {
            return Task.FromResult(_contentsLock);
        }

        public void ReqContentsStatus() => _redisService.ReqContentsStatus();
        public void UpdateSwitchContent(GameModeValue typeId, bool isLock) => _redisService.ReqContentSwitch(typeId, isLock);

    }
}
