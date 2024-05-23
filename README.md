# AdmxParser

.NET-based ADMX/ADML parser library and programmatic Windows policy setting/management framework

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
