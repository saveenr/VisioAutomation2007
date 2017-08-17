using IVisio = Microsoft.Office.Interop.Visio;
using System;
using VA=VisioAutomation;
using System.Linq;

namespace VisioAutomation.Text.Markup
{
    public class FieldBase : Node
    {
        private const string placeholder_string = "[FIELD]";
        public IVisio.VisFieldFormats Format { get; set; }

        internal FieldBase(NodeType nt)
            : base(nt)
        {
        }
        
        public string PlaceholderText
        {
            get
            {
                return placeholder_string;
            }
        }
    }

}
