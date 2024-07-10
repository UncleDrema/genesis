using System.Collections.Generic;
using Geneses.TreeEv.Components;
using Genesis.Common.Components;
using Genesis.GameWorld;
using Genesis.GameWorld.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace Geneses.TreeEv.Systems
{
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class TreeGrowthSystem : UpdateSystem
    {
        private Filter _trees;
        private Filter _ticks;
        private Filter _world;
        
        public override void OnAwake()
        {
            _ticks = World.Filter
                .With<TickEvent>()
                .Build();
            _world = World.Filter
                .With<WorldComponent>()
                .Build();
            _trees = World.Filter
                .With<TreeComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var tick in _ticks)
            {
                foreach (var world in _world)
                {
                    ref var cWorld = ref world.GetComponent<WorldComponent>();
                    foreach (var tree in _trees)
                    {
                        IterateTree(tree);
                    }
                    cWorld.ForEach<TreeEvPixel>((x, y, pixel) =>
                    {
                        if (pixel.Under.Type == PixelType.Wall && pixel.Type == PixelType.Seed)
                        {
                            SpawnTree(pixel.GeneticCode, pixel);
                            pixel.Type = PixelType.Sprout;
                            pixel.Gene = 0;
                        }
                    });
                }
            }
        }

        private void SpawnTree(Dictionary<int, int[]> geneticCode, TreeEvPixel root)
        {
            var tree = World.CreateEntity();
            ref var cTree = ref tree.AddComponent<TreeComponent>();
            cTree.GeneticCode = geneticCode;
            cTree.IsAlive = true;
            cTree.Root = root;
            cTree.Body = new FastList<TreeEvPixel>();
            cTree.Body.Add(root);
            cTree.Energy = 5000;
        }

        private void IterateTree(Entity tree)
        {
            ref var cTree = ref tree.GetComponent<TreeComponent>();
            cTree.Energy -= cTree.Body.length - 1;
            if (cTree.Energy <= 0)
            {
                KillTree(ref cTree);
                tree.RemoveComponent<TreeComponent>();
            }
            else
            {
                GrowTree(ref cTree);
            }
        }

        private void GrowTree(ref TreeComponent cTree)
        {
            foreach (var treePixel in cTree.Body)
            {
                if (treePixel.Type == PixelType.Sprout)
                {
                    treePixel.ExpressGene(ref cTree);
                    treePixel.Type = PixelType.Tree;
                }
            }
        }

        private void KillTree(ref TreeComponent cTree)
        {
            cTree.IsAlive = false;
            foreach (var treePixel in cTree.Body)
            {
                treePixel.Gene = -2;
                if (treePixel.Type != PixelType.Fruit)
                {
                    treePixel.Type = PixelType.Empty;
                    treePixel.GeneticCode = null; // TODO: load clear genetic code
                }
            }
        }
    }
}