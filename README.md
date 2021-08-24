This app is a collection of utilities that sit in the Windows notification area (“systray”). Current features:

* Notify on low disk space (less than 5% or less than 5 GB)

# Publishing a new version

1. In [Visual Studio 2019](https://visualstudio.microsoft.com/vs/), open the project properties (double-click the folder “Properties” in the Solution Explorer).
2. In the Package tab, increment the package version.
3. Save the properties, then commit and push the changes.
4. Run `dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true`
5. [Create a new release](https://github.com/fenhl/systray/releases/new) with the tag and title matching the version, and attach `Systray\bin\Release\net5.0-windows\win-x64\publish\Systray.exe` as `systray-x64.exe`.
