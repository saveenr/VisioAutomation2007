﻿using System.Windows.Forms;
using VA = VisioAutomation;

namespace VisioPowerTools
{
    public partial class FormExportSelectionAsFormat : Form
    {
        public enum enumExportFormat
        {
            ExportSVGXHTML,
            ExportXAML
        }

        public enumExportFormat ExportFormat = enumExportFormat.ExportSVGXHTML;

        public FormExportSelectionAsFormat( enumExportFormat f)
        {
            InitializeComponent();

            var client = VisioPowerToolsAddIn.Client;

            if (!client.HasActiveDocument)
            {
                MessageBox.Show("There is no drawing open to export");
            }

            string ext = null;

            this.ExportFormat = f;
            switch (this.ExportFormat)
            {
                case (enumExportFormat.ExportSVGXHTML) :
                    {
                        ext = ".xhtml";
                        this.labelFormatChoice.Text = "SVG + XHTML";
                        break;
                    }
                case (enumExportFormat.ExportXAML) :
                    {

                        ext = ".xaml";
                        this.labelFormatChoice.Text = "XAML";
                        break;
                    }
                default :
                    {
                        throw new VA.AutomationException("Unsupported format");
                    }
            }

            var application = client.VisioApplication;
            var doc = application.ActiveDocument;           
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var page = application.ActivePage;
            var name = doc.Name + "_" + page.Name + ext;
            var fullname = System.IO.Path.Combine(path, name);
            this.filenamePicker1.Filename = fullname;

            this.labelDocumentName.Text = doc.Name;
            this.labelPageName.Text = page.NameU;
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            string filename = this.filenamePicker1.Filename;

            var path = System.IO.Path.GetDirectoryName(filename);
            if (!System.IO.Directory.Exists(path))
            {
                MessageBox.Show("Output folder does not exist");
                return;
            }
            var client = VisioPowerToolsAddIn.Client;
            if (this.ExportFormat == enumExportFormat.ExportSVGXHTML)
            {
                client.Export.SelectionToSVGXHTML(filename);                
            }
            else if (this.ExportFormat == enumExportFormat.ExportXAML)
            {
                client.Export.ExportSelectionToXAML(filename);
            }
            else
            {
                throw new VA.AutomationException("Unsupported format");
            }
            this.Close();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
