using UnityEngine.UIElements;

namespace UI.ViewModel
{
    public class ModalViewModel : BasicViewModel
    {
        protected ModalViewModel(UIController controller, VisualElement view) : base(controller, view)
        {
            
        }

        public virtual void SetVisibility(bool visible)
        {
            if (visible)
                View.RemoveFromClassList("hide");
            else
                View.AddToClassList("hide");
        }
    }
}