using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;

namespace MyFinances.Application.Users.Views;

public class UserViewModel
{
    public UserViewModel(Guid id, string name, string email, DateOnly birthDate, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        BirthDate = birthDate;
        Role = role;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public Role Role { get; set; }

    public static UserViewModel FromEntity(User user)
    {
        return new UserViewModel(
            user.Id,
            user.Name,
            user.Email,
            user.BirthDate,
            user.Role
        );
    }
}