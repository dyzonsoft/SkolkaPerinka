using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using global::SkolkaPerinka.Shared.Models;
using AKSoftware.Localization.MultiLanguages.Blazor;

namespace SkolkaPerinka.Client.Components
{
    public partial class Childrens
    {
        [Parameter] public bool _withCheckBox { get; set; }
        [Parameter] public bool _withDelete { get; set; }
        [Parameter] public string _selectedDate { get; set; }
        [Parameter] public string _parentEmail { get; set; }
        [Parameter] public EventCallback<List<Children>> OnSetChildrensToParentComponent { get; set; }
        [Parameter] public EventCallback<Children> OnSetChildrensCheckBoxToParentComponent { get; set; }
        [Parameter] public EventCallback<string> OnDeleteChildrenToParentComponent { get; set; }

        private List<Children> childrens = new List<Children>();

        protected override async Task OnInitializedAsync()
        {
            childrens = await httpClient.GetFromJsonAsync<List<Children>>($"/api/childrens/getchildrensforparent/{_parentEmail}/{_selectedDate}");
            await OnSetChildrensToParentComponent.InvokeAsync(childrens);

            language.InitLocalizedComponent(this);
        }

        private async Task ChangeCheckBoxAsync(ChangeEventArgs e, int childrenId)
        {
            Children children = childrens.FirstOrDefault(ch => ch.Id == childrenId);
            children.Checked = (bool)e.Value;
            await OnSetChildrensCheckBoxToParentComponent.InvokeAsync(children);
        }

        private async Task DeleteChildren(int childrenId)
        {
            Children children = childrens.FirstOrDefault(c => c.Id == childrenId);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync($"/api/childrens/deletechild", children);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                childrens = await httpClient.GetFromJsonAsync<List<Children>>($"/api/childrens/getchildrensforparent/{_parentEmail}/{_selectedDate}");
                OnDeleteChildrenToParentComponent.InvokeAsync("");
                //StateHasChanged();
            }
            else
            {
                Console.WriteLine(httpResponseMessage.Content);
            }
        }
    }
}