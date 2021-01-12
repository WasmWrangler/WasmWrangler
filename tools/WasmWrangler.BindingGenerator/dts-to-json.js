// Example Usage:
// node .\tools\WasmWrangler.BindingGenerator\dts-to-json.js .\ext\types\lib.dom.d.ts > .\ext\types\lib.dom.json

const fs = require('fs');
const ts = require(__dirname + '/../../ext/typescript/typescript.min');
const process = require('process');

const source = fs.readFileSync(process.argv[2], 'utf-8');

const sourceFile = ts.createSourceFile(process.argv[2], source, ts.ScriptTarget.Latest, true);

// No need to save the source again.
delete sourceFile.text;

const cache = [];
const json = JSON.stringify(sourceFile, (key, value) => {
  // Discard the following.
  if (key === 'flags' || key === 'transformFlags' || key === 'modifierFlagsCache') {
      return;
  }
  
  // Replace 'kind' with the string representation.
  if (key === 'kind') {
      value = ts.SyntaxKind[value];
  }
  if (typeof value === 'object' && value !== null) {
    // Duplicate reference found, discard key
    if (cache.includes(value)) return;

    cache.push(value);
  }
  return value;
});

console.info(json);
