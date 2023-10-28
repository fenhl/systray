using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace Systray
{
    class DiskSpaceProcessIcon : IDisposable
    {
        const long ONE_KB = 1024L;
        const long ONE_MB = 1024L * 1024;
        const long ONE_GB = 1024L * 1024 * 1024;
        const long ONE_TB = 1024L * 1024 * 1024 * 1024;

        NotifyIcon ni;

        public DiskSpaceProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {
            SetIcon();
            ni.MouseClick += (s, e) => {
                if (e.Button == MouseButtons.Left)
                {
                    WinDirStat();
                }
            };
            Update();
            Timer timer = new Timer();
            timer.Interval = 2 * 60 * 1000; // 2 minutes
            timer.Tick += (s, e) => {
                Update();
            };
            timer.Start();

            SystemEvents.UserPreferenceChanged += (s, e) => {
                if (e.Category == UserPreferenceCategory.General)
                {
                    SetIcon();
                }
            };
        }

        public void Dispose()
        {
            ni.Dispose();
        }

        private void WinDirStat()
        {
            Process.Start("C:\\Users\\fenhl\\scoop\\shims\\windirstat.exe");
        }

        private void SetIcon() //FROM modified https://stackoverflow.com/a/59722925/667338
        {
            var light = (int) Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize").GetValue("SystemUsesLightTheme") == 1;
            using var bitmap = new Bitmap(32, 32);
            using var graphics = Graphics.FromImage(bitmap);
            using var font = new Font("Segoe MDL2 Assets", 12, FontStyle.Regular);
            using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString("\ueda2" /* HardDrive */, font, light ? Brushes.Black : Brushes.White, new Rectangle(0, 0, 32, 32), stringFormat);
            ni.Icon = Icon.FromHandle(bitmap.GetHicon());
        }

        private void Update()
        {
            var allDrives = DriveInfo.GetDrives();
            var fullDrives = new List<DriveInfo>();
            foreach (var drive in allDrives)
            {
                if (drive.AvailableFreeSpace < 5 * ONE_GB || drive.AvailableFreeSpace < drive.TotalSize * 0.05)
                {
                    fullDrives.Add(drive);
                }
            }
            if (fullDrives.Count > 0)
            {
                if (fullDrives.Count == 1)
                {
                    ni.Text = $"{fullDrives[0].Name} is almost full";
                }
                else
                {
                    ni.Text = $"{fullDrives.Count} disks are almost full";
                }
                ni.ContextMenuStrip = ContextMenu(fullDrives);
                ni.Visible = true;
            }
            else
            {
                ni.Visible = false;
            }
        }

        private ContextMenuStrip ContextMenu(List<DriveInfo> fullDrives)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            foreach (var drive in fullDrives)
            {
                item = new ToolStripMenuItem($"{drive.Name} {FormatByteSize(drive.AvailableFreeSpace)} ({drive.AvailableFreeSpace * 100 / drive.TotalSize}%) free");
                item.Enabled = false;
                menu.Items.Add(item);
            }
            menu.Items.Add(new ToolStripSeparator());
            item = new ToolStripMenuItem("Start WinDirStat");
            item.Click += (sender, e) => {
                WinDirStat();
            };
            menu.Items.Add(item);
            item = new ToolStripMenuItem("Exit");
            item.Click += (s, e) => {
                Application.Exit();
            };
            menu.Items.Add(item);
            return menu;
        }

        private string FormatByteSize(long bytes)
        {
            if (bytes >= ONE_TB)
            {
                return $"{bytes / ONE_TB} TB";
            }
            else if (bytes >= ONE_GB)
            {
                return $"{bytes / ONE_GB} GB";
            }
            else if (bytes >= ONE_MB)
            {
                return $"{bytes / ONE_MB} MB";
            }
            else if (bytes >= ONE_KB)
            {
                return $"{bytes / ONE_KB} KB";
            }
            else
            {
                return $"{bytes} B";
            }
        }
    }
}
