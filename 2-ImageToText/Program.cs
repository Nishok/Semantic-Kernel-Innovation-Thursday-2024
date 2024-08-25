using ImageToText.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.ImageToText;

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

//Bind the configuration to the HuggingFaceConfig class and validate it on start
kernelBuilder.Services.AddOptions<HuggingFaceConfig>().BindConfiguration("HuggingFace").ValidateOnStart();
var huggingFaceConfigValues = config.GetSection("HuggingFace").Get<HuggingFaceConfig>()!;

#endregion

//https://huggingface.co/
kernelBuilder.AddHuggingFaceImageToText(new Uri(huggingFaceConfigValues.Endpoint), huggingFaceConfigValues.ApiKey);

Kernel kernel = kernelBuilder.Build();

// Gets the ImageToText Service
var imageToTextService = kernel.GetRequiredService<IImageToTextService>();

// Get the binary content of the JPEG image. You can give it many other image types
string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Images");
var imageBinary = File.ReadAllBytes(imagePath + "/MyImage.jpeg");

// Prepare the image to be sent
var imageContent = new ImageContent(imageBinary, "image/jpeg");

// Retrieves the image description from HuggingFace
var textContent = await imageToTextService.GetTextContentAsync(imageContent);

//Add the image description to the chat history
var history = new ChatHistory(textContent.Text ?? "");
//Feel free to ask the AI any questions about the image

#region Chat Completion

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while(true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");

    history.AddUserMessage(Console.ReadLine() ?? "");

    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history);
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
