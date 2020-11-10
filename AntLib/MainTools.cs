using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Media;

namespace AntLib
{
    public class MainTools
    {

        internal string AppName { get; set; }
        /// <summary>
        /// Returns the string between two or more characters
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        internal string GetElement(string SourceString, string Start, string End)
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
        internal string GetStringFromBytes(byte[] BytesToConvert)
        {
            string StringToReturn = Encoding.UTF8.GetString(BytesToConvert);
            return StringToReturn;
        }
        /// <summary>
        /// Turns a bitmap into a byte[]
        /// </summary>
        /// <param name="BytesToConvert"></param>
        /// <returns></returns>
        internal Bitmap GetBitmapFromBytes(byte[] BytesToConvert)
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
        internal byte[] GetBytesFromBitmap(Bitmap BitmapToConvert)
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
        internal Collection<string> GetStringCollectionFromBytes(byte[] BytesToConvert)
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
        internal Collection<Bitmap> GetBitmapCollectionFromBytes(byte[] BytesToConvert)
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
        internal ImageSource GetBitmapImageFromBitmap(Bitmap InputBitmap)
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
        internal byte[] GetBytesFromStringCollection(Collection<string> CollectionToConvert)
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
        internal byte[] GetBytesFromBitmapCollection(Collection<Bitmap> CollectionToConvert)
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
        internal byte[] GetBytesFromString(string DataString)
        {
            byte[] BytesToReturn = Encoding.ASCII.GetBytes(DataString);
            return BytesToReturn;
        }
        /// <summary>
        /// Writes a line of text to the log file located in the base directory of your app, you have to provide an app name first using the "AppName" property
        /// </summary>
        /// <param name="TextToAppend"></param>
        internal void WriteToLog(string TextToAppend)
        {
            if(AppName == null || AppName == "")
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Log.logs", "[" + DateTime.Now + "]" + TextToAppend + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + AppName + ".logs", "[" + DateTime.Now + "]: " + TextToAppend + Environment.NewLine);
            }
        }
    }
}
