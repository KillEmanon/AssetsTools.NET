﻿using AssetsTools.NET;
using AssetsTools.NET.Extra;
using AssetsView.AssetHelpers;
using AssetsView.Structs;
using AssetsView.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetsView.Winforms
{
    public partial class StartScreen : Form
    {
        private AssetsManager helper;
        private AssetsFileInstance currentFile;
        private FSDirectory rootDir;
        private FSDirectory currentDir;
        private bool rsrcDataAdded;
        private PPtrMap pptrMap;

        public StartScreen()
        {
            InitializeComponent();

            assetList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            assetList.RowPrePaint += new DataGridViewRowPrePaintEventHandler(prePaint);
            assetList.Rows.Add(imageList.Images[1], "Open an asset with", "", "");
            assetList.Rows.Add(imageList.Images[1], "File > Add File", "", "");

            rsrcDataAdded = false;

            helper = new AssetsManager();
            helper.updateAfterLoad = false;
            if (!File.Exists("classdata.tpk"))
            {
                MessageBox.Show("classdata.tpk could not be found. Make sure it exists and restart.", "Assets View");
                Application.Exit();
            }
            helper.LoadClassPackage("classdata.tpk");
        }

        private void prePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string possibleBundleHeader;
                int possibleFormat;
                string emptyVersion;
                using (FileStream fs = File.OpenRead(ofd.FileName))
                using (AssetsFileReader reader = new AssetsFileReader(fs))
                {
                    if (fs.Length < 0x20)
                    {
                        MessageBox.Show("File too small. Are you sure this is a unity file?", "Assets View");
                        return;
                    }
                    possibleBundleHeader = reader.ReadStringLength(7);
                    reader.Position = 0x08;
                    possibleFormat = reader.ReadInt32();
                    reader.Position = 0x14;

                    string possibleVersion = "";
                    char curChar;
                    while (reader.Position < reader.BaseStream.Length && (curChar = (char)reader.ReadByte()) != 0x00)
                    {
                        possibleVersion += curChar;
                        if (possibleVersion.Length < 0xFF)
                        {
                            break;
                        }
                    }
                    emptyVersion = Regex.Replace(possibleVersion, "[a-zA-Z0-9\\.]", "");
                }
                if (possibleBundleHeader == "UnityFS")
                {
                    LoadBundleFile(ofd.FileName);
                    IEManager.Init(helper, currentFile, ofd.FileName, true);
                }
                else if (possibleFormat < 0xFF && emptyVersion == "")
                {
                    LoadAssetsFile(ofd.FileName);
                    IEManager.Init(helper, currentFile, ofd.FileName, false);
                }
                else
                {
                    MessageBox.Show("Couldn't detect file type. Are you sure this is a unity file?", "Assets View");
                }
            }
        }

        private void LoadBundleFile(string path)
        {
            OpenBundleDialog openFile = new OpenBundleDialog(helper, path);
            openFile.ShowDialog();
            if (openFile.selection > -1)
            {
                AssetBundleFile bundleFile = openFile.file;
                BundleFileInstance bundleInst = openFile.inst;
                List<byte[]> files = BundleHelper.LoadAllAssetsDataFromBundle(bundleFile);
                string rootPath = Path.GetDirectoryName(openFile.inst.path);
                if (files.Count > 0)
                {
                    if (files.Count > 1)
                    {
                        for (int i = 1; i < files.Count; i++)
                        {
                            MemoryStream stream = new MemoryStream(files[i]);
                            string name = bundleFile.bundleInf6.dirInf[i].name;
                            AssetsFileInstance inst = helper.LoadAssetsFile(stream, name, openFile.selection == 1, rootPath);
                            inst.parentBundle = bundleInst;
                        }
                    }
                    MemoryStream mainStream = new MemoryStream(files[0]);
                    string mainName = bundleFile.bundleInf6.dirInf[0].name;
                    AssetsFileInstance mainInst = helper.LoadAssetsFile(mainStream, mainName, openFile.selection == 1, rootPath);
                    mainInst.parentBundle = bundleInst;
                    LoadMainAssetsFile(mainInst);
                }
                else
                {
                    MessageBox.Show("No valid assets files found in the bundle.", "Assets View");
                }
            }
        }

        private void LoadAssetsFile(string path)
        {
            OpenAssetsDialog openFile = new OpenAssetsDialog(path);
            openFile.ShowDialog();
            if (openFile.selection > -1)
            {
                LoadMainAssetsFile(helper.LoadAssetsFile(path, openFile.selection == 1));
            }
        }

        public void LoadMainAssetsFile(AssetsFileInstance inst)
        {
            if (currentFile == null || Path.GetFullPath(currentFile.path) != Path.GetFullPath(inst.path))
            {
                inst.table.GenerateQuickLookupTree();
                helper.UpdateDependencies();
                helper.LoadClassDatabaseFromPackage(inst.file.typeTree.unityVersion);
                if (helper.classFile == null)
                {
                    //may still not work but better than nothing I guess
                    //in the future we should probably do a selector
                    //like uabe does
                    ClassDatabaseFile[] files = helper.classPackage.files;
                    helper.classFile = files[files.Length - 1];
                }
                UpdateFileList();
                currentFile = inst;

                string ggmPath = Path.Combine(Path.GetDirectoryName(inst.path), "globalgamemanagers");
                if (inst.name == "resources.assets" && File.Exists(ggmPath))
                {
                    if (MessageBox.Show("Load resources.assets in directory mode? (Significantly faster)", "Assets View",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        AssetsFileInstance ggmInst = helper.LoadAssetsFile(ggmPath, true);
                        helper.UpdateDependencies();
                        LoadResources(ggmInst);
                    }
                    else
                    {
                        LoadGeneric(inst, false);
                    }
                }
                else
                {
                    LoadGeneric(inst, false);
                }

                string[] vers = helper.classFile.header.unityVersions;
                string corVer = vers.FirstOrDefault(v => !v.Contains("*"));
                Text = "AssetsView .NET - ver " + inst.file.typeTree.unityVersion + " / db " + corVer;
            }
        }

        private void clearFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            assetTree.Nodes.Clear();
            helper.files.ForEach(d => {
                if (d != null)
                {
                    d.file.readerPar.Close(); d.table.assetFileInfo = null;
                }
            });
            helper.files.Clear();
            rootDir = null;
            currentFile = null;
            assetList.Rows.Clear();
        }

        private void LoadResources(AssetsFileInstance ggm)
        {
            foreach (AssetFileInfoEx info in ggm.table.assetFileInfo)
            {
                ClassDatabaseType type = AssetHelper.FindAssetClassByID(helper.classFile, info.curFileType);
                if (type.name.GetString(helper.classFile) == "ResourceManager")
                {
                    AssetTypeInstance inst = helper.GetTypeInstance(ggm.file, info);
                    AssetTypeValueField baseField = inst.GetBaseField();
                    AssetTypeValueField m_Container = baseField.Get("m_Container").Get("Array");
                    List<AssetDetails> assets = new List<AssetDetails>();
                    for (int i = 0; i < m_Container.GetValue().AsArray().size; i++)
                    {
                        AssetTypeValueField item = m_Container[i];
                        string path = item.Get("first").GetValue().AsString();
                        AssetTypeValueField pointerField = item.Get("second");
                        //paths[path] = new AssetDetails(new AssetPPtr(fileID, pathID));

                        AssetExternal assetExt = helper.GetExtAsset(ggm, pointerField, true);
                        AssetFileInfoEx assetInfo = assetExt.info;
                        if (assetInfo == null)
                            continue;
                        ClassDatabaseType assetType = AssetHelper.FindAssetClassByID(helper.classFile, assetInfo.curFileType);
                        if (assetType == null)
                            continue;
                        string assetTypeName = assetType.name.GetString(helper.classFile);
                        string assetName = AssetHelper.GetAssetNameFast(assetExt.file.file, helper.classFile, assetInfo);
                        if (path.Contains("/"))
                        {
                            if (path.Substring(path.LastIndexOf('/') + 1) == assetName.ToLower())
                            {
                                path = path.Substring(0, path.LastIndexOf('/') + 1) + assetName;
                            }
                        }
                        else
                        {
                            if (path == assetName.ToLower())
                            {
                                path = path.Substring(0, path.LastIndexOf('/') + 1) + assetName;
                            }
                        }

                        assets.Add(new AssetDetails(new AssetPPtr(0, assetInfo.index), GetIconForName(assetTypeName), path, assetTypeName, (int)assetInfo.curFileSize));
                    }
                    rootDir = new FSDirectory();
                    //rootDir.Create(paths);
                    rootDir.Create(assets);
                    ChangeDirectory("");
                    helper.UpdateDependencies();
                    CheckResourcesInfo();
                    return;
                }
            }
        }

        private void LoadGeneric(AssetsFileInstance mainFile, bool isLevel)
        {
            List<AssetDetails> assets = new List<AssetDetails>();
            foreach (AssetFileInfoEx info in mainFile.table.assetFileInfo)
            {
                ClassDatabaseType type = AssetHelper.FindAssetClassByID(helper.classFile, info.curFileType);
                if (type == null)
                    continue;
                string typeName = type.name.GetString(helper.classFile);
                if (typeName != "GameObject" && isLevel)
                    continue;
                string name = AssetHelper.GetAssetNameFast(mainFile.file, helper.classFile, info);
                if (name == "")
                {
                    name = "[Unnamed]";
                }
                assets.Add(new AssetDetails(new AssetPPtr(0, info.index), GetIconForName(typeName), name, typeName, (int)info.curFileSize));
            }
            rootDir = new FSDirectory();
            rootDir.Create(assets);
            ChangeDirectory("");
        }

        private void ChangeDirectory(string path)
        {
            path = path.TrimEnd('/');
            if (path.StartsWith("/") || path == "")
            {
                currentDir = rootDir;
            }
            if (path != "" && path != "/")
            {
                string[] paths = path.Replace('\\', '/').Split('/');
                foreach (string dir in paths)
                {
                    FSDirectory searchDir = currentDir.children.Where(
                        d => d is FSDirectory && d.name == dir
                    ).FirstOrDefault() as FSDirectory;
                    
                    if (searchDir != null)
                        currentDir = searchDir;
                    else
                        return;
                }
            }
            UpdateDirectoryList();
        }

        public void UpdateFileList()
        {
            assetTree.Nodes.Clear();
            foreach (AssetsFileInstance dep in helper.files)
            {
                assetTree.Nodes.Add(dep.name);
            }
        }

        public void UpdateDirectoryList()
        {
            assetList.Rows.Clear();
            pathBox.Text = currentDir.path;
            assetList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            Image[] images = new Image[imageList.Images.Count];
            for (int i = 0; i < imageList.Images.Count; i++)
            {
                images[i] = imageList.Images[i];
            }
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            LoadingBar lb = new LoadingBar();
            if (currentDir.children.Count > 1000)
            {
                lb.Show(this);
            }

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            lb.pb.Maximum = currentDir.children.Count * 2;
            bw.DoWork += delegate
            {
                int prog = 0;
                foreach (FSObject obj in currentDir.children)
                {
                    if (obj is FSDirectory)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.Height = 32;
                        row.CreateCells(assetList, images[(int)AssetIcon.Folder], obj.name, "Folder", "", 0);
                        rows.Add(row);
                    }
                    prog++;
                    if (prog % 50 == 0)
                        bw.ReportProgress(prog);
                }
                foreach (FSObject obj in currentDir.children)
                {
                    if (obj is FSAsset assetObj)
                    {
                        AssetDetails dets = assetObj.details;
                        DataGridViewRow row = new DataGridViewRow();
                        row.Height = 32;
                        row.CreateCells(assetList, images[(int)dets.icon], assetObj.name, dets.type, dets.pointer.pathID, dets.size);
                        rows.Add(row);
                    }
                    prog++;
                    if (prog % 50 == 0)
                        bw.ReportProgress(prog);
                }
            };
            bw.ProgressChanged += delegate (object s, ProgressChangedEventArgs ev)
            {
                lb.pb.Value = ev.ProgressPercentage;
            };
            bw.RunWorkerCompleted += delegate
            {
                assetList.Rows.AddRange(rows.ToArray());
                lb.Close();
                assetList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            };
            bw.RunWorkerAsync();
        }
        
        private void upDirectory_Click(object sender, EventArgs e)
        {
            if (currentDir != null && currentDir.parent != null)
            {
                currentDir = currentDir.parent;
                UpdateDirectoryList();
            }
        }

        private void AssetList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == MouseButtons.Right)
            {
                DataGridViewCell c = dgv[e.ColumnIndex, e.RowIndex];
                dgv.ClearSelection();
                dgv.CurrentCell = c;
                c.Selected = true;

                var selRow = dgv.Rows[e.RowIndex];
                string typeName = (string)selRow.Cells[2].Value;
                if (typeName == "Folder")
                {
                    viewTextureToolStripMenuItem.Visible = false;
                }
                else
                {
                    AssetFileInfoEx info = currentFile.table.GetAssetInfo((long)selRow.Cells[3].Value);
                    viewTextureToolStripMenuItem.Visible = info.curFileType == 0x1C;
                }

                Point p = dgv.PointToClient(Cursor.Position);
                contextMenuStrip.Show(dgv, p);
            }
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
                return;
            if (assetList.SelectedCells.Count > 0)
            {
                var selRow = assetList.SelectedRows[0];
                string assetName = (string)selRow.Cells[1].Value;
                string typeName = (string)selRow.Cells[2].Value;
                if (typeName == "Folder")
                {
                    AssetInfoViewer viewer = new AssetInfoViewer(
                        assetName,
                        string.Empty //todo
                    );
                    viewer.ShowDialog();
                }
                else
                {
                    AssetFileInfoEx info = currentFile.table.GetAssetInfo((long)selRow.Cells[3].Value);
                    ushort monoId = currentFile.file.typeTree.unity5Types[info.curFileTypeOrIndex].scriptIndex;
                    AssetInfoViewer viewer = new AssetInfoViewer(
                        info.curFileType,
                        info.absoluteFilePos,
                        info.curFileSize,
                        info.index,
                        monoId,
                        assetName,
                        typeName,
                        string.Empty //todo
                    );
                    viewer.ShowDialog();
                }
            }
        }


        private void viewTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
                return;
            if (assetList.SelectedCells.Count > 0)
            {
                var selRow = assetList.SelectedRows[0];
                AssetFileInfoEx info = currentFile.table.GetAssetInfo((long)selRow.Cells[3].Value);
                AssetTypeValueField baseField = helper.GetTypeInstance(currentFile.file, info).GetBaseField();

                TextureViewer texView = new TextureViewer(currentFile, baseField);
                texView.Show();
            }
        }

        private void xRefsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
                return;
            if (assetList.SelectedCells.Count > 0)
            {
                var selRow = assetList.SelectedRows[0];
                long pathId = (long)selRow.Cells[3].Value;
                string assetDir = Path.GetDirectoryName(currentFile.path);

                if (pptrMap == null)
                {
                    string avpmFilePath = Path.Combine(assetDir, "avpm.dat");
                    if (File.Exists(avpmFilePath))
                    {
                        pptrMap = new PPtrMap(new BinaryReader(File.OpenRead(avpmFilePath)));
                    }
                    else
                    {
                        MessageBox.Show("avpm.dat file does not exist.\nTry running Global Search -> PPtr.", "Assets View");
                        return;
                    }
                }
                XRefsDialog xrefs = new XRefsDialog(this, helper, assetDir, pptrMap, new AssetID(currentFile.name, pathId));
                xrefs.Show();
            }
        }         

        /// <summary>
        /// Emanon
        /// 导入Dump后的MonoBehaviour文件,覆盖对于文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Emanon
        /// 导出Dump后的MonoBehaviour文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExportAssets(long id)
        {
            try
            {
                ExportUtils.ExportAssets(id);

            }
            catch (Exception e)
            {
                Console.WriteLine("path id:" + id);
                Console.WriteLine(e.Message);
            }
        }

        private void ImportAssets()
        {
            var inf = currentFile.table.GetAssetInfo("MyBoringAsset");
            var baseField = helper.GetATI(currentFile.file, inf).GetBaseField();
            baseField.Get("m_Name")
                     .GetValue()
                     .Set("MyCoolAsset");
            var newGoBytes = baseField.WriteToByteArray();
            //AssetsReplacerFromMemory's monoScriptIndex should always be 0xFFFF unless it's a MonoBehaviour
            var repl = new AssetsReplacerFromMemory(0, inf.index, (int)inf.curFileType, 0xFFFF, newGoBytes);
            var writer = new AssetsFileWriter(File.OpenWrite("resources-modified.assets"));
            currentFile.file.Write(writer, 1, new AssetsReplacer[] { repl }.ToList(), 1);
        }

        private void assetList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (currentFile == null)
                return;
            if (assetList.SelectedCells.Count > 0)
            {
                var selRow = assetList.SelectedRows[0];
                string typeName = (string)selRow.Cells[2].Value;
                if (typeName == "Folder")
                {
                    string dirName = (string)selRow.Cells[1].Value;
                    ChangeDirectory(dirName);
                }
                else
                {
                    OpenAsset((long)selRow.Cells[3].Value);
                }
            }
        }

        public static AssetTypeValueField GetRootTransform(AssetsManager helper, AssetsFileInstance currentFile, AssetTypeValueField transform)
        {
            AssetTypeValueField fatherPtr = transform["m_Father"];
            if (fatherPtr["m_PathID"].GetValue().AsInt64() != 0)
            {
                AssetTypeValueField father = helper.GetExtAsset(currentFile, fatherPtr).instance.GetBaseField();
                return GetRootTransform(helper, currentFile, father);
            }
            else
            {
                return transform;
            }
        }

        public void OpenAsset(long id)
        {
            GameObjectViewer view = new GameObjectViewer(helper, currentFile, id);
            view.Show();
        }

        private void RecurseForResourcesInfo(FSDirectory dir, AssetsFileInstance afi)
        {
            foreach (FSAsset asset in dir.children.OfType<FSAsset>())
            {
                AssetFileInfoEx info = afi.table.GetAssetInfo(asset.details.pointer.pathID);
                ClassDatabaseType type = AssetHelper.FindAssetClassByID(helper.classFile, info.curFileType);
                string typeName = type.name.GetString(helper.classFile);

                asset.details.type = typeName;
                asset.details.size = (int)info.curFileSize;
                asset.details.icon = GetIconForName(typeName);
            }
            foreach (FSDirectory directory in dir.children.OfType<FSDirectory>())
            {
                RecurseForResourcesInfo(directory, afi);
            }
        }

        private void CheckResourcesInfo()
        {
            if (currentFile.name == "globalgamemanagers" && rsrcDataAdded == false && helper.files.Any(f => f.name == "resources.assets"))
            {
                AssetsFileInstance afi = helper.files.First(f => f.name == "resources.assets");
                RecurseForResourcesInfo(rootDir, afi);
                rsrcDataAdded = true;
                UpdateDirectoryList();
            }
        }

        public void UpdateDependencies()
        {
            helper.UpdateDependencies();
            UpdateFileList();
            //CheckResourcesInfo();
        }

        private AssetIcon GetIconForName(string type)
        {
            if (Enum.TryParse(type, out AssetIcon res))
            {
                return res;
            }
            return AssetIcon.Unknown;
        }

        string lastSearchedAsset = "";
        int lastSearchedIndex = -1;
        private void SearchAsset()
        {
            string text = pathBox.Text;
            int startIndex = 0;
            if (text == lastSearchedAsset)
            {
                startIndex = lastSearchedIndex;
            }
            else
            {
                lastSearchedAsset = "";
                lastSearchedIndex = -1;
            }
            int cellIdx = 1;
            if (text.StartsWith("$id="))
            {
                text = text.Substring(4);
                cellIdx = 3;
            }
            text = text.ToLower();
            assetList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewRow row in assetList.Rows)
            {
                if (row.Index < startIndex)
                    continue;
                assetList.ClearSelection();
                string matchText = row.Cells[cellIdx].Value.ToString().ToLower();
                if (Regex.IsMatch(matchText, WildCardToRegular(text)))
                {
                    row.Selected = true;
                    assetList.FirstDisplayedScrollingRowIndex = row.Index;
                    lastSearchedAsset = pathBox.Text;
                    lastSearchedIndex = row.Index + 1;
                    return;
                }
            }
            lastSearchedIndex = -1;
        }

        private string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
        }

        //updateDependencies(更新依赖)按键执行的方法
        private void updateDependenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
            {
                MessageBox.Show("No current file selected!", "Assets View");
                return;
            }
            if (!AssetUtils.AllDependenciesLoaded(helper, currentFile))
            {
                DialogResult res = MessageBox.Show(
                    "Load all referenced dependencies?",
                    "Assets View",
                    MessageBoxButtons.YesNo);
                if (res == DialogResult.No)
                {
                    return;
                }
                helper.LoadAssetsFile(currentFile.stream, currentFile.path, true);
                UpdateDependencies();
            }
            else
            {
                MessageBox.Show(
                    "All dependencies already loaded.",
                    "Assets View");
            }
        }

        private void GoDirectory_Click(object sender, EventArgs e)
        {
            SearchAsset();
        }

        private void PathBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchAsset();
                e.Handled = true;
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutScreen().ShowDialog();
        }

        private void ViewCurrentAssetInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
            {
                MessageBox.Show("No current file selected!", "Assets View");
                return;
            }
            new AssetsFileInfoViewer(currentFile.file, helper.classFile).Show();
        }

        private void AssetTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            AssetsFileInstance inst = helper.files[e.Node.Index];
            inst.table.GenerateQuickLookupTree();
            helper.UpdateDependencies();
            UpdateFileList();
            currentFile = inst;
            LoadGeneric(inst, false);
            IEManager.AssetsFileInstance = inst;
        }

        private string SelectFolderAndLoad()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                CheckFileExists = false,
                FileName = "[select folder]",
                Title = "Select folder to scan"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string dirName = Path.GetDirectoryName(ofd.FileName);
                if (Directory.Exists(dirName))
                {
                    return dirName;
                }
                else
                {
                    MessageBox.Show("Directory does not exist.", "Assets View");
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private void monoBehaviourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dirName = SelectFolderAndLoad();
            new MonoBehaviourScanner(this, helper, dirName).Show();
        }

        private void assetDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dirName = SelectFolderAndLoad();
            new AssetDataScanner(this, helper, dirName).Show();
        }

        private void exportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inst = IEManager.AssetsFileInstance;

            foreach(var item in inst.table.GetLookupBase())
            {
                string typeName = IEManager.GetTypeName(item.Key);
                if (typeName == "MonoBehaviour")
                    ExportAssets(item.Key);
            }
            MessageBox.Show("导出成功");
        }

        private void exportOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile == null)
                return;
            if (assetList.SelectedCells.Count > 0)
            {
                var selRow = assetList.SelectedRows[0];
                string typeName = (string)selRow.Cells[2].Value;
                if (typeName == "Folder")
                {
                    string dirName = (string)selRow.Cells[1].Value;
                    ChangeDirectory(dirName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(fileIDTextBox.Text))
                    {
                        ExportAssets(long.Parse(fileIDTextBox.Text));
                    }
                    else
                    {
                        ExportAssets((long)selRow.Cells[3].Value);
                    }

                    MessageBox.Show("导出成功");
                }
            }
        }

        /// <summary>
        /// 导入整个文件夹的mono
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imporDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEManager.InputStr = fileIDTextBox.Text;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择要覆盖的Aseets";
            string targetFileName = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (currentFile == null)
                    return;
                if (assetList.SelectedCells.Count > 0)
                {
                    var selRow = assetList.SelectedRows[0];
                    string typeName = (string)selRow.Cells[2].Value;
                    if (typeName == "Folder")
                    {
                        string dirName = (string)selRow.Cells[1].Value;
                        ChangeDirectory(dirName);
                        return;
                    }
                    else
                    {
                        targetFileName = ofd.FileName;
                    }
                }
            }
            else
                return;

            OpenFileDialog ofd2 = new OpenFileDialog
            {
                CheckFileExists = false,
                FileName = "[选择文件夹]",
                Title = "选择要导入的文件夹"
            };
            if (ofd2.ShowDialog() == DialogResult.OK)
            {
                string dirName = Path.GetDirectoryName(ofd2.FileName);
                if (Directory.Exists(dirName))
                {
                    ImportUtils.ImportAssets(dirName, targetFileName);
                    MessageBox.Show("导入成功");
                }
                else
                {
                    MessageBox.Show("文件夹不存在.", "Assets View");
                }
            }
        }

        /// <summary>
        /// 导入单个mono文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEManager.InputStr = fileIDTextBox.Text;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择要覆盖的Aseets";
            string targetFileName = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (currentFile == null)
                    return;
                if (assetList.SelectedCells.Count > 0)
                {
                    var selRow = assetList.SelectedRows[0];
                    string typeName = (string)selRow.Cells[2].Value;
                    if (typeName == "Folder")
                    {
                        string dirName = (string)selRow.Cells[1].Value;
                        ChangeDirectory(dirName);
                        return;
                    }
                    else
                    {
                        targetFileName = ofd.FileName;
                    }
                }
            }
            else
                return;

            OpenFileDialog ofd2 = new OpenFileDialog
            {
                CheckFileExists = false,
                FileName = "[选择文件]",
                Title = "选择要导入的文件"
            };
            if (ofd2.ShowDialog() == DialogResult.OK)
            {
                if (assetList.SelectedCells.Count > 0)
                {
                    var selRow = assetList.SelectedRows[0];
                    if (!string.IsNullOrEmpty(fileIDTextBox.Text))
                    {
                        ImportUtils.ImportAssets(long.Parse(fileIDTextBox.Text), ofd2.FileName, targetFileName);
                    }
                    else
                    {
                        ImportUtils.ImportAssets((long)selRow.Cells[3].Value, ofd2.FileName, targetFileName);
                    }
                    MessageBox.Show("导入成功");
                }
            }
        }

        private void pptrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dirName = SelectFolderAndLoad();
            new PPtrScanner(helper, dirName).Show();
        }

    }
}
