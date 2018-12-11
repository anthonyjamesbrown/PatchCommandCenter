<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MWSetterUC
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.CopyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Button_CalculateMW = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label_Timezone = New System.Windows.Forms.Label()
        Me.Label_TimeZoneLabel = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Button_Copy = New System.Windows.Forms.Button()
        Me.Button_Export2Excel = New System.Windows.Forms.Button()
        Me.Button_SetMWs = New System.Windows.Forms.Button()
        Me.CheckBox_SelectAll = New System.Windows.Forms.CheckBox()
        Me.TextBox_MWName = New System.Windows.Forms.TextBox()
        Me.Label_MW_Name = New System.Windows.Forms.Label()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddMWToAllCheckedCollectionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddCustomMWToCollectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.GetMWDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.bttn_AddMW = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripComboBox_MWType = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolTip_Excel = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTip_Copy = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CopyToolStripMenuItem1
        '
        Me.CopyToolStripMenuItem1.Name = "CopyToolStripMenuItem1"
        Me.CopyToolStripMenuItem1.Size = New System.Drawing.Size(257, 22)
        Me.CopyToolStripMenuItem1.Text = "Copy"
        '
        'Button_CalculateMW
        '
        Me.Button_CalculateMW.Location = New System.Drawing.Point(54, 9)
        Me.Button_CalculateMW.Name = "Button_CalculateMW"
        Me.Button_CalculateMW.Size = New System.Drawing.Size(117, 23)
        Me.Button_CalculateMW.TabIndex = 10
        Me.Button_CalculateMW.Text = "Calculate MWs"
        Me.Button_CalculateMW.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridView1.Size = New System.Drawing.Size(1342, 790)
        Me.DataGridView1.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage2.Controls.Add(Me.DataGridView1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1348, 796)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "MW XML Data"
        '
        'Label_Timezone
        '
        Me.Label_Timezone.AutoSize = True
        Me.Label_Timezone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Timezone.Location = New System.Drawing.Point(640, 14)
        Me.Label_Timezone.Name = "Label_Timezone"
        Me.Label_Timezone.Size = New System.Drawing.Size(0, 13)
        Me.Label_Timezone.TabIndex = 6
        '
        'Label_TimeZoneLabel
        '
        Me.Label_TimeZoneLabel.AutoSize = True
        Me.Label_TimeZoneLabel.Location = New System.Drawing.Point(549, 14)
        Me.Label_TimeZoneLabel.Name = "Label_TimeZoneLabel"
        Me.Label_TimeZoneLabel.Size = New System.Drawing.Size(85, 13)
        Me.Label_TimeZoneLabel.TabIndex = 5
        Me.Label_TimeZoneLabel.Text = "Local Timezone:"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(0, 28)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1356, 822)
        Me.TabControl1.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.AliceBlue
        Me.TabPage1.Controls.Add(Me.Button_Copy)
        Me.TabPage1.Controls.Add(Me.Button_Export2Excel)
        Me.TabPage1.Controls.Add(Me.Button_SetMWs)
        Me.TabPage1.Controls.Add(Me.CheckBox_SelectAll)
        Me.TabPage1.Controls.Add(Me.TextBox_MWName)
        Me.TabPage1.Controls.Add(Me.Label_MW_Name)
        Me.TabPage1.Controls.Add(Me.ListView1)
        Me.TabPage1.Controls.Add(Me.Button_CalculateMW)
        Me.TabPage1.Controls.Add(Me.Label_Timezone)
        Me.TabPage1.Controls.Add(Me.Label_TimeZoneLabel)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1348, 796)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "MW Set"
        '
        'Button_Copy
        '
        Me.Button_Copy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Copy.Image = Global.WindowsApplication1.My.Resources.Resources.Edit_copy_green4
        Me.Button_Copy.Location = New System.Drawing.Point(1279, 5)
        Me.Button_Copy.Name = "Button_Copy"
        Me.Button_Copy.Size = New System.Drawing.Size(27, 27)
        Me.Button_Copy.TabIndex = 17
        Me.ToolTip_Copy.SetToolTip(Me.Button_Copy, "Copy All MW data to the clipboard.")
        Me.Button_Copy.UseVisualStyleBackColor = True
        '
        'Button_Export2Excel
        '
        Me.Button_Export2Excel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Export2Excel.Image = Global.WindowsApplication1.My.Resources.Resources.Excel_icon
        Me.Button_Export2Excel.Location = New System.Drawing.Point(1312, 5)
        Me.Button_Export2Excel.Name = "Button_Export2Excel"
        Me.Button_Export2Excel.Size = New System.Drawing.Size(27, 27)
        Me.Button_Export2Excel.TabIndex = 16
        Me.Button_Export2Excel.Tag = "Export to Excel"
        Me.ToolTip_Excel.SetToolTip(Me.Button_Export2Excel, "Export MW data to Excel.")
        Me.Button_Export2Excel.UseVisualStyleBackColor = True
        '
        'Button_SetMWs
        '
        Me.Button_SetMWs.Location = New System.Drawing.Point(177, 9)
        Me.Button_SetMWs.Name = "Button_SetMWs"
        Me.Button_SetMWs.Size = New System.Drawing.Size(83, 23)
        Me.Button_SetMWs.TabIndex = 15
        Me.Button_SetMWs.Text = "Set Checked"
        Me.Button_SetMWs.UseVisualStyleBackColor = True
        '
        'CheckBox_SelectAll
        '
        Me.CheckBox_SelectAll.AutoSize = True
        Me.CheckBox_SelectAll.Location = New System.Drawing.Point(11, 15)
        Me.CheckBox_SelectAll.Name = "CheckBox_SelectAll"
        Me.CheckBox_SelectAll.Size = New System.Drawing.Size(37, 17)
        Me.CheckBox_SelectAll.TabIndex = 14
        Me.CheckBox_SelectAll.Text = "All"
        Me.CheckBox_SelectAll.UseVisualStyleBackColor = True
        '
        'TextBox_MWName
        '
        Me.TextBox_MWName.Location = New System.Drawing.Point(356, 11)
        Me.TextBox_MWName.Name = "TextBox_MWName"
        Me.TextBox_MWName.ReadOnly = True
        Me.TextBox_MWName.Size = New System.Drawing.Size(175, 20)
        Me.TextBox_MWName.TabIndex = 13
        '
        'Label_MW_Name
        '
        Me.Label_MW_Name.AutoSize = True
        Me.Label_MW_Name.Location = New System.Drawing.Point(281, 14)
        Me.Label_MW_Name.Name = "Label_MW_Name"
        Me.Label_MW_Name.Size = New System.Drawing.Size(69, 13)
        Me.Label_MW_Name.TabIndex = 12
        Me.Label_MW_Name.Text = "MW Naming:"
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.CheckBoxes = True
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11})
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(11, 38)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1329, 750)
        Me.ListView1.TabIndex = 11
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Collection Name"
        Me.ColumnHeader1.Width = 215
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Collection ID"
        Me.ColumnHeader2.Width = 75
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Day"
        Me.ColumnHeader3.Width = 75
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Local Start"
        Me.ColumnHeader4.Width = 140
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Local End"
        Me.ColumnHeader5.Width = 140
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "UTC Start"
        Me.ColumnHeader6.Width = 140
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "UTC End"
        Me.ColumnHeader7.Width = 140
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Duration"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "MW Set?"
        Me.ColumnHeader9.Width = 90
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Collection Comment"
        Me.ColumnHeader10.Width = 300
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Class"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddMWToAllCheckedCollectionsToolStripMenuItem, Me.ToolStripSeparator1, Me.AddCustomMWToCollectionToolStripMenuItem, Me.CopyToolStripMenuItem1, Me.ToolStripSeparator5, Me.GetMWDetailsToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(258, 104)
        '
        'AddMWToAllCheckedCollectionsToolStripMenuItem
        '
        Me.AddMWToAllCheckedCollectionsToolStripMenuItem.Name = "AddMWToAllCheckedCollectionsToolStripMenuItem"
        Me.AddMWToAllCheckedCollectionsToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.AddMWToAllCheckedCollectionsToolStripMenuItem.Text = "Add MW to all checked collections"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(254, 6)
        '
        'AddCustomMWToCollectionToolStripMenuItem
        '
        Me.AddCustomMWToCollectionToolStripMenuItem.Name = "AddCustomMWToCollectionToolStripMenuItem"
        Me.AddCustomMWToCollectionToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.AddCustomMWToCollectionToolStripMenuItem.Text = "Adjust MW Schedule"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(254, 6)
        '
        'GetMWDetailsToolStripMenuItem
        '
        Me.GetMWDetailsToolStripMenuItem.Name = "GetMWDetailsToolStripMenuItem"
        Me.GetMWDetailsToolStripMenuItem.Size = New System.Drawing.Size(257, 22)
        Me.GetMWDetailsToolStripMenuItem.Text = "Get MW Details"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator3, Me.ToolStripSeparator4, Me.bttn_AddMW, Me.ToolStripSeparator2, Me.ToolStripLabel1, Me.ToolStripComboBox_MWType})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1356, 25)
        Me.ToolStrip1.TabIndex = 8
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'bttn_AddMW
        '
        Me.bttn_AddMW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bttn_AddMW.Image = Global.WindowsApplication1.My.Resources.Resources.Plusicon
        Me.bttn_AddMW.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bttn_AddMW.Name = "bttn_AddMW"
        Me.bttn_AddMW.Size = New System.Drawing.Size(23, 22)
        Me.bttn_AddMW.Text = "btt"
        Me.bttn_AddMW.ToolTipText = "Add New MW"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(61, 22)
        Me.ToolStripLabel1.Text = "MW Type:"
        '
        'ToolStripComboBox_MWType
        '
        Me.ToolStripComboBox_MWType.Items.AddRange(New Object() {"Software Updates", "All Deployments"})
        Me.ToolStripComboBox_MWType.Name = "ToolStripComboBox_MWType"
        Me.ToolStripComboBox_MWType.Size = New System.Drawing.Size(121, 25)
        Me.ToolStripComboBox_MWType.Text = "Software Updates"
        '
        'ToolTip_Excel
        '
        Me.ToolTip_Excel.ToolTipTitle = "Export to Excel"
        '
        'ToolTip_Copy
        '
        Me.ToolTip_Copy.ToolTipTitle = "Copy"
        '
        'MWSetterUC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "MWSetterUC"
        Me.Size = New System.Drawing.Size(1356, 850)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CopyToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button_CalculateMW As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label_Timezone As System.Windows.Forms.Label
    Friend WithEvents Label_TimeZoneLabel As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Button_SetMWs As System.Windows.Forms.Button
    Friend WithEvents CheckBox_SelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_MWName As System.Windows.Forms.TextBox
    Friend WithEvents Label_MW_Name As System.Windows.Forms.Label
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddMWToAllCheckedCollectionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddCustomMWToCollectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bttn_AddMW As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripComboBox_MWType As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents GetMWDetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button_Copy As System.Windows.Forms.Button
    Friend WithEvents Button_Export2Excel As System.Windows.Forms.Button
    Friend WithEvents ToolTip_Excel As System.Windows.Forms.ToolTip
    Friend WithEvents ToolTip_Copy As System.Windows.Forms.ToolTip
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader

End Class
