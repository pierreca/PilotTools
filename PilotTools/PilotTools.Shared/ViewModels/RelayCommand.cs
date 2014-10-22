using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PilotTools.ViewModels
{
    public class RelayCommand : ICommand
    {
        private Func<object, bool> canExecute = delegate { return true; };
        private Action<object> execute = delegate { };

        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            this.execute.Invoke(parameter);
        }
    }
}
