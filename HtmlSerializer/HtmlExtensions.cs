using HTML_SERI;
using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlSerializer
{
    
    public static class HtmlExtensions
    {
        private static void MySelect(Selector selector, HtmlElement element, HashSet<HtmlElement> answer)
        {
            if (selector == null)
            {
                return;
            }
            if (selector.Child != null)
            {
                foreach (var e in element.Descendants().Skip(1))
                    if (e.IsMatch(selector))
                    {
                        MySelect(selector.Child, e, answer);
                    }

            }
            else
            {
                foreach (var e in element.Descendants().Skip(1))
                    if (e.IsMatch(selector))
                    {
                        answer.Add(e);
                    }
            }

        }
        public static HashSet<HtmlElement> Query(this HtmlElement element,Selector selector)
        {
            HashSet<HtmlElement> res = new HashSet<HtmlElement>();
           HtmlExtensions.MySelect(selector,element,res);
            return res;
        }
    }
}
