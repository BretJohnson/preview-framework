// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using SkiaSharp;

namespace VisualTestUtils.ImageHash.HashAlgorithms;

/// <summary>
/// Computes an average hash for an image by computing the average value of all its pixels and comparing it,
/// to each pixel in the image. Each bit is a pixel: 1 = higher than average, 0 = lower than average.
/// </summary>
public class AverageHash : IHashAlgorithm
{
    private const int WIDTH = 8;
    private const int HEIGHT = 8;
    private const int NRPIXELS = WIDTH * HEIGHT;
    private const ulong MOSTSIGNIFICANTBITMASK = 1UL << (NRPIXELS - 1);

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

                // Compute the average value
                var averageValue = 0U;
                for (var y = 0; y < HEIGHT; y++)
                {
                    Span<byte> row = grayscaleBitmap.GetPixelRow(y);
                    for (var x = 0; x < WIDTH; x++)
                    {
                        averageValue += row[x];
                    }
                }

                averageValue /= NRPIXELS;

                // Compute the hash: each bit is a pixel
                // 1 = higher than average, 0 = lower than average
                var mask = MOSTSIGNIFICANTBITMASK;

                for (var y = 0; y < HEIGHT; y++)
                {
                    Span<byte> row = grayscaleBitmap.GetPixelRow(y);
                    for (var x = 0; x < WIDTH; x++)
                    {
                        if (row[x] >= averageValue)
                        {
                            hash |= mask;
                        }

                        mask >>= 1;
                    }
                }

                return hash;
            }
        }
    }
}
