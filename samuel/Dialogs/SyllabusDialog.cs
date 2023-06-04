﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace samuel.Dialogs
{
    public class ExercisesDialog : ComponentDialog
    {

        public ExercisesDialog()
            : base(nameof(ExercisesDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your name.") };
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Analiza mi código":
                    await stepContext.Context.SendActivityAsync("sdf", cancellationToken: cancellationToken);
                    break;
                case "Tengo consultas":
                    await stepContext.BeginDialogAsync(nameof(QnADialog), null, cancellationToken);
                    break;
                default:
                    await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                    break;
            }
            return await stepContext.ContinueDialogAsync(cancellationToken);
        }
    }
}