using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class ValidatableBindableBase : BindableBase , IDataErrorInfo
    {
        public string Error { get;  set; }

        public string this[string columnName]
        {
            get
            {
                return OnProperty(columnName);
            }
        }

        protected virtual string OnProperty(string columnName)
        {
            return string.Empty;
        }
    }
}
