using System;
using Genesis.GameWorld.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Feature.Events;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.GameWorld
{
    public class WorldClickHandler : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
    {
        private bool _holding = false;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                World.Default.CreateEventEntity<PauseRequest>();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                World.Default.CreateEventEntity<ResetGameWorldRequest>();
            }
        }

        public void OnPointerClick(PointerEventData ev)
        {
            var rT = GetComponent<RectTransform>();
            var pos = ev.position;
            var screenRect = GetScreenCoordinates(rT);
            if (screenRect.Contains(pos))
            {
                var relPos = pos - screenRect.min;
                ref var cReq = ref World.Default.CreateEventEntity<ClickRequest>();
                cReq.Button = ev.button;
                cReq.ClickPosition = relPos;
            }
        }
        
        public void OnPointerMove(PointerEventData ev)
        {
            if (_holding)
            {
                var rT = GetComponent<RectTransform>();
                var pos = ev.position;
                var screenRect = GetScreenCoordinates(rT);
                if (screenRect.Contains(pos))
                {
                    var relPos = pos - screenRect.min;
                    ref var cReq = ref World.Default.CreateEventEntity<ClickRequest>();
                    cReq.Button = ev.button;
                    cReq.ClickPosition = relPos;
                }
            }
        }
        
        public Rect GetScreenCoordinates(RectTransform uiElement)
        {
            var worldCorners = new Vector3[4];
            uiElement.GetWorldCorners(worldCorners);
            var result = new Rect(
                worldCorners[0].x,
                worldCorners[0].y,
                worldCorners[2].x - worldCorners[0].x,
                worldCorners[2].y - worldCorners[0].y);
            return result;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            _holding = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            _holding = false;
        }
    }
}