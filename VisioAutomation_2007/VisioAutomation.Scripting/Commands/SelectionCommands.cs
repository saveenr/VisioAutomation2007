using System.Collections.Generic;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using System.Linq;

namespace VisioAutomation.Scripting.Commands
{
    public class SelectionCommands : CommandSet
    {
        public SelectionCommands(Client client) :
            base(client)
        {

        }
        
        public IVisio.Selection Get()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();
            
            var application = this.Client.VisioApplication;
            var active_window = application.ActiveWindow;
            var selection = active_window.Selection;
            return selection;
        }

        public void All()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();
            
            var active_window = this.Client.View.GetActiveWindow();
            active_window.SelectAll();
        }

        public void Invert()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();
            
            var application = this.Client.VisioApplication;
            var active_page = application.ActivePage;
            var shapes = active_page.Shapes;
            if (shapes.Count < 1)
            {
                return;
            }

            Invert(application.ActiveWindow);
        }

        private static void Invert(IVisio.Window window)
        {
            if (window == null)
            {
                throw new System.ArgumentNullException("window");
            }

            if (window.Page == null)
            {
                throw new System.ArgumentException("Window has null page", "window");
            }

            var page = (IVisio.Page) window.Page;
            var shapes = page.Shapes;
            var all_shapes = shapes.AsEnumerable();
            var selection = window.Selection;
            var selected_set = new System.Collections.Generic.HashSet<IVisio.Shape>(selection.AsEnumerable());
            var shapes_to_select = all_shapes.Where(shape => !selected_set.Contains(shape)).ToList();

            window.DeselectAll();
            window.Select(shapes_to_select, IVisio.VisSelectArgs.visSelect);
        }

        public void None()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var application = this.Client.VisioApplication;
            var active_window = application.ActiveWindow;
            active_window.DeselectAll();
            active_window.DeselectAll();
        }

        public void Select(IVisio.Shape shape)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (shape == null)
            {
                throw new System.ArgumentNullException("shape");
            }

            var application = this.Client.VisioApplication;
            var active_window = application.ActiveWindow;
            active_window.Select(shape, (short) IVisio.VisSelectArgs.visSelect);
        }

        public void Select(IEnumerable<IVisio.Shape> shapes)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (shapes == null)
            {
                throw new System.ArgumentNullException("shapes");
            }

            var application = this.Client.VisioApplication;
            var active_window = application.ActiveWindow;
            active_window.Select(shapes, IVisio.VisSelectArgs.visSelect);
        }

        public void Select(IEnumerable<int> shapeids)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (shapeids == null)
            {
                throw new System.ArgumentNullException("shapeids");
            }

            var application = this.Client.VisioApplication;
            var active_window = application.ActiveWindow;
            var page = application.ActivePage;
            var page_shapes = page.Shapes;
            var shapes = shapeids.Select(id => page_shapes.ItemFromID[id]).ToList();
            active_window.Select(shapes, IVisio.VisSelectArgs.visSelect);
        }
        
        public void SubSelect(IList<IVisio.Shape> shapes)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (shapes == null)
            {
                throw new System.ArgumentNullException("shapes");
            }
            
            this.Client.VisioApplication.ActiveWindow.Select(shapes, IVisio.VisSelectArgs.visSubSelect);
        }

        public void SelectByMaster(IVisio.Master master)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var application = this.Client.VisioApplication;
            var page = application.ActivePage;
            // Get a selection of connectors, by master: 
            var selection = page.CreateSelection(
                IVisio.VisSelectionTypes.visSelTypeByMaster,
                IVisio.VisSelectMode.visSelModeSkipSub, 
                master);
        }

        public void SelectByLayer(string layername)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (layername == null)
            {
                throw new System.ArgumentNullException("layername");
            }

            if (layername.Length < 1)
            {
                throw new System.ArgumentException("layername");
            }

            var layer = this.Client.Layer.Get(layername);
            var application = this.Client.VisioApplication;
            var page = application.ActivePage;

            // Get a selection of connectors, by layer: 
            var selection = page.CreateSelection(
                IVisio.VisSelectionTypes.visSelTypeByLayer,
                IVisio.VisSelectMode.visSelModeSkipSub, 
                layer);
        }

        public IList<IVisio.Shape> GetShapes()
        {
            this.AssertApplicationAvailable();

            var selection = this.Client.Selection.Get();
            return VA.Selection.SelectionHelper.GetSelectedShapes(selection);
        }

        public IList<IVisio.Shape> GetShapesRecursive()
        {
            this.AssertApplicationAvailable();

            var selection = this.Client.Selection.Get();
            return VA.Selection.SelectionHelper.GetSelectedShapesRecursive(selection);
        }

        public int Count()
        {
            this.AssertApplicationAvailable();
            
            var application = this.Client.VisioApplication;
            var active_window = application.ActiveWindow;
            var selection = active_window.Selection;
            int count = selection.Count;
            return count;
        }

        public IList<IVisio.Shape> GetSubSelectedShapes()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();
            
            //http://www.visguy.com/2008/05/17/detect-sub-selected-shapes-programmatically/
            var shapes = new List<IVisio.Shape>(0);
            var sel = this.Client.Selection.Get();
            var original_itermode = sel.IterationMode;

            // normal selection
            sel.IterationMode = ((short)IVisio.VisSelectMode.visSelModeSkipSub) + ((short)IVisio.VisSelectMode.visSelModeSkipSuper);

            if (sel.Count > 0)
            {
                shapes.AddRange(sel.AsEnumerable());
            }

            // sub selection
            sel.IterationMode = ((short)IVisio.VisSelectMode.visSelModeOnlySub) + ((short)IVisio.VisSelectMode.visSelModeSkipSuper);
            if (sel.Count > 0)
            {
                shapes.AddRange(sel.AsEnumerable());
            }

            sel.IterationMode = original_itermode;
            return shapes;
        }

        public void Delete()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (!this.Client.HasSelectedShapes())
            {
                return;
            }

            var selection = this.Get();
            selection.Delete();
        }

        public void Copy()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (!this.Client.HasSelectedShapes())
            {
                return;
            }

            var flags = IVisio.VisCutCopyPasteCodes.visCopyPasteNormal;

            var selection = this.Get();
            selection.Copy(flags);
        }

        public void Duplicate( IList<IVisio.Shape> target_shapes )
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            int n = this.GetTargetSelection(target_shapes);

            this.Client.WriteVerbose("Number of shapes to duplicate: {0}", n);

            if (n<1)
            {
                this.Client.WriteVerbose("Zero shapes to duplicate. No duplication operation performed");
                return;
            }

            var view = this.Client.View;
            var active_window = view.GetActiveWindow();
            var selection = active_window.Selection;
            selection.Duplicate();
        }

        public bool HasShapes()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            return HasShapes(1);
        }

        public bool HasShapes(int min_items)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (min_items <= 0)
            {
                throw new System.ArgumentOutOfRangeException("min_items");
            }

            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var application = this.Client.VisioApplication;
            var active_window = application.ActiveWindow;
            var selection = active_window.Selection;
            int num_selected = selection.Count;
            bool v = num_selected >= min_items;
            return v;
        }

        internal IEnumerable<IVisio.Shape> EnumShapes2D()
        {

            var shapes = this.GetShapes().Where(s => s.OneD == 0);
            return shapes;
        }
    }
}