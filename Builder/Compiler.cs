using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Builder
{
    public class Compiler
    {
        public Compiler(string fileName)
        {
            FileName = fileName;
            CompileInfo = new StringBuilder();
            ProgreassInfo = new StringBuilder();
        }
        public string FileName { get; set; }
        public StringBuilder CompileInfo { get; private set; }
        public StringBuilder ProgreassInfo { get; private set; }

        delegate bool GetInfoCall(string info);

        //编译信息
        public bool GetCompileInfoCall(string info)
        {
            CompileInfo.AppendLine(info);
            Console.WriteLine("ccccc" + info);
            return true;
        }

        //进度信息
        public bool GetProgreassInfoCall(string info)
        {
            ProgreassInfo.AppendLine(info);
            Console.WriteLine("pppppp" + info);
            return true;
        }

        [DllImport("hha.dll", EntryPoint = "HHA_CompileHHP")]
        private extern static void CompileHHP(string hhpFile, GetInfoCall getCompileInfoCall, GetInfoCall getProgreassInfoCall, int stack);

        public void Compile()
        {
            CompileHHP(FileName, GetCompileInfoCall, GetProgreassInfoCall, 0);
        }
    }
}
