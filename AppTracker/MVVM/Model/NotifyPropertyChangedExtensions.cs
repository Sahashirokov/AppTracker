using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LauncherApp.MVVM.Model;

public static class NotifyPropertyChangedExtensions
{
    public static IDisposable SubscribeToProperty<T>(
        this T source,
        Expression<Func<T, object>> propertySelector,
        Action handler)
        where T : INotifyPropertyChanged
    {
        var memberExpression = propertySelector.Body as MemberExpression;
        if (memberExpression == null)
            throw new ArgumentException("Invalid property selector");

        var propertyName = memberExpression.Member.Name;
        handler(); 
        var handlerWrapper = new PropertyChangedEventHandler((s, e) =>
        {
            if (e.PropertyName == propertyName)
            {
                handler();
            }
        });

        source.PropertyChanged += handlerWrapper;
        return new DisposableAction(() => source.PropertyChanged -= handlerWrapper);
    }

    private class DisposableAction : IDisposable
    {
        private readonly Action _action;
        public DisposableAction(Action action) => _action = action;
        public void Dispose() => _action?.Invoke();
    }
}