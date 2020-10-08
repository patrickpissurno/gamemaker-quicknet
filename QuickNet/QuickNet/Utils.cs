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

        public static bool CacheEntryEquals(object valueA, object valueB) //I'll consider that both A and B have the same types, as types can't change in our implementation
        {
            if (valueA is bool)
                return ((bool)valueA) == ((bool)valueB);
            if (valueA is string)
                return ((string)valueA) == ((string)valueB);
            if (valueA is double)
                return ((double)valueA) == ((double)valueB);
            if (valueA is int)
                return ((int)valueA) == ((int)valueB);

            if (valueA is double[])
                return ArrayEquals((double[])valueA, (double[])valueB);
            if (valueA is bool[])
                return ArrayEquals((bool[])valueA, (bool[])valueB);
            if (valueA is string[])
                return ArrayEquals((string[])valueA, (string[])valueB);
            if (valueA is int[])
                return ArrayEquals((int[])valueA, (int[])valueB);

            return false;
        }

        private static bool ArrayEquals(string[] a, string[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        private static bool ArrayEquals(int[] a, int[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        private static bool ArrayEquals(double[] a, double[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        private static bool ArrayEquals(bool[] a, bool[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        public static bool IsIpAddressValid(string ip)
        {
            // Split string by ".", check that array length is 4
            var arr = ip.Split('.');
            if (arr.Length != 4)
                return false;

            //Check each substring checking that parses to byte
            byte b = 0;
            foreach (var oct in arr)
                if (!byte.TryParse(oct, out b))
                    return false;

            return true;
        }

        public static bool IsPortNumberValid(string port)
        {
            return int.TryParse(port, out var p) && p >= 0 && p <= 65535;
        }
    }
}
