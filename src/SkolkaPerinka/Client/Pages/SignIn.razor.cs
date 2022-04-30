using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using global::SkolkaPerinka.Shared.Models;
using Blazored.LocalStorage;
using SkolkaPerinka.Client.Providers;
using AKSoftware.Localization.MultiLanguages.Blazor;
using static MudBlazor.CategoryTypes;
using MudBlazor;

namespace SkolkaPerinka.Client.Pages
{
    public partial class SignIn
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] ILocalStorageService LocalStorageService { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private UserToSigniIn _userToSignIn = new()
        {Email = "parent@example.com", Password = "Password1!"};
        private bool success = false;
        protected override async Task OnInitializedAsync()
        {
            language.InitLocalizedComponent(this);
            StateHasChanged();
        }

        private async Task signInUser()
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsJsonAsync("/api/user/signin", _userToSignIn);
            var errorMessage = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonWebToken = await httpResponseMessage.Content.ReadAsStringAsync();
                await LocalStorageService.SetItemAsync("bearerToken", jsonWebToken);
                await ((AppAuthenticationStateProvider)AuthenticationStateProvider).SignIn();
                success = true;
                HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", jsonWebToken);
                navigationManager.NavigateTo("appsite");
            }
            else
            {
                if (errorMessage == "NotAccess") Snackbar.Add(language["AccessFalse"], Severity.Error);
                    else Snackbar.Add(language["UserFalse"], Severity.Error);
            }
        }

        private async Task RegisterUser()
        {
            navigationManager.NavigateTo("register");
        }
    }
}