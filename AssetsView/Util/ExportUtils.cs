using AssetsTools.NET;
using AssetsTools.NET.Extra;
using AssetsView.AssetHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetsView.Util
{
    public class ExportUtils
    {

        private static void DumpField(AssetTypeValueField field, ref StringBuilder stringBuilder, string header)
        {
            for (int i = 0; i < field.childrenCount; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(header);
                sb.Append(field[i].templateField.type);
                sb.Append(" ");
                sb.Append(field[i].templateField.name);
                if (field[i].childrenCount >= 1)
                {
                    header += " ";
                    stringBuilder.AppendLine(sb.ToString());
                    DumpField(field[i], ref stringBuilder, header);
                    header = header.Substring(1, header.Length - 1);
                }
                else
                {
                    sb.Append(" = ");

                    if(field[i].value.type == EnumValueTypes.ValueType_String)
                    {
                        sb.Append('"');
                        sb.Append(field[i].value.AsString());
                        sb.Append('"');
                    }
                    else
                    {
                        sb.Append(field[i].value.AsString());
                    }
                    stringBuilder.AppendLine(sb.ToString());
                }
            }
        }

        private static void WriteDumpFiles(string fileName, string rootDirctoryPath, string content)
        {
            String DirPath = Path.Combine(rootDirctoryPath, "FixedMono");
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);
            }
            UTF8Encoding utf8 = new UTF8Encoding(false);
            File.WriteAllText(Path.Combine(DirPath, fileName), content, utf8);
        }

        public static void ExportAssets(long id)
        {
            string path = IEManager.DirctoryPath;
            string className;
            var targetBaseField = IEManager.GetField(id, out className);
            if (targetBaseField == null) return;

            string content = "";
            var stringBuilder = new StringBuilder();
            DumpField(targetBaseField, ref stringBuilder, "");
            content = stringBuilder.ToString();

            //try
            //{
            //    content = JsonConvert.SerializeObject(targetBaseField);
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine(e.StackTrace);
            //}

            WriteDumpFiles(className + id + ".txt", path, content);
        }

   
    }
}
