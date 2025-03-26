using AdminManager.Common;
using Radzen;

namespace AdminManager.Components.Pages
{
    public partial class EventAirdrop
    {
        //  텍스트 박스 Value, Disable 상태 관리
        public class AirdropTextBoxView
        {
            public event Action<long> UseAmountChanged;
            public event Action<long> CancelAmountChanged;
            
            private long _collectAirdropAmount;
            private long _useAirdropAmount;
            private long _cancelAmount;

            private double _usePercentage;

            public long CollectAirdropAmount
            {
                get => _collectAirdropAmount;
                set => _collectAirdropAmount = value;
            }

            public double UsePercentage
            {
                get => _usePercentage;
                set => _usePercentage = value;
            }

            public long UseAirdropAmount
            {
                get => _useAirdropAmount;
                set
                {
                    _useAirdropAmount = value;
                    UseAmountChanged?.Invoke(value);
                }
            }

            public long CancelAmount
            {
                get => _cancelAmount;
                set
                {
                    _cancelAmount = value;
                    CancelAmountChanged?.Invoke(value);
                }
            }

            public const bool IsDisableCollectAirdropAmount = true;
            public bool IsDisableUsePercentage = true;
            public bool IsDisableUseAirdropAmount = true;
            public bool IsDisableCancelAmount = true;
            public bool IsDisableBeginDateTime = true;

            public void Clear()
            {
                _collectAirdropAmount = 0;
                _useAirdropAmount = 0;
                UsePercentage = 0;
                _cancelAmount = 0;
            }

            public void SetAllDisable()
            {
                IsDisableUsePercentage = true;
                IsDisableUseAirdropAmount = true;
                IsDisableCancelAmount = true;
                IsDisableBeginDateTime = true;
            }

            public void SetAllEnable()
            {
                IsDisableUsePercentage = false;
                IsDisableUseAirdropAmount = false;
                IsDisableCancelAmount = false;
                IsDisableBeginDateTime = false;
            }
        }

        private bool IsGameTypeValidation()
        {
            if (_airdropEvent.EventGameType == GameType.None)
            {
                NotificationService.ShowMessage(NotificationSeverity.Error, "타입 선택 필요!", "게임 타입을 선택해주세요");
                return false;
            }

            if (_airdropEvent.EventGameType != GameType.Event && _airdropTextBoxView.CollectAirdropAmount == 0)
            {
                NotificationService.ShowMessage(NotificationSeverity.Error, "수집 금액 필요!", "수집 금액을 확인해주세요");
                return false;
            }

            return true;
        }

        private bool IsInputValidation()
        {
            switch (_airdropTextBoxView.UseAirdropAmount)
            {
                case > int.MaxValue:
                    NotificationService.ShowMessage(NotificationSeverity.Error, "금액 초과!", "금액이 너무 큽니다");
                    return false;
                case 0:
                    NotificationService.ShowMessage(NotificationSeverity.Error, "사용 금액 필요!", "사용 금액을 확인해주세요");
                    return false;
            }

            if (_airdropEvent.Note.Trim() == string.Empty)
            {
                NotificationService.ShowMessage(NotificationSeverity.Error, "메모 필요!", "메모를 입력해주세요");
                return false;
            }

            if (_airdropEvent.BeginDateTime == DateTime.MinValue)
            {
                NotificationService.ShowMessage(NotificationSeverity.Error, "날짜 선택 필요!", "날짜를 선택해주세요");
                return false;
            }

            if (_airdropEvent.BeginDateTime < DateTime.Now.AddMinutes(5))
            {
                NotificationService.ShowMessage(NotificationSeverity.Error, "날짜 확인 필요!", "날짜를 현재 시간보다 5분 이상으로 설정해주세요");
                return false;
            }

            return true;
        }
    }
}
