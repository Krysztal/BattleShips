namespace Battleships.Game.Models;

public class Ship
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Cell[] Cells { get; init; }
    public bool IsDestroyed => Cells.All(x => x.Status == CellStatus.Destroyed || x.Status == CellStatus.ShipDestroyed);
}
