using HtmlSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTML_SERI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var html = await Load("https://hebrewbooks.org/beis");
            var htmlLines = new Regex("<(.*?)>")
      .Split(html)
      .Select(s => new Regex("\\s+").Replace(s, " ").Trim())
      .Where(s => s.Length > 0)
      .ToList();

            var root = buildTree(htmlLines);
            var result = root.Query(Selector.Cast("body a"));
            Console.WriteLine(result.Count);

        }
        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        static HtmlElement buildTree(List<string> htmlLines)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement html = root;
            foreach (var line in htmlLines)
            {
                if (!line.StartsWith("/"))
                {
                    string[] content = line.Split(' ');
                    if (HtmlHelper.Instance.Elements.Contains(content[0]))
                    {
                        HtmlElement current = new HtmlElement();

                        current.Name = content[0];
                        var rest = line.Substring(content[0].Length);
                        var attributeRegex = new Regex("([^\\s]*?)=\"(.*?)\"");
                        var attributes = attributeRegex.Matches(rest);
                        foreach (Match attribute in attributes)
                        {
                            string name = attribute.Groups[1].Value;
                            if (name.Equals("id"))
                            {
                                current.Id = attribute.Groups[2].Value;
                            }
                            else if (name.Equals("class"))
                            {
                                foreach (var g in attribute.Groups[2].Value.Split(' '))
                                {
                                    current.Classes.Add(g);
                                }
                            }
                            else
                            {
                                current.Attributes.Add(attribute.Value);
                            }
                        }
                        current.Parent = html;
                        html.Children.Add(current);
                        if (!(HtmlHelper.Instance.ElementsWithoutClose.Contains(content[0]) || line.EndsWith("/")))
                        {
                            html = current;
                        }
                        else
                        {
                            if (line == "/html")
                                break;

                            if (html.Parent != null)
                                html = html.Parent;
                        }
                    }
                    else
                    {
                        html.InnerHtml += line;
                    }

                }

            }
            return root;
        }
    }
}
