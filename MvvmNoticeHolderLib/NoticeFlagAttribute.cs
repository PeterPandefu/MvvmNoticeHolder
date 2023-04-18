using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MvvmNoticeHolderLib
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NoticeFlagAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;
        public Type Type { get; set; }

        public NoticeFlagAttribute(string names, Type type)
        {
            Name = names;
            Type = type;
        }
    }
}
