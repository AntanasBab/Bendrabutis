using System.ComponentModel.DataAnnotations;

namespace Bendrabutis.Models.Dtos
{
    public record UserDto(string Id, string email);

    public record LoginDto(string email, string password);

    public record SuccessfulLoginDto(string AccessToken);

    public record RegisterUserDto([Required] string Username, [Required] string Password);
}