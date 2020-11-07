# WasmWrangler



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
