using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Geneses.GameOfLife.Components
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct UpdatePixelsComponent : IComponent
    {
        public List<PixelUpdate> PixelUpdates;
    }

    [Serializable]
    public struct PixelUpdate
    {
        public int x;
        public int y;
        public int newState;
    }
}