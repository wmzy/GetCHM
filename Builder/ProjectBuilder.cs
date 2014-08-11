namespace Builder
{
    public class ProjectBuilder
    {
        public ProjectBuilder(string hhpFilePath)
        {
            HhpFilePath = hhpFilePath;
        }

        public string HhpFilePath { get; set; }
        public string HhcFilePath { get; set; }
        public string HhkFilePath { get; set; }

        public void Compile()
        {
            if (string.IsNullOrWhiteSpace(HhpFilePath)) return;

            var compiler = new Compiler(HhpFilePath);
            compiler.Compile();
        }
    }
}
