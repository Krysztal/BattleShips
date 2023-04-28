using Battleships.Game.Models;

namespace Battleships.Game;

public interface IGame
{
    Board ComputerBoard { get; }
    bool IsShootInProgress { get; }
    Board PlayerBoard { get; }
    GameStatus Status { get; }

    void Restart();
    void Shoot(Cell cell);
    void Shoot(int x, int y);
}