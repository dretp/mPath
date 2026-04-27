using Microsoft.AspNetCore.Mvc;

namespace mPath.Controllers;

[Route("api")]
public class DataController :Controller
{
 
    [HttpGet]
    public String example()
    {
        var name = "example";
        
        return name;
    }
}