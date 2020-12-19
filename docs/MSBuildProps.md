# WasmWrangler MSBuild Properties

There are few MSBuild properties that can be set to configure your project.

- **WasmNoAutomaticReferences** - Set to `true` to instruct WasmWrangler not to insert references to the
DLLS from the Mono Wasm SDK. Defaults to `false`.

- **WasmOutputDirectory** - The name of the directory that contains the packaged output. Defaults to `dist`.

