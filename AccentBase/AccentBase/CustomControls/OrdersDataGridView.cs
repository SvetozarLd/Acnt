using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace AccentBase.CustomControls
{
    public class OrdersDataGridView : DataGridView
    {
        [DllImport("user32.dll")] private static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam); private const int WM_SETREDRAW = 11;
        //#region Событие при окончании  или отчёте
        //#region Аргументы к событию 
        //public class MyDataGridEventArgs : EventArgs
        //{
        //    public int Count { get; set; }
        //    public MyDataGridEventArgs(int count) { Count = count;}
        //}
        //#endregion
        //public delegate void MyDataGridEventHandler(object sender, MyDataGridEventArgs e);
        //public event MyDataGridEventHandler SortedEvent;
        //private void OnError(MyDataGridEventArgs e) { ErrorEvent?.Invoke(null, e); }
        //#endregion
        //OrdersDataGridView()
        //{
        //    // Double buffering can make DGV slow in remote desktop 
        //    if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
        //    {
        //        Type dgvType = this.GetType();
        //        PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        //        pi.SetValue(this, value, null); }
        //}


        private bool _delayedMouseDown = false;
        private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;

        private Func<object> _getDragData = null;
        public void EnableDragDrop(Func<object> getDragData)
        {
            _getDragData = getDragData;
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);

            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                int currentRow = CurrentRow.Index;
                List<DataGridViewRow> selectedRows = SelectedRows.OfType<DataGridViewRow>().ToList();
                bool clickedRowSelected = Rows[e.RowIndex].Selected;

                CurrentCell = Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Select previously selected rows, if control is down or the clicked row was already selected
                if ((Control.ModifierKeys & Keys.Control) != 0 || clickedRowSelected)
                {
                    selectedRows.ForEach(row => row.Selected = true);
                }

                // Select a range of new rows, if shift key is down
                if ((Control.ModifierKeys & Keys.Shift) != 0)
                {
                    for (int i = currentRow; i != e.RowIndex; i += Math.Sign(e.RowIndex - currentRow))
                    {
                        Rows[i].Selected = true;
                    }
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            int rowIndex = base.HitTest(e.X, e.Y).RowIndex;
            _delayedMouseDown = (rowIndex >= 0 &&
                (SelectedRows.Contains(Rows[rowIndex]) || (ModifierKeys & Keys.Control) > 0));

            if (!_delayedMouseDown)
            {
                base.OnMouseDown(e);

                if (rowIndex >= 0)
                {
                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    _dragBoxFromMouseDown = new Rectangle(
                        new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
                else
                {
                    // Reset the rectangle if the mouse is not over an item in the datagridview.
                    _dragBoxFromMouseDown = Rectangle.Empty;
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Perform the delayed mouse down before the mouse up
            if (_delayedMouseDown)
            {
                _delayedMouseDown = false;
                base.OnMouseDown(e);
            }

            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // If the mouse moves outside the rectangle, start the drag.
            if (_getDragData != null && (e.Button & MouseButtons.Left) > 0 &&
                _dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
            {
                if (_delayedMouseDown)
                {
                    _delayedMouseDown = false;
                    if ((ModifierKeys & Keys.Control) > 0)
                    {
                        base.OnMouseDown(e);
                    }
                }

                // Proceed with the drag and drop, passing in the drag data
                object dragData = _getDragData();
                if (dragData != null)
                {
                    DoDragDrop(dragData, DragDropEffects.Move);
                }
            }
        }


        //_________________________
        protected override bool ShowFocusCues => false;

        //public event EventHandler<HandledMouseEventArgs> PreMouseDown;

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    if (PreMouseDown != null)
        //    {
        //        HandledMouseEventArgs pe = new HandledMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta);
        //        PreMouseDown(this, pe);
        //        if (pe.Handled) return;
        //    }
        //    base.OnMouseDown(e);
        //}




        #region Событие при ошибке или отчёте
        #region Аргументы к событию 
        public class MyDataGridEventArgs : EventArgs
        {
            public string Text { get; set; }
            public Exception Ex { get; set; }
            public MyDataGridEventArgs(string text, Exception ex) { Text = text; Ex = ex; }
        }
        #endregion
        public delegate void MyDataGridEventHandler(object sender, MyDataGridEventArgs e);
        public event MyDataGridEventHandler ErrorEvent;
        private void OnError(MyDataGridEventArgs e) { ErrorEvent?.Invoke(null, e); }
        #endregion

        #region Класс для сортировки
        public class SortColumn
        {
            public List<string> Name { get; set; }
            public ListSortDirection Sorting { get; set; }
            public SortColumn(List<string> name, ListSortDirection sorting) { Name = name; Sorting = sorting; }
        }
        #endregion

        private DataTable sourceTable = null; // таблица - источник
        private string filter = string.Empty; // строка фильтрации
        private string sorting = string.Empty; // строка сортировки
        // Так как тут мы не знаем что за столбцы будут в таблице и вообще таблица неизвестная,
        // то, чтобы не лазить по таблице постоянно, сохраним имена столбцов  
        private readonly List<string> columnsNames = new List<string>();

        #region Взять или отдать исходную таблицу
        public DataTable SourceTable
        {
            get => sourceTable;
            set
            {
                if (value != null && value.Columns != null && value.Columns.Count > 1)
                {
                    if (sourceTable != null) { sourceTable.Clear(); }
                    sourceTable = value.Copy();
                    //createColumns();
                    ReFresh();
                    OnError(new MyDataGridEventArgs(DateTime.Now.ToString("HH:mm:ss") + ">> Таблица к датагриду подключена.", null));
                }
            }
        }
        #endregion
        #region Отдать отфильтрованную таблицу
        public DataTable FilteredTable => filteredTable();
        #endregion




        #region Фильтр - поиск
        public bool FilterOfAllOrders { get; set; }

        public string Filter
        {
            get => filter;
            set
            {
                if (sourceTable != null)
                {
                    if (SortedColumn != null)
                    {
                        sorting1 = SortedColumn.DataPropertyName;
                        switch (SortOrder)
                        {
                            case SortOrder.Ascending:
                                sorting1 = sorting1 + " ASC";
                                break;
                            default:
                                sorting1 = sorting1 + " DESC";
                                break;
                        }
                        //sorting1 = sorting1 + " " + this.SortOrder;
                    }
                    filter = filtration(value);
                    ReFresh();
                }
            }
        }
        #endregion

        #region Фильтр по типам работ
        private bool filterWorktypesAll = true;
        private bool filterprint = false;
        private bool filtercut = false;
        private bool filtercnc = false;
        private bool filterinstall = false;

        private StringBuilder FiltersWorktypes = new StringBuilder();


        private void FilterOfWorktypes()
        {
            FiltersWorktypes = new StringBuilder();
            if (filterWorktypesAll)
            {
                FiltersWorktypes.Clear();
                return;
            }
            else
            {
                if (!filterprint && !filtercut && !filtercnc & !filterinstall)
                {
                    FiltersWorktypes.Append("print_on = 0 AND cut_on = 0 AND cnc_on = 0 AND installation = 0");
                }
                else
                {
                    if (filterprint) { FiltersWorktypes.Append("print_on = 1"); }
                    if (filtercut)
                    {
                        if (FiltersWorktypes.Length > 0) { FiltersWorktypes.Append(" OR "); }
                        FiltersWorktypes.Append("cut_on = 1");
                    }
                    if (filtercnc)
                    {
                        if (FiltersWorktypes.Length > 0) { FiltersWorktypes.Append(" OR "); }
                        FiltersWorktypes.Append("cnc_on = 1");
                    }
                    if (filterinstall)
                    {
                        if (FiltersWorktypes.Length > 0) { FiltersWorktypes.Append(" OR "); }
                        FiltersWorktypes.Append("installation = 1");
                    }
                }
            }

        }




        public bool FilterAll
        {
            get => filterWorktypesAll;
            set
            {
                if (sourceTable != null)
                {
                    if (SortedColumn != null)
                    {
                        sorting1 = SortedColumn.DataPropertyName;
                        switch (SortOrder)
                        {
                            case SortOrder.Ascending:
                                sorting1 = sorting1 + " ASC";
                                break;
                            default:
                                sorting1 = sorting1 + " DESC";
                                break;
                        }
                    }
                    filterWorktypesAll = value;
                    FilterOfWorktypes();
                    ReFresh();
                }
            }
        }
        public bool FilterPrint
        {
            get => filterprint;
            set
            {
                if (sourceTable != null)
                {
                    if (SortedColumn != null)
                    {
                        sorting1 = SortedColumn.DataPropertyName;
                        switch (SortOrder)
                        {
                            case SortOrder.Ascending:
                                sorting1 = sorting1 + " ASC";
                                break;
                            default:
                                sorting1 = sorting1 + " DESC";
                                break;
                        }
                    }
                    filterprint = value;
                    FilterOfWorktypes();
                    ReFresh();
                }
            }
        }
        public bool FilterCut
        {
            get => filtercut;
            set
            {
                if (sourceTable != null)
                {
                    if (SortedColumn != null)
                    {
                        sorting1 = SortedColumn.DataPropertyName;
                        switch (SortOrder)
                        {
                            case SortOrder.Ascending:
                                sorting1 = sorting1 + " ASC";
                                break;
                            default:
                                sorting1 = sorting1 + " DESC";
                                break;
                        }
                    }
                    filtercut = value;
                    FilterOfWorktypes();
                    ReFresh();
                }
            }
        }
        public bool FilterCnc
        {
            get => filtercnc;
            set
            {
                if (sourceTable != null)
                {
                    if (SortedColumn != null)
                    {
                        sorting1 = SortedColumn.DataPropertyName;
                        switch (SortOrder)
                        {
                            case SortOrder.Ascending:
                                sorting1 = sorting1 + " ASC";
                                break;
                            default:
                                sorting1 = sorting1 + " DESC";
                                break;
                        }
                    }
                    filtercnc = value;
                    FilterOfWorktypes();
                    ReFresh();
                }
            }
        }
        public bool FilterInstall
        {
            get => filterinstall;
            set
            {
                if (sourceTable != null)
                {
                    if (SortedColumn != null)
                    {
                        sorting1 = SortedColumn.DataPropertyName;
                        switch (SortOrder)
                        {
                            case SortOrder.Ascending:
                                sorting1 = sorting1 + " ASC";
                                break;
                            default:
                                sorting1 = sorting1 + " DESC";
                                break;
                        }
                    }
                    filterinstall = value;
                    FilterOfWorktypes();
                    ReFresh();
                }
            }
        }

        #endregion

        #region Фильтр по статусу
        private string filterStatus = string.Empty;
        public string FilterStatus
        {
            get { return filterStatus; }
            set
            {
                if (sourceTable != null)
                {
                    if (SortedColumn != null)
                    {
                        sorting1 = SortedColumn.DataPropertyName;
                        switch (SortOrder)
                        {
                            case SortOrder.Ascending:
                                sorting1 = sorting1 + " ASC";
                                break;
                            default:
                                sorting1 = sorting1 + " DESC";
                                break;
                        }
                    }
                    filterStatus = value;
                    ReFresh();
                }
            }
        }
        #endregion

        #region Сортировка
        public string MySort
        {
            get => sorting;
            set
            {
                if (sourceTable != null)
                {
                    sorting = value;
                    ReFresh();
                }
            }
        }
        #endregion
        #region Обновить датагрид
        public void ReFresh()
        {
            SendMessage(Handle, WM_SETREDRAW, false, 0);
            #region Запомним id выделеной строки
            string selectedrowid = string.Empty;
            if (SelectedRows.Count > 0)
            {
                selectedrowid = SelectedRows[0].Cells["id"].Value.ToString();
            }
            #endregion
            DataSource = null;
            DataSource = filteredTable();
            if ((DataSource as DataTable) != null && (DataSource as DataTable).Rows.Count > 0)
            {
                #region найдём строку с таким id  в коллекции, очистим веделенное, выделим эту строку и сделаем скроллинг на неё, чтоб была центру. 
                if (!selectedrowid.Equals(string.Empty) && Rows.Count > 0)
                {
                    DataGridViewRow row = Rows.Cast<DataGridViewRow>().Where(r => r.Cells["id"].Value.ToString().Equals(selectedrowid)).FirstOrDefault();
                    if (row != null)
                    {
                        CurrentCell = row.Cells["id"];
                        ClearSelection();
                        row.Selected = true;
                        int i = row.Index - (DisplayedRowCount(true) / 2);
                        if (i < 0) { i = 0; }
                        FirstDisplayedScrollingRowIndex = i;
                        //this.Rows[row.Index].Selected = true;
                    }
                    //this.SelectedRows[0] = this.Rows[];
                }
                #endregion
            }
            else { ClearSelection(); }
            SendMessage(Handle, WM_SETREDRAW, true, 0); Refresh();
        }
        #endregion

        //#region Создать копию структуры таблицы в Datagrid. Это неправильно, долго и тормознуто. Подключать можно и сразу или заранее, если известна структура данных. Тут просто как пример.
        //private void createColumns()
        //{
        //    try
        //    {
        //        if (sourceTable != null)
        //        {
        //            this.DataSource = null;
        //            this.Columns.Clear();
        //            this.AutoGenerateColumns = false;
        //            foreach (DataColumn item in sourceTable.Columns)
        //            {
        //                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
        //                column.Name = item.ColumnName;
        //                column.HeaderText = item.ColumnName;
        //                column.DataPropertyName = item.ColumnName;
        //                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
        //                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //                if (column.Name.Equals("id")) { column.Visible = false; } else { column.Visible = true; columnsNames.Add(column.Name); } // поиск по ID делать не будем.
        //                this.Columns.Add(column);
        //            }
        //            DataGridViewTextBoxColumn columnFilling = new DataGridViewTextBoxColumn();
        //            columnFilling.Name = string.Empty;
        //            columnFilling.HeaderText = string.Empty;
        //            columnFilling.DataPropertyName = string.Empty;
        //            columnFilling.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //            this.Columns.Add(columnFilling);
        //        }
        //        OnError(new MyDataGridEventArgs(DateTime.Now.ToString("HH:mm:ss") + ">> Датагрид - колонки созданы", null));
        //    }
        //    catch (Exception ex)
        //    {
        //        OnError(new MyDataGridEventArgs(DateTime.Now.ToString("HH:mm:ss") + ">> Ошибка:", ex));
        //    }

        //}
        //#endregion
        #region создание строки фильтрации/поиска
        private string filtration(string filterString)
        {
            try
            {
                if (filterString.Equals(string.Empty)) { return string.Empty; }
                StringBuilder tmp = new StringBuilder();
                //int x = 0;

                //if (Int32.TryParse(filterString, out x))
                //{
                //    tmp.Append("id =" + x.ToString());
                //}
                if (!FilterOfAllOrders) { tmp.Append("("); }
                tmp.Append("String_id" + " Like '%" + filterString + "%'");
                if (tmp.Length > 0) { tmp.Append(" OR "); }
                tmp.Append("work_name" + " Like '%" + filterString + "%'");
                if (tmp.Length > 0) { tmp.Append(" OR "); }
                tmp.Append("client" + " Like '%" + filterString + "%'");
                if (tmp.Length > 0) { tmp.Append(" OR "); }
                tmp.Append("String_date_start" + " Like '%" + filterString + "%'");
                if (tmp.Length > 0) { tmp.Append(" OR "); }
                tmp.Append("String_dead_line" + " Like '%" + filterString + "%'");
                if (tmp.Length > 0) { tmp.Append(" OR "); }
                tmp.Append("adder" + " Like '%" + filterString + "%'");

                if (!FilterOfAllOrders) { tmp.Append(") AND (" + FilterStatus + ")"); }
                //if (tmp.Length > 0) { tmp.Append(" OR "); } tmp.Append("Datetime_date_start" + " Like '%" + filterString + "%'");
                //   DateTime dt = new DateTime();
                //if (DateTime.TryParse(filterString,out dt))
                //{
                //       if (tmp.Length > 0) { tmp.Append(" OR "); }
                //       tmp.Append("Datetime_date_start <=#" + dt.ToString("MM/dd/yyyy")+"#");
                //   }

                //dvFormula.RowFilter = "#" + startDate.ToString("MM/dd/yyyy") + "# < EndDate OR EndDate = #1/1/1900#";
                //if (tmp.Length > 0) { tmp.Append(" OR "); }
                //try
                //{
                //    StringBuilder tmp = new StringBuilder();
                //    if (sourceTable != null && this.Rows.Count > 0)
                //    {
                //        foreach (DataGridViewTextBoxColumn columnName in Columns)
                //        {                                                
                //            if (tmp.Length > 0) { tmp.Append(" OR "); }
                //            tmp.Append(columnName.DataPropertyName + " Like '%" + filterString + "%'");
                //        }
                //    }
                //    return tmp.ToString();
                return tmp.ToString();
            }
            catch (Exception ex)
            {
                OnError(new MyDataGridEventArgs(DateTime.Now.ToString("HH:mm:ss") + ">> Ошибка:", ex));
                return string.Empty;
            }
            //return tmp.ToString();
        }

        //private string Sorting(string filterString)
        //{
        //    try
        //    {
        //        StringBuilder tmp = new StringBuilder();
        //        if (sourceTable != null && columnsNames.Count > 0)
        //        {
        //            foreach (string columnName in columnsNames)
        //            {
        //                if (tmp.Length > 0) { tmp.Append(" OR "); }
        //                tmp.Append(columnName + " Like '%" + filterString + "%'");
        //            }
        //        }
        //        return tmp.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        OnError(new MyDataGridEventArgs(DateTime.Now.ToString("HH:mm:ss") + ">> Ошибка:", ex));
        //        return string.Empty;
        //    }
        //}
        #endregion
        #region Фильтрация и сортировка
        private string sorting1 = string.Empty;
        public string SortingOrder
        {
            get => sorting1;
            set { sorting1 = value; filteredTable(); ReFresh(); }
        }


        private DataTable filteredTable()
        {
            try
            {
                //SendMessage(this.Handle, WM_SETREDRAW, false, 0);
                string tmp = string.Empty;
                if (filterStatus == string.Empty)
                {
                    if (filter.Length > 0 && FiltersWorktypes.Length > 0)
                    {
                        tmp = " ( " + filter + " ) AND ( " + FiltersWorktypes.ToString() + " ) ";
                    }
                    else
                    {
                        if (filter.Length > 0) { tmp = filter; }
                        else
                        {
                            tmp = FiltersWorktypes.ToString();
                        }
                    }
                }
                else
                {
                    if (filter.Length > 0 && FiltersWorktypes.Length > 0)
                    {
                        if (FilterOfAllOrders)
                        {
                            tmp = " ( " + filter + " ) AND ( " + FiltersWorktypes.ToString() + " )";
                        }
                        else
                        {
                            tmp = " ( " + filter + " ) AND ( " + FiltersWorktypes.ToString() + " ) AND ( " + filterStatus + " ) ";
                        }
                    }
                    else
                    {
                        if (filter.Length > 0) { tmp = filter; }
                        else
                        {
                            if (FiltersWorktypes.Length > 0)
                            {
                                tmp = " ( " + FiltersWorktypes.ToString() + " ) AND ( " + filterStatus + " ) ";
                            }
                            else { tmp = filterStatus; }
                        }
                    }
                }

                //this.DataSource = null;
                sourceTable.DefaultView.RowFilter = tmp;
                sourceTable.DefaultView.Sort = sorting1;
                return sourceTable.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                OnError(new MyDataGridEventArgs(DateTime.Now.ToString("HH:mm:ss") + ">> Ошибка:", ex));
                return null;
            }
        }
        #endregion

    }
}
