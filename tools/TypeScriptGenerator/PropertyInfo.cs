namespace TypeScriptGenerator
{
    public class PropertyInfo
    {
        public string Name { get; init; } = "";

        public string Type { get; init; } = "";

        public override string ToString() => $"{Type} {Name}";
    }
}
