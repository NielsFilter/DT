using DesignerTool.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace DesignerTool.Common.Utils
{
    public class Permissions
    {
        public static void SetFolderPermission(string folderPath, FileSystemRights rights = FileSystemRights.Modify)
        {
            try 
	        {	        
		        if (String.IsNullOrWhiteSpace(folderPath))
                {
                    return;
                }
					
				// Create a security Identifier for the "BuiltinUsers" Group i.e. "LocalComputerName\Users" to be passed to the new access rule
				var sidBuiltInUsers = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

				// Get the folder instance
				var dInfo = new DirectoryInfo(folderPath);

				// Create the directory security object and give the group "Modify" permissions
				var dirSec = new DirectorySecurity();
				dirSec.AddAccessRule(new FileSystemAccessRule(sidBuiltInUsers, FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

				// Apply the permissions to the directory
				dInfo.SetAccessControl(dirSec);
	        }
	        catch (Exception ex)
	        {
                // Suppress exception and log.
                Logger.Log("Could not set Application folder permissions", ex);
	        }
        }
    }
}
