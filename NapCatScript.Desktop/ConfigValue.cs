namespace NapCatScript.Desktop;

public static class ConfigValue
{
    private static string _webUri;

    public static string? WebUri
    {
        get {
            if(_webUri.EndsWith('/'))
                return _webUri.Substring(0, _webUri.Length - 1);
            return _webUri;
        } set => _webUri = value;
    }

    public static string? WebToken { get; set; }
    public static string? AuthToken { get; set; }
}