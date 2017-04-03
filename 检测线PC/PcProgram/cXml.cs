using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
namespace System
{
    public class cXml
    {
        public cXml()
        { }
        public cXml(string _fileName)
        {
            CreateFile(_fileName);
        }
        private static void CreateFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                string DirectoryPath = fileName.Substring(0, fileName.LastIndexOf("\\")) + "\\";
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                XmlTextWriter newTxtWrite = new XmlTextWriter(fileName, Encoding.Unicode);
                newTxtWrite.WriteStartDocument();
                newTxtWrite.WriteStartElement("Setting");
                newTxtWrite.WriteEndElement();
                newTxtWrite.WriteEndDocument();
                newTxtWrite.Close();
            }
        }
        /// <summary>
        /// 将数值写入XML
        /// </summary>
        /// <param name="filePath">string,文件路径</param>
        /// <param name="Node">string,要写入的结点</param>
        /// <param name="writeStr">string,要写入的值</param>
        /// <returns>bool,写入是否成功</returns>
        public static bool WriteNode(string filePath, string Node, string writeStr)
        {
            bool isOk = false;
            try
            {
                XmlDocument newDoc = new XmlDocument();
                if (!File.Exists(filePath))
                {
                    CreateFile(filePath);
                }
                newDoc.Load(filePath);
                XmlNode xn = newDoc.DocumentElement;
                if (xn.ChildNodes.Count > 0)
                {
                    XmlNode firstChild = xn.FirstChild;
                    for (int i = 0; i < xn.ChildNodes.Count; i++)
                    {
                        if (firstChild.Name == Node)
                        {
                            xn.RemoveChild(firstChild);
                            break;
                        }
                        firstChild = firstChild.NextSibling;
                    }
                }
                XmlNode node = newDoc.CreateElement(Node);
                node.InnerText = writeStr;
                xn.AppendChild(node);
                newDoc.Save(filePath);
                isOk = true;
            }
            catch
            {
                isOk = false;
            }
            return isOk;
        }
        /// <summary>
        /// 批量写入结点,速度比单个写入要快
        /// </summary>
        /// <param name="filePath">string,文件路径</param>
        /// <param name="Nodes">string[],要写入的结点数组</param>
        /// <param name="writeStr">string[],要写入的值</param>
        /// <returns>bool,返回写入是否成功</returns>
        public static bool WriteNodes(string filePath, string[] Nodes, string[] writeStr)
        {
            bool isOk = false;
            try
            {
                XmlDocument newDoc = new XmlDocument();
                if (!File.Exists(filePath))
                {
                    CreateFile(filePath);
                }
                newDoc.Load(filePath);
                XmlNode xn = newDoc.DocumentElement;
                for (int j = 0; j < Nodes.Length; j++)
                {
                    if (Nodes[j] != null && Nodes[j] != "")
                    {
                        if (xn.ChildNodes.Count > 0)
                        {
                            foreach (XmlNode firstChild in xn.ChildNodes)
                            {
                                if (firstChild.Name == Nodes[j])
                                {
                                    xn.RemoveChild(firstChild);
                                    break;
                                }
                            }
                        }
                        XmlNode node = newDoc.CreateElement(Nodes[j]);
                        node.InnerText = writeStr[j];
                        xn.AppendChild(node);
                    }
                }
                newDoc.Save(filePath);
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog(exc.ToString());
                isOk = false;
            }
            return isOk;
        }
        /// <summary>
        /// 读取结点数据
        /// </summary>
        /// <param name="filePath">string,文件路径</param>
        /// <param name="Node">string,要读取的结点</param>
        /// <param name="defalutStr">string,读取失败的默认值</param>
        /// <returns>string,返回读取到的值</returns>
        public static string ReadNode(string filePath, string Node, string defaultStr)
        {
            string returnValue = defaultStr;
            try
            {
                XmlDocument root = new XmlDocument();
                if (!File.Exists(filePath))
                {
                    CreateFile(filePath);
                }
                root.Load(filePath);
                XmlNode xn = root.DocumentElement;
                foreach (XmlNode x in xn.ChildNodes)
                {
                    if (x.Name == Node)
                    {
                        returnValue = x.InnerText;
                        break;
                    }
                }
            }
            catch
            { }
            return returnValue;
        }
        /// <summary>
        /// 保存数据到XML文件
        /// </summary>
        /// <param name="filePath">string,要保存的文件路径</param>
        /// <param name="type">Type,数据类型</param>
        /// <param name="ob">object,要保存的数据</param>
        public static bool saveXml(string filePath, Type type, object ob)
        {
            bool isOk = false;
            try
            {
                string DirectoryPath = filePath.Substring(0, filePath.LastIndexOf("\\")) + "\\";
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                XmlSerializer xs = new XmlSerializer(type);
                Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                xs.Serialize(stream, ob);
                stream.Close();
                isOk = true;
            }
            catch { }
            return isOk;
        }
        /// <summary>
        /// 读取XML文件到数据类
        /// </summary>
        /// <param name="filePath">string,要读取的文件路径</param>
        /// <param name="type">Type,数据类型</param>
        /// <returns></returns>
        public static object readXml(string filePath, Type type, object DefaultData)
        {
            object ob = new object();
            try
            {
                if (!File.Exists(filePath))
                {
                    saveXml(filePath, type, DefaultData);
                }
                XmlSerializer xs = new XmlSerializer(type);
                Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                ob = xs.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return ob;
        }
        /// <summary>
        /// 批量读取数据,速度比单个读取快
        /// </summary>
        /// <param name="filePath">string,文件路径</param>
        /// <param name="Nodes">string[],要读取的结点数组</param>
        /// <param name="defaultStr">string[],读取失败的默认值</param>
        /// <returns>string[],读取结点数据</returns>
        public static string[] ReadNodes(string filePath, string[] Nodes, string[] defaultStr)
        {
            string[] DefaultStr = defaultStr;
            if (defaultStr.Length < Nodes.Length)
            {
                DefaultStr = new string[Nodes.Length];
                defaultStr.CopyTo(DefaultStr, 0);
                for (int i = defaultStr.Length; i < Nodes.Length; i++)
                {
                    DefaultStr[i] = "";
                }
            }
            string[] returnValue = new string[Nodes.Length];
            for (int i = 0; i < Nodes.Length; i++)
            {
                returnValue[i] = DefaultStr[i];
            }
            try
            {
                XmlDocument root = new XmlDocument();
                if (!File.Exists(filePath))
                {
                    CreateFile(filePath);
                }
                root.Load(filePath);
                XmlNode xn = root.DocumentElement;
                for (int i = 0; i < Nodes.Length; i++)
                {
                    foreach (XmlNode x in xn.ChildNodes)
                    {
                        if (x.Name == Nodes[i])
                        {
                            returnValue[i] = x.InnerText;
                            break;
                        }
                    }
                }
            }
            catch
            { }
            return returnValue;
        }
    }
}
