using System.Collections.Generic;
using CTRLS=VisioAutomation.Shapes.Controls;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioAutomation.Scripting.Commands
{
    public class ControlCommands : CommandSet
    {
        public ControlCommands(Client client) :
            base(client)
        {

        }

        public IList<int> Add(IList<IVisio.Shape> target_shapes, CTRLS.ControlCells ctrl)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (ctrl == null)
            {
                throw new System.ArgumentNullException("ctrl");
            }

            var shapes = GetTargetShapes(target_shapes);
            if (shapes.Count < 1)
            {
                return new List<int>(0);
            }


            var control_indices = new List<int>();
            using (var undoscope = new VA.Application.UndoScope(this.Client.VisioApplication,"Add Control"))
            {
                foreach (var shape in shapes)
                {
                    int ci = CTRLS.ControlHelper.Add(shape, ctrl);
                    control_indices.Add(ci);
                }
            }

            return control_indices;
        }

        public void Delete(IList<IVisio.Shape> target_shapes, int n)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var shapes = GetTargetShapes(target_shapes);
            if (shapes.Count < 1)
            {
                return;
            }

            using (var undoscope = new VA.Application.UndoScope(this.Client.VisioApplication, "Delete Control"))
            {
                foreach (var shape in shapes)
                {
                    CTRLS.ControlHelper.Delete(shape, n);
                }
            }
        }

        public Dictionary<IVisio.Shape, IList<CTRLS.ControlCells>> Get(IList<IVisio.Shape> target_shapes)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();
            
            var shapes = GetTargetShapes(target_shapes);
            if (shapes.Count < 1)
            {
                return new Dictionary<IVisio.Shape, IList<CTRLS.ControlCells>>(0);
            }

            var dic = new Dictionary<IVisio.Shape, IList<CTRLS.ControlCells>>();
            foreach (var shape in shapes)
            {
                var controls = CTRLS.ControlCells.GetCells(shape);
                dic[shape] = controls;
            }
            return dic;
        }
    }
}