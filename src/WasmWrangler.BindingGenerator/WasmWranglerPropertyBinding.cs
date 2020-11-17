namespace WasmWrangler.BindingGenerator
{
    public class WasmWranglerPropertyBinding
    {
        public string Name { get; set; } = "";

        public string Type { get; set; } = "";

        public bool CanGet { get; set; } = false;

        public bool CanSet { get; set; } = false;
    }
}
