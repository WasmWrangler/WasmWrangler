[![The MIT License](https://img.shields.io/badge/license-MIT-orange.svg?style=flat-square)](http://opensource.org/licenses/MIT)
[![Build Status](https://github.com/WasmWrangler/WasmWrangler/workflows/CI/badge.svg)](https://github.com/WasmWrangler/WasmWrangler/actions)

# WasmWrangler

<img align="right" width="170px" height="170px" src="https://github.com/WasmWrangler/WasmWrangler/raw/master/assets/Logo.png">

WasmWrangler is a NuGet package that enables your C# application to be run in a browser. Think of it as the
[Vanilla JS](http://vanilla-js.com/) for C#.

## Building

If the version of [Mono Wasm SDK](https://github.com/mono/mono/tree/master/sdks/wasm) isn't available
before you start building you'll likely receive an error about "Unable to resolve WebAssembly.Bindings.dll".

Rebuilding might solve the problem but there is a build script provided that will ensure the Mono Wasm SDK
is present:

```
dotnet msbuild build/WasmEnsureSDKAvailable.proj
```

This builds the WasmWrangler assembly and calls the `WasmEnsureSDKAvailable` target which will download the
SDK if it's not available on your machine.
