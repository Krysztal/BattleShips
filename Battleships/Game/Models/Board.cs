namespace Battleships.Game.Models;

public sealed class Board
{
    public readonly int Size = 10;
    private readonly IRandomizer _randomizer;

    public string Player { get; private set; }

    public Cell[][] Cells { get; private set; }

    public Ship[] Ships { get; private set; } = new Ship[3];

    public Board(string player, IRandomizer randomizer)
    {
        Player = player;
        _randomizer = randomizer;
        Cells = GenerateCells();
    }

    public void PlaceShips()
    {
        var ships = new Ship[3];
        ships[0] = PlaceShip(6);
        ships[1] = PlaceShip(5);
        ships[2] = PlaceShip(5);

        Ships = ships;
    }

    public void Hide()
    {
        foreach (var row in Cells)
        {
            foreach (var call in row)
            {
                call.Status = CellStatus.Close;
            }
        }
    }

    private Ship PlaceShip(int size)
    {
        var ship = new Ship()
        {
            Cells = new Cell[size]
        };

        bool shipPlaced = false;

        while (!shipPlaced)
        {
            // randomly choose orientation and starting position
            bool isHorizontal = _randomizer.Next(2) == 0;
            int x = _randomizer.Next(Size);
            int y = _randomizer.Next(Size);

            //seed = (int)DateTime.Now.Ticks;
            //int x = new Random(seed).Next(Size);
            //seed = (int)DateTime.Now.Ticks;
            //int y = new Random(seed).Next(Size);

            // check if ship can be placed in chosen position
            bool canPlaceShip = true;
            for (int j = 0; j < size; j++)
            {
                int checkX = isHorizontal ? x + j : x;
                int checkY = isHorizontal ? y : y + j;

                if (checkX >= Size || checkY >= Size || Cells[checkX][checkY].ShipPart is not null)
                {
                    canPlaceShip = false;
                    break;
                }
            }

            // if ship can be placed, mark it on the board
            if (canPlaceShip)
            {
                for (int j = 0; j < size; j++)
                {
                    int markX = isHorizontal ? x + j : x;
                    int markY = isHorizontal ? y : y + j;
                    Cells[markX][markY].Ship = ship;
                    Cells[markX][markY].ShipPart = j;
                    Cells[markX][markY].Status = CellStatus.OpenShip;
                    ship.Cells[j] = Cells[markX][markY];
                }
                shipPlaced = true;
            }
        }

        return ship;
    }

    private Cell[][] GenerateCells()
    {
        var cells = new Cell[Size][];
        for (int x = 0; x < Size; x++)
        {
            cells[x] = new Cell[Size];
            for (int y = 0; y < Size; y++)
            {
                cells[x][y] = new Cell(x, y);
            }
        }

        return cells;
    }
}
