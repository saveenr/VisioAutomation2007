using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using TREEMODEL = VisioAutomation.Models.Tree;

namespace VisioAutomation.Scripting.Commands
{
    public class DeveloperCommands : CommandSet
    {
        public DeveloperCommands(Client client) :
            base(client)
        {

        }

        public static List<System.Type> GetTypes()
        {
            // TODO: Consider filtering out types that should *not* be exposed despite being public
            var va_type = typeof(VisioAutomation.Application.ApplicationHelper);
            var vas_type = typeof (VisioAutomation.Scripting.CommandSet);

            var va_types = va_type.Assembly.GetExportedTypes().Where(t => t.IsPublic);
            var vas_types = vas_type.Assembly.GetExportedTypes().Where(t => t.IsPublic);
            
            var types = new List<System.Type>();
            types.AddRange(va_types);
            types.AddRange(vas_types);
            
            return types;
        }       

        public IVisio.Document DrawScriptingDocumentation()
        {
            this.AssertApplicationAvailable();

            var formdoc = new VA.Models.Forms.FormDocument();
            formdoc.Subject = "VisioAutomation.Scripting Documenation";
            formdoc.Title = "VisioAutomation.Scripting Documenation";
            formdoc.Creator = "";
            formdoc.Company = "";

            //docbuilder.BodyParaSpacingAfter = 6.0;
            var lines = new List<string>();

            var cmdst_props = VA.Scripting.Client.GetCommandSetProperties().OrderBy(i=>i.Name).ToList();
            var sb = new System.Text.StringBuilder();
            var helpstr = new System.Text.StringBuilder();

            foreach (var cmdset_prop in cmdst_props)
            {
                var cmdset_type = cmdset_prop.PropertyType;

                // Calculate the text
                var methods = CommandSet.GetCommandMethods(cmdset_type);
                lines.Clear();
                foreach (var method in methods)
                {
                    sb.Length = 0;
                    var method_params = method.GetParameters();
                    TextCommandsUtil.Join(sb, ", ", method_params.Select(param => string.Format("{0} {1}", ReflectionUtil.GetNiceTypeName(param.ParameterType), param.Name)));

                    if (method.ReturnType != typeof(void))
                    {
                        string line = string.Format("{0}({1}) -> {2}", method.Name, sb, ReflectionUtil.GetNiceTypeName(method.ReturnType));
                        lines.Add(line);
                    }
                    else
                    {
                        string line = string.Format("{0}({1})", method.Name, sb);
                        lines.Add(line);
                    }
                }

                lines.Sort();
                
                helpstr.Length = 0;
                TextCommandsUtil.Join(helpstr,"\r\n",lines);

                var formpage = new VisioAutomation.Models.Forms.FormPage();
                formpage.Title = cmdset_prop.Name + " commands";
                formpage.Body = helpstr.ToString();
                formpage.Name = cmdset_prop.Name + " commands";
                formpage.Size = new VA.Drawing.Size(8.5, 11);
                formpage.Margin = new VA.Drawing.Margin(0.5, 0.5, 0.5, 0.5);
                formdoc.Pages.Add(formpage);

            }


            //hide_ui_stuff(docbuilder.VisioDocument);

            var app = this.Client.VisioApplication;
            var doc = formdoc.Render(app);
            return doc;
        }

        public IVisio.Document DrawInteropEnumDocumentation()
        {
            this.AssertApplicationAvailable();
            
            var formdoc = new VA.Models.Forms.FormDocument();

            var helpstr = new System.Text.StringBuilder();
            int chunksize = 70;

            var interop_enums = VA.Interop.InteropHelper.GetEnums();
            int pagecount = 0;
            foreach (var enum_ in interop_enums)
            {
                int chunkcount = 0;

                var values = enum_.Values.OrderBy(i => i.Name).ToList();
                foreach (var chunk in Chunk(values, chunksize))
                {
                    helpstr.Length = 0;
                    foreach (var val in chunk)
                    {
                        helpstr.AppendFormat("0x{0}\t{1}\n", val.Value.ToString("x"),val.Name);
                    }

                    var formpage = new VA.Models.Forms.FormPage();
                    formpage.Size = new VA.Drawing.Size(8.5, 11);
                    formpage.Margin = new VA.Drawing.Margin(0.5, 0.5, 0.5, 0.5);
                    formpage.Title = enum_.Name;
                    formpage.Body = helpstr.ToString();
                    if (chunkcount == 0)
                    {
                        formpage.Name = string.Format("{0}", enum_.Name);
                    }
                    else
                    {
                        formpage.Name = string.Format("{0} ({1})", enum_.Name, chunkcount + 1);
                    }

                    //docbuilder.BodyParaSpacingAfter = 2.0;

                    formpage.BodyTextSize = 8.0;

                    formdoc.Pages.Add(formpage);
            

                    var tabstops = new[]
                                 {
                                     new VA.Text.TabStop(1.5, VA.Text.TabStopAlignment.Left)
                                 };

                    //VA.Text.TextFormat.SetTabStops(docpage.VisioBodyShape, tabstops);
                    
                    chunkcount++;
                    pagecount++;
                }
            }

            formdoc.Subject = "Visio Interop Enum Documenation";
            formdoc.Title = "Visio Interop Enum Documenation";
            formdoc.Creator = "";
            formdoc.Company = "";

            //hide_ui_stuff(docbuilder.VisioDocument);


            var doc = formdoc.Render(this.Client.VisioApplication);
            return doc;
        }

        private class PathTreeBuilder
        {
            public readonly Dictionary<string, string> PathToParentPath;
            public readonly List<string> Roots;
            public readonly string Separator;
            public readonly string[] seps;
            private System.StringSplitOptions options = System.StringSplitOptions.None;

            public PathTreeBuilder()
            {
                this.PathToParentPath = new Dictionary<string, string>();
                this.Roots = new List<string>();
                this.Separator = ".";
                this.seps = new[] {this.Separator};
            }

            public void Add(string path)
            {
                if (this.PathToParentPath.ContainsKey(path))
                {
                    return;
                }

                var tokens = path.Split(seps,options);

                if (tokens.Length == 0)
                {
                    throw new VA.Scripting.ScriptingException();
                }
                else if (tokens.Length == 1)
                {
                    string first = tokens[0];
                    this.Roots.Add(first);
                    this.PathToParentPath[first] = null;
                }
                else
                {
                    string parent_path = string.Join(this.Separator, tokens.Take(tokens.Length - 1));
                    this.Add(parent_path);
                    this.PathToParentPath[path] = parent_path;
                }   
            }

            public List<string> GetPaths()
            {
                return this.PathToParentPath.Keys.ToList();
            }
        }

        public IVisio.Document DrawNamespaces()
        {
            return this.DrawNamespaces(VA.Scripting.Commands.DeveloperCommands.GetTypes());
        }

        public IVisio.Document DrawNamespaces(IList<System.Type> types)
        {
            this.AssertApplicationAvailable();

            string def_linecolor = "rgb(140,140,140)";
            string def_fillcolor = "rgb(240,240,240)";
            string def_font = "Segoe UI";

            var doc = this.Client.Document.New(8.5,11,null);
            var fonts = doc.Fonts;
            var font = fonts[def_font];
            int fontid = font.ID16;

            var pathbuilder = new PathTreeBuilder();
            foreach (var type in types)
            {
                pathbuilder.Add(type.Namespace);
            }

            var namespaces = pathbuilder.GetPaths();

            var tree_layout = new TREEMODEL.Drawing();
            tree_layout.LayoutOptions.Direction = TREEMODEL.LayoutDirection.Right;
            tree_layout.LayoutOptions.ConnectorType = TREEMODEL.ConnectorType.CurvedBezier;
            var ns_node_map = new Dictionary<string, TREEMODEL.Node>(namespaces.Count);

            // create nodes for every namespace
            foreach (string ns in namespaces)
            {
                string label = ns;
                int index_of_last_sep = ns.LastIndexOf(pathbuilder.Separator);
                if (index_of_last_sep > 0)
                {
                    label = ns.Substring(index_of_last_sep+1);
                }

                var node = new TREEMODEL.Node(ns);
                node.Text = new VA.Text.Markup.TextElement(label);
                node.Size = new VA.Drawing.Size(2.0, 0.25);
                ns_node_map[ns] = node;
            }

            // add children to nodes
            foreach (string ns in namespaces)
            {
                var parent_ns = pathbuilder.PathToParentPath[ns];

                if (parent_ns != null)
                {
                    // the current namespace has a parent
                    var parent_node = ns_node_map[parent_ns];
                    var child_node = ns_node_map[ns];
                    parent_node.Children.Add(child_node);
                }
                else
                {
                    // that means this namespace is a root, forget about it
                }
            }

            if (pathbuilder.Roots.Count == 0)
            {
                
            }
            else if (pathbuilder.Roots.Count == 1)
            {
                // when there is exactly one root namespace, then that node will be the tree's root node
                var first_root = pathbuilder.Roots[0];
                var root_n = ns_node_map[first_root];
                tree_layout.Root = root_n;
            }
            else
            {
                // if there are multiple root namespaces, inject an empty placeholder root
                var root_n = new TREEMODEL.Node();
                tree_layout.Root = root_n;

                foreach (var root_ns in pathbuilder.Roots)
                {
                    var node = ns_node_map[root_ns];
                    tree_layout.Root.Children.Add(node);
                }
            }

            // format the shapes
            foreach (var node in tree_layout.Nodes)
            {
                if (node.Cells==null)
                {
                    node.Cells = new VA.DOM.ShapeCells();                    
                }
                node.Cells.FillForegnd = def_fillcolor;
                node.Cells.CharFont = fontid;
                node.Cells.LineColor = def_linecolor;
                node.Cells.ParaHorizontalAlign = "0";
            }

            var cxn_cells = new VA.DOM.ShapeCells();
            cxn_cells.LineColor = def_linecolor;
            tree_layout.LayoutOptions.ConnectorCells = cxn_cells;


            tree_layout.Render(doc.Application.ActivePage);

            hide_ui_stuff(doc);
            return doc;
        }

        public IList<VA.Interop.EnumType> GetInteropEnums()
        {
            return VA.Interop.InteropHelper.GetEnums();
        }

        public VA.Interop.EnumType GetInteropEnum(string name)
        {
            return VA.Interop.InteropHelper.GetEnum(name);
        }

        public VA.Interop.EnumType GetEnum(System.Type type)
        {
            return new VA.Interop.EnumType(type);
        }
        
        private static IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        private class TypeInfo
        {
            public readonly System.Type Type;
            public ReflectionUtil.TypeCategory TypeCategory ;
            public readonly string Label;

            public TypeInfo(System.Type type)
            {
                this.Type = type;
                this.TypeCategory = ReflectionUtil.GetTypeCategory(type);
                this.Label = ReflectionUtil.GetTypeCategoryDisplayString(type) + " " + ReflectionUtil.GetNiceTypeName(type);

            }
        }

        public IVisio.Document DrawNamespacesAndClasses()
        {
            return this.DrawNamespacesAndClasses(VA.Scripting.Commands.DeveloperCommands.GetTypes());
        }

        public IVisio.Document DrawNamespacesAndClasses(IList<System.Type> types_)
        {
            this.AssertApplicationAvailable();

            string segoeui_fontname = "Segoe UI";
            string segoeuilight_fontname = "Segoe UI Light";
            string def_linecolor = "rgb(180,180,180)";
            string def_shape_fill = "rgb(245,245,245)";

            var doc = this.Client.Document.New(8.5, 11,null);
            var fonts = doc.Fonts;
            var font_segoe = fonts[segoeui_fontname];
            var font_segoelight = fonts[segoeuilight_fontname];
            int fontid_segoe = font_segoe.ID16;
            int fontid_segoelight = font_segoelight.ID16;

            var types = types_.Select(t=>new TypeInfo(t));

            var pathbuilder = new PathTreeBuilder();
            foreach (var type in types)
            {
                pathbuilder.Add(type.Type.Namespace);
            }

            var namespaces = pathbuilder.GetPaths();

            var tree_layout = new TREEMODEL.Drawing();
            tree_layout.LayoutOptions.Direction = TREEMODEL.LayoutDirection.Down;
            tree_layout.LayoutOptions.ConnectorType = TREEMODEL.ConnectorType.PolyLine;
            var ns_node_map = new Dictionary<string, TREEMODEL.Node>(namespaces.Count);
            var node_to_nslabel = new Dictionary<TREEMODEL.Node, string>(namespaces.Count);

            // create nodes for every namespace
            foreach (string ns in namespaces)
            {
                string label = ns;
                int index_of_last_sep = ns.LastIndexOf(pathbuilder.Separator);
                if (index_of_last_sep > 0)
                {
                    label = ns.Substring(index_of_last_sep + 1);
                }

                string ns1 = ns;
                var types_in_namespace = types.Where(t => t.Type.Namespace == ns1)
                    .OrderBy(t=>t.Type.Name)
                    .Select(t=> t.Label);
                var node = new TREEMODEL.Node(ns);
                node.Size = new VA.Drawing.Size(2.0, (0.15) * (1 + 2 + types_in_namespace.Count()));


                var markup = new VA.Text.Markup.TextElement();
                var m1 = markup.AddElement(label+"\n");
                m1.CharacterCells.Font = fontid_segoe;
                m1.CharacterCells.Size = "12.0pt";
                m1.CharacterCells.Style = "1"; // Bold
                var m2 = markup.AddElement();
                m2.CharacterCells.Font = fontid_segoe;
                m2.CharacterCells.Size = "8.0pt";
                m2.AddText(string.Join("\n", types_in_namespace));

                node.Text = markup;

                ns_node_map[ns] = node;
                node_to_nslabel[node] = label;
            }

            // add children to nodes
            foreach (string ns in namespaces)
            {
                var parent_ns = pathbuilder.PathToParentPath[ns];

                if (parent_ns != null)
                {
                    // the current namespace has a parent
                    var parent_node = ns_node_map[parent_ns];
                    var child_node = ns_node_map[ns];
                    parent_node.Children.Add(child_node);
                }
                else
                {
                    // that means this namespace is a root, forget about it
                }
            }

            if (pathbuilder.Roots.Count == 0)
            {

            }
            else if (pathbuilder.Roots.Count == 1)
            {
                // when there is exactly one root namespace, then that node will be the tree's root node
                var first_root = pathbuilder.Roots[0];
                var root_n = ns_node_map[first_root];
                tree_layout.Root = root_n;
            }
            else
            {
                // if there are multiple root namespaces, inject an empty placeholder root
                var root_n = new TREEMODEL.Node();
                tree_layout.Root = root_n;

                foreach (var root_ns in pathbuilder.Roots)
                {
                    var node = ns_node_map[root_ns];
                    tree_layout.Root.Children.Add(node);
                }
            }

            // format the shapes
            foreach (var node in tree_layout.Nodes)
            {
                if (node.Cells == null)
                {
                    node.Cells = new VA.DOM.ShapeCells();
                }
                node.Cells.FillForegnd = def_shape_fill;
                //node.ShapeCells.LineWeight = "0";
                //node.ShapeCells.LinePattern = "0";
                node.Cells.LineColor = def_linecolor;
                node.Cells.ParaHorizontalAlign = "0";
                node.Cells.VerticalAlign = "0";
            }

            var cxn_cells = new VA.DOM.ShapeCells();
            cxn_cells.LineColor = def_linecolor;
            tree_layout.LayoutOptions.ConnectorCells = cxn_cells;
            tree_layout.Render(doc.Application.ActivePage);

            hide_ui_stuff(doc);

            return doc;
        }

        private static void hide_ui_stuff(IVisio.Document doc)
        {
            var app = doc.Application;
            var active_window = app.ActiveWindow;
            active_window.ShowGrid = 0;
            active_window.ShowPageBreaks = 0;
            active_window.ShowGuides = 0;
        }
    }
}
