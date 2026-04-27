namespace mPath.Models.Configuration;

public class MpathConfiguration
{
    public static MpathConfiguration? Instance;
    
    public MpathConfiguration()
    {
        Init();
    }
    
    private void Init()
    {
        Instance ??= this;
    }

    public MpathConfiguration GetInstance()
    {
        return Instance;
    }
    
    public string DbConnectionString { get; set; } = string.Empty;
}