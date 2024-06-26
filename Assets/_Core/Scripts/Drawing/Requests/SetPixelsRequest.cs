using System;
using System.Collections.Generic;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace Genesis.Drawing.Requests
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    internal struct SetPixelsRequest : IComponent
    {
        public int PixelAmount;
    }
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    internal struct SetPixelsBufferComponent : IComponent
    {
        public FastList<PixelData> Pixels;
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    internal struct PixelData
    {
        public int X;
        public int Y;
        public Color32 Color;
    }
}