using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using Newtonsoft.Json;

namespace CQImage
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        int cropX;
        int cropY;
        int cropWidth;
        int cropHeight;
        public Pen cropPen;
        public DashStyle cropDashStyle = DashStyle.DashDot;
        string PicName;
        string PicPath;
        string JsonName;
        string JsonPath;

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                PictureBox.ImageLocation = OpenFileDialogImage.FileName;
                PicName = OpenFileDialogImage.SafeFileName;
                PicPath = OpenFileDialogImage.FileName;
                JsonName = Path.ChangeExtension(PicName, "json");
                JsonPath = Path.ChangeExtension(PicPath, "json");
                TbFilename.Text = JsonName;
                TbQuestionNum.Enabled = true;
                CbQuestionSec.Enabled = true;
                BtnUpdate.Enabled = true;
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PictureBox.Cursor = Cursors.Cross;
                cropX = e.X;
                cropY = e.Y;
                cropPen = new Pen(Color.Black, 1);
                cropPen.DashStyle = DashStyle.DashDotDot;
            }
            PictureBox.Refresh();
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (PictureBox.Image == null)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PictureBox.Refresh();
                cropWidth = e.X - cropX;
                cropHeight = e.Y - cropY;
                PictureBox.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
                LbLog.Text = "x:  " + cropX + "\n y: " + cropY + "\n w: " + cropWidth + "\n h:" + cropHeight;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TbQuestionNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Left) && (e.KeyChar != (char)Keys.Right))
                e.Handled = true;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TbQuestionNum.Text != "" && CbQuestionSec.SelectedIndex > 0 && cropX > 0 && cropY > 0 && cropWidth > 0 && cropHeight > 0)
            {
                var data = new
                {
                    QuestionNum = int.Parse(TbQuestionNum.Text),
                    QuestionSec = CbQuestionSec.SelectedItem,
                    x = cropX,
                    y = cropY,
                    w = cropWidth,
                    h = cropHeight
                };
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.AppendAllText(JsonPath, json);
                LbLog.Text = "JSON Updated";
            }
        }
    }
}
