using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyscanner.Commands
{
    public class Command
    {
        public string name;
        public Command(string name)
        {
            this.name = name;
        }

        public virtual string execute(Dictionary<string, string> args)
        {
            return "";
        }
    }
}
