using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using OPMedia.ShellSupport.Properties;
using OPMedia.Core;
using System.Diagnostics;
using OPMedia.Runtime.ProTONE;
using OPMedia.Runtime.ProTONE.Utilities;
using System.Threading;
using System.Security.AccessControl;

namespace OPMedia.ShellSupport
{
    //
    // This GUID is defined inside ProTONEConstants.cs in OPMedia.Runtime.ProTONE.
    //
    // If so required change it from there rather than entering a new one here.
    // because the value is used also by the install/uninstall routines.
    // Registration will fail if different GUID's are specified !!
    [Guid(ShellConstants.ShellIntegrationSuportGuid)]
    [ComVisible(true)]
    public class ShellIntegrationSuport : IShellExtInit, IContextMenu
    {
        #region Members
        private List<string> fileList;
        //Bitmap bmp = null;
        IntPtr hBitmap = IntPtr.Zero;
        #endregion

        #region Construction
        public ShellIntegrationSuport()
        {
            try
            {
                hBitmap = Resources.player.GetHbitmap();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                hBitmap = IntPtr.Zero;
            }
        }
        #endregion

        #region Registration / Unregistration
        [ComRegisterFunction()]
        public static void RegisterServer(string s)
        {
            try
            {
                Debug.WriteLine("Attempt to register OPMedia.ShellSupport ...");

                SuiteRegistrationSupport.Init(SupportedFileProvider.Instance);
                SuiteRegistrationSupport.RegisterContextMenuHandler();
                SuiteRegistrationSupport.RegisterKnownFileTypes();
                SuiteRegistrationSupport.ReloadFileAssociations();

                Debug.WriteLine("OPMedia.ShellSupport was succesfully registered !");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [ComUnregisterFunction()]
        public static void UnregisterServer(string s)
        {
            try
            {
                Debug.WriteLine("Attempt to unregister OPMedia.ShellSupport ...");

                SuiteRegistrationSupport.Init(SupportedFileProvider.Instance);
                SuiteRegistrationSupport.UnregisterKnownFileTypes();
                SuiteRegistrationSupport.UnregisterContextMenuHandler();
                SuiteRegistrationSupport.ReloadFileAssociations();

                Debug.WriteLine("OPMedia.ShellSupport was succesfully unregistered !");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region IShellExtInit implementation
        public void Initialize(IntPtr pidlFolder, IntPtr pDataObj, IntPtr hKeyProgID)
        {
            if (pDataObj == IntPtr.Zero)
            {
                throw new ArgumentException();
            }

            //ApplicationInfo.RegisterAppName(GetType().Assembly);
            //Translator.RegisterTranslationAssembly(GetType().Assembly);
            //Translator.SetInterfaceLanguage(AppConfig.LanguageID);

            FORMATETC fe = new FORMATETC();
            fe.cfFormat = 15;// CLIPFORMAT.CF_HDROP;
            fe.ptd = IntPtr.Zero;
            fe.dwAspect = DVASPECT.DVASPECT_CONTENT;
            fe.lindex = -1;
            fe.tymed = TYMED.TYMED_HGLOBAL;
            STGMEDIUM stm = new STGMEDIUM();

            // The pDataObj pointer contains the objects being acted upon. In this 
            // example, we get an HDROP handle for enumerating the selected files 
            // and folders.
            IDataObject dataObject = (IDataObject)Marshal.GetObjectForIUnknown(pDataObj);
            dataObject.GetData(ref fe, out stm);

            try
            {
                // Get an HDROP handle.
                IntPtr hDrop = stm.unionmember;
                if (hDrop == IntPtr.Zero)
                {
                    throw new ArgumentException();
                }

                // Determine how many files are involved in this operation.
                uint nFiles = DllImports.DragQueryFile(hDrop, UInt32.MaxValue, null, 0);

                // Enumerate the selected files and folders.
                if (nFiles > 0)
                {
                    this.fileList = new List<string>();
                    StringBuilder fileName = new StringBuilder(ShellConstants.MAX_FILE_BUFFER);
                    for (uint i = 0; i < Math.Min(nFiles, ShellConstants.MAX_FILES); i++)
                    {
                        // Get the next file name.
                        if (DllImports.DragQueryFile(hDrop, i, fileName, fileName.Capacity) != 0 &&
                            SupportedFileProvider.Instance.IsSupportedMedia(fileName.ToString()))
                        {
                            // Add the file name to the list.
                            fileList.Add(fileName.ToString());
                        }
                    }
                
                    // If we did not find any files we can work with, throw 
                    // exception.
                    if (fileList.Count == 0)
                    {
                        Marshal.ThrowExceptionForHR(ShellConstants.E_FAIL);
                    }
                }
                else
                {
                    Marshal.ThrowExceptionForHR(ShellConstants.E_FAIL);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                fileList.Clear();
            }
            finally
            {
                 DllImports.ReleaseStgMedium(ref stm);
            }
        }
        #endregion

        #region IContextMenu implementation
       
        public int QueryContextMenu(IntPtr hmenu, uint iMenu, uint idCmdFirst, uint idCmdLast, uint uFlags)
        {
            if (fileList != null && fileList.Count > 0)
            {
                uint pos = (uint)DllImports.GetMenuItemCount(hmenu) / 2;

                DllImports.InsertMenu(hmenu, pos, MFMENU.MF_BYPOSITION | MFMENU.MF_SEPARATOR,
                    IntPtr.Zero, string.Empty);

                DllImports.InsertMenu(hmenu, pos + 1, MFMENU.MF_BYPOSITION | MFMENU.MF_STRING,
                    new IntPtr(idCmdFirst + (int)CommandType.PlayFiles),
                    ShellConstants.PlayMenu);

                DllImports.InsertMenu(hmenu, pos + 2, MFMENU.MF_BYPOSITION | MFMENU.MF_STRING,
                    new IntPtr(idCmdFirst + (int)CommandType.EnqueueFiles),
                    ShellConstants.EnqueueMenu);

                DllImports.InsertMenu(hmenu, pos + 3, MFMENU.MF_BYPOSITION | MFMENU.MF_SEPARATOR,
                    IntPtr.Zero, string.Empty);

                if (hBitmap != IntPtr.Zero)
                {
                    DllImports.SetMenuItemBitmaps(hmenu, pos + 1, MFMENU.MF_BYPOSITION, hBitmap, hBitmap);
                    DllImports.SetMenuItemBitmaps(hmenu, pos + 2, MFMENU.MF_BYPOSITION, hBitmap, hBitmap);
                }

                return Math.Max((int)CommandType.PlayFiles, (int)CommandType.EnqueueFiles) + 1;
            }

            return 0;
        }

        public void InvokeCommand(IntPtr pici)
        {
            try
            {
                CMINVOKECOMMANDINFO ici = (CMINVOKECOMMANDINFO)Marshal.PtrToStructure(
                    pici, typeof(CMINVOKECOMMANDINFO));

                int cmd = (ici.verb.ToInt32()) & 0xffff;

                switch(cmd)
                {
                    case (int)CommandType.PlayFiles:
                    case (int)CommandType.EnqueueFiles:
                        PlayerRemoteControl.SendPlayerCommand((CommandType)cmd, fileList.ToArray()); 
                        break;

                    default:
                        Marshal.ThrowExceptionForHR(ShellConstants.E_FAIL);
                        break;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }


        public void GetCommandString(UIntPtr idcmd, uint uflags, IntPtr reserved, StringBuilder commandstring, uint cchMax)
        {
            //commandstring.Clear();
            //commandstring.Append("Launch the files with ProTONE Player");
        }
        #endregion
    }
}
