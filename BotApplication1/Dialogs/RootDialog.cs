using System;
using System.Configuration; //right clicked on project and added reference system.configuration
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using System.Web;

namespace BotApplication1.Dialogs
{
    [LuisModel("4c719492-2f5f-431f-9ebd-4a1c4ba0b6e1", "4205af860baf49d09458f5b60b5ca35e")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        ////added this
        //public const string Entity_Device = "HomeAutomation.Device";
        //public const string Entity_Room = "HomeAutomation.Room";
        //public const string Entity_Operation = "HomeAutomation.Operation";

        //// Intents
        //public const string Intent_TurnOn = "HomeAutomation.TurnOn";
        //public const string Intent_TurnOff = "HomeAutomation.TurnOff";
        //public const string Intent_None = "None";

        //added this
        public const string Entity_SchoolSubject = "School.Subject";

        // Intents
        public const string Intent_Algebra = "HelpWithAlgebra";
        public const string Intent_English = "HelpWithEnglish";
        public const string Intent_Matrices = "HelpWithMatrices";
        public const string Intent_Science = "HelpWithScience";
        public const string Intent_None = "None";

        public RootDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        //[LuisIntent("None")]
        //public async Task NoneIntent(IDialogContext context, LuisResult result)
        //{
        //    await this.ShowLuisResult(context, result);
        //}

        //// Go to https://luis.ai and create a new intent, then train/publish your luis app.
        //// Finally replace "Gretting" with the name of your newly created intent in the following handler
        //[LuisIntent("Greeting")]
        //public async Task GreetingIntent(IDialogContext context, LuisResult result)
        //{
        //    await this.ShowLuisResult(context, result);
        //}

        //[LuisIntent("Cancel")]
        //public async Task CancelIntent(IDialogContext context, LuisResult result)
        //{
        //    await this.ShowLuisResult(context, result);
        //}

        //[LuisIntent("Help")]
        //public async Task HelpIntent(IDialogContext context, LuisResult result)
        //{
        //    await this.ShowLuisResult(context, result);
        //}

        [LuisIntent(Intent_None)]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent(Intent_Algebra)]
        public async Task AlgebraIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent(Intent_English)]
        public async Task EnglishIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent(Intent_Matrices)]
        public async Task MatricesIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent(Intent_Science)]
        public async Task ScienceIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Entities found in result
        public string BotEntityRecognition(LuisResult result)
        {
            StringBuilder entityResults = new StringBuilder();

            if (result.Entities.Count > 0)
            {
                foreach (EntityRecommendation item in result.Entities)
                {
                    // Query: Turn on the [light]
                    // item.Type = "HomeAutomation.Device"
                    // item.Entity = "light"
                    entityResults.Append(item.Type + "=" + item.Entity + ",");
                }
                // remove last comma
                entityResults.Remove(entityResults.Length - 1, 1);
            }

            return entityResults.ToString();
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            // get recognized entities
            string entities = this.BotEntityRecognition(result);

            // round number
            string roundedScore = result.Intents[0].Score != null ? (Math.Round(result.Intents[0].Score.Value, 2).ToString()) : "0";

            await context.PostAsync($"**Query**: {result.Query}, **Intent**: {result.Intents[0].Intent}, **Score**: {roundedScore}. **Entities**: {entities}");
            context.Wait(MessageReceivedAsync);
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // Calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // Return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }
    }
}