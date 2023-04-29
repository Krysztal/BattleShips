namespace Battleships.Game.Models;

public sealed class Ship
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Cell[] Cells { get; init; }
    public bool IsSunk => Cells.All(x => x.Status == CellStatus.Hit || x.Status == CellStatus.ShipSunk);
}
