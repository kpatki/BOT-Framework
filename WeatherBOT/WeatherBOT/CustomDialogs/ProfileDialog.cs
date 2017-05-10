using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using WeatherBOT.Models;

namespace WeatherBOT.CustomDialogs
{
    [Serializable]
    public class ProfileDialog : IDialog<UserProfile>
    {
        public Task StartAsync(IDialogContext context)
        {
            FetchProfileName(context);

            return Task.CompletedTask;
        }

        UserProfile _profile;
        public void FetchProfileName(IDialogContext context)
        {
            if (!context.UserData.TryGetValue("@profile", out _profile))
            {
                _profile = new UserProfile();
                _profile.WeatherQuery = new WeatherQuery();

                if (string.IsNullOrEmpty(_profile.Name))
                {
                    PromptDialog.Text(context, FillName, @"Your Name please ?");
                }
                else
                    FetchProfileEmail(context);
            }
        }               

        public void FetchProfileEmail(IDialogContext context)
        {
            if (string.IsNullOrEmpty(_profile.EmailId))
                PromptDialog.Text(context, FillEmailId, @"Your Email-ID please ?");
            else
                FetchQueryInfo(context);
        }

        public void FetchQueryInfo(IDialogContext context)
        {
            if (string.IsNullOrEmpty(_profile.WeatherQuery.City))
                PromptDialog.Text(context, FillCity, @"Please enter the City Name");
            else
                FetchCountryInfo(context);
        }

        public void FetchCountryInfo(IDialogContext context)
        {
            if (string.IsNullOrEmpty(_profile.WeatherQuery.Country))
                PromptDialog.Text(context, FillCountry, $@"Please enter the Country Name for {_profile.WeatherQuery.City}");
            else
                context.Done(_profile);
        }

        public async Task FillCity(IDialogContext context, IAwaitable<string> result)
        {
            _profile.WeatherQuery.City = await result;

            FetchCountryInfo(context);
        }

        public async Task FillName(IDialogContext context, IAwaitable<string> result)
        {
            _profile.Name = await result;

            FetchProfileEmail(context);
        }

        public async Task FillEmailId(IDialogContext context, IAwaitable<string> result)
        {
            _profile.EmailId = await result;

            FetchQueryInfo(context);
        }

        public async Task FillCountry(IDialogContext context, IAwaitable<string> result)
        {
            _profile.WeatherQuery.Country = await result;

            context.Done(_profile);
        }
    }
}