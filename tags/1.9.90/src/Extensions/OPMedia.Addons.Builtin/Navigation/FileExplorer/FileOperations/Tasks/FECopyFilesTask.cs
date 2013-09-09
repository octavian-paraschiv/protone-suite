﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.UI.FileTasks;
using OPMedia.UI.FileOperations.Tasks;
using System.IO;

namespace OPMedia.Addons.Builtin.Navigation.FileExplorer.FileOperations.Tasks
{
    public class FECopyFilesTask : CopyFilesTask
    {
        public FECopyFilesTask(List<string> srcFiles, string srcFolder)
            : base(srcFiles, srcFolder)
        {
        }

        protected override FileTaskSupport InitSupport()
        {
            return new FEFileTaskSupport(this);
        }

        protected override void CopyLinkedFiles(System.IO.FileInfo fi, string destPath)
        {
            List<string> linkedFiles = _support.GetLinkedFiles(fi);
            if (linkedFiles != null && linkedFiles.Count > 0)
            {
                foreach (string linkedFile in linkedFiles)
                {
                    _support.CheckIfCanContinue(linkedFile);

                    try
                    {
                        string destinationPath = this.GetDestinationPath(linkedFile);

                        FileInfo lfi = new FileInfo(linkedFile);
                        _support.CopyFile(lfi, destinationPath);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
