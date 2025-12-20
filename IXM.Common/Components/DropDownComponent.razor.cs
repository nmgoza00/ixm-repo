using IXM.Models;
using IXM.Web.Components.DataProviders;
using Microsoft.AspNetCore.Components;

namespace IXM.Common.Components
{
    public partial class DropDownComponent
    {
        [Parameter]
        public string ID { get; set; }

        [Parameter]
        public List<IXMDDType> DDList { get; set; }

        [Parameter]
        public EventCallback<IXMDDType> OnSelectionChanged { get; set; }


        private IXMDDType DDListSelect = new();



        void SelectionChanged(IXMDDType newValue)
        {
            OnSelectionChanged.InvokeAsync(newValue);

        }



    }
}