namespace Krabicezpapundeklu.Formatting
{
    public interface IScanner
    {
        ScannerState State { get; set; }

        int Token { get; }
        string Text { get; }

        int Start { get; }
        int End { get; }

        void Scan();
    }
}