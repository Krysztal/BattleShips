using Battleships.Game.Models;

namespace Battleships.Game;

public interface IGame
{
    Board PlayerBoard { get; }
    bool IsShootInProgress { get; }
    Board ComputerBoard { get; }
    GameStatus Status { get; }

    void Restart();
    void Shoot(Cell cell);
    void Shoot(int x, int y);
}