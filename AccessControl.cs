using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileWithText
{
    internal class AccessControl
    {
        // Проверка прав доступа к директории
        public static bool HasAccess(string path)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(path);
                var accessControl = directoryInfo.GetAccessControl();
                var rules = accessControl.GetAccessRules(true, true, typeof(SecurityIdentifier));

                var currentUser = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(currentUser);

                foreach (AuthorizationRule rule in rules)
                {
                    var fileSystemRule = rule as FileSystemAccessRule;
                    if (fileSystemRule == null) continue;

                    if (fileSystemRule.AccessControlType == AccessControlType.Allow)
                    {
                        if ((fileSystemRule.FileSystemRights & FileSystemRights.ReadData) == FileSystemRights.ReadData)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Нет прав доступа
                return false;
            }
            return false;
        }
    }
}
