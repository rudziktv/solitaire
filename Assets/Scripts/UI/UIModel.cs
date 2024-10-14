using System;
using Controllers;
using UnityEngine.UIElements;

namespace UI
{
    public class UIModel
    {
        protected GameManager Manager => GameManager.Instance;
        protected GameSounds Sounds => GameSounds.Instance;
        protected UIController Controller;
        public VisualElement View { get; }
        protected UIResources Assets => Controller.Resources;

        protected UIModel(UIController controller, VisualElement view)
        {
            Controller = controller;
            View = view;
        }
        
        public virtual void OnViewCreated() { }
        public virtual void OnViewDestroy() { }
        
        public virtual void OnUpdate() { }

        protected T CreateView<T>(VisualElement root) where T : UIModel
        {
            // return new UIModel(Controller, root) as T;
            var constructor = typeof(T).GetConstructor(new[] { typeof(UIController), typeof(VisualElement) });

            if (constructor == null)
                throw new InvalidOperationException($"Typ {typeof(T).Name} nie posiada konstruktora (Controller, VisualElement).");
            return (T)constructor.Invoke(new object[] { Controller, root });
        }

        // protected void AddView(VisualElement root)
        // {
        //     
        // }

        ~UIModel()
        {
            OnViewDestroy();
        }
    }
}