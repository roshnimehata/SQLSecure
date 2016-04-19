using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class PolicyInterview : UserControl
    {
        #region fields

        private string m_InterviewText = string.Empty;
        private bool m_Changed = false;

        private const string SAMPLE_TITLE = @"<Enter your title here>";
        private const string SAMPLE_TEXT = @"<Enter your additional internal review notes here>";

        #endregion

        #region ctors

        public PolicyInterview()
        {
            InitializeComponent();

            _textBox_InterviewName.Text = SAMPLE_TITLE;
            _ultraFormattedTextEditor_Interview.Value = SAMPLE_TEXT;

            //_textBox_InterviewName.ReadOnly = 
            //    _ultraFormattedTextEditor_Interview.ReadOnly = true;
            _textBox_InterviewName.ForeColor =
                _ultraFormattedTextEditor_Interview.Appearance.ForeColor = Color.SlateGray;
            //_textBox_InterviewName.BackColor =
            //    _ultraFormattedTextEditor_Interview.Appearance.BackColor = Color.GhostWhite;

            // this breaks the designer
            try
            {
                _textBox_InterviewName.Enabled =
                    _ultraFormattedTextEditor_Interview.Enabled =
                    _ultraButton_EditInterview.Enabled = Program.gController.isAdmin;
            }
            catch
            {
            }
        }

        #endregion

        #region properties

        public string InterviewName
        {
            get
            {
                return (_textBox_InterviewName.Text == SAMPLE_TITLE) ? string.Empty : _textBox_InterviewName.Text;
            }
            set
            {
                string text = value ?? string.Empty;
                if (text.Length == 0)
                {
                    _textBox_InterviewName.Text = SAMPLE_TITLE;
                    _textBox_InterviewName.ForeColor = Color.SlateGray;
                }
                else
                {
                    _textBox_InterviewName.Text = text;
                    _textBox_InterviewName.ForeColor = SystemColors.WindowText;
                }
            }
        }

        #endregion

        #region methods

        public string GetInterviewText()
        {
            if (m_Changed)
            {
                return (_ultraFormattedTextEditor_Interview.Text == SAMPLE_TEXT) ? string.Empty : _ultraFormattedTextEditor_Interview.Text;
            }
            else
            {
                return (m_InterviewText == SAMPLE_TEXT) ? string.Empty : m_InterviewText;
            }
        }

        public void SetInterviewText(string text)
        {
            m_Changed = false;
            m_InterviewText = text ?? string.Empty;
            if (m_InterviewText.Length == 0)
            {
                _ultraFormattedTextEditor_Interview.Text = SAMPLE_TEXT;
                _ultraFormattedTextEditor_Interview.Appearance.ForeColor = Color.SlateGray;
            }
            else
            {
                _ultraFormattedTextEditor_Interview.Text = m_InterviewText;
                _ultraFormattedTextEditor_Interview.Appearance.ForeColor = SystemColors.WindowText;
            }
        }

        public void InitializeControl(bool allowEdit)
        {
            _textBox_InterviewName.Enabled =
                _ultraFormattedTextEditor_Interview.Enabled =
                _ultraButton_EditInterview.Enabled = allowEdit;           
        }

        #endregion

        #region events

        private void _ultraButton_EditInterview_Click(object sender, EventArgs e)
        {
            SuspendLayout();


            //_textBox_InterviewName.ReadOnly = 
            //    _ultraFormattedTextEditor_Interview.ReadOnly = false;
 
            //_textBox_InterviewName.ForeColor =
            //    _ultraFormattedTextEditor_Interview.Appearance.ForeColor = SystemColors.WindowText;
            //_textBox_InterviewName.BackColor =
            //    _ultraFormattedTextEditor_Interview.Appearance.BackColor = SystemColors.Window;

            //_ultraButton_EditInterview.Enabled = false;

            _ultraSpellChecker.ShowSpellCheckDialog(_ultraFormattedTextEditor_Interview);

            ResumeLayout();
        }

        private void _ultraFormattedTextEditor_Interview_TextChanged(object sender, EventArgs e)
        {
            m_Changed = true;
        }
       
        private void _ultraFormattedTextEditor_Interview_KeyDown(object sender, KeyEventArgs e)
        {
            // Trap for Bold, Underline, Italic and don't allow control to process them
            if ((e.KeyCode == Keys.B || e.KeyCode == Keys.U || e.KeyCode == Keys.I) && e.Control)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void _ultraFormattedTextEditor_Interview_Enter(object sender, EventArgs e)
        {
            if (_ultraFormattedTextEditor_Interview.Text == SAMPLE_TEXT)
            {
                _ultraFormattedTextEditor_Interview.Value = string.Empty;
                _ultraFormattedTextEditor_Interview.Appearance.ForeColor = SystemColors.WindowText;
            }
        }

        private void _ultraFormattedTextEditor_Interview_Leave(object sender, EventArgs e)
        {
            if (_ultraFormattedTextEditor_Interview.Text == string.Empty)
            {
                _ultraFormattedTextEditor_Interview.Value = SAMPLE_TEXT;
                _ultraFormattedTextEditor_Interview.Appearance.ForeColor = Color.SlateGray;
            }
        }

        private void _textBox_InterviewName_Enter(object sender, EventArgs e)
        {
            if (_textBox_InterviewName.Text == SAMPLE_TITLE)
            {
                _textBox_InterviewName.Text = string.Empty;
                _textBox_InterviewName.ForeColor = SystemColors.WindowText;
            }
        }

        private void _textBox_InterviewName_Leave(object sender, EventArgs e)
        {
            if (_textBox_InterviewName.Text == string.Empty)
            {
                _textBox_InterviewName.Text = SAMPLE_TITLE;
                _textBox_InterviewName.ForeColor = Color.SlateGray;
            }
        }

        #endregion
    }
}
