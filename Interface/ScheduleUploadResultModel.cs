namespace mPath.Location.Models;

public class ScheduleUploadResultModel
{
    public int TotalRows { get; set; }
    public int CreatedCount { get; set; }
    public int FailedCount => Errors.Count;
    public List<ScheduleUploadErrorModel> Errors { get; set; } = new();
}

public class ScheduleUploadErrorModel
{
    public int RowNumber { get; set; }
    public string Message { get; set; } = string.Empty;
}
