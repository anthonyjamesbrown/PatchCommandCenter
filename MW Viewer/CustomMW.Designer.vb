<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomMW
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
        Me.Bttn_OK = New System.Windows.Forms.Button()
        Me.Bttn_Cancel = New System.Windows.Forms.Button()
        Me.Label_CollectionName = New System.Windows.Forms.Label()
        Me.Label_StartLocalTime = New System.Windows.Forms.Label()
        Me.Label_Timezone = New System.Windows.Forms.Label()
        Me.Label_EndLocalTime = New System.Windows.Forms.Label()
        Me.Label_UTCStartTime = New System.Windows.Forms.Label()
        Me.Label_UTCEndTime = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TextBox_Day = New System.Windows.Forms.TextBox()
        Me.Label_Day = New System.Windows.Forms.Label()
        Me.TextBox_Duration = New System.Windows.Forms.TextBox()
        Me.TextBox_UTCEndTime = New System.Windows.Forms.TextBox()
        Me.TextBox_UTCStartTime = New System.Windows.Forms.TextBox()
        Me.TextBox_LocalEndTime = New System.Windows.Forms.TextBox()
        Me.TextBox_LocalStartTime = New System.Windows.Forms.TextBox()
        Me.TextBox_Timezone = New System.Windows.Forms.TextBox()
        Me.TextBox_CollectionName = New System.Windows.Forms.TextBox()
        Me.Label_Duration = New System.Windows.Forms.Label()
        Me.DateTimePicker_Adjusted = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.NumericUpDown_NewDuration = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.NumericUpDown_NewDuration, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Bttn_OK
        '
        Me.Bttn_OK.Location = New System.Drawing.Point(313, 52)
        Me.Bttn_OK.Name = "Bttn_OK"
        Me.Bttn_OK.Size = New System.Drawing.Size(75, 23)
        Me.Bttn_OK.TabIndex = 0
        Me.Bttn_OK.Text = "Update"
        Me.Bttn_OK.UseVisualStyleBackColor = True
        '
        'Bttn_Cancel
        '
        Me.Bttn_Cancel.Location = New System.Drawing.Point(326, 335)
        Me.Bttn_Cancel.Name = "Bttn_Cancel"
        Me.Bttn_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Bttn_Cancel.TabIndex = 1
        Me.Bttn_Cancel.Text = "Cancel"
        Me.Bttn_Cancel.UseVisualStyleBackColor = True
        '
        'Label_CollectionName
        '
        Me.Label_CollectionName.AutoSize = True
        Me.Label_CollectionName.Location = New System.Drawing.Point(6, 27)
        Me.Label_CollectionName.Name = "Label_CollectionName"
        Me.Label_CollectionName.Size = New System.Drawing.Size(87, 13)
        Me.Label_CollectionName.TabIndex = 2
        Me.Label_CollectionName.Text = "Collection Name:"
        '
        'Label_StartLocalTime
        '
        Me.Label_StartLocalTime.AutoSize = True
        Me.Label_StartLocalTime.Location = New System.Drawing.Point(6, 103)
        Me.Label_StartLocalTime.Name = "Label_StartLocalTime"
        Me.Label_StartLocalTime.Size = New System.Drawing.Size(87, 13)
        Me.Label_StartLocalTime.TabIndex = 3
        Me.Label_StartLocalTime.Text = "Local Start Time:"
        '
        'Label_Timezone
        '
        Me.Label_Timezone.AutoSize = True
        Me.Label_Timezone.Location = New System.Drawing.Point(6, 51)
        Me.Label_Timezone.Name = "Label_Timezone"
        Me.Label_Timezone.Size = New System.Drawing.Size(85, 13)
        Me.Label_Timezone.TabIndex = 4
        Me.Label_Timezone.Text = "Local Timezone:"
        '
        'Label_EndLocalTime
        '
        Me.Label_EndLocalTime.AutoSize = True
        Me.Label_EndLocalTime.Location = New System.Drawing.Point(6, 127)
        Me.Label_EndLocalTime.Name = "Label_EndLocalTime"
        Me.Label_EndLocalTime.Size = New System.Drawing.Size(84, 13)
        Me.Label_EndLocalTime.TabIndex = 5
        Me.Label_EndLocalTime.Text = "Local End Time:"
        '
        'Label_UTCStartTime
        '
        Me.Label_UTCStartTime.AutoSize = True
        Me.Label_UTCStartTime.Location = New System.Drawing.Point(6, 151)
        Me.Label_UTCStartTime.Name = "Label_UTCStartTime"
        Me.Label_UTCStartTime.Size = New System.Drawing.Size(83, 13)
        Me.Label_UTCStartTime.TabIndex = 6
        Me.Label_UTCStartTime.Text = "UTC Start Time:"
        '
        'Label_UTCEndTime
        '
        Me.Label_UTCEndTime.AutoSize = True
        Me.Label_UTCEndTime.Location = New System.Drawing.Point(6, 175)
        Me.Label_UTCEndTime.Name = "Label_UTCEndTime"
        Me.Label_UTCEndTime.Size = New System.Drawing.Size(80, 13)
        Me.Label_UTCEndTime.TabIndex = 7
        Me.Label_UTCEndTime.Text = "UTC End Time:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBox_Day)
        Me.GroupBox1.Controls.Add(Me.Label_Day)
        Me.GroupBox1.Controls.Add(Me.TextBox_Duration)
        Me.GroupBox1.Controls.Add(Me.TextBox_UTCEndTime)
        Me.GroupBox1.Controls.Add(Me.TextBox_UTCStartTime)
        Me.GroupBox1.Controls.Add(Me.TextBox_LocalEndTime)
        Me.GroupBox1.Controls.Add(Me.TextBox_LocalStartTime)
        Me.GroupBox1.Controls.Add(Me.TextBox_Timezone)
        Me.GroupBox1.Controls.Add(Me.TextBox_CollectionName)
        Me.GroupBox1.Controls.Add(Me.Label_Duration)
        Me.GroupBox1.Controls.Add(Me.Label_CollectionName)
        Me.GroupBox1.Controls.Add(Me.Label_UTCEndTime)
        Me.GroupBox1.Controls.Add(Me.Label_Timezone)
        Me.GroupBox1.Controls.Add(Me.Label_UTCStartTime)
        Me.GroupBox1.Controls.Add(Me.Label_StartLocalTime)
        Me.GroupBox1.Controls.Add(Me.Label_EndLocalTime)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(400, 227)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Collection MW Info"
        '
        'TextBox_Day
        '
        Me.TextBox_Day.Location = New System.Drawing.Point(97, 74)
        Me.TextBox_Day.Name = "TextBox_Day"
        Me.TextBox_Day.ReadOnly = True
        Me.TextBox_Day.Size = New System.Drawing.Size(292, 20)
        Me.TextBox_Day.TabIndex = 17
        '
        'Label_Day
        '
        Me.Label_Day.AutoSize = True
        Me.Label_Day.Location = New System.Drawing.Point(6, 77)
        Me.Label_Day.Name = "Label_Day"
        Me.Label_Day.Size = New System.Drawing.Size(29, 13)
        Me.Label_Day.TabIndex = 16
        Me.Label_Day.Text = "Day:"
        '
        'TextBox_Duration
        '
        Me.TextBox_Duration.Location = New System.Drawing.Point(97, 196)
        Me.TextBox_Duration.Name = "TextBox_Duration"
        Me.TextBox_Duration.ReadOnly = True
        Me.TextBox_Duration.Size = New System.Drawing.Size(67, 20)
        Me.TextBox_Duration.TabIndex = 15
        '
        'TextBox_UTCEndTime
        '
        Me.TextBox_UTCEndTime.Location = New System.Drawing.Point(97, 172)
        Me.TextBox_UTCEndTime.Name = "TextBox_UTCEndTime"
        Me.TextBox_UTCEndTime.ReadOnly = True
        Me.TextBox_UTCEndTime.Size = New System.Drawing.Size(292, 20)
        Me.TextBox_UTCEndTime.TabIndex = 14
        '
        'TextBox_UTCStartTime
        '
        Me.TextBox_UTCStartTime.Location = New System.Drawing.Point(97, 148)
        Me.TextBox_UTCStartTime.Name = "TextBox_UTCStartTime"
        Me.TextBox_UTCStartTime.ReadOnly = True
        Me.TextBox_UTCStartTime.Size = New System.Drawing.Size(292, 20)
        Me.TextBox_UTCStartTime.TabIndex = 13
        '
        'TextBox_LocalEndTime
        '
        Me.TextBox_LocalEndTime.Location = New System.Drawing.Point(97, 124)
        Me.TextBox_LocalEndTime.Name = "TextBox_LocalEndTime"
        Me.TextBox_LocalEndTime.ReadOnly = True
        Me.TextBox_LocalEndTime.Size = New System.Drawing.Size(292, 20)
        Me.TextBox_LocalEndTime.TabIndex = 12
        '
        'TextBox_LocalStartTime
        '
        Me.TextBox_LocalStartTime.Location = New System.Drawing.Point(97, 100)
        Me.TextBox_LocalStartTime.Name = "TextBox_LocalStartTime"
        Me.TextBox_LocalStartTime.ReadOnly = True
        Me.TextBox_LocalStartTime.Size = New System.Drawing.Size(292, 20)
        Me.TextBox_LocalStartTime.TabIndex = 11
        '
        'TextBox_Timezone
        '
        Me.TextBox_Timezone.Location = New System.Drawing.Point(97, 48)
        Me.TextBox_Timezone.Name = "TextBox_Timezone"
        Me.TextBox_Timezone.ReadOnly = True
        Me.TextBox_Timezone.Size = New System.Drawing.Size(292, 20)
        Me.TextBox_Timezone.TabIndex = 10
        '
        'TextBox_CollectionName
        '
        Me.TextBox_CollectionName.Location = New System.Drawing.Point(97, 24)
        Me.TextBox_CollectionName.Name = "TextBox_CollectionName"
        Me.TextBox_CollectionName.ReadOnly = True
        Me.TextBox_CollectionName.Size = New System.Drawing.Size(292, 20)
        Me.TextBox_CollectionName.TabIndex = 9
        '
        'Label_Duration
        '
        Me.Label_Duration.AutoSize = True
        Me.Label_Duration.Location = New System.Drawing.Point(6, 199)
        Me.Label_Duration.Name = "Label_Duration"
        Me.Label_Duration.Size = New System.Drawing.Size(50, 13)
        Me.Label_Duration.TabIndex = 8
        Me.Label_Duration.Text = "Duration:"
        '
        'DateTimePicker_Adjusted
        '
        Me.DateTimePicker_Adjusted.CustomFormat = "ddddd, MMMM dd, yyyy hh:mm:ss tt"
        Me.DateTimePicker_Adjusted.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker_Adjusted.Location = New System.Drawing.Point(96, 25)
        Me.DateTimePicker_Adjusted.Name = "DateTimePicker_Adjusted"
        Me.DateTimePicker_Adjusted.Size = New System.Drawing.Size(292, 20)
        Me.DateTimePicker_Adjusted.TabIndex = 9
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.NumericUpDown_NewDuration)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.DateTimePicker_Adjusted)
        Me.GroupBox2.Controls.Add(Me.Bttn_OK)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 246)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(399, 83)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Adjust Schedule"
        '
        'NumericUpDown_NewDuration
        '
        Me.NumericUpDown_NewDuration.Location = New System.Drawing.Point(96, 52)
        Me.NumericUpDown_NewDuration.Name = "NumericUpDown_NewDuration"
        Me.NumericUpDown_NewDuration.Size = New System.Drawing.Size(120, 20)
        Me.NumericUpDown_NewDuration.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Duration (hrs):"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Start Time (UTC):"
        '
        'CustomMW
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 365)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Bttn_Cancel)
        Me.Name = "CustomMW"
        Me.Text = "Custom MW"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.NumericUpDown_NewDuration, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Bttn_OK As System.Windows.Forms.Button
    Friend WithEvents Bttn_Cancel As System.Windows.Forms.Button
    Friend WithEvents Label_CollectionName As System.Windows.Forms.Label
    Friend WithEvents Label_StartLocalTime As System.Windows.Forms.Label
    Friend WithEvents Label_Timezone As System.Windows.Forms.Label
    Friend WithEvents Label_EndLocalTime As System.Windows.Forms.Label
    Friend WithEvents Label_UTCStartTime As System.Windows.Forms.Label
    Friend WithEvents Label_UTCEndTime As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_Duration As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_UTCEndTime As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_UTCStartTime As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_LocalEndTime As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_LocalStartTime As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Timezone As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_CollectionName As System.Windows.Forms.TextBox
    Friend WithEvents Label_Duration As System.Windows.Forms.Label
    Friend WithEvents TextBox_Day As System.Windows.Forms.TextBox
    Friend WithEvents Label_Day As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker_Adjusted As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown_NewDuration As System.Windows.Forms.NumericUpDown
End Class
