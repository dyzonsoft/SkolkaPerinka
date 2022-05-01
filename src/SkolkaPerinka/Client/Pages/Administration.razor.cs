using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using global::SkolkaPerinka.Shared.Models;
using AKSoftware.Localization.MultiLanguages.Blazor;
using MudBlazor;

namespace SkolkaPerinka.Client.Pages
{
    public partial class Administration
    {
        [Inject]
        HttpClient httpClient { get; set; }

        private List<Children> _childrens = new();
        private List<User> _teachers = new();
        private List<User> _parents = new();
        private List<User> _allUsers = new();
        protected override async Task OnInitializedAsync()
        {
            _childrens = await httpClient.GetFromJsonAsync<List<Children>>($"/api/childrens/getallchildrens");
            _allUsers = await httpClient.GetFromJsonAsync<List<User>>($"/api/user/getallusers");
            _teachers = _allUsers.Where((u) => u.Role == "Teacher").ToList();
            _parents = _allUsers.Where((u) => u.Role == "Parent").ToList();
            
            language.InitLocalizedComponent(this);
        }

        private async Task DeleteChildren(int childrenId)
        {
            Children children = _childrens.FirstOrDefault(c => c.Id == childrenId);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync($"/api/childrens/deletechild", children);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                _childrens = await httpClient.GetFromJsonAsync<List<Children>>($"/api/childrens/getallchildrens");
                snackbar.Add(language["Hotovo"], Severity.Success);
                StateHasChanged();
            }
            else
            {
                snackbar.Add(language["SamthingWrong"], Severity.Error);
            }
        }

        private async Task DeleteUser(string userId)
        {
            User user = _allUsers.FirstOrDefault(c => c.Id == userId);
            HttpResponseMessage httpResponseMessageFormChildren = await httpClient.PostAsJsonAsync($"/api/childrens/deleteallchildofparent", user);
            HttpResponseMessage httpResponseMessageFromUser = await httpClient.PostAsJsonAsync($"/api/user/delete", user);
            if (httpResponseMessageFormChildren.IsSuccessStatusCode && httpResponseMessageFromUser.IsSuccessStatusCode)
            {
                _allUsers = await httpClient.GetFromJsonAsync<List<User>>($"/api/user/getallusers");
                _teachers = _allUsers.Where((u) => u.Role == "Teacher").ToList();
                _parents = _allUsers.Where((u) => u.Role == "Parent").ToList();
                snackbar.Add(language["Hotovo"], Severity.Success);
                StateHasChanged();
            }
            else
            {
                snackbar.Add(language["SamthingWrong"], Severity.Error);
            }
        }

        private async Task ChangeAccessUser(string userId)
        {
            User user = _allUsers.FirstOrDefault(c => c.Id == userId);
            if (user.Access) user.Access = false;
            else user.Access = true;

            HttpResponseMessage httpResponseMessage = await httpClient.PutAsJsonAsync($"/api/user/updateaccess/{userId}", user);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                _allUsers = await httpClient.GetFromJsonAsync<List<User>>($"/api/user/getallusers");
                _teachers = _allUsers.Where((u) => u.Role == "Teacher").ToList();
                _parents = _allUsers.Where((u) => u.Role == "Parent").ToList();
                StateHasChanged();
            }
            else
            {
                Console.WriteLine(httpResponseMessage.Content);
            }
        }
    }
}