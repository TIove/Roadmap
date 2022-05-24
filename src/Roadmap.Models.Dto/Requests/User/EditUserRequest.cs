namespace Roadmap.Models.Dto.Requests.User;

public class EditUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public int Status { get; set; }
}