﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OPMedia.Core;

namespace OPMedia.UI.FileTasks
{
    public class MoveFilesTask : BaseFileTask
    {
        public MoveFilesTask(List<string> srcFiles, string srcFolder) :
            base(FileTaskType.Move, srcFiles, srcFolder)
        {
        }

        protected override bool MoveObject(string path)
        {
            _support.CheckIfCanContinue(path);

            try
            {
                string destinationPath = this.GetDestinationPath(path);

                try
                {
                    if (Directory.Exists(path))
                    {
                        if (PathUtils.PathsAreOnSameRoot(path, destinationPath))
                        {
                            if (PathUtils.IsEmptyFolder(path))
                            {
                                // empty folder
                                if (_support.SkipConfirmations || _support.CanMove(path))
                                    _support.MoveTo(path, destinationPath);
                            }
                            else if (_support.SkipConfirmations || _support.CanMoveNonEmptyFolder(path))
                                _support.MoveTo(path, destinationPath);
                        }
                        else
                        {
                            List<string> subObjects = PathUtils.EnumFileSystemEntries(path);
                            if (subObjects != null && subObjects.Count > 0)
                            {
                                foreach (string subObj in subObjects)
                                {
                                    MoveObject(subObj);
                                }
                            }

                            if (Directory.CreateDirectory(path) != null)
                                _support.DeleteFolder(path);
                        }
                    }
                    else
                    {
                        FileInfo srcFile = new FileInfo(path);
                        MoveConnectedFiles(srcFile, destinationPath);
                        _support.MoveFile(srcFile, destinationPath, true);
                    }
                }
                catch (Exception ex)
                {
                    AddToErrorMap(path, ex.Message);
                }
            }
            finally
            {
                ProcessedObjects++;
            }

            return true;
        }

        protected virtual void MoveConnectedFiles(FileInfo fi, string destPath) { }
    }
}
