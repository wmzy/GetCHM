using System.Runtime.InteropServices;

namespace Builder
{
    public class Compiler
    {
        public Compiler(string fileName)
        {
            FileName = fileName;
        }
        public string FileName { get; set; }
        public string CompileInfo { get; private set; }
        public string ProgreassInfo { get; private set; }

        delegate bool GetInfoCall(string info);

        //编译信息
        public bool GetCompileInfoCall(string info)
        {
            CompileInfo = info;
            return true;
        }

        //进度信息
        public bool GetProgreassInfoCall(string info)
        {
            ProgreassInfo = info;
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
