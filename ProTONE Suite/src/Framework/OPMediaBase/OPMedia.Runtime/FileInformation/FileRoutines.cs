using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.IO;
using OPMedia.Core;
using System.Security.Permissions;
using System.ComponentModel;

namespace OPMedia.Runtime.FileInformation
{
    public sealed class FileRoutines
    {
        const long FILESIZE_ATOM = 256 * 1024;
        /*
        public static void CopyFile(FileInfo source, FileInfo destination)
        {
            CopyFile(source, destination, Kernel32.CopyFileOptions.None);
        }

        public static void CopyFile(FileInfo source, FileInfo destination,
            Kernel32.CopyFileOptions options)
        {
            CopyFile(source, destination, options, null);
        }
        */
        public static void CopyFile(string source, string destination,
            Kernel32.CopyFileOptions options, Kernel32.CopyFileCallback callback)
        {
            CopyFile(source, destination, options, callback, null);
        }

        public static void CopyFile(string source, string destination,
            Kernel32.CopyFileOptions options, Kernel32.CopyFileCallback callback, object state)
        {
            if (source == null) 
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if ((options & ~Kernel32.CopyFileOptions.All) != 0)
                throw new ArgumentOutOfRangeException("options");

            new FileIOPermission(FileIOPermissionAccess.Read, source).Demand();
            new FileIOPermission(FileIOPermissionAccess.Write, destination).Demand();

            if (source.Length <= FILESIZE_ATOM)
            {
                File.Copy(source, destination, true);
                callback(source, destination, state, source.Length, source.Length);
            }
            else
            {
                Kernel32.CopyProgressRoutine cpr = callback == null ?
                    null : new Kernel32.CopyProgressRoutine(new Kernel32.CopyProgressData(
                        source, destination, callback, state).CallbackHandler);

                bool cancel = false;
                if (!Kernel32.CopyFileEx(source, destination, cpr, IntPtr.Zero, ref cancel, (int)options))
                {
                    int err = Kernel32.GetLastError();
                    if (err != WinError.S_OK)
                        throw new Win32Exception(err);
                }
            }
        }
    }
}
