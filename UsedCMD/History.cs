using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UsedCMD
{
    public partial class History : Form
    {
        public History()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 本次选中的指令内容
        /// </summary>
        public string Instruction;

        private void History_Load(object sender, EventArgs e)
        {
            Point loc = MousePosition;
            //loc.Offset(0, SystemInformation.CaptionHeight + SystemInformation.Border3DSize.Height);
            this.Location = loc;

            List<LogEntity> LogEntitySet = log.GetInstructionSet();
            foreach (LogEntity item in LogEntitySet)
            {
                ListViewItem lvi = new ListViewItem(item.InstructionNumber.ToString());
                lvi.SubItems.Add(item.Instruction);
                lvi.SubItems.Add(item.InputDate.ToString("yyyy-MM-dd"));
                lvi.SubItems.Add(item.LocalHostName);
                listView1.Items.Add(lvi);
            }
        }

        /// <summary>
        /// 窗体关闭中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void History_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(listView1.SelectedItems.Count>0)
                this.Instruction = listView1.SelectedItems[0].SubItems[1].Text;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                this.Instruction = listView1.SelectedItems[0].SubItems[1].Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
