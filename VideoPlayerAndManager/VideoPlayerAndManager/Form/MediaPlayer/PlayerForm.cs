﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
//using System.Runtime.InteropServices;

namespace MoviePlayer
{
    public partial class PlayerForm : Form
    {
        public event Action<object> AddtoLike;

        private playMode mode=playMode.Sequence;

        private Video currentMovie = null;

        private List<Video> movieList = new List<Video>();//string日后可以替换为电影类

        //不同的构造函数可以调用
        public PlayerForm()
        {
            InitializeComponent();
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 35;
        }

        public PlayerForm(Video movie, List<Video> movies)
        {
            InitializeComponent();
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 35;
            currentMovie = movie;
            movieList = movies;
            foreach (Video one in movies)
            {
                listBox1.Items.Add(one.name);
            }
            try
            {
                axWindowsMediaPlayer1.URL = currentMovie.url;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            catch
            {
                MessageBox.Show("播放失败");
            }
        }

        public PlayerForm(Video movie)
        {
            InitializeComponent();
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 35;
            currentMovie = movie;
            foreach (Video one in movieList)
            {
                listBox1.Items.Add(one.name);
            }
            try
            {
                axWindowsMediaPlayer1.URL = currentMovie.url;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            catch
            {
                MessageBox.Show("播放失败");
            }
        }

        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (!axWindowsMediaPlayer1.fullScreen)
                {
                    axWindowsMediaPlayer1.fullScreen = true;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "mp4文件|*.mp4|mkv文件|*.mkv|avi文件|*。avi";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string url = openFileDialog1.FileName;
                axWindowsMediaPlayer1.URL = url;
                try
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    currentMovie = new Video(fileName, openFileDialog1.FileName);
                    if (!listBox1.Items.Contains(openFileDialog1.SafeFileName))
                    {
                        listBox1.Items.Add(fileName);
                        movieList.Add(currentMovie);
                    }
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                catch
                {
                    MessageBox.Show("播放失败");
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddtoLike(currentMovie);
            MessageBox.Show("添加成功");
        }

        
        private void originToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skinEngine1.Active = false;
        }
        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (skinEngine1.Active == false)
            {
                skinEngine1.Active = true;
            }
            skinEngine1.SkinFile = "DiamondBlue.ssk";
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (skinEngine1.Active == false)
            {
                skinEngine1.Active = true;
            }
            this.skinEngine1.SkinFile = "GlassOrange.ssk";
        }

        private void silverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (skinEngine1.Active == false)
            {
                skinEngine1.Active = true;
            }
            this.skinEngine1.SkinFile = "XPSilver.ssk";
        }

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt
        (
            IntPtr hdcDest,    //目标DC的句柄
            int nXDest,        //目标DC的矩形区域的左上角的x坐标
            int nYDest,        //目标DC的矩形区域的左上角的y坐标
            int nWidth,        //目标DC的句型区域的宽度值
            int nHeight,       //目标DC的句型区域的高度值
            IntPtr hdcSrc,     //源DC的句
            int nXSrc,         //源DC的矩形区域的左上角的x坐标
            int nYSrc,         //源DC的矩形区域的左上角的y坐标
            System.Int32 dwRo  //光栅的处理数值
        );

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public extern static IntPtr GetDC(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public extern static int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private void 视频截图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IntPtr hwnd1 = FindWindow(null, "视频播放器");
            if (!hwnd1.Equals(IntPtr.Zero))
            {
                GetWindowRect(hwnd1, out Rectangle rect);  //获得目标窗体的大小
                Bitmap Pic = new Bitmap(rect.Width, rect.Height);
                Graphics g1 = Graphics.FromImage(Pic);
                IntPtr hdc1 = GetDC(hwnd1);
                IntPtr hdc2 = g1.GetHdc();  //得到Bitmap的DC
                BitBlt(hdc2, 0, 0, rect.Width, rect.Height, hdc1, 0, 0, 13369376);
                g1.ReleaseHdc(hdc2);  //释放掉Bitmap的DC
                //以JPG文件格式保存
                saveFileDialog1.Filter = "jpeg文件|*.jpeg|png文件|*.png|bmp文件|*.bmp";
                //保存对话框是否记忆上次打开的目录
                saveFileDialog1.RestoreDirectory = true;
                //点了保存按钮进入
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string localFilePath = saveFileDialog1.FileName.ToString();
                    Pic.Save(localFilePath);
                }
            }
        }

            //绘制listbox的项
            private void listBox1_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            Color[] colors = new Color[5] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue };
            if (e.Index >= 0)
            {
                e.DrawBackground();
                Brush myBrush = Brushes.Black;
                Color bgColor = colors[e.Index % 5];
                e.Graphics.FillRectangle(new SolidBrush(bgColor), e.Bounds);
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        //中文切换，读取中国版本的资源
        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
            ResourceManager rm = new ResourceManager(typeof(PlayerForm));
            this.Text= rm.GetString("$this.Text");
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Text = rm.GetString(item.Name + ".Text");
                if (item.HasDropDownItems)
                {
                    foreach (ToolStripMenuItem dropitem in item.DropDownItems)
                    {
                        dropitem.Text = rm.GetString(dropitem.Name + ".Text");
                        if (dropitem.HasDropDownItems)
                        {
                            foreach (ToolStripMenuItem detailitem in dropitem.DropDownItems)
                            {
                                detailitem.Text = rm.GetString(detailitem.Name + ".Text");
                            }
                        }
                    }
                }
            }
            label1.Text= rm.GetString("label1.Text");
            label2.Text = rm.GetString("label2.Text");
            label3.Text = rm.GetString("label3.Text");
            label4.Text = rm.GetString("label4.Text");
            label5.Text = rm.GetString("label7.Text");
            label6.Text = rm.GetString("label8.Text");
            tabPage1.Text= rm.GetString("tabPage1.Text");
            tabPage2.Text = rm.GetString("tabPage2.Text");
        }


        //英文切换，读取英国版本的资源
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            ResourceManager rm = new ResourceManager(typeof(PlayerForm));
            this.Text = rm.GetString("$this.Text");
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Text = rm.GetString(item.Name + ".Text");
                if (item.HasDropDownItems)
                {
                    foreach (ToolStripMenuItem dropitem in item.DropDownItems)
                    {
                        dropitem.Text = rm.GetString(dropitem.Name + ".Text");
                        if (dropitem.HasDropDownItems)
                        {
                            foreach (ToolStripMenuItem detailitem in dropitem.DropDownItems)
                            {
                                detailitem.Text = rm.GetString(detailitem.Name + ".Text");
                            }
                        }
                    }
                }
            }
                    label1.Text = rm.GetString("label1.Text");
                    label2.Text = rm.GetString("label2.Text");
                    label3.Text = rm.GetString("label3.Text");
                    label4.Text = rm.GetString("label4.Text");
                    label5.Text = rm.GetString("label7.Text");
                    label6.Text = rm.GetString("label8.Text");
                    tabPage1.Text = rm.GetString("tabPage1.Text");
                    tabPage2.Text = rm.GetString("tabPage2.Text");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                axWindowsMediaPlayer1.URL = movieList[listBox1.SelectedIndex].url;
                try
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    currentMovie = movieList[listBox1.SelectedIndex];
                }
                catch {
                    MessageBox.Show("播放失败");
                }
            }
        }

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMovie.name != "")
            {
                string name = axWindowsMediaPlayer1.currentMedia.getItemInfo("Title");
                string length = axWindowsMediaPlayer1.currentMedia.getItemInfo("Duration");
                string url = axWindowsMediaPlayer1.currentMedia.getItemInfo("sourceURL");
                InformationForm informationForm = new InformationForm(name, length, url, this.skinEngine1.SkinFile);
                informationForm.ShowDialog();
            }
        }


        private void getPath()
        {

            switch (mode)
            {
                case playMode.Sequence:
                    int length1 = listBox1.Items.Count;
                    int num1 = 0;
                    for (int i = 0; i < length1; i++)
                    {
                        if (listBox1.Items[i].Equals(currentMovie.name))
                        {
                            num1 = i;
                        }
                    }
                    currentMovie = movieList[(num1 + 1) % length1];
                    axWindowsMediaPlayer1.URL = movieList[(num1 + 1) % length1].url;
                    break;
                case playMode.Random:
                    int length = movieList.Count;
                    Random rd = new Random();
                    int num2 = rd.Next(0, length);
                    currentMovie = movieList[num2];
                    axWindowsMediaPlayer1.URL = movieList[num2].url;
                    break;
                case playMode.Self:
                    int length3 = listBox1.Items.Count;
                    int num3 = 0;
                    for (int i = 0; i < length3; i++)
                    {
                        if (listBox1.Items[i].Equals(currentMovie.name))
                        {
                            num3 = i;
                        }
                    }
                    currentMovie = movieList[num3];
                    axWindowsMediaPlayer1.URL = movieList[num3].url;
                    break;
            }
        }
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                getPath();
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                label2.Text = axWindowsMediaPlayer1.currentMedia.getItemInfo("Title");
                label4.Text = axWindowsMediaPlayer1.currentMedia.getItemInfo("Duration");
                label6.Text = axWindowsMediaPlayer1.currentMedia.getItemInfo("sourceURL");
            }
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            getPath();
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMovie != null) {
                string url = currentMovie.url;
                System.IO.FileInfo file = new System.IO.FileInfo(url);
                string v_OpenFolderPath = file.DirectoryName; 
                System.Diagnostics.Process.Start("explorer.exe", v_OpenFolderPath);
            }
        }

        private void speedToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in 倍速ToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                if (item.Name == e.ClickedItem.Name) {
                    item.Checked = true;
                    switch (e.ClickedItem.Name)
                    {
                        case "toolStripMenuItem1":
                            axWindowsMediaPlayer1.settings.rate = 1;
                            break;
                        case "toolStripMenuItem2":
                            axWindowsMediaPlayer1.settings.rate = 1.25;
                            break;
                        case "toolStripMenuItem3":
                            axWindowsMediaPlayer1.settings.rate = 1.5;
                            break;
                        case "toolStripMenuItem4":
                            axWindowsMediaPlayer1.settings.rate = 1.75;
                            break;
                        case "toolStripMenuItem5":
                            axWindowsMediaPlayer1.settings.rate = 2.0;
                            break;
                        case "toolStripMenuItem6":
                            SpeedForm s = new SpeedForm(this.skinEngine1.SkinFile);
                            if (s.ShowDialog() == DialogResult.OK)
                            {
                                axWindowsMediaPlayer1.settings.rate = s.getValue();
                            }
                            break;
                    }
                }
            }
            
        }

        private void languageToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                if (item.Text == e.ClickedItem.Text)
                {
                    item.Checked = true;
                }
            }
        }

        private void playmodeToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in playmodeToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                if (item.Text == e.ClickedItem.Text)
                {
                    item.Checked = true;
                }
            }
        }

        private void skinToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in skinToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                if (item.Name == e.ClickedItem.Name)
                {
                    item.Checked = true;
                    switch (e.ClickedItem.Name) {
                        case "sequenceToolStripMenuItem":
                            mode = playMode.Sequence;
                            break;
                        case "selfToolStripMenuItem":
                            mode = playMode.Self;
                            break;
                        case "randomToolStripMenuItem":
                            mode = playMode.Random;
                            break;
                    }
                }
            }
        }

        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }

    public enum playMode { Sequence = 1, Random = 2, Self = 3 };

    public class Video
    {
        public string name { get; set; }

        public string url { get; set; }

        public Video(string name, string url)
        {
            this.name = name;
            this.url = url;
        }
    }
}