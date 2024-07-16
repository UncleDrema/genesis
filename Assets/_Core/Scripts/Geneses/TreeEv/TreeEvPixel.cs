using System;
using System.Collections.Generic;
using System.Linq;
using Geneses.TreeEv.Components;
using Genesis.GameWorld;
using Scellecs.Morpeh.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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
            if (cTree.Energy > 500 && Under.Type is PixelType.Empty)
            {
                Under.Type = PixelType.Fruit;
                if (Random.value > 0.25f)
                {
                    Under.GeneticCode = cTree.Root.GeneticCode;
                }
                else
                {
                    Under.GeneticCode = Mutated(cTree.Root.GeneticCode);
                }
                Under.AddToTree(ref cTree);
                cTree.Energy -= 150;
            }
        }

        private Dictionary<int, int[]> Mutated(Dictionary<int, int[]> genes)
        {
            var res = new Dictionary<int, int[]>();
            foreach (var (key, value) in genes)
            {
                res[key] = value.ToArray();
            }

            res[Random.Range(0, 32)][Random.Range(0, 4)] = (int)(Random.value * 64);
            return res;
        }

        public void AddToTree(ref TreeComponent cTree)
        {
            TreeId = cTree.TreeId;
            cTree.Body.Add(this);
        }

        public bool IsSiblingOf(TreeEvPixel other)
        {
            if (TreeId is null || other.TreeId is null)
            {
                return false;
            }
            else if (TreeId == other.TreeId)
            {
                return true;
            }

            var difference = 0;
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (GeneticCode == null)
                    {
                        Debug.Log($"My code is null!!! {Type} {X} {Y}");
                    }
                    if (other.GeneticCode == null)
                    {
                        Debug.Log($"other code is null!!! {other.Type} {other.X} {other.Y}");
                    }
                    difference += Math.Abs(GeneticCode[i][j] - other.GeneticCode[i][j]);
                }
            }
            return difference < 7;
        }
    }
}