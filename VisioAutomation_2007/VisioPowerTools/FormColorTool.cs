using System.Windows.Forms;
using VisioAutomation.Extensions;
using VA = VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;
using System.Linq;

namespace VisioAutomation.Extensions
{
    public static class ColorMethods
    {
        public static VA.Drawing.ColorRGB ToColorRGB(this IVisio.Color color)
        {
            return new VA.Drawing.ColorRGB(color.Red, color.Green, color.Blue);
        }
    }
}

namespace VisioPowerTools
{
    public partial class FormColorTool : Form
    {
        public class ColorItem
        {
            public string Name;
            public VA.ShapeSheet.SRC SRC;
            public VA.ShapeSheet.FormulaLiteral Formula;

            public ColorItem(string name, VA.ShapeSheet.SRC src)
            {
                this.Name = name;
                this.SRC = src;
                this.Formula = null;
            }
        }

        public class ColorCells
        {
            public ColorItem FillBackgroundColor = new ColorItem("Fill Background", VA.ShapeSheet.SRCConstants.FillBkgnd);
            public ColorItem FillForegroundColor = new ColorItem("Fill Foreground", VA.ShapeSheet.SRCConstants.FillForegnd);
            public ColorItem LineColor = new ColorItem("Line Color", VA.ShapeSheet.SRCConstants.LineColor);
            public ColorItem ShadowBackgroundColor = new ColorItem("Shadow Background", VA.ShapeSheet.SRCConstants.ShdwBkgnd);

            public ColorItem ShadowForegroundColor = new ColorItem("Shadow Foreground",
                                                                   VA.ShapeSheet.SRCConstants.ShdwForegnd);

            public ColorItem Character = new ColorItem("Character", VA.ShapeSheet.SRCConstants.CharColor);
        }

        public ColorCells Colors = new ColorCells();

        public FormColorTool()
        {
            InitializeComponent();
        }

        private void buttonRead_Click(object sender, System.EventArgs e)
        {
            var client = VisioPowerToolsAddIn.Client;
            if (!client.Selection.HasShapes())
            {
                return;
            }

            var application = client.VisioApplication;
            var active_window = application.ActiveWindow;
            var selection = active_window.Selection;
            var s1 = selection[1];
            var doc = application.ActiveDocument;
            var doc_colors = doc.Colors;

            var rgbcolors = ShapeFormatHelper.GetColorsFromShape(s1);

            this.colorSelectorSmallFillFg.Color =
                (System.Drawing.Color) doc_colors[rgbcolors.FillForegroundColor.Result].ToColorRGB();
            this.colorSelectorSmallFillBg.Color =
                (System.Drawing.Color) doc_colors[rgbcolors.FillBackgroundColor.Result].ToColorRGB();
            this.colorSelectorSmallShdFg.Color =
                (System.Drawing.Color) doc_colors[rgbcolors.ShadowForegroundColor.Result].ToColorRGB();
            this.colorSelectorSmallShdBg.Color =
                (System.Drawing.Color) doc_colors[rgbcolors.ShadowBackgroundColor.Result].ToColorRGB();
            this.colorSelectorSmallLine.Color =
                (System.Drawing.Color) doc_colors[rgbcolors.LineColor.Result].ToColorRGB();

        }

        private void buttonApply_Click(object sender, System.EventArgs e)
        {
            var client = VisioPowerToolsAddIn.Client;

            this.Colors.FillForegroundColor.Formula =
                VA.Convert.ColorToFormulaRGB(this.colorSelectorSmallFillFg.Color);
            this.Colors.FillBackgroundColor.Formula =
                VA.Convert.ColorToFormulaRGB(this.colorSelectorSmallFillBg.Color);
            this.Colors.ShadowForegroundColor.Formula =
                VA.Convert.ColorToFormulaRGB(this.colorSelectorSmallShdFg.Color);
            this.Colors.ShadowBackgroundColor.Formula =
                VA.Convert.ColorToFormulaRGB(this.colorSelectorSmallShdBg.Color);
            this.Colors.LineColor.Formula =
                VA.Convert.ColorToFormulaRGB(this.colorSelectorSmallText.Color);

            var cells = new[]
                            {
                                this.Colors.FillBackgroundColor,
                                this.Colors.FillBackgroundColor,
                                this.Colors.ShadowForegroundColor,
                                this.Colors.ShadowBackgroundColor,
                                this.Colors.LineColor
                            };


            var xcells = cells.Where(i => i.Formula.HasValue).ToList();
            var srcs = xcells.Select(i => i.SRC).ToList();
            var formulas = xcells.Select(i => i.Formula.Value).ToList();

            client.ShapeSheet.SetFormula(null,srcs,formulas, IVisio.VisGetSetArgs.visSetBlastGuards);
        }
    }

    internal static class ShapeFormatHelper
    {

        public static ShapeColors GetColorsFromShape(IVisio.Shape shape)
        {
            if (shape == null)
            {
                throw new System.ArgumentNullException("shape");
            }

            var format = VisioAutomation.Shapes.FormatCells.GetCells(shape);
            var cformat = VA.Text.CharacterCells.GetCells(shape);

            var shapecolors = new ShapeFormatHelper.ShapeColors();
            
            shapecolors.FillForegroundColor = format.FillForegnd;
            shapecolors.FillBackgroundColor = format.FillBkgnd;
            shapecolors.ShadowForegroundColor = format.ShdwForegnd;
            shapecolors.ShadowBackgroundColor = format.ShdwBkgnd;
            shapecolors.LineColor = format.LineColor;
            shapecolors.CharacterColor = cformat[0].Color.Result;
            return shapecolors;
        }

        public class ShapeColors
        {
            public VA.ShapeSheet.CellData<int> FillForegroundColor;
            public VA.ShapeSheet.CellData<int> FillBackgroundColor;
            public VA.ShapeSheet.CellData<int> ShadowForegroundColor;
            public VA.ShapeSheet.CellData<int> ShadowBackgroundColor;
            public VA.ShapeSheet.CellData<int> LineColor;
            public VA.ShapeSheet.CellData<int> CharacterColor;

            public override string ToString()
            {
                string s = typeof (ShapeColors).Name;
                return s;
            }
        }
    }
}