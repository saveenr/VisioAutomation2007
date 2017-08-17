using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;
using System.Linq;
using VA = VisioAutomation;

namespace VisioAutomation.Extensions
{
    public static class PageMethods
    {
        public static void ResizeToFitContents(this IVisio.Page page, VA.Drawing.Size padding)
        {
            VA.Pages.PageHelper.ResizeToFitContents(page, padding);
        }

        public static IVisio.Shape DrawLine(this IVisio.Page page, VA.Drawing.Point p1, VA.Drawing.Point p2)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            var shape = surface.DrawLine(p1.X, p1.Y, p2.X, p2.Y);
            return shape;
        }

        public static IVisio.Shape DrawOval(this IVisio.Page page, VA.Drawing.Rectangle rect)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            var shape = surface.DrawOval(rect);
            return shape;
        }

        public static IVisio.Shape DrawRectangle(this IVisio.Page page, VA.Drawing.Rectangle rect)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            var shape = surface.DrawRectangle(rect);
            return shape;
        }

        public static IVisio.Shape DrawBezier(this IVisio.Page page, IList<VA.Drawing.Point> points)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            var shape = surface.DrawBezier(points);
            return shape;        
        }

        public static IVisio.Shape DrawBezier(this IVisio.Page page, IList<VA.Drawing.Point> points, short degree, short flags)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            var shape = surface.DrawBezier(points,degree,flags);
            return shape;        
        }

        public static IVisio.Shape DrawPolyline(this IVisio.Page page, IList<VA.Drawing.Point> points)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            var shape = surface.DrawBezier(points);
            return shape;
        }

        public static IVisio.Shape DrawNURBS(this IVisio.Page page, IList<VA.Drawing.Point> controlpoints,
                                             IList<double> knots,
                                             IList<double> weights, int degree)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            var shape = surface.DrawNURBS(controlpoints, knots, weights, degree);
            return shape;
        }

        public static IVisio.Shape Drop(
            this IVisio.Page page,
            IVisio.Master master,
            VA.Drawing.Point point)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            return surface.Drop(master, point);
        }

        public static short[] DropManyU(
            this IVisio.Page page,
            IList<IVisio.Master> masters,
            IEnumerable<VA.Drawing.Point> points)
        {
            var surface = new VA.Drawing.DrawingSurface(page);
            short[] shapeids = surface.DropManyU(masters, points);
            return shapeids;
        }

        public static IEnumerable<IVisio.Page> AsEnumerable(this IVisio.Pages pages)
        {
            short count = pages.Count;
            for (int i = 0; i < count; i++)
            {
                yield return pages[i + 1];
            }
        }

        public static string[] GetNamesU(this IVisio.Pages pages)
        {
            System.Array names_sa;
            pages.GetNamesU(out names_sa);
            string[] names = (string[])names_sa;
            return names;
        }
    }
}