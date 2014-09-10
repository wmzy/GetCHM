using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCHM.Spider
{
    public interface IDownloader
    {
        void Start();
        void Pause();
        void Stop();
    }
}
