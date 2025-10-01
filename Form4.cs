using Newtonsoft.Json.Linq;
using Siticone.Desktop.UI.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Management_System
{
    public partial class frmClass : Form
    {
        private readonly HttpClient client = new HttpClient();
        private dynamic currentClass; // Add this field

        public frmClass(dynamic cls)
        {
            InitializeComponent();
            currentClass = cls; // Store the class
            LoadClassDetails(cls);
        }

        private async void LoadClassDetails(dynamic cls)
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
                {
                    btn.Click += (s, e) =>
                    {
                        frmInstructorClasses parentForm = new frmInstructorClasses();
                        this.Hide();
                        parentForm.Show();
                    };
                }

                sidebarPanel.Controls.Add(btn);
                topOffset += 60;
            }

            SiticonePanel bodyPanel = new SiticonePanel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.LightGray,
                Padding = new Padding(30, 30, 30, 100)
            };
            this.Controls.Add(bodyPanel);

            int buttonWidth = 180;
            int buttonHeight = 50;
            int rightPadding = 40;
            int buttonSpacing = 10;
            int boxHeight = 100;
            int startY = 120;
            int buttonsY = startY + boxHeight + 20;

            SiticoneButton btnAddAssignment = new SiticoneButton
            {
                Text = "Add New Assignment",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(bodyPanel.Width - buttonWidth - rightPadding, buttonsY),
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
                            ClassID = currentClass.ClassID,
                            Title = form.AssignmentTitle,
                            Instructions = form.Instructions,
                            DatePosted = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            DateOfSubmission = form.DateOfSubmission.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        };

                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(assignmentData);

                        try
                        {
                            using (HttpClient newClient = new HttpClient())
                            {
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                HttpResponseMessage response = await newClient.PostAsync("http://localhost:8000/api/assignments/", content);

                                string responseContent = await response.Content.ReadAsStringAsync();

                                if (response.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Assignment added successfully!");
                                    bodyPanel.Controls.Clear();
                                    await AddDashboardBoxes(bodyPanel, Convert.ToInt32(currentClass.ClassID), currentClass.ClassName);
                                    await LoadAssignments(bodyPanel, Convert.ToInt32(currentClass.ClassID));
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

            SiticoneButton btnRefreshAssignments = new SiticoneButton
            {
                Text = "Refresh Assignments",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(bodyPanel.Width - (buttonWidth * 2) - rightPadding - buttonSpacing, buttonsY),
                FillColor = Color.DodgerBlue,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRefreshAssignments.Click += async (s, e) =>
            {
                bodyPanel.Controls.Clear();
                await AddDashboardBoxes(bodyPanel, Convert.ToInt32(currentClass.ClassID), (string)currentClass.ClassName);
                await LoadAssignments(bodyPanel, Convert.ToInt32(currentClass.ClassID));
            };

            bodyPanel.AutoScroll = true;
            bodyPanel.Controls.Add(btnRefreshAssignments);
            bodyPanel.Controls.Add(btnAddAssignment);
            await AddDashboardBoxes(bodyPanel, Convert.ToInt32(cls.ClassID), (string)cls.ClassName);
            await LoadAssignments(bodyPanel, Convert.ToInt32(cls.ClassID));
        }

        private async Task AddDashboardBoxes(SiticonePanel bodyPanel, int classId, string className)
        {
            string[] boxLabels = { "No. of Students", "Total Assignments", "Total Classes", "Class Schedule" };
            int boxHeight = 100;
            int spacing = 20;
            int startX = 220;
            int startY = 120;

            int totalAvailableWidth = this.ClientSize.Width - startX - 40;
            int boxWidth = (totalAvailableWidth - (spacing * (boxLabels.Length - 1))) / boxLabels.Length;

            // Fetch data from Django backend
            var httpClient = new HttpClient();
            string apiUrl = "http://localhost:8000/api/dashboard/";
            var response = await httpClient.GetStringAsync(apiUrl);
            var jsonData = JObject.Parse(response);

            // Extract values from JSON
            int noOfStudents = 0; // You'll need a separate API endpoint for this
            int totalAssignments = (int)jsonData["total_assignments"];
            int totalClasses = (int)jsonData["total_classes"];
            var classSchedules = jsonData["class_schedules"];

            // Find schedule for the specific class
            string scheduleText = "";
            foreach (var clsSchedule in classSchedules)
            {
                if ((string)clsSchedule["ClassName"] == className)
                {
                    scheduleText = $"{clsSchedule["ScheduleDays"]} {clsSchedule["ScheduleTime"]}";
                    break;
                }
            }

            // Values for each box
            string[] values = {
        noOfStudents.ToString(),
        totalAssignments.ToString(),
        totalClasses.ToString(),
        scheduleText
    };

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

                // Use smaller font for Class Schedule
                Font valueFont = (boxLabels[i] == "Class Schedule")
                    ? new Font("Segoe UI", 10, FontStyle.Bold)
                    : new Font("Segoe UI", 24, FontStyle.Bold);

                SiticoneHtmlLabel lblValue = new SiticoneHtmlLabel
                {
                    Text = values[i],
                    Font = valueFont,
                    ForeColor = Color.MediumSeaGreen,
                    Location = new Point(10, 40),
                    AutoSize = true
                };

                dashBox.Controls.Add(lblBox);
                dashBox.Controls.Add(lblValue);
                bodyPanel.Controls.Add(dashBox);
            }
        }

        private async Task LoadAssignments(SiticonePanel bodyPanel, int classId)
        {
            try
            {
                string url = $"http://localhost:8000/api/get_assignments/?class_id={classId}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to fetch assignments: {errorContent}");
                    return;
                }

                string json = await response.Content.ReadAsStringAsync();
                var assignments = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(json);

                int buttonsHeight = 50;
                int spacingButtons = 20;
                int dashboardBoxesHeight = 100;
                int currentY = 120 + dashboardBoxesHeight + spacingButtons + buttonsHeight + spacingButtons;

                int leftPadding = 220;
                int rightPadding = 40;
                int assignmentBoxWidth = Math.Max(800, bodyPanel.Width - leftPadding - rightPadding);
                int assignmentBoxHeight = 150;
                int assignmentSpacing = 20;

                // REMOVE THIS LINE - Dashboard boxes are already added in LoadClassDetails
                // AddDashboardBoxes(bodyPanel);

                int buttonWidth = 180;
                int buttonHeight = 50;
                int buttonsY = 120 + dashboardBoxesHeight + 20;

                SiticoneButton btnAddAssignment = new SiticoneButton
                {
                    Text = "Add New Assignment",
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(bodyPanel.Width - buttonWidth - rightPadding, buttonsY),
                    FillColor = Color.MediumSeaGreen,
                    ForeColor = Color.White,
                    BorderRadius = 8,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                SiticoneButton btnRefreshAssignments = new SiticoneButton
                {
                    Text = "Refresh Assignments",
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(bodyPanel.Width - (buttonWidth * 2) - rightPadding - 10, buttonsY),
                    FillColor = Color.DodgerBlue,
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
                                ClassID = classId,
                                Title = form.AssignmentTitle,
                                Instructions = form.Instructions,
                                DatePosted = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                DateOfSubmission = form.DateOfSubmission.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                            };

                            string assignmentJson = Newtonsoft.Json.JsonConvert.SerializeObject(assignmentData);

                            try
                            {
                                using (HttpClient newClient = new HttpClient())
                                {
                                    var content = new StringContent(assignmentJson, Encoding.UTF8, "application/json");
                                    HttpResponseMessage addResponse = await newClient.PostAsync("http://localhost:8000/api/assignments/", content);

                                    if (addResponse.IsSuccessStatusCode)
                                    {
                                        MessageBox.Show("Assignment added successfully!");
                                        bodyPanel.Controls.Clear();
                                        await AddDashboardBoxes(bodyPanel, classId, currentClass.ClassName);
                                        await LoadAssignments(bodyPanel, classId);
                                    }
                                    else
                                    {
                                        string responseContent = await addResponse.Content.ReadAsStringAsync();
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

                btnRefreshAssignments.Click += async (s, e) =>
                {
                    bodyPanel.Controls.Clear();
                    await AddDashboardBoxes(bodyPanel, classId, currentClass.ClassName);
                    await LoadAssignments(bodyPanel, classId);
                };

                bodyPanel.Controls.Add(btnRefreshAssignments);
                bodyPanel.Controls.Add(btnAddAssignment);

                foreach (var assignment in assignments)
                {
                    SiticonePanel assignmentBox = new SiticonePanel
                    {
                        Size = new Size(assignmentBoxWidth, assignmentBoxHeight),
                        Location = new Point(leftPadding, currentY),
                        FillColor = Color.White,
                        BorderRadius = 10,
                        ShadowDecoration = { Enabled = true }
                    };

                    SiticoneHtmlLabel lblTitle = new SiticoneHtmlLabel
                    {
                        Text = $"📘 {assignment["Title"]}",
                        Font = new Font("Segoe UI", 16, FontStyle.Bold),
                        Location = new Point(10, 10),
                        ForeColor = Color.MediumSeaGreen,
                        AutoSize = true
                    };

                    SiticoneHtmlLabel lblInstructions = new SiticoneHtmlLabel
                    {
                        Text = $"Instructions: {assignment["Instructions"]}",
                        Font = new Font("Segoe UI", 12, FontStyle.Regular),
                        Location = new Point(10, 45),
                        MaximumSize = new Size(assignmentBox.Width - 220, 60),
                        AutoSize = true
                    };

                    SiticoneHtmlLabel lblDates = new SiticoneHtmlLabel
                    {
                        Text = $"Posted: {assignment["DatePosted"]} | Due: {assignment["DateOfSubmission"]}",
                        Font = new Font("Segoe UI", 11, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        Location = new Point(10, assignmentBoxHeight - 25),
                        AutoSize = true
                    };

                    SiticoneButton btnEdit = new SiticoneButton
                    {
                        Text = "Edit",
                        Size = new Size(90, 35),
                        Location = new Point(assignmentBox.Width - 200, assignmentBox.Height - 45),
                        FillColor = Color.MediumSeaGreen,
                        ForeColor = Color.White,
                        BorderRadius = 8,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };

                    btnEdit.Click += async (s, e) =>
                    {
                        using (frmAssignment editForm = new frmAssignment())
                        {
                            editForm.TxtTitle.Text = assignment["Title"];
                            editForm.TxtInstructions.Text = assignment["Instructions"];
                            if (DateTime.TryParse(assignment["DateOfSubmission"]?.ToString(), out DateTime dueDate))
                                editForm.DtpDueDate.Value = dueDate;
                            else
                                editForm.DtpDueDate.Value = DateTime.Now;

                            if (editForm.ShowDialog() == DialogResult.OK)
                            {
                                string updatedTitle = editForm.AssignmentTitle;
                                string updatedInstructions = editForm.Instructions;
                                string updatedDueDate = editForm.DateOfSubmission.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                                int assignmentId = assignment["AssignmentID"] != null ? (int)assignment["AssignmentID"] : -1;
                                if (assignmentId == -1)
                                {
                                    MessageBox.Show("Invalid assignment ID.");
                                    return;
                                }

                                try
                                {
                                    var payload = new
                                    {
                                        Title = updatedTitle,
                                        Instructions = updatedInstructions,
                                        DateOfSubmission = updatedDueDate
                                    };

                                    var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                                    string updateUrl = $"http://localhost:8000/api/update_assignment/{assignmentId}/";

                                    using (HttpClient httpClient = new HttpClient())
                                    {
                                        HttpResponseMessage putResponse = await httpClient.PutAsync(updateUrl, content);
                                        string responseContent = await putResponse.Content.ReadAsStringAsync();

                                        if (putResponse.IsSuccessStatusCode)
                                        {
                                            MessageBox.Show("Assignment updated successfully!");
                                            bodyPanel.Controls.Clear();
                                            await AddDashboardBoxes(bodyPanel, classId, currentClass.ClassName);
                                            await LoadAssignments(bodyPanel, classId);
                                        }
                                        else
                                        {
                                            MessageBox.Show($"Failed to update assignment. Server response: {responseContent}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error updating assignment: {ex.Message}");
                                }
                            }
                        }
                    };

                    SiticoneButton btnDelete = new SiticoneButton
                    {
                        Text = "Delete",
                        Size = new Size(90, 35),
                        Location = new Point(assignmentBox.Width - 100, assignmentBox.Height - 45),
                        FillColor = Color.IndianRed,
                        ForeColor = Color.White,
                        BorderRadius = 8,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };

                    btnDelete.Click += async (s, e) =>
                    {
                        var confirmResult = MessageBox.Show(
                            $"Are you sure you want to delete the assignment '{assignment["Title"]}'?",
                            "Confirm Delete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (confirmResult == DialogResult.Yes)
                        {
                            int assignmentId = assignment["AssignmentID"] != null ? (int)assignment["AssignmentID"] : -1;
                            if (assignmentId == -1)
                            {
                                MessageBox.Show("Invalid assignment ID.");
                                return;
                            }

                            try
                            {
                                string deleteUrl = $"http://localhost:8000/api/delete_assignment/{assignmentId}/";
                                HttpResponseMessage deleteResponse = await client.DeleteAsync(deleteUrl);
                                string responseContent = await deleteResponse.Content.ReadAsStringAsync();

                                if (deleteResponse.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Assignment deleted successfully!");
                                    bodyPanel.Controls.Clear();
                                    await AddDashboardBoxes(bodyPanel, classId, currentClass.ClassName);
                                    await LoadAssignments(bodyPanel, classId);
                                }
                                else
                                {
                                    MessageBox.Show($"Failed to delete assignment. Server response: {responseContent}");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error deleting assignment: {ex.Message}");
                            }
                        }
                    };

                    assignmentBox.Controls.Add(lblTitle);
                    assignmentBox.Controls.Add(lblInstructions);
                    assignmentBox.Controls.Add(lblDates);
                    assignmentBox.Controls.Add(btnEdit);
                    assignmentBox.Controls.Add(btnDelete);

                    bodyPanel.Controls.Add(assignmentBox);

                    currentY += assignmentBoxHeight + assignmentSpacing;
                }

                bodyPanel.AutoScroll = true;
                bodyPanel.AutoScrollMinSize = new Size(0, currentY + 50);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching assignments: {ex.Message}");
            }
        }
    }
}