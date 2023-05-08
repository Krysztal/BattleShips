using Battleships.Game;

namespace BattleshipsTests;

public class GameTests
{
    private IGame _game;

    [SetUp]
    public void Setup()
    {
        _game = new Game("Me", new Randomizer());
    }

    [Test]
    public void Restart_ShouldPlacedShips_OnPlayerBouard()
    {
        //arrange

        //act
        _game.Restart();

        //assert
        _game.PlayerBoard.Ships.Should().NotBeNull();
        _game.PlayerBoard.Ships.All(x => x.Cells != null).Should().BeTrue();
    }

    [Test]
    public void Restart_ShouldPlacedShips_OnBoards()
    {
        //arrange

        //act
        _game.Restart();

        //assert
        _game.ComputerBoard.Ships.Should().NotBeNull();
        _game.ComputerBoard.Ships.All(x => x.Cells != null).Should().BeTrue();
    }
}