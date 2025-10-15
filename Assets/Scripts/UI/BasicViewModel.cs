using System;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace UI
{
    public class BasicViewModel : UIModel, INotifyBindablePropertyChanged
    {
        protected BasicViewModel(UIController controller, VisualElement view) : base(controller, view)
        {
            
        }

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;
        
        protected void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
    }
}