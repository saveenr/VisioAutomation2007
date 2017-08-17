using System.Collections.Generic;
using System.Linq;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using CONS = VisioAutomation.Shapes.Connections;

namespace VisioAutomation.Scripting.Commands
{
    public class ConnectionPointCommands : CommandSet
    {
        public ConnectionPointCommands(Client client) :
            base(client)
        {

        }

        public IDictionary<IVisio.Shape, IList<CONS.ConnectionPointCells>> Get(IList<IVisio.Shape> target_shapes)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var shapes = GetTargetShapes(target_shapes);

            if (shapes.Count<1)
            {
                return new Dictionary<IVisio.Shape, IList<CONS.ConnectionPointCells>>();
            }

            var dic = new Dictionary<IVisio.Shape, IList<CONS.ConnectionPointCells>>();
            foreach (var shape in shapes)
            {
                var cp = CONS.ConnectionPointCells.GetCells(shape);
                dic[shape] = cp;
            }

            return dic;
        }

        public IList<int> Add( IList<IVisio.Shape> target_shapes, 
            string fx,
            string fy,
            CONS.ConnectionPointType type)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var shapes = GetTargetShapes(target_shapes);
            if (shapes.Count < 1)
            {
                return new List<int>(0);
            }

            int dirx = 0;
            int diry = 0;

            var indices = new List<int>(shapes.Count);

            using (var undoscope = new VA.Application.UndoScope(this.Client.VisioApplication, "Add Connection Point"))
            {
                var cp = new CONS.ConnectionPointCells();
                cp.X = fx;
                cp.Y = fy;
                cp.DirX = dirx;
                cp.DirY = diry;
                cp.Type = (int)type;

                foreach (var shape in shapes)
                {
                    int index = CONS.ConnectionPointHelper.Add(shape, cp);
                    indices.Add(index);
                }
            }

            return indices;
        }


        public IList<int> Add(
            string fx,
            string fy,
            CONS.ConnectionPointType type)
        {
            this.AssertApplicationAvailable();

            return this.Add(null, fx, fy, type);
        }

        public void Delete(List<IVisio.Shape> target_shapes0, int index)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var shapes = GetTargetShapes(target_shapes0);
            if (shapes.Count < 1)
            {
                return;
            }

            var target_shapes = shapes.Where(shape => CONS.ConnectionPointHelper.GetCount(shape) > index);

            using (var undoscope = new VA.Application.UndoScope(this.Client.VisioApplication, "Delete Connection Point"))
            {
                foreach (var shape in target_shapes)
                {
                    CONS.ConnectionPointHelper.Delete(shape, index);
                }
            }
        }
    }
}