using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Siticone.Desktop.UI.WinForms;
using Newtonsoft.Json;

namespace Student_Management_System
{
    public partial class frmInstructorClasses : Form
    {
        private FlowLayoutPanel flowPanel;
        private SiticoneButton btnAddClass;

        public frmInstructorClasses()
        {
            InitializeComponent();

            this.Load += frmInstructorClasses_Load;
        }

        private async void frmInstructorClasses_Load(object sender, EventArgs e)
        {
            this.Text = "Instructor Classes - Student Management System";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 700);

            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Transparent
            };
            this.Controls.Add(topPanel);

            btnAddClass = new SiticoneButton
            {
                Text = "Add New Class",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FillColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Size = new Size(160, 40),
                BorderRadius = 8,
                Location = new Point(this.ClientSize.Width - 360, 15) 
            };
            btnAddClass.Click += BtnAddClass_Click;
            topPanel.Controls.Add(btnAddClass);

            var btnLogout = new SiticoneButton
            {
                Text = "Logout",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FillColor = Color.IndianRed,
                ForeColor = Color.White,
                Size = new Size(160, 40),
                BorderRadius = 8,
                Location = new Point(this.ClientSize.Width - 180, 15)
            };
            btnLogout.Click += BtnLogout_Click;
            topPanel.Controls.Add(btnLogout);

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10, 10, 10, 10) 
            };
            this.Controls.Add(flowPanel);
            flowPanel.BringToFront();

            this.Resize += (s, ev) =>
            {
                btnAddClass.Left = this.ClientSize.Width - 360;
                btnLogout.Left = this.ClientSize.Width - 180;
            };

            await LoadClassesAsync();
        }

        private async Task LoadClassesAsync()
        {
            flowPanel.Controls.Clear();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://127.0.0.1:8000/api/classes/"; 
                    HttpResponseMessage response = await client.GetAsync(url);
                    string content = await response.Content.ReadAsStringAsync();
                    var classes = JsonConvert.DeserializeObject<List<dynamic>>(content);

                    foreach (var cls in classes)
                    {
                        AddClassCard(cls);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load classes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddClassCard(dynamic cls)
        {
            var card = new SiticonePanel
            {
                Size = new Size(340, 250), 
                FillColor = Color.LightGray,
                Margin = new Padding(15)
            };

            var header = new SiticonePanel
            {
                Size = new Size(card.Width, 50), 
                FillColor = Color.MediumSeaGreen,
                Location = new Point(0, 0),
            };
            card.Controls.Add(header);

            var lblName = new SiticoneHtmlLabel
            {
                Text = cls.ClassName.ToString(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit,
                TextAlignment = ContentAlignment.MiddleCenter, 
                AutoSize = false
            };
            header.Controls.Add(lblName);

            var lblCode = new SiticoneHtmlLabel
            {
                Text = $"{cls.ClassCode}",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(10, header.Bottom + 10),
                AutoSize = true
            };
            card.Controls.Add(lblCode);

            var lblDesc = new SiticoneHtmlLabel
            {
                Text = $"{cls.Description}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(10, lblCode.Bottom + 5),
                AutoSize = false,
                MaximumSize = new Size(280, 0), 
            };

            using (Graphics g = lblDesc.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(lblDesc.Text, lblDesc.Font, lblDesc.MaximumSize.Width);
                lblDesc.Size = new Size((int)textSize.Width, (int)textSize.Height + 5); 
            }

            card.Controls.Add(lblDesc);

            var lblYear = new SiticoneHtmlLabel
            {
                Text = $"{cls.YearLevel}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(10, lblDesc.Bottom + 5),
                AutoSize = true
            };
            card.Controls.Add(lblYear);

            var lblSchedule = new SiticoneHtmlLabel
            {
                Text = $"{cls.ScheduleDays} {cls.ScheduleTime}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(10, lblYear.Bottom + 5),
                AutoSize = true
            };
            card.Controls.Add(lblSchedule);

            var btnPanel = new Panel
            {
                Size = new Size(card.Width, 60), 
                Location = new Point(0, card.Height - 70), 
                BackColor = Color.Transparent
            };
            card.Controls.Add(btnPanel);

            var btnOpen = new SiticoneButton
            {
                Text = "Open",
                Size = new Size(100, 40),
                Location = new Point(10, 10), 
                FillColor = Color.SeaGreen,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnOpen.Click += (s, e) =>
            {
                MessageBox.Show($"Open class: {cls.ClassName}");
            };
            btnPanel.Controls.Add(btnOpen);

            var btnEdit = new SiticoneButton
            {
                Text = "Edit",
                Size = new Size(100, 40),
                Location = new Point(120, 10),
                FillColor = Color.Orange,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnEdit.Click += async (s, e) =>
            {
                using (Form editClassForm = new Form())
                {
                    editClassForm.Text = $"Edit Class: {cls.ClassName}";
                    editClassForm.Size = new Size(640, 620);
                    editClassForm.StartPosition = FormStartPosition.CenterParent;

                    Font inputFont = new Font("Segoe UI", 12, FontStyle.Regular);
                    int startY = 20;
                    int spacing = 55;

                    var txtName = new SiticoneTextBox
                    {
                        PlaceholderText = "Class Name",
                        Text = cls.ClassName,
                        Location = new Point(20, startY),
                        Size = new Size(440, 40),
                        Font = inputFont
                    };

                    var txtCode = new SiticoneTextBox
                    {
                        PlaceholderText = "Class Code",
                        Text = cls.ClassCode,
                        Location = new Point(20, startY + spacing),
                        Size = new Size(440, 40),
                        Font = inputFont
                    };

                    var txtDesc = new SiticoneTextBox
                    {
                        PlaceholderText = "Description",
                        Text = cls.Description,
                        Location = new Point(20, startY + spacing * 2),
                        Size = new Size(440, 70),
                        Multiline = true,
                        Font = inputFont
                    };

                    var txtYear = new SiticoneTextBox
                    {
                        PlaceholderText = "Year Level",
                        Text = cls.YearLevel,
                        Location = new Point(20, startY + spacing * 2 + 80),
                        Size = new Size(440, 40),
                        Font = inputFont
                    };

                    var txtDays = new SiticoneTextBox
                    {
                        PlaceholderText = "Schedule Days",
                        Text = cls.ScheduleDays,
                        Location = new Point(20, startY + spacing * 3 + 80),
                        Size = new Size(440, 40),
                        Font = inputFont
                    };

                    var timePicker = new DateTimePicker
                    {
                        Format = DateTimePickerFormat.Time,
                        ShowUpDown = true,
                        Location = new Point(20, txtDays.Bottom + 10),
                        Size = new Size(440, 40),
                        Font = inputFont
                    };

                    if (DateTime.TryParse(cls.ScheduleTime.ToString(), out DateTime parsedTime))
                    {
                        timePicker.Value = parsedTime;
                    }

                    var btnSave = new SiticoneButton
                    {
                        Text = "Save Changes",
                        Size = new Size(580, 50),
                        FillColor = Color.MediumSeaGreen,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        BorderRadius = 8
                    };

                    int bottomPadding = 20;
                    btnSave.Location = new Point(20, editClassForm.ClientSize.Height - btnSave.Height - bottomPadding);

                    editClassForm.Controls.Add(txtName);
                    editClassForm.Controls.Add(txtCode);
                    editClassForm.Controls.Add(txtDesc);
                    editClassForm.Controls.Add(txtYear);
                    editClassForm.Controls.Add(txtDays);
                    editClassForm.Controls.Add(timePicker);
                    editClassForm.Controls.Add(btnSave);

                    btnSave.Click += async (senderSave, ea) =>
                    {
                        var classData = new
                        {
                            ClassName = txtName.Text.Trim(),
                            ClassCode = txtCode.Text.Trim(),
                            Description = txtDesc.Text.Trim(),
                            YearLevel = txtYear.Text.Trim(),
                            ScheduleDays = txtDays.Text.Trim(),
                            ScheduleTime = timePicker.Value.ToString("HH:mm:ss")
                        };

                        string jsonPayload = JsonConvert.SerializeObject(classData);

                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                string url = $"http://127.0.0.1:8000/api/classes/edit/{cls.ClassID}/";
                                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                                HttpResponseMessage response = await client.PostAsync(url, content);
                                string responseContent = await response.Content.ReadAsStringAsync();
                                dynamic result = JsonConvert.DeserializeObject(responseContent);

                                if (response.IsSuccessStatusCode && result.success == true)
                                {
                                    MessageBox.Show(result.message.ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    editClassForm.Close();

                                    await LoadClassesAsync();
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
                    };

                    editClassForm.ShowDialog();
                }
            };

            btnPanel.Controls.Add(btnEdit);

            var btnDelete = new SiticoneButton
            {
                Text = "Delete",
                Size = new Size(100, 40),
                Location = new Point(230, 10),
                FillColor = Color.Red,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnDelete.Click += async (s, e) =>
            {
                var confirmResult = MessageBox.Show(
                    $"Are you sure you want to delete the class: {cls.ClassName}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirmResult == DialogResult.Yes)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            string url = $"http://127.0.0.1:8000/api/classes/{cls.ClassID}/delete/";
                            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);

                            HttpResponseMessage response = await client.SendAsync(request);
                            string responseContent = await response.Content.ReadAsStringAsync();
                            dynamic result = JsonConvert.DeserializeObject(responseContent);

                            if (response.IsSuccessStatusCode && result.success == true)
                            {
                                MessageBox.Show(result.message.ToString(), "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                await LoadClassesAsync();
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
            };

            btnPanel.Controls.Add(btnDelete);

            flowPanel.Controls.Add(card);
        }

        private async void BtnAddClass_Click(object sender, EventArgs e)
        {
            using (Form addClassForm = new Form())
            {
                addClassForm.Text = "Add New Class";
                addClassForm.Size = new Size(640, 620);
                addClassForm.StartPosition = FormStartPosition.CenterParent;

                Font inputFont = new Font("Segoe UI", 12, FontStyle.Regular);

                int startY = 20;
                int spacing = 55;

                var txtName = new SiticoneTextBox
                {
                    PlaceholderText = "Class Name",
                    Location = new Point(20, startY),
                    Size = new Size(440, 40),
                    Font = inputFont
                };

                var txtCode = new SiticoneTextBox
                {
                    PlaceholderText = "Class Code",
                    Location = new Point(20, startY + spacing),
                    Size = new Size(440, 40),
                    Font = inputFont
                };

                var txtDesc = new SiticoneTextBox
                {
                    PlaceholderText = "Description",
                    Location = new Point(20, startY + spacing * 2),
                    Size = new Size(440, 70),
                    Multiline = true,
                    Font = inputFont
                };

                var txtYear = new SiticoneTextBox
                {
                    PlaceholderText = "Year Level",
                    Location = new Point(20, startY + spacing * 2 + 80),
                    Size = new Size(440, 40),
                    Font = inputFont
                };

                var txtDays = new SiticoneTextBox
                {
                    PlaceholderText = "Schedule Days",
                    Location = new Point(20, startY + spacing * 3 + 80),
                    Size = new Size(440, 40),
                    Font = inputFont
                };

                var timePicker = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Time,       
                    ShowUpDown = true,                       
                    Location = new Point(20, txtDays.Bottom + 15), 
                    Size = new Size(440, 40),
                    Font = inputFont
                };

                var btnSubmit = new SiticoneButton
                {
                    Text = "Add Class",
                    Size = new Size(580, 50),
                    FillColor = Color.MediumSeaGreen,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    BorderRadius = 8
                };

                int bottomPadding = 20; 
                btnSubmit.Location = new Point(20, addClassForm.ClientSize.Height - btnSubmit.Height - bottomPadding);

                addClassForm.Controls.Add(txtName);
                addClassForm.Controls.Add(txtCode);
                addClassForm.Controls.Add(txtDesc);
                addClassForm.Controls.Add(txtYear);
                addClassForm.Controls.Add(txtDays);
                addClassForm.Controls.Add(timePicker);
                addClassForm.Controls.Add(btnSubmit);

                btnSubmit.Click += async (s, ea) =>
                {
                    var classData = new
                    {
                        ClassName = txtName.Text.Trim(),
                        ClassCode = txtCode.Text.Trim(),
                        Description = txtDesc.Text.Trim(),
                        YearLevel = txtYear.Text.Trim(),
                        ScheduleDays = txtDays.Text.Trim(),
                        ScheduleTime = timePicker.Value.ToString("HH:mm:ss"),
                        InstructorID = 1 
                    };

                    string jsonPayload = JsonConvert.SerializeObject(classData);

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            string url = "http://127.0.0.1:8000/api/add_class/";
                            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(url, content);
                            string responseContent = await response.Content.ReadAsStringAsync();
                            dynamic result = JsonConvert.DeserializeObject(responseContent);

                            if (response.IsSuccessStatusCode && result.success == true)
                            {
                                MessageBox.Show(result.message.ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                addClassForm.Close();
                                await LoadClassesAsync();
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
                };

                addClassForm.ShowDialog();
            }
        }

        private async void BtnLogout_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://127.0.0.1:8000/api/logout/"; 
                    var response = await client.PostAsync(url, null); 
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(content);

                    if (response.IsSuccessStatusCode && result.success == true)
                    {
                        MessageBox.Show(result.message.ToString(), "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();
                        frmLogin loginForm = new frmLogin();
                        loginForm.Show();
                    }
                    else
                    {
                        MessageBox.Show(result.message.ToString(), "Logout Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
