using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace GREsau.AspNetCore.Test.TestApp
{
    public class Model
    {
        [Range(1, 100)]
        public int Foo { get; set; }

        [Range(1, 100)]
        [JsonPropertyName("foo_bar")]
        public int Bar { get; set; }

        [Range(1, 100)]
        [BindProperty(Name = "bind_overridden")]
        [Display(Name = "display_overridden")]
        public int Baz { get; set; }
    }
}
