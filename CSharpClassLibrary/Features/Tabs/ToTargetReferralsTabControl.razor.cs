using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace DM2BD.Europa.UIComponents.Tabs
{
    public partial class ToTargetReferralsTabControl : ComponentBase
    {
        public ITab TabPage { get; set; }
        private IList<ITab> TabPages = new List<ITab>();
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        public void AddTabPage(ITab tab)
        {
            TabPages.Add(tab);
        }
        private void ActivateTabPage(ITab tab)
        {
            TabPage = tab;
        }
    }
}