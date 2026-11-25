Imports System.IO
Public Class Form1
    ' ----- Controls (normally created by the Designer; WithEvents lets us use Handles) -----
    Private WithEvents txtName As New TextBox()
    Private WithEvents cboCourse As New ComboBox()
    Private WithEvents nudAge As New NumericUpDown()
    Private WithEvents radMale As New RadioButton()
    Private WithEvents radFemale As New RadioButton()
    Private WithEvents chkNews As New CheckBox()
    Private WithEvents btnAdd As New Button()
    Private WithEvents btnRemove As New Button()
    Private WithEvents btnSave As New Button()
    Private WithEvents lstStudents As New ListBox()
    Private WithEvents tmrClock As New Timer()
    Private WithEvents mnuExit As New ToolStripMenuItem("E&xit")
    Private lblClock As New ToolStripStatusLabel()
    Private ep As New ErrorProvider()
    Private tip As New ToolTip()
    ' ----- FORM LOAD: set up the window, build the UI, start the timer -----
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Student Registration"
        Me.Size = New Size(480, 420)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.KeyPreview = True                        ' so the form receives Ctrl+S
        BuildUi()
        tmrClock.Interval = 1000
        tmrClock.Start()
    End Sub
    ' helper: put a caption label + a control on one row
    Private Sub AddRow(caption As String, ctl As Control, top As Integer)
        Dim lbl As New Label() With {.Text = caption, .Left = 10, .Top = top + 3, .AutoSize = True}
        ctl.Left = 110 : ctl.Top = top
        Me.Controls.Add(lbl)
        Me.Controls.Add(ctl)
    End Sub
    Private Sub BuildUi()
        ' menu bar
        Dim ms As New MenuStrip()
        Dim mnuFile As New ToolStripMenuItem("&File")
        mnuFile.DropDownItems.Add(mnuExit)
        ms.Items.Add(mnuFile)
        Me.MainMenuStrip = ms
        Me.Controls.Add(ms)
        ' status bar (shows the clock)
        Dim ss As New StatusStrip()
         ss.Items.Add(lblClock)
        Me.Controls.Add(ss)
        ' input rows
        txtName.Width = 200
        AddRow("Name:", txtName, 40)
        cboCourse.Width = 200
        cboCourse.DropDownStyle = ComboBoxStyle.DropDownList
        cboCourse.Items.AddRange(New String() {"Computer Science", "Engineering", "Business"})
        AddRow("Course:", cboCourse, 70)
        nudAge.Minimum = 16 : nudAge.Maximum = 60 : nudAge.Value = 18
        AddRow("Age:", nudAge, 100)
        radMale.Text = "Male" : radMale.Checked = True : radMale.AutoSize = True
        radFemale.Text = "Female" : radFemale.AutoSize = True
        Dim flow As New FlowLayoutPanel() With {.Width = 200, .Height = 25}
        flow.Controls.AddRange(New Control() {radMale, radFemale})   ' radios share one container
        AddRow("Gender:", flow, 130)
        chkNews.Text = "Subscribe to newsletter" : chkNews.AutoSize = True
        AddRow("", chkNews, 160)
        ' buttons
        btnAdd.Text = "Add" : btnRemove.Text = "Remove" : btnSave.Text = "Save..."
        btnAdd.SetBounds(110, 190, 75, 28)
        btnRemove.SetBounds(190, 190, 75, 28)
        btnSave.SetBounds(270, 190, 75, 28)
        Me.Controls.AddRange(New Control() {btnAdd, btnRemove, btnSave})
        ' list
        lstStudents.SetBounds(10, 230, 440, 120)
        Me.Controls.Add(lstStudents)
        ' tooltip + runtime event wiring with AddHandler (no Handles clause on the Sub below)
        tip.SetToolTip(btnAdd, "Add the student to the list")
        AddHandler lstStudents.DoubleClick, AddressOf lstStudents_DoubleClick
    End Sub
    ' ----- VALIDATION -----
    Private Function ValidateInput() As Boolean
        ep.Clear()
        If txtName.Text.Trim() = "" Then
            ep.SetError(txtName, "Name is required")
            txtName.Focus()
            Return False
        End If
        If cboCourse.SelectedIndex = -1 Then
            ep.SetError(cboCourse, "Please pick a course")


      Return False
        End If
        Return True
    End Function
    ' ----- BUTTON EVENTS -----
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not ValidateInput() Then Return
        Dim gender As String = If(radMale.Checked, "M", "F")
        Dim line As String = $"{txtName.Text.Trim()} | {cboCourse.Text} | {nudAge.Value} | {gender}"
        If chkNews.Checked Then line &= " | Newsletter"
        lstStudents.Items.Add(line)
        txtName.Clear()
        txtName.Focus()
    End Sub
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If lstStudents.SelectedIndex < 0 Then Return
        Dim r = MessageBox.Show("Remove the selected student?", "Confirm",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If r = DialogResult.Yes Then lstStudents.Items.RemoveAt(lstStudents.SelectedIndex)
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Using sfd As New SaveFileDialog() With {.Filter = "Text files|*.txt", .FileName = "students.txt"}
            If sfd.ShowDialog() = DialogResult.OK Then
                File.WriteAllLines(sfd.FileName, lstStudents.Items.Cast(Of String)())
                MessageBox.Show("Saved " & lstStudents.Items.Count & " students.", "Saved",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub
    ' ----- OTHER CONTROL EVENTS -----
    Private Sub txtName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtName.KeyPress
        If Char.IsDigit(e.KeyChar) Then e.Handled = True          ' no digits in a name
    End Sub
    Private Sub cboCourse_SelectedIndexChanged(sender As Object, e As EventArgs) _
            Handles cboCourse.SelectedIndexChanged
        Me.Text = "Student Registration - " & cboCourse.Text
    End Sub
    Private Sub lstStudents_DoubleClick(sender As Object, e As EventArgs)   ' wired with AddHandler
        If lstStudents.SelectedIndex >= 0 Then MessageBox.Show(lstStudents.SelectedItem.ToString())
    End Sub
    Private Sub tmrClock_Tick(sender As Object, e As EventArgs) Handles tmrClock.Tick
        lblClock.Text = DateTime.Now.ToString("HH:mm:ss")
    End Sub

     Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Me.Close()                                                  ' triggers FormClosing below
    End Sub
    ' ----- FORM EVENTS -----
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.S Then
            btnSave.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("Exit the application?", "Quit", MessageBoxButtons.YesNo) = DialogResult.No Then
            e.Cancel = True                                         ' stay open
        End If
    End Sub
End Class