using github0286;
using System;
using System.IO;
using System.Windows.Forms;
namespace 人工学园2修改角色信息
{
    public partial class 主窗体 : Form
    {
        const string 版本 = "20220422";
        人工学园2 数据 = new 人工学园2();
        static string 数据目录 = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;
        string 常见问题路径 = 数据目录 + "常见问题.txt";
        string 文件路径;
        bool 读取数据 = false;
        public 主窗体()
        {
            InitializeComponent();
        }


        private void 主窗体_Load(object sender, EventArgs e)
        {
            Config.Read();
            Text += 版本+"-github0286";
            名字模式下拉框.SelectedItem = Config.Get("名字模式");
            持有物随机以人名为前缀框.Checked = Config.Get("持有物随机以人名为前缀") == "true";
            //为个性选择框绑定操作
            foreach (CheckBox item in 个性容器.Controls)
            {
                item.CheckedChanged += new System.EventHandler(个性选择框_CheckedChanged);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                读取数据 = true;
                文件路径 = openFileDialog1.FileName;
                数据.读取(文件路径);
                //显示图片
                pictureBox1.Image = 数据.获取图片(0);
                pictureBox2.Image = 数据.获取图片(1);
                //显示姓名
                姓文本框.Text = 数据.获取姓();
                名文本框.Text = 数据.获取名();
                //显示个性
                foreach (CheckBox item in 个性容器.Controls)
                {
                    item.Checked = 数据.获取个性(int.Parse(item.Tag.ToString()));
                }
                //显示持有物
                持有物_爱情文本框.Text = 数据.获取持有物_爱情();
                持有物_友情文本框.Text = 数据.获取持有物_友情();
                持有物_色情文本框.Text = 数据.获取持有物_色情();
                //启用按钮
                随机姓按钮.Enabled = true;
                随机名按钮.Enabled = true;
                随机持有物_爱情按钮.Enabled = true;
                随机持有物_友情按钮.Enabled = true;
                随机持有物_色情按钮.Enabled = true;
                保存按钮.Enabled = true;
                读取数据 = false;
                //导出数据块
                //string 路径 = "E:/Download/";
                //File.WriteAllBytes(路径+Path.GetFileNameWithoutExtension(文件路径), 数据.获取游戏数据字节());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new 批量处理().ShowDialog();
        }


        private void 保存按钮_Click(object sender, EventArgs e)
        {
            Enabled = false;
            bool 保存 = true;
            string 姓 = 姓文本框.Text, 名 = 名文本框.Text, 持有物_爱情 = 持有物_爱情文本框.Text, 持有物_友情 = 持有物_友情文本框.Text, 持有物_色情 = 持有物_色情文本框.Text;
            if (姓 == "" || 名 == "" || 持有物_爱情 == "" || 持有物_友情 == "" || 持有物_色情 == "")
            {
                保存 = false;
                MessageBox.Show("姓,名和持有物不能填空的");
            }
            else
            {
                foreach (CheckBox item in 个性容器.Controls)
                {

                }
            }
            if (保存)
            {
                数据.保存(文件路径);
                MessageBox.Show("保存完成");
            }
            Enabled = true;
        }
        //关闭时保存配置
        private void 主窗体_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Config.IsChange())
            {
                Config.Save();
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(File.ReadAllText(常见问题路径));
        }

        private void 名字模式下拉框_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.Set("名字模式", 名字模式下拉框.Text);
        }

        private void 随机姓名按钮_Click(object sender, EventArgs e)
        {
            string 风格 = 名字模式下拉框.Text;
            姓文本框.Text = 数据.获取随机姓(风格);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            string 风格 = 名字模式下拉框.Text;
            名文本框.Text = 数据.获取随机名(风格, 数据.获取性别());
        }

        private void 持有物随机以人名为前缀框_CheckedChanged(object sender, EventArgs e)
        {
            Config.Set("持有物随机以人名为前缀", 持有物随机以人名为前缀框.Checked == true ? "true" : "false");
        }

        private void 随机持有物_爱情按钮_Click(object sender, EventArgs e)
        {
            string 持有物 = 数据.获取随机持有物("爱情");
            if (持有物随机以人名为前缀框.Checked)
            {
                持有物 = 姓文本框.Text + 名文本框.Text + "的" + 持有物;
            }
            持有物_爱情文本框.Text = 持有物;
        }

        private void 随机持有物_友情按钮_Click(object sender, EventArgs e)
        {
            string 持有物 = 数据.获取随机持有物("友情");
            if (持有物随机以人名为前缀框.Checked)
            {
                持有物 = 姓文本框.Text + 名文本框.Text + "的" + 持有物;
            }
            持有物_友情文本框.Text = 持有物;
        }

        private void 随机持有物_色情按钮_Click(object sender, EventArgs e)
        {
            string 持有物 = 数据.获取随机持有物("色情");
            if (持有物随机以人名为前缀框.Checked)
            {
                持有物 = 姓文本框.Text + 名文本框.Text + "的" + 持有物;
            }
            持有物_色情文本框.Text = 持有物;
        }

        private void 姓文本框_TextChanged(object sender, EventArgs e)
        {
            if (!读取数据)
            { 
                数据.设置姓(姓文本框.Text);
            }
        }

        private void 名文本框_TextChanged(object sender, EventArgs e)
        {
            if (!读取数据)
            { 
                数据.设置名(名文本框.Text);
            }
        }

        private void 持有物_爱情文本框_TextChanged(object sender, EventArgs e)
        {
            if (!读取数据)
            {
                数据.设置持有物_爱情(持有物_爱情文本框.Text);
            }
        }

        private void 持有物_友情文本框_TextChanged(object sender, EventArgs e)
        {
            if (!读取数据)
            {
                数据.设置持有物_友情(持有物_友情文本框.Text);
            }
        }

        private void 持有物_色情文本框_TextChanged(object sender, EventArgs e)
        {
            if (!读取数据)
            {
                数据.设置持有物_色情(持有物_色情文本框.Text);
            }
        }
        private void 个性选择框_CheckedChanged(object sender, EventArgs e)
        {
            if (!读取数据)
            {
                CheckBox 选择框 = sender as CheckBox;
                数据.设置个性(int.Parse(选择框.Tag.ToString()),选择框.Checked);
            }
        }
    }
}
