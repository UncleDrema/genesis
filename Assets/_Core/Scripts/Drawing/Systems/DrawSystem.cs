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
    internal sealed class DrawSystem : UpdateSystem
    {
        private Filter _clearRequests;
        private Filter _drawRequests;
        
        public override void OnAwake()
        {
            _clearRequests = World.Filter
                .With<CanvasComponent>()
                .With<ClearCanvasRequest>()
                .Build();
            _drawRequests = World.Filter
                .With<CanvasComponent>()
                .With<SetPixelsBufferComponent>()
                .With<SetPixelsRequest>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var clearRequest in _clearRequests)
            {
                ref var cCanvas = ref clearRequest.GetComponent<CanvasComponent>();
                ref var cClear = ref clearRequest.GetComponent<ClearCanvasRequest>();
                var data = cCanvas.Texture2D.GetPixelData<Color32>(0);
                Color32 clearColor = cClear.Color;
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = clearColor;
                }
                clearRequest.MarkCanvasDirty();
            }
            
            foreach (var drawRequest in _drawRequests)
            {
                ref var cCanvas = ref drawRequest.GetComponent<CanvasComponent>();
                var data = cCanvas.Texture2D.GetPixelData<Color32>(0);
                var width = cCanvas.Width;
                var height = cCanvas.Height;
                ref var cReq = ref drawRequest.GetComponent<SetPixelsRequest>();
                ref var cBuffer = ref drawRequest.GetComponent<SetPixelsBufferComponent>();
                for (int i = 0; i < cReq.PixelAmount; i++)
                {
                    var pixelData = cBuffer.Pixels.data[i];
                    var x = pixelData.X;
                    var y = pixelData.Y;
                    if (x < 0 || x >= width ||
                        y < 0 || y >= height)
                    {
                        continue;
                    }

                    data[width * y + x] = pixelData.Color;
                }
                
                drawRequest.MarkCanvasDirty();
            }
        }
    }
}