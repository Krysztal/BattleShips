using Battleships.Game.Models;

namespace Battleships.Game;

public sealed class Game : IGame
{
    private const string Player = "Computer";
    private static readonly CellStatus[] _foundStatuses = new CellStatus[] { CellStatus.ShipPartDestroyed, CellStatus.ShipDestroyed, CellStatus.Miss };

    public string PlayerName { get; init; }
    public bool IsShootInProgress { get; private set; }
    public GameStatus Status { get; private set; } = GameStatus.PlayerTurn;
    public Board ComputerBoard { get; private set; } = default!;
    public Board PlayerBoard { get; private set; } = default!;

    public Game(string playerName)
    {
        PlayerName = playerName;
        Restart();
    }

    public void Restart()
    {
        ComputerBoard = new Board(Player);
        ComputerBoard.GenerateShips();
        ComputerBoard.Hide();
        PlayerBoard = new Board(PlayerName);
        PlayerBoard.GenerateShips();
        Status = GameStatus.PlayerTurn;
        IsShootInProgress = false;
    }

    public void Shoot(Cell cell)
    {
        if (IsAlreadyShot(cell.X, cell.Y))
            return;

        IsShootInProgress = true;
        if (cell.IsShipPlaced)
        {
            cell.Ship!.Parts[cell.ShipPart!.Value] = true;
            if (cell.Ship.IsDestroyed)
            {
                MarkShipAsDestroyed(cell.Ship);
                if (GetEnemyBoard().Ships.All(x => x.IsDestroyed))
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
        Shoot(GetEnemyBoard().Cells[x][y]);
    }

    private void ComputerShoot()
    {
        var isShot = true;
        var x = -1;
        var y = -1;

        while (isShot)
        {
            x = Random.Shared.Next(PlayerBoard.Size);
            y = Random.Shared.Next(PlayerBoard.Size);
            isShot = IsAlreadyShot(x, y);
        }

        Shoot(x, y);
    }

    private bool IsAlreadyShot(int x, int y)
    {
        var board = GetEnemyBoard();
        return _foundStatuses.Contains(board.Cells[x][y].Status);
    }

    private Board GetEnemyBoard()
    {
        switch (Status)
        {
            case GameStatus.PlayerTurn:
                return ComputerBoard;
            case GameStatus.ComputerTurn:
                return PlayerBoard;
        }

        throw new NotImplementedException($"GetEnemyBoard does not support {Status} status.");
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
            default: throw new NotImplementedException($"SetWinner does not support {Status} status.");
        }
    }
}