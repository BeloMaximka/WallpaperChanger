using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

using System.IO;

namespace WallpaperChanger
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);


        string[] images;
        int currentImage = 0;
        int changesCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                images = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.jpg");
                button.Text = "Получайте удовольствие.";
                timer1.Interval = (int)numericUpDown1.Value;
                timer1.Start();
                timer2.Start();
                button2.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SystemParametersInfo(20, 0, images[currentImage], 0x01 | 0x02);
            RegistryKey rkWallPaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            rkWallPaper.SetValue("WallpaperStyle", 2);
            rkWallPaper.SetValue("TileWallpaper", 0);
            rkWallPaper.Close();

            currentImage++;
            if (currentImage == images.Length)
                currentImage = 0;
            changesCount++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button2.Enabled = false;
            button1.Text = "Начать сеанс терапии";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label1.Text = $"Изменений за 10 секунд: {changesCount}";
            changesCount = 0;
        }
    }
}
