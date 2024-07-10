using System.Collections.Generic;
using Genesis.Common.Components;
using Genesis.GameWorld.Events;
using UnityEngine;

namespace Geneses.TreeEv.Systems
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Addons.Systems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class InitializeTreeEvolutionSystem : UpdateSystem
    {
        private Filter _initializedWorld;
        
        public override void OnAwake()
        {
            _initializedWorld = World.Filter
                .With<WorldComponent>()
                .With<WorldInitializedEvent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var world in _initializedWorld)
            {
                ref var cWorld = ref world.GetComponent<WorldComponent>();
                var count = 3;
                var step = cWorld.Width / (count + 1);
                for (int i = 1; i <= count; i++)
                {
                    MakeSeed((TreeEvPixel) cWorld.Pixels[step * i][cWorld.Height - 2]);
                }
            }
        }
        
        private static Dictionary<int, int[]> BasicCode = new Dictionary<int, int[]>()
        {
            {0, new int[4]{33, 4, 33, 35}},
            {1, new int[4]{5, 33, 6, 35}},
            {2, new int[4]{33, 4, 33, 7}},
            {3, new int[4]{33, 33, 5, 7}},
            {4, new int[4]{33, 22, 33, 35}},
            {5, new int[4]{2, 33, 33, 33}},
            {6, new int[4]{33, 33, 3, 33}},
            {7, new int[4]{31, 23, 20, 9}},
            {8, new int[4]{49, 22, 61, 47}},
            {9, new int[4]{21, 25, 42, 16}},
            {10, new int[4]{33, 1, 33, 35}},
            {11, new int[4]{51, 8, 74, 12}},
            {12, new int[4]{2, 33, 2, 35}},
            {13, new int[4]{48, 39, 63, 24}},
            {14, new int[4]{47, 19, 73, 9}},
            {15, new int[4]{51, 8, 74, 12}},
            {16, new int[4]{1, 33, 1, 35}},
            {17, new int[4]{17, 47, 10, 30}},
            {18, new int[4]{31, 1, 9, 54}},
            {19, new int[4]{60, 42, 68, 43}},
            {20, new int[4]{1, 34, 14, 19}},
            {21, new int[4]{57, 56, 14, 35}},
            {22, new int[4]{33, 10, 33, 35}},
            {23, new int[4]{44, 11, 6, 1}},
            {24, new int[4]{38, 70, 76, 2}},
            {25, new int[4]{41, 37, 26, 71}},
            {26, new int[4]{34, 21, 51, 73}},
            {27, new int[4]{4, 19, 13, 58}},
            {28, new int[4]{26, 9, 24, 35}},
            {29, new int[4]{11, 21, 66, 33}},
            {30, new int[4]{16, 69, 65, 38}},
            {31, new int[4]{21, 76, 15, 34}},
        };

        private void MakeSeed(TreeEvPixel pixel)
        {
            pixel.Type = PixelType.Seed;
            pixel.GeneticCode = BasicCode;
        }

        private Dictionary<int, int[]> GenRandomCode()
        {
            var res = new Dictionary<int, int[]>();
            for (int i = 0; i < 32; i++)
            {
                res[i] = new int[4]
                {
                    (int)(Random.value * 64),
                    (int)(Random.value * 64),
                    (int)(Random.value * 64),
                    (int)(Random.value * 64),
                };
            }

            return res;
        }
    }
}