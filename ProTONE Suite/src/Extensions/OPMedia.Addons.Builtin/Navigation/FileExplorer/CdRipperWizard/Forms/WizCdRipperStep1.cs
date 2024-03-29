﻿using OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Tasks;
using OPMedia.Core;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.FileInformation;
using OPMedia.Runtime.ProTONE.Rendering.Cdda;
using OPMedia.Runtime.ProTONE.Rendering.Cdda.Freedb;
using OPMedia.UI.Controls;
using OPMedia.UI.Controls.Dialogs;
using OPMedia.UI.Wizards;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WF = System.Windows.Forms;

namespace OPMedia.Addons.Builtin.Navigation.FileExplorer.CdRipperWizard.Forms
{
    public partial class WizCdRipperStep1 : WizardBaseCtl
    {
        ImageList ilSelection = new ImageList();

        WF.TextBox tbEditArtist = new WF.TextBox();
        WF.TextBox tbEditAlbum = new WF.TextBox();
        WF.TextBox tbEditTitle = new WF.TextBox();
        OPMComboBox cmbEditgenre = new OPMComboBox();

        public override Size DesiredSize
        {
            get
            {
                return new Size(800, 600);
            }
        }

        public WizCdRipperStep1()
        {
            InitializeComponent();

            this.SuspendLayout();
            cmbEditgenre.Items.Add(string.Empty); // no genre
            foreach (string gi in ID3FileInfo.AudioGenres)
            {
                cmbEditgenre.Items.Add(gi);
            }

            ResizeColumns();

            ilSelection.ColorDepth = ColorDepth.Depth32Bit;
            ilSelection.Images.Add(OPMedia.UI.Properties.Resources.Error);
            ilSelection.Images.Add(OPMedia.UI.Properties.Resources.OK);

            lvTracks.SmallImageList = ilSelection;
            lvTracks.Resize += new EventHandler(lvTracks_Resize);

            lvTracks.RegisterEditControl(tbEditArtist);
            lvTracks.RegisterEditControl(tbEditAlbum);
            lvTracks.RegisterEditControl(tbEditTitle);
            lvTracks.RegisterEditControl(cmbEditgenre);

            this.ResumeLayout();
        }

        void lvTracks_Resize(object sender, EventArgs e)
        {
            ResizeColumns();
        }

        private void ResizeColumns()
        {
            colTrackNo.Width = 45;
            colSizeBytes.Width = 60;
            colDuration.Width = 55;

            int w = lvTracks.EffectiveWidth - colTrackNo.Width - colDuration.Width - colSizeBytes.Width - 15;

            colAlbum.Width = colArtist.Width = colTitle.Width = colGenre.Width = w / 4;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Wizard.CanMoveNext = false;
                PopulateAudioCDDrives();
            }
            finally
            {
            }
        }

        protected override void OnPageEnter_Initializing()
        {
            Wizard.CanMoveNext = false;
            PopulateAudioCDDrives();

            lvTracks.SubItemEdited += new OPMListView.EditableListViewEventHandler(lvTracks_SubItemEdited);
        }

        void lvTracks_SubItemEdited(object sender, ListViewSubItemEventArgs args)
        {
            Track t = args.Item.Tag as Track;
            if (t != null)
            {
                if (args.SubItemIndex == colAlbum.Index)
                {
                    t.Album = args.SubItem.Text;
                }
                else if (args.SubItemIndex == colArtist.Index)
                {
                    t.Artist = args.SubItem.Text;
                }
                else if (args.SubItemIndex == colTitle.Index)
                {
                    t.Title = args.SubItem.Text;
                }
                else if (args.SubItemIndex == colGenre.Index)
                {
                    t.Genre = args.SubItem.Text;
                }
            }
        }

        private void PopulateAudioCDDrives()
        {
            cmbAudioCDDrives.Items.Clear();
            lvTracks.Items.Clear();

            DriveInfoItem selected = null;

            btnRefresh.Enabled = false;
            pbWaiting.Visible = true;
            Application.DoEvents();

            ThreadPool.QueueUserWorkItem((c) =>
                {
                    try
                    {
                        DriveInfo[] audioCDs = CDDrive.GetAllAudioCDDrives();
                        foreach (DriveInfo di in audioCDs)
                        {
                            DriveInfoItem item = new DriveInfoItem(di);

                            MainThread.Post((d) => { cmbAudioCDDrives.Items.Add(item); });

                            if ((BkgTask as Task).DrivePath == null)
                            {
                                (BkgTask as Task).DrivePath = di.RootDirectory.FullName;
                            }

                            if (Path.Equals(di.RootDirectory.FullName, (BkgTask as Task).DrivePath))
                            {
                                selected = item;
                            }
                        }

                        MainThread.Post((d) =>
                        {
                            cmbAudioCDDrives.SelectedItem = selected;
                        });
                    }
                    finally
                    {
                        MainThread.Post((d) =>
                        {
                            pbWaiting.Visible = false;
                            btnRefresh.Enabled = true;
                        });
                    }
                });
        }

        private void OnDriveSelected(object sender, EventArgs e)
        {
            Wizard.CanMoveNext = false;
            lvTracks.Items.Clear();

            DriveInfoItem item = cmbAudioCDDrives.SelectedItem as DriveInfoItem;
            if (item != null)
            {
                string rootPath = System.IO.Path.GetPathRoot(item.Path);
                if (!string.IsNullOrEmpty(rootPath))
                {
                    (BkgTask as Task).DrivePath = rootPath;

                    CDEntry cdEntry = null;
                    char letter = rootPath.ToUpperInvariant()[0];
                    using (CDDrive cd = new CDDrive())
                    {
                        if (cd.Open(letter) && cd.Refresh())
                        {
                            // Check whether the disc is already added to our persistent storage
                            string discId = cd.GetCDDBDiskID();
                            cdEntry = CDEntry.LoadPersistentDisc(discId);

                            if (cdEntry == null)
                            {
                                switch (ProTONEConfig.AudioCdInfoSource)
                                {
                                    case CddaInfoSource.CdText:
                                        cdEntry = CDAFileInfo.BuildCdEntryByCdText(cd, cd.GetCDDBDiskID());
                                        break;

                                    case CddaInfoSource.Cddb:
                                        cdEntry = CDAFileInfo.BuildCdEntryByCddb(cd, cd.GetCDDBDiskID());
                                        break;

                                    case CddaInfoSource.CdText_Cddb:
                                        {
                                            cdEntry = CDAFileInfo.BuildCdEntryByCdText(cd, cd.GetCDDBDiskID());
                                            CDEntry cde = CDAFileInfo.BuildCdEntryByCddb(cd, cd.GetCDDBDiskID());
                                            cdEntry = CDAFileInfo.Merge(cdEntry, cde);
                                        }
                                        break;

                                    case CddaInfoSource.Cddb_CdText:
                                        {
                                            cdEntry = CDAFileInfo.BuildCdEntryByCddb(cd, cd.GetCDDBDiskID());
                                            CDEntry cde = CDAFileInfo.BuildCdEntryByCdText(cd, cd.GetCDDBDiskID());
                                            cdEntry = CDAFileInfo.Merge(cdEntry, cde);
                                        }
                                        break;

                                    default:
                                        break;
                                }

                                if (cdEntry != null)
                                {
                                    // Cache the disk to persistent storage for retrieving it faster later on
                                    cdEntry.PersistDisc();
                                }
                            }
                        }

                        if (cdEntry != null)
                        {
                            for (int i = 1; i <= cdEntry.NumberOfTracks; i++)
                            {
                                double size = cd.TrackSize(i);
                                int duration = cd.GetSeconds(i);

                                ListViewItem lvItem = new ListViewItem(i.ToString());

                                lvItem.SubItems.Add(TimeSpan.FromSeconds(duration).ToString());
                                lvItem.SubItems.Add(((size / (1024 * 1024)).ToString("F")) + " MB");

                                OPMListViewSubItem subItem = new OPMListViewSubItem(tbEditAlbum, lvItem,
                                    cdEntry.Tracks[i - 1].Album ?? string.Empty);
                                subItem.ReadOnly = false;
                                lvItem.SubItems.Add(subItem);

                                subItem = new OPMListViewSubItem(tbEditArtist, lvItem,
                                    cdEntry.Tracks[i - 1].Artist ?? string.Empty);
                                subItem.ReadOnly = false;
                                lvItem.SubItems.Add(subItem);

                                subItem = new OPMListViewSubItem(tbEditTitle, lvItem,
                                    cdEntry.Tracks[i - 1].Title ?? string.Empty);
                                subItem.ReadOnly = false;
                                lvItem.SubItems.Add(subItem);

                                subItem = new OPMListViewSubItem(cmbEditgenre, lvItem,
                                    cdEntry.Tracks[i - 1].Genre ?? string.Empty);
                                subItem.ReadOnly = false;
                                lvItem.SubItems.Add(subItem);

                                if (Wizard.RepeatCount == 0)
                                {
                                    lvItem.ImageIndex = 1;
                                }
                                else
                                {
                                    //lvItem.ImageIndex = ((BkgTask as Task).Tracks.Contains(i)) ? 1 : 0;
                                }

                                lvItem.Tag = cdEntry.Tracks[i - 1];

                                lvTracks.Items.Add(lvItem);
                            }
                        }
                    }
                }
            }

            CheckNextButton();
        }

        private void lvTracks_DoubleClick(object sender, EventArgs e)
        {
            Point ptDoubleClick = lvTracks.PointToClient(MousePosition);
            ListViewItem lvClickedItem = lvTracks.GetItemAt(ptDoubleClick.X, ptDoubleClick.Y);

            if (lvClickedItem == null)
                return;

            lvClickedItem.ImageIndex = (lvClickedItem.ImageIndex == 0) ? 1 : 0;

            lvTracks.SelectedItems.Clear();
            lvClickedItem.Selected = true;

            CheckNextButton();
        }

        private void CheckNextButton()
        {
            (BkgTask as Task).Tracks.Clear();
            foreach (ListViewItem item in lvTracks.Items)
            {
                if (item.ImageIndex == 1)
                {
                    int idx = -1;
                    if (int.TryParse(item.SubItems[0].Text, out idx))
                    {
                        (BkgTask as Task).Tracks.Add(item.Tag as Track);
                    }
                }
            }

            Wizard.CanMoveNext = ((BkgTask as Task).Tracks.Count > 0);
        }


        private void OnUncheckAll(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem lvi in lvTracks.Items)
                lvi.ImageIndex = 0;

            CheckNextButton();
        }

        private void OnCheckAll(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem lvi in lvTracks.Items)
                lvi.ImageIndex = 1;

            CheckNextButton();
        }


    }
}
