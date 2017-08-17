﻿using VA = VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace VisioAutomationSamples
{
    public static class SimpleGeometrySamples
    {
        public static void BezierCircle()
        {
            // Draw an approximation of a circle using Bezier Curves

            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            var curve0 = VA.Drawing.BezierCurve.FromEllipse(
                new VA.Drawing.Point(5, 4),
                new VA.Drawing.Size(1, 1));

            var s0 = page.DrawBezier(curve0.ControlPoints);
            s0.Text = "Bezier approximating circle";
        }

        public static void BezierEllipse()
        {
            // Draw an approximation of an ellipse using Bezier Curves

            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            var curve0 = VA.Drawing.BezierCurve.FromEllipse(
                new VA.Drawing.Point(2, 4),
                new VA.Drawing.Size(1, 0.5));
            var s0 = page.DrawBezier(curve0.ControlPoints);
            s0.Text = "Bezier approximating ellipse";
        }

        public static void BezierSimple()
        {
            // Draw a Simple Bezier curve

            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            var points = new[] {0.0, 0.0, 1.0, 2.0, 3.0, 0.5, 4.0, 0.5};
            var cpoints = VA.Drawing.Point.FromDoubles(points).ToList();
            var s0 = page.DrawBezier(cpoints);
            s0.Text = "Bezier curve";
            foreach (var p in cpoints)
            {
                var p1 = p.Subtract(0.1, 0.1);
                var p2 = p.Add(0.1, 0.1);
                page.DrawRectangle(p1.X, p1.Y, p2.X, p2.Y);
            }
        }

        public static void NURBS1()
        {
            // Draw a simple NURBS
            // Example from this page:http://www.robthebloke.org/opengl_programming.html

            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            var points = new[]
                             {
                                 new VA.Drawing.Point(10, 10),
                                 new VA.Drawing.Point(5, 10),
                                 new VA.Drawing.Point(-5, 5),
                                 new VA.Drawing.Point(-10, 5),
                                 new VA.Drawing.Point(-4, 10),
                                 new VA.Drawing.Point(-4, 5),
                                 new VA.Drawing.Point(-8, 1)
                             };

            var origin = new VA.Drawing.Point(4, 4);
            var scale = new VA.Drawing.Size(1.0/4.0, 1.0/4.0);

            var controlpoints = points.Select(x => (x*scale) + origin).ToList();
            var knots = new double[] {0, 0, 0, 0, 1, 2, 3, 4, 4, 4, 4};
            var degree = 3;
            var weights = controlpoints.Select(i => 1.0).ToList();

            var s0 = page.DrawNURBS(controlpoints, knots, weights, degree);
            s0.Text = "Generic NURBS shape";
        }

        public static void NURBS2()
        {
            // Draw a simple NURBS
            // Example from Graham Wideman's book

            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            var points = new[]
                             {
                                 new VA.Drawing.Point(0.2500, 0.2500),
                                 new VA.Drawing.Point(0.2500, 0.7500),
                                 new VA.Drawing.Point(0.4063, 0.8125),
                                 new VA.Drawing.Point(0.5625, 0.3750),
                                 new VA.Drawing.Point(0.5538, 0.8125),
                                 new VA.Drawing.Point(0.7600, 0.7500),
                                 new VA.Drawing.Point(0.7600, 0.2500)
                             };

            var origin = new VA.Drawing.Point(4, 4);
            var scale = new VA.Drawing.Size(4, 4);

            var controlpoints = points.Select(x => (x*scale) + origin).ToList();
            var knots = new double[] {0, 0, 0, 0, 25, 50, 75, 100, 100, 100, 100};
            var degree = 3;
            var Weights = controlpoints.Select(i => 1.0).ToList();

            var s0 = page.DrawNURBS(controlpoints, knots, Weights, degree);
            s0.Text = "Generic NURBS shape";
        }
    }
}