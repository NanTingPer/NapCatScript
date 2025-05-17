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

    public static string? WebUriHostNotPort
    {
        get {
            if(WebUri is null) return null;
            var endTmp = 0;
            var startTmp = 0;
            for (int i = WebUri.Length - 1; i >= 0; i--) {
                if (WebUri[i] == ':') {
                    endTmp = i;
                    break;
                }
            }
            
            for (int i = 0; i <= WebUri.Length - 1; i++) {
                if (WebUri[i] == ':') {
                    startTmp = i;
                    break;
                }
            }

            return WebUri[(startTmp + 3) ..endTmp];
        }
    }
    
    public static string? WebToken { get; set; }
    public static string? AuthToken { get; set; }
}