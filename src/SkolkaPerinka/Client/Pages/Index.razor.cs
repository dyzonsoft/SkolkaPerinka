using AKSoftware.Localization.MultiLanguages.Blazor;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using SkolkaPerinka.Client.Providers;
using System.Net.Http.Headers;

namespace SkolkaPerinka.Client.Pages
{
    public partial class Index
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] ILocalStorageService LocalStorageService { get; set; }
        [CascadingParameter] protected Task<AuthenticationState> authenticationState { get; set; }
        private bool _access { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (await LocalStorageService.ContainKeyAsync("bearerToken"))
            {
                string savedToken = await LocalStorageService.GetItemAsync<string>("bearerToken");
                await ((AppAuthenticationStateProvider)AuthenticationStateProvider).SignIn();
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

                var user = (await authenticationState).User;
                var access = user.Identity.Name;
                StateHasChanged();
            }

            language.InitLocalizedComponent(this);
            StateHasChanged();
        }
    }
}