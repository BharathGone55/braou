using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Database
{
    public class DapperConfiguration
    {
        public static void Configure()
        {
            //Mapper<EventMaster>();
        }

        public static void Mapper<T>()
        {
            SqlMapper.SetTypeMap(typeof(T), new CustomPropertyTypeMap(typeof(T),
                (type, columnName) => type.GetProperties().FirstOrDefault(prop => GetDescriptionFromAttribute(prop) == columnName.ToLower())));
        }

        private static string GetDescriptionFromAttribute(MemberInfo member)
        {
            if (member == null) return null;

            var attrib = (DescriptionAttribute)Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute), false);
            return (attrib?.Description ?? member.Name).ToLower();
        }

    }
}
