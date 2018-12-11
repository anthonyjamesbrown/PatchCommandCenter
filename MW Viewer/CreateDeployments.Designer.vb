<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CreateDeployments
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button_GetDeployments = New System.Windows.Forms.Button()
        Me.ComboBox_SUGList = New System.Windows.Forms.ComboBox()
        Me.ListView_Deployments = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.GetMWDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckBox_CheckAll = New System.Windows.Forms.CheckBox()
        Me.Button_Set = New System.Windows.Forms.Button()
        Me.Button_Copy = New System.Windows.Forms.Button()
        Me.Button_Export2Excel = New System.Windows.Forms.Button()
        Me.ToolTip_Excel = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTip_Copy = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_CopySelected = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(155, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Software Update Group:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button_GetDeployments)
        Me.GroupBox1.Controls.Add(Me.ComboBox_SUGList)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(705, 55)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Create Deployments"
        '
        'Button_GetDeployments
        '
        Me.Button_GetDeployments.Location = New System.Drawing.Point(542, 22)
        Me.Button_GetDeployments.Name = "Button_GetDeployments"
        Me.Button_GetDeployments.Size = New System.Drawing.Size(157, 23)
        Me.Button_GetDeployments.TabIndex = 2
        Me.Button_GetDeployments.Text = "Query Collection Deployments"
        Me.Button_GetDeployments.UseVisualStyleBackColor = True
        '
        'ComboBox_SUGList
        '
        Me.ComboBox_SUGList.FormattingEnabled = True
        Me.ComboBox_SUGList.Location = New System.Drawing.Point(167, 24)
        Me.ComboBox_SUGList.Name = "ComboBox_SUGList"
        Me.ComboBox_SUGList.Size = New System.Drawing.Size(369, 21)
        Me.ComboBox_SUGList.TabIndex = 1
        '
        'ListView_Deployments
        '
        Me.ListView_Deployments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_Deployments.CheckBoxes = True
        Me.ListView_Deployments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9})
        Me.ListView_Deployments.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView_Deployments.FullRowSelect = True
        Me.ListView_Deployments.GridLines = True
        Me.ListView_Deployments.Location = New System.Drawing.Point(16, 98)
        Me.ListView_Deployments.Name = "ListView_Deployments"
        Me.ListView_Deployments.Size = New System.Drawing.Size(1310, 727)
        Me.ListView_Deployments.TabIndex = 2
        Me.ListView_Deployments.UseCompatibleStateImageBehavior = False
        Me.ListView_Deployments.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Collection Name"
        Me.ColumnHeader1.Width = 263
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Collection ID"
        Me.ColumnHeader2.Width = 80
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Last MW Name"
        Me.ColumnHeader3.Width = 172
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Nbr Of MWs"
        Me.ColumnHeader4.Width = 89
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Deployed"
        Me.ColumnHeader5.Width = 121
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Enabled"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Collection Comments"
        Me.ColumnHeader7.Width = 460
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Class"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "StartTime (Local)"
        Me.ColumnHeader9.Width = 160
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetMWDetailsToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(156, 26)
        '
        'GetMWDetailsToolStripMenuItem
        '
        Me.GetMWDetailsToolStripMenuItem.Name = "GetMWDetailsToolStripMenuItem"
        Me.GetMWDetailsToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.GetMWDetailsToolStripMenuItem.Text = "Get MW Details"
        '
        'CheckBox_CheckAll
        '
        Me.CheckBox_CheckAll.AutoSize = True
        Me.CheckBox_CheckAll.Location = New System.Drawing.Point(16, 75)
        Me.CheckBox_CheckAll.Name = "CheckBox_CheckAll"
        Me.CheckBox_CheckAll.Size = New System.Drawing.Size(37, 17)
        Me.CheckBox_CheckAll.TabIndex = 3
        Me.CheckBox_CheckAll.Text = "All"
        Me.CheckBox_CheckAll.UseVisualStyleBackColor = True
        '
        'Button_Set
        '
        Me.Button_Set.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Set.Enabled = False
        Me.Button_Set.Location = New System.Drawing.Point(59, 71)
        Me.Button_Set.Name = "Button_Set"
        Me.Button_Set.Size = New System.Drawing.Size(142, 23)
        Me.Button_Set.TabIndex = 4
        Me.Button_Set.Text = "Create Deployments!!!"
        Me.Button_Set.UseVisualStyleBackColor = False
        '
        'Button_Copy
        '
        Me.Button_Copy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Copy.Image = Global.WindowsApplication1.My.Resources.Resources.Edit_copy_green4
        Me.Button_Copy.Location = New System.Drawing.Point(1266, 65)
        Me.Button_Copy.Name = "Button_Copy"
        Me.Button_Copy.Size = New System.Drawing.Size(27, 27)
        Me.Button_Copy.TabIndex = 19
        Me.ToolTip_Copy.SetToolTip(Me.Button_Copy, "Copy All deployment data to the clipboard.")
        Me.Button_Copy.UseVisualStyleBackColor = True
        '
        'Button_Export2Excel
        '
        Me.Button_Export2Excel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Export2Excel.Image = Global.WindowsApplication1.My.Resources.Resources.Excel_icon
        Me.Button_Export2Excel.Location = New System.Drawing.Point(1299, 65)
        Me.Button_Export2Excel.Name = "Button_Export2Excel"
        Me.Button_Export2Excel.Size = New System.Drawing.Size(27, 27)
        Me.Button_Export2Excel.TabIndex = 18
        Me.Button_Export2Excel.Tag = "Export to Excel"
        Me.ToolTip_Excel.SetToolTip(Me.Button_Export2Excel, "Export all Deployment data to Excel.")
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
        'Button_CopySelected
        '
        Me.Button_CopySelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_CopySelected.Image = Global.WindowsApplication1.My.Resources.Resources.Edit_copy_green4
        Me.Button_CopySelected.Location = New System.Drawing.Point(1233, 65)
        Me.Button_CopySelected.Name = "Button_CopySelected"
        Me.Button_CopySelected.Size = New System.Drawing.Size(27, 27)
        Me.Button_CopySelected.TabIndex = 20
        Me.ToolTip_Copy.SetToolTip(Me.Button_CopySelected, "Copy selected deployment data to the clipboard.")
        Me.Button_CopySelected.UseVisualStyleBackColor = True
        '
        'CreateDeployments
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button_CopySelected)
        Me.Controls.Add(Me.Button_Copy)
        Me.Controls.Add(Me.Button_Export2Excel)
        Me.Controls.Add(Me.Button_Set)
        Me.Controls.Add(Me.CheckBox_CheckAll)
        Me.Controls.Add(Me.ListView_Deployments)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "CreateDeployments"
        Me.Size = New System.Drawing.Size(1343, 850)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox_SUGList As System.Windows.Forms.ComboBox
    Friend WithEvents ListView_Deployments As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button_GetDeployments As System.Windows.Forms.Button
    Friend WithEvents CheckBox_CheckAll As System.Windows.Forms.CheckBox
    Friend WithEvents Button_Set As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents GetMWDetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button_Copy As System.Windows.Forms.Button
    Friend WithEvents Button_Export2Excel As System.Windows.Forms.Button
    Friend WithEvents ToolTip_Excel As System.Windows.Forms.ToolTip
    Friend WithEvents ToolTip_Copy As System.Windows.Forms.ToolTip
    Friend WithEvents Button_CopySelected As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader

End Class
