using System;
using System.IO;
using System.Reflection;

namespace BlipSharp.Utilities
{
    public class Cloneable<T> where T : class
    {
        public static void Clone(T input, ref T output)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (output == null)
                throw new ArgumentNullException("output");

            output = input;
        }

    }
}