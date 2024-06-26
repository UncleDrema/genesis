using Genesis.Drawing.Requests;
using Genesis.Drawing.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace Genesis.Drawing
{
    public static class DrawingExtensions
    {
        public static void SetPixel(this Entity entity, int x, int y, Color color)
        {
            if (!entity.Has<CanvasComponent>())
            {
                Debug.LogError($"Попытка отрисовки на сущности без {nameof(CanvasComponent)}: {entity}");
                return;
            }
            if (!entity.Has<SetPixelsBufferComponent>())
            {
                Debug.LogError($"Попытка отрисовки на сущности без {nameof(SetPixelsBufferComponent)}: {entity}");
                return;
            }
            //
            // var pixel = new PixelData()
            // {
            //     Color = color,
            //     X = x,
            //     Y = y,
            // };

            ref var cBuf = ref entity.GetComponent<SetPixelsBufferComponent>();
            ref var cReq = ref entity.GetSetPixelsRequest();
            ref var pixel = ref cBuf.Pixels.data[cReq.PixelAmount];
            pixel.X = x;
            pixel.Y = y;
            pixel.Color = color;
            cReq.PixelAmount++;
        }

        private static ref SetPixelsRequest GetSetPixelsRequest(this Entity entity)
        {
            if (entity.Has<SetPixelsRequest>())
            {
                return ref entity.GetComponent<SetPixelsRequest>();
            }

            ref var cReq = ref entity.AddComponent<SetPixelsRequest>();
            cReq.PixelAmount = 0;
            return ref cReq;
        }

        public static void MarkCanvasDirty(this Entity entity)
        {
            if (!entity.Has<ApplyTextureChangesRequest>())
                entity.AddComponent<ApplyTextureChangesRequest>();
        }
    }
}