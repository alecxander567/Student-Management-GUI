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
    public partial class frmLogin : Form
    {

        private SiticoneHtmlLabel lblTitle;
        private SiticoneTextBox txtEmail;
        private SiticoneTextBox txtPassword;
        private SiticoneButton btnLogin;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.Text = "Login - Student Management System";
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new SiticoneHtmlLabel
            {
                Text = "Student Management System",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.SeaGreen,
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTitle.Location = new Point(
                (this.ClientSize.Width - lblTitle.Width) / 2,
                40
            );

            PictureBox pbLogo = new PictureBox
            {
                Size = new Size(150, 150), 
                SizeMode = PictureBoxSizeMode.Zoom, 
                BackColor = Color.Transparent
            };

            pbLogo.Image = Image.FromFile(" C:\\Users\\Lenovo\\Pictures\\logo.png"); 

            this.Controls.Add(pbLogo);

            pbLogo.Location = new Point(
                (this.ClientSize.Width - pbLogo.Width) / 2,
                lblTitle.Bottom + 20 
            );

            txtEmail = new SiticoneTextBox
            {
                PlaceholderText = "Email",
                Font = new Font("Segoe UI", 12, FontStyle.Regular), 
                Size = new Size(320, 45),
                BorderRadius = 8
            };
            this.Controls.Add(txtEmail);
            txtEmail.Location = new Point(
                (this.ClientSize.Width - txtEmail.Width) / 2,
                pbLogo.Bottom + 20 
            );

            txtPassword = new SiticoneTextBox
            {
                PlaceholderText = "Password",
                Font = new Font("Segoe UI", 12, FontStyle.Regular), 
                Size = new Size(320, 45),
                PasswordChar = '●',
                BorderRadius = 8
            };
            this.Controls.Add(txtPassword);
            txtPassword.Location = new Point(
                (this.ClientSize.Width - txtPassword.Width) / 2,
                txtEmail.Bottom + 20 
            );

            btnLogin = new SiticoneButton
            {
                Text = "Login",
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
                Size = new Size(320, 50),
                BorderRadius = 8,
                FillColor = Color.SeaGreen,
                ForeColor = Color.White
            };
            btnLogin.Click += BtnLogin_ClickAsync;
            this.Controls.Add(btnLogin);
            btnLogin.Location = new Point(
                (this.ClientSize.Width - btnLogin.Width) / 2,
                txtPassword.Bottom + 20 
            );

            Label lblNoAccount = new Label
            {
                Text = "Don't have an account?",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                AutoSize = true
            };
            this.Controls.Add(lblNoAccount);
            lblNoAccount.Location = new Point(
                (this.ClientSize.Width - lblNoAccount.Width) / 2,
                btnLogin.Bottom + 20
            );

            LinkLabel lnkRegister = new LinkLabel
            {
                Text = "Register",
                Font = new Font("Segoe UI", 12, FontStyle.Underline),
                LinkColor = Color.Blue,
                AutoSize = true
            };
            lnkRegister.Click += (s, args) =>
            {
                frmSignup signupForm = new frmSignup();
                signupForm.Show();
                this.Hide(); 
            };
            this.Controls.Add(lnkRegister);
            lnkRegister.Location = new Point(
                (this.ClientSize.Width - lnkRegister.Width) / 2,
                lblNoAccount.Bottom + 5
            );
        }

        private async void BtnLogin_ClickAsync(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var payload = new
            {
                email = email,
                password = password
            };
            string jsonPayload = JsonConvert.SerializeObject(payload);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://127.0.0.1:8000/api/login/"; 
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseContent);

                    if (response.IsSuccessStatusCode && result.success == true)
                    {
                        string fullName = result.user.full_name;
                        string role = result.user.role;

                        MessageBox.Show($"Welcome {fullName}!\nRole: {role}", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (role == "Instructor")
                        {
                            frmInstructorClasses instructorForm = new frmInstructorClasses();
                            instructorForm.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.message.ToString(), "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error connecting to server: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
