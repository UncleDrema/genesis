using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.GameWorld.Requests
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    internal struct ClickRequest : IComponent
    {
        public Vector2 ClickPosition;
        public PointerEventData.InputButton Button;
    }
}