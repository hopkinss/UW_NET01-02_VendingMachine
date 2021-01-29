using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine
{
    public class Utility
    {
        public static T ParseEnum<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return default;
            }
        }
    }
}
