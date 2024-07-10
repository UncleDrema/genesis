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
                if (_type is PixelType.Empty)
                {
                    GeneticCode = null;
                    TreeId = null;
                }
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
        public string TreeId { get; set; }

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
                    pixel.AddToTree(ref cTree);
                }
            }
        }

        private void SpawnFruit(ref TreeComponent cTree)
        {
            if (cTree.Energy > 1000 && Under.Type is PixelType.Empty)
            {
                Under.Type = PixelType.Fruit;
                Under.GeneticCode = cTree.Root.GeneticCode;
                Under.AddToTree(ref cTree);
                cTree.Energy -= 500;
            }
        }

        public void AddToTree(ref TreeComponent cTree)
        {
            TreeId = cTree.TreeId;
            cTree.Body.Add(this);
        }

        public bool IsSiblingOf(TreeEvPixel other)
        {
            return TreeId != null && other.TreeId != null && TreeId == other.TreeId;
        }
    }
}