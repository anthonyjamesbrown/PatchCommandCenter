<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AppConfigure
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
        Me.Button_RemoveDomain = New System.Windows.Forms.Button()
        Me.TextBox_BaseOU = New System.Windows.Forms.TextBox()
        Me.TextBox_DomainNetBIOSName = New System.Windows.Forms.TextBox()
        Me.TextBox_LDAPPath = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Button_AddDomain = New System.Windows.Forms.Button()
        Me.TextBox_AddDomain = New System.Windows.Forms.TextBox()
        Me.ListBox_DomainList = New System.Windows.Forms.ListBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TextBox_SMTPMailFrom = New System.Windows.Forms.TextBox()
        Me.TextBox_SMTPMailTo = New System.Windows.Forms.TextBox()
        Me.TextBox_SMTPServer = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TextBox_CMServerFQDN = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.TextBox_UpdatesDownloadPath = New System.Windows.Forms.TextBox()
        Me.TextBox_XMLFileName = New System.Windows.Forms.TextBox()
        Me.TextBox_ChangeDirectoryPath = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button_Update = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button_RemoveDomain)
        Me.GroupBox1.Controls.Add(Me.TextBox_BaseOU)
        Me.GroupBox1.Controls.Add(Me.TextBox_DomainNetBIOSName)
        Me.GroupBox1.Controls.Add(Me.TextBox_LDAPPath)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Button_AddDomain)
        Me.GroupBox1.Controls.Add(Me.TextBox_AddDomain)
        Me.GroupBox1.Controls.Add(Me.ListBox_DomainList)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 298)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(794, 238)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Active Directory"
        '
        'Button_RemoveDomain
        '
        Me.Button_RemoveDomain.Location = New System.Drawing.Point(161, 124)
        Me.Button_RemoveDomain.Name = "Button_RemoveDomain"
        Me.Button_RemoveDomain.Size = New System.Drawing.Size(56, 23)
        Me.Button_RemoveDomain.TabIndex = 10
        Me.Button_RemoveDomain.Text = "Remove"
        Me.Button_RemoveDomain.UseVisualStyleBackColor = True
        '
        'TextBox_BaseOU
        '
        Me.TextBox_BaseOU.Location = New System.Drawing.Point(223, 207)
        Me.TextBox_BaseOU.Name = "TextBox_BaseOU"
        Me.TextBox_BaseOU.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_BaseOU.TabIndex = 9
        '
        'TextBox_DomainNetBIOSName
        '
        Me.TextBox_DomainNetBIOSName.Location = New System.Drawing.Point(223, 180)
        Me.TextBox_DomainNetBIOSName.Name = "TextBox_DomainNetBIOSName"
        Me.TextBox_DomainNetBIOSName.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_DomainNetBIOSName.TabIndex = 8
        '
        'TextBox_LDAPPath
        '
        Me.TextBox_LDAPPath.Location = New System.Drawing.Point(223, 153)
        Me.TextBox_LDAPPath.Name = "TextBox_LDAPPath"
        Me.TextBox_LDAPPath.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_LDAPPath.TabIndex = 7
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(7, 210)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(125, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "MW Group OU Location:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 183)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(119, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Domain NetBIOS Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 156)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(102, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Domain LDAP Path:"
        '
        'Button_AddDomain
        '
        Me.Button_AddDomain.Location = New System.Drawing.Point(702, 24)
        Me.Button_AddDomain.Name = "Button_AddDomain"
        Me.Button_AddDomain.Size = New System.Drawing.Size(86, 23)
        Me.Button_AddDomain.TabIndex = 3
        Me.Button_AddDomain.Text = "Add"
        Me.Button_AddDomain.UseVisualStyleBackColor = True
        '
        'TextBox_AddDomain
        '
        Me.TextBox_AddDomain.Location = New System.Drawing.Point(223, 26)
        Me.TextBox_AddDomain.Name = "TextBox_AddDomain"
        Me.TextBox_AddDomain.Size = New System.Drawing.Size(473, 20)
        Me.TextBox_AddDomain.TabIndex = 2
        '
        'ListBox_DomainList
        '
        Me.ListBox_DomainList.FormattingEnabled = True
        Me.ListBox_DomainList.Location = New System.Drawing.Point(223, 52)
        Me.ListBox_DomainList.Name = "ListBox_DomainList"
        Me.ListBox_DomainList.Size = New System.Drawing.Size(565, 95)
        Me.ListBox_DomainList.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Domain List:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TextBox_SMTPMailFrom)
        Me.GroupBox2.Controls.Add(Me.TextBox_SMTPMailTo)
        Me.GroupBox2.Controls.Add(Me.TextBox_SMTPServer)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 192)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(794, 100)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "SMTP Settings"
        '
        'TextBox_SMTPMailFrom
        '
        Me.TextBox_SMTPMailFrom.Location = New System.Drawing.Point(223, 70)
        Me.TextBox_SMTPMailFrom.Name = "TextBox_SMTPMailFrom"
        Me.TextBox_SMTPMailFrom.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_SMTPMailFrom.TabIndex = 5
        '
        'TextBox_SMTPMailTo
        '
        Me.TextBox_SMTPMailTo.Location = New System.Drawing.Point(223, 44)
        Me.TextBox_SMTPMailTo.Name = "TextBox_SMTPMailTo"
        Me.TextBox_SMTPMailTo.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_SMTPMailTo.TabIndex = 4
        '
        'TextBox_SMTPServer
        '
        Me.TextBox_SMTPServer.Location = New System.Drawing.Point(223, 17)
        Me.TextBox_SMTPServer.Name = "TextBox_SMTPServer"
        Me.TextBox_SMTPServer.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_SMTPServer.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(55, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Mail From:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Mail To:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "SMTP Server:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TextBox_CMServerFQDN)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Location = New System.Drawing.Point(13, 13)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(794, 55)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Configuration Manager"
        '
        'TextBox_CMServerFQDN
        '
        Me.TextBox_CMServerFQDN.Location = New System.Drawing.Point(223, 22)
        Me.TextBox_CMServerFQDN.Name = "TextBox_CMServerFQDN"
        Me.TextBox_CMServerFQDN.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_CMServerFQDN.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(211, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Configuration Manager Site Server (FQDN):"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.TextBox_UpdatesDownloadPath)
        Me.GroupBox4.Controls.Add(Me.TextBox_XMLFileName)
        Me.GroupBox4.Controls.Add(Me.TextBox_ChangeDirectoryPath)
        Me.GroupBox4.Controls.Add(Me.Label4)
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Location = New System.Drawing.Point(13, 74)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(794, 112)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "General"
        '
        'TextBox_UpdatesDownloadPath
        '
        Me.TextBox_UpdatesDownloadPath.Location = New System.Drawing.Point(223, 80)
        Me.TextBox_UpdatesDownloadPath.Name = "TextBox_UpdatesDownloadPath"
        Me.TextBox_UpdatesDownloadPath.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_UpdatesDownloadPath.TabIndex = 5
        '
        'TextBox_XMLFileName
        '
        Me.TextBox_XMLFileName.Location = New System.Drawing.Point(223, 53)
        Me.TextBox_XMLFileName.Name = "TextBox_XMLFileName"
        Me.TextBox_XMLFileName.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_XMLFileName.TabIndex = 4
        '
        'TextBox_ChangeDirectoryPath
        '
        Me.TextBox_ChangeDirectoryPath.Location = New System.Drawing.Point(223, 23)
        Me.TextBox_ChangeDirectoryPath.Name = "TextBox_ChangeDirectoryPath"
        Me.TextBox_ChangeDirectoryPath.Size = New System.Drawing.Size(565, 20)
        Me.TextBox_ChangeDirectoryPath.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 84)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(126, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Updates Download Path:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(136, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Collections XML File Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(117, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Change Directory Path:"
        '
        'Button_Update
        '
        Me.Button_Update.Enabled = False
        Me.Button_Update.Location = New System.Drawing.Point(643, 542)
        Me.Button_Update.Name = "Button_Update"
        Me.Button_Update.Size = New System.Drawing.Size(75, 23)
        Me.Button_Update.TabIndex = 4
        Me.Button_Update.Text = "Update"
        Me.Button_Update.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(732, 542)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 5
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'AppConfigure
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(820, 578)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Update)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "AppConfigure"
        Me.Text = "Configuration"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_SMTPMailFrom As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_SMTPMailTo As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_SMTPServer As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_CMServerFQDN As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_UpdatesDownloadPath As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_XMLFileName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_ChangeDirectoryPath As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox_BaseOU As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_DomainNetBIOSName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_LDAPPath As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Button_AddDomain As System.Windows.Forms.Button
    Friend WithEvents TextBox_AddDomain As System.Windows.Forms.TextBox
    Friend WithEvents ListBox_DomainList As System.Windows.Forms.ListBox
    Friend WithEvents Button_Update As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Button_RemoveDomain As System.Windows.Forms.Button
End Class
