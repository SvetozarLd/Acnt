using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentBase.CustomControls
{
    public partial class DataGridViewPanel : Component
    {
        public DataGridViewPanel()
        {
            InitializeComponent();
        }

        public DataGridViewPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
