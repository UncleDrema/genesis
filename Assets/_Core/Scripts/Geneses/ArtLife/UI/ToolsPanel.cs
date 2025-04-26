using Geneses.ArtLife.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Feature.Events;
using TriInspector;
using UnityEngine;

namespace Geneses.ArtLife.UI
{
    public class ToolsPanel : MonoBehaviour
    {
        [Button]
        public void SetTool(ToolType tool)
        {
            World.Default.CreateEventEntity<SetToolRequest>().Tool = tool;
        }
    }
}