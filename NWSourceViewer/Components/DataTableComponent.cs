using NWSourceViewer.Models;
using Radzen;

namespace NWSourceViewer.Components;

public abstract class DataTableComponent<T> : DataComponent where T : Base2daRowModel, new()
{
    protected IEnumerable<T>? Data { get; private set; } = null;

    protected override bool IsLoaded { get { return base.IsLoaded && Data != null; } }

    protected async Task LoadDataAsync(string tableName, CancellationToken cancellationToken)
    {
        var tableTask = FileLoader.Load2daAsync<T>(tableName, cancellationToken);
        var tlkTask = LoadTlk(cancellationToken);
        await Task.WhenAll(tableTask, tlkTask);
        var tableResult = await tableTask;
        if (tableResult != null)
        {
            Data = tableResult.AsODataEnumerable();
        }
    }
}
