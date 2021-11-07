using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClassLibrary.CQRS.Commands
{
    public sealed class CommandsConnectionString
    {
        public string Value { get; }

        public CommandsConnectionString(string value)
        {
            Value = value;
        }
    }
}
