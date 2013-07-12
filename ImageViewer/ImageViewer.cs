using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer
{
    public partial class ImageViewer : Form
    {

        internal class Img
        {
            public Image Image;
            public Stream Source;
        }
        internal sealed class NaturalStringCompare : IComparer<string>
        {
            [SuppressUnmanagedCodeSecurity]
            internal class SafeNativeMethods
            {
                [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
                public static extern int StrCmpLogicalW(string psz1, string psz2);
            }

            public int Compare(string a, string b)
            {
                return SafeNativeMethods.StrCmpLogicalW(a, b);
            }
        }

        readonly IComparer<string> _sorter = new NaturalStringCompare();

        private List<String> _images;

        private List<String> _siblings;

        private Stream _currentImageStream;

        private Preferences Preferences
        {
            get
            {
                return Properties.Settings.Default.Preferences ?? (Properties.Settings.Default.Preferences = new Preferences());
            }
        }

        public ImageViewer()
        {
            InitializeComponent();
            CenterMessage();
            if (Preferences.CurrentImage == null) return;
            var images = GetImages(Preferences.CurrentImage);
            var siblings = GetSiblings(Preferences.CurrentImage);
            _images = images.Result;
            _siblings = siblings.Result;
            LoadImage(Preferences.CurrentImage);
        }

        private void CenterMessage()
        {
            var width = ClientSize.Width;
            var height = ClientSize.Height;
            if (CurrentStatus.Visible)
            {
                height -= CurrentStatus.Height;
            }
            Message.Top = (height - Message.Height) / 2;
            Message.Left = (width - Message.Width) / 2;
        }

        private void LoadImage(string path)
        {
            if (File.Exists(path))
            {
                var image = GetImage(path);
                var oldstream = _currentImageStream;
                CurrentStatusMessage.Text = "Loading...";
                CurrentImageName.Text = Path.GetFileNameWithoutExtension(path);
                Text = Path.GetFileName(Path.GetDirectoryName((path)));
                var idx = _images.FindIndex(item => item == path) + 1;
                CurrentProgress.Text = string.Format("{0}/{1}", idx, _images.Count);
                var img = image.Result;
                if (CurrentImage.Image != null)
                {
                    CurrentImage.Image.Dispose();
                }
                CurrentImage.Image = img.Image;
                _currentImageStream = img.Source;
                CurrentStatusMessage.Text = "";
                Preferences.CurrentImage = path;
                Properties.Settings.Default.Save();
                if (oldstream != null)
                {
                    oldstream.Dispose();
                }
            }
            else
            {
                Preferences.CurrentImage = path;
                Properties.Settings.Default.Save();
                if (_currentImageStream!=null)
                {
                    _currentImageStream.Dispose();
                    _currentImageStream = null;
                }
                if (CurrentImage.Image != null)
                {
                    CurrentImage.Image.Dispose();
                } 
                CurrentImage.Image = null;
                CurrentImageName.Text = "No Image Loaded.";
                CurrentProgress.Text = "??/??";
                Text = Path.GetFileName(path);
            }
    }

        private void MoveImage(int by)
        {
            var idx = _images.IndexOf(Preferences.CurrentImage);
            var next = idx + by;
            if (next < 0 || idx < 0) next = 0;
            if (next >= _images.Count) next = _images.Count - 1;
            LoadImage(_images[next]);
        }
        private void SetImage(int idx)
        {
            if (idx < 0) idx = 0;
            if (idx >= _images.Count) idx = _images.Count - 1;
            LoadImage(_images[idx]);
        }

        private void NextSibling()
        {
            var path = Preferences.CurrentImage;
            var dir = path;
            if (File.Exists(path))
            {
                dir = Path.GetDirectoryName(path);
            }
            var idx = _siblings.IndexOf(dir) + 1;
            if (idx < _siblings.Count)
            {
                dir = _siblings[idx];
                var images = GetImages(dir);
                var siblings = GetSiblings(dir);
                var img = Preferences.GetDefaultImage(dir);
                _images = images.Result;
                _siblings = siblings.Result;
                if (_images.Count > 0)
                {
                    if (img == null) img = _images[0];
                    LoadImage(img);
                }
                else
                {
                    LoadImage(dir);
                }
            }
        }
        private void PrevSibling()
        {
            var path = Preferences.CurrentImage;
            var dir = path;
            if (File.Exists(path))
            {
                dir = Path.GetDirectoryName(path);
            }
            var idx = _siblings.IndexOf(dir) - 1;
            if (idx >= 0)
            {
                dir = _siblings[idx];
                var images = GetImages(dir);
                var siblings = GetSiblings(dir);
                var img = Preferences.GetDefaultImage(dir);
                _images = images.Result;
                _siblings = siblings.Result;
                if (_images.Count > 0)
                {
                    if (img == null) img = _images[0];
                    LoadImage(img);
                }
                else
                {
                    LoadImage(dir);
                }
            }
        }

        private Task<Img> GetImage(string path)
        {
            return Task.Run(() =>
                {
                    var strm = new MemoryStream();
                    using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        file.CopyTo(strm);
                    }
                    return new Img { Image = Image.FromStream(strm), Source = strm };
                });
        }

        private Task<List<string>> GetSiblings(string path)
        {
            return Task.Run(() =>
            {
                if (path == null) return new List<string>();
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
                path = Path.GetDirectoryName(path);
                if (path == null) return new List<string>();
                return Directory.EnumerateDirectories(path).OrderBy(dir => dir, _sorter).ToList();
            });
        }

        private void OpenImage()
        {
            ChooseImage.InitialDirectory = Path.GetDirectoryName(Preferences.CurrentImage);
            ChooseImage.FileName = Preferences.CurrentImage;
            ChooseImage.Filter = "Images|*.jpg;*.gif;*.png;*.jpeg";
            if (ChooseImage.ShowDialog() == DialogResult.OK)
            {
                var path = ChooseImage.FileName;
                var images = GetImages(path);
                var siblings = GetSiblings(path);
                _images = images.Result;
                _siblings = siblings.Result;
                LoadImage(path); 
                
            }
        }

        private Task<List<String>> GetImages(string path)
        {
            return Task.Run(() =>
            {
                if (path == null) return new List<string>();
                var dir = path;
                if (File.Exists(path))
                {
                    dir = Path.GetDirectoryName(path);
                }
                if (dir == null) return new List<string>();
                var matcher = new Regex(@"[.](jpg|gif|png|jpeg)$");
                return Directory.EnumerateFiles(dir).Where(file=>matcher.IsMatch(file)).OrderBy(file => file, _sorter).ToList();
            });
        }

        private void ImageViewer_Resize(object sender, EventArgs e)
        {
            CenterMessage();
        }

        private void ImageViewer_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    MoveImage(1);
                    break;
                case Keys.Left:
                    MoveImage(-1);
                    break;
                case Keys.Up:
                    MoveImage(5);
                    break;
                case Keys.Down:
                    MoveImage(-5);
                    break;
                case Keys.PageUp:
                    MoveImage(-20);
                    break;
                case Keys.PageDown:
                    MoveImage(20);
                    break;
                case Keys.Home:
                    SetImage(0);
                    break;
                case Keys.End:
                    SetImage(_images.Count);
                    break;
                case Keys.Oemcomma:
                    PrevSibling();
                    break;
                case Keys.OemPeriod:
                    NextSibling();
                    break;
                case Keys.O:
                    OpenImage();
                    break;
            }
        }
    }
}
