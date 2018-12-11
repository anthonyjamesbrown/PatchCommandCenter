<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdateStates
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button_Refresh = New System.Windows.Forms.Button()
        Me.TextBox_ServerName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView_Updates = New System.Windows.Forms.DataGridView()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StripLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ArticleID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BulletinID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Name1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StartTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Deadline = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EvaluationState = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ErrorCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView_Updates, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button_Refresh)
        Me.GroupBox1.Controls.Add(Me.TextBox_ServerName)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(785, 49)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Server"
        '
        'Button_Refresh
        '
        Me.Button_Refresh.Location = New System.Drawing.Point(704, 15)
        Me.Button_Refresh.Name = "Button_Refresh"
        Me.Button_Refresh.Size = New System.Drawing.Size(75, 23)
        Me.Button_Refresh.TabIndex = 2
        Me.Button_Refresh.Text = "Refresh"
        Me.Button_Refresh.UseVisualStyleBackColor = True
        '
        'TextBox_ServerName
        '
        Me.TextBox_ServerName.Enabled = False
        Me.TextBox_ServerName.Location = New System.Drawing.Point(85, 17)
        Me.TextBox_ServerName.Name = "TextBox_ServerName"
        Me.TextBox_ServerName.Size = New System.Drawing.Size(613, 20)
        Me.TextBox_ServerName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Server Name:"
        '
        'DataGridView_Updates
        '
        Me.DataGridView_Updates.AllowUserToAddRows = False
        Me.DataGridView_Updates.AllowUserToDeleteRows = False
        Me.DataGridView_Updates.AllowUserToResizeRows = False
        Me.DataGridView_Updates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView_Updates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_Updates.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ArticleID, Me.BulletinID, Me.Name1, Me.StartTime, Me.Deadline, Me.Description, Me.EvaluationState, Me.ErrorCode})
        Me.DataGridView_Updates.Location = New System.Drawing.Point(13, 68)
        Me.DataGridView_Updates.Name = "DataGridView_Updates"
        Me.DataGridView_Updates.ReadOnly = True
        Me.DataGridView_Updates.RowHeadersVisible = False
        Me.DataGridView_Updates.Size = New System.Drawing.Size(1247, 407)
        Me.DataGridView_Updates.TabIndex = 1
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StripLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 490)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1272, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StripLabel1
        '
        Me.StripLabel1.Name = "StripLabel1"
        Me.StripLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'ArticleID
        '
        Me.ArticleID.HeaderText = "ArticleID"
        Me.ArticleID.Name = "ArticleID"
        Me.ArticleID.ReadOnly = True
        Me.ArticleID.Width = 75
        '
        'BulletinID
        '
        Me.BulletinID.HeaderText = "BulletinID"
        Me.BulletinID.Name = "BulletinID"
        Me.BulletinID.ReadOnly = True
        Me.BulletinID.Width = 75
        '
        'Name1
        '
        Me.Name1.HeaderText = "Name"
        Me.Name1.Name = "Name1"
        Me.Name1.ReadOnly = True
        Me.Name1.Width = 300
        '
        'StartTime
        '
        Me.StartTime.HeaderText = "StartTime"
        Me.StartTime.Name = "StartTime"
        Me.StartTime.ReadOnly = True
        Me.StartTime.Width = 125
        '
        'Deadline
        '
        Me.Deadline.HeaderText = "Deadline"
        Me.Deadline.Name = "Deadline"
        Me.Deadline.ReadOnly = True
        Me.Deadline.Width = 125
        '
        'Description
        '
        Me.Description.HeaderText = "Description"
        Me.Description.Name = "Description"
        Me.Description.ReadOnly = True
        Me.Description.Width = 300
        '
        'EvaluationState
        '
        Me.EvaluationState.HeaderText = "EvaluationState"
        Me.EvaluationState.Name = "EvaluationState"
        Me.EvaluationState.ReadOnly = True
        '
        'ErrorCode
        '
        Me.ErrorCode.HeaderText = "ErrorCode"
        Me.ErrorCode.Name = "ErrorCode"
        Me.ErrorCode.ReadOnly = True
        Me.ErrorCode.Width = 200
        '
        'UpdateStates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1272, 512)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.DataGridView_Updates)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "UpdateStates"
        Me.Text = "Update States"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DataGridView_Updates, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Refresh As System.Windows.Forms.Button
    Friend WithEvents TextBox_ServerName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView_Updates As System.Windows.Forms.DataGridView
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StripLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ArticleID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BulletinID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Name1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StartTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Deadline As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Description As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EvaluationState As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ErrorCode As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
