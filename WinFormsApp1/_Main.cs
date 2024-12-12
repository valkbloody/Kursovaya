using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Кусовая;

namespace WinFormsApp1
{


    internal class _Main
    {
        static void Main(string[] args)
        {

            ApplicationConfiguration.Initialize();
            Application.Run(new _Database());
        }
    }
}
