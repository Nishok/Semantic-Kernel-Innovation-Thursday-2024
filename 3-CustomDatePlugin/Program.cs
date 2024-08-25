using CustomDatePlugin.Config;
using CustomDatePlugin.Plugins;
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
kernelBuilder.Services.AddOptions<OpenAIConfig>().BindConfiguration(configSection).ValidateOnStart();

var configValues = config.GetSection(configSection).Get<OpenAIConfig>()!;

kernelBuilder.AddAzureOpenAIChatCompletion(configValues.ModelId, configValues.Endpoint, configValues.ApiKey);

#endregion

//Add the Time Plugin to the kernel
kernelBuilder.Plugins.AddFromType<TimePlugin>();

Kernel kernel = kernelBuilder.Build();

//Allow the kernel to automatically invoke kernel functions whenever it sees fit
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

#region Chat Completion

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();

while(true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");

    history.AddUserMessage(Console.ReadLine() ?? "");

    //Make sure to pass the openAIPromptExecutionSettings and kernel, so it knows about the kernel functions
    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, openAIPromptExecutionSettings, kernel);
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
