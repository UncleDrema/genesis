using System;
using System.Collections.Generic;
using Scellecs.Morpeh.Collections;

namespace Geneses.TreeEv.Components
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct TreeComponent : IComponent
    {
        public Dictionary<int, int[]> GeneticCode;
        public int Energy;
        public bool IsAlive;
        public TreeEvPixel Root;
        public FastList<TreeEvPixel> Body;
    }
}