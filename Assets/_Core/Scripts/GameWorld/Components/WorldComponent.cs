using System;
using Genesis.GameWorld;

namespace Genesis.Common.Components
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct WorldComponent : IComponent
    {
        public int Width;
        public int Height;
        public IPixel[][] Pixels;
        public int PixelSize;
    }
}