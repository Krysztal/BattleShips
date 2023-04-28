namespace Battleships.Game.Models;

public class Stage
{
    public readonly int Size = 10;

    public string Player { get; private set; }

    public Cell[][] Cells { get; private set; }

    public Ship[] Ships { get; private set; } = new Ship[3];

    public Stage(string player)
    {
        Player = player;
        Cells = GenerateCells();
    }

    public void GenerateShips()
    {
        var ships = new Ship[3];
        ships[0] = GenerateShip(6);
        ships[1] = GenerateShip(5);
        ships[2] = GenerateShip(5);

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

    private Ship GenerateShip(int size)
    {
        var ship = new Ship()
        {
            Cells = new Cell[size],
            Parts = new bool[size]
        };

        bool shipPlaced = false;

        // keep trying to place ship until successful
        while (!shipPlaced)
        {
            // randomly choose orientation and starting position
            var seed = (int)DateTime.Now.Ticks;
            bool isHorizontal = new Random(seed).Next(2) == 0;

            seed = (int)DateTime.Now.Ticks;
            int x = new Random(seed).Next(9);
            seed = (int)DateTime.Now.Ticks;
            int y = new Random(seed).Next(9);

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
