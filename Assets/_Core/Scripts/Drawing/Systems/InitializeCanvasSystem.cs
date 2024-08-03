using Genesis.Drawing.Requests;
using Genesis.Drawing.Components;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace Genesis.Drawing.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class InitializeCanvasSystem : UpdateSystem
    {
        private Filter _initRequests;
        
        public override void OnAwake()
        {
            _initRequests = World.Filter
                .With<InitializeCanvasRequest>()
                .With<RendererComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var initRequest in _initRequests)
            {
                ref var cReq = ref initRequest.GetComponent<InitializeCanvasRequest>();
                ref var cRenderer = ref initRequest.GetComponent<RendererComponent>();
                ref var cCanvas = ref initRequest.AddComponent<CanvasComponent>();
                ref var cBuffer = ref initRequest.AddComponent<SetPixelsBufferComponent>();
                
                cCanvas.Width = cReq.Width;
                cCanvas.Height = cReq.Height;
                cBuffer.Pixels = new FastList<PixelData>(cReq.Width * cReq.Height);
                
                cCanvas.Texture2D = new Texture2D(cCanvas.Width, cCanvas.Height);
                cCanvas.Texture2D.filterMode = FilterMode.Point;
                cRenderer.RawImage.texture = cCanvas.Texture2D;

                ref var cClear = ref initRequest.AddComponent<ClearCanvasRequest>();
                cClear.Color = Color.white;
            }
        }
    }
}