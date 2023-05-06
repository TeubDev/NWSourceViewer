using Microsoft.AspNetCore.Components;
using NWSourceViewer.Models;
using NWSourceViewer.Services;

namespace NWSourceViewer.Components;

public class DataComponent : ComponentBase
{
    [Inject]
    protected NavigationManager NavManager { get; set; } = default!;
    [Inject]
    protected IFileLoader FileLoader { get; set; } = default!;
    [Inject]
    protected Config Config { get; set; } = default!;
    protected TlkDictionary? Tlk { get; set; } = null;

    protected virtual bool IsLoaded { get { return Tlk != null; } }

    protected async Task LoadTlk(CancellationToken cancellationToken)
    {
        Tlk = await FileLoader.LoadTlkAsync(cancellationToken);
    }

    protected static string GetYesNo(bool value)
    {
        if (value)
        {
            return "Yes";
        }
        return "No";
    }

    protected void GoToFeat(uint Id)
    {
        NavManager.NavigateTo($"feats/{Id}");
    }

    protected void GoToClass(uint Id)
    {
        NavManager.NavigateTo($"classes/{Id}");
    }

    protected void GoToRace(uint Id)
    {
        NavManager.NavigateTo($"races/{Id}");
    }

    protected void GoToSpell(uint Id)
    {
        NavManager.NavigateTo($"spells/{Id}");
    }
}
