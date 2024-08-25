using ChatGPT.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

#region Quick'n'Dirty Prep

//IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

////FOR DEMO PURPOSE - Do not use this in production, as it exposes your API key in the code. Use the example below in the "Builder + Configuration Prep" region instead.
//kernelBuilder.AddAzureOpenAIChatCompletion("gpt-4o", "https://my-openai-endpoint.openai.azure.com/", "MySecretApiKey");

#endregion

#region Builder + Configuration Prep

string configSection = "AzureOpenAI"; //Change this to "OpenAI" or "Ollama" to use the other models.

//Load configuration from user secrets
IConfigurationRoot config = new ConfigurationBuilder()
                                .AddUserSecrets<Program>()
                                .Build();

//Create the kernel builder
IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

//Bind the configuration to the OpenAIConfig class and validate it on start
kernelBuilder.Services.TryAddSingleton<IConfiguration>(config);
kernelBuilder.Services.AddOptions<OpenAIConfig>().BindConfiguration(configSection).ValidateOnStart();

//Get the configuration values
var configValues = config.GetSection(configSection).Get<OpenAIConfig>()!;

//Azure OpenAI configuration:
kernelBuilder.AddAzureOpenAIChatCompletion(configValues.ModelId, configValues.Endpoint, configValues.ApiKey);
//OpenAI configuration:
//kernelBuilder.AddOpenAIChatCompletion(configValues.ModelId, configValues.ApiKey);
//Ollama configuration:
//kernelBuilder.AddOpenAIChatCompletion(configValues.ModelId, new Uri(configValues.Endpoint), null);
#endregion

//Build the kernel
Kernel kernel = kernelBuilder.Build();

#region Chat Completion

//Get the chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

//Give the SK a system message, in this case describing its purpose
var systemMessage = @"You are specialized in SQL, more specifically TSQL for Miscrosoft SQL,
You are here to help the user with anything SQL related, and nothing more,
If the user asks you anything that has nothing to do with SQL, state that your knowledge only consists of SQL.";

var history = new ChatHistory(systemMessage);


while(true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");

    //Add the user's message to the chat history.
    history.AddUserMessage(Console.ReadLine() ?? "");

    //Get the AI's response
    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history);
    string fullResponse = "";

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("AI > ");
    await foreach (var item in result)
    {
        Console.Write(item.Content);
        fullResponse += item.Content;
    }

    //Add the AI's response to the chat history.
    history.AddAssistantMessage(fullResponse);

    Console.WriteLine();

    //Note: The chat history is not persisted in this example.
    //In a real-world scenario, you would likely want to store the chat history in a database or other storage mechanism.
    //Also note that it's best to avoid storing everything, as it will increase the token amount used significantly.
}

#endregion
