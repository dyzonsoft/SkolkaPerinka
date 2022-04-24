using System.Data;
using System.Net.Http.Json;
using AKSoftware.Localization.MultiLanguages.Blazor;
using global::SkolkaPerinka.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace SkolkaPerinka.Client.Pages
{
    public partial class CreateChildren
    {
        [CascadingParameter] protected Task<AuthenticationState> authenticationState { get; set; }
        [Inject] HttpClient HttpClient { get; set; }
        private ChildrenToRegister _childrenToRegister = new();
        public List<string> serverErrorMessage { get; set; } = new();
        private bool success = false;


        protected override async Task OnInitializedAsync()
        {
            language.InitLocalizedComponent(this);
            StateHasChanged();
        }

        private async Task RegisterChildren()
        {
            success = false;
            var user = (await authenticationState).User;
            string parentEmail = user.FindFirst(c => c.Type == "sub")?.Value;
            _childrenToRegister.ParentEmail = parentEmail;

            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsJsonAsync($"/api/childrens/register", _childrenToRegister);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                navigationManager.NavigateTo("appsite");
            }
            else
            {
                serverErrorMessage = await httpResponseMessage.Content.ReadFromJsonAsync<List<string>>();
                success = false;
            }
        }
    }
}