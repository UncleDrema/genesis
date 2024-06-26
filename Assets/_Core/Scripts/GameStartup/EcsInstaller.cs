using System;
using Geneses.Blank;
using Geneses.TreeEv;
using Genesis.Drawing;
using Genesis.GameWorld;
using Scellecs.Morpeh.Addons.Feature;
using Scellecs.Morpeh.Addons.Feature.Unity;
using Scellecs.Morpeh.Addons.Unity.VContainer;
using VContainer;

namespace Genesis.GameStartup
{
    internal class EcsInstaller : BaseFeaturesInstaller
    {
        private IObjectResolver _container;

        [Inject]
        private void Inject(IObjectResolver container)
        {
            _container = container;
        }
        
        protected override void InitializeShared() { }

        protected override UpdateFeature[] InitializeUpdateFeatures()
        {
            return new UpdateFeature[]
            {
                _container.CreateFeature<TreeEvFeature>(),
                _container.CreateFeature<GameWorldFeature>(),
                _container.CreateFeature<DrawingFeature>(),
            };
        }

        protected override FixedUpdateFeature[] InitializeFixedUpdateFeatures()
        {
            return Array.Empty<FixedUpdateFeature>();
        }

        protected override LateUpdateFeature[] InitializeLateUpdateFeatures()
        {
            return new LateUpdateFeature[]
            {
                _container.CreateFeature<GameWorldLateFeature>()
            };
        }
    }
}