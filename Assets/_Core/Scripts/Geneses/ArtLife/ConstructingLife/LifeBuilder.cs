using System;
using System.Collections.Generic;
using System.Linq;
using Geneses.ArtLife.ConstructingLife.Tokens;

namespace Geneses.ArtLife.ConstructingLife
{
    public class LifeBuilder
    {
        private List<LifeToken> _tokens;
        
        public LifeBuilder()
        {
            _tokens = new List<LifeToken>();
        }
        
        public List<LifeToken> GetRawTokens()
        {
            return _tokens;
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
        
        public LifeBuilder CheckSurrounded(byte level, string ifLabel, string elseLabel)
        {
            var checkSurrounded = new GenomeOperation(ArtLifeGenome.CheckSurrounded);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkSurrounded);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
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
        
        public LifeBuilder CheckMinerals(byte level, string ifLabel, string elseLabel)
        {
            var checkMinerals = new GenomeOperation(ArtLifeGenome.CheckMinerals);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkMinerals);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
            return this;
        }
        
        public LifeBuilder CheckPhotosynthesisFlow(byte level, string ifLabel, string elseLabel)
        {
            var checkMinerals = new GenomeOperation(ArtLifeGenome.CheckPhotosynthesisFlow);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkMinerals);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
            return this;
        }
        
        public LifeBuilder CheckMineralsFlow(byte level, string ifLabel, string elseLabel)
        {
            var checkMinerals = new GenomeOperation(ArtLifeGenome.CheckMineralFlow);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkMinerals);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
            return this;
        }
        
        public LifeBuilder CheckHeight(byte level, string ifLabel, string elseLabel)
        {
            var checkHeight = new GenomeOperation(ArtLifeGenome.CheckHeight);
            var levelArg = new GenomeOperationArgument(level);
            var ifLabelPlaceholder = new LabelPlaceholder(ifLabel, 2);
            var elseLabelPlaceholder = new LabelPlaceholder(elseLabel, 3);
            _tokens.Add(checkHeight);
            _tokens.Add(levelArg);
            _tokens.Add(ifLabelPlaceholder);
            _tokens.Add(elseLabelPlaceholder);
            return this;
        }

        public LifeBuilder Duplicate()
        {
            var duplicate = new GenomeOperation(ArtLifeGenome.Duplicate);
            _tokens.Add(duplicate);
            return this;
        }

        public LifeBuilder Rotate(byte direction, bool absolute)
        {
            var rotate = absolute
                ? new GenomeOperation(ArtLifeGenome.AbsoluteRotate)
                : new GenomeOperation(ArtLifeGenome.RelativeRotate);
            var directionArg = new GenomeOperationArgument((byte)direction);
            _tokens.Add(rotate);
            _tokens.Add(directionArg);
            return this;
        }

        public LifeBuilder Move(byte direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
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
        
        public LifeBuilder Look(byte direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
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
        
        public LifeBuilder Eat(byte direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
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
        
        public LifeBuilder Share(byte direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
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
        
        public LifeBuilder Gift(byte direction, bool absolute, string ifEmpty = null, string ifWall = null, string ifOrganic = null, string ifCell = null)
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
        
        public LifeBuilder AlighHorizontal()
        {
            var alignHorizontal = new GenomeOperation(ArtLifeGenome.AlignHorizontal);
            _tokens.Add(alignHorizontal);
            return this;
        }
        
        public LifeBuilder AlignVertical()
        {
            var alignVertical = new GenomeOperation(ArtLifeGenome.AlignVertical);
            _tokens.Add(alignVertical);
            return this;
        }
        
        public LifeBuilder Jump(byte offset)
        {
            var jump = new Jump(offset);
            _tokens.Add(jump);
            return this;
        }
        
        public LifeBuilder Exit()
        {
            var exit = new GenomeOperation(ArtLifeGenome.Exit);
            _tokens.Add(exit);
            return this;
        }

        public LifeBuilder ConvertMinerals()
        {
            var convertMinerals = new GenomeOperation(ArtLifeGenome.ConvertMinerals);
            _tokens.Add(convertMinerals);
            return this;
        }
    }
}