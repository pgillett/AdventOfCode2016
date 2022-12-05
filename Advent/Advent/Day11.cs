using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent;

public class Day11
{
    public int CountMoves(string input, bool withExtras)
    {
        var queue = new Queue<State>();
        var start = Parse(input);

        if (withExtras)
        {
            for (int i = 0; i < 4; i++)
            {
                start.Floors[0] += ItemCount;
                ItemCount <<= 1;
            }
        }

        queue.Enqueue(start);

        var seen = new HashSet<int>();

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();

            var hash = state.Hash();
            if (seen.Contains(hash))
            {
                continue;
            }

            seen.Add(hash);

            if (state.Complete)
            {
                return state.Moves;
            }
            if (state.Ok(ItemCount))
            {
                var skip = false;
                for (var i = 1; i < ItemCount; i <<= 1)
                {
                    for (var j = i; j < ItemCount; j <<= 1)
                    {
                        var has1 = (state.Floors[state.OnFloor] & i) != 0;
                        var has2 = (state.Floors[state.OnFloor] & j) != 0;
                        if (has1 && has2)
                        {
                            if (i % 2 == 1 && j == (i << 1))
                            {
                                if (skip)
                                {
                                    continue;
                                }

                                skip = true;
                            }
                        
                            if (state.OnFloor < 3)
                            {
                                var newState = state.Copy(state.OnFloor, i, j, state.OnFloor + 1);
                                queue.Enqueue(newState);
                            }
                            if (state.OnFloor > 0)
                            {
                                var newState = state.Copy(state.OnFloor, i, j, state.OnFloor - 1);
                                queue.Enqueue(newState);
                            }
                        }
                    }
                }
            }
        }

        throw new Exception("Didn't complete");
    }

    public State Parse(string input)
    {
        var floors = input.Split(Environment.NewLine);
        return new State
        {
            OnFloor = 0,
            Floors = floors.Select(Convert).ToArray()
        };
    }

    public int Convert(string input)
    {
        var items = 0;

        input = input.Replace(" and ", ", ").Substring(input.IndexOf("contains") + 9);

        foreach (var item in input.Split(", ").Where(i => i[0] == 'a'))
        {
            var split = item.Split(' ');
            var isotope = split[1][..2];
            var isMicrochip = split[2][0] == 'm';

            int bit;
            if (ItemCode.ContainsKey(isotope))
            {
                bit = ItemCode[isotope];
            }
            else
            {
                bit = ItemCount;
                ItemCode[isotope] = bit;
                ItemCount <<= 2;
            }

            if (isMicrochip)
            {
                items += bit;
            }
            else
            {
                items += bit << 1;
            }
        }

        return items;
    }

    public int ItemCount = 1;
    public Dictionary<string, int> ItemCode = new();
}

public class State
{
    public int OnFloor;
    public int[] Floors;
    public int Moves;

    //public string Hash => $"{OnFloor} {Floors[0]} {Floors[1]} {Floors[2]} {Floors[3]}";

    public int Hash()
    {
        var start = OnFloor.GetHashCode();
        for (var f = 0; f < 4; f++)
        {
            start = HashCode.Combine(start, Floors[f]);
        }

        return start;
    }

    public bool Complete => Floors[0] == 0 && Floors[1] == 0 && Floors[2] == 0;

    public bool Ok(int max)
    {
        foreach (var floor in Floors)
        {
            for (var i = 1; i < max; i <<= 2)
            {
                if ((floor & i) != 0)
                {
                    if ((floor & (i << 1)) == 0)
                    {
                        for (var j = 2; j < max; j <<= 2)
                        {
                            if ((floor & j) != 0)
                                return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public State Copy(int fromFloor, int item, int item2, int toFloor)
    {
        var copy = new State()
        {
            Moves = this.Moves + 1,
            Floors = new int[4],
            OnFloor = toFloor
        };
        
        for (var f = 0; f < 4; f++)
        {
            if (f == fromFloor)
            {
                copy.Floors[f] = Floors[f] & ~item & ~item2;
            }
            else if (f == toFloor)
            {
                copy.Floors[f] = Floors[f] | item | item2;
            }
            else
            {
                copy.Floors[f] = Floors[f];
            }
        }

        return copy;
    }
}
