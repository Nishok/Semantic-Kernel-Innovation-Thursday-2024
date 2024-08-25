using System.ComponentModel.DataAnnotations;

namespace ChatGPT.Config
{
    public class OpenAIConfig
    {
        [Required]
        public required string ModelId { get; set; }
        [Required]
        public required string Endpoint { get; set; }
        [Required]
        public required string ApiKey { get; set; }
    }
}
