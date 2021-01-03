using WebAssembly;

namespace WasmWrangler
{
    public static class JSObjectExtensions
    {
        public static T GetObjectProperty<T>(this JSObject obj, string name) =>
            (T)obj.GetObjectProperty(name);
    }
}
