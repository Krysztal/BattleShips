namespace Battleships.Game;

public interface IRandomizer
{
    int Next();
    int Next(int maxValue);
    int Next(int minValue, int MaxValue);
}