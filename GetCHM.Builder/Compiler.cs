using System.Runtime.InteropServices;
using System.Text;

namespace GetCHM.Builder
{
    public class Compiler
    {
        public Compiler(string fileName)
        {
            FileName = fileName;
        }
        public string FileName { get; set; }
        private readonly StringBuilder _compileInfo = new StringBuilder();
        private readonly StringBuilder _progressInfo = new StringBuilder();

        public string CompileInfo
        {
            get
            {
                return _compileInfo.ToString();
            }
        }

        public string ProgressInfo
        {
            get
            {
                return _progressInfo.ToString();
            }
        }


        //编译信息
        private bool GetCompileInfoCall(string info)
        {
            _compileInfo.AppendLine(info);
            return true;
        }

        //进度信息
        private bool GetProgreassInfoCall(string info)
        {
            _progressInfo.AppendLine(info);
            return true;
        }

        [DllImport("hha.dll", EntryPoint = "HHA_CompileHHP")]
        private extern static void CompileHHP(string hhpFile, GetInfoCall getCompileInfoCall, GetInfoCall getProgreassInfoCall, int stack);

        public void Compile()
        {
            CompileHHP(FileName, GetCompileInfoCall, GetProgreassInfoCall, 0);
        }
        public static void Compile(string fileName)
        {
            CompileHHP(fileName, info => true, info => true, 0);
        }
        public static void Compile(string fileName, GetInfoCall getCompileInfoCall, GetInfoCall getProgreassInfoCall)
        {
            CompileHHP(fileName, getCompileInfoCall, getProgreassInfoCall, 0);
        }
    }

    public delegate bool GetInfoCall(string info);
}
