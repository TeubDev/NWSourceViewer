# NWSourceViewer

A WASM program that runs in your browser and interprets 2da and tlk files into something human-readable.

## Compiling

To add custom 2da and tlk files, create a folder named `source` in the `wwwroot` folder, and drop all the files in there. Edit the `appsettings.json` file if your tlk file is named anything other than `custom.tlk`.

To compile this program, you need the .NET 7 or later SDK installed. Then in the NWSourceViewer sub-folder (the one that contains the NWSourceViewer.csproj file), run this command:

```
dotnet publish -c Release
```

This will create files in the `/bin/Release/net7.0/publish` folder. All of these are needed for running the application.

## Running

If you are planning on hosting this program on the internet, all the files created by publishing should be hosted together.

If you are only running it locally, run this command where you would have run the publish command.

```
dotnet run
```