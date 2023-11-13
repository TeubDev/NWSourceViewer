using NWSourceViewer.Models;
using Polly;

namespace NWSourceViewer.Services;

/// <summary>
/// For loading custom content files.
/// </summary>
public interface IFileLoader
{
    /// <summary>
    /// Gets a 2da file with the given name.
    /// </summary>
    /// <remarks>
    /// First attempts to load the 2da file from the custom files. If it was not found, this will attempt to
    /// load it from the default NWN source files. If it was not found there, returns null.
    /// </remarks>
    /// <param name="fileName">The name of the file without the .2da extension.</param>
    /// <param name="converter">For converting from an entry in the file to the T.</param>
    /// <param name="cancellationToken">For cancelling the request.</param>
    Task<List<T>?> Load2daAsync<T>(string fileName, CancellationToken cancellationToken) where T : Base2daRowModel, new();

    /// <summary>
    /// Calls <see cref="Load2daAsync{T}(string, CancellationToken)"/> and then attempts to fetch a single row.
    /// </summary>
    /// <remarks>
    /// If something went wrong (table not loaded, row ID out of range, etc.), returns null.
    /// </remarks>
    Task<T?> Load2daRowAsync<T>(string fileName, uint rowId, CancellationToken cancellationToken) where T : Base2daRowModel, new();

    /// <summary>
    /// Loads the TLK file(s). If they didn't load properly, the result will be empty.
    /// </summary>
    Task<TlkDictionary> LoadTlkAsync(CancellationToken cancellationToken);
}

/// <inheritdoc cref="IFileLoader"/>
public class FileLoader : IFileLoader
{
    private readonly HttpClient httpClient;
    private readonly IAsyncPolicy cachePolicy;
    private readonly Config config;

    public FileLoader(HttpClient httpClient, IAsyncPolicy cachePolicy, Config configuration)
    {
        this.httpClient = httpClient;
        this.cachePolicy = cachePolicy;
        this.config = configuration;
    }

    public async Task<List<T>?> Load2daAsync<T>(string fileName, CancellationToken cancellationToken) where T : Base2daRowModel, new()
    {
        return await cachePolicy.ExecuteAsync(async context =>
        {
            if (fileName == Constants.NullString)
            {
                return null;
            }
            var tlkTask = LoadTlkAsync(cancellationToken);
            var response = await httpClient.GetAsync($"source/{fileName.ToLowerInvariant()}.2da", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                response = await httpClient.GetAsync($"default-source/{fileName.ToLowerInvariant()}.2da", cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
            }
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var tlk = await tlkTask;

            string[]? rows = responseBody.Replace("\r", "").Split("\n");
            var headers = rows[2].Split2daRow();
            headers.Insert(0, "Index");
            var data = new List<T>();
            for (uint rowIndex = 3; rowIndex < rows.Length; rowIndex++)
            {
                if (!string.IsNullOrWhiteSpace(rows[rowIndex]))
                {
                    var row = rows[rowIndex].Split2daRow();
                    Dictionary<string, string> dataRow = new();
                    for (int colIndex = 0; colIndex < headers.Count; colIndex++)
                    {
                        dataRow[headers[colIndex]] = row[colIndex];
                    }
                    var typedRow = new T();
                    typedRow.ConvertData(dataRow, tlk);
                    if ((typedRow.HasData || config.IncludeEmpty2daRows)
                        && (typedRow.UseableForPlayers || config.IncludeUnuseableForPlayers2daRows))
                    {
                        data.Add(typedRow);
                    }
                }
            }
            return data;
        }, new Context(fileName + " 2da"));
    }

    public async Task<T?> Load2daRowAsync<T>(string fileName, uint rowId, CancellationToken cancellationToken) where T : Base2daRowModel, new()
    {
        var table = await Load2daAsync<T>(fileName, cancellationToken);
        return table?.FirstOrDefault(r => r.HasData && r.Index == rowId);
    }

    public async Task<TlkDictionary> LoadTlkAsync(CancellationToken cancellationToken)
    {
        return await cachePolicy.ExecuteAsync(async context =>
        {
            var dialogResponse = await httpClient.GetAsync($"default-source/dialog.tlk", cancellationToken);
            var dialogResponseBody = await dialogResponse.Content.ReadAsByteArrayAsync(cancellationToken);
            var tlkEntries = new TlkDictionary();
            tlkEntries.AddTlkFile(dialogResponseBody);
            var customResponse = await httpClient.GetAsync($"source/{config.TlkFileName}", cancellationToken);
            if (customResponse.IsSuccessStatusCode)
            {
                var customResponseBody = await customResponse.Content.ReadAsByteArrayAsync(cancellationToken);
                tlkEntries.AddTlkFile(customResponseBody, 16777216);
            }
            return tlkEntries;
        }, new Context("Tlk"));

    }
}
