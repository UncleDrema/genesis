using Scellecs.Morpeh.Providers;
using TriInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.Drawing.Components.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class RendererComponentProvider : MonoProvider<RendererComponent>, IPointerClickHandler
    {
        [Button]
        public void Draw(int x, int y)
        {
            Entity.SetPixel(x, y, Color.green);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clicked at {eventData.position}");
        }
    }
}