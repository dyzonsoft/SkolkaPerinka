using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using global::SkolkaPerinka.Shared.Models;
using AKSoftware.Localization.MultiLanguages.Blazor;
using Microsoft.AspNetCore.Components.Authorization;

namespace SkolkaPerinka.Client.Components
{
    public partial class Calendar
    {
        [Inject]
        HttpClient HttpClient { get; set; }
        private Day[]? Days { get; set; } = new Day[0];
        private string? MonthString { get; set; }
        private string? YearString { get; set; }
        private DateTime CurrentMonthonth { get; set; }
        [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
        bool _isAuthenticated;
        private string _parentEmail;

        protected override async Task OnInitializedAsync()
        {
            CurrentMonthonth = DateTime.Today;
            var user = (await AuthenticationState).User;
            if (!user.Identity.IsAuthenticated) 
            {
                navigationManager.NavigateTo("signin");
            } else
            {
                _parentEmail = user.FindFirst(c => c.Type == "sub")?.Value;
                MonthString = GetMonthString(CurrentMonthonth.Month);
                YearString = CurrentMonthonth.Year.ToString();
                Days = await HttpClient.GetFromJsonAsync<Day[]>($"/api/calendar/getalldays/{CurrentMonthonth.Month}/{CurrentMonthonth.Year}/{_parentEmail}");

                language.InitLocalizedComponent(this);
                //StateHasChanged();
            }
        }

        private string GetMonthString (int monthNumber)
        {
            switch (monthNumber)
            {
                case 1: return "leden";
                case 2: return "unor";
                case 3: return "brezen";
                case 4: return "duben";
                case 5: return "kveten";
                case 6: return "cerven";
                case 7: return "cervenec";
                case 8: return "srpen";
                case 9: return "zari";
                case 10: return "prosinec";
            }
            return "";
        }

        private void DateClick(DateTime date)
        {
            DateTime toDay = DateTime.Today;
            if (date == toDay)
            {
                var message = language["DnesNejdePrihlasit"];
            }
            else if(date < toDay)
            {
                var message = language["DnesNejdePrihlasit"];
            }
            else
            {
                navigationManager.NavigateTo($"signingchildrenstoschool/{date.ToShortDateString()}");
            }
        }

        private async void CalendarBack()
        {
            CurrentMonthonth = CurrentMonthonth.AddMonths(-1);
            Days = await HttpClient.GetFromJsonAsync<Day[]>($"/api/calendar/getalldays/{CurrentMonthonth.Month}/{CurrentMonthonth.Year}/{_parentEmail}");
            MonthString = GetMonthString(CurrentMonthonth.Month);
            YearString = CurrentMonthonth.Year.ToString();
            StateHasChanged();
        }

        private async void CalendarNext()
        {
            CurrentMonthonth = CurrentMonthonth.AddMonths(1);
            Days = await HttpClient.GetFromJsonAsync<Day[]>($"/api/calendar/getalldays/{CurrentMonthonth.Month}/{CurrentMonthonth.Year}/{_parentEmail}");
            MonthString = GetMonthString(CurrentMonthonth.Month);
            YearString = CurrentMonthonth.Year.ToString();
            StateHasChanged();
        }

        public async Task RefreshCalendar()
        {
            Days = await HttpClient.GetFromJsonAsync<Day[]>($"/api/calendar/getalldays/{CurrentMonthonth.Month}/{CurrentMonthonth.Year}/{_parentEmail}");
            StateHasChanged();
        }
    }
}