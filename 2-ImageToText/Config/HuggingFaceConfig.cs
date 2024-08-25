using System.ComponentModel.DataAnnotations;

namespace ImageToText.Config
{
    public class HuggingFaceConfig
    {
        [Required]
        public required string Endpoint { get; set; }
        [Required]
        public required string ApiKey { get; set; }
    }
}
