using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Blazor;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SkolkaPerinka.Client.Providers;

namespace SkolkaPerinka.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] ILanguageContainerService language { get; set; }
        [Inject] ILocalStorageService localStorage { get; set; }
        [Inject] AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
        private string userName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthenticationState).User;
            if (user.Identity.IsAuthenticated == true)
            {
                userName = user.Identity.Name;
            }

            language.InitLocalizedComponent(this);
        }

        private async Task SignOut()
        {
            if (await localStorage.ContainKeyAsync("bearerToken"))
            {
                await localStorage.RemoveItemAsync("bearerToken");
                ((AppAuthenticationStateProvider)authenticationStateProvider).SignOut();
            }

            userName = "";
            StateHasChanged();
            navigationManager.NavigateTo("signin");
        }
    }
}