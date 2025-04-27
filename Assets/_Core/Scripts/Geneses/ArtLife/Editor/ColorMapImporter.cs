using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Geneses.ArtLife
{
    [ScriptedImporter(1, "colormap")]
    public class ColorMapImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var colorMap = ScriptableObject.CreateInstance<ColorMap>();
            var scalars = new List<float>();
            var colors = new List<Color>();

            try
            {
                using (var reader = new StreamReader(ctx.assetPath))
                {
                    // Read and validate the header
                    var header = reader.ReadLine();
                    if (header == null || !header.Contains("scalar,RGB_r,RGB_g,RGB_b"))
                    {
                        throw new FormatException("Invalid CSV header format.");
                    }

                    // Parse each line
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');
                        if (values.Length != 4)
                        {
                            throw new FormatException("Invalid CSV row format.");
                        }

                        // Parse scalar and RGB values
                        var scalar = float.Parse(values[0], CultureInfo.InvariantCulture);
                        var r = float.Parse(values[1], CultureInfo.InvariantCulture);
                        var g = float.Parse(values[2], CultureInfo.InvariantCulture);
                        var b = float.Parse(values[3], CultureInfo.InvariantCulture);

                        scalars.Add(scalar);
                        colors.Add(new Color(r, g, b));
                    }
                }

                colorMap.Initialize(scalars, colors);
                ctx.AddObjectToAsset("ColorMap", colorMap);
                ctx.SetMainObject(colorMap);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to import colormap: {ex.Message}");
            }
        }
    }
}