using ControllerWebAPI.Models.Enums;

namespace ControllerWebAPI.Models.APIModels;

public class ItemAPIModel {

    public long Id { get; set; }
    public string? Name { get; set; }
    public ItemSize ItemSize { get; set; }
}