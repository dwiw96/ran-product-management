namespace ran_product_management_net.Utils;

internal static class HttpStatus
{
    internal static Dictionary<int, string> HttpStatusPair = new Dictionary<int, string>
    {
        {200, "OK"},
        {201, "Created"},
        {400, "Bad Request"},
        {404, "Not Found"},
        {500, "Status Server Error"}
    };
}

public class SuccessWithMessage(string message)
{
    public int Code { get; set; } = 200;
    public string Status { get; set; } = HttpStatus.HttpStatusPair[200];
    public string Message { get; set; } = message;
    public DateTime ExecutedAt { get; set; } = DateTime.Now;
}

public class SuccessWithData<T>(int code, string description, T data)
{
    public int Code { get; set; } = code;
    public string Status { get; set; } = HttpStatus.HttpStatusPair[code];
    public string Description { get; set; } = description;
    public T Data { get; set; } = data;
    public DateTime ExecutedAt { get; set; } = DateTime.Now;
}

public class SuccessWithDataPagination<T>(int code, string status, string description, T data, int size, int total, int currentPage, int totalPages)
{
    public int Code { get; set; } = code;
    public string Status { get; set; } = status;
    public string Description { get; set; } = description;
    public T Data { get; set; } = data;
    public Pagination Page { get; set; } = new Pagination(size, total, currentPage, totalPages);

    public DateTime ExecutedAt { get; set; } = DateTime.Now;
}

public class Pagination(int size, int total, int currentPage, int totalPages)
{
    public int Size { get; set; } = size;
    public int Total { get; set; } = total;
    public int CurrentPage { get; set; } = currentPage;
    public int TotalPages { get; set; } = totalPages;
}

public class FailedResponse(int code, string errorMessage, Dictionary<string, List<string>> errors)
{
    public int Code { get; set; } = code;
    public string Status { get; set; } = HttpStatus.HttpStatusPair[code];
    public string ErrorMessage { get; set; } = errorMessage;
    public Dictionary<string, List<string>> Errors { get; set; } = errors;
    public DateTime ExecutedAt { get; set; } = DateTime.Now;
}
