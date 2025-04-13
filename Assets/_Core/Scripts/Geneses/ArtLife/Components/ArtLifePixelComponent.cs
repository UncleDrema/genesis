using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Geneses.ArtLife.Components
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ArtLifePixelComponent : IComponent
    {
        public byte[] Genome;
        public int CommandCounter;
        public int ActiveGeneIndex;
        public int Direction;
        public Vector2 Position;
        public int Energy;
        public int Age;
        public int LastEnergySource;
        public HashSet<int> EnergySources;
    }
}