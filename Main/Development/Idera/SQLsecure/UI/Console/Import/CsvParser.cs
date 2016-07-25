using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Idera.SQLsecure.UI.Console.Import.Models;

namespace Idera.SQLsecure.UI.Console.Import
{
    public class CsvParser
    {

        private readonly TextReader _reader;
        private IEnumerable<IList<string>> _parsedData;
        private IEnumerable<IList<string>> _data;
        private List<string> _header;


        private const char DelimeterChar = ',';
        private const char QualifierChar = '"'; //For inner objects

        #region ctrors
        public CsvParser(StreamReader reader)
        {
            if (reader == null || reader.EndOfStream) throw new Exception("Import Data empty. Please Specify valid data file.");
            _reader = reader;
        }

        public CsvParser(string dataToParse)
        {
            _reader = new StringReader(dataToParse);
        }
        #endregion




        #region Public Members


        public List<string> GetHeader()
        {

            if (_parsedData == null)
            {
                Initialize();
            }
            return _header;
        }
        public IEnumerable<T> Parse<T>() where T : new()
        {

            var result = new List<T>();


            var data = GetData();

            foreach (IList<string> parsedData in data)
            {
                var idx = 0;
                var obj = new T();
                PropertyInfo[] properties = obj.GetType().GetProperties();


                Array.Sort(properties, new PropertyOrderAttributeComparer());

                if (parsedData.Count != properties.Length) throw new InvalidDataException("Can not parse row. Data not valid!");

                while (idx < parsedData.Count)
                {
                    var dataType = properties[idx].PropertyType;
                    if (IsString(dataType))
                    {
                        properties[idx].SetValue(obj, parsedData[idx], null);
                    }
                    else if (IsInteger(dataType))
                    {
                        var intValue = IntValue(parsedData[idx]);
                        properties[idx].SetValue(obj, intValue, null);
                    }
                    else if (IsBoolean(dataType))
                    {
                        var boolVal = BoolValue(parsedData[idx]);
                        properties[idx].SetValue(obj, boolVal, null);
                    }


                    idx++;
                }
                result.Add(obj);

            }
            return result;
        }



        public static void Clear(StringBuilder value)
        {
            value.Length = 0;
            value.Capacity = 0;
        }
        #endregion


        #region Private Members
        private IEnumerable<IList<string>> GetData()
        {
            if (_parsedData == null)
            {
                Initialize();
            }
            return _data;
        }

        private IEnumerable<T> GetBody<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext()) yield return enumerator.Current;
        }
        private void Initialize()
        {
            _parsedData = Parse(_reader, DelimeterChar, QualifierChar);

            var enumerator = _parsedData.GetEnumerator();
            enumerator.MoveNext();

            _header = enumerator.Current == null ? new List<string>() : new List<string>(enumerator.Current);
            _data = GetBody(enumerator);
        }



        private IEnumerable<IList<string>> Parse(TextReader reader, char delimiter, char qualifier)
        {
            var inQuote = false;
            var record = new List<string>();
            var sb = new StringBuilder();
            while (reader.Peek() != -1)
            {
                var readChar = (char)reader.Read();
                //Process Line Endings (both \n and \r\n)
                if (readChar == '\n' || (readChar == '\r' && (char)reader.Peek() == '\n'))
                {
                    if (readChar == '\r') reader.Read();

                    if (record.Count > 0 || sb.Length > 0)
                    {
                        record.Add(sb.ToString());
                        Clear(sb);
                    }

                    if (record.Count > 0) yield return record;

                    record = new List<string>(record.Count);

                }
                else if (sb.Length == 0 && !inQuote)
                {
                    if (readChar == qualifier) inQuote = true;
                    else if (readChar == delimiter)
                    {
                        record.Add(sb.ToString());
                        Clear(sb);
                    }
                    else if (!char.IsWhiteSpace(readChar))
                        sb.Append(readChar);
                }
                else if (readChar == delimiter)
                {
                    if (inQuote)
                        sb.Append(delimiter);
                    else
                    {
                        record.Add(sb.ToString());
                        Clear(sb);
                    }
                }
                else if (readChar == qualifier)
                {
                    if (inQuote)
                    {
                        inQuote = false;
                    }
                    else
                        sb.Append(readChar);
                }
                else
                    sb.Append(readChar);
            }

            if (record.Count > 0 || sb.Length > 0)
                record.Add(sb.ToString());

            if (record.Count > 0)
                yield return record;
        }

        private static bool IsInteger(Type dataType)
        {
            return dataType.IsEnum || dataType.Name == typeof(int).Name;
        }

        private static bool IsBoolean(Type dataType)
        {
            return dataType.Name == typeof(bool).Name;
        }

        private static bool IsString(Type dataType)
        {
            return dataType.Name == typeof(string).Name;
        }

        private static bool BoolValue(string stringValue)
        {
            bool val;
            if (stringValue.Trim().Equals("1")) val = true; //allow to parse 1=true; 0=false
            else bool.TryParse(stringValue, out val);
            return val;
        }

        private static int IntValue(string stringValue)
        {
            int intValue;
            int.TryParse(stringValue, out intValue);
            return intValue;
        }
        #endregion
    }
}
