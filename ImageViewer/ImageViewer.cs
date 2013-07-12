using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer
{
    public partial class ImageViewer : Form
    {
        public ImageViewer()
        {
            InitializeComponent();
            CenterMessage();
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

        private void ImageViewer_Resize(object sender, EventArgs e)
        {
            CenterMessage();
        }
    }
}
