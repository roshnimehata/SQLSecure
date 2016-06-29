using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_AddMetricValue : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        public Form_AddMetricValue()
        {
            InitializeComponent();
        }

        public Form_AddMetricValue(string value)
        {
            InitializeComponent();

            textBox_Values.Text = value.Replace("''", "'");
            textBox_Values.Multiline = false;
        }

        public Form_AddMetricValue(string[] values)
        {
            InitializeComponent();

            textBox_Values.Text = string.Join("\r\n", values).Replace("''", "'");
            textBox_Values.SelectionStart = textBox_Values.Text.Length;
        }

        public static string[] Process(string description)
        {
            Form_AddMetricValue dlg = new Form_AddMetricValue();
            dlg.Description = description;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                // Escape any single quotes ' for SQL
                string text = dlg.textBox_Values.Text.Replace("'", "''");
                return text.Split('\n');
            }
            return new string[]{};
        }

        public static string Process(string description, string value)
        {
            Form_AddMetricValue dlg = new Form_AddMetricValue(value);
            dlg.Description = description;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Escape any single quotes ' for SQL
                string text = dlg.textBox_Values.Text.Replace("'", "''");
                return text;
            }
            return string.Empty;
        }

        public static string[] Process(string description, string[] values)
        {
            Form_AddMetricValue dlg = new Form_AddMetricValue(values);
            dlg.Description = description;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Escape any single quotes ' for SQL
                string text = dlg.textBox_Values.Text.Replace("'", "''");
                return text.Split('\n');
            }

            // If not ok, then return the original list back
            return values;
        }

        private void Form_AddMetricValue_Load(object sender, EventArgs e)
        {

        }
    }
}

