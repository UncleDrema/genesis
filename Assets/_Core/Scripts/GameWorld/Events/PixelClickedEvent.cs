using System;
using UnityEngine.EventSystems;

namespace Genesis.GameWorld.Events
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct PixelClickedEvent : IComponent
    {
        public IPixel Pixel;
        public PointerEventData.InputButton Button;
    }
}