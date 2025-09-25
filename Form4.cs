using Newtonsoft.Json;
using Siticone.Desktop.UI.WinForms;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Student_Management_System
{
    public partial class frmClass : Form
    {
        public frmClass(dynamic cls)
        {
            InitializeComponent();
            LoadClassDetails(cls);
        }

        private void LoadClassDetails(dynamic cls)
        {
            this.Text = $"Classroom - {cls.ClassName}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 800);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 700);

            SiticonePanel headerPanel = new SiticonePanel
            {
                Dock = DockStyle.Top,
                Height = 100,
                FillColor = Color.MediumSeaGreen
            };
            this.Controls.Add(headerPanel);

            SiticoneHtmlLabel lblClassName = new SiticoneHtmlLabel
            {
                Text = cls.ClassName,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 30),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblClassName);

            SiticonePanel sidebarPanel = new SiticonePanel
            {
                Dock = DockStyle.Left,
                Width = 200,
                FillColor = Color.FromArgb(34, 40, 49),
                Padding = new Padding(10)
            };
            this.Controls.Add(sidebarPanel);

            string[] menuNames = { "Dashboard", "Student List", "Assignments & Activities", "Settings", "Back to Classes" };
            int topOffset = 20;
            foreach (var menu in menuNames)
            {
                SiticoneButton btn = new SiticoneButton
                {
                    Text = menu,
                    Size = new Size(180, 50),
                    Location = new Point(10, topOffset),
                    FillColor = Color.FromArgb(57, 62, 70),
                    ForeColor = Color.White,
                    BorderRadius = 8,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                if (menu == "Back to Classes")
                    btn.Click += (s, e) => this.Close();

                sidebarPanel.Controls.Add(btn);
                topOffset += 60;
            }

            SiticonePanel bodyPanel = new SiticonePanel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.LightGray,
                Padding = new Padding(20)
            };
            this.Controls.Add(bodyPanel);

            string[] boxLabels = { "No. of Students", "Total Activities Today", "Assignments", "Total Submissions" };
            int boxHeight = 100;
            int spacing = 20;
            int startX = sidebarPanel.Width + 20;
            int startY = 120;

            int totalAvailableWidth = this.ClientSize.Width - startX - 40;
            int boxWidth = (totalAvailableWidth - (spacing * (boxLabels.Length - 1))) / boxLabels.Length;

            for (int i = 0; i < boxLabels.Length; i++)
            {
                int boxX = startX + i * (boxWidth + spacing);

                SiticonePanel dashBox = new SiticonePanel
                {
                    Size = new Size(boxWidth, boxHeight),
                    Location = new Point(boxX, startY),
                    FillColor = Color.White,
                    BorderRadius = 10,
                    ShadowDecoration = { Enabled = true }
                };

                SiticoneHtmlLabel lblBox = new SiticoneHtmlLabel
                {
                    Text = boxLabels[i],
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(10, 10),
                    AutoSize = true
                };

                SiticoneHtmlLabel lblValue = new SiticoneHtmlLabel
                {
                    Text = "0",
                    Font = new Font("Segoe UI", 24, FontStyle.Bold),
                    ForeColor = Color.MediumSeaGreen,
                    Location = new Point(10, 40),
                    AutoSize = true
                };

                dashBox.Controls.Add(lblBox);
                dashBox.Controls.Add(lblValue);
                bodyPanel.Controls.Add(dashBox);
            }

            int buttonWidth = 180;
            int buttonHeight = 50;
            int spacingButtons = 20; 

            int buttonsY = startY + boxHeight + 20;
            int buttonsStartX = bodyPanel.Width - buttonWidth * 2 - spacingButtons - 20; 

            SiticoneButton btnAddAssignment = new SiticoneButton
            {
                Text = "Add New Assignment",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(buttonsStartX, buttonsY),
                FillColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnAddAssignment.Click += async (s, e) =>
            {
                using (var form = new frmAssignment())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var assignmentData = new
                        {
                            ClassID = cls.ClassID,
                            Title = form.AssignmentTitle,
                            Instructions = form.Instructions,
                            DatePosted = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), 
                            DateOfSubmission = form.DateOfSubmission.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") 
                        };

                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(assignmentData);

                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                HttpResponseMessage response = await client.PostAsync("http://localhost:8000/api/assignments/", content);

                                string responseContent = await response.Content.ReadAsStringAsync();

                                if (response.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Assignment added successfully!");
                                }
                                else
                                {
                                    MessageBox.Show($"Error adding assignment: {responseContent}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Exception: {ex.Message}");
                        }
                    }
                }
            };

            bodyPanel.Controls.Add(btnAddAssignment);

            SiticoneButton btnAddActivity = new SiticoneButton
            {
                Text = "Add New Activity",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(buttonsStartX + buttonWidth + spacingButtons, buttonsY),
                FillColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnAddActivity.Click += async (s, e) =>
            {
                var activityData = new
                {
                    ClassID = cls.ClassID,
                    Title = "Sample Activity",
                    Instructions = "Complete this activity",
                    DatePosted = DateTime.Now,
                    DateOfSubmission = DateTime.Now.AddDays(3)
                };

                string json = System.Text.Json.JsonSerializer.Serialize(activityData);

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:8000/api/activities/", content);

                    if (response.IsSuccessStatusCode)
                        MessageBox.Show("Activity added successfully!");
                    else
                        MessageBox.Show("Error adding activity.");
                }
            };

            bodyPanel.Controls.Add(btnAddActivity);
        }
    }
}