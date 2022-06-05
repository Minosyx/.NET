using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kolory_MAUI.ViewModel
{
    using Model;
    public class ColorsVM : INotifyPropertyChanged
    {
        private readonly Colors model = Settings.Load();

        public event PropertyChangedEventHandler? PropertyChanged;

        public double R
        {
            get => model.R;
            set
            {
                model.R = value;
                onPropertyChanged(nameof(R));
            }
        }

        public double G
        {
            get => model.G;
            set
            {
                model.G = value;
                onPropertyChanged(nameof(G));
            }
        }

        public double B
        {
            get => model.B;
            set
            {
                model.B = value;
                onPropertyChanged(nameof(B));
            }
        }

        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private ICommand resetCommand;

        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                    resetCommand = new ResetCommand(this);
                return resetCommand;
            }
        }
    }
}
