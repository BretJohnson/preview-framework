// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using SkiaSharp;

namespace VisualTestUtils.ImageHash.HashAlgorithms;

/// <summary>
/// Calculates the hash difference between two bitmap images.
/// </summary>
public class DifferenceHash : IHashAlgorithm
{
    private const int WIDTH = 9;
    private const int HEIGHT = 8;

    /// <inheritdoc />
    public ulong Hash(SKBitmap bitmap)
    {
        if (bitmap == null)
        {
            throw new ArgumentNullException(nameof(bitmap));
        }

        // Resize the bitmap
        using (SKBitmap resizedBitmap = bitmap.Resize(new SKImageInfo(WIDTH, HEIGHT), SKFilterQuality.High))
        {
            // Convert the bitmap to grayscale
            using (SKBitmap grayscaleBitmap = resizedBitmap.ConvertToGrayscale())
            {
                var hash = 0UL;

                // Compute the hash
                var mask = 1UL << ((HEIGHT * (WIDTH - 1)) - 1);

                for (var y = 0; y < HEIGHT; y++)
                {
                    Span<byte> row = grayscaleBitmap.GetPixelRow(y);
                    byte leftPixel = row[0];

                    for (var index = 1; index < WIDTH; index++)
                    {
                        byte rightPixel = row[index];
                        if (leftPixel < rightPixel)
                        {
                            hash |= mask;
                        }

                        leftPixel = rightPixel;
                        mask >>= 1;
                    }
                }

                return hash;
            }
        }
    }
}
