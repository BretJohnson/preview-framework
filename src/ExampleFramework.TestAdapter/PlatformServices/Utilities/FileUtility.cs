// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !WINDOWS_UWP

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ExampleFramework.TestAdapter.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ExampleFramework.TestAdapter.PlatformServices.Utilities;

internal class FileUtility
{
    private readonly AssemblyUtility _assemblyUtility;

    public FileUtility()
    {
        _assemblyUtility = new AssemblyUtility();
    }

    public virtual void CreateDirectoryIfNotExists(string directory)
    {
        DebugEx.Assert(!string.IsNullOrEmpty(directory), "directory");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);    // Creates subdir chain if necessary.
        }
    }

    /// <summary>
    /// Replaces the invalid file/path characters from the parameter file name with '_'.
    /// </summary>
    /// <param name="fileName"> The file Name. </param>
    /// <returns> The fileName devoid of any invalid characters. </returns>
    public static string ReplaceInvalidFileNameCharacters(string fileName)
    {
        DebugEx.Assert(!string.IsNullOrEmpty(fileName), "fileName");

        return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, ch) => current.Replace(ch, '_'));
    }

    /// <summary>
    /// Checks whether directory with specified name exists in the specified directory.
    /// If it exits, adds [1],[2]... to the directory name and checks again.
    /// Returns full directory name (full path) of the iteration when the file does not exist.
    /// </summary>
    /// <param name="parentDirectoryName">The directory where to check.</param>
    /// <param name="originalDirectoryName">The original directory (that we would add [1],[2],.. in the end of if needed) name to check.</param>
    /// <returns>A unique directory name.</returns>
    public virtual string GetNextIterationDirectoryName(string parentDirectoryName, string originalDirectoryName)
    {
        DebugEx.Assert(!string.IsNullOrEmpty(parentDirectoryName), "parentDirectoryName");
        DebugEx.Assert(!string.IsNullOrEmpty(originalDirectoryName), "originalDirectoryName");

        uint iteration = 0;
        do
        {
            string tryMe;
            if (iteration == 0)
            {
                tryMe = originalDirectoryName;
            }
            else
            {
                tryMe = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", originalDirectoryName, iteration.ToString(CultureInfo.InvariantCulture));
            }

            string tryMePath = Path.Combine(parentDirectoryName, tryMe);

            if (!File.Exists(tryMePath) && !Directory.Exists(tryMePath))
            {
                return tryMePath;
            }

            ++iteration;
        }
        while (iteration != uint.MaxValue);

        // Return the original path in case file does not exist and let it fail.
        return Path.Combine(parentDirectoryName, originalDirectoryName);
    }

    public virtual List<string> AddFilesFromDirectory(string directoryPath, bool ignoreIOExceptions)
    {
        return AddFilesFromDirectory(directoryPath, null, ignoreIOExceptions);
    }

    public virtual List<string> AddFilesFromDirectory(string directoryPath, Func<string, bool>? ignoreDirectory, bool ignoreIOExceptions)
    {
        var fileContents = new List<string>();

        try
        {
            var files = GetFilesInADirectory(directoryPath);
            fileContents.AddRange(files);
        }
        catch (IOException)
        {
            if (!ignoreIOExceptions)
            {
                throw;
            }
        }

        foreach (var subDirectoryPath in GetDirectoriesInADirectory(directoryPath))
        {
            if (ignoreDirectory != null && ignoreDirectory(subDirectoryPath))
            {
                continue;
            }

            var subDirectoryContents = AddFilesFromDirectory(subDirectoryPath, ignoreDirectory, true);
            if (subDirectoryContents?.Count > 0)
            {
                fileContents.AddRange(subDirectoryContents);
            }
        }

        return fileContents;
    }

    public static string TryConvertPathToRelative(string path, string rootDir)
    {
        DebugEx.Assert(!string.IsNullOrEmpty(path), "path should not be null or empty.");
        DebugEx.Assert(!string.IsNullOrEmpty(rootDir), "rootDir should not be null or empty.");

        if (Path.IsPathRooted(path) && path.StartsWith(rootDir, StringComparison.OrdinalIgnoreCase))
        {
            return path.Substring(rootDir.Length).TrimStart(Path.DirectorySeparatorChar);
        }

        return path;
    }

    /// <summary>
    /// The function goes among the subdirectories of the specified one and clears all of
    /// them.
    /// </summary>
    /// <param name="filePath">The root directory to clear.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Requirement is to handle all kinds of user exceptions and message appropriately.")]
    public virtual void DeleteDirectories(string filePath)
    {
        Validate.IsFalse(string.IsNullOrEmpty(filePath), "Invalid filePath provided");
        try
        {
            var root = new DirectoryInfo(filePath);
            root.Delete(true);
        }
        catch (Exception ex)
        {
            EqtTrace.ErrorIf(EqtTrace.IsErrorEnabled, "DeploymentManager.DeleteDirectories failed for the directory '{0}': {1}", filePath, ex);
        }
    }

    public virtual bool DoesDirectoryExist(string deploymentDirectory)
    {
        return Directory.Exists(deploymentDirectory);
    }

    public virtual bool DoesFileExist(string testSource)
    {
        return File.Exists(testSource);
    }

    public virtual void SetAttributes(string path, FileAttributes fileAttributes)
    {
        File.SetAttributes(path, fileAttributes);
    }

    public virtual string[] GetFilesInADirectory(string directoryPath)
    {
        return Directory.GetFiles(directoryPath);
    }

    public virtual string[] GetDirectoriesInADirectory(string directoryPath)
    {
        return Directory.GetDirectories(directoryPath);
    }

    /// <summary>
    /// Returns either PDB file name from inside compiled binary or null if this cannot be done.
    /// Does not throw.
    /// </summary>
    /// <param name="path">path to symbols file.</param>
    /// <returns>Pdb file name or null if non-existent.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Requirement is to handle all kinds of user exceptions and message appropriately.")]
    private static string? GetSymbolsFileName(string? path)
    {
        if (string.IsNullOrEmpty(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
        {
            if (EqtTrace.IsWarningEnabled)
            {
                EqtTrace.Warning("Path is either null or invalid. Path = '{0}'", path);
            }

            return null;
        }

        string pdbFile = Path.ChangeExtension(path, ".pdb");
        if (File.Exists(pdbFile))
        {
            if (EqtTrace.IsInfoEnabled)
            {
                EqtTrace.Info("Pdb file found for path '{0}'", path);
            }

            return pdbFile;
        }

        return null;
    }
}

#endif
