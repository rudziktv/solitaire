using System;
using UnityEngine.UIElements;

namespace UI
{
    public class BasicViewModel : UIModel, INotifyBindablePropertyChanged
    {
        protected BasicViewModel(UIController controller, VisualElement view) : base(controller, view)
        {
            
        }

        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;
    }
}