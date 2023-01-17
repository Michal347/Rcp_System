using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
     
        #region Error handling
        public readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors
        {
            get
            {
                foreach (List<string> item in errors.Values)
                {
                    if (item.Count > 0) return true;
                }
                return false;
            }
        }

        public void OnErrorsChanged(object sender, string property)
        {
            if (ErrorsChanged != null)
                ErrorsChanged.Invoke(sender, new DataErrorsChangedEventArgs(property));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName))
            {
                List<string> propertyError = new List<string>();
                errors.TryGetValue(propertyName, out propertyError);
                return propertyError;

            }
            return null;
        }

        public void OnErrorCreated(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors.Add(propertyName, new List<string>());

            errors[propertyName].Add(error);
            if (ErrorsChanged != null)
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearPropertyErrors(object sender, string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors[propertyName].Clear();
                OnErrorsChanged(sender, propertyName);
            }
        }



        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
