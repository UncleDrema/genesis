using System;
using UnityEngine;
using UnityEngine.UI;

namespace Genesis.Drawing.Components
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    internal struct RendererComponent : IComponent
    {
        public RawImage RawImage;
        public int ScaleX;
        public int ScaleY;
    }
}