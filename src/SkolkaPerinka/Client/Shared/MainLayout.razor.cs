using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Blazor;
using Microsoft.AspNetCore.Components;

namespace SkolkaPerinka.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] ILanguageContainerService language { get; set; }

        protected override void OnInitialized()
        {
            language.InitLocalizedComponent(this);
        }

        //private async Task SignOut()
        //{
        //    if (await LocalStorageService.ContainKeyAsync("bearerToken"))
        //    {
        //        await LocalStorageService.RemoveItemAsync("bearerToken");
        //        ((AppAuthenticationStateProvider)AuthenticationStateProvider).SignOut();
        //    }

        //    StateHasChanged();

        //    var jenda = await HttpClient.GetAsync(APIEndpoints.s_getalldays);



        //    NavigationManager.NavigateTo("/");
        //}
    }
}