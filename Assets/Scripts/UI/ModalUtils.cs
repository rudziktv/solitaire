using UnityEngine.UIElements;

namespace UI
{
    public static class ModalUtils
    {
        public static VisualElement CreateModalLayer(this UIModel viewModel)
        {
            var modalLayer = CreateModalLayer();
            viewModel.View.Add(modalLayer);
            return modalLayer;
        }
        
        public static VisualElement CreateModalLayer()
        {
            var modalLayer = new VisualElement
            {
                name = "modal-layer"
            };
            
            modalLayer.AddToClassList("modal-layer");
            modalLayer.AddToClassList("modal-backdrop");
            modalLayer.AddToClassList("hide");
            modalLayer.AddToClassList("hiddable-element");
            
            return modalLayer;
        }
    }
}