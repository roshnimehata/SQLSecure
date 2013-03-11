using Infragistics.Win;

namespace Idera.SQLsecure.UI.Console.Utility
{
    internal sealed class HideFocusRectangleDrawFilter : IUIElementDrawFilter
    {
        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            return true;
        }

        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            return DrawPhase.BeforeDrawFocus;
        }
    }
}