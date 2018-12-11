<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HotfixDialog
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
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ServerName_Label = New System.Windows.Forms.Label()
        Me.ListView_Hotfixes = New System.Windows.Forms.ListView()
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopySelectedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Server Name:"
        '
        'ServerName_Label
        '
        Me.ServerName_Label.AutoSize = True
        Me.ServerName_Label.Location = New System.Drawing.Point(91, 13)
        Me.ServerName_Label.Name = "ServerName_Label"
        Me.ServerName_Label.Size = New System.Drawing.Size(0, 13)
        Me.ServerName_Label.TabIndex = 1
        '
        'ListView_Hotfixes
        '
        Me.ListView_Hotfixes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader11, Me.ColumnHeader12, Me.ColumnHeader13, Me.ColumnHeader14, Me.ColumnHeader15})
        Me.ListView_Hotfixes.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView_Hotfixes.Location = New System.Drawing.Point(16, 40)
        Me.ListView_Hotfixes.Name = "ListView_Hotfixes"
        Me.ListView_Hotfixes.Size = New System.Drawing.Size(843, 565)
        Me.ListView_Hotfixes.TabIndex = 2
        Me.ListView_Hotfixes.UseCompatibleStateImageBehavior = False
        Me.ListView_Hotfixes.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "HotfixID"
        Me.ColumnHeader11.Width = 98
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Description"
        Me.ColumnHeader12.Width = 167
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Installed On"
        Me.ColumnHeader13.Width = 102
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "Installed By"
        Me.ColumnHeader14.Width = 198
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Caption"
        Me.ColumnHeader15.Width = 266
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(784, 613)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem, Me.CopySelectedToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(150, 48)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'CopySelectedToolStripMenuItem
        '
        Me.CopySelectedToolStripMenuItem.Name = "CopySelectedToolStripMenuItem"
        Me.CopySelectedToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.CopySelectedToolStripMenuItem.Text = "Copy Selected"
        '
        'HotfixDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(873, 648)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListView_Hotfixes)
        Me.Controls.Add(Me.ServerName_Label)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.Name = "HotfixDialog"
        Me.Text = "Installed Hotfixes"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ServerName_Label As System.Windows.Forms.Label
    Friend WithEvents ListView_Hotfixes As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopySelectedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
