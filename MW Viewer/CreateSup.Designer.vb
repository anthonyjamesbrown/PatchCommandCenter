<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CreateSup
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button_GetUpdates = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_SupName = New System.Windows.Forms.TextBox()
        Me.ListView_Updates = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.NumericUpDown_Days = New System.Windows.Forms.NumericUpDown()
        Me.Button_CreateSUG = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label_status = New System.Windows.Forms.Label()
        Me.Label_Progress = New System.Windows.Forms.Label()
        Me.ProgressBar_DL = New System.Windows.Forms.ProgressBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TextBox_PackageName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button_Copy = New System.Windows.Forms.Button()
        Me.Button_Export2Excel = New System.Windows.Forms.Button()
        Me.ToolTip_Excel = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTip_Copy = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.NumericUpDown_Days, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_GetUpdates
        '
        Me.Button_GetUpdates.Location = New System.Drawing.Point(433, 75)
        Me.Button_GetUpdates.Name = "Button_GetUpdates"
        Me.Button_GetUpdates.Size = New System.Drawing.Size(75, 23)
        Me.Button_GetUpdates.TabIndex = 0
        Me.Button_GetUpdates.Text = "Get Updates"
        Me.Button_GetUpdates.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(153, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Software Update Group Name:"
        '
        'TextBox_SupName
        '
        Me.TextBox_SupName.Location = New System.Drawing.Point(165, 24)
        Me.TextBox_SupName.Name = "TextBox_SupName"
        Me.TextBox_SupName.Size = New System.Drawing.Size(343, 20)
        Me.TextBox_SupName.TabIndex = 2
        '
        'ListView_Updates
        '
        Me.ListView_Updates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_Updates.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7})
        Me.ListView_Updates.FullRowSelect = True
        Me.ListView_Updates.GridLines = True
        Me.ListView_Updates.Location = New System.Drawing.Point(12, 119)
        Me.ListView_Updates.Name = "ListView_Updates"
        Me.ListView_Updates.Size = New System.Drawing.Size(1318, 713)
        Me.ListView_Updates.TabIndex = 3
        Me.ListView_Updates.UseCompatibleStateImageBehavior = False
        Me.ListView_Updates.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "CI ID"
        Me.ColumnHeader1.Width = 92
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Article ID"
        Me.ColumnHeader2.Width = 84
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Bulletin ID"
        Me.ColumnHeader3.Width = 91
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Title"
        Me.ColumnHeader4.Width = 656
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Date Revised"
        Me.ColumnHeader5.Width = 103
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "URL"
        Me.ColumnHeader6.Width = 196
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Severity"
        Me.ColumnHeader7.Width = 90
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Updates in Last Days:"
        '
        'NumericUpDown_Days
        '
        Me.NumericUpDown_Days.Location = New System.Drawing.Point(165, 75)
        Me.NumericUpDown_Days.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.NumericUpDown_Days.Minimum = New Decimal(New Integer() {14, 0, 0, 0})
        Me.NumericUpDown_Days.Name = "NumericUpDown_Days"
        Me.NumericUpDown_Days.Size = New System.Drawing.Size(120, 20)
        Me.NumericUpDown_Days.TabIndex = 5
        Me.NumericUpDown_Days.TabStop = False
        Me.NumericUpDown_Days.Value = New Decimal(New Integer() {14, 0, 0, 0})
        '
        'Button_CreateSUG
        '
        Me.Button_CreateSUG.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_CreateSUG.Enabled = False
        Me.Button_CreateSUG.Location = New System.Drawing.Point(639, 75)
        Me.Button_CreateSUG.Name = "Button_CreateSUG"
        Me.Button_CreateSUG.Size = New System.Drawing.Size(75, 23)
        Me.Button_CreateSUG.TabIndex = 6
        Me.Button_CreateSUG.Text = "Create SUG"
        Me.Button_CreateSUG.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label_status)
        Me.GroupBox1.Controls.Add(Me.Label_Progress)
        Me.GroupBox1.Controls.Add(Me.ProgressBar_DL)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Button_CreateSUG)
        Me.GroupBox1.Location = New System.Drawing.Point(544, 9)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(720, 104)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Software Update Group and Package Creation"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Details:"
        '
        'Label_status
        '
        Me.Label_status.AutoSize = True
        Me.Label_status.Location = New System.Drawing.Point(54, 24)
        Me.Label_status.Name = "Label_status"
        Me.Label_status.Size = New System.Drawing.Size(0, 13)
        Me.Label_status.TabIndex = 10
        '
        'Label_Progress
        '
        Me.Label_Progress.AutoSize = True
        Me.Label_Progress.Location = New System.Drawing.Point(54, 52)
        Me.Label_Progress.Name = "Label_Progress"
        Me.Label_Progress.Size = New System.Drawing.Size(0, 13)
        Me.Label_Progress.TabIndex = 9
        '
        'ProgressBar_DL
        '
        Me.ProgressBar_DL.Location = New System.Drawing.Point(9, 75)
        Me.ProgressBar_DL.Name = "ProgressBar_DL"
        Me.ProgressBar_DL.Size = New System.Drawing.Size(624, 23)
        Me.ProgressBar_DL.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Status:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TextBox_PackageName)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.NumericUpDown_Days)
        Me.GroupBox2.Controls.Add(Me.Button_GetUpdates)
        Me.GroupBox2.Controls.Add(Me.TextBox_SupName)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 9)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(514, 104)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Software Update Group"
        '
        'TextBox_PackageName
        '
        Me.TextBox_PackageName.Location = New System.Drawing.Point(165, 49)
        Me.TextBox_PackageName.Name = "TextBox_PackageName"
        Me.TextBox_PackageName.Size = New System.Drawing.Size(343, 20)
        Me.TextBox_PackageName.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 52)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Package Dir Name:"
        '
        'Button_Copy
        '
        Me.Button_Copy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Copy.Image = Global.WindowsApplication1.My.Resources.Resources.Edit_copy_green4
        Me.Button_Copy.Location = New System.Drawing.Point(1270, 86)
        Me.Button_Copy.Name = "Button_Copy"
        Me.Button_Copy.Size = New System.Drawing.Size(27, 27)
        Me.Button_Copy.TabIndex = 19
        Me.ToolTip_Copy.SetToolTip(Me.Button_Copy, "Copy All update data to the clipboard.")
        Me.Button_Copy.UseVisualStyleBackColor = True
        '
        'Button_Export2Excel
        '
        Me.Button_Export2Excel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Export2Excel.Image = Global.WindowsApplication1.My.Resources.Resources.Excel_icon
        Me.Button_Export2Excel.Location = New System.Drawing.Point(1303, 86)
        Me.Button_Export2Excel.Name = "Button_Export2Excel"
        Me.Button_Export2Excel.Size = New System.Drawing.Size(27, 27)
        Me.Button_Export2Excel.TabIndex = 18
        Me.Button_Export2Excel.Tag = "Export to Excel"
        Me.ToolTip_Excel.SetToolTip(Me.Button_Export2Excel, "Export all of the update data to Excel.")
        Me.Button_Export2Excel.UseVisualStyleBackColor = True
        '
        'ToolTip_Excel
        '
        Me.ToolTip_Excel.ToolTipTitle = "Export to Excel"
        '
        'ToolTip_Copy
        '
        Me.ToolTip_Copy.ToolTipTitle = "Copy"
        '
        'CreateSup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button_Copy)
        Me.Controls.Add(Me.Button_Export2Excel)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ListView_Updates)
        Me.Name = "CreateSup"
        Me.Size = New System.Drawing.Size(1343, 849)
        CType(Me.NumericUpDown_Days, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_GetUpdates As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox_SupName As System.Windows.Forms.TextBox
    Friend WithEvents ListView_Updates As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown_Days As System.Windows.Forms.NumericUpDown
    Friend WithEvents Button_CreateSUG As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label_status As System.Windows.Forms.Label
    Friend WithEvents Label_Progress As System.Windows.Forms.Label
    Friend WithEvents ProgressBar_DL As System.Windows.Forms.ProgressBar
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_PackageName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Button_Copy As System.Windows.Forms.Button
    Friend WithEvents Button_Export2Excel As System.Windows.Forms.Button
    Friend WithEvents ToolTip_Excel As System.Windows.Forms.ToolTip
    Friend WithEvents ToolTip_Copy As System.Windows.Forms.ToolTip

End Class
