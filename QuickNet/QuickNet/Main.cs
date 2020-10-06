using System;
using System.Runtime.InteropServices;

namespace QuickNet
{
    public static class Main
    {
        [DllExport("Square", CallingConvention = CallingConvention.Cdecl)]
        public static double Square(double a)
        {
            return a * a;
        }
    }
}
