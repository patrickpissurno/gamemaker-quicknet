using System;
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
        
        public static string EncodeArray(int[] array)
        {
            if (array.Length == 0)
            {
                return "";
            }
            else if (array.Length == 1)
            {
                return array[0].ToString();
            }
            else if (array.Length < 300)  // string builder is faster on larger arrays, but slower on smaller
            {
                var result = array[0].ToString();

                for (var i = 1; i < array.Length; i++)
                    result += ";" + array[i];

                return result;
            }
            else
            {
                var result = new StringBuilder(array[0].ToString());

                for (var i = 1; i < array.Length; i++)
                {
                    result.Append(";");
                    result.Append(array[i]);
                }

                return result.ToString();
            }
        }

        public static string EncodeArray(double[] array)
        {
            if (array.Length == 0)
            {
                return "";
            }
            else if (array.Length == 1)
            {
                return array[0].ToString();
            }
            else if (array.Length < 300)  // string builder is faster on larger arrays, but slower on smaller
            {
                var result = array[0].ToString();

                for (var i = 1; i < array.Length; i++)
                    result += ";" + array[i];

                return result;
            }
            else
            {
                var result = new StringBuilder(array[0].ToString());

                for (var i = 1; i < array.Length; i++)
                {
                    result.Append(";");
                    result.Append(array[i]);
                }

                return result.ToString();
            }
        }

        public static string EncodeArray(bool[] array)
        {
            if (array.Length == 0)
            {
                return "";
            }
            else if (array.Length == 1)
            {
                return (array[0] ? 1 : 0).ToString();
            }
            else if (array.Length < 300)  // string builder is faster on larger arrays, but slower on smaller
            {
                var result = (array[0] ? 1 : 0).ToString();

                for (var i = 1; i < array.Length; i++)
                    result += ";" + (array[i] ? 1 : 0);

                return result;
            }
            else
            {
                var result = new StringBuilder((array[0] ? 1 : 0).ToString());

                for (var i = 1; i < array.Length; i++)
                {
                    result.Append(";");
                    result.Append(array[i] ? 1 : 0);
                }

                return result.ToString();
            }
        }

        public static string[] DecodeArray(string encoded)
        {
            if (encoded.Length == 0)
                return new string[] { };

            var arr = encoded.Split(';');
            for (var i = 0; i < arr.Length; i++)
                arr[i] = Base64Decode(arr[i]);

            return arr;
        }

        public static double[] DecodeDoubleArray(string encoded)
        {
            if (encoded.Length == 0)
                return new double[] { };

            var arr = encoded.Split(';');
            var result = new double[arr.Length];
            for (var i = 0; i < arr.Length; i++)
                result[i] = double.Parse(arr[i]);

            return result;
        }
        
        public static int[] DecodeIntArray(string encoded)
        {
            if (encoded.Length == 0)
                return new int[] { };

            var arr = encoded.Split(';');
            var result = new int[arr.Length];
            for (var i = 0; i < arr.Length; i++)
                result[i] = int.Parse(arr[i]);

            return result;
        }

        public static bool[] DecodeBoolArray(string encoded)
        {
            if (encoded.Length == 0)
                return new bool[] { };

            var arr = encoded.Split(';');
            var result = new bool[arr.Length];
            for (var i = 0; i < arr.Length; i++)
                result[i] = int.Parse(arr[i]) == 1;

            return result;
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
