using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EquipApps.Hardware.ValueDecorators
{
    /// <summary>
    /// Декоратор. 
    /// С поддержкой <see cref="IObservable{}"/>.
    /// </summary>   
    public class ValueDecoratorObservable<TValue> : IValueComonent<TValue>, IDisposable
    {
        private ReplaySubject<TValue> _valueComonentSubject = new ReplaySubject<TValue>();
        private IValueComonent<TValue> _valueComonent;

        public ValueDecoratorObservable(IValueComonent<TValue> valueComonent)
        {
            _valueComonent = valueComonent ?? throw new ArgumentNullException(nameof(valueComonent));
        }


        public TValue Value
        {
            get => _valueComonent.Value;
            set
            {
                _valueComonent.Value = value;
                _valueComonentSubject.OnNext(value);
            }
        }

        public IObservable<TValue> Observable => _valueComonentSubject.AsObservable();

        public void Dispose()
        {
            _valueComonentSubject.Dispose();
            _valueComonentSubject = null;
            _valueComonent = null;
        }
    }
}
