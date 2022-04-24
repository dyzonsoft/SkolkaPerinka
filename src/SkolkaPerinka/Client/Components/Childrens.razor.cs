using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using global::SkolkaPerinka.Shared.Models;
using AKSoftware.Localization.MultiLanguages.Blazor;

namespace SkolkaPerinka.Client.Components
{
    public partial class Childrens
    {
        [Inject] HttpClient httpClient { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }

        private List<Children> childrens = new List<Children>();
        [CascadingParameter]  protected Task<AuthenticationState> authenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await authenticationState).User;
            string parentEmail = user.FindFirst(c => c.Type == "sub")?.Value;
            childrens = await httpClient.GetFromJsonAsync<List<Children>>($"/api/childrens/getchildrensforparent/{parentEmail}");

            language.InitLocalizedComponent(this);
            StateHasChanged();
        }

        private async Task CreateChildren()
        {
            navigationManager.NavigateTo("createchildren");
        }
    }
}