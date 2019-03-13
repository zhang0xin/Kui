using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Kui.Core 
{
    public static class Constants 
    {
        public static readonly string DllPath = 
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static readonly string SqliteFileName=Path.Combine(DllPath, "db.sqlite");
        static Constants() { }
    }
}