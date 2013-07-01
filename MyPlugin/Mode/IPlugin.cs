using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPlugin.Mode
{
    public interface IPlugin
    {
        string SID { get; set; }

        string description { get; set; }

        string Aurthor { get; set; }
    }
}
