using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

using UsedCMD;

namespace UsedCMD
{
    public partial class CMD : Form
    {
        public CMD()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Execute(txt_Code.Text.Trim());
        }

        private int InstructionNumber = 1;
        /// <summary>
        /// 处理指令并且执行
        /// </summary>
        /// <param name="code"></param>
        private void Execute(string code)
        {
            //自增指令记录编号
            InstructionNumber++;

            //获取键入命令的最后一行[最新指令]
            StringReader sr = new StringReader(txt_Code.Text);
            string l;
            string endLine = string.Empty;
            while ((l = sr.ReadLine()) != null)
            {
                endLine = l;
            }
            code = endLine;
            //增加一条数据到指令集记录集合中
            log.AddInstructionSet(new LogEntity(InstructionNumber, endLine, DateTime.Now, System.Environment.MachineName));
            //代码就绪提示和换到新行
            string temp = txt_Code.Text + System.Environment.NewLine + "Completed, you can now type a new command:";
            txt_Code.Text = string.Empty;
            txt_Code.Text = temp + System.Environment.NewLine + "";
            txt_Code.SelectionStart = txt_Code.TextLength;
            //内部命令操作
            if (code == null || code.Trim().Length <= 0)
                return;
            if (code.Trim().ToLower().Equals("apphelp.icefrog"))//查看帮助信息指令
            {
                new Help().ShowDialog();
                return;
            }
            if (code.Trim().ToLower().Equals("cls"))
            {
                txt_Return.Text = string.Empty;
                return;
            }   
            //多线程访问cmd并且执行操作
            string returnContent = "";
            ThreadStart thread = new ThreadStart(delegate()
            {
                returnContent = Runcmd(code);
                this.AppendText(returnContent);
            });
            Thread t = new Thread(thread);
            t.Start();
        }

        //使用委托实现线程访问控件
        delegate void AppendTextHandler(string text);
        private void AppendText(string returnContent)
        {
            try
            {
                if (txt_Return.InvokeRequired)
                    txt_Return.Invoke(new AppendTextHandler(AppendText), returnContent);
                else
                {
                    if (returnContent == null || returnContent.Trim().Length <= 5 || returnContent.Split(' ').ToString().Length <= 5)
                    {
                        //Console.WriteLine(returnContent+","+returnContent.Length);
                        //txt_Return.AppendText(System.Environment.NewLine + "[ 未得到控制台反馈,可能的原因如下 ]" +
                        // System.Environment.NewLine + "1.命令错误-请检查键入命令是否有误。" +
                        //System.Environment.NewLine + "2.为可执行程序-检查是否有打开的应用。" +
                        // System.Environment.NewLine + "3.部分系统/软件服务打开并没有反馈,实际可能已经打开." +
                        // System.Environment.NewLine + "如果以上选项没有解决您的问题，请联系开发者[@ICE FROG]或键入[apphelp.icefrog]查看帮助");
                    }
                    else
                    {
                        txt_Return.AppendText(System.Environment.NewLine + returnContent);

                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// cmd执行函数
        /// </summary>
        /// <param name="command">cmd命令</param>
        /// <returns>控制台反馈字符串</returns>
        private string Runcmd(string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + command;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string content = null;//p.StandardOutput.ReadToEnd();
            while ((content = p.StandardOutput.ReadLine()) != null)
            {
                this.AppendText(content);
            }
            p.Close();
            //p.Kill();
            return content;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txt_Return.Text = log.copyr + System.Environment.NewLine;
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            txt_Code.Text = string.Empty;
            txt_Return.Text = string.Empty;
        }
        /// <summary>
        /// 执行选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Execute(txt_Code.SelectedText.Trim());
        }

        /// <summary>
        /// 键入历史查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            //History his = new History();
            // his.Deactivate += (x, y) =>
            //{
            //    his.Close();
            //};
            // his.Show(this);
            History his = new History();
            his.ShowDialog();
            txt_Code.AppendText(System.Environment.NewLine + his.Instruction);
        }

        private void CMD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Execute("exit");
            Application.Exit();
        }
    }
}