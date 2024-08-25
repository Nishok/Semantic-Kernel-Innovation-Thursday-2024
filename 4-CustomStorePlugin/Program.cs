using CustomStorePlugin;
using CustomStorePlugin.Config;
using CustomStorePlugin.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#region Builder + Configuration Prep

string configSection = "AzureOpenAI";

IConfigurationRoot config = new ConfigurationBuilder()
                                .AddUserSecrets<Program>()
                                .Build();

IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

kernelBuilder.Services.TryAddSingleton<IConfiguration>(config);
kernelBuilder.Services.AddDbContext<ApplicationDbContext>();
kernelBuilder.Services.AddOptions<OpenAIConfig>().BindConfiguration(configSection).ValidateOnStart();

var configValues = config.GetSection(configSection).Get<OpenAIConfig>()!;

kernelBuilder.AddAzureOpenAIChatCompletion(configValues.ModelId, configValues.Endpoint, configValues.ApiKey);

#endregion

//Add the Store Plugin to the kernel
kernelBuilder.Plugins.AddFromType<StorePlugin>();

Kernel kernel = kernelBuilder.Build();

OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

#region Chat Completion

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

var systemMessage = @"Your task is to help the user view purchasable items, and help them make the needed purchases.
You are here to help the user with the store, and nothing more.
You will be the first to start the chat, so greet the user stating you are there to help as a shopkeeper.";

var history = new ChatHistory(systemMessage);

var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, openAIPromptExecutionSettings, kernel);
//Made the AI initiate the conversation, no particular reason.
await AIReply(history, result);

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");

    history.AddUserMessage(Console.ReadLine() ?? "");

    result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, openAIPromptExecutionSettings, kernel);
    await AIReply(history, result);
}

//Extracted the AI reply to a separate method, as it's now used twice.
static async Task AIReply(ChatHistory history, IAsyncEnumerable<StreamingChatMessageContent> result)
{
    string fullResponse = "";

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("AI > ");
    await foreach (var item in result)
    {
        Console.Write(item.Content);
        fullResponse += item.Content;
    }

    history.AddAssistantMessage(fullResponse);

    Console.WriteLine();
}

#endregion
