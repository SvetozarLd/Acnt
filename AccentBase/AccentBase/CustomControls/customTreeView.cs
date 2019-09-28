using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

namespace AccentBase.CustomControls
{
    class customTreeView : TreeView
    {
        #region для скролинга для тривью
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        #endregion

        #region Component Designer
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer_ForScrolling = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer_ForScrolling
            // 
            this.timer_ForScrolling.Interval = 200;
            this.timer_ForScrolling.Tick += new System.EventHandler(this.timer_ForScrolling_Tick);
            // 
            // customTreeView
            // 
            this.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.customTreeView_BeforeCollapse);
            this.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.customTreeView_BeforeExpand);
            this.ResumeLayout(false);

        }
        #endregion




        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }







        private Timer timer_ForScrolling;
        private IContainer components;

        public customTreeView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitComponent call

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            SelectedNodeName = e.Node.Name;
            SelectedNodeId = Convert.ToInt32(e.Node.Name);
            SelectedNodeText = e.Node.Text;
            base.OnAfterSelect(e);
        }

        #region DataBinding

        #region подключение DataTable
        private DataTable nodesTable = null; //Эталонная БД 


        private HashSet<int> checkedIds = new HashSet<int>();

        public DataTable NodesTable
        {
            get
            {
                return nodesTable;
            }
            set
            {

                if (value != null)
                {
                    DateTime start = DateTime.Now;
                    //if (expandedNodes != null && expandedNodes.Count > 0) { expandedNodes.Clear(); }
                    nodesTable = new DataTable();
                    value.DefaultView.RowFilter = string.Empty;
                    nodesTable = value.DefaultView.ToTable();
                    nodesFilterTable = new DataTable();
                    nodesTable.DefaultView.RowFilter = nodesFilter;
                    nodesFilterTable = value.DefaultView.ToTable();
                    bindTreeView();
                    TimeSpan elapsed = DateTime.Now.Subtract(start);
                    Trace.WriteLine(elapsed.TotalMilliseconds.ToString());
                }

            }
        }

        public HashSet<int> CheckedList
        {
            get
            {
                return checkedIds;
            }
            set
            {
                if (value != null)
                {
                    checkedIds = new HashSet<int>();
                    checkedIds = value;
                    bindTreeView();
                }
            }
        }

        public enum CheckNodesEnum
        {
            CheckAll = 0,
            CheckOff = 1,
            CheckRandom = 2
        }
        #region Check/Uncheck Nodes
        private CheckNodesEnum CheckNodes = CheckNodesEnum.CheckAll;
        public void CheckAllNodes(CheckNodesEnum cn)
        {
            CheckNodes = cn;
            CheckNodesStart(cn);
        }

        private void CheckNodesStart(CheckNodesEnum cn)
        {
            foreach (TreeNode tn in Nodes)
            {
                switch (cn)
                {
                    case CheckNodesEnum.CheckAll:
                        tn.Checked = true;

                        break;
                    case CheckNodesEnum.CheckOff:
                        tn.Checked = false;

                        break;
                }
                CheckNodesRecoursive(cn, tn);
            }
        }
        private void CheckNodesRecoursive(CheckNodesEnum cn, TreeNode parent)
        {
            foreach (TreeNode tn in parent.Nodes)
            {
                switch (cn)
                {
                    case CheckNodesEnum.CheckAll:
                        tn.Checked = true;
                        break;
                    case CheckNodesEnum.CheckOff:
                        tn.Checked = false;
                        break;
                }
                CheckNodesRecoursive(cn, tn);
            }
        }

        #endregion



        #endregion
        #region Фильтр
        private DataTable nodesFilterTable = null; //Отфильтрованная БД
        private string nodesFilter = string.Empty; //Строка - фильтр
        private DataTable nodesFilterTableHash = null;
        public string NodesFilter
        {
            get
            {
                return nodesFilter;
            }
            set
            {
                nodesFilter = value;
                if (nodesTable != null)
                {

                    nodesFilterTable = new DataTable();
                    nodesTable.DefaultView.RowFilter = nodesFilter;
                    nodesFilterTable = nodesTable.DefaultView.ToTable();
                    nodesFilterTableHash = new DataTable();
                    nodesFilterTableHash = nodesFilterTable.DefaultView.ToTable();
                    if (nodesFilter != string.Empty && nodesFilterTable.Rows.Count > 0)
                    {
                        int parentId = 0;
                        foreach (DataRow dr in nodesFilterTable.Rows)
                        {
                            parentId = Convert.ToInt32(dr["id_parent"]);
                            if (parentId > 0)
                            {
                                recursivesearch(parentId);
                            }
                        }
                        nodesFilterTable = new DataTable();
                        nodesFilterTable = nodesFilterTableHash.DefaultView.ToTable();
                        nodesFilterTableHash.Clear();
                    }
                    bindTreeView();
                }
            }
        }

        private void recursivesearch(int ParentId)
        {

            if (ParentId > 0)
            {
                DataRow result = nodesTable.Select("id = " + ParentId.ToString()).SingleOrDefault();
                DataRow result2 = nodesFilterTableHash.Select("id = " + ParentId.ToString()).SingleOrDefault();
                if (result != null)
                {
                    if (result2 == null) { nodesFilterTableHash.Rows.Add(result.ItemArray); }
                    recursivesearch(Convert.ToInt32(result["id_parent"]));
                }
            }
        }

        #endregion
        #region Binding
        //public string SelectedNodeName = string.Empty; 
        private void bindTreeView()
        {
            SetVisibleCore(false);
            BlockAutoCheck = true;
            DataRowsToNodes();
            //DataRowsToNodes(SelectedNodeName);
            if (SelectedNode != null) { SelectedNode.EnsureVisible(); }
            BlockAutoCheck = false;
            Sort();
            SetVisibleCore(true);

        }



        internal void DataRowsToNodes()
        {
            if (Nodes.Count > 0) { Nodes.Clear(); }
            string tmp = string.Empty;
            if (nodesFilterTable != null && nodesFilterTable.Rows.Count > 0)
            {
                DataRow[] rows = nodesFilterTable.Select("id_parent = 0");
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        TreeNode tn = new TreeNode();
                        tn.Name = row["id"].ToString();
                        tn.Text = row["name"].ToString();
                        tmp = row["comment"].ToString();
                        if (tmp.Trim() == string.Empty)
                        {
                            tn.ToolTipText = tn.Text;
                        }
                        else
                        {
                            tn.ToolTipText = row["comment"].ToString();
                        }
                        tn.Tag = row["id_parent"].ToString();
                        if (this.CheckBoxes)
                        {
                            switch (CheckNodes)
                            {
                                case CheckNodesEnum.CheckAll:
                                    tn.Checked = true;
                                    if (!checkedIds.Contains(Convert.ToInt32(tn.Name))) { checkedIds.Add(Convert.ToInt32(tn.Name)); }
                                    OnAfterCheck(new TreeViewEventArgs(tn));
                                    break;
                                case CheckNodesEnum.CheckOff:
                                    tn.Checked = false;
                                    if (checkedIds.Contains(Convert.ToInt32(tn.Name))) { checkedIds.Remove(Convert.ToInt32(tn.Name)); }
                                    OnAfterCheck(new TreeViewEventArgs(tn));
                                    break;
                                case CheckNodesEnum.CheckRandom:
                                    if (checkedIds != null && checkedIds.Contains(Convert.ToInt32(row["id"]))) { tn.Checked = true; OnAfterCheck(new TreeViewEventArgs(tn)); }
                                    break;
                            }
                        }
                        Nodes.Add(tn);
                        if (SelectedNodeName != string.Empty && tn.Name == SelectedNodeName) { SelectedNode = tn; }
                        recursiveReadNode(Convert.ToInt32(row["id"]), tn);
                        if (expandedNodes.Contains(tn.Name)) { tn.Expand(); }
                    }
                }
            }
            if (SelectedNode == null && Nodes.Count > 0) { SelectedNode = Nodes[0]; }
        }



        private void recursiveReadNode(int parentid, TreeNode parentNode)
        {
            DataRow[] rows = nodesFilterTable.Select("id_parent = " + parentid);
            if (rows.Length > 0)
            {
                string tmp = string.Empty;
                foreach (DataRow row in rows)
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = row["id"].ToString();
                    tn.Text = row["name"].ToString();
                    tmp = row["comment"].ToString();
                    if (tmp.Trim() == string.Empty)
                    {
                        tn.ToolTipText = tn.Text;
                    }
                    else
                    {
                        tn.ToolTipText = row["comment"].ToString();
                    }
                    tn.Tag = row["id_parent"].ToString();
                    if (this.CheckBoxes)
                    {
                        switch (CheckNodes)
                        {
                            case CheckNodesEnum.CheckAll:
                                tn.Checked = true;
                                if (!checkedIds.Contains(Convert.ToInt32(tn.Name))) { checkedIds.Add(Convert.ToInt32(tn.Name)); }
                                OnAfterCheck(new TreeViewEventArgs(tn));
                                break;
                            case CheckNodesEnum.CheckOff:
                                tn.Checked = false;
                                if (checkedIds.Contains(Convert.ToInt32(tn.Name))) { checkedIds.Remove(Convert.ToInt32(tn.Name)); }
                                OnAfterCheck(new TreeViewEventArgs(tn));
                                break;
                            case CheckNodesEnum.CheckRandom:
                                if (checkedIds != null && checkedIds.Contains(Convert.ToInt32(row["id"]))) { tn.Checked = true; OnAfterCheck(new TreeViewEventArgs(tn)); }
                                break;
                        }
                    }
                    parentNode.Nodes.Add(tn);

                    if (SelectedNodeName != string.Empty && tn.Name == SelectedNodeName) { SelectedNode = tn; }
                    recursiveReadNode(Convert.ToInt32(row["id"]), tn);
                    if (expandedNodes.Contains(tn.Name)) { tn.Expand(); }
                }
            }
        }
        #endregion

        #endregion

        #region Selected Node
        int SelectedNodeId = 0;
        string SelectedNodeName = string.Empty;
        string SelectedNodeText = string.Empty;

        public int SelectedNode_id
        {
            get { return SelectedNodeId; }
            set
            {
                SelectedNodeId = value;
                SelectedNodeName = value.ToString();
                SelectNodeById(Nodes);
            }
        }
        public string SelectedNode_Name
        {
            get { return SelectedNodeText; }
            set
            {
                SelectedNodeName = value;
                SelectNodeById(Nodes);
            }

        }

        bool node_found = false;

        TreeNode FoundNode = null;
        private TreeNode SelectNodeById(TreeNodeCollection nodes)
        {
            if (node_found) { return FoundNode; }
            foreach (TreeNode node in nodes)
            {
                if (node.Name.Equals(SelectedNodeName))
                {
                    node_found = true;
                    FoundNode = node;
                    SelectedNode = node;
                    SelectedNode.EnsureVisible();
                }
                if (!node_found)
                {
                    FoundNode = SelectNodeById(node.Nodes);
                }
            }
            return FoundNode;
        }
        private TreeNode SelectNodeByName(TreeNodeCollection nodes)
        {
            if (node_found) { return FoundNode; }
            foreach (TreeNode node in nodes)
            {
                if (node.Text.Equals(SelectedNodeText))
                {
                    node_found = true;
                    FoundNode = node;
                    SelectedNode = node;
                    SelectedNode.EnsureVisible();
                }
                if (!node_found)
                {
                    FoundNode = SelectNodeById(node.Nodes);
                }
            }
            return FoundNode;
        }
        #endregion

        #region OverRide Events
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (HitTest(e.Location).Node != null) { SelectedNode = HitTest(e.Location).Node; SelectedNodeName = SelectedNode.Name; } else { SelectedNode = null; SelectedNodeName = string.Empty; }
            base.OnMouseDown(e);
            base.OnAfterSelect(new TreeViewEventArgs(SelectedNode));
        }
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            Point clientPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
            //Trace.WriteLine(drgevent.X.ToString() + "X" + drgevent.Y.ToString() + ": " + scrollingTimer.ToString());
            if (scrollingTimer)
            {
                // See where the cursor is
                //Point pt = treeView_Customers.PointToClient(Cursor.Position);

                // See if we need to scroll up or down
                if ((clientPoint.Y + scrollRegion) > (Height - 5))
                {
                    // Call the API to scroll down
                    SendMessage(Handle, (int)277, (int)1, 0);
                    scrollingTimer = false;
                    timer_ForScrolling.Start();
                }
                else if ((clientPoint.Y + 10) < (Top + scrollRegion))
                {
                    // Call thje API to scroll up
                    SendMessage(Handle, (int)277, (int)0, 0);
                    scrollingTimer = false;
                    timer_ForScrolling.Start();
                }
            }
            base.OnDragOver(drgevent);
        }
        //#region таймер для скроллинга - для задержки автоскроллинга при Drag&Drop
        bool scrollingTimer = true; //флаг - проверять ли необходимость автопрокрутки 
        const Single scrollRegion = 20; // размер области для автоскроллинга
        //bool blockTreeviewFilter = false;
        //Timer timer_ForScrolling = new Timer();

        private void timer_ForScrolling_Tick(object sender, EventArgs e)
        {
            //timer_ForScrolling.Interval = 200;
            scrollingTimer = true;
            timer_ForScrolling.Stop();
        }


        bool BlockAutoCheck = true;
        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            if (!BlockAutoCheck)
            {
                CheckNodes = CheckNodesEnum.CheckRandom;
                if (checkedIds == null) { checkedIds = new HashSet<int>(); }
                if (e.Node.Checked)
                {
                    if (!checkedIds.Contains(Convert.ToInt32(e.Node.Name))) { checkedIds.Add(Convert.ToInt32(e.Node.Name)); }
                }
                else
                {
                    if (checkedIds.Contains(Convert.ToInt32(e.Node.Name))) { checkedIds.Remove(Convert.ToInt32(e.Node.Name)); }
                }
            }
            base.OnAfterCheck(e);
        }

        #endregion

        #region Get Children List
        public Dictionary<int, string> GetAllChildren(int parentuid)
        {
            Dictionary<int, string> Listresult = new Dictionary<int, string>();
            Listresult = GetAllChildsId(parentuid, Listresult);
            return Listresult;
        }

        private Dictionary<int, string> GetAllChildsId(int parentuid, Dictionary<int, string> Id_by_All_Child)
        {
            DataRow[] rows = nodesFilterTable.Select("id_parent = " + parentuid);
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    Id_by_All_Child.Add(Convert.ToInt32(row["id"]), row["name"].ToString());
                    Id_by_All_Child = GetAllChildsId(Convert.ToInt32(row["id"]), Id_by_All_Child);
                }
            }
            return Id_by_All_Child;
        }
        #endregion

        #region Get string of checked nodes
        public string StringOfCheckedNodes
        {
            get
            {
                string val = ";";
                val += String.Join(";", CheckBoxesList());
                if (val.Length > 1) { val += ";"; }
                return val;
            }
            set
            {
                if (value != string.Empty)
                {
                    List<string> val = value.Split(';').ToList<string>();
                    val.RemoveAll(String.IsNullOrEmpty);
                    checkedIds.Clear();
                    foreach (string i in val)
                    {
                        checkedIds.Add(Convert.ToInt32(i));
                    }
                    CheckNodes = CheckNodesEnum.CheckRandom;
                    bindTreeView();
                }
            }
        }

        private HashSet<string> CheckBoxesList()
        {
            HashSet<string> hs = new HashSet<string>();
            foreach (TreeNode tn in Nodes)
            {
                if (tn.Checked) { hs.Add(tn.Name); }
                CheckBoxesListRecoursive(hs, tn);
            }
            return hs;
        }
        private HashSet<string> CheckBoxesListRecoursive(HashSet<string> hs, TreeNode parent)
        {
            foreach (TreeNode tn in parent.Nodes)
            {
                if (tn.Checked) { hs.Add(tn.Name); }
                CheckBoxesListRecoursive(hs, tn);
            }
            return hs;
        }
        #endregion

        #region Get string of parent nodes to root
        public string StringOfParentsNodes
        {
            get
            {

                string val = ";";
                val += String.Join(";", ParentsList());
                if (val.Length > 1) { val += ";"; }
                return val;
            }
        }
        private HashSet<string> ParentsList()
        {
            HashSet<string> hs = new HashSet<string>();
            if (SelectedNode != null && SelectedNode.Parent != null)
            {
                hs.Add(SelectedNode.Parent.Name);
                ParentsListRecoursive(hs, SelectedNode.Parent);
            }
            return hs;
        }
        private HashSet<string> ParentsListRecoursive(HashSet<string> hs, TreeNode currentNode)
        {
            if (currentNode.Parent != null)
            {
                hs.Add(currentNode.Parent.Name);
                ParentsListRecoursive(hs, currentNode.Parent);
            }
            return hs;
        }
        #endregion

        #region Memory of expanded nodes
        private HashSet<string> expandedNodes = new HashSet<string>();
        private void customTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (expandedNodes.Contains(e.Node.Name)) { expandedNodes.Remove(e.Node.Name); }
        }

        private void customTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!expandedNodes.Contains(e.Node.Name)) { expandedNodes.Add(e.Node.Name); }
        }
        #endregion

    }
}

