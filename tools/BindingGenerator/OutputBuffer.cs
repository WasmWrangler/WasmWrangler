using System.Text;

namespace BindingGenerator
{
    public class OutputBuffer
    {
        private readonly StringBuilder _sb;
        private int _indentLevel;
        private bool _isNewLine = true;

        public OutputBuffer()
        {
            _sb = new StringBuilder(1024);
        }

        public override string ToString() => _sb.ToString();

        public void IncreaseIndent() => _indentLevel++;

        public void DecreaseIndent() => _indentLevel--;

        public string GetIndent() => new string('\t', _indentLevel);

        public void Append(string value)
        {
            EnsureIndent();
            _sb.Append(value);
        }

        public void AppendLine()
        {
            _sb.AppendLine();
            _isNewLine = true;
        }

        public void AppendLine(string value)
        {
            EnsureIndent();
            _sb.AppendLine(value);
            _isNewLine = true;
        }

        private void EnsureIndent()
        {
            if (_isNewLine && _indentLevel > 0)
                _sb.Append(GetIndent());

            _isNewLine = false;
        }
    }
}
