using Newtonsoft.Json;
using Siticone.Desktop.UI.WinForms;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Student_Management_System
{
    public partial class frmSignup : Form
    {
        private SiticoneTextBox txtFullName;
        private SiticoneTextBox txtEmail;
        private SiticoneTextBox txtPassword;
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

            int controlWidth = 320;
            int controlHeightTextBox = 45;
            int controlHeightButton = 50;
            int verticalSpacing = 20; 
            int startY = 30; 

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
                startY
            );

            startY += lblTitle.Height + verticalSpacing * 2;

            txtFullName = new SiticoneTextBox
            {
                PlaceholderText = "Full Name",
                Font = new Font("Segoe UI", 12),
                Size = new Size(controlWidth, controlHeightTextBox),
                BorderRadius = 8
            };
            this.Controls.Add(txtFullName);
            txtFullName.Location = new Point(
                (this.ClientSize.Width - controlWidth) / 2,
                startY
            );

            startY += controlHeightTextBox + verticalSpacing;

            txtEmail = new SiticoneTextBox
            {
                PlaceholderText = "Email",
                Font = new Font("Segoe UI", 12),
                Size = new Size(controlWidth, controlHeightTextBox),
                BorderRadius = 8
            };
            this.Controls.Add(txtEmail);
            txtEmail.Location = new Point(
                (this.ClientSize.Width - controlWidth) / 2,
                startY
            );

            startY += controlHeightTextBox + verticalSpacing;

            txtPassword = new SiticoneTextBox
            {
                PlaceholderText = "Password",
                Font = new Font("Segoe UI", 12),
                Size = new Size(controlWidth, controlHeightTextBox),
                PasswordChar = '●',
                BorderRadius = 8
            };
            this.Controls.Add(txtPassword);
            txtPassword.Location = new Point(
                (this.ClientSize.Width - controlWidth) / 2,
                startY
            );

            startY += controlHeightTextBox + verticalSpacing * 2;

            btnRegister = new SiticoneButton
            {
                Text = "Register",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Size = new Size(controlWidth, controlHeightButton),
                BorderRadius = 8,
                FillColor = Color.SeaGreen,
                ForeColor = Color.White
            };
            this.Controls.Add(btnRegister);
            btnRegister.Location = new Point(
                (this.ClientSize.Width - controlWidth) / 2,
                startY
            );
            btnRegister.Click += BtnRegister_ClickAsync;

            startY += controlHeightButton + verticalSpacing;

            btnCancel = new SiticoneButton
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Size = new Size(controlWidth, controlHeightTextBox),
                BorderRadius = 8,
                FillColor = Color.Gray,
                ForeColor = Color.White
            };
            this.Controls.Add(btnCancel);
            btnCancel.Location = new Point(
                (this.ClientSize.Width - controlWidth) / 2,
                startY
            );
            btnCancel.Click += BtnCancel_Click;
        }

        private async void BtnRegister_ClickAsync(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var payload = new
            {
                full_name = fullName,
                email = email,
                password = password
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
