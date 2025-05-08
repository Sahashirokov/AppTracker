using System;
using System.Collections.Generic;
using System.Linq;

namespace LauncherApp.Messanger;

public interface IMessenger
{
    void Register<TMessage>(object recipient, Action<TMessage> action);
    void Unregister<TMessage>(object recipient);
    void Send<TMessage>(TMessage message);
}
public abstract class MessageBase
{
    public object Sender { get; protected set; }
}

// 3. Конкретная реализация мессенджера
public class Messenger : IMessenger
{
    private readonly Dictionary<Type, List<WeakAction>> _recipients = new();
    private readonly object _lock = new();

    public void Register<TMessage>(object recipient, Action<TMessage> action)
    {
        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_recipients.ContainsKey(messageType))
                _recipients[messageType] = new List<WeakAction>();

            var weakAction = new WeakAction(recipient, action);
            _recipients[messageType].Add(weakAction);
        }
    }

    public void Unregister<TMessage>(object recipient)
    {
        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_recipients.ContainsKey(messageType)) return;

            _recipients[messageType].RemoveAll(wa => 
                wa.Target != null && wa.Target.Equals(recipient));
        }
    }

    public void Send<TMessage>(TMessage message)
    {
        List<WeakAction> actions;
        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_recipients.TryGetValue(messageType, out var list)) return;

            actions = list.ToList();
        }

        foreach (var action in actions)
        {
            if (action.IsAlive)
                action.Execute(message);
        }
    }

    private class WeakAction
    {
        private readonly WeakReference _target;
        private readonly Delegate _action;

        public object Target => _target.Target;

        public bool IsAlive => _target.IsAlive;

        public WeakAction(object target, Delegate action)
        {
            _target = new WeakReference(target);
            _action = action;
        }

        public void Execute(object parameter)
        {
            if (_action is Action<object> actionObj)
                actionObj(parameter);
            else
                _action.DynamicInvoke(parameter);
        }
    }
}