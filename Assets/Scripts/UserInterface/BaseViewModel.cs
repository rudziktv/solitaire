using System;
using UnityEngine.UIElements;

namespace UserInterface
{
    public class BaseViewModel : INotifyBindablePropertyChanged
    {
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;
    }
}