namespace Battleships.Game.Models;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public CellStatus Status { get; set; } = CellStatus.Open;
    public int? ShipPart { get; set; }
    public Ship? Ship { get; set; }
    public bool IsShipPlaced => Ship != null;

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }
}