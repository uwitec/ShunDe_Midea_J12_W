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
        /// ����ֵд��XML
        /// </summary>
        /// <param name="filePath">string,�ļ�·��</param>
        /// <param name="Node">string,Ҫд��Ľ��</param>
        /// <param name="writeStr">string,Ҫд���ֵ</param>
        /// <returns>bool,д���Ƿ�ɹ�</returns>
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
        /// ����д����,�ٶȱȵ���д��Ҫ��
        /// </summary>
        /// <param name="filePath">string,�ļ�·��</param>
        /// <param name="Nodes">string[],Ҫд��Ľ������</param>
        /// <param name="writeStr">string[],Ҫд���ֵ</param>
        /// <returns>bool,����д���Ƿ�ɹ�</returns>
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
        /// ��ȡ�������
        /// </summary>
        /// <param name="filePath">string,�ļ�·��</param>
        /// <param name="Node">string,Ҫ��ȡ�Ľ��</param>
        /// <param name="defalutStr">string,��ȡʧ�ܵ�Ĭ��ֵ</param>
        /// <returns>string,���ض�ȡ����ֵ</returns>
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
        /// �������ݵ�XML�ļ�
        /// </summary>
        /// <param name="filePath">string,Ҫ������ļ�·��</param>
        /// <param name="type">Type,��������</param>
        /// <param name="ob">object,Ҫ���������</param>
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
        /// ��ȡXML�ļ���������
        /// </summary>
        /// <param name="filePath">string,Ҫ��ȡ���ļ�·��</param>
        /// <param name="type">Type,��������</param>
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
        /// ������ȡ����,�ٶȱȵ�����ȡ��
        /// </summary>
        /// <param name="filePath">string,�ļ�·��</param>
        /// <param name="Nodes">string[],Ҫ��ȡ�Ľ������</param>
        /// <param name="defaultStr">string[],��ȡʧ�ܵ�Ĭ��ֵ</param>
        /// <returns>string[],��ȡ�������</returns>
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
