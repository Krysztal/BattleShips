namespace Battleships.Models;

public class Game
{
    private const string Player = "Computer";
    private readonly string _playerName;
    public GameStatus Status { get; private set; }
    public Stage ComputerStage { get; private set; } = default!;
    public Stage PlayerStage { get; private set; } = default!;

    public Game(string playerName)
    {
        _playerName = playerName;
        Restart();
    }

    public void Restart()
    {
        ComputerStage = new Stage(Player);
        ComputerStage.GenerateShips();
        ComputerStage.Hide();
        PlayerStage = new Stage(_playerName);
        PlayerStage.GenerateShips();
    }

    public void Shoot(Cell cell)
    {
        if (cell.Ship != null)
        {
            cell.Ship.Parts[cell.ShipPart!.Value] = true;
            if (cell.Ship.Parts.All(x => x))
            {
                foreach (var shipCell in cell.Ship.Cells)
                {
                    shipCell.Status = CellStatus.ShipDestroyed;
                }
            }
            else
            {
                cell.Status = CellStatus.ShipPartDestroyed;
                if (ComputerStage.Ships.All(x => x.IsDestroyed))
                {
                    Status = GameStatus.PlayerWon;
                }
                else
                {
                    Status = GameStatus.ComputerTurn;
                }
            }
        }
        else
        {
            cell.Status = CellStatus.Hit;
            Status = GameStatus.ComputerTurn;
        }
    }

    public void Shot(int x, int y)
    {
        Shoot(ComputerStage.Cells[x][y]);
    }
}