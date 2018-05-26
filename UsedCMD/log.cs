using System;
using System.Collections.Generic;
using System.Text;

namespace UsedCMD
{
    public static class log
    {
        public static string copyr = "Copyright ©2016 [ICE FROG] Powered By [CMD Helper] Version 1.0.0";

        public static List<LogEntity> InstructionSet = new List<LogEntity>();

        /// <summary>
        /// 增加一条指令记录对象到集合中
        /// </summary>
        /// <param name="loge"></param>
        public static void AddInstructionSet(LogEntity loge)
        {
            InstructionSet.Add(loge);
        }

        /// <summary>
        /// 获得指令集
        /// </summary>
        /// <returns></returns>
        public static List<LogEntity> GetInstructionSet()
        {
            return InstructionSet;
        }
    }

    public class LogEntity
    {
        /// <summary>
        /// 当前指令编号
        /// </summary>
        public int InstructionNumber { get; set; }

        /// <summary>
        /// 指令
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// 键入指令时间
        /// </summary>
        public DateTime InputDate { get; set; }

        /// <summary>
        /// 本地主机名
        /// </summary>
        public string LocalHostName { get; set; }

        public LogEntity(int InstructionNumber, string Instruction, DateTime InputDate, string LocalHostName)
        {
            this.InstructionNumber = InstructionNumber;
            this.Instruction = Instruction;
            this.InputDate = InputDate;
            this.LocalHostName = LocalHostName;
        }
    }
}
