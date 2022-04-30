using Microsoft.AspNetCore.Components;
using global::SkolkaPerinka.Shared.Models;
using AKSoftware.Localization.MultiLanguages.Blazor;
using System.Net.Http.Json;
using MudBlazor;

namespace SkolkaPerinka.Client.Pages
{
    public partial class SigningChildrensToSchool
    {
        [CascadingParameter] protected Task<AuthenticationState> authenticationState { get; set; }
        [Parameter] public string SelectedDate { get; set; }
        private ChildrenToSchool _childrenToSchool = new();
        private string parentEmail { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await authenticationState).User;
            parentEmail = user.FindFirst(c => c.Type == "sub")?.Value;
            language.InitLocalizedComponent(this);
        }
        protected async Task SignedChildren()
        {
            _childrenToSchool.CurrentDay = DateTime.Parse(SelectedDate);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync($"/api/calendar/addchildrenstoschool", _childrenToSchool);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Snackbar.Add("zapsání probìhlo úspìšnì", Severity.Success);
                navigationManager.NavigateTo("appsite");
            }
            else
            {
                StateHasChanged();
                Console.WriteLine(httpResponseMessage.Content);
            }
        }

        protected async Task SetChildrensToParentComponent(List<Children> allChildren)
        {
            _childrenToSchool.ChildrenOfParent = allChildren;
        }

        protected async Task SetChildrensCheckBoxToParentComponent(Children checkedChildren)
        {
            Children currentChildren = _childrenToSchool.ChildrenOfParent.FirstOrDefault(ch => ch.Id == checkedChildren.Id);
            currentChildren.Checked = checkedChildren.Checked; 
        }
    }
}