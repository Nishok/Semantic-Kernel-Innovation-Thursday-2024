# Semantic Kernel Innovation Thursday 2024

This repository contains code that was created for demonstrating some Semantic Kernel capabilities.
It is intended to showcase basic concepts and should not be considered production-ready. 
Before deploying to a production environment, please review, test, and improve upon the code to ensure it meets your security, performance, and scalability requirements.

## Prerequisites

- A subscription on either Azure (Azure for the AI Studio) or a subscription on OpenAI.
- Visual Studio (Code)
- SQL Server
- Run the SQL script `DB-Create.sql` to generate the database needed for the projects `4-CustomStorePlugin` and `4-CustomStorePluginExtended`.


## 🛠 User Secrets

These fields are required in your User Secrets:
```json
{
  "AzureOpenAI": {
    "ModelId": "gpt-4o-mini",
    "Endpoint": "", //Get your Endpoint from Azure AI Studio
    "ApiKey": "" //Get your Key from Azure AI Studio
  },
  "OpenAI": {
    "ModelId": "gpt-4o-mini",
    "Endpoint": "https://api.openai.com/",
    "ApiKey": "" //Get your Key from OpenAI.
  },
  "Ollama": {
    "ModelId": "llama3.1",
    "Endpoint": "http://localhost:11434",
    "ApiKey": "null" //Ollama has no auth, so null is fine.
  },
  "HuggingFace": {
    "Endpoint": "https://api-inference.huggingface.co/models/Salesforce/blip-image-captioning-base",
    "ApiKey": "" //Get your key from https://huggingface.co/
  },
  "ConnectionStrings": {
    "DefaultConnection": "" //Your DB connection string.
  }
}
```


## 🔗 Useful Links

- Microsoft Learn – Semantic Kernel
	[https://learn.microsoft.com/en-us/semantic-kernel/overview/](https://learn.microsoft.com/en-us/semantic-kernel/overview/)

- Semantic Kernel GitHub
	[https://github.com/microsoft/semantic-kernel](https://github.com/microsoft/semantic-kernel)

- Azure AI Studio + Pricing
	[https://ai.azure.com/](https://ai.azure.com/)
	[https://azure.microsoft.com/en-us/pricing/details/cognitive-services/openai-service/](https://azure.microsoft.com/en-us/pricing/details/cognitive-services/openai-service/)

- OpenAI + Pricing
	[https://platform.openai.com/](https://platform.openai.com/)
	[https://openai.com/api/pricing/](https://openai.com/api/pricing/)

- Ollama
	[https://github.com/ollama/ollama/](https://github.com/ollama/ollama/)