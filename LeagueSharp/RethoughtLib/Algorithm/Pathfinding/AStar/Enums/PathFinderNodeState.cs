﻿namespace RethoughtLib.Algorithm.AStar.Enums
{
    public enum PathFinderNodeState
    {
        Start = 1,

        End = 2,

        Open = 4,

        Close = 8,

        Current = 16,

        Path = 32
    }
}
