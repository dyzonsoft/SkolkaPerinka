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
        [Inject] HttpClient httpClient { get; set; }

        protected override void OnInitialized()
        {
            language.InitLocalizedComponent(this);
        }

        private async Task SignOut()
        {
            if (await localStorage.ContainKeyAsync("bearerToken"))
            {
                await localStorage.RemoveItemAsync("bearerToken");
                ((AppAuthenticationStateProvider)authenticationStateProvider).SignOut();
            }

            //var jenda = await HttpClient.GetAsync(APIEndpoints.s_getalldays);

            StateHasChanged();
            navigationManager.NavigateTo("/");
        }
    }
}