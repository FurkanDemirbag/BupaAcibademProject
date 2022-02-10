using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models
{
    public class Settings
    {
        public List<string> ConnectionStrings { get; set; }

        private static Settings _Current;

        public static Settings Current
        {
            get { return _Current; }
            set { _Current = value; }
        }
    }
}
