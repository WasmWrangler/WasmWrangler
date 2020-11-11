using WebAssembly;

namespace WasmWrangler
{
    public static partial class JS
    {
        public static class console
        {
            private static JSObject? __js;

            private static JSObject _js
            {
                get
                {
                    if (__js == null)
                    {
                        __js = (JSObject)Runtime.GetGlobalObject(nameof(console));
                    }

                    return __js;
                }
            }

            public static void dir(object? item = null, object? options = null)
            {
                _js.Invoke(nameof(dir), item, options);
            }

            public static void info(params object[] data)
            {
                _js.Invoke(nameof(info), data);
            }
        }
    }
}
