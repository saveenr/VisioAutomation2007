﻿using VA = VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;
using SRCCON = VisioAutomation.ShapeSheet.SRCConstants;

namespace VisioAutomation.Text.Markup
{
    public class ParagraphCells 
    {
        public VA.ShapeSheet.FormulaLiteral Bullet { get; set; }
        public VA.ShapeSheet.FormulaLiteral BulletFont { get; set; }
        public VA.ShapeSheet.FormulaLiteral BulletFontSize { get; set; }
        public VA.ShapeSheet.FormulaLiteral BulletString { get; set; }
        public VA.ShapeSheet.FormulaLiteral Flags { get; set; }
        public VA.ShapeSheet.FormulaLiteral HorizontalAlign { get; set; }
        public VA.ShapeSheet.FormulaLiteral IndentFirst { get; set; }
        public VA.ShapeSheet.FormulaLiteral IndentLeft { get; set; }
        public VA.ShapeSheet.FormulaLiteral IndentRight { get; set; }
        public VA.ShapeSheet.FormulaLiteral LocBulletFont { get; set; }
        public VA.ShapeSheet.FormulaLiteral SpacingAfter { get; set; }
        public VA.ShapeSheet.FormulaLiteral SpacingBefore { get; set; }
        public VA.ShapeSheet.FormulaLiteral SpacingLine { get; set; }
        public VA.ShapeSheet.FormulaLiteral TextPosAfterBullet { get; set; }
        
        internal void ApplyFormulas(VA.ShapeSheet.Update update, short row)
        {
            update.SetFormulaIgnoreNull(SRCCON.Para_IndLeft.ForRow(row), this.IndentLeft);
            update.SetFormulaIgnoreNull(SRCCON.Para_IndFirst.ForRow(row), this.IndentFirst);
            update.SetFormulaIgnoreNull(SRCCON.Para_IndRight.ForRow(row), this.IndentRight);
            update.SetFormulaIgnoreNull(SRCCON.Para_SpAfter.ForRow(row), this.SpacingAfter);
            update.SetFormulaIgnoreNull(SRCCON.Para_SpBefore.ForRow(row), this.SpacingBefore);
            update.SetFormulaIgnoreNull(SRCCON.Para_SpLine.ForRow(row), this.SpacingLine);
            update.SetFormulaIgnoreNull(SRCCON.Para_HorzAlign.ForRow(row), this.HorizontalAlign);
            update.SetFormulaIgnoreNull(SRCCON.Para_BulletFont.ForRow(row), this.BulletFont);
            update.SetFormulaIgnoreNull(SRCCON.Para_Bullet.ForRow(row), this.Bullet);
            update.SetFormulaIgnoreNull(SRCCON.Para_BulletFontSize.ForRow(row), this.BulletFontSize);
            update.SetFormulaIgnoreNull(SRCCON.Para_LocalizeBulletFont.ForRow(row), this.LocBulletFont);
            update.SetFormulaIgnoreNull(SRCCON.Para_TextPosAfterBullet.ForRow(row), this.TextPosAfterBullet);
            update.SetFormulaIgnoreNull(SRCCON.Para_Flags.ForRow(row), this.Flags);
            update.SetFormulaIgnoreNull(SRCCON.Para_BulletStr.ForRow(row), this.BulletString);
        }

        public void ApplyFormulasTo(ParagraphCells other)
        {
            if (this.IndentFirst.HasValue) { other.IndentFirst = this.IndentFirst; }
            if (this.IndentLeft.HasValue) { other.IndentLeft = this.IndentLeft; }
            if (this.IndentRight.HasValue) { other.IndentRight = this.IndentRight; }

            if (this.SpacingAfter.HasValue) { other.SpacingAfter = this.SpacingAfter; }
            if (this.SpacingBefore.HasValue) { other.SpacingBefore = this.SpacingBefore; }
            if (this.SpacingBefore.HasValue) { other.SpacingBefore = this.SpacingBefore; }

            if (this.Bullet.HasValue) { other.Bullet = this.Bullet; }
            if (this.BulletFont.HasValue) { other.BulletFont = this.BulletFont; }
            if (this.BulletFontSize.HasValue) { other.BulletFontSize = this.BulletFontSize; }

            if (this.BulletString.HasValue) { other.BulletString = this.BulletString; }
            if (this.Flags.HasValue) { other.Flags = this.Flags; }
            if (this.HorizontalAlign.HasValue) { other.HorizontalAlign = this.HorizontalAlign; }
            if (this.LocBulletFont.HasValue) { other.LocBulletFont = this.LocBulletFont; }
            if (this.TextPosAfterBullet.HasValue) { other.TextPosAfterBullet = this.TextPosAfterBullet; }
        }
    }
}