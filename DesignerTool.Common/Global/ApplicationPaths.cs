using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Global
{
    public static class ApplicationPaths
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
                return Path.Combine(ApplicationPaths.ProgramData, "Log");
            }
        }

        public static void CreateAppDirectories()
        {
            // Log
            if (!Directory.Exists(ApplicationPaths.Log))
            {
                Directory.CreateDirectory(ApplicationPaths.Log);
            }
        }
    }
}
