using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioAutomation.DOM
{
    public class Node
    {
        public Node Parent { get; internal set; }
        public object Data { get; set; }

        protected Node()
        {
        }
    }
}