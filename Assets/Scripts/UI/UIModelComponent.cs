using UnityEngine.UIElements;

namespace UI
{
    public class UIModelComponent : UIModel
    {
        public UIModelComponent(UIController controller, VisualElement view) : base(controller, view)
        {
            OnViewCreated();
        }

        ~UIModelComponent()
        {
            OnViewDestroy();
        }
    }
}