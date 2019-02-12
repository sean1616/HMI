using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_Kernel.Periphery
{
    public interface IHeater
    {
        string RetDataFrame { get; set; }
        string CmdSetValue(double val);
        string CmdReadValue();

    }
}
