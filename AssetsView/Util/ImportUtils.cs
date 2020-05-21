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

        public static void ImportAssets(long path_id, string filedFileName, string targetFileName){
			AssetTypeValueField baseField;

			//         try
			//{
			//	baseField = JsonConvert.DeserializeObject<AssetTypeValueField>(content);
			//}
			//catch (Exception)
			//{
			//	throw;
			//}
			string className;
			int lines = 0;
			UTF8Encoding utf8 = new UTF8Encoding(false);
			string[] contents = File.ReadAllLines(filedFileName, utf8);

			baseField = IEManager.GetField(path_id, out className);
			ChangeField(baseField, contents, ref lines);

			var fileName = targetFileName;
			var inst = IEManager.AssetsFileInstance;
			var inf = inst.table.GetAssetInfo(path_id);
			var newGoBytes = baseField.WriteToByteArray();
			ushort monoId = inst.file.typeTree.unity5Types[inf.curFileTypeOrIndex].scriptIndex;

			//AssetsReplacerFromMemory's monoScriptIndex should always be 0xFFFF unless it's a MonoBehaviour
			var repl = new AssetsReplacerFromMemory(0, path_id, (int)inf.curFileType, monoId, newGoBytes);
			var writer = new AssetsFileWriter(File.OpenWrite(fileName));
			inst.file.Write(writer, 0, new AssetsReplacer[] { repl }.ToList(), 0);
			writer.Close();
		}

		public static void ChangeField(AssetTypeValueField field, string[] contents, ref int lines)
		{
            for (int i = 0; i < field.childrenCount; i++)
            {
                string str = contents[lines];

                string[] fieldStr = str.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);

                field[i].templateField.type = fieldStr[0];
                field[i].templateField.name = fieldStr[1];
				
				//字符串的特殊处理
				//TODO 多行拼接有bug需要修复
				if (fieldStr[0] == "string")
				{
					string replace = fieldStr[0] + " " + fieldStr[1] + " = ";
					string value = str.Replace(replace, "").Trim();

					value = value.Substring(1, value.Length - 1);
					//不是单行字符串需要追踪全部行数
					if (str.Last() != '"')
					{
						lines++;
						//还可能存在字符串内自带"的情况,先不考虑,不出问题就不处理,以"结尾不出意外应该是没问题的
						while (contents[lines] == string.Empty || contents[lines].Last() != ('"'))
						{
							lines++;
							value += "\n" + contents[lines];
						}
					}
					value = value.Substring(0, value.Length - 1);
					fieldStr[3] = value;
					//fieldStr[3] = IEManager.String2Unicode(value);	//原本打算用Unicode强行怼过去,但是没有生效
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
					field[i].value.Set(fieldStr[3]);
				}
            }
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
