using Microsoft.AspNetCore.Components;
using AKSoftware.Localization.MultiLanguages.Blazor;
using SkolkaPerinka.Client.Components;

namespace SkolkaPerinka.Client.Pages
{
    public partial class AppSite
    {
        [CascadingParameter] protected Task<AuthenticationState> authenticationState { get; set; }
        private string parentEmail { get; set; }

        Calendar calendar;

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

        private async Task Administration()
        {
            navigationManager.NavigateTo("administration");
        }
        
        private async Task ChildrenWasDeleted(string message)
        {
            await calendar.RefreshCalendar();
            StateHasChanged();
        }
    }
}