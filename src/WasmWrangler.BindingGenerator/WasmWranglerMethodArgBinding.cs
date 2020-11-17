namespace WasmWrangler.BindingGenerator
{
    public class WasmWranglerMethodArgBinding
    {
        public string Name { get; set; } = "";

        public bool Params { get; set; } = false;

        public string Type { get; set; } = "";

        public string? Default { get; set; }
    }
}
