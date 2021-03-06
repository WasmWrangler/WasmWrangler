// <auto-generated />
#nullable enable
using WebAssembly;

namespace WasmWrangler.Interop.Browser
{
	public static partial class document
	{
		private static JSObject? __js;

		private static JSObject _js
		{
			get
			{
				if (__js == null)
					__js = (JSObject)Runtime.GetGlobalObject(nameof(document));

				return __js;
			}
		}

		public static HTMLElement? getElementById(string element)
		{
			var result = _js.Invoke(nameof(getElementById), element);

			if (result == null)
				return null;

			return JSObjectWrapperFactory.Create<HTMLElement>(result);
		}

		public static T? getElementById<T>(string element)
			where T: HTMLElement
		{
			var result = _js.Invoke(nameof(getElementById), element);

			if (result == null)
				return null;

			return JSObjectWrapperFactory.Create<T>(result);
		}

	}

}
