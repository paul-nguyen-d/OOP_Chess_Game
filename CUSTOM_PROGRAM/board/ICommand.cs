using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUSTOM_PROGRAM.board
{
    /// <summary>
    /// A Command interface. It can execute, undo, and redo
    /// </summary>
    public interface ICommand
    {
        public bool Execute();
        public void Undo();
        public void Redo();
    }
}
