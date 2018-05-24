namespace GD.Extentions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public static class Ex
    {
        public static T DeepCopy<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }

        public static T GetEnum<T>(this string s)
        {
            return (T) Enum.Parse(typeof(T), s);
        }

        public static int Clamp_0_100(this int i)
        {
            if (i > 100)
                return 100;
          
            if (i < 0)
                return 0;

            return i;
        }

        public static int Clamp_0_MAX(this int i)
        {
            return i < 0 ? 0 : i;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void NullableRemove<T>(this List<T> collection, T item)
        {
            if (item != null)
            {
                collection.Remove(item);
            }
        }

        public static bool ContainsType<T>(this List<T> collection, Type type)
        {
            return collection.Any(i => i.GetType() == type);
        }

        public static bool GetChance(this int chance)
        {
            return Random.Range(0, 100) < chance;
        }

        public static float ToPercent(this int percent)
        {
            return (float)percent / 100;
        }

        public static int GetRange(int min, int max)
        {
            return Random.Range(min, max + 1);
        }

        public static List<T> ToListFromXML<T>(string dataPath) where T : class
        {
            List<T> result = new List<T>();

            string asset = File.ReadAllText(Application.streamingAssetsPath + "/" + dataPath);
            XDocument data = XDocument.Parse(asset);

            List<XElement> elements = data.Root.Elements().ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            result.AddRange(elements.Select(t => serializer.Deserialize(t.CreateReader()) as T));

            return result;
        }

        public static float ToFloat(this string s)
        {
            return string.IsNullOrEmpty(s) ? 0 : float.Parse(s);
        }

        public static T GetRandElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static List<string> GetText(this XElement e)
        {
            return e.Element("Texts") != null ? e.Element("Texts").Elements("Text").Select(_ => _.Value).ToList() : null;
        }

        public static int[,] CloneEx(this int[,] array)
        {
            return array.Clone() as int[,];
        }

        public static string F(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string F(this string str, string s1)
        {
            return string.Format(str, s1);
        }

        public static string F(this string str, string s1, string s2)
        {
            return string.Format(str, s1, s2);
        }

        public static string F(this string str, string s1, string s2, string s3)
        {
            return string.Format(str, s1, s2, s3);
        }

        public static string F(this string str, string s1, string s2, string s3, string s4)
        {
            return string.Format(str, s1, s2, s3, s4);
        }

        public static IEnumerable<T> GetEnumValues<T>() where T : struct
        {
            return (T[]) Enum.GetValues(typeof(T));
        }

        public static string[] GetEnumNames<T>() where T : struct
        {
            return Enum.GetNames(typeof(T));
        }

        public static int ToInt(this string s)
        {
            return string.IsNullOrEmpty(s) ? 0 : int.Parse(s);
        }

        public static bool ToBool(this string s)
        {
            return !string.IsNullOrEmpty(s) && bool.Parse(s);
        }
    }
}