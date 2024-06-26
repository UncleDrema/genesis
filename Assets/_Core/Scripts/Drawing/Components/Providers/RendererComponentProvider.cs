using Scellecs.Morpeh.Providers;
using TriInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Genesis.Drawing.Components.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class RendererComponentProvider : MonoProvider<RendererComponent>
    {
        protected override void OnValidate()
        {
            base.OnValidate();
            ref var data = ref GetData();
            data.Renderer = GetComponent<Renderer>();
        }

        [Button]
        public void Draw(int x, int y)
        {
            Entity.SetPixel(x, y, Color.green);
        }
    }
}