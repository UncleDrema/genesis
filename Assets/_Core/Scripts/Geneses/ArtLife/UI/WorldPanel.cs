using System;
using System.Collections.Generic;
using Geneses.ArtLife.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Feature.Events;
using TMPro;
using TriInspector;
using UnityEngine;

namespace Geneses.ArtLife.UI
{
    public class WorldPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _drawModeText;
        
        private static Dictionary<DrawMode, string> _drawModeNames = new Dictionary<DrawMode, string>()
        {
            {DrawMode.EnergySource, "Источник энергии"},
            {DrawMode.Energy, "Энергия"},
            {DrawMode.Age, "Возраст"},
            {DrawMode.Mutations, "Мутации"},
            {DrawMode.Minerals, "Минералы"},
        };

        private void Start()
        {
            SetDrawMode(DrawMode.EnergySource);
        }

        [Button]
        public void SetDrawMode(DrawMode drawMode)
        {
            World.Default.CreateEventEntity<SetDrawModeRequest>().DrawMode = drawMode;
            _drawModeText.text = $"Выбрано: {_drawModeNames[drawMode]}";
        }
        
        public void SetEnergySourceMode()
        {
            SetDrawMode(DrawMode.EnergySource);
        }
        
        public void SetEnergyMode()
        {
            SetDrawMode(DrawMode.Energy);
        }
        
        public void SetAgeMode()
        {
            SetDrawMode(DrawMode.Age);
        }
        
        public void SetMutationsMode()
        {
            SetDrawMode(DrawMode.Mutations);
        }
        
        public void SetMineralsMode()
        {
            SetDrawMode(DrawMode.Minerals);
        }
    }
}