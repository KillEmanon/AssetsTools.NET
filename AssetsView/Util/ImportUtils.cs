using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using AssetsView.AssetHelpers;
using Newtonsoft.Json;

namespace AssetsView.Util
{
    class ImportUtils
    {
		static string logPath;

		/// <summary>
		/// 导入一整个目录的资源文件
		/// </summary>
		/// <param name="directoryPath"></param>
		/// <param name="targetFileName"></param>
		public static void ImportAssets(string directoryPath, string targetFileName)
		{
			if (Directory.Exists(directoryPath))
			{
				var files = Directory.GetFiles(directoryPath);
				var replList = new List<AssetsReplacer>();

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
					string name;

					if (splitArr.Length == 2)
						name = splitArr[1];
					else
						name = splitArr[3];

					var temp = GenReplacerFromMemory(long.Parse(name), files[i], splitArr);
					//var temp = GenReplacerFromMemoryByJson(long.Parse(name), files[i]);

					if (temp != null) replList.Add(temp);
				}

				var inst = IEManager.AssetsFileInstance;
				var writer = new AssetsFileWriter(File.OpenWrite(targetFileName));
				//Bundle和普通的分离操作
				if (IEManager.IsBundle)
				{
					AssetBundleFile abFile = inst.parentBundle.file;
					var index = FindAssetsIDInBundle(inst, abFile);
					BundleReplacerFromAssets brfa = new BundleReplacerFromAssets(Path.GetFileName(inst.path), null, inst.file, replList, index);
					List<BundleReplacer> bRepList = new List<BundleReplacer>();
					bRepList.Add(brfa);
					abFile.Write(writer, bRepList);
				}
				else
				{
					inst.file.Write(writer, 0, replList, 0);
				}
				
				writer.Close();
			}
		}

		public static uint FindAssetsIDInBundle(AssetsFileInstance inst, AssetBundleFile abFile)
		{
			for (uint i = 0; i < abFile.bundleInf6.dirInf.Length; i++)
			{
				if (inst.name == abFile.bundleInf6.dirInf[i].name)
				{
					return i;
				}
			}

			throw new Exception("找不到AssetsFile在AB包中的index");
		}

		//TODO 暂时用于功能测试
		public static void ImportAssets(long path_id, string filedFileName, string targetFileName)
		{
			//TextureFile.GetTextureDataFromBytes

			//var inst = IEManager.AssetsFileInstance;
			//var repl = GenReplacerFromMemory(path_id, filedFileName);
			//var writer = new AssetsFileWriter(File.OpenWrite(targetFileName));
			//inst.file.Write(writer, 0, new AssetsReplacer[] { repl }.ToList(), 0);
			//writer.Close();
		}

		public static AssetsReplacerFromMemory GenReplacerFromMemoryByJson(long path_id, string filedFileName)
		{
			UTF8Encoding utf8 = new UTF8Encoding(false);
			string content = File.ReadAllText(filedFileName, utf8);

			AssetTypeValueField baseField = IEManager.DeserializeObject(content);

			var inst = IEManager.AssetsFileInstance;
			var inf = inst.table.GetAssetInfo(path_id);
			var newGoBytes = baseField.WriteToByteArray();
			ushort monoId = inst.file.typeTree.unity5Types[inf.curFileTypeOrIndex].scriptIndex;

			return new AssetsReplacerFromMemory(0, path_id, (int)inf.curFileType, monoId, newGoBytes);
		}

		public static AssetsReplacerFromMemory GenReplacerFromMemory(long path_id, string filedFileName, string[] splitArr)
		{
			AssetTypeValueField baseField;

			
			UTF8Encoding utf8 = new UTF8Encoding(false);
			string[] contents = File.ReadAllLines(filedFileName, utf8);
			//TODO 过长的数据库类现在先不处理
			if (contents.Length > 10000)
			{
				Console.WriteLine($"文件过大,暂不处理--{path_id}--{filedFileName}");
				File.AppendAllLines(logPath, new string[] { $"文件过大,暂不处理--{path_id}--{filedFileName}" });
				return null;
			}

			int lines = 0;
			baseField = IEManager.GetField(path_id);

			if(baseField == null)
			{
				Console.WriteLine($"处理失败,在导入中找不到对应id--{path_id}--{filedFileName}");
				File.AppendAllLines(logPath, new string[] { $"处理失败,在导入中找不到对应id--{path_id}--{filedFileName}" });
				return null;
			}

			var baseField_new = baseField;
			//试试无中生有
			if (splitArr[0] == "MonoBehaviour")
			{
				lines = 0;
				int count = CalculationChildrenCount(contents, -1, -1);
				baseField_new = IEManager.InitMonoAssetTypeValueField(count);
				CreateField(baseField_new, contents, ref lines, count);
			}
			else
			{
				var sucess = ChangeField(baseField_new, contents, ref lines);
				if (!sucess)
				{
					Console.WriteLine($"处理失败,有null值--{path_id}--{filedFileName}");
					File.AppendAllLines(logPath, new string[] { $"处理失败,有null值--{path_id}--{filedFileName}" });
					return null;
				}
			}

			string content1 = IEManager.SerializeObject(baseField);
			string content2 = IEManager.SerializeObject(baseField_new);

			File.WriteAllText("mono/origin_" + path_id + ".json", content1);
			File.WriteAllText("mono/datachange_" + path_id + ".json", content2);

			var inst = IEManager.AssetsFileInstance;
			var inf = inst.table.GetAssetInfo(path_id);
			var newGoBytes = baseField_new.WriteToByteArray();
			ushort monoId = inst.file.typeTree.unity5Types[inf.curFileTypeOrIndex].scriptIndex;

			return new AssetsReplacerFromMemory(0, path_id, (int)inf.curFileType, monoId, newGoBytes);
		}

		public static bool ChangeField(AssetTypeValueField field, string[] contents, ref int lines)
		{
            for (int i = 0; i < field.childrenCount; i++)
            {
                string str = contents[lines];

                string[] fieldStr = str.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);

				//数据预处理
				fieldStr = ValueHandler(fieldStr, str);

				field[i].templateField.type = fieldStr[0];
				field[i].templateField.name = fieldStr[1];

				if(fieldStr.Length >= 5)
					field[i].templateField.align = fieldStr[4] == "true";

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
                    {
                        continue;
                        //return false;
                    }
                }
            }
			return true;
        }

		/// <summary>
		/// 无中生有一个Field(解析yaml生成一个新的Field)
		/// </summary>
		/// <param name="field"></param>
		/// <param name="contents"></param>
		/// <param name="lines"></param>
		public static void CreateField(AssetTypeValueField field, string[] contents, ref int lines, int count)
		{
			for (int i = 0; i < count; i++)
			{
				//子项
				int subCount = CalculationChildrenCount(contents, lines);

				string str = contents[lines];

				string[] fieldStr = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				//预处理字符串内容
				fieldStr = ValueHandler(fieldStr, str);

				//是否是数组
				bool isArray = fieldStr[0] == "Array";
				bool hasSub = subCount > 0;
				bool hasValue = subCount == 0 && !isArray;

				//格式类
				AssetTypeTemplateField tempField = new AssetTypeTemplateField();
				tempField.type = fieldStr[0];
				tempField.name = fieldStr[1];
				tempField.isArray = isArray;
				tempField.valueType = AssetTypeValueField.GetValueTypeByTypeName(fieldStr[0]);
				tempField.hasValue = hasValue;

				if (hasSub)
				{
					tempField.align = fieldStr.Length >= 3 ? fieldStr[2] == "True" : false;
				}
				else
				{
					tempField.align = fieldStr.Length >= 5 ? fieldStr[4] == "True" : false;
				}

				//空数组的特殊处理
				if (isArray && !hasSub)
				{
					tempField.children = new AssetTypeTemplateField[2];
					tempField.childrenCount = 2;
					tempField.children[0] = new AssetTypeTemplateField();
					tempField.children[0].hasValue = true;
					tempField.children[0].name = "size";
					tempField.children[0].type = "int";
					tempField.children[0].valueType = EnumValueTypes.ValueType_Int32;
				}
				else
				{
					tempField.children = new AssetTypeTemplateField[subCount];
					tempField.childrenCount = subCount;
				}

				//承载类
				AssetTypeValueField newField = new AssetTypeValueField();
				newField.templateField = tempField;
				newField.childrenCount = subCount;
				newField.children = new AssetTypeValueField[subCount];

				if (isArray)
				{
					newField.value = new AssetTypeValue(EnumValueTypes.ValueType_Int32, subCount);
				}
				else if (!hasSub)
				{
					var value = fieldStr.Length >= 4 ? fieldStr[3] : null;
					newField.value = new AssetTypeValue(tempField.valueType, value);
				}
				else
				{
					newField.value = null;
				}

				lines++;

				//套娃操作
				if (hasSub)
				{
					CreateField(newField, contents, ref lines, subCount);
				}

				//补充实例信息给父节点
				field.children[i] = newField;

				//补充模板信息给父节点
				field.templateField.children[i] = tempField;

				////数组的模板类有特殊处理
				//if (field.templateField.isArray && i == 0)
				//	field.templateField.children[1] = tempField;
				//else
				//	field.templateField.children[i] = tempField;
			}
		}

		/// <summary>
		///	根据数据类型进行的预先处理,跟导出规则有关
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string[] ValueHandler(string[] fieldStr, string lineStr)
		{
			//某个搞事的类型,简直了
			if (fieldStr[0] == "unsigned")
			{
				fieldStr[0] = $"{fieldStr[0]} {fieldStr[1]}";
				fieldStr[1] = fieldStr[2];
				fieldStr[3] = fieldStr[4];
				if (fieldStr.Length > 4)
					fieldStr[4] = fieldStr[5];
			}

			//字符串的特殊处理,还原
			if (fieldStr[0] == "string")
			{
				string replace = fieldStr[0] + " " + fieldStr[1] + " = ";

				//去除额外的Align参数
				if (lineStr.EndsWith("True"))
				{
					lineStr = lineStr.Substring(0, lineStr.Length - 5);
				}
				else
				{
					lineStr = lineStr.Substring(0, lineStr.Length - 6);
				}

				fieldStr[3] = lineStr.Replace(replace, "").Trim().Replace("\"", "").Replace("\\n", "\n");
			}
			//bool的特殊处理
			if (fieldStr[0] == "bool")
			{
				if (fieldStr[3] == "true")
					fieldStr[3] = "1";
				else
					fieldStr[3] = "0";
			}

			return fieldStr;
		}

		public static int CalculationChildrenCount(string[] content, int start)
		{
			int origin_num = CalculationSpace(content[start]);
			return CalculationChildrenCount(content, start, origin_num);
		}

		public static int CalculationChildrenCount(string[] content, int start, int origin_num)
		{
			int count = 0;
			int p = start + 1;
			while (p < content.Length)
			{
				int num = CalculationSpace(content[p]);
				if(num == origin_num + 1)
				{
					count++;
				}
				else if(num < origin_num + 1)
				{
					break;
				}
				p++;
			}
			return count;
		}

		/// <summary>
		/// 计算字符串前置空格数量
		/// </summary>
		/// <returns></returns>
		public static int CalculationSpace(string str)
		{
			int i = 0;
			while(i < str.Length && char.IsWhiteSpace(str[i]))
			{
				i++;
			}
			return i;
		}

	}
}
