using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.UI
{
    public class WorldClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData ev)
        {
            Debug.Log($"{ev.button} {ev.position}");
        }
    }
}