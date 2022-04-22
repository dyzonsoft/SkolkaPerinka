using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using global::SkolkaPerinka.Shared.Models;
using AKSoftware.Localization.MultiLanguages.Blazor;

namespace SkolkaPerinka.Client.Pages
{
    public partial class Register
    {
        [Inject] HttpClient HttpClient { get; set; }

        private UserToRegister _userToRegister = new()
        {FirstName = "Dušan", LastName = "Ulièný", Address = "Støední 55", Phone = "123123123", Email = "dusan@nafukovacireklama.cz"};
        private bool success = false;
        public bool _IAgree { get; set; } = false;
        public string _role { get; set; }

        public List<string> serverErrorMessage { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            language.InitLocalizedComponent(this);
            StateHasChanged();
        }

        private async Task RegisterUser()
        {
            success = false;
            if (_userToRegister.Password != _userToRegister.ConfirmPassword)
            {
                serverErrorMessage.Add("confirm password does not match with password.");
            }
            else
            {
                HttpResponseMessage httpResponseMessage = await HttpClient.PostAsJsonAsync($"/api/user/register/{_role}", _userToRegister);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    navigationManager.NavigateTo("/");
                }
                else
                {
                    serverErrorMessage = await httpResponseMessage.Content.ReadFromJsonAsync<List<string>>();
                    success = false;
                }
            }
        }
    }
}