namespace NapCatScript.Core.JsonFormat;

public abstract class RequestJson
{
    public abstract string JsonText { get; set; }
    public virtual string GetString()
    {
        return JsonText;
    }

    public override string ToString()
    {
        return JsonText;
    }

}
