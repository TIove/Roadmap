using System.ComponentModel.DataAnnotations;

namespace Roadmap.Models.Dto.Requests.User;

public class CreateUserRequest
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
}