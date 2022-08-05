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
    /// Loads the TLK file(s). If they didn't load properly, the result will be empty.
    /// </summary>
    Task<TlkDictionary> LoadTlkAsync(CancellationToken cancellationToken);
}

/// <inheritdoc cref="IFileLoader"/>
public class FileLoader : IFileLoader
{
    private readonly HttpClient httpClient;
    private readonly IAsyncPolicy cachePolicy;
    private readonly IConfiguration configuration;

    public FileLoader(HttpClient httpClient, IAsyncPolicy cachePolicy, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.cachePolicy = cachePolicy;
        this.configuration = configuration;
    }

    public async Task<List<T>?> Load2daAsync<T>(string fileName, CancellationToken cancellationToken) where T : Base2daRowModel, new()
    { // TODO: Convert to returning a dictionary.
        return await cachePolicy.ExecuteAsync(async context =>
        {
            var response = await httpClient.GetAsync($"/source/{fileName}.2da", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                response = await httpClient.GetAsync($"/default-source/{fileName}.2da", cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
            }
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            string[]? rows = responseBody.Split("\r\n");
            var headers = rows[2].Split2daRow();
            headers.Insert(0, "Index");
            var data = new List<T>();
            for (uint rowIndex = 3; rowIndex < rows.Length; rowIndex++)
            {
                if (!string.IsNullOrWhiteSpace(rows[rowIndex]))
                {
                    var row = rows[rowIndex].Split2daRow();
                    Dictionary<string, string> dataRow = new();
                    for (int colIndex = 0; colIndex < row.Count; colIndex++)
                    {
                        dataRow[headers[colIndex]] = row[colIndex];
                    }
                    var typedRow = new T();
                    typedRow.ConvertData(dataRow);
                    data.Add(typedRow);
                }
            }
            return data;
        }, new Context(fileName + " 2da"));
    }

    private async Task<T?> Load2daRowAsync<T>(string fileName, uint RowId, CancellationToken cancellationToken) where T : Base2daRowModel, new()
    {
        var table = await Load2daAsync<T>(fileName, cancellationToken);
        if (table?.Count > RowId)
        {
            return table[(int)RowId];
        }
        return null;
    }

    public async Task<TlkDictionary> LoadTlkAsync(CancellationToken cancellationToken)
    {
        return await cachePolicy.ExecuteAsync(async context =>
        {
            var dialogResponse = await httpClient.GetAsync($"/default-source/dialog.tlk", cancellationToken);
            var dialogResponseBody = await dialogResponse.Content.ReadAsByteArrayAsync(cancellationToken);
            var tlkEntries = new TlkDictionary();
            tlkEntries.AddTlkFile(dialogResponseBody);
            var customResponse = await httpClient.GetAsync($"/source/{configuration["tlkFileName"]}", cancellationToken);
            if (customResponse.IsSuccessStatusCode)
            {
                var customResponseBody = await customResponse.Content.ReadAsByteArrayAsync(cancellationToken);
                tlkEntries.AddTlkFile(customResponseBody, 16777216);
            }
            return tlkEntries;
        }, new Context("Tlk"));

    }
}
