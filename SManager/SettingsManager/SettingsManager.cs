using System.Reflection;
using System.Xml.Linq;
using static System.IO.File;

namespace SettingsManager
{
    public class SettingsManager<T>
        where T : class, new()
    {
        //private T settingsObject;

        private Dictionary<string, object> values = new Dictionary<string, object>();
        private string path;
        private bool useDefaultValuesInsteadOfThrowingException;
        private IFormatProvider formatProvider;

        private SettingsManager(Dictionary<string, object> values, string path, bool useDefaultValuesInsteadOfThrowingException = false, IFormatProvider formatProvider = null)
        {
            if (formatProvider == null) formatProvider = System.Globalization.CultureInfo.InvariantCulture;

            this.values = values;
            this.path = path;
            this.useDefaultValuesInsteadOfThrowingException = useDefaultValuesInsteadOfThrowingException;
            this.formatProvider = formatProvider;
        }

        public SettingsManager(T settingsObject, string path, bool useDefaultValuesInsteadOfThrowingException = false, IFormatProvider formatProvider = null)
            : this(extractValues(settingsObject), path, useDefaultValuesInsteadOfThrowingException, formatProvider)
        {
            //tu ma nic nie być
        }

        private static bool isPropert(FieldInfo field)
        {
            bool result = !field.IsNotSerialized && !field.IsStatic;
            return result;
        }

        private static bool isPropert(PropertyInfo property)
        {
            bool result = property.CanWrite;
            return result; //TODO: trzeba ustalić warunki serializacji
        }

        //Reflection
        private static Dictionary<string, object> extractValues(T settingsObject)
        {
            if (settingsObject == null) throw new SettingsException("Empty object");

            Dictionary<string, object> values = new Dictionary<string, object>();

            //FieldInfo[] fields = settingsObject.GetType().GetFields();            
            //FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo field in fields)
            {
                if (isPropert(field))
                {
                    object value = field.GetValue(settingsObject); // tu moze byc convert
                    values.Add(field.Name, value);
                }
            }

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (isPropert(property))
                {
                    object value = property.GetValue(settingsObject);
                    values.Add(property.Name, value);
                }
            }

            return values;
        }

        private static string valueToString(object value, IFormatProvider formatProvider)
        {
            if (value.GetType().IsArray)
            {
                string s = "";
                Array array = value as Array;
                //foreach (object element in array) s += element.ToString() + ArrayElementsSeparator;
                foreach (object element in array) s += Convert.ToString(element, formatProvider) + ArrayElementsSeparator;
                s = s.Substring(0, s.Length - 1);
                return s;
            }
            else return Convert.ToString(value, formatProvider);
        }

        public void Save()
        {
            try
            {
                XDocument xml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("utworzono " + DateTime.Now.ToString()),
                    new XElement("Settings",
                        from KeyValuePair<string, object> value in values
                        orderby value.Key
                        select new XElement("Setting",
                            new XAttribute("Name", value.Key),
                            new XAttribute("Type", value.Value.GetType().Name),
                            //value.Value.ToString()
                            //Convert.ToString(value.Value, formatProvider)
                            valueToString(value.Value, formatProvider)
                            )
                    )
                );
                xml.Save(path);
            }
            catch (Exception exc)
            {
                throw new SettingsException("Błąd przy zapisie do pliku XML", exc);
            }
        }

        public static SettingsManager<T> Load(string path, bool useDefaultValuesInsteadOfThrowingException = false, IFormatProvider formatProvider = null)
        {
            if (!Exists(path)) throw new SettingsException("Brak pliku XML z ustawieniami");

            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                foreach (XElement element in XDocument.Load(path).Descendants("Setting"))
                {
                    string name = element.Attribute("Name").Value;
                    string value = element.Value;
                    values.Add(name, value); //TODO: tu chyba powinna być konwersja na typ
                }
                return new SettingsManager<T>(values, path, useDefaultValuesInsteadOfThrowingException, formatProvider);
            }
            catch (Exception exc)
            {
                throw new SettingsException("Błąd przy odczycie pliku XML", exc);
            }
        }

        private static object buildMemberObject(Type memberType, object memberValue, IFormatProvider formatProvider)
        {
            if (memberValue == null) return null;
            string svalue = memberValue.ToString();
            if (memberType != typeof(string) && svalue.Equals(memberType.ToString()))
                throw new SettingsException("Podejrzewam, że wartość ma tylko nazwę typu");

            object value = null;
            if (memberType.IsEnum) value = Enum.Parse(memberType, svalue);
            else if (memberType.IsArray) value = buildMemberArray(memberType, svalue, formatProvider);
            else value = Convert.ChangeType(memberValue, memberType, formatProvider);

            return value;
        }

        public static char ArrayElementsSeparator = ';';

        private static object buildMemberArray(Type memberType, string svalue, IFormatProvider formatProvider)
        {
            string[] svalues = svalue.Split(ArrayElementsSeparator);
            Type elementType = memberType.GetElementType();
            Array instance = Array.CreateInstance(elementType, svalues.Length);
            for (int i = instance.GetLowerBound(0); i <= instance.GetUpperBound(0); ++i)
            {
                object o = Convert.ChangeType(svalues[i - instance.GetLowerBound(0)], elementType, formatProvider);
                instance.SetValue(o, i);
            }
            return instance;
        }


        private static T buildObject(Dictionary<string, object> values, bool useDefaultValuesInsteadOfThrowingException, IFormatProvider formatProvider)
        {
            T settingsObject = new T();

            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo field in fields)
            {
                if (isPropert(field))
                {
                    object value = null;
                    if (values.ContainsKey(field.Name))
                    {
                        value = buildMemberObject(field.FieldType, values[field.Name], formatProvider);
                        field.SetValue(settingsObject, value);
                    }
                }
            }

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (isPropert(property))
                {
                    object value = null;
                    if (values.ContainsKey(property.Name))
                    {
                        value = buildMemberObject(property.PropertyType, values[property.Name], formatProvider);
                        property.SetValue(settingsObject, value);
                    }
                }
            }

            return settingsObject;
        }

        public T GetSettingsObject()
        {
            return buildObject(values, useDefaultValuesInsteadOfThrowingException, formatProvider);
        }
    }


    public class SettingsException : Exception
    {
        public SettingsException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
