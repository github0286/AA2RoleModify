using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 人工学园2修改角色信息
{
    public partial class 进度 : Form
    {
        public 进度()
        {
            InitializeComponent();
        }

        public Dictionary<string, object> 数据=new Dictionary<string, object>();
        private void 批量任务_FormClosing(object sender, FormClosingEventArgs e)
        {
            //禁止主动关闭窗口
            e.Cancel = true;
        }
    }
}
