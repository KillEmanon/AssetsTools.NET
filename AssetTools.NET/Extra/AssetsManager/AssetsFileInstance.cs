﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssetsTools.NET.Extra
{
    public class AssetsFileInstance
    {
        public Stream stream;
        public string path;
        public string name;
        public AssetsFile file;
        public AssetsFileTable table;
        public List<AssetsFileInstance> dependencies = new List<AssetsFileInstance>();
        public BundleFileInstance parentBundle = null;
        //for monobehaviours
        public Dictionary<uint, string> monoIdToName = new Dictionary<uint, string>();

        public AssetsFileInstance(Stream stream, string filePath, string root)
        {
            this.stream = stream;
            if (string.IsNullOrEmpty(root))
            {
                path = Path.GetFullPath(filePath);
                name = Path.Combine(root, Path.GetFileName(path));
            }
            else
            {
                path = Path.Combine(root, Path.GetFileName(filePath));
                name = filePath;
            }

            file = new AssetsFile(new AssetsFileReader(stream));
            table = new AssetsFileTable(file);
            dependencies.AddRange(
                Enumerable.Range(0, file.dependencies.dependencyCount)
                          .Select(d => (AssetsFileInstance)null)
            );
        }
        public AssetsFileInstance(FileStream stream, string root)
        {
            this.stream = stream;
            path = stream.Name;
            name = Path.Combine(root, Path.GetFileName(path));
            file = new AssetsFile(new AssetsFileReader(stream));
            table = new AssetsFileTable(file);
            dependencies.AddRange(
                Enumerable.Range(0, file.dependencies.dependencyCount)
                          .Select(d => (AssetsFileInstance)null)
            );
        }

        public AssetsFileInstance GetDependency(AssetsManager am, int depIdx)
        {
            if (dependencies[depIdx] == null)
            {
                string depPath = file.dependencies.dependencies[depIdx].assetPath;
                int instIndex = am.files.FindIndex(f => Path.GetFileName(f.path).ToLower() == Path.GetFileName(depPath).ToLower());
                if (instIndex == -1)
                {
                    string absPath = Path.Combine(path, depPath);
                    string localAbsPath = Path.Combine(path, Path.GetFileName(depPath));
                    if (File.Exists(absPath))
                    {
                        dependencies[depIdx] = am.LoadAssetsFile(File.OpenRead(absPath), true);
                    }
                    else if (File.Exists(localAbsPath))
                    {
                        dependencies[depIdx] = am.LoadAssetsFile(File.OpenRead(localAbsPath), true);
                    }
                }
                else
                {
                    dependencies[depIdx] = am.files[instIndex];
                }
            }
            return dependencies[depIdx];
        }
    }
}
