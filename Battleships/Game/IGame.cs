using Battleships.Game.Models;

namespace Battleships.Game;

public interface IGame
{
    Stage ComputerStage { get; }
    bool IsShootInProgress { get; }
    Stage PlayerStage { get; }
    GameStatus Status { get; }

    void Restart();
    void Shoot(Cell cell);
    void Shoot(int x, int y);
}