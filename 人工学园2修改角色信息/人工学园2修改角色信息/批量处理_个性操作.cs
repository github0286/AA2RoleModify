using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 人工学园2修改角色信息
{
    public partial class 批量处理_个性操作 : Form
    {
        public List<int> 个性 = new List<int>();
        public int 操作类型;
        public 批量处理_个性操作()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            操作类型 = 0;
            编辑个性();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            操作类型 = 1;
            编辑个性();
        }
        public void 编辑个性()
        { 
            foreach (CheckBox item in 个性容器.Controls)
            {
                if (item.Checked)
                {
                    个性.Add(int.Parse(item.Tag.ToString()));
                }
            }
            Close();
        }
    }
}
