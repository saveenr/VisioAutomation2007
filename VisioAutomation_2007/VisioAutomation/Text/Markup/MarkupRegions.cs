using System.Collections.Generic;

namespace VisioAutomation.Text.Markup
{
    class MarkupRegions
    {
        public IList<TextRegion> FormatRegions { get; private set; }
        public IList<TextRegion> FieldRegions { get; private set; }

        public MarkupRegions()
        {
            this.FormatRegions = new List<TextRegion>();
            this.FieldRegions = new List<TextRegion>();
        }
    }
}