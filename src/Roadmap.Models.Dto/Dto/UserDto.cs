namespace Roadmap.Models.Dto.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public int Status { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}