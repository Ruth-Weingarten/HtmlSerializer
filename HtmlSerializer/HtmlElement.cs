using HtmlSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_SERI
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }

        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue= new Queue<HtmlElement>();
            queue.Enqueue(this);
            while(queue.Count>0)
            {
                HtmlElement current= queue.Dequeue();
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
                yield return current;

            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while(current.Parent!=null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
        public bool IsMatch(Selector selector)
        {
            if(selector.TagName!=null&&selector.TagName!=this.Name) return false;
            if(selector.Id!=null&&!selector.Id.Equals(this.Id)) return false;
            foreach(var  cl in selector.Classes)
            {
                if(!this.Classes.Contains(cl)) return false;
            }
            return true;
        }
       

    }
}
