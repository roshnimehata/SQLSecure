using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_GridColumnChooser : Form
    {
        private Form_GridColumnChooser(
                Infragistics.Win.UltraWinGrid.UltraGrid grid,
                string header
            )
        {
            InitializeComponent();

            _ultraGridColumnChooser.SourceGrid = grid;

            if (!string.IsNullOrEmpty(header))
            {
                Text = header;
            }
            else
            {
                Text = "Select Columns";
            }
        }

        public static void Process(
                Infragistics.Win.UltraWinGrid.UltraGrid grid,
                string header
            )
        {
            Debug.Assert(grid != null);

            Form_GridColumnChooser form = new Form_GridColumnChooser(grid, header);
            form.ShowDialog();
        }

        public static void Process(
                Infragistics.Win.UltraWinGrid.UltraGrid grid
            )
        {
            Debug.Assert(grid != null);

            Form_GridColumnChooser form = new Form_GridColumnChooser(grid, string.Empty);
            form.ShowDialog();
        }
                
    }
}