using Bendrabutis.Models.Enums;

namespace Bendrabutis.Models.Dtos
{
    public record NewPostDto(RequestType Type, string Description);
}
