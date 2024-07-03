using OPMedia.Core;
using OPMedia.Runtime.FileInformation;


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
