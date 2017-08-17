using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioAutomation.Scripting.Commands
{
    public class DocumentCommands : CommandSet
    {
        public DocumentCommands(Client client) :
            base(client)
        {

        }

        public void Activate(string name)
        {
            this.AssertApplicationAvailable();

            var application = this.Client.VisioApplication;
            var documents = application.Documents;
            var doc = documents[name];

            this.Activate(doc);
        }

        public void Activate(IVisio.Document doc)
        {
            this.AssertApplicationAvailable();
            VA.Documents.DocumentHelper.Activate(doc);
        }

        public void Close(bool force)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var application = this.Client.VisioApplication;
            var doc = application.ActiveDocument;

            if (doc.Type != IVisio.VisDocumentTypes.visTypeDrawing)
            {
                this.Client.WriteVerbose("Not a Drawing Window", doc.Name);
                throw new VA.AutomationException("Not a Drawing Window");
            }

            this.Client.WriteVerbose( "Closing Document Name=\"{0}\"", doc.Name);
            this.Client.WriteVerbose( "Closing Document FullName=\"{0}\"", doc.FullName);

            if (force)
            {
                using (var alert = new VA.Application.AlertResponseScope(application, VA.Application.AlertResponseCode.No))
                {
                    doc.Close();
                }
            }
            else
            {
                doc.Close();
            }
        }

        public void CloseAllWithoutSaving()
        {
            this.AssertApplicationAvailable();
            var application = this.Client.VisioApplication;
            var documents = application.Documents;
            var docs = documents.AsEnumerable().Where(doc => doc.Type == IVisio.VisDocumentTypes.visTypeDrawing).ToList();

            using (var alert = new VA.Application.AlertResponseScope(application, VA.Application.AlertResponseCode.No))
            {
                foreach (var doc in docs)
                {
                    this.Client.WriteVerbose( "Closing Document Name=\"{0}\"", doc.Name);
                    this.Client.WriteVerbose( "Closing Document FullName=\"{0}\"", doc.FullName);
                    doc.Close();
                }
            }
        }

        public IVisio.Document New()
        {
            return this.New(null);
        }

        public IVisio.Document New(string template)
        {
            this.AssertApplicationAvailable();

            this.Client.WriteVerbose("Creating Empty Drawing");
            var application = this.Client.VisioApplication;
            var documents = application.Documents;
            
            if (template == null)
            {
                var doc = documents.Add(string.Empty);
                return doc;
            }
            else
            {

                var doc = documents.Add(string.Empty);
                var template_doc = documents.AddEx(template, IVisio.VisMeasurementSystem.visMSDefault,
                              (int)IVisio.VisOpenSaveArgs.visAddStencil +
                              (int)IVisio.VisOpenSaveArgs.visOpenDocked,
                              0);
                return doc;
            }
        }

        public void Save()
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();
            
            var application = this.Client.VisioApplication;
            var doc = application.ActiveDocument;
            doc.Save();
        }

        public void SaveAs(string filename)
        {
            this.AssertApplicationAvailable();
            this.AssertDocumentAvailable();

            var application = this.Client.VisioApplication;
            var doc = application.ActiveDocument;
            doc.SaveAs(filename);
        }

        public IVisio.Document New(double w, double h)
        {
            return this.New(w, h, null);
        }

        public IVisio.Document New(double w, double h,string template)
        {
            this.AssertApplicationAvailable();

            var doc = this.New(template);
            var pagesize = new VA.Drawing.Size(w, h);
            this.Client.Page.SetSize(pagesize);
            return doc;
        }

        public IVisio.Document OpenStencil(string name)
        {
            this.AssertApplicationAvailable();
            
            if (name == null)
            {
                throw new System.ArgumentNullException("name");
            }

            if (name.Length == 0)
            {
                throw new System.ArgumentException("name");
            }

            this.Client.WriteVerbose( "Loading stencil \"{0}\"", name);

            var application = this.Client.VisioApplication;
            var documents = application.Documents;
            var doc = documents.OpenStencil(name);

            this.Client.WriteVerbose( "Finished loading stencil \"{0}\"", name);
            return doc;
        }

        public IVisio.Document Open(string filename)
        {
            this.AssertApplicationAvailable();
            
            if (filename == null)
            {
                throw new System.ArgumentNullException(filename);
            }

            if (filename.Length == 0)
            {
                throw new System.ArgumentException(filename);
            }

            string abs_filename = System.IO.Path.GetFullPath(filename);

            this.Client.WriteVerbose( "Input filename: {0}", filename);
            this.Client.WriteVerbose( "Absolute filename: {0}", abs_filename);

            if (!System.IO.File.Exists(abs_filename))
            {
                throw new System.ArgumentException("File does not exist", "filename");
            }

            var application = this.Client.VisioApplication;
            var documents = application.Documents;
            var doc = documents.Add(filename);
            return doc;
        }


        public IVisio.Document Get(string name)
        {
            this.AssertApplicationAvailable();
            
            var application = this.Client.VisioApplication;
            var documents = application.Documents;
            var doc = documents[name];
            return doc;
        }

        public List<IVisio.Document> GetDocumentsByName(string name)
        {
            var documents = this.Client.VisioApplication.Documents;
            if (name == null || name == "*")
            {
                // return all documents
                var docs1 = documents.AsEnumerable().ToList();
                return docs1;
            }

            // get the named document
            var docs2 = VA.TextUtil.FilterObjectsByNames(documents.AsEnumerable(), new[] {name}, d => d.Name, true, VA.TextUtil.FilterAction.Include).ToList();
            return docs2;
        }
    }
}