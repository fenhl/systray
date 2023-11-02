**Maintenance notice:** This app is deprecated in favor of the `systray-diskspace` binary in [fenhl/diskspace](https://github.com/fenhl/diskspace).

This app is a collection of utilities that sit in the Windows notification area (“systray”). Current features:

* Notify on low disk space (less than 5% or less than 5 GB)

# Publishing a new version

1. In `Systray/Systray.csproj`, increment the `Version` entry.
2. Commit and push the changes.
3. Run `dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true`
4. [Create a new release](https://github.com/fenhl/systray/releases/new) with the tag and title matching the version, and attach `Systray\bin\Release\net5.0-windows\win-x64\publish\Systray.exe` as `systray-x64.exe`.
