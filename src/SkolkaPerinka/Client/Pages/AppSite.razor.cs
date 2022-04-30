using Microsoft.AspNetCore.Components;
using AKSoftware.Localization.MultiLanguages.Blazor;

namespace SkolkaPerinka.Client.Pages
{
    public partial class AppSite
    {
        [CascadingParameter] protected Task<AuthenticationState> authenticationState { get; set; }
        private string parentEmail { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await authenticationState).User;
            parentEmail = user.FindFirst(c => c.Type == "sub")?.Value;
            language.InitLocalizedComponent(this);
        }

        private async Task CreateChildren()
        {
            navigationManager.NavigateTo("createchildren");
        }
    }
}