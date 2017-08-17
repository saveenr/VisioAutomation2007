﻿using IVisio = Microsoft.Office.Interop.Visio;
using VA=VisioAutomation;

namespace VisioAutomation.Extensions
{
    public static class CellDataMethods
    {
        public static VA.ShapeSheet.CellData<int> ToInt(this VA.ShapeSheet.CellData<double> cd)
        {
            return new VA.ShapeSheet.CellData<int>(cd.Formula,(int)cd.Result);
        }

        public static VA.ShapeSheet.CellData<bool> ToBool(this VA.ShapeSheet.CellData<double> cd)
        {
            return new VA.ShapeSheet.CellData<bool>(cd.Formula, VA.Convert.DoubleToBool(cd.Result));
        }
    }
}