using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    class CommandManager
    {
        private List<ICommand> commandList = new List<ICommand>();
        private int index;
        public void AddCommand(ICommand command)
        {
            if (index < commandList.Count)
                commandList.RemoveRange(index, commandList.Count - index);

            commandList.Add(command);
            command.Execute();
            index++;
        }
        public void Redo()
        {
            if (commandList.Count == 0) return;
            if (index < commandList.Count)
            {
                commandList[index].Execute();
                index++;
            }
        }
        public void Undo()
        {
            if (commandList.Count == 0) return;
            if (index > 0)
            {
                commandList[index - 1].Undo();
                index--;
            }
        }
    }
}
