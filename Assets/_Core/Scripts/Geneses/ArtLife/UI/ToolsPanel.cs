using System;
using System.Collections.Generic;
using Geneses.ArtLife.ConstructingLife;
using Geneses.ArtLife.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Feature.Events;
using TMPro;
using TriInspector;
using UnityEngine;

namespace Geneses.ArtLife.UI
{
    public class ToolsPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _spawningCellText;
        
        [SerializeField]
        private TMP_Text _toolText;
        
        [SerializeField]
        private TMP_Text _toolSizeText;
        
        private static List<(string, byte[])> _spawningCells = new List<(string, byte[])>()
        {
            ("Простая клетка", LifePresets.SimpleLife()),
            ("Водоросль", LifePresets.Algae()),
            ("Хищник", LifePresets.PredatorLife())
        };
        
        private static Dictionary<ToolType, string> _toolNames = new Dictionary<ToolType, string>()
        {
            {ToolType.Clear, "Очистить"},
            {ToolType.SpawnWall, "Стена"},
            {ToolType.SpawnCell, "Клетка"},
            {ToolType.SpawnOrganic, "Органика"}
        };

        private static List<int> _toolSizes = new List<int>()
        {
            1,
            2,
            3,
            5,
            10
        };
        
        private int _spawningCellIndex = 0;
        private int _toolSizeIndex = 0;

        private void Start()
        {
            SetTool(ToolType.Clear);
            SetToolSize(_toolSizes[_toolSizeIndex]);
            SetSpawningCell(_spawningCellIndex);
        }

        private void SetToolSize(int toolSiz)
        {
            World.Default.CreateEventEntity<SetToolSizeRequest>().ToolSize = toolSiz;
            _toolSizeText.text = $"Размер: {toolSiz}";
        }

        public void NextSpawningCell()
        {
            _spawningCellIndex++;
            if (_spawningCellIndex >= _spawningCells.Count)
                _spawningCellIndex = 0;
            
            SetSpawningCell(_spawningCellIndex);
        }
        
        public void NextToolSize()
        {
            _toolSizeIndex++;
            if (_toolSizeIndex >= _toolSizes.Count)
                _toolSizeIndex = 0;
            
            SetToolSize(_toolSizes[_toolSizeIndex]);
        }
        
        public void SetSpawningCell(int index)
        {
            if (index < 0 || index >= _spawningCells.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of range");
            
            var (name, genome) = _spawningCells[index];
            World.Default.CreateEventEntity<SetSpawningCellRequest>().SpawningCellGenome = genome;
            _spawningCellText.text = name;
        }
        
        [Button]
        public void SetTool(ToolType tool)
        {
            World.Default.CreateEventEntity<SetToolRequest>().Tool = tool;
            _toolText.text = $"Выбрано: {_toolNames[tool]}";
        }
        
        [Button]
        public void SendClearOrganic()
        {
            World.Default.CreateEventEntity<ClearOrganicRequest>();
        }
        
        public void SetClearTool()
        {
            SetTool(ToolType.Clear);
        }
        
        public void SetWallTool()
        {
            SetTool(ToolType.SpawnWall);
        }
        
        public void SetCellTool()
        {
            SetTool(ToolType.SpawnCell);
        }

        public void SetOrganicTool()
        {
            SetTool(ToolType.SpawnOrganic);
        }
    }
}