using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using WMZY.Util;

namespace Spider
{
    public class Downloader : IDownloader
    {
        private readonly IRegistry _registry;

        public Downloader()
        {
            _registry = Registry.Instance;
            FilePath = @"D:\GetCHM\tem\";
            ElementQueries = new List<ElementQuery>()
            {
                new ElementQuery {Query = "//a", AttributeName = "href", Suffix = ".html",IsAutoIdentifySuffix = true},
                new ElementQuery {Query = "//img", AttributeName = "src", IsAutoIdentifySuffix = true},
                new ElementQuery {Query = @"//script[@type='text/javascript']", AttributeName = "src", Suffix = ".js", IsAutoIdentifySuffix = false},
                new ElementQuery {Query = @"//link[@rel='stylesheet']", AttributeName = "href", Suffix = ".css",IsAutoIdentifySuffix = false}
            };

            SuffixMap = new Dictionary<string, string>
            {
#region 文件名后缀
                {".3dm", ""},
				{".3dmf", ""},
				{".a", ""},
				{".aab", ""},
				{".aam", ""},
				{".aas", ""},
				{".abc", ""},
				{".acgi", ".html"},
				{".afl", ""},
				{".ai", ""},
				{".aif", ""},
				{".aifc", ""},
				{".aiff", ""},
				{".aim", ""},
				{".aip", ""},
				{".ani", ""},
				{".aos", ""},
				{".aps", ""},
				{".arc", ""},
				{".arj", ""},
				{".art", ""},
				{".asf", ""},
				{".asm", ""},
				{".asp", ".html"},
				{".asx", ".html"},
				{".au", ""},
				{".avi", ""},
				{".avs", ""},
				{".bcpio", ""},
				{".bin", ""},
				{".bm", ""},
				{".bmp", ""},
				{".boo", ""},
				{".book", ""},
				{".boz", ""},
				{".bsh", ""},
				{".bz", ""},
				{".bz2", ""},
				{".c", ""},
				{".c++", ""},
				{".cat", ""},
				{".cc", ""},
				{".ccad", ""},
				{".cco", ""},
				{".cdf", ""},
				{".cer", ""},
				{".cha", ""},
				{".chat", ""},
				{".class", ""},
				{".conf", ""},
				{".cpio", ""},
				{".cpp", ""},
				{".cpt", ""},
				{".crl", ""},
				{".crt", ""},
				{".csh", ""},
				{".css", ""},
				{".cxx", ""},
				{".dcr", ""},
				{".deepv", ""},
				{".def", ""},
				{".der", ""},
				{".dif", ""},
				{".dir", ""},
				{".dl", ""},
				{".doc", ""},
				{".dot", ""},
				{".dp", ""},
				{".drw", ""},
				{".dump", ""},
				{".dv", ""},
				{".dvi", ""},
				{".dwf", ""},
				{".dwg", ""},
				{".dxf", ""},
				{".dxr", ""},
				{".el", ""},
				{".elc", ""},
				{".env", ""},
				{".eps", ""},
				{".es", ""},
				{".etx", ""},
				{".evy", ""},
				{".exe", ""},
				{".f", ""},
				{".f77", ""},
				{".f90", ""},
				{".fdf", ""},
				{".fif", ""},
				{".fli", ""},
				{".flo", ""},
				{".flx", ""},
				{".fmf", ""},
				{".for", ""},
				{".fpx", ""},
				{".frl", ""},
				{".funk", ""},
				{".g", ""},
				{".g3", ""},
				{".gif", ""},
				{".gl", ""},
				{".gsd", ""},
				{".gsm", ""},
				{".gsp", ""},
				{".gss", ""},
				{".gtar", ""},
				{".gz", ""},
				{".gzip", ""},
				{".h", ""},
				{".hdf", ""},
				{".help", ""},
				{".hgl", ""},
				{".hh", ""},
				{".hlb", ""},
				{".hlp", ""},
				{".hpg", ""},
				{".hpgl", ""},
				{".hqx", ""},
				{".hta", ""},
				{".htc", ""},
				{".htm", ".html"},
				{".html", ".html"},
				{".htmls", ".html"},
				{".htt", ""},
				{".htx", ".html"},
				{".ice", ""},
				{".ico", ""},
				{".idc", ""},
				{".ief", ""},
				{".iefs", ""},
				{".iges", ""},
				{".igs", ""},
				{".ima", ""},
				{".imap", ""},
				{".inf", ""},
				{".ins", ""},
				{".ip", ""},
				{".isu", ""},
				{".it", ""},
				{".iv", ""},
				{".ivr", ""},
				{".ivy", ""},
				{".jam", ""},
				{".jav", ""},
				{".java", ""},
				{".jcm", ""},
				{".jfif", ""},
				{".jfif-tbnl", ""},
				{".jpe", ""},
				{".jpeg", ""},
				{".jpg", ""},
				{".jps", ".html"},
				{".js", ""},
				{".jut", ""},
				{".kar", ""},
				{".ksh", ""},
				{".la", ""},
				{".lam", ""},
				{".latex", ""},
				{".lha", ""},
				{".lhx", ""},
				{".list", ""},
				{".lma", ""},
				{".log", ""},
				{".lsp", ""},
				{".lst", ""},
				{".lsx", ""},
				{".ltx", ""},
				{".lzh", ""},
				{".lzx", ""},
				{".m", ""},
				{".m1v", ""},
				{".m2a", ""},
				{".m2v", ""},
				{".m3u", ""},
				{".man", ""},
				{".map", ""},
				{".mar", ""},
				{".mbd", ""},
				{".mc$", ""},
				{".mcd", ""},
				{".mcf", ""},
				{".mcp", ""},
				{".me", ""},
				{".mht", ""},
				{".mhtml", ".html"},
				{".mid", ""},
				{".midi", ""},
				{".mif", ""},
				{".mime", ""},
				{".mjf", ""},
				{".mjpg", ""},
				{".mm", ""},
				{".mme", ""},
				{".mod", ""},
				{".moov", ""},
				{".mov", ""},
				{".movie", ""},
				{".mp2", ""},
				{".mp3", ""},
				{".mpa", ""},
				{".mpc", ""},
				{".mpe", ""},
				{".mpeg", ""},
				{".mpg", ""},
				{".mpga", ""},
				{".mpp", ""},
				{".mpt", ""},
				{".mpv", ""},
				{".mpx", ""},
				{".mrc", ""},
				{".ms", ""},
				{".mv", ""},
				{".my", ""},
				{".mzz", ""},
				{".nap", ""},
				{".naplps", ""},
				{".nc", ""},
				{".ncm", ""},
				{".nif", ""},
				{".niff", ""},
				{".nix", ""},
				{".nsc", ""},
				{".nvd", ""},
				{".o", ""},
				{".oda", ""},
				{".omc", ""},
				{".omcd", ""},
				{".omcr", ""},
				{".p", ""},
				{".p10", ""},
				{".p12", ""},
				{".p7a", ""},
				{".p7c", ""},
				{".p7m", ""},
				{".p7r", ""},
				{".p7s", ""},
				{".part", ""},
				{".pas", ""},
				{".pbm", ""},
				{".pcl", ""},
				{".pct", ""},
				{".pcx", ""},
				{".pdb", ""},
				{".pdf", ""},
				{".pfunk", ""},
				{".pgm", ""},
				{".pic", ""},
				{".pict", ""},
				{".pkg", ""},
				{".pko", ""},
				{".pl", ""},
				{".plx", ""},
				{".pm", ""},
				{".pm4", ""},
				{".pm5", ""},
				{".png", ""},
				{".pnm", ""},
				{".pot", ""},
				{".pov", ""},
				{".ppa", ""},
				{".ppm", ""},
				{".pps", ""},
				{".ppt", ""},
				{".ppz", ""},
				{".pre", ""},
				{".prt", ""},
				{".ps", ""},
				{".psd", ""},
				{".pvu", ""},
				{".pwz", ""},
				{".py", ""},
				{".pyc", ""},
				{".qcp", ""},
				{".qd3", ""},
				{".qd3d", ""},
				{".qif", ""},
				{".qt", ""},
				{".qtc", ""},
				{".qti", ""},
				{".qtif", ""},
				{".ra", ""},
				{".ram", ""},
				{".ras", ""},
				{".rast", ""},
				{".rexx", ""},
				{".rf", ""},
				{".rgb", ""},
				{".rm", ""},
				{".rmi", ""},
				{".rmm", ""},
				{".rmp", ""},
				{".rng", ""},
				{".rnx", ""},
				{".roff", ""},
				{".rp", ""},
				{".rpm", ""},
				{".rt", ""},
				{".rtf", ""},
				{".rtx", ""},
				{".rv", ""},
				{".s", ""},
				{".s3m", ""},
				{".saveme", ""},
				{".sbk", ""},
				{".scm", ""},
				{".sdml", ""},
				{".sdp", ""},
				{".sdr", ""},
				{".sea", ""},
				{".set", ""},
				{".sgm", ""},
				{".sgml", ""},
				{".sh", ""},
				{".shar", ""},
				{".shtml", ".html"},
				{".sid", ""},
				{".sit", ""},
				{".skd", ""},
				{".skm", ""},
				{".skp", ""},
				{".skt", ""},
				{".sl", ""},
				{".smi", ""},
				{".smil", ""},
				{".snd", ""},
				{".sol", ""},
				{".spc", ""},
				{".spl", ""},
				{".spr", ""},
				{".sprite", ""},
				{".src", ""},
				{".ssi", ""},
				{".ssm", ""},
				{".sst", ""},
				{".step", ""},
				{".stl", ""},
				{".stp", ""},
				{".sv4cpio", ""},
				{".sv4crc", ""},
				{".svf", ""},
				{".svr", ""},
				{".swf", ""},
				{".t", ""},
				{".talk", ""},
				{".tar", ""},
				{".tbk", ""},
				{".tcl", ""},
				{".tcsh", ""},
				{".tex", ""},
				{".texi", ""},
				{".texinfo", ""},
				{".text", ""},
				{".tgz", ""},
				{".tif", ""},
				{".tiff", ""},
				{".tr", ""},
				{".tsi", ""},
				{".tsp", ""},
				{".tsv", ""},
				{".turbot", ""},
				{".txt", ""},
				{".uil", ""},
				{".uni", ""},
				{".unis", ""},
				{".unv", ""},
				{".uri", ""},
				{".uris", ""},
				{".ustar", ""},
				{".uu", ""},
				{".uue", ""},
				{".vcd", ""},
				{".vcs", ""},
				{".vda", ""},
				{".vdo", ""},
				{".vew", ""},
				{".viv", ""},
				{".vivo", ""},
				{".vmd", ""},
				{".vmf", ""},
				{".voc", ""},
				{".vos", ""},
				{".vox", ""},
				{".vqe", ""},
				{".vqf", ""},
				{".vql", ""},
				{".vrml", ""},
				{".vrt", ""},
				{".vsd", ""},
				{".vst", ""},
				{".vsw", ""},
				{".w60", ""},
				{".w61", ""},
				{".w6w", ""},
				{".wav", ""},
				{".wb1", ""},
				{".wbmp", ""},
				{".web", ""},
				{".wiz", ""},
				{".wk1", ""},
				{".wmf", ""},
				{".wml", ""},
				{".wmlc", ""},
				{".wmls", ""},
				{".wmlsc", ""},
				{".word", ""},
				{".wp", ""},
				{".wp5", ""},
				{".wp6", ""},
				{".wpd", ""},
				{".wq1", ""},
				{".wri", ""},
				{".wrl", ""},
				{".wrz", ""},
				{".wsc", ""},
				{".wsrc", ""},
				{".wtk", ""},
				{".xbm", ""},
				{".xdr", ""},
				{".xgz", ""},
				{".xif", ""},
				{".xl", ""},
				{".xla", ""},
				{".xlb", ""},
				{".xlc", ""},
				{".xld", ""},
				{".xlk", ""},
				{".xll", ""},
				{".xlm", ""},
				{".xls", ""},
				{".xlt", ""},
				{".xlv", ""},
				{".xlw", ""},
				{".xm", ""},
				{".xml", ""},
				{".xmz", ""},
				{".xpix", ""},
				{".xpm", ""},
				{".x-png", ""},
				{".xsr", ""},
				{".xwd", ""},
				{".xyz", ""},
				{".z", ""},
				{".zip", ""},
				{".zoo", ""},
				{".zsh", ""}

	#endregion
            };
        }
        public Downloader(IRegistry registry)
        {
            _registry = registry;
        }

        public void Start()
        {
            while (true)
            {
                var taskQueue = new Queue<Task>();
                Task t;
                while (_registry.HasNew)
                {
                    var record = _registry.PopNew();
                    t = FetchAsync(record);

                    taskQueue.Enqueue(t);
                }
                if (taskQueue.Count <= 0)
                {
                    break;
                }

                t = taskQueue.Dequeue();
                t.Wait(5000);
            }
        }

        private async Task FetchAsync(Record record)
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(record.Uri);
            hwr.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            hwr.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            try
            {
                using (var res = await hwr.GetResponseAsync())
                {
                    if (Filter(res))
                    {
                        if (res.ContentType.Contains("text/html"))
                        {
                            var hDoc = new HtmlDocument();
                            using (var resStream = res.GetResponseStream())
                            {
                                hDoc.Load(resStream, Encoding.UTF8);
                            }
                            record.Title = GetTitle(hDoc);
                            ReplaceUrl(hDoc, record.Uri);
                            hDoc.Save(Path.Combine(FilePath, record.FileName));
                        }
                        else
                        {
                            using (var fs = File.OpenWrite(FilePath + record.FileName))
                            {
                                var responseStream = res.GetResponseStream();
                                if (responseStream != null) await responseStream.CopyToAsync(fs);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Source + exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }

        private string GetTitle(HtmlDocument htmlDocument)
        {
            var elem = htmlDocument.DocumentNode.SelectSingleNode("/html/head/title");
            return elem == null ? null : elem.InnerText;
        }

        public List<ElementQuery> ElementQueries { get; set; }

        private void ReplaceUrl(HtmlDocument hDoc, Uri uri)
        {
            foreach (var elementQuery in ElementQueries)
            {
                var elements = hDoc.DocumentNode.SelectNodes(elementQuery.Query);
                if (elements == null)
                    continue;
                foreach (var elem in elements)
                {
                    var urlAttr = elem.Attributes[elementQuery.AttributeName];
                    if (urlAttr == null)
                        continue;
                    var url = urlAttr.Value;
                    if (FilterUrl(url))
                    {
                        string suffix = elementQuery.IsAutoIdentifySuffix ? GetSuffixFromUrl(url).NullOrWhiteSpaceDefault(elementQuery.Suffix) : elementQuery.Suffix;
                        var newUrl = new Uri(uri, url);
                        var record = _registry.Add(newUrl, suffix);
                        int index = url.IndexOf('#');
                        elem.Attributes[elementQuery.AttributeName].Value = index > -1
                            ? record.FileName +
                              url.Substring(index)
                            : record.FileName;
                    }
                }
            }
        }

        private string GetSuffixFromUrl(string url)
        {
            string suffix = null;
            int lIndex = url.IndexOfAny(new char[] { '#', '?' });
            int rIndex;
            int suffixIndex;
            if (lIndex > -1)
            {
                rIndex = url.LastIndexOf('/', lIndex);
                suffixIndex = url.LastIndexOf('.', lIndex);
                if (rIndex >= suffixIndex) return null;

                suffix = url.Substring(suffixIndex, lIndex - suffixIndex);
            }
            else
            {
                rIndex = url.LastIndexOf('/');
                suffixIndex = url.LastIndexOf('.');
                if (rIndex >= suffixIndex) return null;

                suffix = url.Substring(suffixIndex);
            }
            return !SuffixMap.ContainsKey(suffix) ? null : SuffixMap[suffix].NullOrEmptyDefault(suffix);
        }

        public IDictionary<string, string> SuffixMap { get; set; }

        public Filter Filter = res => true;
        public FilterUrl FilterUrl = url => true;

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private string _fileName;

        public string FilePath
        {
            get { return _fileName; }

            set
            {
                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }
                _fileName = value;
            }
        }
    }

    public delegate bool Filter(WebResponse res);

    public delegate bool FilterUrl(string url);
}
