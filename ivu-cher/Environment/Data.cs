#region Using directives

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using AvengersUtd.Explore.Data.Resources;
using System.Text;
using AvengersUtd.Explore.Environment;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

#endregion

namespace AvengersUtd.Explore.Data
{
    public static class DataManager
    {
        #region Private fields

        #endregion

        #region Properties

        #endregion

        #region Configuration Keys
        public const string Section_TemplateConfig = "TEMPLATE CONFIG";

        // Template config.
        public const string TemplateConfig_Directory = "Directory";
        public const string TemplateConfig_AskOnStartup = "AskOnStartup";


        public static string FileName = "\\Explore.ini";
        public static bool AskOnStartup = false;
        #endregion

        public static string iniPath = "";
        public static Dictionary<string, string> settingsDictionary;

        static DataManager()
        {
            try
            {
                iniPath = GetIniPath(System.Reflection.Assembly.GetExecutingAssembly().Location);
                DataManager.Resources = LoadResource();

            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void ReCreateResources(bool delete = true)
        {
            if(delete)            
                new FileInfo(getDataFilePath()).Delete();

            DataManager.Resources = LoadResource();
        }

        private static AbstractResource[] LoadResource()
        {
            FileInfo fileData = new FileInfo(getDataFilePath());
            if (!fileData.Exists)
            { CreateNewFileData(); }
            return DataManager.DeserializeCollection<AbstractResource>(getDataFilePath());
        }

        private static void CreateNewFileData()
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.Description = "Selezionare la cartella che contiene i file da aggiungere all'elenco risorse.";
             FileInfo[] files = new FileInfo[0];
            DialogResult dialogres = folderDialog.ShowDialog();
            if (dialogres == DialogResult.OK)
            {
                var path = DataManager.ResourceFolder = folderDialog.SelectedPath;
                DirectoryInfo dir = new DirectoryInfo(path);
                files= dir.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            }
                CreateDataXml(files);
        }

        private static void CreateDataXml(FileInfo[] files)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?><ArrayOfResource xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            int i = 0;
            foreach (var item in files)
            {

                /*
                 <Resource Type="Image" Title="Imm6" Id="06"><Uri>C:/Users/altro/Desktop/IMG_0222.JPG</Uri><TagList /></Resource>
                 */
                ResourceType type = GetTypeFromExtension(item.Extension);
                if (type != ResourceType.Unknown)
                {
                    sb.AppendFormat(@"<Resource Type=""{0}"" Title=""{1}"" Id=""{2}""><Uri>{3}</Uri><TagList /></Resource>",
                                  type.ToString(),
                                  item.Name.Replace(item.Extension, ""),
                                  i,
                                  item.FullName);
                    i++;
                }
            }
            sb.AppendLine("</ArrayOfResource>");
            using (System.IO.StreamWriter sw = new StreamWriter(getDataFilePath()))
            {
                sw.Write(sb.ToString());
                sw.Flush();   
            }
             
        }

        private static ResourceType GetTypeFromExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".wav":
                case ".mp3":
                case ".ogg":
                    return ResourceType.Audio;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".bmp":
                    return ResourceType.Image;
                //case ".model":
                //    return ResourceType.Model;
                case ".rtf":
                case ".txt":
                    return ResourceType.Text;
                case ".avi":
                case ".dvx":
                case ".mpeg":
                    return ResourceType.Video;
                default:
                    return ResourceType.Unknown;
            }
        }

        private static string getDataFilePath()
        {
            return new FileInfo(string.IsNullOrEmpty(ResourceFolder) ? System.Windows.Application.ResourceAssembly.Location : ResourceFolder).DirectoryName + "\\data.xml";
        }

        public static AbstractResource[] Resources
        {
            get;
            set;
        }

        public static String ResourceFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Deserializes an object of type <c>T</c> stored in a Xml file.
        /// </summary>
        /// <typeparam name="T">The type of the serialized object.</typeparam>
        /// <param name="filename">The filename of xml file.</param>
        /// <returns>An object of type <c>T</c>.</returns>
        /// <exception cref="FileNotFoundException">The specified filename cannot be found.</exception>
        public static T Deserialize<T>(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("The specified filename cannot be found.", filename);

            T data;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                data = (T)xmlSerializer.Deserialize(xmlReader);
            }

            return data;
        }

        public static void Serialize<T>(T obj, string filename)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { Indent = true };
            if (Global.Template == TemplateType.UrbanGame)
                xmlWriterSettings.Encoding = System.Text.Encoding.GetEncoding("iso-8859-1");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (XmlWriter xmlWriter = XmlWriter.Create(filename, xmlWriterSettings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("Style", "http://www.avengersutd.com");

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteComment(
                    string.Format(
                        "This is an Explore data file, generated on {0}.\nPlease do not modify it if you don't know what you are doing.\n",
                        DateTime.Now.ToString("f"), Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
                xmlSerializer.Serialize(xmlWriter, obj, ns);
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }

        /// <summary>
        /// Deserializes a generic collection of type <c>T</c> stored in a Xml file.
        /// </summary>
        /// <typeparam name="T">The type of the serialized objects.</typeparam>
        /// <param name="filename">The filename of xml file.</param>
        /// <returns>An array of <c>T</c> objects.</returns>
        /// <exception cref="FileNotFoundException">The specified filename cannot be found.</exception>
        public static T[] DeserializeCollection<T>(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("The specified filename cannot be found.", filename);

            T[] collection;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T[]));

            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                collection = (T[])xmlSerializer.Deserialize(xmlReader);
            }

            return collection;
        }

        public static string LoadText(Uri uri)
        {
            StreamReader streamReader = new StreamReader(uri.LocalPath);

            string text = streamReader.ReadToEnd();

            streamReader.Close();
            return text;
        }


        #region Ini
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Return .ini file path.
        /// </summary>
        /// <param name="path">Application Exe current path.</param>
        /// <returns>Path .ini file.</returns>
        public static string GetIniPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return FileName;
            return new FileInfo(path).DirectoryName + FileName;
        }

        /// <summary>
        /// Create .ini file empty
        /// </summary>
        /// <param name="path">Path del file di configurazione.</param>
        public static void CreateEmptyFile(string path)
        {
            // Template config.
            WritePrivateProfileString(Section_TemplateConfig, TemplateConfig_Directory, string.Empty, path);
        }

        /// <summary>
        /// Create dictionary for store .ini values
        /// </summary>
        /// <returns>Dictionary with .ini file values.</returns>
        public static Dictionary<string, string> CreateEmptySettingsDictionary()
        {
            Dictionary<string, string> dicToReturn = new Dictionary<string, string>();

            // Template config. 
            dicToReturn.Add(TemplateConfig_Directory, System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
            dicToReturn.Add(TemplateConfig_AskOnStartup, true.ToString());

            return dicToReturn;
        }

        /// <summary>
        /// Read from .ini file.
        /// </summary>
        /// <param name="path">Path .ini file.</param>
        /// <returns>Dictionary with file .ini values.</returns>
        public static Dictionary<string, string> ReadFromIni(string path)
        {
            Dictionary<string, string> dicToReturn = new Dictionary<string, string>();
            StringBuilder value;

            if (File.Exists(path))
            {
                // Template config.
                value = new StringBuilder(255);
                GetPrivateProfileString(Section_TemplateConfig, TemplateConfig_Directory, string.Empty, value, 255, path);
                dicToReturn.Add(TemplateConfig_Directory, value.ToString());
                value = new StringBuilder(255);
                GetPrivateProfileString(Section_TemplateConfig, TemplateConfig_AskOnStartup, string.Empty, value, 255, path);
                dicToReturn.Add(TemplateConfig_AskOnStartup, value.ToString());
            }
            return dicToReturn;
        }

        /// <summary>
        /// Write .ini file from dictionary content.
        /// </summary>
        /// <param name="path">Path .ini file.</param>
        /// <param name="dictionary">Dictionary content for .ini file.</param>
        public static void WriteToIni(string path, Dictionary<string, string> dictionary)
        {
            // Template config.
            WritePrivateProfileString(Section_TemplateConfig, TemplateConfig_Directory, dictionary[TemplateConfig_Directory], path);
            WritePrivateProfileString(Section_TemplateConfig, TemplateConfig_AskOnStartup, dictionary[TemplateConfig_AskOnStartup], path);

        }
        



        internal static void LoadFromIniFile()
        {
            settingsDictionary = ReadFromIni(iniPath);

            ResourceFolder = settingsDictionary[TemplateConfig_Directory];
            ReCreateResources(false);

            AskOnStartup = Convert.ToBoolean(settingsDictionary[TemplateConfig_AskOnStartup]);
        }

        internal static void CreateDefaultIni()
        {
            CreateEmptyFile(iniPath);
            settingsDictionary = CreateEmptySettingsDictionary();

            // Template config.
            //settingsDictionary[TemplateConfig_Directory] = "";
            //settingsDictionary[TemplateConfig_AskOnStartup] = "true";
            WriteToIni(iniPath, settingsDictionary);
        }
        #endregion
    }
}