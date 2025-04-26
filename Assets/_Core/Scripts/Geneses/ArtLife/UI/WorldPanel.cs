using Geneses.ArtLife.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Feature.Events;
using TriInspector;
using UnityEngine;

namespace Geneses.ArtLife.UI
{
    public class WorldPanel : MonoBehaviour
    {
        [Button]
        public void SetDrawMode(DrawMode drawMode)
        {
            World.Default.CreateEventEntity<SetDrawModeRequest>().DrawMode = drawMode;
        }
        
        [Button]
        public void SendClearOrganic()
        {
            World.Default.CreateEventEntity<ClearOrganicRequest>();
        }
    }
}