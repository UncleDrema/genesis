using System;
using System.Collections.Generic;
using Geneses.ArtLife.Components;
using Geneses.ArtLife.ConstructingLife;
using Geneses.ArtLife.Requests;
using Geneses.ArtLife.UI;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Geneses.ArtLife.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateArtLifeDisplay : UpdateSystem
    {
        private Filter _ticks;
        private Filter _updateRequests;
        private Filter _artLifeDisplay;
        private Filter _ui;
        
        public override void OnAwake()
        {
            _ticks = World.Filter
                .With<TickEvent>()
                .Build();
            _updateRequests = World.Filter
                .With<UpdateDisplayRequest>()
                .Build();
            _artLifeDisplay = World.Filter
                .With<ArtLifeDisplayComponent>()
                .Build();
            _ui = World.Filter
                .With<UiComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var tick in _ticks)
            {
                UpdateDisplay();
            }
            
            foreach (var request in _updateRequests)
            {
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            foreach (var display in _artLifeDisplay)
            {
                ref var cDisplay = ref display.GetComponent<ArtLifeDisplayComponent>();
                UpdateData(ref cDisplay);

                foreach (var ui in _ui)
                {
                    ref var cUi = ref ui.GetComponent<UiComponent>();

                    UpdateView(ref cDisplay, cUi.Ui);
                }
            }
        }

        private static Dictionary<string, string> Empty = new(0);
        
        private void UpdateView(ref ArtLifeDisplayComponent cDisplay, GameUI ui)
        {
            var cell = cDisplay.DisplayedCell;
            if (cell == null)
            {
                ui.LoadParametersIntoTable(Empty);
                ui.SetGenomeText(string.Empty);
            }
            else
            {
                var cellInfo = new Dictionary<string, string>();

                cellInfo["Энергия"] = cell.Energy.ToString();
                cellInfo["Возраст"] = cell.Age.ToString();
                cellInfo["Поворот"] = cell.Rotation.ToString();
                cellInfo["Минералы"] = cell.AccumulatedMineralsCount.ToString();
                cellInfo["Всего фотосинтеза"] = cell.TotalPhotosynthesisEnergy.ToString();
                cellInfo["Всего минералов"] = cell.TotalMineralEnergy.ToString();
                cellInfo["Всего органики"] = cell.TotalOrganicEnergy.ToString();
                cellInfo["Счетчик команд"] = cell.GeneCounter.ToString();
                cellInfo["Позиция"] = $"X={cell.Position.X} Y={cell.Position.Y}";
                var code = GenomeDecompiler.PrettyPrint(GenomeDecompiler.Decompile(cell.Genome), cell.PrevGeneCounter, cell.GeneCounter);
                
                ui.LoadParametersIntoTable(cellInfo);
                ui.SetGenomeText(code);
            }
        }

        private void UpdateData(ref ArtLifeDisplayComponent cDisplay)
        {
            if (cDisplay.DisplayedCell != null && cDisplay.DisplayedCell.Node == null)
            {
                cDisplay.DisplayedCell = null;
            }
        }
    }
}