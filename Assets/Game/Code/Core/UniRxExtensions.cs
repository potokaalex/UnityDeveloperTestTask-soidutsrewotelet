using System;
using UniRx;
using Unity.Netcode;

namespace Game.Code.Core
{
    public static class UniRxExtensions
    {
        public static IObservable<Unit> OnChangeAsObservable<T>(this NetworkVariable<T> networkVariable)
        {
            return Observable.FromEvent<NetworkVariable<T>.OnValueChangedDelegate, Unit>(
                handler => (_, _) => handler(Unit.Default),
                h => networkVariable.OnValueChanged += h,
                h => networkVariable.OnValueChanged -= h
            );
        }
    }
}