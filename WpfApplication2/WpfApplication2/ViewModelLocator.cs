using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class ViewModelLocator : ValidatableBindableBase
    {

        #region Inject DataContext

        public static readonly DependencyProperty MappingDataContextProperty =
            DependencyProperty.RegisterAttached("MappingDataContext", typeof(bool) , typeof(ViewModelLocator), new PropertyMetadata(false, MappingDataContextPropertyChanged));
        
        public static void MappingDataContextPropertyChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
            {
                FrameworkElement fe = d as FrameworkElement;                
                fe.DataContext = new ViewModelLocator();
            }
        }

        public static void SetMappingDataContext(UIElement element, bool value)
        {
            element.SetValue(MappingDataContextProperty, value);
        }

        public static bool GetMappingDataContext(UIElement element, bool value)
        {
            return (bool)element.GetValue(MappingDataContextProperty);
        }

        #endregion

        #region SubViewModel



        private static BindableBase _window1ViewModel = new TestViewModel();
        public  static BindableBase Window1ViewModel
        {
            get { return _window1ViewModel; }
            set
            {
                _window1ViewModel = value;
                Window1ViewModelChanged(null, EventArgs.Empty);
            }
        }


        private  BindableBase _testViewModel2 = new TestViewModel();
        public  BindableBase TestViewModel2
        {
            set
            {
                SetProperty<BindableBase>(ref _testViewModel2, value);
            }

            get
            {
                if (_testViewModel2 == null)
                    throw new ArgumentNullException("_testViewModel");

                return _testViewModel2;

            }
        }


        private BindableBase _testViewModel;
        public  BindableBase TestViewModel
        {
            set { SetProperty<BindableBase>(ref _testViewModel, value); }

            get
            {
                if (_testViewModel == null)
                    throw new ArgumentNullException("_testViewModel");

                return _testViewModel;
            }
        }

        #endregion

        #region .ctor

        //管理所有 ViewModel 的父親
        public ViewModelLocator()
        {
            //inject
            _testViewModel = new TestViewModel();
            _clickCommand = new RelayCommand(click);
            _loadedCommand = new RelayCommand(loaded);
        }

        #endregion

        #region Command

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            set { SetProperty<ICommand>(ref _clickCommand, value); }
            get { return _clickCommand; }
        }

        private void click(object args)
        {
            //置換
            TestViewModel2 = new TestViewModel() { CallName = "CallName", ShowName = "ShowName", Type = "Type", EditType = "EditType" };

        }


        private ICommand _loadedCommand;
        public  ICommand LoadedCommand
        {
            get { return _loadedCommand; }
            set { SetProperty<ICommand>(ref _loadedCommand, value); }            
        }

        private void loaded(object args)
        {
            Window1 wn = new Window1();            
            ViewModelLocator.Window1ViewModel = new TestViewModel() { CallName = "CallName", ShowName = "ShowName", Type = "Type", EditType = "EditType" };
            wn.Show();
        }

        #endregion

        #region  Static PropertyChanged Event 

        //named rule : PropertyName + Changed

        public static event EventHandler Window1ViewModelChanged = delegate { };
        
        #endregion




    }
}
