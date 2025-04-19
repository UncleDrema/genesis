using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Geneses.ArtLife.ConstructingLife
{
    public class LifeBuilder
    {
        private List<LifeToken> _tokens;
        
        public LifeBuilder()
        {
            _tokens = new List<LifeToken>();
        }

        public byte[] Build()
        {
            if (_tokens.Count(t => t is not Label) > 256)
            {
                throw new OverflowException("Число команд превышает 256");
            }
            
            var labelPositions = new Dictionary<string, byte>();
            var actualTokens = new List<LifeToken>();
            
            byte codePosition = 0;
            foreach (var token in _tokens)
            {
                if (token is Label label)
                {
                    labelPositions.Add(label.Name, codePosition);
                }
                else
                {
                    actualTokens.Add(token);
                    codePosition++;
                }
            }

            var geneticCode = new byte[actualTokens.Count];

            for (int i = 0; i < actualTokens.Count; i++)
            {
                var token = actualTokens[i];
                switch (token)
                {
                    case GenomeOperation operation:
                        geneticCode[i] = (byte)operation.Genome;
                        break;
                    case GenomeOperationArgument argument:
                        geneticCode[i] = argument.Value;
                        break;
                    case LabelPlaceholder placeholder:
                        var labelPosition = labelPositions[placeholder.Name];
                        var offset = placeholder.OffsetFromBranch;
                        if (labelPosition - (i - offset) < 0)
                            throw new ArgumentOutOfRangeException(nameof(labelPosition), $"Нельзя сделать переход назад: с позиции {i} на метку {placeholder.Name}({labelPosition}) при старте с {i - offset}");
                        geneticCode[i] = (byte) (labelPosition - (i - offset));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(token));
                }
            }

            return geneticCode;
        }
        
        public LifeBuilder DeclareLabel(string name)
        {
            var label = new Label(name);
            _tokens.Add(label);
            return this;
        }

        public LifeBuilder CheckEnergy(byte level, string ifLabel, string elseLabel)
        {
            var checkEnergy = new GenomeOperation(ArtLifeGenome.CheckEnergy);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkEnergy);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
            return this;
        }
        
        public void CheckMinerals(byte level, string ifLabel, string elseLabel)
        {
            var checkMinerals = new GenomeOperation(ArtLifeGenome.CheckMinerals);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkMinerals);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
        }
        
        public void CheckPhotosynthesisFlow(byte level, string ifLabel, string elseLabel)
        {
            var checkMinerals = new GenomeOperation(ArtLifeGenome.CheckPhotosynthesisFlow);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkMinerals);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
        }
        
        public void CheckMineralsFlow(byte level, string ifLabel, string elseLabel)
        {
            var checkMinerals = new GenomeOperation(ArtLifeGenome.CheckMineralFlow);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkMinerals);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
        }
        
        public void CheckHeight(byte level, string ifLabel, string elseLabel)
        {
            var checkHeight = new GenomeOperation(ArtLifeGenome.CheckHeight);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkHeight);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
        }

        public LifeBuilder Duplicate()
        {
            var duplicate = new GenomeOperation(ArtLifeGenome.Duplicate);
            _tokens.Add(duplicate);
            return this;
        }

        public LifeBuilder Rotate(Direction direction, bool absolute)
        {
            var rotate = absolute
                ? new GenomeOperation(ArtLifeGenome.AbsoluteRotate)
                : new GenomeOperation(ArtLifeGenome.RelativeRotate);
            var directionArg = new GenomeOperationArgument((byte)direction);
            _tokens.Add(rotate);
            _tokens.Add(directionArg);
            return this;
        }

        public LifeBuilder Move(Direction direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
        {
            var movePosition = _tokens.Count(t => t is not Label);
            var label = new Label($"Move_{movePosition}");
            _tokens.Add(label);
            ifEmpty ??= label.Name;
            ifWall ??= label.Name;
            ifOrganic ??= label.Name;
            ifCell ??= label.Name;
            var move = absolute
                ? new GenomeOperation(ArtLifeGenome.AbsoluteMove)
                : new GenomeOperation(ArtLifeGenome.RelativeMove);
            _tokens.Add(move);
            var directionArg = new GenomeOperationArgument((byte)direction);
            _tokens.Add(directionArg);
            _tokens.Add(new LabelPlaceholder(ifEmpty, 2));
            _tokens.Add(new LabelPlaceholder(ifWall, 3));
            _tokens.Add(new LabelPlaceholder(ifOrganic, 4));
            _tokens.Add(new LabelPlaceholder(ifCell, 5));
            return this;
        }
        
        public LifeBuilder Look(Direction direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
        {
            var lookPosition = _tokens.Count(t => t is not Label);
            var label = new Label($"Look_{lookPosition}");
            _tokens.Add(label);
            ifEmpty ??= label.Name;
            ifWall ??= label.Name;
            ifOrganic ??= label.Name;
            ifCell ??= label.Name;
            var look = absolute
                ? new GenomeOperation(ArtLifeGenome.AbsoluteLook)
                : new GenomeOperation(ArtLifeGenome.RelativeLook);
            _tokens.Add(look);
            var directionArg = new GenomeOperationArgument((byte)direction);
            _tokens.Add(directionArg);
            _tokens.Add(new LabelPlaceholder(ifEmpty, 2));
            _tokens.Add(new LabelPlaceholder(ifWall, 3));
            _tokens.Add(new LabelPlaceholder(ifOrganic, 4));
            _tokens.Add(new LabelPlaceholder(ifCell, 5));
            return this;
        }
        
        public LifeBuilder Eat(Direction direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
        {
            var eatPosition = _tokens.Count(t => t is not Label);
            var label = new Label($"Eat_{eatPosition}");
            _tokens.Add(label);
            ifEmpty ??= label.Name;
            ifWall ??= label.Name;
            ifOrganic ??= label.Name;
            ifCell ??= label.Name;
            var eat = absolute
                ? new GenomeOperation(ArtLifeGenome.AbsoluteEat)
                : new GenomeOperation(ArtLifeGenome.RelativeEat);
            _tokens.Add(eat);
            var directionArg = new GenomeOperationArgument((byte)direction);
            _tokens.Add(directionArg);
            _tokens.Add(new LabelPlaceholder(ifEmpty, 2));
            _tokens.Add(new LabelPlaceholder(ifWall, 3));
            _tokens.Add(new LabelPlaceholder(ifOrganic, 4));
            _tokens.Add(new LabelPlaceholder(ifCell, 5));
            return this;
        }
        
        public LifeBuilder Share(Direction direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
        {
            var sharePosition = _tokens.Count(t => t is not Label);
            var label = new Label($"Share_{sharePosition}");
            _tokens.Add(label);
            ifEmpty ??= label.Name;
            ifWall ??= label.Name;
            ifOrganic ??= label.Name;
            ifCell ??= label.Name;
            var share = absolute
                ? new GenomeOperation(ArtLifeGenome.AbsoluteShare)
                : new GenomeOperation(ArtLifeGenome.RelativeShare);
            _tokens.Add(share);
            var directionArg = new GenomeOperationArgument((byte)direction);
            _tokens.Add(directionArg);
            _tokens.Add(new LabelPlaceholder(ifEmpty, 2));
            _tokens.Add(new LabelPlaceholder(ifWall, 3));
            _tokens.Add(new LabelPlaceholder(ifOrganic, 4));
            _tokens.Add(new LabelPlaceholder(ifCell, 5));
            return this;
        }
        
        public LifeBuilder Gift(Direction direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
        {
            var giftPosition = _tokens.Count(t => t is not Label);
            var label = new Label($"Gift_{giftPosition}");
            _tokens.Add(label);
            ifEmpty ??= label.Name;
            ifWall ??= label.Name;
            ifOrganic ??= label.Name;
            ifCell ??= label.Name;
            var gift = absolute
                ? new GenomeOperation(ArtLifeGenome.AbsoluteGift)
                : new GenomeOperation(ArtLifeGenome.RelativeGift);
            _tokens.Add(gift);
            var directionArg = new GenomeOperationArgument((byte)direction);
            _tokens.Add(directionArg);
            _tokens.Add(new LabelPlaceholder(ifEmpty, 2));
            _tokens.Add(new LabelPlaceholder(ifWall, 3));
            _tokens.Add(new LabelPlaceholder(ifOrganic, 4));
            _tokens.Add(new LabelPlaceholder(ifCell, 5));
            return this;
        }
        
        public LifeBuilder Photosynthesis()
        {
            var photosynthesis = new GenomeOperation(ArtLifeGenome.Photosynthesis);
            _tokens.Add(photosynthesis);
            return this;
        }

        public LifeBuilder CheckSurrounded(string ifLabel, string elseLabel)
        {
            var checkSurrounded = new GenomeOperation(ArtLifeGenome.CheckSurrounded);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 1);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 2);
            _tokens.Add(checkSurrounded);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
            return this;
        }
        
        public LifeBuilder Exit()
        {
            var exit = new GenomeOperation(ArtLifeGenome.Exit);
            _tokens.Add(exit);
            return this;
        }

        private class LifeToken { }

        private class Label : LifeToken
        {
            public string Name { get; }
            
            public Label(string name)
            {
                Name = name;
            }
        }
        
        private class LabelPlaceholder : LifeToken
        {
            public string Name { get; }
            
            public int OffsetFromBranch { get; }
            
            public LabelPlaceholder(string name, int offsetFromBranch)
            {
                Name = name;
                OffsetFromBranch = offsetFromBranch;
            }
        }

        private class GenomeOperation : LifeToken
        {
            public ArtLifeGenome Genome { get; }
            
            public GenomeOperation(ArtLifeGenome genome)
            {
                Genome = genome;
            }
        }

        private class GenomeOperationArgument : LifeToken
        {
            public byte Value { get; }
            
            public GenomeOperationArgument(byte value)
            {
                Value = value;
            }
        }

        public enum ArtLifeGenome : byte
        {
            Photosynthesis = 0,
            AbsoluteRotate = 1,
            RelativeRotate = 2,
            AbsoluteMove = 3,
            RelativeMove = 4,
            AbsoluteLook = 5,
            RelativeLook = 6,
            AlignHorizontal = 7,
            AlignVertical = 8,
            AbsoluteShare = 9,
            RelativeShare = 10,
            AbsoluteGift = 11,
            RelativeGift = 12,
            AbsoluteEat = 13,
            RelativeEat = 14,
            ConvertMinerals = 15,
            Duplicate = 16,
            CheckEnergy = 17,
            CheckHeight = 18,
            CheckMinerals = 19,
            CheckSurrounded = 20,
            CheckPhotosynthesisFlow = 21,
            CheckMineralFlow = 22,
            Exit = 255
        }

        public enum Direction : byte
        {
            Right = 0,
            DownRight = 1,
            Down = 2,
            DownLeft = 3,
            Left = 4,
            UpLeft = 5,
            Up = 6,
            UpRight = 7
        }
    }
}