# AdmxParser

[![NuGet Version](https://img.shields.io/nuget/v/AdmxParser)](https://www.nuget.org/packages/AdmxParser/) [![GitHub Sponsors](https://img.shields.io/github/sponsors/rkttu)](https://github.com/sponsors/rkttu/)

.NET-based ADMX/ADML parser library and programmatic Windows policy setting/management framework

## Minimum Requirements

- Requires a platform with .NET Standard 2.0 or later that can handle receiving and sending connections over TCP sockets.
  - Supported .NET Version: .NET Core 2.0+, .NET 5+, .NET Framework 4.6.1+, Mono 5.4+, UWP 10.0.16299+, Unity 2018.1+

## How to use

### Loading PolicyDefinitions installed in the Windows directory

```csharp
using AdmxParser;

...

var instance = AdmxDirectory.GetSystemPolicyDefinitions();
await instance.LoadAsync(true);

var admxCollection = instance.LoadedAdmxFiles;

// Use admxCollection variable to investigate system policies.
```

## License

This library follows Apache-2.0 license. See [LICENSE](./LICENSE) file for more information.
