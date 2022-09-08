using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Utility.Extensions
{
    public static class StringExtensions
    {

        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            var place = source.LastIndexOf(find, StringComparison.Ordinal);

            if (place == -1)
                return source;

            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static string RemoveDiacritics(this String s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }


        public static string SerializeXml<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlSerializer.Serialize(writer, value);
                return stringWriter.ToString();
            }
        }

        public static string GetQueryString(this object obj) {
            var properties = from p in obj.GetType().GetProperties()
                where p.GetValue(obj, null) != null
                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return String.Join("&", properties.ToArray());
        }


        public static string GetUniqueFilePath(this string filepath)
        {
            if (!File.Exists(filepath)) return filepath;

            var folder = Path.GetDirectoryName(filepath);
            var filename = Path.GetFileNameWithoutExtension(filepath);
            var extension = Path.GetExtension(filepath);
            var number = 1;

            Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

            if (regex.Success)
            {
                filename = regex.Groups[1].Value;
                number = int.Parse(regex.Groups[2].Value);
            }

            do
            {
                number++;
                filepath = Path.Combine(folder ?? throw new InvalidOperationException(),
                    $"{filename} ({number}){extension}");
            }
            while (File.Exists(filepath));
            return filepath;
        }

        public static int TryConvertToInt(this string value)
        {
            int.TryParse(value, out var result);
            return result;
        }

        public static decimal TryConvertToDecimal(this string value)
        {
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }


        public static decimal TryConvertToDecimalExcludeLetters(this string value)
        {

            var numbers = "";
            var pattern = @"([0-9]{0,3}([0-9]{3})*(.[0-9]+)?)";

            var mc = Regex.Matches(value, pattern);

            foreach (var m in mc)
            {
                numbers = $"{numbers}{m}";
            }

            if (numbers.Trim() == "")
                numbers = "0";

            decimal.TryParse(numbers, out var result);

            return result;
        }

        public static long TryConvertToLong(this string value)
        {
            long.TryParse(value, out var result);
            return result;
        }

        public static bool ContainsIgnoreCase(this string value, string searchText)
        {
            return value.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public static bool EqualIgnoreCase(this string value, string compare)
        {
            if (value == null || compare == null)
            {
                return false;
            }

            return value.Equals(compare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string FisrtCharLower(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string FisrtCharUpper(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }

        public static string SplitInWords(this string value, bool toLower = true)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            var result = r.Replace(value, " ");

            if (!toLower) return result;

            result = result.ToLower();
            result = result.FisrtCharUpper();
            return result;
        }

        public static string GetDescription<T>(this T enumerationValue) where T : struct

        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0) return enumerationValue.ToString();
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0 ? ((DescriptionAttribute) attrs[0]).Description : enumerationValue.ToString();
            //If we have no description attribute, just return the ToString of the enum
        }

        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if(stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if(stream.CanSeek)
                {
                    stream.Position = originalPosition; 
                }
            }
        }

    }
}
