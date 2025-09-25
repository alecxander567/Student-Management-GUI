using Siticone.Desktop.UI.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_System
{
    public partial class frmAssignment : Form 
    {
        public string AssignmentTitle { get; private set; }
        public string Instructions { get; private set; }
        public DateTime DateOfSubmission { get; private set; }

        private SiticoneTextBox txtTitle;
        private SiticoneTextBox txtInstructions;
        private DateTimePicker dtpDueDate;
        private SiticoneButton btnSubmit;

        public frmAssignment()
        {
            this.Text = "Add New Assignment";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            SetupForm();
        }

        private void SetupForm()
        {
            Label lblTitle = new Label { Text = "Title:", Location = new Point(20, 30), AutoSize = true };
            txtTitle = new SiticoneTextBox { Location = new Point(20, 60), Width = 340, BorderRadius = 8 };

            Label lblInstructions = new Label { Text = "Instructions:", Location = new Point(20, 110), AutoSize = true };
            txtInstructions = new SiticoneTextBox
            {
                Location = new Point(20, 140),
                Width = 340,
                Height = 80,
                Multiline = true,
                BorderRadius = 8
            };

            Label lblDueDate = new Label { Text = "Due Date:", Location = new Point(20, 240), AutoSize = true };
            dtpDueDate = new DateTimePicker { Location = new Point(20, 270), Width = 200 };

            btnSubmit = new SiticoneButton
            {
                Text = "Submit",
                Location = new Point(20, 320),
                Size = new Size(340, 40),
                FillColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                BorderRadius = 8
            };
            btnSubmit.Click += BtnSubmit_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtTitle);
            this.Controls.Add(lblInstructions);
            this.Controls.Add(txtInstructions);
            this.Controls.Add(lblDueDate);
            this.Controls.Add(dtpDueDate);
            this.Controls.Add(btnSubmit);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtInstructions.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
