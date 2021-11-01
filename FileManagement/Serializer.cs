using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileManagement
{
    public enum SerializationType
    {
        JSON
    }

    public class Serializer
    {
        public CustomFile File { get; private set; }

        public SerializationType SerializationType { get; private set; }

        public string JsonString { get; set; }

        public Serializer(CustomFile file,SerializationType serializationType = SerializationType.JSON)
        {
            File = file;

            SetSerializationType(serializationType);
        }

        public void Serialize()
        {
            switch (SerializationType)
            {
                case SerializationType.JSON:

                    JsonString = Serialize(File.DataAsString);

                    break;

                default:
                    break;
            }


            //object data = null;

            //try
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        memoryStream.Write(File.Data, 0, File.Data.Length);
            //        memoryStream.Seek(0, SeekOrigin.Begin);

            //        BinaryFormatter bfWrite = new BinaryFormatter();
            //        bfWrite.Serialize(memoryStream, data);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Hata : " + ex.Message);
            //}
        }

        //public static CustomFile ConvertToFile(string fileName,string extension,object value)
        //{
        //    var jsonString = Serialize(value);

        //    var file = new CustomFile(fileName, extension, CustomFile.StringToByteArray(jsonString));

        //    file.SetSerializer();

        //    return file;
        //}

        public object Deserialize()
        {
            JsonString = JsonString ?? File.DataAsString;

            return Deserialize<object>(JsonString);
        }

        public object Deserialize<T>()
        {
            JsonString = JsonString ?? File.DataAsString;

            return Deserialize<T>(JsonString);
        }

        public static string Serialize(object value)
        {
            return JsonSerializer.Serialize(value);
        }

        public static T Deserialize<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }


        public Serializer SetSerializationType(SerializationType serializationType)
        {
            SerializationType = serializationType;

            return this;
        }
    }
}
