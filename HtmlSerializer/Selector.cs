using HTML_SERI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlSerializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public static Selector Cast(string query)
        {
            String[] selectors = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var root = new Selector();
            var current = root;
            foreach (var selector in selectors)
            {
                String[] attributes = Regex.Split(selector, "(?=[#.])");
                Selector newSelector = new Selector();
                foreach (var attr in attributes)
                {
                    if (attr.StartsWith("#"))
                    {
                        newSelector.Id = attr.Substring(1);
                    }
                    else if (attr.StartsWith('.'))
                    {
                        newSelector.Classes.Add(attr.Substring(1));
                    }
                    else if (HtmlHelper.Instance.Elements.Contains(attr))
                    {
                        newSelector.TagName = attr;
                    }
                }
                current.Child = newSelector;
                newSelector.Parent = current;
                current = current.Child;
            }
            if (root.Child != null)
                root.Child.Parent = null;
            return root.Child;
        }

       
    }
}
