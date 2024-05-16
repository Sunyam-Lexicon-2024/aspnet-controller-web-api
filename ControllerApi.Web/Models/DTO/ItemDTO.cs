using ControllerApi.Web.Models.Enums;

namespace ControllerApi.Web.Models.DTO;

public class ItemDTO {

    public long Id { get; set; }
    public string? Name { get; set; }
    public ItemSize ItemSize { get; set; }
}