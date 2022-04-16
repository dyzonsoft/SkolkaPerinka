using AKSoftware.Localization.MultiLanguages.Blazor;

namespace SkolkaPerinka.Client.Pages
{
    public partial class Index
    {
        protected async override Task OnInitializedAsync()
        {
            language.InitLocalizedComponent(this);
            StateHasChanged();
        }
    }
}