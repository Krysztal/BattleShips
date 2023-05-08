using Battleships.Game.Models;

namespace Battleships.Game;

public sealed class Game : IGame
{
    private const string _computerName = "Computer";
    private readonly IRandomizer _randomizer;
    private static readonly CellStatus[] _foundStatuses = new CellStatus[] { CellStatus.Hit, CellStatus.ShipSunk, CellStatus.Miss };

    public string PlayerName { get; init; }
    public bool IsShootInProgress { get; private set; }
    public GameStatus Status { get; private set; } = GameStatus.PlayerTurn;
    public Board PlayerBoard { get; private set; } = default!;
    public Board ComputerBoard { get; private set; } = default!;

    public Game(string playerName, IRandomizer randomizer)
    {
        PlayerName = playerName;
        _randomizer = randomizer;
        Restart();
    }

    public void Restart()
    {
        PlayerBoard = new Board(PlayerName, _randomizer);
        PlayerBoard.PlaceShips();
        PlayerBoard.Hide();
        ComputerBoard = new Board(_computerName, _randomizer);
        ComputerBoard.PlaceShips();
        Status = GameStatus.PlayerTurn;
        IsShootInProgress = false;
    }

    public void Shoot(Cell cell)
    {
        if (Status == GameStatus.PlayerWon || Status == GameStatus.ComputerWon)
            return;
        if (IsAlreadyShot(cell.X, cell.Y))
            return;

        IsShootInProgress = true;
        if (cell.IsShipPlaced)
        {
            cell.Status = CellStatus.Hit;
            if (cell.Ship!.IsSunk)
            {
                MarkShipAsDestroyed(cell.Ship);
                if (GetEnemyBoard().Ships.All(x => x.IsSunk))
                {
                    SetWinner();
                }
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

    //very simple algorithm
    private void ComputerShoot()
    {
        var isShot = true;
        var x = -1;
        var y = -1;

        while (isShot)
        {
            x = _randomizer.Next(ComputerBoard.Size);
            y = _randomizer.Next(ComputerBoard.Size);
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
        return Status switch
        {
            GameStatus.PlayerTurn => PlayerBoard,
            GameStatus.ComputerTurn => ComputerBoard,
            _ => throw new NotImplementedException($"GetEnemyBoard does not support {Status} status."),
        };
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
            shipCell.Status = CellStatus.ShipSunk;
        }
    }

    private void SetWinner()
    {
        Status = Status switch
        {
            GameStatus.PlayerTurn => GameStatus.PlayerWon,
            GameStatus.ComputerTurn => GameStatus.ComputerWon,
            _ => throw new NotImplementedException($"SetWinner does not support {Status} status."),
        };
    }
}