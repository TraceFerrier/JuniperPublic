using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace JuniperMarket.ViewModels.Common
{
    public abstract class BasePropertyChanged : INotifyPropertyChanged
    {
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected bool SetProperty<T>(ref T varToSet, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(varToSet, newValue))
                return false;

            varToSet = newValue;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            string propName = GetExpressionName(expression);
            NotifyPropertyChanged(propName);
        }

        protected static string GetExpressionName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("The body must be a member expression");
            }

            return body.Member.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
