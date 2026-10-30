using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Text.Json;

namespace HTML_SERI
{
    public class HtmlHelper
    {
        private static readonly HtmlHelper _instance =new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] Elements { get; set; }
        public string[] ElementsWithoutClose { get; set; }
        private HtmlHelper()
        {

            var contentElements = File.ReadAllText("HtmlTags.json");
            var contentElementsWithoutClose = File.ReadAllText("HtmlVoidTags.json");
            Elements = JsonSerializer.Deserialize<string[]>(contentElements);
            ElementsWithoutClose = JsonSerializer.Deserialize<string[]>(contentElementsWithoutClose);

        }
    }
}
