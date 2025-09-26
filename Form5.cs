using Siticone.Desktop.UI.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

using Siticone.Desktop.UI.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_System
{
    public partial class frmAssignment : Form
    {

        private SiticoneTextBox txtTitle;
        private SiticoneTextBox txtInstructions;
        private DateTimePicker dtpDueDate;
        private SiticoneButton btnSubmit;

        public string AssignmentTitle { get; private set; }
        public string Instructions { get; private set; }
        public DateTime DateOfSubmission { get; private set; }

        public SiticoneTextBox TxtTitle => txtTitle;
        public SiticoneTextBox TxtInstructions => txtInstructions;
        public DateTimePicker DtpDueDate => dtpDueDate;

        public frmAssignment()
        {
            this.Text = "Add New Assignment";
            this.Size = new Size(550, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            SetupForm();
        }

        private void SetupForm()
        {
            int margin = 25;
            int spacing = 30;
            int labelSpacing = 6;

            Font labelFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font textFont = new Font("Segoe UI", 10, FontStyle.Regular);
            Font buttonFont = new Font("Segoe UI", 11, FontStyle.Bold);

            int controlWidth = this.ClientSize.Width - (margin * 2);
            int yPos = margin;

            Label lblTitle = new Label
            {
                Text = "Title:",
                Location = new Point(margin, yPos),
                Size = new Size(controlWidth, 20),
                Font = labelFont,
                TextAlign = ContentAlignment.BottomLeft
            };
            yPos += lblTitle.Height + labelSpacing;

            txtTitle = new SiticoneTextBox
            {
                Location = new Point(margin, yPos),
                Size = new Size(controlWidth, 35),
                BorderRadius = 6,
                Font = textFont
            };
            yPos += txtTitle.Height + spacing;

            Label lblInstructions = new Label
            {
                Text = "Instructions:",
                Location = new Point(margin, yPos),
                Size = new Size(controlWidth, 20),
                Font = labelFont,
                TextAlign = ContentAlignment.BottomLeft
            };
            yPos += lblInstructions.Height + labelSpacing;

            txtInstructions = new SiticoneTextBox
            {
                Location = new Point(margin, yPos),
                Size = new Size(controlWidth, 100),
                Multiline = true,
                BorderRadius = 6,
                Font = textFont,
                ScrollBars = ScrollBars.Vertical
            };
            yPos += txtInstructions.Height + spacing;

            Label lblDueDate = new Label
            {
                Text = "Due Date:",
                Location = new Point(margin, yPos),
                Size = new Size(controlWidth, 20),
                Font = labelFont,
                TextAlign = ContentAlignment.BottomLeft
            };
            yPos += lblDueDate.Height + labelSpacing;

            dtpDueDate = new DateTimePicker
            {
                Location = new Point(margin, yPos),
                Size = new Size(250, 25),
                Font = textFont,
                Format = DateTimePickerFormat.Short
            };
            yPos += dtpDueDate.Height + spacing + 10;

            int buttonWidth = 180;
            int buttonX = (this.ClientSize.Width - buttonWidth) / 2;

            btnSubmit = new SiticoneButton
            {
                Text = "Submit Assignment",
                Location = new Point(buttonX, yPos),
                Size = new Size(buttonWidth, 40),
                FillColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = buttonFont
            };
            btnSubmit.Click += BtnSubmit_Click;

            this.Controls.AddRange(new Control[] {
                lblTitle,
                txtTitle,
                lblInstructions,
                txtInstructions,
                lblDueDate,
                dtpDueDate,
                btnSubmit
            });
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title for the assignment.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtInstructions.Text))
            {
                MessageBox.Show("Please enter instructions for the assignment.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtInstructions.Focus();
                return;
            }

            if (dtpDueDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Due date cannot be in the past.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDueDate.Focus();
                return;
            }

            AssignmentTitle = txtTitle.Text.Trim();
            Instructions = txtInstructions.Text.Trim();
            DateOfSubmission = dtpDueDate.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
