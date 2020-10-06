﻿using System;
using System.Text;

namespace QuickNet
{
    internal static class Utils
    {
        public static void Log(object data)
        {
            Console.WriteLine(data.ToString());
        }

        public static string EncodeArray(string[] array)
        {
            if (array.Length == 0)
            {
                return "";
            }
            else if (array.Length == 1)
            {
                return Base64Encode(array[0]);
            }
            else if (array.Length < 300)  // string builder is faster on larger arrays, but slower on smaller
            {
                var result = Base64Encode(array[0]);

                for (var i = 1; i < array.Length; i++)
                    result += ";" + Base64Encode(array[i]);

                return result;
            }
            else
            {
                var result = new StringBuilder(Base64Encode(array[0]));

                for (var i = 1; i < array.Length; i++)
                {
                    result.Append(";");
                    result.Append(Base64Encode(array[i]));
                }

                return result.ToString();
            }
        }

        public static string Base64Encode(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string data)
        {
            var bytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}