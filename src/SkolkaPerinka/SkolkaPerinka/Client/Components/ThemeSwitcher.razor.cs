using Microsoft.AspNetCore.Components;
using MudBlazor;
using Blazored.LocalStorage;

namespace SkolkaPerinka.Client.Components
{
    public partial class ThemeSwitcher
    {
        [Inject]
        ILocalStorageService localStorage { get; set; }

        private bool _isDarkMode;
        protected async override Task OnInitializedAsync()
        {
            if (await localStorage.ContainKeyAsync("theme"))
            {
                string _theme = await localStorage.GetItemAsStringAsync("theme");
                if (_theme == "dark")
                    _isDarkMode = true;
                else
                    _isDarkMode = false;
            }
        }

        private MudTheme _theme = new();
        private async Task ChangeThemeAsync()
        {
            string themeName = _isDarkMode ? "dark" : "light";
            await localStorage.SetItemAsStringAsync("theme", themeName);
        }
    }
}