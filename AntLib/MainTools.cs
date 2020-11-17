using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Media;

namespace AntLib
{
    public class MainTools
    {
        /// <summary>
        /// Set's the app's config directory
        /// </summary>
        public string ConfigDir { get; set; }

        public string DefaultConfig { get; set; }

        public string AppName { get; set; }
        /// <summary>
        /// Returns the string between two or more characters
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public string GetElement(string SourceString, string Start, string End)
        {
            if (SourceString.Contains(Start) && SourceString.Contains(End))
            {
                int StartPos, EndPos;
                StartPos = SourceString.IndexOf(Start, 0) + Start.Length;
                EndPos = SourceString.IndexOf(End, StartPos);
                return SourceString.Substring(StartPos, EndPos - StartPos);
            }
            return "";
        }
        /// <summary>
        /// Turns a byte[] into a string to be read(mainly when received from the network)
        /// </summary>
        /// <param name="BytesToConvert"></param>
        /// <returns></returns>
        public string GetStringFromBytes(byte[] BytesToConvert)
        {
            string StringToReturn = Encoding.UTF8.GetString(BytesToConvert);
            return StringToReturn;
        }
        /// <summary>
        /// Turns a bitmap into a byte[]
        /// </summary>
        /// <param name="BytesToConvert"></param>
        /// <returns></returns>
        public Bitmap GetBitmapFromBytes(byte[] BytesToConvert)
        {
            using (MemoryStream ImageConverterStream = new MemoryStream(BytesToConvert))
            {
                Bitmap BitmapToReturn = (Bitmap)Bitmap.FromStream(ImageConverterStream);
                return BitmapToReturn;
            }
        }
        /// <summary>
        /// Turns a byte[] into a bitmap
        /// </summary>
        /// <param name="BitmapToConvert"></param>
        /// <returns></returns>
        public byte[] GetBytesFromBitmap(Bitmap BitmapToConvert)
        {
            using (MemoryStream ImageConverterStream = new MemoryStream())
            {
                BitmapToConvert.Save(ImageConverterStream, ImageFormat.Png);
                byte[] BytesToReturn = ImageConverterStream.ToArray();
                return BytesToReturn;
            }
        }
        /// <summary>
        /// Turns a collection of strings into a byte[] to be transfered over the network or written to a file
        /// </summary>
        /// <param name="BytesToConvert"></param>
        /// <returns></returns>
        public Collection<string> GetStringCollectionFromBytes(byte[] BytesToConvert)
        {
            BinaryFormatter CollectionFormatter = new BinaryFormatter();
            using (MemoryStream StreamConverter = new MemoryStream(BytesToConvert))
            {
                Collection<string> CollectionToReturn = (Collection<string>)CollectionFormatter.Deserialize(StreamConverter);
                return CollectionToReturn;
            }
        }
        /// <summary>
        /// Turns a collection of bitmaps into a byte[] to be transfered over the network or written to a file
        /// </summary>
        /// <param name="BytesToConvert"></param>
        /// <returns></returns>
        public Collection<Bitmap> GetBitmapCollectionFromBytes(byte[] BytesToConvert)
        {
            BinaryFormatter CollectionFormatter = new BinaryFormatter();
            using (MemoryStream StreamConverter = new MemoryStream(BytesToConvert))
            {
                Collection<Bitmap> CollectionToReturn = (Collection<Bitmap>)CollectionFormatter.Deserialize(StreamConverter);
                return CollectionToReturn;
            }
        }
        /// <summary>
        /// Returns a bitmapimage from a bitmap, to be used with WPF
        /// </summary>
        /// <param name="InputBitmap"></param>
        /// <returns></returns>
        public ImageSource GetBitmapImageFromBitmap(Bitmap InputBitmap)
        {
            MemoryStream BitMapConverterStream = new MemoryStream();
            InputBitmap.Save(BitMapConverterStream, ImageFormat.Png);
            System.Windows.Media.Imaging.BitmapImage ConvertedBitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            ConvertedBitmapImage.BeginInit();
            ConvertedBitmapImage.StreamSource = BitMapConverterStream;
            ConvertedBitmapImage.EndInit();
            return ConvertedBitmapImage;
        }
        /// <summary>
        /// Returns a byte[] from a string collection(Either received from the network or written to a file)
        /// </summary>
        /// <param name="CollectionToConvert"></param>
        /// <returns></returns>
        public byte[] GetBytesFromStringCollection(Collection<string> CollectionToConvert)
        {
            BinaryFormatter CollectionFormatter = new BinaryFormatter();
            using (MemoryStream CollectionStream = new MemoryStream())
            {
                CollectionFormatter.Serialize(CollectionStream, CollectionToConvert);
                byte[] CollectionArray = CollectionStream.ToArray();
                CollectionStream.Dispose();
                return CollectionArray;
            }
        }
        /// <summary>
        /// Returns a byte[] from a bitmap collection(Either received from the network or written to a file)
        /// </summary>
        /// <param name="CollectionToConvert"></param>
        /// <returns></returns>
        public byte[] GetBytesFromBitmapCollection(Collection<Bitmap> CollectionToConvert)
        {
            BinaryFormatter CollectionFormatter = new BinaryFormatter();
            using (MemoryStream CollectionStream = new MemoryStream())
            {
                CollectionFormatter.Serialize(CollectionStream, CollectionToConvert);
                byte[] CollectionArray = CollectionStream.ToArray();
                CollectionStream.Dispose();
                return CollectionArray;
            }
        }
        /// <summary>
        /// Turns a string into a byte[] to be read(mainly when received from the network)
        /// </summary>
        /// <param name="DataString"></param>
        /// <returns></returns>
        public byte[] GetBytesFromString(string DataString)
        {
            byte[] BytesToReturn = Encoding.ASCII.GetBytes(DataString);
            return BytesToReturn;
        }
        /// <summary>
        /// Writes a line of text to the log file located in the base directory of your app, you have to provide an app name first using the "AppName" property
        /// </summary>
        /// <param name="TextToAppend"></param>
        public void WriteToLog(string TextToAppend)
        {
            if(AppName == null || AppName == "")
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Log.logs", "[" + DateTime.Now + "]: " + TextToAppend + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + AppName + ".logs", "[" + DateTime.Now + "]: " + TextToAppend + Environment.NewLine);
            }
        }
        /// <summary>
        /// Reads a value from config, valid entries are "IP" and "Port"
        /// </summary>
        /// <param name="Property"></param>
        /// <param name="DefaultConfig"></param>
        /// <returns></returns>
        public string ReadFromConfig(string Property, string ConfigDir)
        {
            CheckConfig(ConfigDir);
            string Config = File.ReadAllText(ConfigDir);
            if (Property == "IP")
            {
                string StringToReturn = GetElement(Config, "/IP ", "\\");
                return StringToReturn;
            }
            else if (Property == "Port")
            {
                string StringToReturn = GetElement(Config, "/Port ", "\\");
                return StringToReturn;
            }
            else if (Property == "Status")
            {
                string StringToReturn = GetElement(Config, "/Status ", "\\");
                return StringToReturn;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the config file and verifies that it exists, if it doesn't then it creates a default one, if a default config file is created then it returns true
        /// </summary>
        /// <param name="ConfigDir"></param>
        public bool CheckConfig(string ConfigDir)
        {
            if (File.Exists(ConfigDir) == false)
            {
                File.WriteAllText(ConfigDir, DefaultConfig);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the image in the specified directory is of PNG format and returns a boolean
        /// </summary>
        /// <param name="FileDir"></param>
        /// <returns></returns>
        public bool CheckIfImageIsPng(string FileDir)
        {
            byte[] BytesToCheck = File.ReadAllBytes(FileDir);
            if (Encoding.ASCII.GetString(BytesToCheck).Contains("�PNG"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region Console entries

        /// <summary>
        /// Prints an info message on the console
        /// </summary>
        /// <param name="Text"></param>
        public void WriteInfo(string Text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[INFO] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Text);
        }
        /// <summary>
        /// Prints an error message on the console
        /// </summary>
        /// <param name="Text"></param>
        public void WriteError(string Text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Text);
        }
        /// <summary>
        /// Prints a debug message on the console
        /// </summary>
        /// <param name="Text"></param>
        public void WriteDebug(string Text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[DEBUG] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Text);
        }
        /// <summary>
        /// Prints an "OK" message on the console
        /// </summary>
        /// <param name="Text"></param>
        public void WriteOK(string Text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[OK] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Text);
        }
        #endregion
    }
}
