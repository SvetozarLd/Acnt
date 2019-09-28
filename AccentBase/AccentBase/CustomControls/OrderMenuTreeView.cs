using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AccentBase.CustomControls
{
    class OrderMenuTreeView: TreeView
    {

        //OrderMenuTreeView()
        //{
        //    this.ow
        //}

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


    }
}
