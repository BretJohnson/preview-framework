// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using SkiaSharp;

namespace VisualTestUtils.ImageHash;

/// <summary>
/// Helper methods.
/// </summary>
internal static class ImageHashExtensions
{
    /// <summary>
    /// Gets the pixel row for a given Y value.
    /// </summary>
    /// <param name="bitmap">The SKBitmap.</param>
    /// <param name="y">The Y value.</param>
    /// <returns>Array of bytes.</returns>
    public static byte[] GetPixelRow(this SKBitmap bitmap, int y)
    {
        ReadOnlySpan<byte> pixels = bitmap.GetPixelSpan();
        int width = bitmap.Width;
        var rowPixels = new byte[width];

        for (int x = 0; x < width; x++)
        {
            rowPixels[x] = pixels[(y * width) + x];
        }

        return rowPixels;
    }

    /// <summary>Calculate the hash of the image (stream) using the hashImplementation.</summary>
    /// <param name="hashImplementation">HashImplementation to calculate the hash.</param>
    /// <param name="stream">Stream should 'contain' raw image data.</param>
    /// <returns>hash value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="hashImplementation"/> or <paramref name="stream"/> is <see langword="null"/>.</exception>
    public static ulong Hash(this IHashAlgorithm hashImplementation, Stream stream)
    {
        if (hashImplementation == null)
        {
            throw new ArgumentNullException(nameof(hashImplementation));
        }

        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        using var image = SKBitmap.Decode(stream);
        return hashImplementation.Hash(image);
    }

    /// <summary>Calculate the hash of the image (byte array) using the hashImplementation.</summary>
    /// <param name="hashImplementation">HashImplementation to calculate the hash.</param>
    /// <param name="byteArray">ByteArray should 'contain' raw image data.</param>
    /// <returns>hash value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="hashImplementation"/> or <paramref name="byteArray"/> is <see langword="null"/>.</exception>
    public static ulong Hash(this IHashAlgorithm hashImplementation, byte[] byteArray)
    {
        if (hashImplementation == null)
        {
            throw new ArgumentNullException(nameof(hashImplementation));
        }

        if (byteArray == null)
        {
            throw new ArgumentNullException(nameof(byteArray));
        }

        using var image = SKBitmap.Decode(byteArray);
        return hashImplementation.Hash(image);
    }

    /// <summary>
    /// Converts a given SKBitmap to grayscale.
    /// </summary>
    /// <param name="resizedBitmap">The original bitmap.</param>
    /// <returns>Grayscale Image.</returns>
    public static SKBitmap ConvertToGrayscale(this SKBitmap resizedBitmap)
    {
        SKBitmap grayscaleBitmap =
            new SKBitmap(resizedBitmap.Width, resizedBitmap.Height, SKColorType.Gray8, SKAlphaType.Opaque);

        using (SKCanvas canvas = new SKCanvas(grayscaleBitmap))
        {
            using (SKPaint paint = new SKPaint())
            {
                paint.ColorFilter = SKColorFilter.CreateColorMatrix(new float[]
                {
                    0.299f, 0.299f, 0.299f, 0f, 0f,
                    0.587f, 0.587f, 0.587f, 0f, 0f,
                    0.114f, 0.114f, 0.114f, 0f, 0f,
                    0f, 0f, 0f, 1f, 0f,
                });
                canvas.DrawBitmap(resizedBitmap, 0, 0, paint);
            }
        }

        return grayscaleBitmap;
    }
}
