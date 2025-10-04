using Siticone.Desktop.UI.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_System
{
    public partial class frmStudent : Form
    {
        public string FirstName
        {
            get => txtFirstName.Text.Trim();
            set => txtFirstName.Text = value;
        }

        public string LastName
        {
            get => txtLastName.Text.Trim();
            set => txtLastName.Text = value;
        }

        public string Sex
        {
            get => cmbSex.SelectedItem?.ToString();
            set
            {
                if (cmbSex.Items.Contains(value))
                    cmbSex.SelectedItem = value;
            }
        }

        public string Department
        {
            get => txtDepartment.Text.Trim();
            set => txtDepartment.Text = value;
        }

        public int YearLevel
        {
            get => (int)nudYearLevel.Value;
            set
            {
                if (value >= nudYearLevel.Minimum && value <= nudYearLevel.Maximum)
                    nudYearLevel.Value = value;
            }
        }

        private SiticoneTextBox txtFirstName;
        private SiticoneTextBox txtLastName;
        private SiticoneComboBox cmbSex;
        private SiticoneTextBox txtDepartment;
        private SiticoneNumericUpDown nudYearLevel;
        private SiticoneButton btnSubmit;
        private SiticoneButton btnCancel;

        public frmStudent()
        {
            InitializeComponent();
            this.Text = "Add New Student";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(550, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int top = 25;
            int labelWidth = 120;
            int controlWidth = 320;
            int spacing = 15;
            int leftMargin = 25;

            Font labelFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 10, FontStyle.Regular);
            int inputHeight = 34;

            void AddLabelAndControl(SiticoneHtmlLabel label, Control control)
            {
                label.Top = control.Top + (control.Height - label.Height) / 2;
                this.Controls.Add(label);
                this.Controls.Add(control);
            }

            txtFirstName = new SiticoneTextBox
            {
                Location = new Point(labelWidth + spacing + leftMargin, top),
                Size = new Size(controlWidth, inputHeight),
                Font = inputFont,
                BorderRadius = 5
            };
            var lblFirstName = new SiticoneHtmlLabel
            {
                Text = "First Name:",
                AutoSize = true,
                Font = labelFont,
                Left = leftMargin
            };
            AddLabelAndControl(lblFirstName, txtFirstName);

            top += inputHeight + 25;

            txtLastName = new SiticoneTextBox
            {
                Location = new Point(labelWidth + spacing + leftMargin, top),
                Size = new Size(controlWidth, inputHeight),
                Font = inputFont,
                BorderRadius = 5
            };
            var lblLastName = new SiticoneHtmlLabel
            {
                Text = "Last Name:",
                AutoSize = true,
                Font = labelFont,
                Left = leftMargin
            };
            AddLabelAndControl(lblLastName, txtLastName);

            top += inputHeight + 25;

            cmbSex = new SiticoneComboBox
            {
                Location = new Point(labelWidth + spacing + leftMargin, top),
                Size = new Size(controlWidth, inputHeight),
                Font = inputFont,
                BorderRadius = 5
            };
            cmbSex.Items.AddRange(new string[] { "M", "F", "O" });
            cmbSex.SelectedIndex = 0;
            var lblSex = new SiticoneHtmlLabel
            {
                Text = "Sex:",
                AutoSize = true,
                Font = labelFont,
                Left = leftMargin
            };
            AddLabelAndControl(lblSex, cmbSex);

            top += inputHeight + 25;

            txtDepartment = new SiticoneTextBox
            {
                Location = new Point(labelWidth + spacing + leftMargin, top),
                Size = new Size(controlWidth, inputHeight),
                Font = inputFont,
                BorderRadius = 5
            };
            var lblDepartment = new SiticoneHtmlLabel
            {
                Text = "Department:",
                AutoSize = true,
                Font = labelFont,
                Left = leftMargin
            };
            AddLabelAndControl(lblDepartment, txtDepartment);

            top += inputHeight + 25;

            nudYearLevel = new SiticoneNumericUpDown
            {
                Location = new Point(labelWidth + spacing + leftMargin, top),
                Size = new Size(controlWidth, inputHeight),
                Minimum = 1,
                Maximum = 5,
                Value = 1,
                Font = inputFont,
                BorderRadius = 5
            };
            var lblYearLevel = new SiticoneHtmlLabel
            {
                Text = "Year Level:",
                AutoSize = true,
                Font = labelFont,
                Left = leftMargin
            };
            AddLabelAndControl(lblYearLevel, nudYearLevel);

            top += inputHeight + 90; 

            int buttonWidth = 140;
            int buttonSpacing = 30;
            int totalButtonsWidth = (buttonWidth * 2) + buttonSpacing;
            int buttonStartX = (this.ClientSize.Width - totalButtonsWidth) / 2;

            btnSubmit = new SiticoneButton
            {
                Text = "Submit",
                Location = new Point(buttonStartX, top),
                Size = new Size(buttonWidth, 42),
                FillColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnCancel = new SiticoneButton
            {
                Text = "Cancel",
                Location = new Point(buttonStartX + buttonWidth + buttonSpacing, top),
                Size = new Size(buttonWidth, 42),
                FillColor = Color.IndianRed,
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            btnSubmit.Click += BtnSubmit_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(btnSubmit);
            this.Controls.Add(btnCancel);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Department))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}