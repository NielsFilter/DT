using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Global
{
    public static class PathContext
    {
        public static string ProgramData
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DesignerTool");
            }
        }

        public static string Log
        {
            get
            {
                return Path.Combine(PathContext.ProgramData, "Log");
            }
        }


        public static void CreateAppDirectories()
        {
            // Log
            if (!Directory.Exists(PathContext.Log))
            {
                Directory.CreateDirectory(PathContext.Log);
            }
        }
    }
}
