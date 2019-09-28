namespace AccentBase.Forms
{
    partial class FormBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBase));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Черновики", 10, 10);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Ожидают", 1, 1);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("В работе");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Постобработка");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Сделаны (склад)");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Открыты", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Закрыты (архив)");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Остановлены");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Все заявки");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Корзина");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu_Orders = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_NewOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_OrderCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_OrderEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_OrderDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_OrderPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_OrderPdfSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_OrderFilesDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_OrderExit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_AppExit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_ToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_SocketSend = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_TreeViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_TreeViewAllOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuView_TreeViewDraft = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_TreeViewAwait = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_TreeViewInWork = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_TreeViewPostProc = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_TreeViewStock = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_TreeViewStopped = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuView_TreeViewArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuView_Orders = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderTimeStart = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderTimeEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderWorkTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderManager = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderCustomer = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuView_QuickSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_OrderListInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_Worktypes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuView_Messenger = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_FtpList = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_Stock = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView_GoogleTable = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuService = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuService_SocketServerTransmite = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuService_FtpTransmite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuService__Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuService_WinCalc = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаAccentBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_CheckUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_Orders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_OrderSave = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Print = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_InstallPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_PrintON = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_CutON = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_CncON = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_InstallON = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_Await = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_InWork = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_PostProc = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Stock = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Closed = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_ChangeState = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_DraftDouble = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_AwaiteDouble = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_InworkDouble = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_PostProcDouble = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_StockDouble = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_ClosedDouble = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator_Stop = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_Stoped = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_Basket = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox_QuickSearch_Customers = new System.Windows.Forms.PictureBox();
            this.panel_QuickSearch = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox_QuickSearch = new System.Windows.Forms.TextBox();
            this.toolStrip_Main = new System.Windows.Forms.ToolStrip();
            this.Button_OrderNew = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderCopy = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderEdit = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_OrderPrint = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderPdfSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_OrderFilesDownload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_FtpFiles = new System.Windows.Forms.ToolStripButton();
            this.Button_Messenger = new System.Windows.Forms.ToolStripButton();
            this.Button_Stock = new System.Windows.Forms.ToolStripButton();
            this.Button_GoogleTable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Calc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator25 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Settings = new System.Windows.Forms.ToolStripButton();
            this.Button_Help = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.TabOrder = new System.Windows.Forms.TabPage();
            this.panel_Center = new System.Windows.Forms.Panel();
            this.customDataGridView_Orders = new AccentBase.CustomControls.OrdersDataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.String_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Datetime_date_start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.String_date_start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Datetime_dead_line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.String_dead_line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.work_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image_WorkTypes = new System.Windows.Forms.DataGridViewImageColumn();
            this.Materials = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time_recieve = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip_OrderDatagridview = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.splitter_SocketSend = new System.Windows.Forms.Splitter();
            this.dataGridView_SocketSend = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter_Right = new System.Windows.Forms.Splitter();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.panel_OrderAbout = new System.Windows.Forms.Panel();
            this.groupBox_OrderInfo = new System.Windows.Forms.GroupBox();
            this.richTextBoxEx1 = new AccentBase.CustomControls.RichTextBoxEx();
            this.splitter_Preview = new System.Windows.Forms.Splitter();
            this.panel_Preview = new System.Windows.Forms.Panel();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.splitter_Left = new System.Windows.Forms.Splitter();
            this.panel_Left = new System.Windows.Forms.Panel();
            this.panel_History = new System.Windows.Forms.Panel();
            this.groupBox_History = new System.Windows.Forms.GroupBox();
            this.dataGridView_History = new System.Windows.Forms.DataGridView();
            this.uid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Datetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter_History = new System.Windows.Forms.Splitter();
            this.panel_Treeview = new System.Windows.Forms.Panel();
            this.orderMenuTreeView1 = new AccentBase.CustomControls.OrderMenuTreeView();
            this.toolStrip_Worktypes = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.Button_OrderViewAll = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderViewPrint = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderViewCut = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderViewCnc = new System.Windows.Forms.ToolStripButton();
            this.Button_OrderViewInstall = new System.Windows.Forms.ToolStripButton();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.contextMenuStrip_treeview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Treeview_newOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_ClearBasket = new System.Windows.Forms.ToolStripMenuItem();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filling = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewRichTextBoxColumn1 = new AccentBase.CustomControls.DataGridViewRichTextBoxColumn();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip_Orders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_QuickSearch_Customers)).BeginInit();
            this.panel_QuickSearch.SuspendLayout();
            this.toolStrip_Main.SuspendLayout();
            this.TabOrder.SuspendLayout();
            this.panel_Center.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView_Orders)).BeginInit();
            this.toolStrip_OrderDatagridview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SocketSend)).BeginInit();
            this.panel_Right.SuspendLayout();
            this.panel_OrderAbout.SuspendLayout();
            this.groupBox_OrderInfo.SuspendLayout();
            this.panel_Preview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.panel_Left.SuspendLayout();
            this.panel_History.SuspendLayout();
            this.groupBox_History.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_History)).BeginInit();
            this.panel_Treeview.SuspendLayout();
            this.toolStrip_Worktypes.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.contextMenuStrip_treeview.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 646);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1405, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Image = global::AccentBase.Properties.Resources.taskend;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(157, 17);
            this.toolStripStatusLabel1.Text = "Подключение к серверу";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Orders,
            this.Menu_View,
            this.MenuService,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1405, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu_Orders
            // 
            this.Menu_Orders.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_NewOrder,
            this.Menu_OrderCopy,
            this.Menu_OrderEdit,
            this.Menu_OrderDelete,
            this.toolStripSeparator6,
            this.Menu_OrderPrint,
            this.Menu_OrderPdfSave,
            this.toolStripSeparator16,
            this.Menu_OrderFilesDownload,
            this.toolStripSeparator17,
            this.Menu_OrderExit,
            this.Menu_AppExit});
            this.Menu_Orders.Name = "Menu_Orders";
            this.Menu_Orders.Size = new System.Drawing.Size(64, 20);
            this.Menu_Orders.Text = "Главное";
            // 
            // Menu_NewOrder
            // 
            this.Menu_NewOrder.Image = global::AccentBase.Properties.Resources.page_white_add;
            this.Menu_NewOrder.Name = "Menu_NewOrder";
            this.Menu_NewOrder.Size = new System.Drawing.Size(253, 22);
            this.Menu_NewOrder.Text = "Новое задание";
            this.Menu_NewOrder.Click += new System.EventHandler(this.NewOrder_Click);
            // 
            // Menu_OrderCopy
            // 
            this.Menu_OrderCopy.Enabled = false;
            this.Menu_OrderCopy.Image = global::AccentBase.Properties.Resources.page_white_copy;
            this.Menu_OrderCopy.Name = "Menu_OrderCopy";
            this.Menu_OrderCopy.Size = new System.Drawing.Size(253, 22);
            this.Menu_OrderCopy.Text = "Копия задания";
            // 
            // Menu_OrderEdit
            // 
            this.Menu_OrderEdit.Enabled = false;
            this.Menu_OrderEdit.Image = global::AccentBase.Properties.Resources.page_white_edit;
            this.Menu_OrderEdit.Name = "Menu_OrderEdit";
            this.Menu_OrderEdit.Size = new System.Drawing.Size(253, 22);
            this.Menu_OrderEdit.Text = "Открыть задание";
            // 
            // Menu_OrderDelete
            // 
            this.Menu_OrderDelete.Enabled = false;
            this.Menu_OrderDelete.Image = global::AccentBase.Properties.Resources.page_white_delete;
            this.Menu_OrderDelete.Name = "Menu_OrderDelete";
            this.Menu_OrderDelete.Size = new System.Drawing.Size(253, 22);
            this.Menu_OrderDelete.Text = "Удалить задание";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(250, 6);
            // 
            // Menu_OrderPrint
            // 
            this.Menu_OrderPrint.Image = global::AccentBase.Properties.Resources.printer;
            this.Menu_OrderPrint.Name = "Menu_OrderPrint";
            this.Menu_OrderPrint.Size = new System.Drawing.Size(253, 22);
            this.Menu_OrderPrint.Text = "Печать заявки задания";
            this.Menu_OrderPrint.Click += new System.EventHandler(this.Button_OrderPrint_Click);
            // 
            // Menu_OrderPdfSave
            // 
            this.Menu_OrderPdfSave.Image = global::AccentBase.Properties.Resources.page_white_acrobat_put;
            this.Menu_OrderPdfSave.Name = "Menu_OrderPdfSave";
            this.Menu_OrderPdfSave.Size = new System.Drawing.Size(253, 22);
            this.Menu_OrderPdfSave.Text = "Сохранить бланк заявки задания";
            this.Menu_OrderPdfSave.Click += new System.EventHandler(this.Button_OrderPdfSave_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(250, 6);
            // 
            // Menu_OrderFilesDownload
            // 
            this.Menu_OrderFilesDownload.Enabled = false;
            this.Menu_OrderFilesDownload.Image = global::AccentBase.Properties.Resources.Download_16x16;
            this.Menu_OrderFilesDownload.Name = "Menu_OrderFilesDownload";
            this.Menu_OrderFilesDownload.Size = new System.Drawing.Size(253, 22);
            this.Menu_OrderFilesDownload.Text = "Сохранить все файлы задания";
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(250, 6);
            // 
            // Menu_OrderExit
            // 
            this.Menu_OrderExit.Image = global::AccentBase.Properties.Resources.Text_Document_16x16;
            this.Menu_OrderExit.Name = "Menu_OrderExit";
            this.Menu_OrderExit.Size = new System.Drawing.Size(253, 22);
            this.Menu_OrderExit.Text = "Закрыть список заданий";
            this.Menu_OrderExit.Click += new System.EventHandler(this.Menu_OrderExit_Click);
            // 
            // Menu_AppExit
            // 
            this.Menu_AppExit.Image = global::AccentBase.Properties.Resources.door_in;
            this.Menu_AppExit.Name = "Menu_AppExit";
            this.Menu_AppExit.Size = new System.Drawing.Size(253, 22);
            this.Menu_AppExit.Text = "Выйти из приложения";
            this.Menu_AppExit.Click += new System.EventHandler(this.Menu_AppExit_Click);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuView_ToolStrip,
            this.MenuView_SocketSend,
            this.MenuView_TreeViewMenu,
            this.toolStripSeparator19,
            this.MenuView_Orders,
            this.MenuView_OrderPreview,
            this.MenuView_OrderNotes,
            this.MenuView_OrderHistory,
            this.toolStripSeparator18,
            this.MenuView_QuickSearch,
            this.MenuView_OrderListInfo,
            this.MenuView_Worktypes,
            this.toolStripSeparator20,
            this.MenuView_Messenger,
            this.MenuView_FtpList,
            this.MenuView_Stock,
            this.MenuView_GoogleTable});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(39, 20);
            this.Menu_View.Text = "Вид";
            // 
            // MenuView_ToolStrip
            // 
            this.MenuView_ToolStrip.Checked = true;
            this.MenuView_ToolStrip.CheckOnClick = true;
            this.MenuView_ToolStrip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_ToolStrip.Name = "MenuView_ToolStrip";
            this.MenuView_ToolStrip.Size = new System.Drawing.Size(374, 22);
            this.MenuView_ToolStrip.Text = "Панель инструментов";
            // 
            // MenuView_SocketSend
            // 
            this.MenuView_SocketSend.Checked = true;
            this.MenuView_SocketSend.CheckOnClick = true;
            this.MenuView_SocketSend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_SocketSend.Name = "MenuView_SocketSend";
            this.MenuView_SocketSend.Size = new System.Drawing.Size(374, 22);
            this.MenuView_SocketSend.Text = "Показывать список отсылаемых на сервер изменений";
            // 
            // MenuView_TreeViewMenu
            // 
            this.MenuView_TreeViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuView_TreeViewAllOrders,
            this.toolStripSeparator24,
            this.MenuView_TreeViewDraft,
            this.MenuView_TreeViewAwait,
            this.MenuView_TreeViewInWork,
            this.MenuView_TreeViewPostProc,
            this.MenuView_TreeViewStock,
            this.MenuView_TreeViewStopped,
            this.toolStripSeparator23,
            this.MenuView_TreeViewArchive});
            this.MenuView_TreeViewMenu.Image = global::AccentBase.Properties.Resources.application_side_expand;
            this.MenuView_TreeViewMenu.Name = "MenuView_TreeViewMenu";
            this.MenuView_TreeViewMenu.Size = new System.Drawing.Size(374, 22);
            this.MenuView_TreeViewMenu.Text = "Меню";
            this.MenuView_TreeViewMenu.Visible = false;
            // 
            // MenuView_TreeViewAllOrders
            // 
            this.MenuView_TreeViewAllOrders.Checked = true;
            this.MenuView_TreeViewAllOrders.CheckOnClick = true;
            this.MenuView_TreeViewAllOrders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewAllOrders.Name = "MenuView_TreeViewAllOrders";
            this.MenuView_TreeViewAllOrders.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewAllOrders.Text = "Все заявки";
            // 
            // toolStripSeparator24
            // 
            this.toolStripSeparator24.Name = "toolStripSeparator24";
            this.toolStripSeparator24.Size = new System.Drawing.Size(162, 6);
            // 
            // MenuView_TreeViewDraft
            // 
            this.MenuView_TreeViewDraft.Checked = true;
            this.MenuView_TreeViewDraft.CheckOnClick = true;
            this.MenuView_TreeViewDraft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewDraft.Name = "MenuView_TreeViewDraft";
            this.MenuView_TreeViewDraft.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewDraft.Text = "Черновики";
            this.MenuView_TreeViewDraft.CheckedChanged += new System.EventHandler(this.ViewItem_CheckedChanged);
            // 
            // MenuView_TreeViewAwait
            // 
            this.MenuView_TreeViewAwait.Checked = true;
            this.MenuView_TreeViewAwait.CheckOnClick = true;
            this.MenuView_TreeViewAwait.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewAwait.Name = "MenuView_TreeViewAwait";
            this.MenuView_TreeViewAwait.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewAwait.Text = "Ожидают";
            // 
            // MenuView_TreeViewInWork
            // 
            this.MenuView_TreeViewInWork.Checked = true;
            this.MenuView_TreeViewInWork.CheckOnClick = true;
            this.MenuView_TreeViewInWork.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewInWork.Name = "MenuView_TreeViewInWork";
            this.MenuView_TreeViewInWork.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewInWork.Text = "В работе";
            // 
            // MenuView_TreeViewPostProc
            // 
            this.MenuView_TreeViewPostProc.Checked = true;
            this.MenuView_TreeViewPostProc.CheckOnClick = true;
            this.MenuView_TreeViewPostProc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewPostProc.Name = "MenuView_TreeViewPostProc";
            this.MenuView_TreeViewPostProc.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewPostProc.Text = "Постобработка";
            // 
            // MenuView_TreeViewStock
            // 
            this.MenuView_TreeViewStock.Checked = true;
            this.MenuView_TreeViewStock.CheckOnClick = true;
            this.MenuView_TreeViewStock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewStock.Name = "MenuView_TreeViewStock";
            this.MenuView_TreeViewStock.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewStock.Text = "Сделаны (склад)";
            // 
            // MenuView_TreeViewStopped
            // 
            this.MenuView_TreeViewStopped.Checked = true;
            this.MenuView_TreeViewStopped.CheckOnClick = true;
            this.MenuView_TreeViewStopped.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewStopped.Name = "MenuView_TreeViewStopped";
            this.MenuView_TreeViewStopped.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewStopped.Text = "Остановлены";
            // 
            // toolStripSeparator23
            // 
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            this.toolStripSeparator23.Size = new System.Drawing.Size(162, 6);
            // 
            // MenuView_TreeViewArchive
            // 
            this.MenuView_TreeViewArchive.Checked = true;
            this.MenuView_TreeViewArchive.CheckOnClick = true;
            this.MenuView_TreeViewArchive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_TreeViewArchive.Name = "MenuView_TreeViewArchive";
            this.MenuView_TreeViewArchive.Size = new System.Drawing.Size(165, 22);
            this.MenuView_TreeViewArchive.Text = "Закрыты (архив)";
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(371, 6);
            // 
            // MenuView_Orders
            // 
            this.MenuView_Orders.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuView_OrderTimeStart,
            this.MenuView_OrderTimeEnd,
            this.MenuView_OrderWorkTypes,
            this.MenuView_OrderMaterial,
            this.MenuView_OrderManager,
            this.MenuView_OrderCustomer});
            this.MenuView_Orders.Image = global::AccentBase.Properties.Resources.Text_Document_16x16;
            this.MenuView_Orders.Name = "MenuView_Orders";
            this.MenuView_Orders.Size = new System.Drawing.Size(374, 22);
            this.MenuView_Orders.Text = "Список заданий";
            // 
            // MenuView_OrderTimeStart
            // 
            this.MenuView_OrderTimeStart.Checked = true;
            this.MenuView_OrderTimeStart.CheckOnClick = true;
            this.MenuView_OrderTimeStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderTimeStart.Name = "MenuView_OrderTimeStart";
            this.MenuView_OrderTimeStart.Size = new System.Drawing.Size(138, 22);
            this.MenuView_OrderTimeStart.Text = "Начать";
            // 
            // MenuView_OrderTimeEnd
            // 
            this.MenuView_OrderTimeEnd.Checked = true;
            this.MenuView_OrderTimeEnd.CheckOnClick = true;
            this.MenuView_OrderTimeEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderTimeEnd.Name = "MenuView_OrderTimeEnd";
            this.MenuView_OrderTimeEnd.Size = new System.Drawing.Size(138, 22);
            this.MenuView_OrderTimeEnd.Text = "Сделать до";
            // 
            // MenuView_OrderWorkTypes
            // 
            this.MenuView_OrderWorkTypes.Checked = true;
            this.MenuView_OrderWorkTypes.CheckOnClick = true;
            this.MenuView_OrderWorkTypes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderWorkTypes.Name = "MenuView_OrderWorkTypes";
            this.MenuView_OrderWorkTypes.Size = new System.Drawing.Size(138, 22);
            this.MenuView_OrderWorkTypes.Text = "Виды работ";
            // 
            // MenuView_OrderMaterial
            // 
            this.MenuView_OrderMaterial.Checked = true;
            this.MenuView_OrderMaterial.CheckOnClick = true;
            this.MenuView_OrderMaterial.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderMaterial.Name = "MenuView_OrderMaterial";
            this.MenuView_OrderMaterial.Size = new System.Drawing.Size(138, 22);
            this.MenuView_OrderMaterial.Text = "Материал";
            // 
            // MenuView_OrderManager
            // 
            this.MenuView_OrderManager.Checked = true;
            this.MenuView_OrderManager.CheckOnClick = true;
            this.MenuView_OrderManager.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderManager.Name = "MenuView_OrderManager";
            this.MenuView_OrderManager.Size = new System.Drawing.Size(138, 22);
            this.MenuView_OrderManager.Text = "Менеджер";
            // 
            // MenuView_OrderCustomer
            // 
            this.MenuView_OrderCustomer.Checked = true;
            this.MenuView_OrderCustomer.CheckOnClick = true;
            this.MenuView_OrderCustomer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderCustomer.Name = "MenuView_OrderCustomer";
            this.MenuView_OrderCustomer.Size = new System.Drawing.Size(138, 22);
            this.MenuView_OrderCustomer.Text = "Заказчик";
            // 
            // MenuView_OrderPreview
            // 
            this.MenuView_OrderPreview.Checked = true;
            this.MenuView_OrderPreview.CheckOnClick = true;
            this.MenuView_OrderPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderPreview.Name = "MenuView_OrderPreview";
            this.MenuView_OrderPreview.Size = new System.Drawing.Size(374, 22);
            this.MenuView_OrderPreview.Text = "Изображение задания";
            // 
            // MenuView_OrderNotes
            // 
            this.MenuView_OrderNotes.Checked = true;
            this.MenuView_OrderNotes.CheckOnClick = true;
            this.MenuView_OrderNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderNotes.Name = "MenuView_OrderNotes";
            this.MenuView_OrderNotes.Size = new System.Drawing.Size(374, 22);
            this.MenuView_OrderNotes.Text = "Описание задания";
            // 
            // MenuView_OrderHistory
            // 
            this.MenuView_OrderHistory.Checked = true;
            this.MenuView_OrderHistory.CheckOnClick = true;
            this.MenuView_OrderHistory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderHistory.Name = "MenuView_OrderHistory";
            this.MenuView_OrderHistory.Size = new System.Drawing.Size(374, 22);
            this.MenuView_OrderHistory.Text = "История задания";
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(371, 6);
            // 
            // MenuView_QuickSearch
            // 
            this.MenuView_QuickSearch.Checked = true;
            this.MenuView_QuickSearch.CheckOnClick = true;
            this.MenuView_QuickSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_QuickSearch.Name = "MenuView_QuickSearch";
            this.MenuView_QuickSearch.Size = new System.Drawing.Size(374, 22);
            this.MenuView_QuickSearch.Text = "Строка быстрого поиска";
            this.MenuView_QuickSearch.Visible = false;
            // 
            // MenuView_OrderListInfo
            // 
            this.MenuView_OrderListInfo.Checked = true;
            this.MenuView_OrderListInfo.CheckOnClick = true;
            this.MenuView_OrderListInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_OrderListInfo.Name = "MenuView_OrderListInfo";
            this.MenuView_OrderListInfo.Size = new System.Drawing.Size(374, 22);
            this.MenuView_OrderListInfo.Text = "Сводная информация";
            // 
            // MenuView_Worktypes
            // 
            this.MenuView_Worktypes.Checked = true;
            this.MenuView_Worktypes.CheckOnClick = true;
            this.MenuView_Worktypes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuView_Worktypes.Name = "MenuView_Worktypes";
            this.MenuView_Worktypes.Size = new System.Drawing.Size(374, 22);
            this.MenuView_Worktypes.Text = "Выбор видов работ";
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            this.toolStripSeparator20.Size = new System.Drawing.Size(371, 6);
            // 
            // MenuView_Messenger
            // 
            this.MenuView_Messenger.Image = global::AccentBase.Properties.Resources.Forward_16x16;
            this.MenuView_Messenger.Name = "MenuView_Messenger";
            this.MenuView_Messenger.Size = new System.Drawing.Size(374, 22);
            this.MenuView_Messenger.Text = "Мессенджер";
            this.MenuView_Messenger.CheckedChanged += new System.EventHandler(this.ViewItem_CheckedChanged);
            // 
            // MenuView_FtpList
            // 
            this.MenuView_FtpList.Image = global::AccentBase.Properties.Resources.Synchronize_16x16;
            this.MenuView_FtpList.Name = "MenuView_FtpList";
            this.MenuView_FtpList.Size = new System.Drawing.Size(374, 22);
            this.MenuView_FtpList.Text = "Обмен файлами";
            this.MenuView_FtpList.CheckedChanged += new System.EventHandler(this.ViewItem_CheckedChanged);
            // 
            // MenuView_Stock
            // 
            this.MenuView_Stock.Image = global::AccentBase.Properties.Resources.iconfinder_folder_green_1530;
            this.MenuView_Stock.Name = "MenuView_Stock";
            this.MenuView_Stock.Size = new System.Drawing.Size(374, 22);
            this.MenuView_Stock.Text = "Материалы и оборудование";
            this.MenuView_Stock.CheckedChanged += new System.EventHandler(this.ViewItem_CheckedChanged);
            // 
            // MenuView_GoogleTable
            // 
            this.MenuView_GoogleTable.Image = global::AccentBase.Properties.Resources.googleimg;
            this.MenuView_GoogleTable.Name = "MenuView_GoogleTable";
            this.MenuView_GoogleTable.Size = new System.Drawing.Size(374, 22);
            this.MenuView_GoogleTable.Text = "Google таблицы - склад";
            this.MenuView_GoogleTable.CheckedChanged += new System.EventHandler(this.ViewItem_CheckedChanged);
            // 
            // MenuService
            // 
            this.MenuService.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuService_SocketServerTransmite,
            this.MenuService_FtpTransmite,
            this.toolStripSeparator21,
            this.MenuService__Settings,
            this.toolStripSeparator22,
            this.MenuService_WinCalc});
            this.MenuService.Name = "MenuService";
            this.MenuService.Size = new System.Drawing.Size(59, 20);
            this.MenuService.Text = "Сервис";
            this.MenuService.DropDownOpening += new System.EventHandler(this.MenuService_DropDownOpening);
            // 
            // MenuService_SocketServerTransmite
            // 
            this.MenuService_SocketServerTransmite.Image = global::AccentBase.Properties.Resources.server_delete;
            this.MenuService_SocketServerTransmite.Name = "MenuService_SocketServerTransmite";
            this.MenuService_SocketServerTransmite.Size = new System.Drawing.Size(224, 22);
            this.MenuService_SocketServerTransmite.Text = "Прервать связь с сервером";
            this.MenuService_SocketServerTransmite.Click += new System.EventHandler(this.MenuService_SocketServerTransmite_Click);
            // 
            // MenuService_FtpTransmite
            // 
            this.MenuService_FtpTransmite.Image = global::AccentBase.Properties.Resources.drive_delete;
            this.MenuService_FtpTransmite.Name = "MenuService_FtpTransmite";
            this.MenuService_FtpTransmite.Size = new System.Drawing.Size(224, 22);
            this.MenuService_FtpTransmite.Text = "Остановить обмен файлов";
            this.MenuService_FtpTransmite.Click += new System.EventHandler(this.MenuService_FtpTransmite_Click);
            // 
            // toolStripSeparator21
            // 
            this.toolStripSeparator21.Name = "toolStripSeparator21";
            this.toolStripSeparator21.Size = new System.Drawing.Size(221, 6);
            // 
            // MenuService__Settings
            // 
            this.MenuService__Settings.Image = global::AccentBase.Properties.Resources.Settings_24x24;
            this.MenuService__Settings.Name = "MenuService__Settings";
            this.MenuService__Settings.Size = new System.Drawing.Size(224, 22);
            this.MenuService__Settings.Text = "Настройки";
            this.MenuService__Settings.Click += new System.EventHandler(this.MenuService__Settings_Click);
            // 
            // toolStripSeparator22
            // 
            this.toolStripSeparator22.Name = "toolStripSeparator22";
            this.toolStripSeparator22.Size = new System.Drawing.Size(221, 6);
            // 
            // MenuService_WinCalc
            // 
            this.MenuService_WinCalc.Image = global::AccentBase.Properties.Resources.glasscalc_91001;
            this.MenuService_WinCalc.Name = "MenuService_WinCalc";
            this.MenuService_WinCalc.Size = new System.Drawing.Size(224, 22);
            this.MenuService_WinCalc.Text = "Windows Калькулятор";
            this.MenuService_WinCalc.Click += new System.EventHandler(this.MenuService_WinCalc_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справкаAccentBaseToolStripMenuItem,
            this.toolStripSeparator5,
            this.Menu_CheckUpdate,
            this.toolStripSeparator4,
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // справкаAccentBaseToolStripMenuItem
            // 
            this.справкаAccentBaseToolStripMenuItem.Enabled = false;
            this.справкаAccentBaseToolStripMenuItem.Image = global::AccentBase.Properties.Resources.Help_16x16;
            this.справкаAccentBaseToolStripMenuItem.Name = "справкаAccentBaseToolStripMenuItem";
            this.справкаAccentBaseToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.справкаAccentBaseToolStripMenuItem.Text = "Справка AccentBase";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(201, 6);
            // 
            // Menu_CheckUpdate
            // 
            this.Menu_CheckUpdate.Image = global::AccentBase.Properties.Resources.Globe_16x16;
            this.Menu_CheckUpdate.Name = "Menu_CheckUpdate";
            this.Menu_CheckUpdate.Size = new System.Drawing.Size(204, 22);
            this.Menu_CheckUpdate.Text = "Проверить обновление";
            this.Menu_CheckUpdate.Click += new System.EventHandler(this.Menu_CheckUpdate_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(201, 6);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Enabled = false;
            this.оПрограммеToolStripMenuItem.Image = global::AccentBase.Properties.Resources.Information_16x16;
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // contextMenuStrip_Orders
            // 
            this.contextMenuStrip_Orders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuStrip_Open,
            this.ToolStripMenuItem_OrderSave,
            this.ToolStripMenuItem_Print,
            this.ToolStripMenuItem_InstallPrint,
            this.toolStripSeparator8,
            this.ToolStripMenuItem_PrintON,
            this.ToolStripMenuItem_CutON,
            this.ToolStripMenuItem_CncON,
            this.ToolStripMenuItem_InstallON,
            this.toolStripSeparator9,
            this.ToolStripMenuItem_Await,
            this.ToolStripMenuItem_InWork,
            this.ToolStripMenuItem_PostProc,
            this.ToolStripMenuItem_Stock,
            this.ToolStripMenuItem_Closed,
            this.ToolStripMenuItem_ChangeState,
            this.toolStripSeparator_Stop,
            this.ToolStripMenuItem_Stoped,
            this.toolStripSeparator10,
            this.ToolStripMenuItem_Basket,
            this.ToolStripMenuItem_Remove});
            this.contextMenuStrip_Orders.Name = "contextMenuStrip_Orders";
            this.contextMenuStrip_Orders.Size = new System.Drawing.Size(237, 424);
            // 
            // contextMenuStrip_Open
            // 
            this.contextMenuStrip_Open.BackgroundImage = global::AccentBase.Properties.Resources.headback;
            this.contextMenuStrip_Open.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.contextMenuStrip_Open.Name = "contextMenuStrip_Open";
            this.contextMenuStrip_Open.Size = new System.Drawing.Size(236, 22);
            this.contextMenuStrip_Open.Text = "Заявка № 000000";
            this.contextMenuStrip_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.contextMenuStrip_Open.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.contextMenuStrip_Open.Click += new System.EventHandler(this.contextMenuStrip_Open_Click);
            // 
            // ToolStripMenuItem_OrderSave
            // 
            this.ToolStripMenuItem_OrderSave.Image = global::AccentBase.Properties.Resources.disk;
            this.ToolStripMenuItem_OrderSave.Name = "ToolStripMenuItem_OrderSave";
            this.ToolStripMenuItem_OrderSave.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_OrderSave.Text = "Сохранить все данные заявки";
            this.ToolStripMenuItem_OrderSave.Click += new System.EventHandler(this.Button_OrderFilesDownload_Click);
            // 
            // ToolStripMenuItem_Print
            // 
            this.ToolStripMenuItem_Print.Image = global::AccentBase.Properties.Resources.printer;
            this.ToolStripMenuItem_Print.Name = "ToolStripMenuItem_Print";
            this.ToolStripMenuItem_Print.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_Print.Text = "Печать заявки";
            this.ToolStripMenuItem_Print.Click += new System.EventHandler(this.Button_OrderPrint_Click);
            // 
            // ToolStripMenuItem_InstallPrint
            // 
            this.ToolStripMenuItem_InstallPrint.Image = global::AccentBase.Properties.Resources.page_print;
            this.ToolStripMenuItem_InstallPrint.Name = "ToolStripMenuItem_InstallPrint";
            this.ToolStripMenuItem_InstallPrint.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_InstallPrint.Text = "Печать монтажного задания";
            this.ToolStripMenuItem_InstallPrint.Click += new System.EventHandler(this.ToolStripMenuItem_InstallPrint_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(233, 6);
            // 
            // ToolStripMenuItem_PrintON
            // 
            this.ToolStripMenuItem_PrintON.Checked = true;
            this.ToolStripMenuItem_PrintON.CheckOnClick = true;
            this.ToolStripMenuItem_PrintON.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItem_PrintON.Name = "ToolStripMenuItem_PrintON";
            this.ToolStripMenuItem_PrintON.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_PrintON.Text = "Напечатано";
            this.ToolStripMenuItem_PrintON.Click += new System.EventHandler(this.ToolStripMenuItem_StatesChange_Click);
            // 
            // ToolStripMenuItem_CutON
            // 
            this.ToolStripMenuItem_CutON.Checked = true;
            this.ToolStripMenuItem_CutON.CheckOnClick = true;
            this.ToolStripMenuItem_CutON.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItem_CutON.Name = "ToolStripMenuItem_CutON";
            this.ToolStripMenuItem_CutON.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_CutON.Text = "Порезано на каттере";
            this.ToolStripMenuItem_CutON.Click += new System.EventHandler(this.ToolStripMenuItem_StatesChange_Click);
            // 
            // ToolStripMenuItem_CncON
            // 
            this.ToolStripMenuItem_CncON.Checked = true;
            this.ToolStripMenuItem_CncON.CheckOnClick = true;
            this.ToolStripMenuItem_CncON.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItem_CncON.Name = "ToolStripMenuItem_CncON";
            this.ToolStripMenuItem_CncON.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_CncON.Text = "Отфрезеровано";
            this.ToolStripMenuItem_CncON.Click += new System.EventHandler(this.ToolStripMenuItem_StatesChange_Click);
            // 
            // ToolStripMenuItem_InstallON
            // 
            this.ToolStripMenuItem_InstallON.Checked = true;
            this.ToolStripMenuItem_InstallON.CheckOnClick = true;
            this.ToolStripMenuItem_InstallON.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItem_InstallON.Name = "ToolStripMenuItem_InstallON";
            this.ToolStripMenuItem_InstallON.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_InstallON.Text = "Монтаж";
            this.ToolStripMenuItem_InstallON.Click += new System.EventHandler(this.ToolStripMenuItem_StatesChange_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(233, 6);
            // 
            // ToolStripMenuItem_Await
            // 
            this.ToolStripMenuItem_Await.Image = global::AccentBase.Properties.Resources.hourglass1;
            this.ToolStripMenuItem_Await.Name = "ToolStripMenuItem_Await";
            this.ToolStripMenuItem_Await.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_Await.Tag = "0";
            this.ToolStripMenuItem_Await.Text = "В ожидающие";
            this.ToolStripMenuItem_Await.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_InWork
            // 
            this.ToolStripMenuItem_InWork.Image = global::AccentBase.Properties.Resources.printer;
            this.ToolStripMenuItem_InWork.Name = "ToolStripMenuItem_InWork";
            this.ToolStripMenuItem_InWork.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_InWork.Tag = "1";
            this.ToolStripMenuItem_InWork.Text = "В работу";
            this.ToolStripMenuItem_InWork.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_PostProc
            // 
            this.ToolStripMenuItem_PostProc.Image = global::AccentBase.Properties.Resources.wrench_orange;
            this.ToolStripMenuItem_PostProc.Name = "ToolStripMenuItem_PostProc";
            this.ToolStripMenuItem_PostProc.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_PostProc.Tag = "2";
            this.ToolStripMenuItem_PostProc.Text = "В постобработку";
            this.ToolStripMenuItem_PostProc.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_Stock
            // 
            this.ToolStripMenuItem_Stock.Image = global::AccentBase.Properties.Resources.Archive_16x16;
            this.ToolStripMenuItem_Stock.Name = "ToolStripMenuItem_Stock";
            this.ToolStripMenuItem_Stock.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_Stock.Tag = "3";
            this.ToolStripMenuItem_Stock.Text = "Готово (на склад)";
            this.ToolStripMenuItem_Stock.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_Closed
            // 
            this.ToolStripMenuItem_Closed.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItem_Closed.Image")));
            this.ToolStripMenuItem_Closed.Name = "ToolStripMenuItem_Closed";
            this.ToolStripMenuItem_Closed.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_Closed.Tag = "4";
            this.ToolStripMenuItem_Closed.Text = "Закрыто (отдано клиенту)";
            this.ToolStripMenuItem_Closed.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_ChangeState
            // 
            this.ToolStripMenuItem_ChangeState.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_DraftDouble,
            this.ToolStripMenuItem_AwaiteDouble,
            this.ToolStripMenuItem_InworkDouble,
            this.ToolStripMenuItem_PostProcDouble,
            this.ToolStripMenuItem_StockDouble,
            this.ToolStripMenuItem_ClosedDouble});
            this.ToolStripMenuItem_ChangeState.Name = "ToolStripMenuItem_ChangeState";
            this.ToolStripMenuItem_ChangeState.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_ChangeState.Text = "Изменить статус задания";
            // 
            // ToolStripMenuItem_DraftDouble
            // 
            this.ToolStripMenuItem_DraftDouble.Image = global::AccentBase.Properties.Resources.table_edit;
            this.ToolStripMenuItem_DraftDouble.Name = "ToolStripMenuItem_DraftDouble";
            this.ToolStripMenuItem_DraftDouble.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItem_DraftDouble.Tag = "7";
            this.ToolStripMenuItem_DraftDouble.Text = "В черновики";
            this.ToolStripMenuItem_DraftDouble.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_AwaiteDouble
            // 
            this.ToolStripMenuItem_AwaiteDouble.Image = global::AccentBase.Properties.Resources.hourglass1;
            this.ToolStripMenuItem_AwaiteDouble.Name = "ToolStripMenuItem_AwaiteDouble";
            this.ToolStripMenuItem_AwaiteDouble.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItem_AwaiteDouble.Tag = "0";
            this.ToolStripMenuItem_AwaiteDouble.Text = "В ожидающие";
            this.ToolStripMenuItem_AwaiteDouble.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_InworkDouble
            // 
            this.ToolStripMenuItem_InworkDouble.Image = global::AccentBase.Properties.Resources.printer;
            this.ToolStripMenuItem_InworkDouble.Name = "ToolStripMenuItem_InworkDouble";
            this.ToolStripMenuItem_InworkDouble.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItem_InworkDouble.Tag = "1";
            this.ToolStripMenuItem_InworkDouble.Text = "В работу";
            this.ToolStripMenuItem_InworkDouble.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_PostProcDouble
            // 
            this.ToolStripMenuItem_PostProcDouble.Image = global::AccentBase.Properties.Resources.wrench_orange;
            this.ToolStripMenuItem_PostProcDouble.Name = "ToolStripMenuItem_PostProcDouble";
            this.ToolStripMenuItem_PostProcDouble.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItem_PostProcDouble.Tag = "2";
            this.ToolStripMenuItem_PostProcDouble.Text = "В постобработку";
            this.ToolStripMenuItem_PostProcDouble.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_StockDouble
            // 
            this.ToolStripMenuItem_StockDouble.Image = global::AccentBase.Properties.Resources.Archive_16x16;
            this.ToolStripMenuItem_StockDouble.Name = "ToolStripMenuItem_StockDouble";
            this.ToolStripMenuItem_StockDouble.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItem_StockDouble.Tag = "3";
            this.ToolStripMenuItem_StockDouble.Text = "Готово (на склад)";
            this.ToolStripMenuItem_StockDouble.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_ClosedDouble
            // 
            this.ToolStripMenuItem_ClosedDouble.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItem_ClosedDouble.Image")));
            this.ToolStripMenuItem_ClosedDouble.Name = "ToolStripMenuItem_ClosedDouble";
            this.ToolStripMenuItem_ClosedDouble.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItem_ClosedDouble.Tag = "4";
            this.ToolStripMenuItem_ClosedDouble.Text = "Закрыто (отдано клиенту)";
            this.ToolStripMenuItem_ClosedDouble.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // toolStripSeparator_Stop
            // 
            this.toolStripSeparator_Stop.Name = "toolStripSeparator_Stop";
            this.toolStripSeparator_Stop.Size = new System.Drawing.Size(233, 6);
            // 
            // ToolStripMenuItem_Stoped
            // 
            this.ToolStripMenuItem_Stoped.Image = global::AccentBase.Properties.Resources.table_error;
            this.ToolStripMenuItem_Stoped.Name = "ToolStripMenuItem_Stoped";
            this.ToolStripMenuItem_Stoped.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_Stoped.Tag = "5";
            this.ToolStripMenuItem_Stoped.Text = "Остановить задание";
            this.ToolStripMenuItem_Stoped.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(233, 6);
            // 
            // ToolStripMenuItem_Basket
            // 
            this.ToolStripMenuItem_Basket.Image = global::AccentBase.Properties.Resources.trash_aqua_full_icon;
            this.ToolStripMenuItem_Basket.Name = "ToolStripMenuItem_Basket";
            this.ToolStripMenuItem_Basket.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_Basket.Tag = "6";
            this.ToolStripMenuItem_Basket.Text = "Переместить в корзину";
            this.ToolStripMenuItem_Basket.Click += new System.EventHandler(this.ToolStripMenuItem_Await_Click);
            // 
            // ToolStripMenuItem_Remove
            // 
            this.ToolStripMenuItem_Remove.Image = global::AccentBase.Properties.Resources.trash_aqua_empty_icon;
            this.ToolStripMenuItem_Remove.Name = "ToolStripMenuItem_Remove";
            this.ToolStripMenuItem_Remove.Size = new System.Drawing.Size(236, 22);
            this.ToolStripMenuItem_Remove.Text = "Удалить";
            this.ToolStripMenuItem_Remove.Click += new System.EventHandler(this.RemoveOrders_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "package.png");
            this.imageList1.Images.SetKeyName(1, "hourglass.png");
            this.imageList1.Images.SetKeyName(2, "hourglass_add.png");
            this.imageList1.Images.SetKeyName(3, "hourglass_delete.png");
            this.imageList1.Images.SetKeyName(4, "hourglass_go.png");
            this.imageList1.Images.SetKeyName(5, "hourglass_link.png");
            this.imageList1.Images.SetKeyName(6, "trashAltFull.png");
            this.imageList1.Images.SetKeyName(7, "application_view_columns.png");
            this.imageList1.Images.SetKeyName(8, "table_add.png");
            this.imageList1.Images.SetKeyName(9, "table_delete.png");
            this.imageList1.Images.SetKeyName(10, "table_edit.png");
            this.imageList1.Images.SetKeyName(11, "table_error.png");
            this.imageList1.Images.SetKeyName(12, "table_gear.png");
            this.imageList1.Images.SetKeyName(13, "table_go.png");
            this.imageList1.Images.SetKeyName(14, "table_key.png");
            this.imageList1.Images.SetKeyName(15, "table_lightning.png");
            this.imageList1.Images.SetKeyName(16, "table_link.png");
            this.imageList1.Images.SetKeyName(17, "table_multiple.png");
            this.imageList1.Images.SetKeyName(18, "table_refresh.png");
            this.imageList1.Images.SetKeyName(19, "table_relationship.png");
            this.imageList1.Images.SetKeyName(20, "table_row_delete.png");
            this.imageList1.Images.SetKeyName(21, "table_row_insert.png");
            this.imageList1.Images.SetKeyName(22, "table_save.png");
            this.imageList1.Images.SetKeyName(23, "table_sort.png");
            this.imageList1.Images.SetKeyName(24, "table.png");
            this.imageList1.Images.SetKeyName(25, "taskend.png");
            this.imageList1.Images.SetKeyName(26, "taskwait.png");
            this.imageList1.Images.SetKeyName(27, "wrench_orange.png");
            this.imageList1.Images.SetKeyName(28, "printer.png");
            this.imageList1.Images.SetKeyName(29, "Archive_16x16.png");
            this.imageList1.Images.SetKeyName(30, "Okicon.png");
            this.imageList1.Images.SetKeyName(31, "lock.png");
            this.imageList1.Images.SetKeyName(32, "trash-aqua-empty-icon.png");
            this.imageList1.Images.SetKeyName(33, "trash-aqua-full-icon.png");
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(418, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Тест растеризации заявки";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(558, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Тест растеризации Pdf\\Eps";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox_QuickSearch_Customers
            // 
            this.pictureBox_QuickSearch_Customers.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox_QuickSearch_Customers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_QuickSearch_Customers.Image = global::AccentBase.Properties.Resources.magnifier;
            this.pictureBox_QuickSearch_Customers.Location = new System.Drawing.Point(3, 1);
            this.pictureBox_QuickSearch_Customers.Name = "pictureBox_QuickSearch_Customers";
            this.pictureBox_QuickSearch_Customers.Size = new System.Drawing.Size(18, 18);
            this.pictureBox_QuickSearch_Customers.TabIndex = 3;
            this.pictureBox_QuickSearch_Customers.TabStop = false;
            this.pictureBox_QuickSearch_Customers.Visible = false;
            this.pictureBox_QuickSearch_Customers.Click += new System.EventHandler(this.QuickSearch_Click);
            // 
            // panel_QuickSearch
            // 
            this.panel_QuickSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_QuickSearch.BackColor = System.Drawing.SystemColors.Window;
            this.panel_QuickSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_QuickSearch.Controls.Add(this.pictureBox_QuickSearch_Customers);
            this.panel_QuickSearch.Controls.Add(this.checkBox1);
            this.panel_QuickSearch.Controls.Add(this.textBox_QuickSearch);
            this.panel_QuickSearch.Location = new System.Drawing.Point(814, 0);
            this.panel_QuickSearch.Name = "panel_QuickSearch";
            this.panel_QuickSearch.Size = new System.Drawing.Size(587, 24);
            this.panel_QuickSearch.TabIndex = 1;
            this.panel_QuickSearch.Visible = false;
            this.panel_QuickSearch.Click += new System.EventHandler(this.QuickSearch_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.SystemColors.Window;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(446, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(134, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Игнорировать статус";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBox_QuickSearch
            // 
            this.textBox_QuickSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_QuickSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_QuickSearch.Location = new System.Drawing.Point(21, 4);
            this.textBox_QuickSearch.Margin = new System.Windows.Forms.Padding(5);
            this.textBox_QuickSearch.Name = "textBox_QuickSearch";
            this.textBox_QuickSearch.Size = new System.Drawing.Size(417, 13);
            this.textBox_QuickSearch.TabIndex = 4;
            this.textBox_QuickSearch.Visible = false;
            this.textBox_QuickSearch.TextChanged += new System.EventHandler(this.textBox_QuickSearch_TextChanged);
            // 
            // toolStrip_Main
            // 
            this.toolStrip_Main.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_OrderNew,
            this.Button_OrderCopy,
            this.Button_OrderEdit,
            this.Button_OrderDelete,
            this.toolStripSeparator11,
            this.Button_OrderPrint,
            this.Button_OrderPdfSave,
            this.toolStripSeparator12,
            this.Button_OrderFilesDownload,
            this.toolStripSeparator13,
            this.Button_FtpFiles,
            this.Button_Messenger,
            this.Button_Stock,
            this.Button_GoogleTable,
            this.toolStripSeparator14,
            this.Button_Calc,
            this.toolStripSeparator25,
            this.toolStripLabel9,
            this.toolStripTextBox1,
            this.toolStripSeparator15,
            this.Button_Settings,
            this.Button_Help,
            this.toolStripSeparator7});
            this.toolStrip_Main.Location = new System.Drawing.Point(0, 24);
            this.toolStrip_Main.Name = "toolStrip_Main";
            this.toolStrip_Main.Size = new System.Drawing.Size(1405, 25);
            this.toolStrip_Main.TabIndex = 5;
            this.toolStrip_Main.Text = "toolStrip1";
            // 
            // Button_OrderNew
            // 
            this.Button_OrderNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_OrderNew.Image = global::AccentBase.Properties.Resources.page_white_add;
            this.Button_OrderNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderNew.Name = "Button_OrderNew";
            this.Button_OrderNew.Size = new System.Drawing.Size(23, 22);
            this.Button_OrderNew.Text = "Новое задание";
            this.Button_OrderNew.Click += new System.EventHandler(this.NewOrder_Click);
            // 
            // Button_OrderCopy
            // 
            this.Button_OrderCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_OrderCopy.Enabled = false;
            this.Button_OrderCopy.Image = global::AccentBase.Properties.Resources.page_white_copy;
            this.Button_OrderCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderCopy.Name = "Button_OrderCopy";
            this.Button_OrderCopy.Size = new System.Drawing.Size(23, 22);
            this.Button_OrderCopy.Text = "Копия";
            // 
            // Button_OrderEdit
            // 
            this.Button_OrderEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_OrderEdit.Enabled = false;
            this.Button_OrderEdit.Image = global::AccentBase.Properties.Resources.page_white_edit;
            this.Button_OrderEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderEdit.Name = "Button_OrderEdit";
            this.Button_OrderEdit.Size = new System.Drawing.Size(23, 22);
            this.Button_OrderEdit.Text = "Изменить";
            this.Button_OrderEdit.Click += new System.EventHandler(this.Button_OrderEdit_Click);
            // 
            // Button_OrderDelete
            // 
            this.Button_OrderDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_OrderDelete.Enabled = false;
            this.Button_OrderDelete.Image = global::AccentBase.Properties.Resources.page_white_delete;
            this.Button_OrderDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderDelete.Name = "Button_OrderDelete";
            this.Button_OrderDelete.Size = new System.Drawing.Size(23, 22);
            this.Button_OrderDelete.Text = "Удалить";
            this.Button_OrderDelete.Click += new System.EventHandler(this.Button_OrderDelete_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // Button_OrderPrint
            // 
            this.Button_OrderPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_OrderPrint.Image = global::AccentBase.Properties.Resources.printer;
            this.Button_OrderPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderPrint.Name = "Button_OrderPrint";
            this.Button_OrderPrint.Size = new System.Drawing.Size(23, 22);
            this.Button_OrderPrint.Text = "toolStripButton4";
            this.Button_OrderPrint.Click += new System.EventHandler(this.Button_OrderPrint_Click);
            // 
            // Button_OrderPdfSave
            // 
            this.Button_OrderPdfSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_OrderPdfSave.Image = global::AccentBase.Properties.Resources.page_white_acrobat_put;
            this.Button_OrderPdfSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderPdfSave.Name = "Button_OrderPdfSave";
            this.Button_OrderPdfSave.Size = new System.Drawing.Size(23, 22);
            this.Button_OrderPdfSave.Text = "Сохранить PDF";
            this.Button_OrderPdfSave.Click += new System.EventHandler(this.Button_OrderPdfSave_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // Button_OrderFilesDownload
            // 
            this.Button_OrderFilesDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_OrderFilesDownload.Enabled = false;
            this.Button_OrderFilesDownload.Image = global::AccentBase.Properties.Resources.Download_16x16;
            this.Button_OrderFilesDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderFilesDownload.Name = "Button_OrderFilesDownload";
            this.Button_OrderFilesDownload.Size = new System.Drawing.Size(23, 22);
            this.Button_OrderFilesDownload.Text = "toolStripButton9";
            this.Button_OrderFilesDownload.Click += new System.EventHandler(this.Button_OrderFilesDownload_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // Button_FtpFiles
            // 
            this.Button_FtpFiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_FtpFiles.Enabled = false;
            this.Button_FtpFiles.Image = global::AccentBase.Properties.Resources.Synchronize_16x16;
            this.Button_FtpFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_FtpFiles.Name = "Button_FtpFiles";
            this.Button_FtpFiles.Size = new System.Drawing.Size(23, 22);
            this.Button_FtpFiles.Text = "Передача файлов";
            // 
            // Button_Messenger
            // 
            this.Button_Messenger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Messenger.Enabled = false;
            this.Button_Messenger.Image = global::AccentBase.Properties.Resources.Forward_16x16;
            this.Button_Messenger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Messenger.Name = "Button_Messenger";
            this.Button_Messenger.Size = new System.Drawing.Size(23, 22);
            this.Button_Messenger.Text = "Мессенджер";
            // 
            // Button_Stock
            // 
            this.Button_Stock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Stock.Enabled = false;
            this.Button_Stock.Image = global::AccentBase.Properties.Resources.iconfinder_folder_green_1530;
            this.Button_Stock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Stock.Name = "Button_Stock";
            this.Button_Stock.Size = new System.Drawing.Size(23, 22);
            this.Button_Stock.Text = "Материалы и оборудование";
            // 
            // Button_GoogleTable
            // 
            this.Button_GoogleTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_GoogleTable.Enabled = false;
            this.Button_GoogleTable.Image = global::AccentBase.Properties.Resources.googleimg;
            this.Button_GoogleTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_GoogleTable.Name = "Button_GoogleTable";
            this.Button_GoogleTable.Size = new System.Drawing.Size(23, 22);
            this.Button_GoogleTable.Text = "Google таблицы - склад";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // Button_Calc
            // 
            this.Button_Calc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Calc.Image = global::AccentBase.Properties.Resources.glasscalc_91001;
            this.Button_Calc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Calc.Name = "Button_Calc";
            this.Button_Calc.Size = new System.Drawing.Size(23, 22);
            this.Button_Calc.Text = "Windows калькулятор";
            this.Button_Calc.Click += new System.EventHandler(this.MenuService_WinCalc_Click);
            // 
            // toolStripSeparator25
            // 
            this.toolStripSeparator25.Name = "toolStripSeparator25";
            this.toolStripSeparator25.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel9.Image = global::AccentBase.Properties.Resources.magnifier;
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(16, 22);
            this.toolStripLabel9.Text = "Поиск";
            this.toolStripLabel9.ToolTipText = "Поиск";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 25);
            this.toolStripTextBox1.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
            // 
            // Button_Settings
            // 
            this.Button_Settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Settings.Image = global::AccentBase.Properties.Resources.Settings_16x16;
            this.Button_Settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Settings.Name = "Button_Settings";
            this.Button_Settings.Size = new System.Drawing.Size(23, 22);
            this.Button_Settings.Text = "Настройки";
            this.Button_Settings.Click += new System.EventHandler(this.MenuService__Settings_Click);
            // 
            // Button_Help
            // 
            this.Button_Help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Help.Enabled = false;
            this.Button_Help.Image = global::AccentBase.Properties.Resources.Help_16x16;
            this.Button_Help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Help.Name = "Button_Help";
            this.Button_Help.Size = new System.Drawing.Size(23, 22);
            this.Button_Help.Text = "Помощь";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // TabOrder
            // 
            this.TabOrder.Controls.Add(this.panel_Center);
            this.TabOrder.Controls.Add(this.splitter_Right);
            this.TabOrder.Controls.Add(this.panel_Right);
            this.TabOrder.Controls.Add(this.splitter_Left);
            this.TabOrder.Controls.Add(this.panel_Left);
            this.TabOrder.Controls.Add(this.toolStrip_Worktypes);
            this.TabOrder.Location = new System.Drawing.Point(4, 25);
            this.TabOrder.Name = "TabOrder";
            this.TabOrder.Padding = new System.Windows.Forms.Padding(3);
            this.TabOrder.Size = new System.Drawing.Size(1397, 568);
            this.TabOrder.TabIndex = 0;
            this.TabOrder.Text = "Заявки";
            this.TabOrder.UseVisualStyleBackColor = true;
            // 
            // panel_Center
            // 
            this.panel_Center.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Center.Controls.Add(this.customDataGridView_Orders);
            this.panel_Center.Controls.Add(this.toolStrip_OrderDatagridview);
            this.panel_Center.Controls.Add(this.splitter_SocketSend);
            this.panel_Center.Controls.Add(this.dataGridView_SocketSend);
            this.panel_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Center.Location = new System.Drawing.Point(337, 3);
            this.panel_Center.Name = "panel_Center";
            this.panel_Center.Size = new System.Drawing.Size(748, 537);
            this.panel_Center.TabIndex = 3;
            // 
            // customDataGridView_Orders
            // 
            this.customDataGridView_Orders.AllowDrop = true;
            this.customDataGridView_Orders.AllowUserToAddRows = false;
            this.customDataGridView_Orders.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.customDataGridView_Orders.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView_Orders.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.customDataGridView_Orders.BackgroundColor = System.Drawing.SystemColors.Window;
            this.customDataGridView_Orders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customDataGridView_Orders.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customDataGridView_Orders.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customDataGridView_Orders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.customDataGridView_Orders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView_Orders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.String_id,
            this.Datetime_date_start,
            this.String_date_start,
            this.Datetime_dead_line,
            this.String_dead_line,
            this.work_name,
            this.client,
            this.Image_WorkTypes,
            this.Materials,
            this.manager,
            this.Column1,
            this.time_recieve,
            this.OrderState});
            this.customDataGridView_Orders.ContextMenuStrip = this.contextMenuStrip_Orders;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView_Orders.DefaultCellStyle = dataGridViewCellStyle9;
            this.customDataGridView_Orders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView_Orders.Filter = "";
            this.customDataGridView_Orders.FilterAll = true;
            this.customDataGridView_Orders.FilterCnc = false;
            this.customDataGridView_Orders.FilterCut = false;
            this.customDataGridView_Orders.FilterInstall = false;
            this.customDataGridView_Orders.FilterOfAllOrders = false;
            this.customDataGridView_Orders.FilterPrint = false;
            this.customDataGridView_Orders.FilterStatus = "";
            this.customDataGridView_Orders.Location = new System.Drawing.Point(0, 0);
            this.customDataGridView_Orders.MySort = "";
            this.customDataGridView_Orders.Name = "customDataGridView_Orders";
            this.customDataGridView_Orders.ReadOnly = true;
            this.customDataGridView_Orders.RowHeadersVisible = false;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customDataGridView_Orders.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.customDataGridView_Orders.RowTemplate.Height = 50;
            this.customDataGridView_Orders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView_Orders.Size = new System.Drawing.Size(744, 342);
            this.customDataGridView_Orders.SortingOrder = "";
            this.customDataGridView_Orders.SourceTable = null;
            this.customDataGridView_Orders.TabIndex = 0;
            this.customDataGridView_Orders.ErrorEvent += new AccentBase.CustomControls.OrdersDataGridView.MyDataGridEventHandler(this.customDataGridView1_ErrorEvent);
            this.customDataGridView_Orders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView_Orders_CellDoubleClick);
            this.customDataGridView_Orders.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.customDataGridView_Orders_DataBindingComplete);
            this.customDataGridView_Orders.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.customDataGridView_Orders_RowPrePaint);
            this.customDataGridView_Orders.SelectionChanged += new System.EventHandler(this.customDataGridView_Orders_SelectionChanged);
            this.customDataGridView_Orders.DragOver += new System.Windows.Forms.DragEventHandler(this.customDataGridView_Orders_DragOver);
            this.customDataGridView_Orders.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.customDataGridView_Orders_QueryContinueDrag);
            this.customDataGridView_Orders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.customDataGridView_Orders_KeyDown);
            this.customDataGridView_Orders.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customDataGridView_Orders_MouseDown);
            this.customDataGridView_Orders.MouseMove += new System.Windows.Forms.MouseEventHandler(this.customDataGridView_Orders_MouseMove);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.id.DataPropertyName = "id";
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = "0";
            this.id.DefaultCellStyle = dataGridViewCellStyle3;
            this.id.HeaderText = "№";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 60;
            // 
            // String_id
            // 
            this.String_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.String_id.DataPropertyName = "String_id";
            this.String_id.HeaderText = "String_id";
            this.String_id.Name = "String_id";
            this.String_id.ReadOnly = true;
            this.String_id.Visible = false;
            this.String_id.Width = 80;
            // 
            // Datetime_date_start
            // 
            this.Datetime_date_start.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Datetime_date_start.DataPropertyName = "Datetime_date_start";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "g";
            dataGridViewCellStyle4.NullValue = null;
            this.Datetime_date_start.DefaultCellStyle = dataGridViewCellStyle4;
            this.Datetime_date_start.HeaderText = "Начать с";
            this.Datetime_date_start.Name = "Datetime_date_start";
            this.Datetime_date_start.ReadOnly = true;
            this.Datetime_date_start.Visible = false;
            // 
            // String_date_start
            // 
            this.String_date_start.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.String_date_start.DataPropertyName = "String_date_start";
            this.String_date_start.HeaderText = "Начать";
            this.String_date_start.Name = "String_date_start";
            this.String_date_start.ReadOnly = true;
            // 
            // Datetime_dead_line
            // 
            this.Datetime_dead_line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Datetime_dead_line.DataPropertyName = "Datetime_dead_line";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "g";
            dataGridViewCellStyle5.NullValue = null;
            this.Datetime_dead_line.DefaultCellStyle = dataGridViewCellStyle5;
            this.Datetime_dead_line.HeaderText = "Сделать до";
            this.Datetime_dead_line.Name = "Datetime_dead_line";
            this.Datetime_dead_line.ReadOnly = true;
            // 
            // String_dead_line
            // 
            this.String_dead_line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.String_dead_line.DataPropertyName = "String_dead_line";
            this.String_dead_line.HeaderText = "String_dead_line";
            this.String_dead_line.Name = "String_dead_line";
            this.String_dead_line.ReadOnly = true;
            this.String_dead_line.Visible = false;
            // 
            // work_name
            // 
            this.work_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.work_name.DataPropertyName = "work_name";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.work_name.DefaultCellStyle = dataGridViewCellStyle6;
            this.work_name.HeaderText = "Наименование";
            this.work_name.Name = "work_name";
            this.work_name.ReadOnly = true;
            this.work_name.Width = 250;
            // 
            // client
            // 
            this.client.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.client.DataPropertyName = "client";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.client.DefaultCellStyle = dataGridViewCellStyle7;
            this.client.HeaderText = "Заказчик";
            this.client.Name = "client";
            this.client.ReadOnly = true;
            this.client.Width = 150;
            // 
            // Image_WorkTypes
            // 
            this.Image_WorkTypes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Image_WorkTypes.DataPropertyName = "Image_WorkTypes";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle8.NullValue")));
            this.Image_WorkTypes.DefaultCellStyle = dataGridViewCellStyle8;
            this.Image_WorkTypes.HeaderText = "Виды работ";
            this.Image_WorkTypes.Name = "Image_WorkTypes";
            this.Image_WorkTypes.ReadOnly = true;
            // 
            // Materials
            // 
            this.Materials.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Materials.DataPropertyName = "Order_Materials";
            this.Materials.HeaderText = "Материал";
            this.Materials.Name = "Materials";
            this.Materials.ReadOnly = true;
            this.Materials.Width = 180;
            // 
            // manager
            // 
            this.manager.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.manager.DataPropertyName = "adder";
            this.manager.HeaderText = "Менеджер";
            this.manager.Name = "manager";
            this.manager.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // time_recieve
            // 
            this.time_recieve.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.time_recieve.DataPropertyName = "time_recieve";
            this.time_recieve.HeaderText = "time_recieve";
            this.time_recieve.Name = "time_recieve";
            this.time_recieve.ReadOnly = true;
            this.time_recieve.Visible = false;
            // 
            // OrderState
            // 
            this.OrderState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OrderState.DataPropertyName = "status";
            this.OrderState.HeaderText = "OrderState";
            this.OrderState.Name = "OrderState";
            this.OrderState.ReadOnly = true;
            this.OrderState.Visible = false;
            // 
            // toolStrip_OrderDatagridview
            // 
            this.toolStrip_OrderDatagridview.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_OrderDatagridview.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_OrderDatagridview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripLabel3,
            this.toolStripSeparator1,
            this.toolStripLabel4,
            this.toolStripLabel5,
            this.toolStripSeparator2,
            this.toolStripLabel6,
            this.toolStripLabel7,
            this.toolStripSeparator3,
            this.toolStripLabel8,
            this.toolStripButton1});
            this.toolStrip_OrderDatagridview.Location = new System.Drawing.Point(0, 342);
            this.toolStrip_OrderDatagridview.Name = "toolStrip_OrderDatagridview";
            this.toolStrip_OrderDatagridview.Size = new System.Drawing.Size(744, 25);
            this.toolStrip_OrderDatagridview.TabIndex = 1;
            this.toolStrip_OrderDatagridview.Text = "toolStrip3";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(75, 22);
            this.toolStripLabel2.Text = "Всего строк:";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(13, 22);
            this.toolStripLabel3.Text = "0";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(102, 22);
            this.toolStripLabel4.Text = "Не прочитанных:";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(13, 22);
            this.toolStripLabel5.Text = "0";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(93, 22);
            this.toolStripLabel6.Text = "Всего выбрано:";
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(13, 22);
            this.toolStripLabel7.Text = "0";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(58, 22);
            this.toolStripLabel8.Text = "Текущая:";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(77, 22);
            this.toolStripButton1.Text = "Не выбрана";
            this.toolStripButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // splitter_SocketSend
            // 
            this.splitter_SocketSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter_SocketSend.Location = new System.Drawing.Point(0, 367);
            this.splitter_SocketSend.Name = "splitter_SocketSend";
            this.splitter_SocketSend.Size = new System.Drawing.Size(744, 4);
            this.splitter_SocketSend.TabIndex = 5;
            this.splitter_SocketSend.TabStop = false;
            this.splitter_SocketSend.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter_SocketSend_SplitterMoved);
            // 
            // dataGridView_SocketSend
            // 
            this.dataGridView_SocketSend.AllowUserToAddRows = false;
            this.dataGridView_SocketSend.AllowUserToDeleteRows = false;
            this.dataGridView_SocketSend.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Info;
            this.dataGridView_SocketSend.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView_SocketSend.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dataGridView_SocketSend.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_SocketSend.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView_SocketSend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_SocketSend.ColumnHeadersVisible = false;
            this.dataGridView_SocketSend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15});
            this.dataGridView_SocketSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView_SocketSend.Location = new System.Drawing.Point(0, 371);
            this.dataGridView_SocketSend.Name = "dataGridView_SocketSend";
            this.dataGridView_SocketSend.ReadOnly = true;
            this.dataGridView_SocketSend.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView_SocketSend.RowHeadersVisible = false;
            this.dataGridView_SocketSend.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Info;
            this.dataGridView_SocketSend.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridView_SocketSend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_SocketSend.Size = new System.Drawing.Size(744, 162);
            this.dataGridView_SocketSend.TabIndex = 6;
            this.dataGridView_SocketSend.SelectionChanged += new System.EventHandler(this.dataGridView_SocketSend_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "id";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Format = "N0";
            dataGridViewCellStyle13.NullValue = "0";
            this.dataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn11.FillWeight = 40F;
            this.dataGridViewTextBoxColumn11.HeaderText = "uid";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            this.dataGridViewTextBoxColumn11.Width = 40;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "dt_date_insert";
            dataGridViewCellStyle14.Format = "g";
            dataGridViewCellStyle14.NullValue = null;
            this.dataGridViewTextBoxColumn12.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn12.HeaderText = "Дата создания";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn13.HeaderText = "Наименование";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Width = 200;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "notes";
            this.dataGridViewTextBoxColumn14.HeaderText = "Примечание";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Width = 300;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn15.HeaderText = "";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // splitter_Right
            // 
            this.splitter_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter_Right.Location = new System.Drawing.Point(1085, 3);
            this.splitter_Right.Name = "splitter_Right";
            this.splitter_Right.Size = new System.Drawing.Size(3, 537);
            this.splitter_Right.TabIndex = 5;
            this.splitter_Right.TabStop = false;
            // 
            // panel_Right
            // 
            this.panel_Right.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Right.Controls.Add(this.panel_OrderAbout);
            this.panel_Right.Controls.Add(this.splitter_Preview);
            this.panel_Right.Controls.Add(this.panel_Preview);
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Right.Location = new System.Drawing.Point(1088, 3);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Size = new System.Drawing.Size(306, 537);
            this.panel_Right.TabIndex = 4;
            this.panel_Right.SizeChanged += new System.EventHandler(this.panel_Right_SizeChanged);
            // 
            // panel_OrderAbout
            // 
            this.panel_OrderAbout.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_OrderAbout.Controls.Add(this.groupBox_OrderInfo);
            this.panel_OrderAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_OrderAbout.Location = new System.Drawing.Point(0, 197);
            this.panel_OrderAbout.Name = "panel_OrderAbout";
            this.panel_OrderAbout.Size = new System.Drawing.Size(306, 340);
            this.panel_OrderAbout.TabIndex = 2;
            // 
            // groupBox_OrderInfo
            // 
            this.groupBox_OrderInfo.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox_OrderInfo.Controls.Add(this.richTextBoxEx1);
            this.groupBox_OrderInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_OrderInfo.Location = new System.Drawing.Point(0, 0);
            this.groupBox_OrderInfo.Name = "groupBox_OrderInfo";
            this.groupBox_OrderInfo.Size = new System.Drawing.Size(302, 336);
            this.groupBox_OrderInfo.TabIndex = 0;
            this.groupBox_OrderInfo.TabStop = false;
            this.groupBox_OrderInfo.Text = "Описание заявки";
            // 
            // richTextBoxEx1
            // 
            this.richTextBoxEx1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxEx1.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxEx1.Name = "richTextBoxEx1";
            this.richTextBoxEx1.Size = new System.Drawing.Size(296, 317);
            this.richTextBoxEx1.TabIndex = 0;
            this.richTextBoxEx1.Text = "";
            // 
            // splitter_Preview
            // 
            this.splitter_Preview.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter_Preview.Location = new System.Drawing.Point(0, 193);
            this.splitter_Preview.Name = "splitter_Preview";
            this.splitter_Preview.Size = new System.Drawing.Size(306, 4);
            this.splitter_Preview.TabIndex = 1;
            this.splitter_Preview.TabStop = false;
            // 
            // panel_Preview
            // 
            this.panel_Preview.BackColor = System.Drawing.SystemColors.Window;
            this.panel_Preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Preview.Controls.Add(this.pictureBoxPreview);
            this.panel_Preview.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Preview.Location = new System.Drawing.Point(0, 0);
            this.panel_Preview.Name = "panel_Preview";
            this.panel_Preview.Size = new System.Drawing.Size(306, 193);
            this.panel_Preview.TabIndex = 0;
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(302, 189);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 0;
            this.pictureBoxPreview.TabStop = false;
            // 
            // splitter_Left
            // 
            this.splitter_Left.Location = new System.Drawing.Point(333, 3);
            this.splitter_Left.Name = "splitter_Left";
            this.splitter_Left.Size = new System.Drawing.Size(4, 537);
            this.splitter_Left.TabIndex = 2;
            this.splitter_Left.TabStop = false;
            // 
            // panel_Left
            // 
            this.panel_Left.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Left.Controls.Add(this.panel_History);
            this.panel_Left.Controls.Add(this.splitter_History);
            this.panel_Left.Controls.Add(this.panel_Treeview);
            this.panel_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_Left.Location = new System.Drawing.Point(3, 3);
            this.panel_Left.Name = "panel_Left";
            this.panel_Left.Size = new System.Drawing.Size(330, 537);
            this.panel_Left.TabIndex = 1;
            // 
            // panel_History
            // 
            this.panel_History.BackColor = System.Drawing.SystemColors.Window;
            this.panel_History.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_History.Controls.Add(this.groupBox_History);
            this.panel_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_History.Location = new System.Drawing.Point(0, 215);
            this.panel_History.Name = "panel_History";
            this.panel_History.Size = new System.Drawing.Size(330, 322);
            this.panel_History.TabIndex = 2;
            // 
            // groupBox_History
            // 
            this.groupBox_History.Controls.Add(this.dataGridView_History);
            this.groupBox_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_History.Location = new System.Drawing.Point(0, 0);
            this.groupBox_History.Name = "groupBox_History";
            this.groupBox_History.Size = new System.Drawing.Size(326, 318);
            this.groupBox_History.TabIndex = 0;
            this.groupBox_History.TabStop = false;
            this.groupBox_History.Text = "История заявки";
            // 
            // dataGridView_History
            // 
            this.dataGridView_History.AllowUserToAddRows = false;
            this.dataGridView_History.AllowUserToDeleteRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView_History.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridView_History.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView_History.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_History.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_History.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView_History.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_History.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridView_History.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_History.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uid,
            this.Datetime,
            this.Notes,
            this.User,
            this.Column2});
            this.dataGridView_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_History.Location = new System.Drawing.Point(3, 16);
            this.dataGridView_History.Name = "dataGridView_History";
            this.dataGridView_History.ReadOnly = true;
            this.dataGridView_History.RowHeadersVisible = false;
            this.dataGridView_History.Size = new System.Drawing.Size(320, 299);
            this.dataGridView_History.TabIndex = 0;
            // 
            // uid
            // 
            this.uid.DataPropertyName = "id";
            dataGridViewCellStyle18.Format = "N0";
            dataGridViewCellStyle18.NullValue = "0";
            this.uid.DefaultCellStyle = dataGridViewCellStyle18;
            this.uid.HeaderText = "id";
            this.uid.Name = "uid";
            this.uid.ReadOnly = true;
            this.uid.Visible = false;
            // 
            // Datetime
            // 
            this.Datetime.DataPropertyName = "Datetime_date";
            this.Datetime.HeaderText = "Время";
            this.Datetime.Name = "Datetime";
            this.Datetime.ReadOnly = true;
            this.Datetime.Width = 90;
            // 
            // Notes
            // 
            this.Notes.DataPropertyName = "note";
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Notes.DefaultCellStyle = dataGridViewCellStyle19;
            this.Notes.HeaderText = "Описание";
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            this.Notes.Width = 130;
            // 
            // User
            // 
            this.User.DataPropertyName = "adder";
            this.User.HeaderText = "Пользователь";
            this.User.Name = "User";
            this.User.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // splitter_History
            // 
            this.splitter_History.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter_History.Location = new System.Drawing.Point(0, 211);
            this.splitter_History.Name = "splitter_History";
            this.splitter_History.Size = new System.Drawing.Size(330, 4);
            this.splitter_History.TabIndex = 1;
            this.splitter_History.TabStop = false;
            // 
            // panel_Treeview
            // 
            this.panel_Treeview.BackColor = System.Drawing.SystemColors.Window;
            this.panel_Treeview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Treeview.Controls.Add(this.orderMenuTreeView1);
            this.panel_Treeview.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Treeview.Location = new System.Drawing.Point(0, 0);
            this.panel_Treeview.Name = "panel_Treeview";
            this.panel_Treeview.Size = new System.Drawing.Size(330, 211);
            this.panel_Treeview.TabIndex = 0;
            // 
            // orderMenuTreeView1
            // 
            this.orderMenuTreeView1.AllowDrop = true;
            this.orderMenuTreeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.orderMenuTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderMenuTreeView1.ImageIndex = 0;
            this.orderMenuTreeView1.ImageList = this.imageList1;
            this.orderMenuTreeView1.ItemHeight = 20;
            this.orderMenuTreeView1.Location = new System.Drawing.Point(0, 0);
            this.orderMenuTreeView1.Name = "orderMenuTreeView1";
            treeNode1.ImageIndex = 10;
            treeNode1.Name = "nodeDraft";
            treeNode1.SelectedImageIndex = 10;
            treeNode1.Text = "Черновики";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "nodeAwait";
            treeNode2.SelectedImageIndex = 1;
            treeNode2.Text = "Ожидают";
            treeNode3.ImageKey = "printer.png";
            treeNode3.Name = "nodeInWork";
            treeNode3.SelectedImageIndex = 28;
            treeNode3.Text = "В работе";
            treeNode4.ImageKey = "wrench_orange.png";
            treeNode4.Name = "nodePostProc";
            treeNode4.SelectedImageKey = "wrench_orange.png";
            treeNode4.Text = "Постобработка";
            treeNode5.ImageKey = "Archive_16x16.png";
            treeNode5.Name = "nodeStock";
            treeNode5.SelectedImageKey = "Archive_16x16.png";
            treeNode5.Text = "Сделаны (склад)";
            treeNode6.ImageKey = "table_lightning.png";
            treeNode6.Name = "nodeOpen";
            treeNode6.SelectedImageKey = "table_lightning.png";
            treeNode6.Text = "Открыты";
            treeNode7.ImageKey = "lock.png";
            treeNode7.Name = "nodeArchive";
            treeNode7.SelectedImageKey = "lock.png";
            treeNode7.Text = "Закрыты (архив)";
            treeNode7.ToolTipText = "Заявки, отданые клиенту.";
            treeNode8.ImageKey = "table_error.png";
            treeNode8.Name = "nodeStopped";
            treeNode8.SelectedImageKey = "table_error.png";
            treeNode8.Text = "Остановлены";
            treeNode9.ImageKey = "table.png";
            treeNode9.Name = "nodeAll";
            treeNode9.SelectedImageKey = "table.png";
            treeNode9.Text = "Все заявки";
            treeNode10.ImageKey = "trash-aqua-full-icon.png";
            treeNode10.Name = "nodeBasket";
            treeNode10.SelectedImageKey = "trash-aqua-full-icon.png";
            treeNode10.Text = "Корзина";
            this.orderMenuTreeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            this.orderMenuTreeView1.SelectedImageIndex = 0;
            this.orderMenuTreeView1.SelectedNode_id = 0;
            this.orderMenuTreeView1.SelectedNode_Name = "";
            this.orderMenuTreeView1.Size = new System.Drawing.Size(326, 207);
            this.orderMenuTreeView1.TabIndex = 0;
            this.orderMenuTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.orderMenuTreeView1_AfterSelect);
            this.orderMenuTreeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.orderMenuTreeView1_DragDrop);
            this.orderMenuTreeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.orderMenuTreeView1_DragOver);
            this.orderMenuTreeView1.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.orderMenuTreeView1_QueryContinueDrag);
            this.orderMenuTreeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.orderMenuTreeView1_MouseDown);
            // 
            // toolStrip_Worktypes
            // 
            this.toolStrip_Worktypes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_Worktypes.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_Worktypes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.Button_OrderViewAll,
            this.Button_OrderViewPrint,
            this.Button_OrderViewCut,
            this.Button_OrderViewCnc,
            this.Button_OrderViewInstall});
            this.toolStrip_Worktypes.Location = new System.Drawing.Point(3, 540);
            this.toolStrip_Worktypes.Name = "toolStrip_Worktypes";
            this.toolStrip_Worktypes.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip_Worktypes.Size = new System.Drawing.Size(1391, 25);
            this.toolStrip_Worktypes.TabIndex = 4;
            this.toolStrip_Worktypes.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel1.Text = "Тип работ:";
            // 
            // Button_OrderViewAll
            // 
            this.Button_OrderViewAll.Checked = true;
            this.Button_OrderViewAll.CheckOnClick = true;
            this.Button_OrderViewAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Button_OrderViewAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_OrderViewAll.Image = ((System.Drawing.Image)(resources.GetObject("Button_OrderViewAll.Image")));
            this.Button_OrderViewAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderViewAll.Name = "Button_OrderViewAll";
            this.Button_OrderViewAll.Size = new System.Drawing.Size(29, 22);
            this.Button_OrderViewAll.Text = "все";
            this.Button_OrderViewAll.CheckedChanged += new System.EventHandler(this.Button_OrderViewAll_CheckedChanged);
            // 
            // Button_OrderViewPrint
            // 
            this.Button_OrderViewPrint.CheckOnClick = true;
            this.Button_OrderViewPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_OrderViewPrint.Enabled = false;
            this.Button_OrderViewPrint.Image = global::AccentBase.Properties.Resources.printer_empty;
            this.Button_OrderViewPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderViewPrint.Name = "Button_OrderViewPrint";
            this.Button_OrderViewPrint.Size = new System.Drawing.Size(48, 22);
            this.Button_OrderViewPrint.Text = "печать";
            this.Button_OrderViewPrint.CheckedChanged += new System.EventHandler(this.Button_OrderViewPrint_CheckedChanged);
            // 
            // Button_OrderViewCut
            // 
            this.Button_OrderViewCut.CheckOnClick = true;
            this.Button_OrderViewCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_OrderViewCut.Enabled = false;
            this.Button_OrderViewCut.Image = global::AccentBase.Properties.Resources.plotter_icon;
            this.Button_OrderViewCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderViewCut.Name = "Button_OrderViewCut";
            this.Button_OrderViewCut.Size = new System.Drawing.Size(41, 22);
            this.Button_OrderViewCut.Text = "резка";
            this.Button_OrderViewCut.CheckedChanged += new System.EventHandler(this.Button_OrderViewCut_CheckedChanged);
            // 
            // Button_OrderViewCnc
            // 
            this.Button_OrderViewCnc.CheckOnClick = true;
            this.Button_OrderViewCnc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_OrderViewCnc.Enabled = false;
            this.Button_OrderViewCnc.Image = global::AccentBase.Properties.Resources.PrintingCncMachineIcon;
            this.Button_OrderViewCnc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderViewCnc.Name = "Button_OrderViewCnc";
            this.Button_OrderViewCnc.Size = new System.Drawing.Size(76, 22);
            this.Button_OrderViewCnc.Text = "фрезеровка";
            this.Button_OrderViewCnc.CheckedChanged += new System.EventHandler(this.Button_OrderViewCnc_CheckedChanged);
            // 
            // Button_OrderViewInstall
            // 
            this.Button_OrderViewInstall.CheckOnClick = true;
            this.Button_OrderViewInstall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_OrderViewInstall.Enabled = false;
            this.Button_OrderViewInstall.Image = global::AccentBase.Properties.Resources.lorry;
            this.Button_OrderViewInstall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_OrderViewInstall.Name = "Button_OrderViewInstall";
            this.Button_OrderViewInstall.Size = new System.Drawing.Size(120, 22);
            this.Button_OrderViewInstall.Text = "монтажные работы";
            this.Button_OrderViewInstall.CheckedChanged += new System.EventHandler(this.Button_OrderViewInstall_CheckedChanged);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Location = new System.Drawing.Point(380, 3);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.miniToolStrip.Size = new System.Drawing.Size(1391, 25);
            this.miniToolStrip.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.TabOrder);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Location = new System.Drawing.Point(0, 49);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1405, 597);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 3;
            // 
            // contextMenuStrip_treeview
            // 
            this.contextMenuStrip_treeview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Treeview_newOrder,
            this.ToolStripMenuItem_ClearBasket});
            this.contextMenuStrip_treeview.Name = "contextMenuStrip_treeview";
            this.contextMenuStrip_treeview.Size = new System.Drawing.Size(147, 48);
            // 
            // Treeview_newOrder
            // 
            this.Treeview_newOrder.Image = global::AccentBase.Properties.Resources.page_white_add;
            this.Treeview_newOrder.Name = "Treeview_newOrder";
            this.Treeview_newOrder.Size = new System.Drawing.Size(146, 22);
            this.Treeview_newOrder.Text = "Новая заявка";
            this.Treeview_newOrder.Click += new System.EventHandler(this.Treeview_newOrder_Click);
            // 
            // ToolStripMenuItem_ClearBasket
            // 
            this.ToolStripMenuItem_ClearBasket.Image = global::AccentBase.Properties.Resources.trash_aqua_empty_icon;
            this.ToolStripMenuItem_ClearBasket.Name = "ToolStripMenuItem_ClearBasket";
            this.ToolStripMenuItem_ClearBasket.Size = new System.Drawing.Size(146, 22);
            this.ToolStripMenuItem_ClearBasket.Text = "Очистить";
            this.ToolStripMenuItem_ClearBasket.Click += new System.EventHandler(this.очиститьToolStripMenuItem_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            dataGridViewCellStyle20.Format = "N0";
            dataGridViewCellStyle20.NullValue = "0";
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle20;
            this.dataGridViewTextBoxColumn1.FillWeight = 40F;
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Datetime_date";
            dataGridViewCellStyle21.Format = "g";
            dataGridViewCellStyle21.NullValue = null;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridViewTextBoxColumn2.HeaderText = "Время";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "note";
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle22;
            this.dataGridViewTextBoxColumn3.HeaderText = "Описание";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 140;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "adder";
            this.dataGridViewTextBoxColumn4.HeaderText = "Column2";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 300;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "id";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle23.Format = "N0";
            dataGridViewCellStyle23.NullValue = "0";
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle23;
            this.dataGridViewTextBoxColumn6.FillWeight = 40F;
            this.dataGridViewTextBoxColumn6.HeaderText = "uid";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 40;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "notes";
            this.dataGridViewTextBoxColumn7.HeaderText = "Примечание";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 300;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "note";
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle24;
            this.dataGridViewTextBoxColumn8.HeaderText = "Описание";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 140;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "adder";
            this.dataGridViewTextBoxColumn9.HeaderText = "Пользователь";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn10.HeaderText = "";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // StartDate
            // 
            this.StartDate.DataPropertyName = "dt_date_insert";
            dataGridViewCellStyle25.Format = "g";
            dataGridViewCellStyle25.NullValue = null;
            this.StartDate.DefaultCellStyle = dataGridViewCellStyle25;
            this.StartDate.HeaderText = "Дата создания";
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            // 
            // OrderName
            // 
            this.OrderName.DataPropertyName = "name";
            this.OrderName.HeaderText = "Наименование";
            this.OrderName.Name = "OrderName";
            this.OrderName.ReadOnly = true;
            this.OrderName.Width = 200;
            // 
            // filling
            // 
            this.filling.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.filling.HeaderText = "";
            this.filling.Name = "filling";
            this.filling.ReadOnly = true;
            // 
            // dataGridViewRichTextBoxColumn1
            // 
            this.dataGridViewRichTextBoxColumn1.HeaderText = "Column3";
            this.dataGridViewRichTextBoxColumn1.Name = "dataGridViewRichTextBoxColumn1";
            // 
            // FormBase
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1405, 668);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel_QuickSearch);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip_Main);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormBase";
            this.Text = "База данных заданий компании \"Акцент\"";
            this.Activated += new System.EventHandler(this.FormBase_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBase_FormClosing);
            this.Load += new System.EventHandler(this.FormBase_Load);
            this.Shown += new System.EventHandler(this.FormBase_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip_Orders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_QuickSearch_Customers)).EndInit();
            this.panel_QuickSearch.ResumeLayout(false);
            this.panel_QuickSearch.PerformLayout();
            this.toolStrip_Main.ResumeLayout(false);
            this.toolStrip_Main.PerformLayout();
            this.TabOrder.ResumeLayout(false);
            this.TabOrder.PerformLayout();
            this.panel_Center.ResumeLayout(false);
            this.panel_Center.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView_Orders)).EndInit();
            this.toolStrip_OrderDatagridview.ResumeLayout(false);
            this.toolStrip_OrderDatagridview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SocketSend)).EndInit();
            this.panel_Right.ResumeLayout(false);
            this.panel_OrderAbout.ResumeLayout(false);
            this.groupBox_OrderInfo.ResumeLayout(false);
            this.panel_Preview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.panel_Left.ResumeLayout(false);
            this.panel_History.ResumeLayout(false);
            this.groupBox_History.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_History)).EndInit();
            this.panel_Treeview.ResumeLayout(false);
            this.toolStrip_Worktypes.ResumeLayout(false);
            this.toolStrip_Worktypes.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.contextMenuStrip_treeview.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox_QuickSearch_Customers;
        private System.Windows.Forms.Panel panel_QuickSearch;
        private System.Windows.Forms.ToolStripMenuItem Menu_Orders;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStrip toolStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem MenuService;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_CheckUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem справкаAccentBaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem MenuService__Settings;
        private CustomControls.DataGridViewRichTextBoxColumn dataGridViewRichTextBoxColumn1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Orders;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_OrderSave;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_Stop;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Basket;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Remove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_PrintON;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_CutON;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_CncON;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Stock;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStrip_Open;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Closed;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton Button_OrderNew;
        private System.Windows.Forms.ToolStripButton Button_OrderEdit;
        private System.Windows.Forms.ToolStripButton Button_OrderCopy;
        private System.Windows.Forms.ToolStripButton Button_OrderPdfSave;
        private System.Windows.Forms.ToolStripButton Button_OrderPrint;
        private System.Windows.Forms.ToolStripButton Button_OrderDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton Button_FtpFiles;
        private System.Windows.Forms.ToolStripButton Button_Settings;
        private System.Windows.Forms.ToolStripButton Button_Messenger;
        private System.Windows.Forms.ToolStripButton Button_OrderFilesDownload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton Button_Stock;
        private System.Windows.Forms.ToolStripMenuItem Menu_NewOrder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripButton Button_Help;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.TabPage TabOrder;
        private System.Windows.Forms.Panel panel_Center;
        private CustomControls.OrdersDataGridView customDataGridView_Orders;
        private System.Windows.Forms.ToolStrip toolStrip_OrderDatagridview;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Splitter splitter_Right;
        private System.Windows.Forms.Panel panel_Right;
        private System.Windows.Forms.Panel panel_OrderAbout;
        private System.Windows.Forms.GroupBox groupBox_OrderInfo;
        private CustomControls.RichTextBoxEx richTextBoxEx1;
        private System.Windows.Forms.Splitter splitter_Preview;
        private System.Windows.Forms.Panel panel_Preview;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Splitter splitter_Left;
        private System.Windows.Forms.Panel panel_Left;
        private System.Windows.Forms.Panel panel_History;
        private System.Windows.Forms.GroupBox groupBox_History;
        private System.Windows.Forms.DataGridView dataGridView_History;
        private System.Windows.Forms.Splitter splitter_History;
        private System.Windows.Forms.Panel panel_Treeview;
        private CustomControls.OrderMenuTreeView orderMenuTreeView1;
        private System.Windows.Forms.ToolStrip toolStrip_Worktypes;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton Button_OrderViewAll;
        private System.Windows.Forms.ToolStripButton Button_OrderViewPrint;
        private System.Windows.Forms.ToolStripButton Button_OrderViewCut;
        private System.Windows.Forms.ToolStripButton Button_OrderViewCnc;
        private System.Windows.Forms.ToolStripButton Button_OrderViewInstall;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem Menu_OrderCopy;
        private System.Windows.Forms.ToolStripMenuItem Menu_OrderEdit;
        private System.Windows.Forms.ToolStripMenuItem Menu_OrderDelete;
        private System.Windows.Forms.ToolStripMenuItem Menu_OrderPrint;
        private System.Windows.Forms.ToolStripMenuItem Menu_OrderPdfSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem Menu_OrderFilesDownload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem Menu_OrderExit;
        private System.Windows.Forms.ToolStripMenuItem Menu_AppExit;
        private System.Windows.Forms.ToolStripMenuItem MenuView_Orders;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderTimeStart;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderTimeEnd;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderWorkTypes;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderMaterial;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderManager;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderCustomer;
        private System.Windows.Forms.ToolStripMenuItem MenuView_ToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderPreview;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderNotes;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderHistory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem MenuView_QuickSearch;
        private System.Windows.Forms.ToolStripMenuItem MenuView_OrderListInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
        private System.Windows.Forms.ToolStripMenuItem MenuView_Messenger;
        private System.Windows.Forms.ToolStripMenuItem MenuView_FtpList;
        private System.Windows.Forms.ToolStripMenuItem MenuView_Stock;
        private System.Windows.Forms.ToolStripMenuItem MenuService_SocketServerTransmite;
        private System.Windows.Forms.ToolStripMenuItem MenuView_Worktypes;
        private System.Windows.Forms.ToolStripMenuItem MenuService_FtpTransmite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
        private System.Windows.Forms.ToolStripMenuItem MenuService_WinCalc;
        private System.Windows.Forms.ToolStripMenuItem MenuView_GoogleTable;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_treeview;
        private System.Windows.Forms.ToolStripMenuItem Treeview_newOrder;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewAllOrders;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator24;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewDraft;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewAwait;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewInWork;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewPostProc;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewStock;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewStopped;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripMenuItem MenuView_TreeViewArchive;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn filling;
        private System.Windows.Forms.Splitter splitter_SocketSend;
        private System.Windows.Forms.ToolStripMenuItem MenuView_SocketSend;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridView dataGridView_SocketSend;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.ToolStripButton Button_GoogleTable;
        private System.Windows.Forms.ToolStripButton Button_Calc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator25;
        private System.Windows.Forms.DataGridViewTextBoxColumn uid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Datetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_InstallON;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ChangeState;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Await;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_InWork;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_PostProc;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_DraftDouble;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ClosedDouble;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_StockDouble;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_PostProcDouble;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_InworkDouble;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_AwaiteDouble;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Stoped;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_InstallPrint;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ClearBasket;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox_QuickSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn String_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Datetime_date_start;
        private System.Windows.Forms.DataGridViewTextBoxColumn String_date_start;
        private System.Windows.Forms.DataGridViewTextBoxColumn Datetime_dead_line;
        private System.Windows.Forms.DataGridViewTextBoxColumn String_dead_line;
        private System.Windows.Forms.DataGridViewTextBoxColumn work_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn client;
        private System.Windows.Forms.DataGridViewImageColumn Image_WorkTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Materials;
        private System.Windows.Forms.DataGridViewTextBoxColumn manager;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn time_recieve;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderState;
    }
}