using Template.Server.Domain.Core;

namespace Template.Server.API.Users.Domain;

public class User : Entity
{
    public required string CPF { get; set; }
    public string? CEP { get; set; }
    public string? Street { get; set; }
}
