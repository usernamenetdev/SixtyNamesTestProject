using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Commands
{
    public class CommandDescriptionAttribute : DescriptionAttribute
    {
        public string CommandDescription { get; private set; }

        public CommandDescriptionAttribute(string command, string commandDescription) : base(command)
        {
            CommandDescription = commandDescription;
        }
    }
}
