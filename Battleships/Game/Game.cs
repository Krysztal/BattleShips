using Battleships.Game.Models;

namespace Battleships.Game;

public sealed class Game : IGame
{
    private const string Player = "Computer";
    public string PlayerName { get; init; }
    public bool IsShootInProgress { get; private set; }
    public GameStatus Status { get; private set; } = GameStatus.PlayerTurn;
    public Stage ComputerStage { get; private set; } = default!;
    public Stage PlayerStage { get; private set; } = default!;

    public Game(string playerName)
    {
        PlayerName = playerName;
        Restart();
    }

    public void Restart()
    {
        ComputerStage = new Stage(Player);
        ComputerStage.GenerateShips();
        ComputerStage.Hide();
        PlayerStage = new Stage(PlayerName);
        PlayerStage.GenerateShips();
        Status = GameStatus.PlayerTurn;
        IsShootInProgress = false;
    }

    public void Shoot(Cell cell)
    {
        IsShootInProgress = true;
        if (cell.IsShipPlaced)
        {
            cell.Ship!.Parts[cell.ShipPart!.Value] = true;
            if (cell.Ship.IsDestroyed)
            {
                MarkShipAsDestroyed(cell.Ship);
                if (GetEnemyStage().Ships.All(x => x.IsDestroyed))
                {
                    SetWinner();
                }
            }
            else
            {
                cell.Status = CellStatus.ShipPartDestroyed;
            }

            if (Status == GameStatus.ComputerTurn)
            {
                ComputerShoot();
            }
        }
        else
        {
            cell.Status = CellStatus.Miss;
            ChangeGameTurn();
            if (Status == GameStatus.ComputerTurn)
            {
                ComputerShoot();
            }
        }

        IsShootInProgress = false;
    }

    public void Shoot(int x, int y)
    {
        Shoot(GetEnemyStage().Cells[x][y]);
    }

    private void ComputerShoot()
    {
        var isShooted = true;
        var x = -1;
        var y = -1;
        while (isShooted)
        {
            x = Random.Shared.Next(PlayerStage.Size);
            y = Random.Shared.Next(PlayerStage.Size);
            var foundStatuses = new CellStatus[] { CellStatus.ShipPartDestroyed, CellStatus.ShipDestroyed, CellStatus.Miss };
            isShooted = foundStatuses.Contains(PlayerStage.Cells[x][y].Status);
        }

        Shoot(x, y);
    }

    private Stage GetEnemyStage()
    {
        switch (Status)
        {
            case GameStatus.PlayerTurn:
                return ComputerStage;
            case GameStatus.ComputerTurn:
                return PlayerStage;
        }

        throw new NotImplementedException($"GetCurrentStage do not support {Status} status.");
    }

    private void ChangeGameTurn()
    {
        if (Status == GameStatus.ComputerTurn)
        {
            Status = GameStatus.PlayerTurn;
        }
        else
        {
            Status = GameStatus.ComputerTurn;
        }
    }

    private static void MarkShipAsDestroyed(Ship ship)
    {
        foreach (var shipCell in ship.Cells)
        {
            shipCell.Status = CellStatus.ShipDestroyed;
        }
    }

    private void SetWinner()
    {
        switch (Status)
        {
            case GameStatus.PlayerTurn:
                Status = GameStatus.PlayerWon;
                break;
            case GameStatus.ComputerTurn:
                Status = GameStatus.ComputerWon;
                break;
            default: throw new NotImplementedException($"SetWinner do not support {Status} status.");
        }
    }
}