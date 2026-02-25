using System;

namespace BattleSim.Utils
{
    public static class Extensions
    {
        public static T GetValueEnum<T>(this int current) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), current);
        }
    }
}