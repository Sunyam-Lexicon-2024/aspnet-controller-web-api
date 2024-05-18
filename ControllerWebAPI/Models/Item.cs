using ControllerWebAPI.Models.Enums;

namespace ControllerWebAPI.Models;

public class Item
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public ItemSize ItemSize { get; set; }
    public string? Secret { get; set; }
}