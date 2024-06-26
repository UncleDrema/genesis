using Genesis.Drawing.Requests;
using Genesis.Drawing.Components;
using UnityEngine;

namespace Genesis.Drawing.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class ApplyTextureChangesSystem : UpdateSystem
    {
        private Filter _applyRequests;
        
        public override void OnAwake()
        {
            _applyRequests = World.Filter
                .With<CanvasComponent>()
                .With<ApplyTextureChangesRequest>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var applyRequest in _applyRequests)
            {
                ref var cCanvas = ref applyRequest.GetComponent<CanvasComponent>();
                cCanvas.Texture2D.Apply();
            }
        }
    }
}