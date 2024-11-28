using Python.Runtime;

namespace ProjectTools.Core.Scripting;

public sealed class PythonManager
{
    private static readonly Lazy<PythonManager> Lazy = new(() => new PythonManager());

    private PythonManager()
    {
        Runtime.PythonDLL = "";
    }

    public static PythonManager Manager => Lazy.Value;
}
