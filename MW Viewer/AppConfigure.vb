Imports Microsoft.Win32

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the AppConfigure UI.  This is the form that is used to manage the configuration data that is stored in 
'  the registry.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class AppConfigure

    Dim Changed As Boolean = False

    Private Sub AppConfigure_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles loading the form.  If a registry value is missing it will populate the UI and the registry value with the default.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim MyRegPath As String = "SOFTWARE\AB Utilities\Patch Command Center"

        Dim registryKey As RegistryKey
        registryKey = Registry.CurrentUser

        Dim registrySubKey As RegistryKey

        ' Create the Keys in the registry if they don't already exist.
        registrySubKey = registryKey.OpenSubKey(MyRegPath, True)
        If registrySubKey Is Nothing Then
            registrySubKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\AB Utilities")
            registrySubKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\AB Utilities\Patch Command Center")
        End If

        Dim DefaultDomains() As String = {"LDAP://DC=icmasu,DC=icm", "LDAP://DC=int,DC=asurion,DC=com", "LDAP://DC=asurion,DC=org"}

        ' Check that each registry value contains data or populate it with the default.
        If registrySubKey.GetValue("CMServerFQDN") Is Nothing Then registrySubKey.SetValue("CMServerFQDN", "NDCINFCMMP701.int.asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("ChangeDirectoryPath") Is Nothing Then registrySubKey.SetValue("ChangeDirectoryPath", "\\nasappfp01\budfar\changeControl\Security Patching\", RegistryValueKind.String)
        If registrySubKey.GetValue("XMLFileName") Is Nothing Then registrySubKey.SetValue("XMLFileName", "MWData.xml", RegistryValueKind.String)
        If registrySubKey.GetValue("UpdatesDownloadPath") Is Nothing Then registrySubKey.SetValue("UpdatesDownloadPath", "\\NDCINFCMMP701\MS Packages\WindowsUpdates\", RegistryValueKind.String)
        If registrySubKey.GetValue("SMTPServer") Is Nothing Then registrySubKey.SetValue("SMTPServer", "GLBSMTP.int.asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("SMTPMailTo") Is Nothing Then registrySubKey.SetValue("SMTPMailTo", "IT-SCCMSupport@asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("SMTPMailFrom") Is Nothing Then registrySubKey.SetValue("SMTPMailFrom", "sccm@asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("LDAPPath") Is Nothing Then registrySubKey.SetValue("LDAPPath", "LDAP://DC=int,DC=asurion,DC=com", RegistryValueKind.String)
        If registrySubKey.GetValue("DomainNetBIOSName") Is Nothing Then registrySubKey.SetValue("DomainNetBIOSName", "HQDomain", RegistryValueKind.String)
        If registrySubKey.GetValue("BaseOU") Is Nothing Then registrySubKey.SetValue("BaseOU", "OU=Maintenance Windows,OU=Groups,OU=AsurionRoot,DC=int,DC=asurion,DC=com", RegistryValueKind.String)
        If registrySubKey.GetValue("DomainList") Is Nothing Then registrySubKey.SetValue("DomainList", DefaultDomains, RegistryValueKind.MultiString)

        ' Populate the UI fields.
        TextBox_CMServerFQDN.Text = registrySubKey.GetValue("CMServerFQDN")
        TextBox_ChangeDirectoryPath.Text = registrySubKey.GetValue("ChangeDirectoryPath")
        TextBox_XMLFileName.Text = registrySubKey.GetValue("XMLFileName")
        TextBox_UpdatesDownloadPath.Text = registrySubKey.GetValue("UpdatesDownloadPath")
        TextBox_SMTPServer.Text = registrySubKey.GetValue("SMTPServer")
        TextBox_SMTPMailTo.Text = registrySubKey.GetValue("SMTPMailTo")
        TextBox_SMTPMailFrom.Text = registrySubKey.GetValue("SMTPMailFrom")

        Dim DomainArry() As String = registrySubKey.GetValue("DomainList")

        ' Populate the Domains Listbox in the UI
        For Each Domain As String In DomainArry
            ListBox_DomainList.Items.Add(Domain)
        Next

        TextBox_LDAPPath.Text = registrySubKey.GetValue("LDAPPath")
        TextBox_DomainNetBIOSName.Text = registrySubKey.GetValue("DomainNetBIOSName")
        TextBox_BaseOU.Text = registrySubKey.GetValue("BaseOU")

        registrySubKey.Close()
        Button_Update.Enabled = False
    End Sub

    Private Sub Button_AddDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddDomain.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub adds a domain to the domainlist and then updates the UI and the registry value.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim NewDomain As String = TextBox_AddDomain.Text
        If NewDomain <> "" Then
            If Not ListBox_DomainList.Items.Contains(NewDomain) Then ListBox_DomainList.Items.Add(NewDomain)
            Changed = True
            Button_Update.Enabled = True
        End If
    End Sub

    Private Sub Button_RemoveDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RemoveDomain.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub removes the selected domain from the domainlist and then updates the UI and the registry value.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Domain As String = ListBox_DomainList.SelectedItem
        ListBox_DomainList.Items.Remove(Domain)
        Changed = True
        Button_Update.Enabled = True
    End Sub

    Private Sub Button_Update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Update.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles updating all of the registry values with the data from the UI fields when the 'Update' button is clicked.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If Changed = True Then
            Dim MyRegPath As String = "SOFTWARE\AB Utilities\Patch Command Center"

            Dim registryKey As RegistryKey
            registryKey = Registry.CurrentUser

            Dim registrySubKey As RegistryKey

            registrySubKey = registryKey.OpenSubKey(MyRegPath, True)
            If registrySubKey Is Nothing Then
                registrySubKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\AB Utilities")
                registrySubKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\AB Utilities\Patch Command Center")
            End If

            registrySubKey.SetValue("CMServerFQDN", TextBox_CMServerFQDN.Text, RegistryValueKind.String)
            registrySubKey.SetValue("ChangeDirectoryPath", TextBox_ChangeDirectoryPath.Text, RegistryValueKind.String)
            registrySubKey.SetValue("XMLFileName", TextBox_XMLFileName.Text, RegistryValueKind.String)
            registrySubKey.SetValue("UpdatesDownloadPath", TextBox_UpdatesDownloadPath.Text, RegistryValueKind.String)
            registrySubKey.SetValue("SMTPServer", TextBox_SMTPServer.Text, RegistryValueKind.String)
            registrySubKey.SetValue("SMTPMailTo", TextBox_SMTPMailTo.Text, RegistryValueKind.String)
            registrySubKey.SetValue("SMTPMailFrom", TextBox_SMTPMailFrom.Text, RegistryValueKind.String)

            Dim Count As Integer = ListBox_DomainList.Items.Count - 1
            Dim DomainArry(Count) As String
            Dim I As Integer

            For I = 0 To (Count)
                DomainArry(I) = ListBox_DomainList.Items(I).ToString
            Next
            registrySubKey.SetValue("DomainList", DomainArry, RegistryValueKind.MultiString)

            registrySubKey.SetValue("LDAPPath", TextBox_LDAPPath.Text, RegistryValueKind.String)
            registrySubKey.SetValue("DomainNetBIOSName", TextBox_DomainNetBIOSName.Text, RegistryValueKind.String)
            registrySubKey.SetValue("BaseOU", TextBox_BaseOU.Text, RegistryValueKind.String)

            registrySubKey.Close()

        End If
        Button_Update.Enabled = False
        Changed = False
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub closes this form.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Me.Close()
    End Sub

    Private Sub TextBox_CMServerFQDN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_CMServerFQDN.TextChanged, _
        TextBox_ChangeDirectoryPath.TextChanged,
        TextBox_XMLFileName.TextChanged,
        TextBox_UpdatesDownloadPath.TextChanged,
        TextBox_SMTPServer.TextChanged,
        TextBox_SMTPMailTo.TextChanged,
        TextBox_SMTPMailFrom.TextChanged,
        TextBox_LDAPPath.TextChanged,
        TextBox_DomainNetBIOSName.TextChanged,
        TextBox_BaseOU.TextChanged

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub keeps track of if any of the data fields have been edited in the UI.  If an edit is made the 'Update' button is enabled.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Changed = True
        Button_Update.Enabled = True
    End Sub

End Class