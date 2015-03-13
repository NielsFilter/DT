using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DesignerTool.Common.Global
{
    public static class ApplicationPaths
    {
        #region Properties

        public static string InstalledDirectory
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        public static string ProgramData
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DesignerTool");
            }
        }

        public static string MasterDatabaseFilePath
        {
            get
            {
                return Path.Combine(InstalledDirectory, "Databases", "DesignerToolDb.mdf");
            }
        }

        public static string DatabaseFilePath
        {
            get
            {
                return Path.Combine(ProgramData, "DesignerToolDb.mdf");
            }
        }

        public static string Log
        {
            get
            {
                return Path.Combine(ApplicationPaths.ProgramData, "Log");
            }
        }

        #endregion

        /// <summary>
        /// Initializes the Application Paths
        /// </summary>
        public static void Initialize()
        {
            setFolderPermisions();
            createAppDirectories();
            copyDatabaseToProgramData();
        }

        private static void setFolderPermisions()
        {
            Permissions.SetFolderPermission(ApplicationPaths.ProgramData);
        }

        private static void createAppDirectories()
        {
            // Log
            if (!Directory.Exists(ApplicationPaths.Log))
            {
                Directory.CreateDirectory(ApplicationPaths.Log);
            }
        }

        public static void copyDatabaseToProgramData()
        {
            if (!File.Exists(DatabaseFilePath))
            {
                // Copy the master database to the destination path (ProgramData)
                File.Copy(MasterDatabaseFilePath, DatabaseFilePath);
            }
        }
    }
}
