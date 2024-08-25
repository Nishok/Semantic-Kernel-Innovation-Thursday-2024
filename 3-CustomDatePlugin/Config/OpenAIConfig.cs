using System.ComponentModel.DataAnnotations;

namespace CustomDatePlugin.Config
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
