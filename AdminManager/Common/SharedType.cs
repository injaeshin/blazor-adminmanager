using System.ComponentModel;

namespace AdminManager.Common;

public enum SchedulerType
{
    [Description("-")]
    None = 0,
    [Description("에어드랍")]
    Airdrop = 1,
    [Description("시스템 메시지")]
    SystemMessage = 2,
}

public enum GameType
{
    [Description("-")]
    None = 0,
    [Description("퓨처 헌터")]
    FutureHunter = 1,
    [Description("홀덤")]
    Holdem = 2,
    [Description("이벤트")]
    Event = 3
}

public enum EventResultType
{
    [Description("-")]
    None = 0,
    [Description("대기 중")]
    Waiting = 1,
    [Description("요청 중")]
    Request = 2,
    [Description("성공")]
    Success = 3,
    [Description("실패")]
    Fail = 4,
    [Description("취소")]
    Cancel = 5,
}

public enum MessageType
{
    [Description("즉시")]
    Instantly = 0,
    [Description("예약")]
    Schedule = 1
}

public enum MessageIntervalType
{
    [Description("-")]
    None = 0,
    [Description("1분")]
    OneMinute = 1,
    [Description("5분")]
    FiveMinute = 5,
    [Description("10분")]
    TenMinute = 10,
    [Description("25분")]
    TwentyFiveMinute = 25,
    [Description("30분")]
    ThirtyMinute = 30
}

public enum ProcessStatus
{
    [Description("-")]
    None = 0,
    [Description("대기")]
    Wait = 1,
    [Description("실행중")]
    Progress = 2,
    [Description("완료")]
    Completed = 3,
    [Description("만료")]
    Expired = 4
}

public enum TargetUserType
{
    None = 0,
    [Description("계정")]
    Account = 1,
    [Description("캐릭터")]
    Character = 2
}

public enum TargetCharacterType
{
    [Description("캐릭터 ID")]
    CharacterUID = 2,
    [Description("캐릭터 이름")]
    CharacterName = 3
}

public enum TargetAllType
{
    [Description("계정 UID")]
    AccountUId = 1,
    [Description("캐릭터 UID")]
    CharacterUId = 2,
    [Description("캐릭터 이름")]
    CharacterName = 3
}

public enum CharStatus
{
    [Description("-")]
    None = 0,
    [Description("정상")]
    Active = 1,
    [Description("삭제")]
    Deleted = 2,
    [Description("삭제 대기")]
    DeleteSoon = 3
}

public enum MailState
{
    [Description("미확인")]
    New = 0,
    [Description("읽음")]
    Checked = 1,
    [Description("아이템 획득")]
    Rewarded = 2,
    [Description("삭제")]
    Deleted = 3,
}

public enum QuestStatus
{
    [Description("-")]
    Wait = 0,
    [Description("진행 중")]
    Progress = 1,
    [Description("완료")]
    Completed = 2,
    [Description("보상 완료")]
    Reward = 3
}

public enum LogCateType
{
    None = 0,

    Account = 500,  // 계정
    Tier = 1000,    // 티어
    Character = 1500,    // 캐릭터
    Item = 2000,    // 아이템
    Currency = 2500,    // 재화
    Wallet = 3000,    // 지갑
    Shop = 3500,    // 상점
    Quest = 4000,    // 퀘스트
    Mission = 4100,    // 미션
    Tutorial = 4500,    // 튜토리얼
    Mail = 5000,    // 우편
    Friends = 5500,    // 친구
    QuickSlot = 6500,    // 퀵 슬롯
    Skill = 7000,    // 스킬
    Cheat = 7500,    // 치트
    Expel = 8000,    // 강제 종료
    Jackpot = 8500,    // 잭팟
    Airdrop = 9000,    // 에어드랍
    Coin = 9100,       // 코인

    NoOneEscape = 20000,    // 플랫폼 게임 (유저)
    NoOneEscapeRecord = 20100,    // 플랫폼 게임 기록 (서버)
    HeadAndTail = 20200,    // 동전 던지기 (유저)
    HeadAndTailRecord = 20300,    // 동전 던지기 기록 (서버)
    TreasureLottery = 20400,    // 보물 상자 (유저)
    TreasureLotteryRecord = 20500,    // 보물 상자 기록 (서버)
    Holdem = 20600,    // 홀덤 포커 (유저)
    HoldemRecord = 20700,    // 홀덤 포커 기록 (서버)w
}

public enum LogFilterType
{
    None = 0,
    [Description("계정")]
    Account = LogCateType.Account,
    [Description("계정 생성")]
    AccountCreate,
    AccountCreateFailure,
    [Description("로그인")]
    Login,
    LoginFailure,
    [Description("재 연결")]
    Reconnect,
    ReconnectFailure,
    [Description("로그 아웃")]
    Logout,
    LogoutFailure,
    [Description("강제 종료(Kick)")]
    Expel,
    ExpelFailure,
    [Description("티어")]
    Tier = LogCateType.Tier,
    [Description("티어 경험치 변경")]
    TierExpUpdate,
    TierExpUpdateFailure,
    [Description("티어 등급 변경 성공")]
    TierGradeup,
    TierGradeupFailure,
    [Description("캐릭터")]
    Character = LogCateType.Character,
    [Description("캐릭터 생성")]
    CharacterCreate,
    CharacterCreateFailure,
    [Description("캐릭터 삭제")]
    CharacterDelete,
    CharacterDeleteFailure,
    [Description("캐릭터 삭제 취소")]
    CharacterDeleteCancel,
    CharacterDeleteCancelFailure,
    [Description("캐릭터 입장")]
    CharacterEnter,
    CharacterEnterFailure,
    [Description("캐릭터 퇴장")]
    CharacterLeave,
    CharacterLeaveFailure,
    [Description("캐릭터 채널 이동")]
    CharacterMoveChannel,
    CharacterMoveChannelFailure,
    [Description("캐릭터 강제 종료(중복 접속)")]
    CharacterExporting,
    CharacterExportingFailure,
    [Description("아이템")]
    Item = LogCateType.Item,
    [Description("아이템 수량 증가")]
    ItemIncreaseAmount,
    ItemIncreaseAmountFailure,
    [Description("아이템 수량 감소")]
    ItemDecreaseAmount,
    ItemDecreaseAmountFailure,
    [Description("아이템 착용")]
    ItemEquip,
    ItemEquipFailure,
    [Description("아이템 착용 해제")]
    ItemUnequip,
    ItemUnequipFailure,
    [Description("재화")]
    Currency = LogCateType.Currency,
    [Description("재화 증가")]
    CurrencyIncrease,
    CurrencyIncreaseFailure,
    [Description("재화 감소")]
    CurrencyDecrease,
    CurrencyDecreaseFailure,
    [Description("우편")]
    Mail = LogCateType.Mail,
    [Description("우편 획득")]
    MailReceive,
    MailReceiveFailure,
    [Description("우편 삭제")]
    MailDelete,
    MailDeleteFailure,
    [Description("우편 읽음")]
    MailOpen,
    MailOpenFailure,
    [Description("우편 첨부 아이템 획득")]
    MailAcquireItem,
    MailAcquireItemFailure,
    [Description("친구")]
    Friends = LogCateType.Friends,
    [Description("친구 요청")]
    FriendsRequest,
    FriendsRequestFailure,
    [Description("친구 요청 수락")]
    FriendsAccept,
    FriendsAcceptFailure,
    [Description("친구 요청 취소")]
    FriendsCancel,
    FriendsCancelFailure,
    [Description("친구 삭제")]
    FriendsDelete,
    FriendsDeleteFailure,
    [Description("친구 차단")]
    FriendsBlock,
    FriendsBlockFailure,
    [Description("친구 포인트 보내기")]
    FriendsPointSend,
    FriendsPointSendFailure,
    [Description("친구 포인트 받기")]
    FriendsPointRecv,
    FriendsPointRecvFailure,
    [Description("친구 포인트 전체 보내기")]
    FriendsPointSendAll,
    FriendsPointSendAllFailure,
    [Description("상점")]
    Shop = LogCateType.Shop,
    [Description("일반 상점 상품 구매")]
    ShopProductBuy,
    ShopProductBuyFailure,
    [Description("퀘스트")]
    Quest = LogCateType.Quest,
    [Description("퀘스트 수락")]
    QuestAccept,
    QuestAcceptFailure,
    [Description("퀘스트 취소")]
    QuestCancel,
    QuestCancelFailure,
    [Description("퀘스트 진행")]
    QuestProgress,
    QuestProgressFailure,
    [Description("퀘스트 완료")]
    QuestComplete,
    QuestCompleteFailure,
    [Description("퀘스트 보상 획득")]
    QuestReward,
    QuestRewardFailure,
    [Description("스킬")]
    Skill = LogCateType.Skill,
    [Description("스킬 획득")]
    SkillLearn,
    SkillLearnFailure,
    [Description("스킬 레벨업")]
    SkillLevelup,
    SkillLevelupFailure,
    [Description("퀵 슬롯")]
    QuickSlot = LogCateType.QuickSlot,
    [Description("퀵 슬롯 저장")]
    QuickSlotSave,
    QuickSlotSaveFailure,
    [Description("치트")]
    Cheat = LogCateType.Cheat,
    CheatSuccess,
    CheatFailure,
    [Description("지갑")]
    Wallet = LogCateType.Wallet,
    [Description("지갑 연결")]
    WalletConnect,
    WalletConnectFailure,
    [Description("지갑 연결 해제")]
    WalletDisconnect,
    WalletDisconnectFailure,
    [Description("지갑 변경")]
    WalletModify,
    WalletModifyFailure,
    [Description("지갑 출금")]
    WalletOnChainWithdraw,
    WalletOnChainWithdrawFailure,
    [Description("지갑 Nft 출금")]
    WalletOnChainWithdrawNft,
    WalletOnChainWithdrawNftFailure,
    [Description("지갑 확인")]
    WalletOffChainIncome,
    WalletOffChainIncomeFailure,
    [Description("지갑 지출")]
    WalletOffChainExpense,
    WalletOffChainExpenseFailure,
    PlatformGame = LogCateType.NoOneEscape,
    PlatformGameMatchBegin,
    PlatformGameMatchBeginFailure,
    PlatformGameMatchEnd,
    PlatformGameMatchEndFailure,
    PlatformGameMatchCancel,
    PlatformGameMatchCancelFailure,
    PlatformGameBegin,
    PlatformGameBeginFailure,
    PlatformGameEnd,
    PlatformGameEndFailure,
    PlatformGameResult,
    PlatformGameResultFailure,
    PlatformGameExit,
    PlatformGameExitFailure,
    GameRecord = LogCateType.NoOneEscapeRecord,
    GameMatchBegin,
    GameMatchBeginFailure,
    GameMatchEnd,
    GameMatchEndFailure,
    GameBegin,
    GameBeginFailure,
    GameEnd,
    GameEndFailure,
    GameSeasonBegin,
    GameSeasonBeginFailure,
    GameSeasonEnd,
    GameSeasonEndFailure,
    TreasureLottery = LogCateType.TreasureLottery,
    TreasureLotteryBegin,
    TreasureLotteryBeginFailure,
    TreasureLotteryEnd,
    TreasureLotteryEndFailure,
    TreasureLotteryBonusPoint,
    TreasureLotteryBonusPointFailure,
    TreasureLotteryBonusPointReward,
    TreasureLotteryBonusPointRewardFailure,
    TreasureLotteryRecord = LogCateType.TreasureLotteryRecord,
    TreasureLotteryRecordSeasonBegin,
    TreasureLotteryRecordSeasonBeginFailure,
    TreasureLotteryRecordSeasonEnd,
    TreasureLotteryRecordSeasonEndFailure,
    TreasureLotteryRecordJackpot,
    TreasureLotteryRecordJackpotFailure,
    HeadsAndTails = LogCateType.HeadAndTail,
    HeadsAndTailsBegin,
    HeadsAndTailsBeginFailure,
    HeadsAndTailsEnd,
    HeadsAndTailsEndFailure,
    HeadsAndTailsResult,
    HeadsAndTailsResultFailure,
    [Description("홀덤")]
    Holdem = LogCateType.Holdem,
    [Description("홀덤 룸 생성")]
    HoldemCreateRoom,
    [Description("홀덤 룸 생성 실패")]
    HoldemCreateRoomFailure,
    [Description("홀덤 룸 파괴")]
    HoldemRemoveRoom,
    [Description("홀덤 룸 파괴 실패")]
    HoldemRemoveRoomFailure,
    [Description("홀덤 입장")]
    HoldemBegin,
    [Description("홀덤 입장 실패")]
    HoldemBeginFailure,
    [Description("홀덤 종료")]
    HoldemEnd,
    [Description("홀덤 종료 실패")]
    HoldemEndFailure,
    [Description("프리플랍")]
    HoldemPreFlop,
    [Description("플랍")]
    HoldemFlop,
    [Description("턴")]
    HoldemTurn,
    [Description("리버")]
    HoldemRiver,
    [Description("쇼다운")]
    HoldemShowdown,
    [Description("쇼다운 실패")]
    HoldemShowdownFailure,
    [Description("폴드 윈")]
    HoldemFoldWin,
    HoldemFoldWinFailure,
    [Description("홀덤 나가기")]
    HoldemExit,
    [Description("홀덤 나가기 실패")]
    HoldemExitFailure,
    [Description("홀덤 나가기 예약")]
    HoldemReserveExit,
    HoldemReserveExitFailure,
    [Description("홀덤 나가기 예약 취소")]
    HoldemReserveExitCancel,
    HoldemReserveExitCancelFailure,
    [Description("홀덤 배팅")]
    HoldemBetting,
    [Description("홀덤 배팅 실패")]
    HoldemBettingFailure,
    [Description("홀덤 폴드")]
    HoldemFold,
    [Description("홀덤 폴드 실패")]
    HoldemFoldFailure,
    Coin = LogCateType.Coin,
    [Description("코인 가격 갱신")]
    CoinRefresh,
}
public enum LogReasonType
{
    [Description("None")]
    None = 0,
    [Description("실패 ")]
    Fail,
    [Description("GDT 찾을 수 없음")]
    NotFoundGDT,
    [Description("찾을 수 없음")]
    NotFound,
    [Description("채널 탐색 불가")]
    NotFoundChannel,
    [Description("채널 입장 불가")]
    ChannelStateFull,
    [Description("중복 로그인")]
    DuplicateLogin,
    [Description("사용 불가")]
    NotAvailable,
    [Description("유효하지 않은 슬롯")]
    InvalidSlot,
    [Description("슬롯 부족")]
    NotEnoughSlot,
    [Description("아이템 부족")]
    NotEnoughItem,
    [Description("아이템 장착")]
    ItemEquip,
    [Description("장착 아이템 아님")]
    NotEquipItem,
    [Description("장착 아님")]
    NotEquip,
    [Description("장착 실패")]
    FailEquip,
    [Description("해제 실패")]
    FailUnequip,
    [Description("재 사용 대기")]
    CoolTime,
    [Description("진행 중")]
    InProgress,
    [Description("이미 완료됨")]
    Completed,
    [Description("시간 종료")]
    Timeout,
    [Description("중복 이름")]
    DuplicateCharacterName,
    [Description("초기화 실패")]
    InitializeFail,
    [Description("삭제한 캐릭터")]
    DeleteCharacter,
    [Description("상품 구매")]
    ProductPurchase,
    [Description("재화 구매")]
    CurrencyPurchase,
    [Description("재화 부족")]
    NotEnoughCurrency,
    [Description("시즌 종료 보상")]
    SeasonCompleteReward,
    [Description("플랫폼 게임 - 멀티 모드 입장")]
    GameModeMultiEnter,
    [Description("플랫폼 게임 - 멀티 모드 랭킹 보상")]
    GameModeMultiRankReward,
    [Description("플랫폼 게임 싱글 모드 입장")]
    GameModeSingleEnter,
    [Description("플랫폼 게임 싱글 모드 보상")]
    GameModeSingleReward,
    [Description("우편 첨부 아이템 획득")]
    TakeMailItem,
    [Description("티어 등급 상향 보상")]
    TierGradeupReward,
    [Description("스킬 레벨 증가")]
    SkillLevelup,
    [Description("패키지 아이템 오픈")]
    ItemBundleOpen,
    [Description("아이템 사용")]
    ItemUse,
    [Description("아이템 성장")]
    ItemGrowth,
    [Description("아이템 합성")]
    ItemSynthesis,
    [Description("아이템 옵션 추가")]
    ItemOptionAdd,
    [Description("액자 렌탈")]
    ItemFrameRental,
    [Description("맵 아이디 체크")]
    MapIDCheck,
    [Description("퀘스트 보상")]
    QuestRewardComplete,
    [Description("퀘스트 보상 메일")]
    QuestRewardMail,
    [Description("퀘스트 보상 실패")]
    QuestNotReward,
    [Description("퀘스트 코인 보상 실패")]
    QuestNotRewardCoin,
    [Description("퀘스트 NPC 수락")]
    QuestAcceptNpc,
    [Description("퀘스트 NPC 위치")]
    QuestNpcPosition,
    [Description("퀘스트 NPC 맵")]
    QuestNpcMap,
    [Description("퀘스트 NPC ID")]
    QuestNpcCharacterID,
    [Description("퀘스트 수락 존관련")]
    QuestAcceptEntry,
    [Description("퀘스트 수락 시나리오 대화 관련")]
    QuestAcceptScenarioScene,
    [Description("보상이 없는 퀘스트")]
    EmptyRewardQuest,
    [Description("이미 수락된 퀘스트")]
    AcceptedQuest,
    [Description("존 타입 오류")]
    InvalidZone,
    [Description("타입 오류")]
    InvalidType,
    [Description("선행 퀘스트 필요")]
    RequirementsQuest,
    [Description("미 완료")]
    NotComplete,
    [Description("레벨 부족")]
    NotEnoughLevel,
    [Description("레벨 초과")]
    ExceedLevel,
    [Description("퀘스트의 태스크가 없음")]
    NotFoundTask,
    [Description("유효하지 않은 상태")]
    InvalidState,
    [Description("미 오픈")]
    NotOpen,
    [Description("차단됨")]
    Blocked,
    [Description("대기")]
    Waiting,
    [Description("초과")]
    Exceeded,
    [Description("오늘 포인트 초과")]
    TodayPointExceeded,
    [Description("오늘 포인트 초과")]
    PointExceeded,
    [Description("포인트 초과")]
    TargetExceeded,
    [Description("친구 가 아님")]
    NotFriends,
    [Description("스킬을 찾을 수 없음")]
    NotFoundSkill,
    [Description("배우기 실패")]
    LearnFail,
    [Description("증가 실패")]
    NotIncrease,
    [Description("티어 찾을 수 없음")]
    NotFoundTier,
    [Description("티어가 충분하지 않음")]
    NotEnoughTier,
    [Description("캐릭터 우편 첨부 코인 획득")]
    CMailTakeCoin,
    [Description("계정 우편 첨부 코인 획득")]
    AMailTakeCoin,
    [Description("묶음 아이템 오픈(코인)")]
    ItemBundleOpenCoin,
    [Description("코인 구매")]
    ProductPurchaseCoin,
    [Description("운영툴 전체 메일")]
    AdminAllMail,
    [Description("운영툴 계정 개별 발송")]
    AdminTargetMailA,
    [Description("운영툴 캐릭터 개별 발송")]
    AdminTargetMailC,
    [Description("싱글 게임 시간 초과")]
    GameModeSingleEnterTimeOut,
    [Description("멀티 게임 시간 초과")]
    GameModeMultiEnterTimeOut,
    [Description("오브젝트 획득")]
    ConsumeObject,

    [Description("워프 비용")]
    WarpFee,
    [Description("친구 포인트 보냄")]
    FriendsPointSend,
    [Description("친구 포인트 찾을 수 없음")]
    FriendsPointNotFound,

    [Description("보물복권 비용")]
    TreasureLotteryCost,
    [Description("보물복권 보상")]
    TreasureLotteryReward,
    [Description("보물복권 보너스 보상")]
    TreasureLotteryBonusPointReward,
    [Description("보물복권 플레이")]
    TreasureLotteryPlay,

    [Description("동전 던지기 플레이")]
    HeadsAndTailsPlay,
    [Description("동전 던지기 보상")]
    HeadsAndTailsReward,
    [Description("동전 던지기 비용")]
    HeadsAndTailsCost,
    [Description("동전 던지기 선택 안함")]
    HeadsAndTailsNonePick,
    [Description("동전 던지기 승리")]
    HeadsAndTailsWin,
    [Description("동전 던지기 패배")]
    HeadsAndTailsLose,

    [Description("홀덤 플레이")]
    HoldemPlay,
    [Description("홀덤 카드 배분")]
    HoldemPreFlopDistribution,
    [Description("홀덤 무료 리필")]
    HoldemFreeRefill,
    [Description("홀덤 승리 보상")]
    HoldemWinReward,
    [Description("홀덤 잭팟 승리 보상")]
    HoldemWinJackpotReward,
    [Description("홀덤 Fold 승리 보상")]
    HoldemFoldWinReward,
    [Description("홀덤 배팅 오류")]
    HoldemNotBetOrder,
    [Description("홀덤 Ante로 사용")]
    HoldemAnte,

    [Description("홀덤 베팅")]
    HoldemBetting,
    [Description("홀덤 배팅 시간 만료")]
    HoldemBettingExpired,
    [Description("홀덤 배팅 타입 오류")]
    HoldemInvalidBetType,
    [Description("홀덤 올인 으로 인한 쇼 다운")]
    HoldemAllInShowdown,
    [Description("홀덤 승리 플레이어 없음")]
    HoldemNoWinner,
    [Description("홀덤 플레이어 체크 오류")]
    HoldemNoUser,
    [Description("홀덤 최소 금액 부족")]
    HoldemNotEnoughMinCash,
    [Description("홀덤 나가기 예약 완료 처리")]
    HoldemReserveExit,
    [Description("홀덤 플레이어 요청 처리")]
    HoldemUserRequest,
    [Description("홀덤 방 없음")]
    HoldemNoneRoom,

    [Description("콘텐츠 잠김")]
    ContentLocked,
    [Description("NFT 인출")]
    WithdrawNft,
    [Description("NFT 아이템 증가")]
    SpendingNft,
    [Description("NFT 를 게임 아이템으로 변경")]
    NftToGameItem,
    [Description("신규 지갑 연결")]
    NewWalletConnect,
    [Description("연결 기록이 있는 지갑 연결")]
    WalletConnect,

    [Description("코인 상점")]
    CoinShop,
    [Description("일반 상점")]
    Shop,

    Cheat = 10000,
}
