using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Core.Model;

namespace Core.Utils
{
    public abstract class Helper
    {
        public static List<T> Cast<T>(List<Dictionary<string, object>> rows) where T : BaseObject
        {
            var tList = new List<T>();

            var properties = typeof(T).GetProperties();

            foreach (var row in rows)
            {
                var t = CreateInstance<T>();

                var columns = row.Select(r => new {
                    Name = r.Key,
                    Value = r.Value
                });

                foreach (var column in columns)
                {
                    if (properties.Any(p => p.Name == column.Name))
                    {
                        var property = properties.FirstOrDefault(p => p.Name == column.Name);

                        var propertyType = property.PropertyType;

                        var genericBaseTypeName = property.PropertyType.GenericTypeArguments.Any() ? property.PropertyType.GenericTypeArguments?.FirstOrDefault().BaseType.Name : null;

                        if (propertyType.IsGenericType &&
                            genericBaseTypeName == "Enum" &&
                            propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            var intValue = 0;

                            if (int.TryParse(column.Value?.ToString(), out intValue))
                            {
                                property.SetValue(t, Helper.Int32toEnum(intValue, property.PropertyType.GenericTypeArguments.FirstOrDefault()));
                            }
                        }
                        else
                            property.SetValue(t, column.Value);
                    }
                }

                if (columns.Any(c => c.Name.Contains(".")))
                {
                    var navigationProperties = columns.Where(c => c.Name.Contains(".")).Select(c =>
                          new {
                              Table = c.Name.Split('.')[0],
                              Property = c.Name.Split('.')[1],
                              Value = c.Value
                          });

                    var groups = navigationProperties.GroupBy(np => np.Table);

                    foreach (var group in groups)
                    {
                        var propertyList = new List<Dictionary<string, object>>();

                        propertyList.Add(group.ToList().ToDictionary(a => a.Property, a => a.Value));

                        var navigationPropertyType = Helper.GetTypeFromAssembly(typeof(BaseObject), group.Key);

                        var navigationPropertyObject = Helper.InvokeMethod(typeof(Helper), null, "Cast", navigationPropertyType, new object[] { propertyList });

                        var navigationProperty = properties.Where(p => p.PropertyType == navigationPropertyType).FirstOrDefault();

                        IList collection = (IList)navigationPropertyObject;

                        navigationProperty.SetValue(t, collection[0]);
                    }
                }

                tList.Add(t);
            }


            return tList;
        }

        public static T Clone<T>(object sourceObject, params object[] parametersToCreateInstance)
        {
            var cloneObject = CreateInstance<T>(parametersToCreateInstance);

            var targetTypePropertyInfos = typeof(T).GetProperties();

            var sourceObjectTypePropertyInfo = sourceObject.GetType().GetProperties();

            foreach (var propertyInfo in targetTypePropertyInfos.Where(tp => tp.CanWrite))
            {
                if (sourceObjectTypePropertyInfo.Any(i => i.Name == propertyInfo.Name))
                {
                    var valueToClone = propertyInfo.GetValue(sourceObject);

                    if (valueToClone != null)
                    {
                        propertyInfo.SetValue(cloneObject, valueToClone);
                    }
                }
            }

            return cloneObject;
        }
        public static T CreateInstance<T>(params object[] parameters)
        {
            return (T)Activator.CreateInstance(typeof(T), parameters);
        }

        public static T Deserialize<T>(string jsonString, Action<Exception> logAction = null)
        {
            var t = default(T);

            try
            {
                t = JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return t;
        }

        public static IEnumerable<object> EnumInformation(Type sampleTypeInAssembly, string typeName)
        {
            var enumType = GetTypeFromAssembly(sampleTypeInAssembly, typeName);

            var enumValues = enumType.GetEnumValues();

            foreach (var enumValue in enumValues)
            {
                yield return new {
                    Name = enumValue.ToString(),
                    Value = (int)enumValue
                };
            }
        }

        public static IEnumerable<object> EnumInformation(string typeName)
        {
            var sampleTypeInAssembly = typeof(BaseObject);

            return EnumInformation(sampleTypeInAssembly, typeName);
        }

        public static char GenerateUnusedCharacterInAWord(string word)
        {
            var alias = ' ';

            var allEnglishLettersInAWord = "The quick brown fox jumps over the lazy dog".Replace(" ", "");

            do
            {
                var randomIndex = new Random().Next(0, allEnglishLettersInAWord.Length);

                alias = allEnglishLettersInAWord[randomIndex];
            } while (word.Contains(alias));

            return alias;
        }

        public static PropertyInfo? GetPropertyOf(Type type,string propertyName)
        {
            return type.GetProperty(propertyName);
        }

        public static PropertyInfo? GetPropertyOf<T>(string propertyName) where T : class
        {
            return typeof(T).GetProperty(propertyName);
        }

        public static PropertyInfo[] GetPropertiesOf(Type type, Action<Exception> logAction = null)
        {
            try
            {
                return type.GetProperties();
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return null;
        }


        public static PropertyInfo[] GetPropertiesOf<T>(Action<Exception> logAction = null) where T : class
        {
            return GetPropertiesOf(typeof(T), logAction);
        }

        public static Type? GetTypeFromAssembly(Type sampleTypeInAssembly, string typeName,Action<Exception> logAction = null)
        {
            try
            {
                Assembly assembly = sampleTypeInAssembly.Assembly;

                return assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);

                return null;
            }
        }

        public static object? InvokeMethod(Type invokerType,
            object invokerObject,
            string methodName,
            Type genericType = null,
            object?[]? parameters = null,
            Action<Exception> logAction = null)
        {
            try
            {
                var methodInfo = invokerType
                            .GetMethod(methodName);

                if (genericType != null)
                    methodInfo = methodInfo.MakeGenericMethod(genericType);

                return methodInfo.Invoke(invokerObject, parameters);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);

                return null;
            }
        }

        public static object Int32toEnum(int value, Type targetEnumType)
        {
            return Enum.ToObject(targetEnumType, value);
        }

        public static void LoadExpandoObject(ExpandoObject eo, string key, object value)
        {
            var expandoDict = eo as IDictionary<string, object>;

            if (expandoDict.ContainsKey(key))
                expandoDict[key] = value;
            else
                expandoDict.Add(key, value);
        }

        public static string Serialize(object value, Action<Exception> logAction = null)
        {
            string serializedObject = null;

            try
            {
                serializedObject = JsonSerializer.Serialize(value);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return serializedObject;
        }

        public static string SplitThenTakeByIndex(string word, int index, char seperator = '.')
        {
            var splittedValues = word.Split(seperator);

            if (splittedValues.Length == 0)
                return null;

            if (index < 0)
            {
                return splittedValues.TakeLast(Math.Abs(index)).FirstOrDefault();
            }

            return splittedValues[index];
        }

        public static byte[] StringToByteArray(string value, Encoding encoding = null, Action<Exception> logAction = null)
        {
            byte[] array = null;

            try
            {
                encoding ??= Encoding.UTF8;

                array = encoding.GetBytes(value);
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }

            return array;
        }
    }
}
