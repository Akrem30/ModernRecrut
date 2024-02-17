namespace ModernRecrut.MVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class ResponseResult
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessage Errors { get; set; }

    }

    public class ResponseErrorMessage
    {
        public List<string> Messages { get; set; }
    }
}