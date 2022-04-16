using Microsoft.AspNetCore.Components;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using System.Globalization;

namespace SkolkaPerinka.Client.Components
{
    public partial class LanguageSwitcher
    {
        [Inject] ILanguageContainerService language { get; set; }
        [Inject] ILocalStorageService localStorage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (await localStorage.ContainKeyAsync("language"))
            {
                string _cultureCode = await localStorage.GetItemAsStringAsync("language");
                language.SetLanguage(CultureInfo.GetCultureInfo(_cultureCode));
            }
        }

        private async Task ChangeLanguageAsync(string cultureCode)
        {
            language.SetLanguage(CultureInfo.GetCultureInfo(cultureCode));
            await localStorage.SetItemAsStringAsync("language", cultureCode);
        }
    }
}