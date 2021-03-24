using AssetsTools.NET;
using AssetsTools.NET.Extra;
using AssetsView.Util;
using AssetsView.Winforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetsView.AssetHelpers
{
    class IEManager
    {
        private static AssetsManager assetsManager;
        private static AssetsFileInstance assetsFileInstance;

        private static string dirctoryPath;
        private static string assetsFileName;
        private static string inputStr;

        private static bool isBundle;

        public static void Init(AssetsManager assetsManager, AssetsFileInstance assetsFileInstance, string path, bool isBundle)
        {
            IEManager.assetsManager = assetsManager;
            IEManager.assetsFileInstance = assetsFileInstance;
            dirctoryPath = Path.GetDirectoryName(path);
            assetsFileName = Path.GetFileNameWithoutExtension(path);
            IEManager.isBundle = isBundle;
        }

        public static AssetTypeValueField GetField(long id)
        {
            string temp;
            return GetField(id, out temp);
        }

        public static AssetTypeValueField GetField(long id, out string className)
        {
            className = "";
            string path = DirctoryPath;
            AssetsManager helper = AssetsManager;
            ClassDatabaseFile classFile = helper.classFile;
            AssetsFileInstance correctAti = AssetsFileInstance;
            AssetFileInfoEx info = correctAti.table.GetAssetInfo(id);

            if(info == null)
            {
                Console.WriteLine($"path_id:{id} is not found,maybe in other file");
                return null;
            }

            ClassDatabaseType classType = AssetHelper.FindAssetClassByID(classFile, info.curFileType);
            string typeName = classType.name.GetString(classFile);
            AssetTypeValueField baseField = helper.GetATI(correctAti.file, info).GetBaseField();
            className = AssetHelper.FindAssetClassByID(helper.classFile, info.curFileType)
                .name.GetString(helper.classFile);
            AssetTypeValueField targetBaseField = baseField;
            if (className == "MonoBehaviour")
            {
                if (AssetUtils.AllDependenciesLoaded(helper, correctAti))
                {
                    className += $"--{GetClassName(helper, correctAti, targetBaseField)}--{targetBaseField[3].value.AsString().TrimEnd('\0')}--";
                    string managedPath = Path.Combine(Path.GetDirectoryName(correctAti.path), "Managed");
                    if (Directory.Exists(managedPath))
                    {
                        targetBaseField = helper.GetMonoBaseFieldCached(correctAti, info, managedPath);
                    }
                }
                else
                {
                    MessageBox.Show("Can't display MonoBehaviour data until dependencies are loaded", "Assets View");
                    return null;
                }
            }
            else
            {
                className += "--";
            }
            return targetBaseField;
        }

        public static string GetTypeName(long id)
        {
            AssetsManager helper = AssetsManager;
            AssetsFileInstance correctAti = AssetsFileInstance;
            AssetFileInfoEx info = correctAti.table.GetAssetInfo(id);
            return AssetHelper.FindAssetClassByID(helper.classFile, info.curFileType).name.GetString(helper.classFile);
        }

        /// <summary>
        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        public static AssetsManager AssetsManager { get => assetsManager; set => assetsManager = value; }
        public static AssetsFileInstance AssetsFileInstance { get => assetsFileInstance; set => assetsFileInstance = value; }
        public static string DirctoryPath { get => dirctoryPath; set => dirctoryPath = value; }
        public static string AssetsFileName { get => assetsFileName; set => assetsFileName = value; }
        public static string InputStr { get => inputStr; set => inputStr = value; }
        public static bool IsBundle { get => isBundle; set => isBundle = value; }

        private static string GetClassName(AssetsManager manager, AssetsFileInstance inst, AssetTypeValueField baseField)
        {
            AssetTypeInstance scriptAti = manager.GetExtAsset(inst, baseField.Get("m_Script")).instance;
            return scriptAti.GetBaseField().Get("m_Name").GetValue().AsString();
        }
    }
}
