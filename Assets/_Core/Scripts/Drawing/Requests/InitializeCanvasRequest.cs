using System;

namespace Genesis.Drawing.Requests
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct InitializeCanvasRequest : IComponent
    {
        public int Width;
        public int Height;
    }
}