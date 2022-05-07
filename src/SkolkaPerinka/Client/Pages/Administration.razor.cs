using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using global::SkolkaPerinka.Shared.Models;
using AKSoftware.Localization.MultiLanguages.Blazor;
using MudBlazor;
using SkolkaPerinka.Client.Components;
using Microsoft.JSInterop;

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
            if (children != null)
            {
                User user = await httpClient.GetFromJsonAsync<User>($"/api/user/getusersbyemail/{children.ParentEmail}");
                var parameters = new DialogParameters();
                parameters.Add("_childrenName", children.FirstName + " " + children.LastName);
                parameters.Add("_userName", user.FirstName + " " + user.LastName);
                parameters.Add("_parentText", language["Parent"]);
                parameters.Add("_warning", language["ChildrenDeleteWarning"]);
                parameters.Add("_delete", "children");

                var options = new DialogOptions() { CloseButton = true };

                var dialog = DialogService.Show<MudDialogDeleteChildren>("Delete children", parameters);
                var result = await dialog.Result;

                if (!result.Cancelled)
                {
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
            }
            else
            {
                snackbar.Add(language["SamthingWrong"], Severity.Error);
            }
        }

        private async Task DeleteUser(string userId)
        {
            User user = _allUsers.FirstOrDefault(c => c.Id == userId);
            List<Children> childrens = await httpClient.GetFromJsonAsync<List<Children>>($"/api/childrens/getparentchildrens/{user.Email}");
            var parameters = new DialogParameters();
            parameters.Add("_userName", user.FirstName + " " + user.LastName);
            parameters.Add("_childrens", childrens);
            parameters.Add("_warning", language["UserDeleteWarning"]);
            parameters.Add("_delete", "user");

            var options = new DialogOptions() { CloseButton = true };

            var dialog = DialogService.Show<MudDialogDeleteUser>("Delete user", parameters);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {

                HttpResponseMessage httpResponseMessageFormChildren = await httpClient.PostAsJsonAsync($"/api/childrens/deleteallchildofparent", user);
                HttpResponseMessage httpResponseMessageFromUser = await httpClient.PostAsJsonAsync($"/api/user/delete", user);
                if (httpResponseMessageFormChildren.IsSuccessStatusCode && httpResponseMessageFromUser.IsSuccessStatusCode)
                {
                    _childrens = await httpClient.GetFromJsonAsync<List<Children>>($"/api/childrens/getallchildrens");
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

        private void GoBack()
        {
            _jsRuntime.InvokeVoidAsync("history.go", -1);
        }
    }
}