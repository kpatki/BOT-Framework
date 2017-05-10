using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using WeatherBOT.Models;
using WeatherBOT.WeatherClient;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Threading;

namespace WeatherBOT.CustomDialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        UserProfile _profileData;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == "exit")
            {
                PromptDialog.Confirm(
                    context,
                    AfterSelectOption,
                    "Hope you liked the Weather BOT?", "Please let us know your thoughts about it.",
                    promptStyle: PromptStyle.PerLine);
            }
            else if (!context.UserData.TryGetValue("@profile", out _profileData))
            {
                context.Call(new ProfileDialog(), FillProfileData);
            }

            await Task.CompletedTask;
        }

        public async Task FillProfileData(IDialogContext context, IAwaitable<UserProfile> result)
        {
            var profile = await result;

            context.UserData.SetValue("@profile", profile);

            var firstMarkDownContent = $@"That's Great **{profile.Name}**. Welcome to WeatherBOT!";
            firstMarkDownContent += "\n\n";
            firstMarkDownContent += $@"Please wait while we fetch weather for **{profile.WeatherQuery.City}** (*{profile.WeatherQuery.Country})*";
            await context.PostAsync(firstMarkDownContent);

            var weatherResponse = Helper.Fetch(profile.WeatherQuery.City, (profile.WeatherQuery.Country.Substring(0, 2)));

            var secondMarkDownContent = $@"Weather today for *{profile.WeatherQuery.City}* is **{weatherResponse.weather[0].description}**";
            await context.PostAsync(secondMarkDownContent);

            PromptDialog.Choice(context, this.OnOptionSelected, new[] { "EXIT", "STAY" }, "Please select one option", null, 2, PromptStyle.PerLine);

            context.Wait(MessageReceivedAsync);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            context.Wait(MessageReceivedAsync);

        }

        public async Task AfterSelectOption(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;

            //var _profile = new UserProfile();
            //context.UserData.TryGetValue("@profile", out _profile);

            if (confirm)
            {
                //_profile.WeatherQuery = new WeatherQuery();
                await context.PostAsync("No problem, WeatherBOT is signing OFF!");
            }
            else
            {
                await context.PostAsync("Thanks a lot for your time.");
            }

            //context.UserData.SetValue("@profile", _profile);

            context.Wait(MessageReceivedAsync);
        }
       
    }
}