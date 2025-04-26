using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Genesis.GameWorld.Requests
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ResetGameWorldRequest : IComponent { }
}
