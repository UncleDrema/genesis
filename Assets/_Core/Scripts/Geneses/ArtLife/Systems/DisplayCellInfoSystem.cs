using Geneses.ArtLife.Components;
using Geneses.ArtLife.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Addons.Systems;
using Unity.IL2CPP.CompilerServices;

namespace Geneses.ArtLife.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DisplayCellInfoSystem : UpdateSystem
    {
        private Filter _displayRequests;
        private Filter _artLifeDisplay;
        
        public override void OnAwake()
        {
            _artLifeDisplay = World.Filter.With<ArtLifeDisplayComponent>().Build();
            _displayRequests = World.Filter.With<DisplayCellInfoRequest>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var req in _displayRequests)
            {
                ref var cReq = ref req.GetComponent<DisplayCellInfoRequest>();
                
                foreach (var display in _artLifeDisplay)
                {
                    ref var cDisplay = ref display.GetComponent<ArtLifeDisplayComponent>();
                    cDisplay.DisplayedCell = cReq.Cell;
                }
            }
        }
    }
}