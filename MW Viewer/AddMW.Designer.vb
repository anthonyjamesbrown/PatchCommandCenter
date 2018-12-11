<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddMW
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.GroupBox_Citrix = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label_Type = New System.Windows.Forms.Label()
        Me.TextBox_Reboot = New System.Windows.Forms.TextBox()
        Me.NumericUpDown_Duration = New System.Windows.Forms.NumericUpDown()
        Me.ComboBox_AMPM = New System.Windows.Forms.ComboBox()
        Me.NumericUpDown_Minutes = New System.Windows.Forms.NumericUpDown()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.NumericUpDown_Hour = New System.Windows.Forms.NumericUpDown()
        Me.ComboBox_Day = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox_CollComment = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CheckBox_Citrix = New System.Windows.Forms.CheckBox()
        Me.ComboBox_MWType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_Name = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TextBox_FullPath = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBox_FormatTime = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TextBox_ADDesc = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TextBox_FullName = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Button_Create = New System.Windows.Forms.Button()
        Me.Button_Exit = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.ComboBox_Class = New System.Windows.Forms.ComboBox()
        Me.GroupBox_Citrix.SuspendLayout()
        CType(Me.NumericUpDown_Duration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_Minutes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_Hour, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Citrix
        '
        Me.GroupBox_Citrix.Controls.Add(Me.ComboBox_Class)
        Me.GroupBox_Citrix.Controls.Add(Me.Label14)
        Me.GroupBox_Citrix.Controls.Add(Me.Label10)
        Me.GroupBox_Citrix.Controls.Add(Me.Label_Type)
        Me.GroupBox_Citrix.Controls.Add(Me.TextBox_Reboot)
        Me.GroupBox_Citrix.Controls.Add(Me.NumericUpDown_Duration)
        Me.GroupBox_Citrix.Controls.Add(Me.ComboBox_AMPM)
        Me.GroupBox_Citrix.Controls.Add(Me.NumericUpDown_Minutes)
        Me.GroupBox_Citrix.Controls.Add(Me.Label8)
        Me.GroupBox_Citrix.Controls.Add(Me.NumericUpDown_Hour)
        Me.GroupBox_Citrix.Controls.Add(Me.ComboBox_Day)
        Me.GroupBox_Citrix.Controls.Add(Me.Label7)
        Me.GroupBox_Citrix.Controls.Add(Me.Label6)
        Me.GroupBox_Citrix.Controls.Add(Me.Label5)
        Me.GroupBox_Citrix.Controls.Add(Me.Label4)
        Me.GroupBox_Citrix.Controls.Add(Me.TextBox_CollComment)
        Me.GroupBox_Citrix.Controls.Add(Me.Label3)
        Me.GroupBox_Citrix.Controls.Add(Me.CheckBox_Citrix)
        Me.GroupBox_Citrix.Controls.Add(Me.ComboBox_MWType)
        Me.GroupBox_Citrix.Controls.Add(Me.Label2)
        Me.GroupBox_Citrix.Controls.Add(Me.TextBox_Name)
        Me.GroupBox_Citrix.Controls.Add(Me.Label1)
        Me.GroupBox_Citrix.Location = New System.Drawing.Point(13, 13)
        Me.GroupBox_Citrix.Name = "GroupBox_Citrix"
        Me.GroupBox_Citrix.Size = New System.Drawing.Size(594, 232)
        Me.GroupBox_Citrix.TabIndex = 0
        Me.GroupBox_Citrix.TabStop = False
        Me.GroupBox_Citrix.Text = "MW Information"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(121, 70)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(33, 13)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "MW_"
        '
        'Label_Type
        '
        Me.Label_Type.AutoSize = True
        Me.Label_Type.Location = New System.Drawing.Point(402, 70)
        Me.Label_Type.Name = "Label_Type"
        Me.Label_Type.Size = New System.Drawing.Size(0, 13)
        Me.Label_Type.TabIndex = 19
        '
        'TextBox_Reboot
        '
        Me.TextBox_Reboot.Location = New System.Drawing.Point(124, 198)
        Me.TextBox_Reboot.Name = "TextBox_Reboot"
        Me.TextBox_Reboot.Size = New System.Drawing.Size(374, 20)
        Me.TextBox_Reboot.TabIndex = 11
        '
        'NumericUpDown_Duration
        '
        Me.NumericUpDown_Duration.Location = New System.Drawing.Point(124, 172)
        Me.NumericUpDown_Duration.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.NumericUpDown_Duration.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown_Duration.Name = "NumericUpDown_Duration"
        Me.NumericUpDown_Duration.Size = New System.Drawing.Size(120, 20)
        Me.NumericUpDown_Duration.TabIndex = 10
        Me.NumericUpDown_Duration.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ComboBox_AMPM
        '
        Me.ComboBox_AMPM.FormattingEnabled = True
        Me.ComboBox_AMPM.Items.AddRange(New Object() {"AM", "PM"})
        Me.ComboBox_AMPM.Location = New System.Drawing.Point(283, 145)
        Me.ComboBox_AMPM.Name = "ComboBox_AMPM"
        Me.ComboBox_AMPM.Size = New System.Drawing.Size(57, 21)
        Me.ComboBox_AMPM.TabIndex = 9
        Me.ComboBox_AMPM.Text = "AM"
        '
        'NumericUpDown_Minutes
        '
        Me.NumericUpDown_Minutes.Location = New System.Drawing.Point(212, 146)
        Me.NumericUpDown_Minutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.NumericUpDown_Minutes.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown_Minutes.Name = "NumericUpDown_Minutes"
        Me.NumericUpDown_Minutes.Size = New System.Drawing.Size(65, 20)
        Me.NumericUpDown_Minutes.TabIndex = 8
        Me.NumericUpDown_Minutes.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(196, 148)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(10, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = ":"
        '
        'NumericUpDown_Hour
        '
        Me.NumericUpDown_Hour.Location = New System.Drawing.Point(124, 146)
        Me.NumericUpDown_Hour.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.NumericUpDown_Hour.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown_Hour.Name = "NumericUpDown_Hour"
        Me.NumericUpDown_Hour.Size = New System.Drawing.Size(66, 20)
        Me.NumericUpDown_Hour.TabIndex = 7
        Me.NumericUpDown_Hour.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ComboBox_Day
        '
        Me.ComboBox_Day.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBox_Day.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Day.FormattingEnabled = True
        Me.ComboBox_Day.Items.AddRange(New Object() {"Tuesday0", "Wednesday0", "Thursday0", "Friday0", "Saturday0", "Sunday0", "Monday0", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday2", "Wednesday2", "Thursday2", "Friday2", "Saturday2", "Sunday2", "Monday2 "})
        Me.ComboBox_Day.Location = New System.Drawing.Point(124, 119)
        Me.ComboBox_Day.Name = "ComboBox_Day"
        Me.ComboBox_Day.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_Day.TabIndex = 6
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(10, 201)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(102, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Reboot Handled by:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 174)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Duration:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 148)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "IW Start Time:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 122)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "IW Day:"
        '
        'TextBox_CollComment
        '
        Me.TextBox_CollComment.Location = New System.Drawing.Point(124, 93)
        Me.TextBox_CollComment.Name = "TextBox_CollComment"
        Me.TextBox_CollComment.Size = New System.Drawing.Size(374, 20)
        Me.TextBox_CollComment.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(103, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Collection Comment:"
        '
        'CheckBox_Citrix
        '
        Me.CheckBox_Citrix.AutoSize = True
        Me.CheckBox_Citrix.Location = New System.Drawing.Point(124, 44)
        Me.CheckBox_Citrix.Name = "CheckBox_Citrix"
        Me.CheckBox_Citrix.Size = New System.Drawing.Size(77, 17)
        Me.CheckBox_Citrix.TabIndex = 3
        Me.CheckBox_Citrix.Text = "Citrix MW?"
        Me.CheckBox_Citrix.UseVisualStyleBackColor = True
        '
        'ComboBox_MWType
        '
        Me.ComboBox_MWType.FormattingEnabled = True
        Me.ComboBox_MWType.Items.AddRange(New Object() {"(Auto Reboot)", "(Manual Reboot)"})
        Me.ComboBox_MWType.Location = New System.Drawing.Point(124, 17)
        Me.ComboBox_MWType.Name = "ComboBox_MWType"
        Me.ComboBox_MWType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_MWType.TabIndex = 1
        Me.ComboBox_MWType.Text = "(Auto Reboot)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Type:"
        '
        'TextBox_Name
        '
        Me.TextBox_Name.Location = New System.Drawing.Point(156, 67)
        Me.TextBox_Name.Name = "TextBox_Name"
        Me.TextBox_Name.Size = New System.Drawing.Size(240, 20)
        Me.TextBox_Name.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBox_FullPath)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.TextBox_FormatTime)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.TextBox_ADDesc)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.TextBox_FullName)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 252)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(594, 133)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Computed Values"
        '
        'TextBox_FullPath
        '
        Me.TextBox_FullPath.Enabled = False
        Me.TextBox_FullPath.Location = New System.Drawing.Point(124, 97)
        Me.TextBox_FullPath.Name = "TextBox_FullPath"
        Me.TextBox_FullPath.Size = New System.Drawing.Size(464, 20)
        Me.TextBox_FullPath.TabIndex = 4
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 100)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(81, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "Collection Path:"
        '
        'TextBox_FormatTime
        '
        Me.TextBox_FormatTime.Enabled = False
        Me.TextBox_FormatTime.Location = New System.Drawing.Point(124, 71)
        Me.TextBox_FormatTime.Name = "TextBox_FormatTime"
        Me.TextBox_FormatTime.Size = New System.Drawing.Size(153, 20)
        Me.TextBox_FormatTime.TabIndex = 3
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 74)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(72, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "IW Start Time"
        '
        'TextBox_ADDesc
        '
        Me.TextBox_ADDesc.Enabled = False
        Me.TextBox_ADDesc.Location = New System.Drawing.Point(124, 44)
        Me.TextBox_ADDesc.Name = "TextBox_ADDesc"
        Me.TextBox_ADDesc.Size = New System.Drawing.Size(464, 20)
        Me.TextBox_ADDesc.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 47)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(81, 13)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "AD Description:"
        '
        'TextBox_FullName
        '
        Me.TextBox_FullName.Enabled = False
        Me.TextBox_FullName.Location = New System.Drawing.Point(124, 17)
        Me.TextBox_FullName.Name = "TextBox_FullName"
        Me.TextBox_FullName.Size = New System.Drawing.Size(464, 20)
        Me.TextBox_FullName.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 20)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(57, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Full Name:"
        '
        'Button_Create
        '
        Me.Button_Create.Location = New System.Drawing.Point(449, 391)
        Me.Button_Create.Name = "Button_Create"
        Me.Button_Create.Size = New System.Drawing.Size(75, 23)
        Me.Button_Create.TabIndex = 1
        Me.Button_Create.Text = "Create"
        Me.Button_Create.UseVisualStyleBackColor = True
        '
        'Button_Exit
        '
        Me.Button_Exit.Location = New System.Drawing.Point(532, 391)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Exit.TabIndex = 2
        Me.Button_Exit.Text = "Close"
        Me.Button_Exit.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(280, 20)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(35, 13)
        Me.Label14.TabIndex = 21
        Me.Label14.Text = "Class:"
        '
        'ComboBox_Class
        '
        Me.ComboBox_Class.FormattingEnabled = True
        Me.ComboBox_Class.Items.AddRange(New Object() {"Prod", "Non-Prod"})
        Me.ComboBox_Class.Location = New System.Drawing.Point(321, 17)
        Me.ComboBox_Class.Name = "ComboBox_Class"
        Me.ComboBox_Class.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_Class.TabIndex = 2
        Me.ComboBox_Class.Text = "Prod"
        '
        'AddMW
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(622, 424)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.Button_Create)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox_Citrix)
        Me.Name = "AddMW"
        Me.Text = "Add a MW"
        Me.GroupBox_Citrix.ResumeLayout(False)
        Me.GroupBox_Citrix.PerformLayout()
        CType(Me.NumericUpDown_Duration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_Minutes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_Hour, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_Citrix As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBox_Citrix As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox_MWType As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Day As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox_CollComment As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Reboot As System.Windows.Forms.TextBox
    Friend WithEvents NumericUpDown_Duration As System.Windows.Forms.NumericUpDown
    Friend WithEvents ComboBox_AMPM As System.Windows.Forms.ComboBox
    Friend WithEvents NumericUpDown_Minutes As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown_Hour As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label_Type As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_FullName As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox_ADDesc As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextBox_FormatTime As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBox_FullPath As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Button_Create As System.Windows.Forms.Button
    Friend WithEvents Button_Exit As System.Windows.Forms.Button
    Friend WithEvents ComboBox_Class As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
End Class
