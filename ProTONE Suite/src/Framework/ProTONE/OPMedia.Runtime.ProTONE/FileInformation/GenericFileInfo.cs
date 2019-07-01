using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Runtime.FileInformation;
using System.IO;
using OPMedia.Core;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.ShellSupport;

namespace OPMedia.Runtime.ProTONE.FileInformation
{
    public class NativeFileInfoFactory
    {
        public static NativeFileInfo FromPath(string path)
        {
            string fileType = PathUtils.GetExtension(path);
            if (SupportedFileProvider.Instance.AllMediaTypes.Contains(fileType))
            {
                return MediaFileInfo.FromPath(path, true);
            }

            return new NativeFileInfo(path, true);
        }
    }
}
