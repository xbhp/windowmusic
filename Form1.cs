using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsMusic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> list = new List<string>();//用于储存音乐的全路径
        bool b = true;
        private void pay_Click(object sender, EventArgs e)
        {
            if (pay.Text == "播放")
            {
                if (b)
                {
                    //获得选中的歌曲  让音乐从头播放
                    PlayerMusic.URL = list[listBox1.SelectedIndex];
                }
                PlayerMusic.Ctlcontrols.play();
                pay.Text = "暂停";
            }
            else if (pay.Text == "暂停")
            {
                PlayerMusic.Ctlcontrols.pause();
                pay.Text = "播放";
                b = false;
            }
        }

        private void pause_Click(object sender, EventArgs e)
        {
            PlayerMusic.Ctlcontrols.stop();//停止
        }

        private void next_Click(object sender, EventArgs e)
        {
            //获得当前选中的索引
            int a = listBox1.SelectedIndex + 1;
            //清空所有选中的索引   这里是因为我们开启了多选属性，才需要清理
            listBox1.SelectedIndices.Clear();
            if (a == listBox1.Items.Count)
            {
                a = 0;
            }
            //将改变后的索引重新赋值给当前选中项的索引
            listBox1.SelectedIndex = a;
            PlayerMusic.URL = list[a];
            PlayerMusic.Ctlcontrols.play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //在程序加载的时候，取消播放器的自动播放功能
            PlayerMusic.settings.autoStart = false;
            PlayerMusic.URL = @"E:\KuGou\张杰&张碧晨 - 只要平凡.mp3";

           // label1.Image = Image.FromFile(@"C:\Users\14505\Desktop\继续.jpg");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("请选择音乐！");
                return;
            }
            try
            {
                PlayerMusic.URL = list[listBox1.SelectedIndex];
                PlayerMusic.Ctlcontrols.play();
                pay.Text = "暂停";
                PlayerMusic.Text = PlayerMusic.Ctlcontrols.currentPosition.ToString();
            }
            catch { }
        }

        private void selectmusic_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择您的文件";
            ofd.Filter = "音乐文件|*.mp3|全部文件|*.*";
            ofd.InitialDirectory = @"E:\CloudMusic";
            ofd.Multiselect = true;
            ofd.ShowDialog();

            //获得在文本框中选择的全路径
            string[] path = ofd.FileNames;
            for (int i = 0; i < path.Length; i++)
            {
                list.Add(path[i]);
                //将音乐文件的文件名存储到listbox中

                listBox1.Items.Add(Path.GetFileName(path[i]));
            }
        }

        private void up_Click(object sender, EventArgs e)
        {
            int a = listBox1.SelectedIndex - 1;
            listBox1.SelectedIndices.Clear();
            if (a < 0)
            {
                a = listBox1.Items.Count - 1;
            }
            //将改变后的索引重新赋值给当前选中项的索引
            listBox1.SelectedIndex = a;
            PlayerMusic.URL = list[a];
            PlayerMusic.Ctlcontrols.play();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //如果播放器的状态时正在播放中
            if (PlayerMusic.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                info.Text = PlayerMusic.currentMedia.duration.ToString() + "\r\n" + PlayerMusic.currentMedia.durationString + "\r\n" + PlayerMusic.Ctlcontrols.currentPositionString;

                double b1 = double.Parse(PlayerMusic.currentMedia.duration.ToString());
                double b2 = double.Parse(PlayerMusic.Ctlcontrols.currentPosition.ToString()) + 1;
                //如果歌曲当前的播放时间等于歌曲的总时间，自动播放下一曲    //比较时间的值
                if (b1 <= b2)
                {
                    //获得当前选中的索引
                    int a = listBox1.SelectedIndex + 1;
                    //清空所有选中的索引
                    listBox1.SelectedIndices.Clear();
                    if (a == listBox1.Items.Count)
                    {
                        a = 0;
                    }
                    //将改变后的索引重新赋值给当前选中项的索引
                    listBox1.SelectedIndex = a;
                    PlayerMusic.URL = list[a];
                    PlayerMusic.Ctlcontrols.play();
                }
            }
            //比较时间的值
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //先删集合
            //首先获得要删除的歌曲的数量
            int count = listBox1.SelectedItems.Count;
            for (int i = 0; i < count; i++)
            {
                //先删集合
                list.RemoveAt(listBox1.SelectedIndex);
                //在删列表
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }
    }
}
