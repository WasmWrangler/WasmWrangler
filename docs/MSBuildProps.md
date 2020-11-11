# WasmWrangler MSBuild Properties

There are few MSBuild properties that can be set to configure your project.

- **WasmEntryAssembly** - Which assembly contains the main entry point. Defaults to `$(AssemblyName).dll`.

- **WasmMainEntryPoint** - Which method should be called as the main entry point. The format of the value
should be `Namespace.Class:Method`. Defaults to `$(AssemblyName).Program:Main`.

- **WasmNoAutomaticReferences** - Set to `false` to insturct WasmWrangler not to insert references to the
DLLS from the Mono Wasm SDK. Defaults to `true`.

- **WasmOutputDirectory** - The name of the directory that contains the packaged output. Defaults to `dist`.

- **WasmScriptsDirectory** - The name of the directory where you project has scripts. Used to inject the
WasmWrangler.ts file on restore. Defaults to `scripts`.
