using Calculator.WpfApp.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Calculator.WpfApp.Commands.RelayCommand;

namespace Calculator.WpfApp.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _screenVal;
        private List<string> _availableOperations = new List<string> {"+","-","/","*"};
        private DataTable _dataTable = new DataTable();
        private bool _isLastSignOperation = false;
        private bool _isCommaPressed = false;
        public string ScreenVal 
        { 
            get { return _screenVal; } 
            set 
            { 
                _screenVal = value;
                AddNumberCommand = new RelayCommand(AddNumber);
                AddOperationCommand = new RelayCommand(AddOperation);
                ClearScreenCommand = new RelayCommand(ClearScreen);
                GetResultCommand = new RelayCommand(GetResult);
                AddCommaCommand = new RelayCommand(AddComma, CanAddComma);
                OnPropertyChanged();
            } 
        }

        private bool CanAddComma(object obj) => !_isCommaPressed;

        private void AddComma(object obj)
        {
            var com = obj as string;
            if (!_isCommaPressed)
            {
                if (_availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
                {
                    com = "0,";
                }
                ScreenVal += com;
                _isCommaPressed = true;
            }
            
        }

        private void GetResult(object obj)
        {
            if (_isLastSignOperation)
            {
                ScreenVal = ScreenVal.Remove(ScreenVal.Length - 1, 1);
            }
            var result = Math.Round(Convert.ToDouble(_dataTable.Compute(ScreenVal.Replace(",","."), "")), 5);

            ScreenVal = result.ToString();
            _isLastSignOperation = false;
            if(result % 1 == 0)
            {
                _isCommaPressed = false;
            }
            else
            {
                _isCommaPressed = true;
            }
            
        }

        private void ClearScreen(object obj)
        {
            ScreenVal = "0";
            _isLastSignOperation = false;
            _isCommaPressed = false;
        }

        private void AddOperation(object obj)
        {
            var operation = obj as string;

            if (_isLastSignOperation)
            {
                ScreenVal = ScreenVal.Remove(ScreenVal.Length - 1, 1) + operation;
            }
            else
            {
                ScreenVal += operation;
                _isLastSignOperation = true;
            }

            _isCommaPressed = false;
        }

        private void AddNumber(object obj)
        {
            //MessageBox.Show(obj as string);
            var number = obj as string;

            if (ScreenVal == "0")
            {
                ScreenVal = String.Empty;
            }

            ScreenVal += number;
            _isLastSignOperation = false;

        }
        public ICommand AddCommaCommand { get; set; }
        public ICommand GetResultCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }
        public ICommand AddNumberCommand { get; set; }

        public MainViewModel()
        {
            ScreenVal = 0.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
