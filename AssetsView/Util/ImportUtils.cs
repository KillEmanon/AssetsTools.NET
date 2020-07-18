using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using AssetsView.AssetHelpers;
using Newtonsoft.Json;

namespace AssetsView.Util
{
    class ImportUtils
    {
		static string logPath;

		public static void ImportAssets(string directoryPath, string targetFileName)
		{
			if (Directory.Exists(directoryPath))
			{
				var files = Directory.GetFiles(directoryPath);
				var replList = new List<AssetsReplacer>();
				//var str = IEManager.InputStr;
				//var arr = str.Split('-');

				//清空LOG文件
				logPath = Path.GetFileNameWithoutExtension(targetFileName) + ".txt";
				File.WriteAllText(logPath, "", Encoding.UTF8);

				for (int i = 0; i < files.Length; i++)
				{
					//锁定范围(920,1000]
					//if (i >= 920 && i <= 960) continue;
					//if (i >= 1000 && i <= 1500) continue;
					//if (i >= int.Parse(arr[0]) && i <= int.Parse(arr[1])) continue;

					var splitArr = Path.GetFileNameWithoutExtension(files[i]).Split(new string[] { "--" }, StringSplitOptions.None);
					var temp = GenReplacerFromMemory(long.Parse(splitArr[3]), files[i]);
					if(temp != null) replList.Add(temp);
				}

				var inst = IEManager.AssetsFileInstance;
				var writer = new AssetsFileWriter(File.OpenWrite(targetFileName));
				//var writer = new AssetsFileWriter(File.OpenWrite(@"E:\AndroidStudioProject\Slime\Slime\src\main\assets\bin\Data\sharedassets0.assets")); 
				inst.file.Write(writer, 0, replList, 0);
				writer.Close();
			}
		}

		public static void ImportAssets(long path_id, string filedFileName, string targetFileName)
		{
			var inst = IEManager.AssetsFileInstance;
			var repl = GenReplacerFromMemory(path_id, filedFileName);
			var writer = new AssetsFileWriter(File.OpenWrite(targetFileName));
			inst.file.Write(writer, 0, new AssetsReplacer[] { repl }.ToList(), 0);
			writer.Close();
		}

		public static AssetsReplacerFromMemory GenReplacerFromMemory(long path_id, string filedFileName)
		{
			AssetTypeValueField baseField;

			int lines = 0;
			UTF8Encoding utf8 = new UTF8Encoding(false);
			string[] contents = File.ReadAllLines(filedFileName, utf8);
			//TODO 过长的数据库类现在先不处理
			if (contents.Length > 10000)
			{
				Console.WriteLine($"文件过大,暂不处理--{path_id}--{filedFileName}");
				File.AppendAllLines(logPath, new string[] { $"文件过大,暂不处理--{path_id}--{filedFileName}" });
				return null;
			}

			baseField = IEManager.GetField(path_id);
			
			var sucess = ChangeField(baseField, contents, ref lines);
			if (!sucess)
			{
				Console.WriteLine($"处理失败,有null值--{path_id}--{filedFileName}");
				File.AppendAllLines(logPath, new string[] { $"处理失败,有null值--{path_id}--{filedFileName}" });
				return null;
			}

			var inst = IEManager.AssetsFileInstance;
			var inf = inst.table.GetAssetInfo(path_id);
			var newGoBytes = baseField.WriteToByteArray();
			ushort monoId = inst.file.typeTree.unity5Types[inf.curFileTypeOrIndex].scriptIndex;

			return new AssetsReplacerFromMemory(0, path_id, (int)inf.curFileType, monoId, newGoBytes);
		}

		public static bool ChangeField(AssetTypeValueField field, string[] contents, ref int lines)
		{
            for (int i = 0; i < field.childrenCount; i++)
            {
                string str = contents[lines];

                string[] fieldStr = str.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);

                field[i].templateField.type = fieldStr[0];
                field[i].templateField.name = fieldStr[1];

				//某个搞事的类型,简直了
				if(fieldStr[0] == "unsigned")
				{
					field[i].templateField.type = $"{fieldStr[0]} {fieldStr[1]}";
					field[i].templateField.name = fieldStr[2];
					fieldStr[3] = fieldStr[4];
				}
				
				//字符串的特殊处理
				//TODO 多行拼接有bug需要修复
				if (fieldStr[0] == "string")
				{
					string replace = fieldStr[0] + " " + fieldStr[1] + " = ";
					fieldStr[3] = str.Replace(replace, "").Trim().Replace("\"", "").Replace("\\n", "\n");

					//value = value.Substring(1, value.Length - 1);
					////不是单行字符串需要追踪全部行数
					//if (str.Last() != '"')
					//{
					//	lines++;
					//	//还可能存在字符串内自带"的情况,先不考虑,不出问题就不处理,以"结尾不出意外应该是没问题的
					//	while (contents[lines] == string.Empty || contents[lines].Last() != ('"'))
					//	{
					//		lines++;
					//		value += "\n" + contents[lines];
					//	}
					//}
					//value = value.Substring(0, value.Length - 1);
					//fieldStr[3] = value;
				}
				//bool的特殊处理
				if(fieldStr[0] == "bool")
				{
					if (fieldStr[3] == "true")
						fieldStr[3] = "1";
					else
						fieldStr[3] = "0";
				}

                if (field[i].childrenCount >= 1)
                {
					lines++;
                    ChangeField(field[i], contents, ref lines);
                }
                else
                {
					lines++;
					//field[i].value.type = StringToEnum(fieldStr[0]);		//可以改变字段类型,是否有必要还是个未知数
					//用于针对TypeNull
					if (fieldStr.Length >= 4)
						field[i].value.Set(fieldStr[3]);
					else
						return false;
				}
            }
			return true;
        }

		public static EnumValueTypes StringToEnum(string type)
		{
			switch (type)
			{
				case "int":
					return EnumValueTypes.ValueType_Int32;
				case "UInt8":
					return EnumValueTypes.ValueType_UInt8;
				case "SInt64":
					return EnumValueTypes.ValueType_Int64;
				case "float":
					return EnumValueTypes.ValueType_Float;
				case "bool":
					return EnumValueTypes.ValueType_Bool;
				case "string":
					return EnumValueTypes.ValueType_String;
				default:
					Console.WriteLine(type);
					throw new Exception(type);
			}
		}

	}
}
