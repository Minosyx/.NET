using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kolory_MAUI.ViewModel
{
    public class ResetCommand : ICommand
    {
        private ColorsVM vmodel;
        private bool previousCanExecuteValue;

        public ResetCommand(ColorsVM vmodel)
        {
            this.vmodel = vmodel;
            previousCanExecuteValue = CanExecute(null);
            this.vmodel.PropertyChanged += Vmodel_PropertyChanged;
        }

        private void Vmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            bool canExecuteValue = CanExecute(null);
            if (canExecuteValue != previousCanExecuteValue && CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
            previousCanExecuteValue = canExecuteValue;
        }

        public bool CanExecute(object? parameter)
        {
            return vmodel.R != 0 || vmodel.G != 0 || vmodel.B != 0;
        }

        public void Execute(object? parameter)
        {
            if (vmodel != null)
            {
                vmodel.R = 0;
                vmodel.G = 0;
                vmodel.B = 0;
            }
        }

        public event EventHandler? CanExecuteChanged;
    }
}
