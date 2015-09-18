using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OPMedia.Core.Utilities;

namespace OPMedia.Core
{
    public class Ole32
    {
        const string OLE32 = "ole32.dll";
        const string OLEPRO32 = "olepro32.dll";

        [ComVisible(false)]
        [Flags]
        public enum CLSCTX : uint
        {
            CLSCTX_INPROC_SERVER = 0x1,
            CLSCTX_INPROC_HANDLER = 0x2,
            CLSCTX_LOCAL_SERVER = 0x4,
            CLSCTX_INPROC_SERVER16 = 0x8,
            CLSCTX_REMOTE_SERVER = 0x10,
            CLSCTX_INPROC_HANDLER16 = 0x20,
            CLSCTX_RESERVED1 = 0x40,
            CLSCTX_RESERVED2 = 0x80,
            CLSCTX_RESERVED3 = 0x100,
            CLSCTX_RESERVED4 = 0x200,
            CLSCTX_NO_CODE_DOWNLOAD = 0x400,
            CLSCTX_RESERVED5 = 0x800,
            CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
            CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
            CLSCTX_NO_FAILURE_LOG = 0x4000,
            CLSCTX_DISABLE_AAA = 0x8000,
            CLSCTX_ENABLE_AAA = 0x10000,
            CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
            CLSCTX_ACTIVATE_32_BIT_SERVER = 0x40000,
            CLSCTX_ACTIVATE_64_BIT_SERVER = 0x80000,
            CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
            CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
            CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
        }

#if HAVE_MONO
        public static void CoUninitialize() { }

        public static void ReleaseStgMedium(ref STGMEDIUM pmedium) { }

        public static int OleCreatePropertyFrame(
            IntPtr hwndOwner,
            int x,
            int y,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
            int cObjects,
            [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] 
			ref object ppUnk,
            int cPages,
            IntPtr lpPageClsID,
            int lcid,
            int dwReserved,
            IntPtr lpvReserved
            )
        {
            return -1;
        }

        public static int CreateBindCtx(int reserved, out IBindCtx ppbc)
        {
            ppbc = null;
            return -1;
        }

        public static int MkParseDisplayName(IBindCtx pbc, string szUserName, ref int pchEaten, out IMoniker ppmk)
        {
            ppmk = null;
            return -1;
        }

        public static int CoCreateInstance(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
            IntPtr pUnkOuter,
            CLSCTX dwClsContext,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IntPtr rReturnedComObject
            )
        {
            rReturnedComObject = IntPtr.Zero;
            return -1;
        }
#else  
        [DllImport(OLE32)]
        public static extern void CoUninitialize();

        [DllImport(OLE32)]
        public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

        /// <summary>
        /// COM function helper for displaying properties dialog
        /// </summary>
        [DllImport(OLEPRO32)]
        public static extern int OleCreatePropertyFrame(
            IntPtr hwndOwner,
            int x,
            int y,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
            int cObjects,
            [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] 
			ref object ppUnk,
            int cPages,
            IntPtr lpPageClsID,
            int lcid,
            int dwReserved,
            IntPtr lpvReserved
            );

        [DllImport(OLE32)]
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport(OLE32, CharSet = CharSet.Unicode)]
        public static extern int MkParseDisplayName(IBindCtx pbc, string szUserName, ref int pchEaten, out IMoniker ppmk);

        [DllImport(OLE32)]
        public static extern int CoCreateInstance(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
            IntPtr pUnkOuter,
            CLSCTX dwClsContext,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IntPtr rReturnedComObject
            );
#endif
    }

    public class Quartz
    {
        const string QUARTZ = "quartz.dll";

#if HAVE_MONO
        public static int AMGetErrorText(int hr, StringBuilder buf, int max)
        {
            return -1;
        }
#else
        [DllImport(QUARTZ, CharSet = CharSet.Auto)]
        public static extern int AMGetErrorText(int hr, StringBuilder buf, int max);
#endif
    }

    public static class PathUtils
    {
        static char[] _dirSeps = new char[] { Path.DirectorySeparatorChar };
        static string _dirSep = new string(_dirSeps);

        static char[] _curDirs = new char[] { '.' };
        static string _curDir = new string(_curDirs);

        static string _parentDir = "..";

        static string _networkPathStart = string.Format("{0}{1}", 
            DirectorySeparator, DirectorySeparator);

        public static string DirectorySeparator
        {
            get
            {
                return _dirSep;
            }
        }

        public static char[] DirectorySeparatorChars
        {
            get
            {
                return _dirSeps;
            }
        }

        public static string CurrentDir
        {
            get
            {
                return _curDir;
            }
        }

        public static char[] CurrentDirChars
        {
            get
            {
                return _curDirs;
            }
        }

        public static string ParentDir
        {
            get
            {
                return _parentDir;
            }
        }

        public static string NetworkPathStart
        {
            get
            {
                return _networkPathStart;
            }
        }

        public static string GetExtension(string path)
        {
            try
            {
                string ext = Path.GetExtension(path);
                return ext.Trim('.').ToLowerInvariant();
            }
            catch 
            {
                return string.Empty;
            }
        }

        public static bool ObjectHasAttribute(string path, FileAttributes fa)
        {
            FileSystemInfo fsi = null;

            if ((fa & FileAttributes.Directory) == FileAttributes.Directory)
                fsi = new DirectoryInfo(path);
            else
                fsi = new FileInfo(path);

            return (fsi != null) && ((fsi.Attributes & fa) == fa);
        }

        public static bool PathHasChildFolder(string path, string childName)
        {
            return Directory.Exists(Path.Combine(path, childName));
        }


        public static bool IsRootPath(string path)
        {
            string strRootSpec = Path.GetPathRoot(path).TrimEnd(new char[]{ Path.DirectorySeparatorChar });
            string strDirSpec = path.TrimEnd(new char[]{ Path.DirectorySeparatorChar });

            // Paths under Windows are case-insensitive
            return (strRootSpec.ToLowerInvariant() == strDirSpec.ToLowerInvariant());
        }

        public static bool PathsAreOnSameRoot(string path1, string path2)
        {
            try
            {
                return (string.Compare(Path.GetPathRoot(path1), Path.GetPathRoot(path2), true) == 0);
            }
            catch{}
            
            return false;
        }

        public delegate void ObjectDeleted(FileSystemInfo fsi);

        public static void DeleteFolderTree(string folder, ObjectDeleted deletedCB = null)
        {
            if (folder != CurrentDir)
            {
                List<string> subdirs = EnumDirectories(folder);
                foreach (string subdir in subdirs)
                {
                    DeleteFolderTree(subdir);
                }

                List<string> files = EnumFiles(folder);
                foreach (string file in files)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        fi.Attributes ^= fi.Attributes;
                        fi.Delete();

                        if (deletedCB != null)
                            deletedCB(fi);
                    }
                    catch
                    {
                    }
                }

                try
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    di.Attributes ^= di.Attributes;
                    di.Delete();

                    if (deletedCB != null)
                        deletedCB(di);
                }
                catch
                {
                }
            }
        }

        public static string LocalPathToNetworkPath(string localPath, string machineName)
        {
            if (!string.IsNullOrEmpty(localPath))
            {
                return string.Format(@"{0}{1}{2}{3}",
                    NetworkPathStart,
                    machineName, 
                    DirectorySeparator,
                    localPath.Replace(":", "$"));
            }

            return string.Empty;
        }

        public static string NetworkPathToLocalPath(string networkPath, ref string machineName)
        {
            machineName = string.Empty;

            if (!string.IsNullOrEmpty(networkPath) && networkPath.StartsWith(NetworkPathStart))
            {
                string path = networkPath.Replace("$", ":").Replace(NetworkPathStart, string.Empty);
                string[] pathParts = path.Split(DirectorySeparatorChars, StringSplitOptions.RemoveEmptyEntries);

                if (pathParts.Length > 1)
                {
                    machineName = pathParts[0];

                    string retVal = string.Empty;

                    for (int i = 1; i < pathParts.Length; i++ )
                    {
                        retVal += pathParts[i];
                        retVal += DirectorySeparator;
                    }

                    return retVal.TrimEnd(DirectorySeparatorChars);
                }
            }

            return string.Empty;
        }

        #region EnumerateDirectories

        public static List<DirectoryInfo> EnumDirectoriesUsingMultiFilter(ManualResetEvent abortEvent, DirectoryInfo sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrEmpty(multiFilter))
                return PathUtils.EnumDirectories(abortEvent, sourceFolder, "*", searchOption);

            if (multiFilter.Contains(";") == false)
                return PathUtils.EnumDirectories(abortEvent, sourceFolder, multiFilter, searchOption);

            List<DirectoryInfo> subfolders = new List<DirectoryInfo>();

            string[] allFilters = multiFilter.Replace(" ;", ";").Replace("; ", ";").Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string filter in allFilters)
            {
                if (abortEvent != null && abortEvent.WaitOne(0))
                    break;

                subfolders.AddRange(PathUtils.EnumDirectories(abortEvent, sourceFolder, filter, searchOption));
            }

            return subfolders;
        }

        public static List<DirectoryInfo> EnumDirectoriesUsingMultiFilter(DirectoryInfo sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return EnumDirectoriesUsingMultiFilter(null, sourceFolder, multiFilter, searchOption);
        }

        public static List<string> EnumDirectoriesUsingMultiFilter(string sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceFolder);
                return (from file in EnumDirectoriesUsingMultiFilter(di, multiFilter, searchOption)
                        select file.FullName).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }


        public static List<string> EnumDirectories(string path,
           string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);

                return (from dir in EnumDirectories(null, di, searchPattern, searchOptions)
                        select dir.FullName).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<DirectoryInfo> EnumDirectories(DirectoryInfo di,
            string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            return EnumDirectories(null, di, searchPattern, searchOptions);
        }

        public static List<DirectoryInfo> EnumDirectories(ManualResetEvent abortEvent, DirectoryInfo di, 
            string searchPattern="*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            List<DirectoryInfo> dirList = new List<DirectoryInfo>();

            try
            {
                if (abortEvent == null || abortEvent.WaitOne(0) == false)
                {
                    Application.DoEvents();
                    InternalEnumDirectories(abortEvent, di, searchPattern, searchOptions, ref dirList);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return dirList;
        }

        private static void InternalEnumDirectories(ManualResetEvent abortEvent, DirectoryInfo di, string searchPattern, SearchOption searchOptions,
            ref List<DirectoryInfo> dirList)
        {
            try
            {
                if (abortEvent != null && abortEvent.WaitOne(0))
                    return;

                Application.DoEvents();

                List<DirectoryInfo> diList = InternalEnumSubFolders(di, searchPattern);
                dirList.AddRange(diList);

                foreach (DirectoryInfo dir in diList)
                {
                    if (abortEvent != null && abortEvent.WaitOne(0))
                        return;

                    Application.DoEvents();

                    try
                    {
                        if (searchOptions == SearchOption.AllDirectories)
                            InternalEnumDirectories(abortEvent, dir, searchPattern, searchOptions, ref dirList);
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message;
                    }

                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private static List<DirectoryInfo> InternalEnumSubFolders(DirectoryInfo di, string searchPattern)
        {
            List<DirectoryInfo> subfolders = new List<DirectoryInfo>();

            try
            {
                IEnumerable<DirectoryInfo> dirList = di.EnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
                if (dirList != null)
                {
                    // This is done to exclude the matches on the 8.3 DOS-like file names.
                    // This app is not 16-bit and it does not target 16-bit OS-es.
                    // So we're not interested in the 8.3 DOS-like file names.
                    // We need to keep only the long file names that match the specified pattern.
                    var x = from dir in dirList
                            where StringUtils.StringMatchesPattern(dir.Name, searchPattern)
                            select dir;

                    if (x != null)
                        subfolders.AddRange(x.ToList());
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return subfolders;
        }
        #endregion

        #region EnumerateFiles

        public static List<FileInfo> EnumFilesUsingMultiFilter(ManualResetEvent abortEvent, DirectoryInfo sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrEmpty(multiFilter))
                return PathUtils.EnumFiles(abortEvent, sourceFolder, "*", searchOption);

            if (multiFilter.Contains(";") == false)
                return PathUtils.EnumFiles(abortEvent, sourceFolder, multiFilter, searchOption);

            List<FileInfo> files = new List<FileInfo>();

            string[] allFilters = multiFilter.Replace(" ;", ";").Replace("; ", ";").Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string filter in allFilters)
            {
                if (abortEvent != null && abortEvent.WaitOne(0))
                    break;

                files.AddRange(PathUtils.EnumFiles(abortEvent, sourceFolder, filter, searchOption));
            }

            return files;
        }

        public static List<FileInfo> EnumFilesUsingMultiFilter(DirectoryInfo sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return EnumFilesUsingMultiFilter(null, sourceFolder, multiFilter, searchOption);
        }

        public static List<string> EnumFilesUsingMultiFilter(string sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceFolder);
                return (from file in EnumFilesUsingMultiFilter(di, multiFilter, searchOption)
                        select file.FullName).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<string> EnumFiles(string path,
           string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);

                return (from file in EnumFiles(null, di, searchPattern, searchOptions)
                        select file.FullName).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<FileInfo> EnumFiles(DirectoryInfo di,
            string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            return EnumFiles(null, di, searchPattern, searchOptions);
        }

        public static List<FileInfo> EnumFiles(ManualResetEvent abortEvent, DirectoryInfo di,
            string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            List<FileInfo> fileList = new List<FileInfo>();

            try
            {
                if (abortEvent == null || abortEvent.WaitOne(0) == false)
                {
                    Application.DoEvents();
                    InternalEnumFiles(abortEvent, di, searchPattern, searchOptions, ref fileList);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return fileList;
        }

        private static void InternalEnumFiles(ManualResetEvent abortEvent, DirectoryInfo di, string searchPattern, SearchOption searchOptions,
            ref List<FileInfo> fileList)
        {
            if (abortEvent != null && abortEvent.WaitOne(0))
                return;

            Application.DoEvents();

            List<FileInfo> filesInThisFolder = InternalEnumFilesInFolder(di, searchPattern);
            fileList.AddRange(filesInThisFolder);

            var diList = di.EnumerateDirectories();
            if (diList != null)
            {
                List<DirectoryInfo> dl = diList.ToList();
                foreach (DirectoryInfo dir in dl)
                {
                    if (abortEvent != null && abortEvent.WaitOne(0))
                        return;

                    Application.DoEvents();

                    try
                    {
                        if (searchOptions == SearchOption.AllDirectories)
                            InternalEnumFiles(abortEvent, dir, searchPattern, searchOptions, ref fileList);
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message;
                    }

                    Application.DoEvents();
                }
            }
        }

        private static List<FileInfo> InternalEnumFilesInFolder(DirectoryInfo di, string searchPattern)
        {
            List<FileInfo> files = new List<FileInfo>();

            try
            {
                IEnumerable<FileInfo> fiList = di.EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
                if (fiList != null)
                {
                    // This is done to exclude the matches on the 8.3 DOS-like file names.
                    // This app is not 16-bit and it does not target 16-bit OS-es.
                    // So we're not interested in the 8.3 DOS-like file names.
                    // We need to keep only the long file names that match the specified pattern.
                    var x = from fi in fiList
                            where StringUtils.StringMatchesPattern(fi.Name, searchPattern)
                            select fi;

                    if (x != null)
                        files.AddRange(x.ToList());
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return files;
        }

        #endregion 

        #region EnumFileSystemEntries
        public static List<FileSystemInfo> EnumFileSystemEntriesUsingMultiFilter(ManualResetEvent abortEvent, DirectoryInfo sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrEmpty(multiFilter))
                return PathUtils.EnumFileSystemEntries(abortEvent, sourceFolder, "*", searchOption);

            if (multiFilter.Contains(";") == false)
                return PathUtils.EnumFileSystemEntries(abortEvent, sourceFolder, multiFilter, searchOption);

            List<FileSystemInfo> fsiList = new List<FileSystemInfo>();

            string[] allFilters = multiFilter.Replace(" ;", ";").Replace("; ", ";").Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string filter in allFilters)
            {
                if (abortEvent != null && abortEvent.WaitOne(0))
                    break;

                fsiList.AddRange(PathUtils.EnumFileSystemEntries(abortEvent, sourceFolder, filter, searchOption));
            }

            return fsiList;
        }

        public static List<FileSystemInfo> EnumFileSystemEntriesUsingMultiFilter(DirectoryInfo sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return EnumFileSystemEntriesUsingMultiFilter(null, sourceFolder, multiFilter, searchOption);
        }

        public static List<string> EnumFileSystemEntriesUsingMultiFilter(string sourceFolder, string multiFilter,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceFolder);
                return (from fsi in EnumFileSystemEntriesUsingMultiFilter(di, multiFilter, searchOption)
                        select fsi.FullName).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<string> EnumFileSystemEntries(string path,
           string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);

                return (from file in EnumFileSystemEntries(null, di, searchPattern, searchOptions)
                        select file.FullName).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<FileSystemInfo> EnumFileSystemEntries(DirectoryInfo di,
            string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            return EnumFileSystemEntries(null, di, searchPattern, searchOptions);
        }

        public static List<FileSystemInfo> EnumFileSystemEntries(ManualResetEvent abortEvent, DirectoryInfo di,
            string searchPattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            List<FileSystemInfo> fsiList = new List<FileSystemInfo>();

            try
            {
                if (abortEvent == null || abortEvent.WaitOne(0) == false)
                {
                    Application.DoEvents();
                    InternalEnumFileSystemEntries(abortEvent, di, searchPattern, searchOptions, ref fsiList);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return fsiList;
        }

        private static void InternalEnumFileSystemEntries(ManualResetEvent abortEvent, DirectoryInfo di, string searchPattern, 
            SearchOption searchOptions, ref List<FileSystemInfo> fsiList)
        {
            List<DirectoryInfo> diList = new List<DirectoryInfo>();
            InternalEnumDirectories(abortEvent, di, searchPattern, searchOptions, ref diList);

            List<FileInfo> fiList = new List<FileInfo>();
            InternalEnumFiles(abortEvent, di, searchPattern, searchOptions, ref fiList);

            fsiList.AddRange(diList);
            fsiList.AddRange(fiList);
        }
        #endregion

        #region IsEmptyFolder

        public static bool IsEmptyFolder(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                return IsEmptyFolder(di);
            }
            catch
            {
                return true;
            }
        }

        public static bool IsEmptyFolder(DirectoryInfo di)
        {
            return IsEmptyFolder(null, di);
        }

        public static bool IsEmptyFolder(ManualResetEvent abortEvent, DirectoryInfo di)
        {
            List<FileSystemInfo> fsiList = new List<FileSystemInfo>();

            try
            {
                if (abortEvent == null || abortEvent.WaitOne(0) == false)
                {
                    Application.DoEvents();
                    InternalEnumFileSystemEntries(abortEvent, di, "*", SearchOption.TopDirectoryOnly, ref fsiList);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return (fsiList.Count < 1);
        }
        #endregion
    }

    public static class WindowHelper
    {
        class find_arg
        {
            public List<IntPtr> _out_array { get; set; }
            public string _in_className { get; set; }
        }

        public static IntPtr FindWindow(string className = null)
        {
            find_arg fa = new find_arg();
            fa._in_className = className;
            fa._out_array = new List<IntPtr>();
            GCHandle gch = GCHandle.Alloc(fa);

            int res = User32.EnumWindows(new User32.EnumWindowProc(EnumWindowCallBack), (IntPtr)gch);

            if (res > 0 && fa._out_array != null && fa._out_array.Count > 0)
            {
                return fa._out_array[0];
            }

            return IntPtr.Zero;
        }

        private static bool EnumWindowCallBack(IntPtr hwnd, IntPtr lParam)
        {
            GCHandle gch = (GCHandle)lParam;
            find_arg fa = gch.Target as find_arg;

            if (fa != null)
            {
                StringBuilder sbc = new StringBuilder(256);
                User32.GetClassName(hwnd, sbc, sbc.Capacity);

                if (sbc.Length > 0)
                {
                    if (sbc.ToString().Contains(fa._in_className))
                        fa._out_array.Add(hwnd);
                }

                return true;
            }

            return false;
        }
    }

   
}
