using System.Collections.Generic;
using System.Linq;
using Geneses.TreeEv.Components;
using Genesis.GameWorld;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace Geneses.TreeEv
{
    public class TreeEvPixel : IPixel
    {
        private PixelType _type;
        
        public bool IsDirty { get; set; }
        public Color32 Color => Type.ToColor();

        public PixelType Type
        {
            get => _type;
            set
            {
                IsDirty = true;
                _type = value;
            }
        }
        public int Gene { get; set; }
        public Dictionary<int, int[]> GeneticCode { get; set; }
        public TreeEvPixel Above { get; set; }
        public TreeEvPixel Under { get; set; }
        public TreeEvPixel Left { get; set; }
        public TreeEvPixel Right { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void ExpressGene(ref TreeComponent cTree)
        {
            var treeCode = cTree.GeneticCode;
            if (Gene is 7 or 20)
            {
                SpawnFruit(ref cTree);
            }
            else if (Gene is < 32 and > -1)
            {
                var genes = treeCode[Gene];

                var neighbours = new TreeEvPixel[4]
                {
                    Left, Above, Right, Under
                };
                foreach (var (gene, pixel) in genes.Zip(neighbours, 
                             (gene, pixel) => (gene, pixel)).Where(pair => 
                             pair.pixel.Type is PixelType.Empty &&
                             pair.gene is < 32 and > -1))
                {
                    pixel.Type = PixelType.Sprout;
                    pixel.Gene = gene;
                    cTree.Body.Add(pixel);
                }
            }
        }

        private void SpawnFruit(ref TreeComponent cTree)
        {
            if (cTree.Energy > 1000 && Under.Type is PixelType.Empty)
            {
                Under.Type = PixelType.Fruit;
                Under.GeneticCode = cTree.Root.GeneticCode;
                cTree.Body.Add(Under);
                cTree.Energy -= 500;
            }
        }
    }
}