using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using InteractionEventTrigger = System.Windows.Interactivity.EventTrigger;

namespace WpfApplication2
{
    //mvvm framwork
    public class InvokeDelegateCommandAction : TriggerAction<DependencyObject>
    {
        #region DependencyProperty and BridgingProperty

        //可以 Binding InvokeEventArgs 用來傳遞 EventArgs
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeDelegateCommandAction));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeDelegateCommandAction));

        public static readonly DependencyProperty InvokeEventArgsProperty =
            DependencyProperty.Register("InvokeEventArgs", typeof(object), typeof(InvokeDelegateCommandAction));



        //原始的 CommandParameter Property
        public object CommandParameter
        {
            set { SetValue(CommandParameterProperty, value); }
            get { return GetValue(CommandParameterProperty); }
        }

        //原始的 Command Property
        public ICommand Command
        {
            set { SetValue(CommandProperty, value); }
            get { return (ICommand)GetValue(CommandProperty); }
        }

        //儲存 EventArgs
        public object InvokeEventArgs
        {
            set { SetValue(InvokeEventArgsProperty, value); }
            get { return GetValue(InvokeEventArgsProperty); }
        }

        #endregion

        //可以直接輸入 CommandName 藉由 ResolveCommand() 去 AssociatedObject.DataContext 中反射
        private string _commandName;
        public  string CommandName
        {
            get { return _commandName; }
            set
            {
                if (!value.Equals(_commandName))
                    this._commandName = value;
            }
        }


        protected override void Invoke(object parameter)
        {
            // EventArgs inject to InvokeEventArgs 
            this.InvokeEventArgs = parameter;

            if(this.AssociatedObject != null)
            {
                ICommand cmd = this.ResolveCommand();
                if ((cmd != null) && cmd.CanExecute(this.CommandParameter))
                    cmd.Execute(this.CommandParameter);
            }
        }



        private ICommand ResolveCommand()
        {
            ICommand command = null;


            if (this.Command != null)
                return this.Command;


            //當this.Command 為 null 時，試著從 AssociatedObject.DataContext 找
            var frameworkElement = this.AssociatedObject as FrameworkElement;
            if( frameworkElement != null )
            {
                object dataContext = frameworkElement.DataContext;
                if(dataContext != null)
                {
                    PropertyInfo commandProeprtyInfo = dataContext
                        .GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(
                            p =>
                            typeof(ICommand).IsAssignableFrom(p.PropertyType) &&
                            string.Equals(p.Name, this.CommandName, StringComparison.Ordinal)
                        );

                    if (commandProeprtyInfo != null)
                        command = (ICommand)commandProeprtyInfo.GetValue(dataContext, null);
                }
            }
            return command;
        }


        public static void SetTrigger(FrameworkElement contentControl, string evnetName, string commandName)
        {
            
            InvokeDelegateCommandAction action = new InvokeDelegateCommandAction();

            //設定 binding 到 action 物件中
            Binding binding = new Binding(commandName);
            BindingOperations.SetBinding(action, InvokeDelegateCommandAction.CommandProperty, binding);
            binding = new Binding("InvokeEventArgs") { RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self } };
            BindingOperations.SetBinding(action, InvokeDelegateCommandAction.CommandParameterProperty, binding);


            //設定Event Trigger
            InteractionEventTrigger eventTrigger = new InteractionEventTrigger() { EventName = evnetName };

            //將 action 加入 eventTrigger
            eventTrigger.Actions.Add(action);


            //取得 contentControl 的Event Triggers
            //並將 eventTrigger 設定到 Triggers 中
            var triggers = System.Windows.Interactivity.Interaction.GetTriggers(contentControl);
            triggers.Add(eventTrigger);
        }

    }
}
