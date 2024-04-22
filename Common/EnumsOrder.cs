using System.Collections.Generic;
using System;
using System.Linq;

namespace LuxeIQ.Common
{
    
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumOrderAttribute : Attribute
    {
        public readonly int Order;

        public EnumOrderAttribute(int order)
        {
            Order = order;
        }
    }

    public static class EnumsOrder
    {
        public static IEnumerable<T> ToList<T>() where T : IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new System.ArgumentException($"{type.Name} is not an Enum.");

            var values = Enum.GetValues(type).Cast<T>().ToList();

            return values.OrderBy(x => {
                var memInfo = type.GetMember(type.GetEnumName(x));
                var orderAttributes = memInfo[0].GetCustomAttributes(typeof(EnumOrderAttribute), false);
                var order = orderAttributes.Length > 0
                    ? ((EnumOrderAttribute)orderAttributes.First()).Order
                    : Convert.ToInt32(x) + values.Count;

                return order;
            });

        }
    }
}
