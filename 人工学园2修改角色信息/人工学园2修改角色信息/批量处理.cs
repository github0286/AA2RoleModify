using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace 人工学园2修改角色信息
{
    public partial class 批量处理 : Form
    {
        private BackgroundWorker 后台任务 = new BackgroundWorker();
        进度 进度窗口 = new 进度();
        public 批量处理()
        {
            InitializeComponent();
            后台任务.WorkerReportsProgress = true;
            后台任务.DoWork += 任务_执行;
            后台任务.ProgressChanged += 任务_进度改变;
            后台任务.RunWorkerCompleted += 任务_完成;
        }


        private void 任务_执行(object sender, DoWorkEventArgs e)
        {
            string 命令 = (string)e.Argument;
            人工学园2 数据 = new 人工学园2();
            int 数量 = 0;
            string[] 文件 = (string[])进度窗口.数据["文件"];
            if (命令 == "以角色名字重命名文件")
            {
                foreach (var item in 文件)
                {
                    数据.读取(item);
                    string 角色名字 = 数据.获取姓() + 数据.获取名();//因为编码不一样不能直接拼接要用变量保存
                    string 名字 = Path.GetDirectoryName(item) + Path.DirectorySeparatorChar + 角色名字 + ".png";
                    if (!File.Exists(名字))
                    {
                        File.Move(item, 名字);
                    }
                    数量++;
                    后台任务.ReportProgress(数量, "更新数量");
                }
            }
            else if (命令 == "对角色中式重命名" || 命令 == "对角色日式重命名")
            {
                string 风格 = 命令 == "对角色中式重命名" ? "中式" : "日式";
                List<string> 名字列表 = new List<string>();
                foreach (var item in 文件)
                {
                    数据.读取(item);
                    string 姓 = 数据.获取随机姓(风格), 名 = 数据.获取随机名(风格, 数据.获取性别());
                    string 名字 = 姓 + 名;
                    while (名字列表.Contains(名字))
                    {
                        姓 = 数据.获取随机姓(风格);
                        名 = 数据.获取随机名(风格, 数据.获取性别());
                    }
                    名字列表.Add(名字);
                    数据.设置姓(姓);
                    数据.设置名(名);
                    数据.保存(item);
                    数量++;
                    后台任务.ReportProgress(数量, "更新数量");
                }
            }
            else if (命令 == "个性添加" || 命令 == "个性删除")
            {
                bool 拥有 = 命令 == "个性添加" ? true : false;
                int[] 个性 = (int[])进度窗口.数据["个性"];
                foreach (var item in 文件)
                {
                    数据.读取(item);
                    foreach (var 个性索引 in 个性)
                    {
                        数据.设置个性(个性索引, 拥有);
                    }
                    数据.保存(item);
                    数量++;
                    后台任务.ReportProgress(数量, "更新数量");
                }
            }
        }
        private void 任务_进度改变(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                float 总数量 = (int)进度窗口.数据["总数量"], 数量 = e.ProgressPercentage;
                进度窗口.label1.Text = 数量.ToString() + "/" + 总数量.ToString();
                进度窗口.progressBar1.Value = (int)(数量 / 总数量 * 100);
            }
        }
        private void 任务_完成(object sender, RunWorkerCompletedEventArgs e)
        {
            进度窗口.Hide();
            MessageBox.Show("完成");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] 文件 = openFileDialog1.FileNames;
                进度窗口.数据["总数量"] = 文件.Length;
                进度窗口.数据["文件"] = 文件;
                后台任务.RunWorkerAsync("以角色名字重命名文件");
                进度窗口.ShowDialog();//要放在运行后面才有效
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] 文件 = openFileDialog1.FileNames;
                进度窗口.数据["总数量"] = 文件.Length;
                进度窗口.数据["文件"] = 文件;
                后台任务.RunWorkerAsync("对角色中式重命名");
                进度窗口.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] 文件 = openFileDialog1.FileNames;
                进度窗口.数据["总数量"] = 文件.Length;
                进度窗口.数据["文件"] = 文件;
                后台任务.RunWorkerAsync("对角色日式重命名");
                进度窗口.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                批量处理_个性操作 窗口 = new 批量处理_个性操作();
                窗口.ShowDialog();
                int[] 个性 = 窗口.个性.ToArray();
                int 操作类型 = 窗口.操作类型;
                if (个性.Length > 0)
                {
                    string[] 文件 = openFileDialog1.FileNames;
                    进度窗口.数据["个性"] = 个性;
                    进度窗口.数据["总数量"] = 文件.Length;
                    进度窗口.数据["文件"] = 文件;
                    后台任务.RunWorkerAsync("个性" + (操作类型 == 0 ? "添加" : "删除"));
                    进度窗口.ShowDialog();
                }
            }
        }

    }
}
