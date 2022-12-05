using System.Linq;
using Advent;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventTest;

[TestClass]
public class Day11Test
{
    private Day11 _day11;

    [TestInitialize]
    public void Setup()
    {
        _day11 = new Day11();
    }

    [TestMethod]
    public void InputParsesCorrectly()
    {
        var expectedResult = new [] {0b0101, 0b0010, 0b1000, 0b0000};

        var actualResult = _day11.Parse(_input).Floors.Select(f => f);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void InputSlidingShouldBe11()
    {
        var expectedResult = 11;

        var actualResult = _day11.CountMoves(_input, false);

        actualResult.Should().Be(expectedResult);
    }

    private string _input = @"The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
The second floor contains a hydrogen generator.
The third floor contains a lithium generator.
The fourth floor contains nothing relevant.";

}
