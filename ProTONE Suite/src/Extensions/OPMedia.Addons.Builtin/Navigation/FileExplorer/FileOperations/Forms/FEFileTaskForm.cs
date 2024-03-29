﻿using OPMedia.Addons.Builtin.Navigation.FileExplorer.FileOperations.Tasks;
using OPMedia.UI.FileTasks;
using System.Collections.Generic;

namespace OPMedia.Addons.Builtin.FileExplorer.FileOperations.Forms
{
    internal class FEFileTaskForm : FileTaskForm
    {
        public FEFileTaskForm(FileTaskType type, List<string> srcFiles, string srcPath)
            : base(type, srcFiles, srcPath)
        {
        }

        protected override BaseFileTask InitTask(FileTaskType type, List<string> srcFiles, string srcPath)
        {
            switch (type)
            {
                case FileTaskType.Copy:
                    return new FECopyFilesTask(srcFiles, srcPath);

                case FileTaskType.Move:
                    return new FEMoveFilesTask(srcFiles, srcPath);

                case FileTaskType.Delete:
                    return new FEDeleteFilesTask(srcFiles);
            }

            return null;
        }
    }
}
