namespace Battleships.Game;

public class Randomizer : IRandomizer
{
    private static readonly Lazy<Random> random = new(() => new Random((int)DateTime.Now.Ticks));

    public int Next()
    {
        return random.Value.Next();
    }

    public int Next(int maxValue)
    {
        return random.Value.Next(maxValue);
    }

    public int Next(int minValue, int MaxValue)
    {
        return random.Value.Next(minValue, MaxValue);
    }
}
