<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HotfixSpotCheck
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
        Me.ListView_Serverlist = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Bttn_HotfixQuery = New System.Windows.Forms.Button()
        Me.Hotfix_TextBox = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView_Serverlist
        '
        Me.ListView_Serverlist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView_Serverlist.GridLines = True
        Me.ListView_Serverlist.Location = New System.Drawing.Point(12, 67)
        Me.ListView_Serverlist.Name = "ListView_Serverlist"
        Me.ListView_Serverlist.Size = New System.Drawing.Size(351, 568)
        Me.ListView_Serverlist.TabIndex = 0
        Me.ListView_Serverlist.UseCompatibleStateImageBehavior = False
        Me.ListView_Serverlist.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Server Name"
        Me.ColumnHeader1.Width = 177
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Hotfix Status"
        Me.ColumnHeader2.Width = 167
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Bttn_HotfixQuery)
        Me.GroupBox1.Controls.Add(Me.Hotfix_TextBox)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(351, 49)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Hotfix Query"
        '
        'Bttn_HotfixQuery
        '
        Me.Bttn_HotfixQuery.Location = New System.Drawing.Point(270, 18)
        Me.Bttn_HotfixQuery.Name = "Bttn_HotfixQuery"
        Me.Bttn_HotfixQuery.Size = New System.Drawing.Size(75, 23)
        Me.Bttn_HotfixQuery.TabIndex = 1
        Me.Bttn_HotfixQuery.Text = "Hotfix Query"
        Me.Bttn_HotfixQuery.UseVisualStyleBackColor = True
        '
        'Hotfix_TextBox
        '
        Me.Hotfix_TextBox.Location = New System.Drawing.Point(7, 20)
        Me.Hotfix_TextBox.Name = "Hotfix_TextBox"
        Me.Hotfix_TextBox.Size = New System.Drawing.Size(257, 20)
        Me.Hotfix_TextBox.TabIndex = 0
        '
        'HotfixSpotCheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(374, 648)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ListView_Serverlist)
        Me.Name = "HotfixSpotCheck"
        Me.Text = "HotfixSpotCheck"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListView_Serverlist As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Bttn_HotfixQuery As System.Windows.Forms.Button
    Friend WithEvents Hotfix_TextBox As System.Windows.Forms.TextBox
End Class
