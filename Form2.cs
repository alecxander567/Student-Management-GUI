using Newtonsoft.Json;
using Siticone.Desktop.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Management_System
{
    public partial class frmSignup : Form
    {

        private SiticoneTextBox txtFullName;
        private SiticoneTextBox txtEmail;
        private SiticoneTextBox txtPassword;
        private SiticoneComboBox cmbRole;
        private SiticoneButton btnRegister;
        private SiticoneButton btnCancel;
        private SiticoneHtmlLabel lblTitle;
        public frmSignup()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "Sign Up - Student Management System";
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new SiticoneHtmlLabel
            {
                Text = "Create an Account",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.SeaGreen,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);
            lblTitle.Location = new Point(
                (this.ClientSize.Width - lblTitle.Width) / 2,
                30
            );

            txtFullName = new SiticoneTextBox
            {
                PlaceholderText = "Full Name",
                Font = new Font("Segoe UI", 12),
                Size = new Size(320, 45),
                BorderRadius = 8
            };
            this.Controls.Add(txtFullName);
            txtFullName.Location = new Point(
                (this.ClientSize.Width - txtFullName.Width) / 2,
                120
            );

            txtEmail = new SiticoneTextBox
            {
                PlaceholderText = "Email",
                Font = new Font("Segoe UI", 12),
                Size = new Size(320, 45),
                BorderRadius = 8
            };
            this.Controls.Add(txtEmail);
            txtEmail.Location = new Point(
                (this.ClientSize.Width - txtEmail.Width) / 2,
                180
            );

            txtPassword = new SiticoneTextBox
            {
                PlaceholderText = "Password",
                Font = new Font("Segoe UI", 12),
                Size = new Size(320, 45),
                PasswordChar = '●',
                BorderRadius = 8
            };
            this.Controls.Add(txtPassword);
            txtPassword.Location = new Point(
                (this.ClientSize.Width - txtPassword.Width) / 2,
                240
            );

            cmbRole = new SiticoneComboBox
            {
                Font = new Font("Segoe UI", 12),
                Size = new Size(320, 45),
                BorderRadius = 8,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new object[] { "Student", "Teacher" });
            this.Controls.Add(cmbRole);
            cmbRole.Location = new Point(
                (this.ClientSize.Width - cmbRole.Width) / 2,
                300
            );

            btnRegister = new SiticoneButton
            {
                Text = "Register",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Size = new Size(320, 50),
                BorderRadius = 8,
                FillColor = Color.SeaGreen,
                ForeColor = Color.White
            };
            this.Controls.Add(btnRegister);
            btnRegister.Location = new Point(
                (this.ClientSize.Width - btnRegister.Width) / 2,
                370
            );
            btnRegister.Click += BtnRegister_ClickAsync;

            btnCancel = new SiticoneButton
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Size = new Size(320, 45),
                BorderRadius = 8,
                FillColor = Color.Gray,
                ForeColor = Color.White
            };
            this.Controls.Add(btnCancel);
            btnCancel.Location = new Point(
                (this.ClientSize.Width - btnCancel.Width) / 2,
                430
            );
            btnCancel.Click += BtnCancel_Click;
        }

        private async void BtnRegister_ClickAsync(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var payload = new
            {
                full_name = fullName,
                email = email,
                password = password,
                role = role
            };
            string jsonPayload = JsonConvert.SerializeObject(payload);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://127.0.0.1:8000/api/signup/";
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseContent);

                    if (response.IsSuccessStatusCode && result.success == true)
                    {
                        MessageBox.Show(result.message.ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        frmLogin loginForm = new frmLogin();
                        loginForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show(result.message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error connecting to server: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
            this.Hide();
        }
    }
}
