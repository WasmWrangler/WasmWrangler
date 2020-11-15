﻿using WebAssembly;

namespace WasmWrangler
{
    public static partial class JS
    {
        public static class document
        {
            private static JSObject? __js;

            private static JSObject _js
            {
                get
                {
                    if (__js == null)
                    {
                        __js = (JSObject)Runtime.GetGlobalObject(nameof(document));
                    }

                    return __js;
                }
            }

            public static JSObject getElementById(string element)
            {
                return (JSObject)_js.Invoke(nameof(getElementById), element);
            }
        }
    }
}