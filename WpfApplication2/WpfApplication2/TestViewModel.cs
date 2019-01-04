using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class TestViewModel : ValidatableBindableBase
    {

        private List<string> RecordErrorProperies = new List<string>();

        private string _showName = "ShowName";
        public  string ShowName
        {
            get { return _showName; }
            set { SetProperty<string>(ref _showName, value); }
        }

        private string _callName;
        public string CallName
        {
            get { return _callName; }
            set { SetProperty<string>(ref _callName, value); }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { SetProperty<string>(ref _type, value); }
        }

        private string _editType;
        public string EditType
        {
            get { return _editType; }
            set { SetProperty<string>(ref _editType, value); }
        }


        private bool _hasError;
        public bool HasError
        {
            get { return _hasError; }
            set { SetProperty<bool>(ref _hasError, value); }
        }

        
        public TestViewModel()
        {

        }


        protected override string OnProperty(string columnName)
        {
            double showNameValue;

            switch (columnName)
            {              
                case "ShowName" :      
                                                   
                    if (string.IsNullOrEmpty(_showName))
                    {
                        ShowName = "ShowName";
                        return "Show Name Colud not be Empty";
                    }
                        
                    if (Double.TryParse(_showName, out showNameValue))
                        return "Show Name Colud not be Nummic";
                    else
                        return string.Empty;

                case "EditType":


                    if (string.IsNullOrEmpty(_editType))
                        return "Edit Type Colud not be Empty";
                    if (Double.TryParse(_editType, out showNameValue))
                        return "Edit Type Colud not be Nummic";
                    else
                        return string.Empty;


                case "Type" :

                    if (string.IsNullOrEmpty(_type))
                        return "Type Colud not be Empty";
                    if (Double.TryParse(_type, out showNameValue))
                        return "Type Colud not be Nummic";
                    else
                        return string.Empty;


                case "CallName":

                    if (string.IsNullOrEmpty(_callName))
                        return "Call Name Colud not be Empty";
                    if (Double.TryParse(_callName, out showNameValue))
                        return "Call Name Colud not be Nummic";
                    else
                        return string.Empty;

                default:
                    throw new ArgumentOutOfRangeException("Unknow Property Name {0}", columnName);
            }
        }
    }
}
