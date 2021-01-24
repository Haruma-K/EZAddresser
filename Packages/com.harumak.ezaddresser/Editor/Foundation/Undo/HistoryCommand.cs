using System;

namespace EZAddresser.Editor.Foundation.Undo
{
    internal class HistoryCommand
    {
        public HistoryCommand(Action redo, Action undo, int groupId)
        {
            if (redo == null)
            {
                throw new ArgumentNullException(nameof(redo));
            }

            if (undo == null)
            {
                throw new ArgumentNullException(nameof(undo));
            }

            Redo = redo;
            Undo = undo;
            GroupId = groupId;
        }

        public int GroupId { get; }

        private event Action Redo;
        private event Action Undo;

        public void ExecuteRedo()
        {
            Redo?.Invoke();
        }

        public void ExecuteUndo()
        {
            Undo?.Invoke();
        }
    }
}