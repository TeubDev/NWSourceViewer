using NWSourceViewer.Models;

namespace NWSourceViewer.Components;

public abstract class DataRowComponent<T> : DataComponent where T : Base2daRowModel, new()
{
    protected T? Data { get; set; } = default;

    protected override bool IsLoaded { get { return base.IsLoaded && Data != null; } }

    protected async Task LoadDataAsync(string tableName, int rowIndex, CancellationToken cancellationToken)
    {
        var tableTask = FileLoader.Load2daAsync<T>(tableName, cancellationToken);
        var tlkTask = LoadTlk(cancellationToken);
        await Task.WhenAll(tableTask, tlkTask);
        var table = await tableTask;
        if (table != null)
        {
            Data = table.ToList()[rowIndex];
        }
    }
}
