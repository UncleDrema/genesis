using System;
using Geneses.ArtLife;
using Genesis.Drawing;
using Genesis.GameWorld;
using Scellecs.Morpeh.Addons.Feature;
using Scellecs.Morpeh.Addons.Feature.Unity;
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
                CreateFeature<GameWorldFeature>(_container),
                CreateFeature<ArtLifeFeature>(_container),
                CreateFeature<DrawingFeature>(_container)
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
                CreateFeature<GameWorldLateFeature>(_container),
            };
        }

        private static TFeature CreateFeature<TFeature>(IObjectResolver container)
            where TFeature : BaseFeature
        {
            var builder = new RegistrationBuilder(typeof(TFeature), Lifetime.Transient);
            var registration = builder.Build();
            return registration.SpawnInstance(container) as TFeature;
        }
    }
}