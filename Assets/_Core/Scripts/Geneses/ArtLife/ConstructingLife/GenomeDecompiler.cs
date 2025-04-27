using System;
using System.Collections.Generic;
using System.Linq;
using Geneses.ArtLife.ConstructingLife.Tokens;
using UnityEngine;

namespace Geneses.ArtLife.ConstructingLife
{
    public static class GenomeDecompiler
    {
        private enum GenomeCommandOperandType
        {
            ValueArgument,
            LabelPlaceholder,
        }

        private static List<GenomeCommandOperandType> Zero = new(0);
        
        private static List<GenomeCommandOperandType> Direction = new()
        {
            GenomeCommandOperandType.ValueArgument
        };
        
        private static List<GenomeCommandOperandType> CheckIfElse = new()
        {
            GenomeCommandOperandType.LabelPlaceholder,
            GenomeCommandOperandType.LabelPlaceholder
        };

        private static List<GenomeCommandOperandType> CheckValueIfElse = new()
        {
            GenomeCommandOperandType.ValueArgument,
            GenomeCommandOperandType.LabelPlaceholder,
            GenomeCommandOperandType.LabelPlaceholder
        };
        
        private static List<GenomeCommandOperandType> DirectionIfEmptyIfWallIfOrganicIfCell = new()
        {
            GenomeCommandOperandType.ValueArgument,
            GenomeCommandOperandType.LabelPlaceholder,
            GenomeCommandOperandType.LabelPlaceholder,
            GenomeCommandOperandType.LabelPlaceholder,
            GenomeCommandOperandType.LabelPlaceholder
        };

        private static Dictionary<ArtLifeGenome, List<GenomeCommandOperandType>> GenomeCommandArity = new()
        {
            { ArtLifeGenome.Photosynthesis, Zero},
            { ArtLifeGenome.AbsoluteRotate, Direction},
            { ArtLifeGenome.RelativeRotate, Direction},
            { ArtLifeGenome.AbsoluteMove, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.RelativeMove, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.AbsoluteLook, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.RelativeLook, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.AlignHorizontal, Zero},
            { ArtLifeGenome.AlignVertical, Zero},
            { ArtLifeGenome.AbsoluteShare, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.RelativeShare, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.AbsoluteGift, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.RelativeGift, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.AbsoluteEat, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.RelativeEat, DirectionIfEmptyIfWallIfOrganicIfCell},
            { ArtLifeGenome.ConvertMinerals, Zero},
            { ArtLifeGenome.Duplicate, Zero},
            { ArtLifeGenome.CheckEnergy, CheckValueIfElse },
            { ArtLifeGenome.CheckHeight, CheckValueIfElse },
            { ArtLifeGenome.CheckMinerals, CheckValueIfElse },
            { ArtLifeGenome.CheckSurrounded, CheckValueIfElse },
            { ArtLifeGenome.CheckPhotosynthesisFlow, CheckValueIfElse },
            { ArtLifeGenome.CheckMineralFlow, CheckValueIfElse },
            { ArtLifeGenome.Exit, Zero}
        };

        public static string PrettyPrint(List<LifeToken> tokens, int highlightCommand1 = -1, int highlightCommand2 = -1)
        {
            var sb = new System.Text.StringBuilder();
            
            var usedLabels = new HashSet<string>();
            var lastUsedLabelIndex = 0;
            var commandIndex = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] is LabelPlaceholder placeholder)
                {
                    usedLabels.Add(placeholder.Name);
                    lastUsedLabelIndex = commandIndex;
                }

                if (tokens[i] is not Label)
                {
                    commandIndex++;
                }
            }


            commandIndex = 0;
            bool printingOperation = false;
            int currentArity = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                if (printingOperation && token is not Label)
                {
                    currentArity--;
                }

                if (token is not Label)
                {
                    if (commandIndex == highlightCommand1)
                        sb.Append("<color=red>");
                    else if (commandIndex == highlightCommand2)
                        sb.Append("<color=yellow>");
                }
                
                if (token is Label label && usedLabels.Contains(label.Name))
                {
                    if (printingOperation)
                    {
                        sb.Append($" {label.Name}: ");
                    }
                    else
                    {
                        sb.AppendLine($"{label.Name}:");
                    }
                }

                if (token is Jump jump)
                {
                    sb.AppendLine($"  jmp {jump.Value}");
                }
                
                if (token is LabelPlaceholder placeholder)
                {
                    sb.Append($" {placeholder.Name}");
                }
                
                if (token is GenomeOperation operation)
                {
                    sb.Append($"  {operation.Genome}");
                    if (operation.Genome == ArtLifeGenome.Exit && commandIndex >= lastUsedLabelIndex)
                    {
                        break;
                    }

                    printingOperation = true;
                    currentArity = GenomeCommandArity[operation.Genome].Count;
                }
                
                if (token is GenomeOperationArgument argument)
                {
                    sb.Append($" {argument.Value}");
                }

                if (token is not Label)
                {
                    if (commandIndex == highlightCommand1 || commandIndex == highlightCommand2)
                    {
                        sb.Append("</color>");
                    }
                    commandIndex++;
                }
                
                if (printingOperation && currentArity == 0)
                {
                    sb.Append("\n");
                    printingOperation = false;
                }
            }

            return sb.ToString();
        }
        
        public static List<LifeToken> Decompile(byte[] genome)
        {
            var tokens = new List<LifeToken>();
            bool isParsingCommand = false;
            int argumentIndex = 0;
            ArtLifeGenome parsingCommand = default;

            Dictionary<int, Label> pregenLabels = new(genome.Length);
            for (int i = 0; i < genome.Length; i++)
            {
                var label = new Label($"_{i}");
                pregenLabels.Add(i, label);
            }
            
            for (int i = 0; i < genome.Length; i++)
            {
                tokens.Add(pregenLabels[i]);
                if (!isParsingCommand)
                {
                    if (TryParseGenome(genome[i], out parsingCommand))
                    {
                        if (GenomeCommandArity.TryGetValue(parsingCommand, out var arity))
                        {
                            tokens.Add(new GenomeOperation(parsingCommand));

                            if (arity.Count > 0)
                            {
                                isParsingCommand = true;
                                argumentIndex = 0;
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Unknown genome command: {genome[i]}");
                        }
                    }
                    else
                    {
                        tokens.Add(new Jump(genome[i]));
                    }
                }
                else
                {
                    var arityElement = GenomeCommandArity[parsingCommand][argumentIndex];
                    switch (arityElement)
                    {
                        case GenomeCommandOperandType.ValueArgument:
                            tokens.Add(new GenomeOperationArgument(genome[i]));
                            break;
                        case GenomeCommandOperandType.LabelPlaceholder:
                            var offset = argumentIndex + 1;
                            var branchPosition = i - offset;
                            var jumpOffset = genome[i];
                            var labelPosition = branchPosition + jumpOffset;
                            if (labelPosition < 0 || labelPosition >= genome.Length)
                            {
                                tokens.Add(new Jump(jumpOffset));
                            }
                            else
                            {
                                var label = pregenLabels[labelPosition];
                                tokens.Add(new LabelPlaceholder(label.Name, offset));
                            }
                            break;
                    }
                    argumentIndex++;
                    if (argumentIndex == GenomeCommandArity[parsingCommand].Count)
                    {
                        isParsingCommand = false;
                    }
                }
            }
            
            return tokens;
        }
        
        private static ArtLifeGenome[] _artLifeGenomeValues = Enum.GetValues(typeof(ArtLifeGenome))
            .Cast<ArtLifeGenome>()
            .ToArray();

        private static bool TryParseGenome(byte value, out ArtLifeGenome result)
        {
            foreach (var genome in _artLifeGenomeValues)
            {
                if ((byte)genome == value)
                {
                    result = genome;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}