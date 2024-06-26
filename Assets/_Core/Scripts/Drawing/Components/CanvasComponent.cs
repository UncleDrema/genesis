using System;
using UnityEngine;

namespace Genesis.Drawing.Components
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    internal struct CanvasComponent : IComponent
    {
        public int Width;
        public int Height;
        public Texture2D Texture2D;
    }
}