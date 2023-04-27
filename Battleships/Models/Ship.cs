namespace Battleships.Models;

public class Ship
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Size => Parts.Length;
    public required Cell[] Cells { get; init; }

    public required bool[] Parts { get; init; }

    public bool IsDestroyed => Parts.All(x => x);
}
