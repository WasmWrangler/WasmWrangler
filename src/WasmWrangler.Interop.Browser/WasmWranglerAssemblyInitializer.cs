using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmWrangler.Interop.Browser
{
    public static class WasmWranglerAssemblyInitializer
    {
        public static void Initialize()
        {
            HTMLCanvas.Initialize();
            HTMLElement.Initialize();
        }
    }
}
