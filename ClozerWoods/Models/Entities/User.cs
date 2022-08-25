namespace ClozerWoods.Models.Entities;

public class User {
    public uint Id { get; init; }
    public string Email { get; init; }
    public byte[] Password { get; init; }
}
