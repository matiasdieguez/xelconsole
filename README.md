# XEL Console
SQL Server Audits XEL File Multiplatform console tool

Converts XEL files to json, html or txt formats to simplify cross-platform viewing

![.NET](https://github.com/matiasdieguez/xelconsole/workflows/.NET/badge.svg)

# Download release binaries
https://github.com/matiasdieguez/xelconsole/releases

# Usage

```
  xelconsole -i <input-file-name> -o <output-file-name>

  -i, --input     Required. Input file name. Must be a .XEL file.

  -o, --output    Required. Output file name.

  -f, --format    (Default: json) Output format: json|txt|html

  --help          Display this help screen.

  --version       Display version information.
```

# Build

## Requisites

- .NET 5 SDK https://dotnet.microsoft.com/download/dotnet/5.0

## Windows

```
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true -o windows
```

## Linux
```
dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true -o linux
```

## macOS

```
dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true -o osx
```

