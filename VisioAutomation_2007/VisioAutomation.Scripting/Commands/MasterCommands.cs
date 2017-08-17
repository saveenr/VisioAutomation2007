using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioAutomation.Scripting.Commands
{
    public class MasterCommands : CommandSet
    {
        public MasterCommands(Client client) :
            base(client)
        {

        }

        public void OpenForEdit(IVisio.Master master)
        {
            var mdraw_window = master.OpenDrawWindow();
            mdraw_window.Activate();
        }

        public void CloseMasterEditing()
        {
            var window = this.Client.VisioApplication.ActiveWindow;

            var st = window.SubType;
            if (st != 64)
            {
                throw new AutomationException("The active window is not a master window");
            }

            var master = (IVisio.Master)window.Master;
            master.Close();
        }

        public IList<IVisio.Master> Get()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var application = this.Client.VisioApplication;
            var doc = application.ActiveDocument;
            var doc_masters = doc.Masters;
            var masters = doc_masters.AsEnumerable().ToList();
            return masters;
        }

        public IList<IVisio.Master> Get(IVisio.Document doc)
        {
            this.AssertApplicationAvailable();
            var doc_masters = doc.Masters;
            var masters = doc_masters.AsEnumerable().ToList();
            return masters;
        }

        public IVisio.Master Get(string name)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (name == null)
            {
                throw new System.ArgumentNullException("name");
            }

            if (name.Length < 1)
            {
                throw new System.ArgumentException("mastername");
            }
            
            IVisio.Master master;
            try
            {
                var application = this.Client.VisioApplication;
                var active_document = application.ActiveDocument;
                var masters = active_document.Masters;
                master = masters.ItemU[name];
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                string msg = string.Format("No such master \"{0}\"", name);
                throw new VA.Scripting.ScriptingException(msg);
            }
            return master;
        }

        public IVisio.Master Get(string master, IVisio.Document doc)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (master == null)
            {
                throw new System.ArgumentNullException("master");
            }

            if (doc == null)
            {
                throw new System.ArgumentNullException("doc");
            }

            var application = this.Client.VisioApplication;
            var documents = application.Documents;

            var masters = doc.Masters;
            IVisio.Master masterobj = this.TryGetMaster(masters, master);
            if (masterobj == null)
            {
                string msg = string.Format("No such master \"{0}\" in \"{1}\"", master, doc);
                throw new VA.Scripting.ScriptingException(msg);
            }

            return masterobj;
        }

        public List<IVisio.Master> GetMastersByName(string name, IVisio.Document doc)
        {
            if (name == null || name == "*")
            {
                // return all masters
                var masters = doc.Masters.AsEnumerable().ToList();
                return masters;
            }
            else
            {
                // return masters matching the name
                var masters2 = doc.Masters.AsEnumerable();
                var masters3 = VA.TextUtil.FilterObjectsByNames(masters2, new[] { name }, p => p.Name, true, VA.TextUtil.FilterAction.Include).ToList();
                return masters3;
            } 
        }

        public List<IVisio.Master> GetMastersByName(string name)
        {
            var application = this.Client.VisioApplication;
            var doc = application.ActiveDocument;
            return this.GetMastersByName(name, doc);
        }

        private IVisio.Master TryGetMaster(IVisio.Masters masters, string name)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            try
            {
                var masterobj = masters.ItemU[name];
                return masterobj;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return null;
            }
        }

        public IVisio.Shape Drop(IVisio.Master master, double x, double y)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var application = this.Client.VisioApplication;
            var page = application.ActivePage;
            var shape = page.Drop(master, x, y);
            return shape;
        }

        public short[] Drop(IList<IVisio.Master> masters, IList<VA.Drawing.Point> points)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            if (masters == null)
            {
                throw new System.ArgumentNullException("points");
            }

            if (points == null)
            {
                throw new System.ArgumentNullException("points");
            }

            var application = this.Client.VisioApplication;
            var page = application.ActivePage;
            var shapeids = page.DropManyU(masters, points);
            return shapeids;
        }

        public IVisio.Master New(IVisio.Document document, string name)
        {
            this.AssertApplicationAvailable();

            if (document == null)
            {
                document = this.Client.VisioApplication.ActiveDocument;
                if (document == null)
                {
                    throw new AutomationException("No Active Document");
                }
            }

            var masters = document.Masters;
            var master = masters.AddEx(IVisio.VisMasterTypes.visTypeMaster);
            if (name != null)
            {
                master.Name = name;
            }

            return master;
        }
    }
}