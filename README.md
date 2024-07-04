# AdmxParser

[![NuGet Version](https://img.shields.io/nuget/v/AdmxParser)](https://www.nuget.org/packages/AdmxParser/) ![Build Status](https://github.com/rkttu/AdmxParser/actions/workflows/dotnet.yml/badge.svg) [![GitHub Sponsors](https://img.shields.io/github/sponsors/rkttu)](https://github.com/sponsors/rkttu/)

.NET-based ADMX/ADML parser library

## Breaking Changes

### From 0.6.7 to 0.7

- Removed `GetMangledMemberNameFromDisplayName` extension methods due to lack of usage.

### From 0.6.2 to 0.6.3

- I understand that changing policy settings is the last step in the mechanism, changing the registry. Since the framework does not handle looking up or modifying policies directly, and instead has to make it use the IGroupPolicyObject2 interface, I removed the associated functionality and test cases.
- Removed other unused extension methods.

### From 0.5 to 0.6

All types of models and contract interfaces has been removed. Instead, the library now uses auto-generated XML schema classes to represent ADMX/ADML files for accuracy and integrity.

## Minimum Requirements

- Requires a platform with .NET Standard 2.0 or later, and Windows Vista+, Windows Server 2008+
  - This library does not support ADM files.
  - Supported .NET Version: .NET Core 2.0+, .NET 5+, .NET Framework 4.6.1+, Mono 5.4+, UWP 10.0.16299+, Unity 2018.1+

## How to use

### Loading PolicyDefinitions XML schema

Explore the policy model with C# classes that replicate the ADMX/ADML XML schema prototype.

```csharp
using AdmxParser;

...

var instance = AdmxDirectory.GetSystemPolicyDefinitions();
await instance.LoadAsync(true);

var admxCollection = instance.LoadedAdmxFiles;

// Use admxCollection variable to investigate system policies.
```

### Parsing ADMX/ADML files

You can explore the changes that will actually be made to your POL file in a preview form.

```csharp
using AdmxParser;

...

var instance = AdmxDirectory.GetSystemPolicyDefinitions();
await instance.LoadAsync(true);

var models = admxDirectory.ParseModels();

foreach (var eachPolicy in models)
{
	// ...
}
```

## XML Schema Notes

- The XML schema used in this library is based on the schema defined in the [ADMX File Schema](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-gpreg/6e10478a-e9e6-4fdc-a1f6-bdd9bd7f2209).
- You can regenerate the XML schema classes by running the `xsd.exe` tool with the parameters `PolicyDefinitionFiles.xsd /c /order /eld /edb /l:CS /namespace:AdmxParser.Serialization /nologo`.

## License

This library follows Apache-2.0 license. See [LICENSE](./LICENSE) file for more information.
