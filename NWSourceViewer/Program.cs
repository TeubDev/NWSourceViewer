using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Caching.Memory;
using NWSourceViewer;
using NWSourceViewer.Models;
using NWSourceViewer.Services;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IFileLoader, FileLoader>();
builder.Services.AddScoped<IClassService, ClassService>();
var config = new Config();
builder.Configuration.Bind(config);
builder.Services.AddSingleton(config);

var memoryCache = new MemoryCache(new MemoryCacheOptions());
var memoryCacheProvider = new MemoryCacheProvider(memoryCache);
AsyncCachePolicy cachePolicy = Policy.CacheAsync(memoryCacheProvider, TimeSpan.FromDays(1));
builder.Services.AddScoped<IAsyncPolicy>(s => cachePolicy);

await builder.Build().RunAsync();
