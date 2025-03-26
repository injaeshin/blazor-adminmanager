using AdminManager.Model.Shared;

namespace AdminManager.ModelView;

public class CurrencyModelView : UpdateDateTimeModelView
{
    public short SubType { get; set; }

    public int DefaultAmount { get; set; }

    public int CurrentAmount { get; set; }

    public string SubTypeStr { get; set; } = default!;
}
