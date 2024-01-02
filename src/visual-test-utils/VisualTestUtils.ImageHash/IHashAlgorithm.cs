// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using SkiaSharp;

namespace VisualTestUtils.ImageHash;

/// <summary>
/// Interface for perceptual image hashing algorithm.
/// </summary>
public interface IHashAlgorithm
{
    /// <summary>Hash the image using the algorithm.</summary>
    /// <param name="bitmap">image to calculate hash from.</param>
    /// <returns>hash value of the image.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bitmap"/> is <see langword="null"/>.</exception>
    ulong Hash(SKBitmap bitmap);
}
