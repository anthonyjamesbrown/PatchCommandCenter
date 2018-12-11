Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports System.Text
Imports System.ComponentModel

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the Main form in this application.  This form contains a tab control with pages for each major
'  functionality.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class Main
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Declare Top Level Parameters
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public MWFunc As New MWFunctions()
    Public MWSet As New MWSetterUC()
    Public CreateDep As New CreateDeployments()
    Public AddSup As New CreateSup()
    Public Dists As New DistributionsUC()

    Dim DomainArray() As String
    Public sAdmin As String
    Private originalListItems As New List(Of ListViewItem)
    Public CurrentUser As String = My.User.Name
    Public CurrentUserRole As String
    Private Workers() As BackgroundWorker
    Private NumWorkers = 0

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Declare Column Indexices
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public ColServerDescription As Integer = 1
    Public ColOS As Integer = 2
    Public ColGroupDescription As Integer = 3
    Public ColMemberOf As Integer = 4
    Public ColDepartment As Integer = 5
    Public ColPingStatus As Integer = 6
    Public ColUptime As Integer = 7
    Public ColPasswordLastChanged As Integer = 8
    Public ColLastLogin As Integer = 9
    Public ColGroupCount As Integer = 10
    Public ColDomain As Integer = 11
    Public ColCount As Integer = 11


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles launching the Login form when the application starts.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        MWFunc.Initialize()
        sAdmin = CurrentUser
        UserRole_Label.Text = "User: " & sAdmin
        Dim iRole As Integer = 0
        If (sAdmin.ToUpper = "HQDOMAIN\!ANTHONY.BROWN" _
            Or sAdmin.ToUpper = "HQDOMAIN\!CHRIS.GAINES" _
            Or sAdmin.ToUpper = "HQDOMAIN\!CORY.PLASTEK" _
            Or sAdmin.ToUpper = "HQDOMAIN\_SWAPNIL.LAVALEKAR" _
            Or sAdmin.ToUpper = "HQDOMAIN\!PRAMOD.BADE" _
            Or sAdmin.ToUpper = "HQDOMAIN\_ALVIN.ALVARADO") Then
            iRole = 1
        End If
        'Dim User As New 
        If My.User.IsInRole("Asurion.org\PatchCmdCenter_Full_Admins") Or My.User.IsInRole("HQDOMAIN\PatchCmdCenter_Full_Admins") Then
            iRole = 1
        End If
        If (sAdmin.ToUpper = "HQDOMAIN\!SEAN.GRIFFIN" _
            Or sAdmin.ToUpper = "HQDOMAIN\!BABULA.PATRO") Then
            iRole = 2
        End If

        Select Case iRole
            Case 1
                CurrentUserRole = "Admin"
            Case 2
                CurrentUserRole = "Citrix"

                ' If User is not in the Admin role, remove all of the CM tabs from the UI
                TabControl1.TabPages.Remove(TabPage2)
                TabControl1.TabPages.Remove(TabPage3)
                TabControl1.TabPages.Remove(TabPage4)
                TabControl1.TabPages.Remove(TabPage5)

                ' Disable menu items
                NonProdToolStripMenuItem.Enabled = False
                PRODToolStripMenuItem.Enabled = False
                SetNoMWToolStripMenuItem1.Enabled = False
                MWNewBuildsToolStripMenuItem1.Enabled = False
            Case Else
                CurrentUserRole = "User"

                ' Disable menu items
                SetChangeMWGroupToolStripMenuItem.Enabled = False

                ' If User is not in the Admin role, remove all of the CM tabs from the UI
                TabControl1.TabPages.Remove(TabPage2)
                TabControl1.TabPages.Remove(TabPage3)
                TabControl1.TabPages.Remove(TabPage4)
                TabControl1.TabPages.Remove(TabPage5)
        End Select

        Role_Label.Text = "Access Level: " & CurrentUserRole
        RemoveAJKK_Button.Enabled = False

        ' This next section is used to dynamically add Options to the Move MW Group context menu by looping through the collections data xml file.
        ' Grab a copy of the xml dataset as a local dataview object.
        Dim Collections As DataView = Me.MWFunc.dataSet.Tables(0).DefaultView
        Collections.Sort = "CollectionName" ' Sort the dataview so that the entries added to the context menu will be in alpha order.

        ' Loop through the dataview and classify each item by Prod Auto, Prod Manual, Non-Prod, or Citrix and add them to the correct place in the context menu.
        '  also add an envent handler for each new item.
        For Each DR As DataRowView In Collections
            Dim CollectionName As String = DR("CollectionName").ToString
            Dim CClass As String = DR("Class").ToString
            Dim Reboot As Boolean = CollectionName.Contains("Auto")
            Dim Citrix As Boolean = CollectionName.Contains("Citrix")
            If CClass = "Non-Prod" And Citrix = False Then
                Dim Item As ToolStripItem = NonProdToolStripMenuItem.DropDownItems.Add(CollectionName)
                AddHandler Item.Click, AddressOf AnyToolStripMenuItem_Click
            End If
            If CClass = "Prod" And Citrix = False Then
                If Reboot Then
                    Dim Item As ToolStripItem = RebootToolStripMenuItem.DropDownItems.Add(CollectionName)
                    AddHandler Item.Click, AddressOf AnyToolStripMenuItem_Click
                Else
                    Dim Item As ToolStripItem = NoRebootToolStripMenuItem.DropDownItems.Add(CollectionName)
                    AddHandler Item.Click, AddressOf AnyToolStripMenuItem_Click
                End If
            End If
            If Citrix Then
                Dim Item As ToolStripItem = CitrixManualRebootToolStripMenuItem.DropDownItems.Add(CollectionName)
                AddHandler Item.Click, AddressOf AnyToolStripMenuItem_Click
            End If
        Next

    End Sub

    Private Sub bttnQueryAD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnQueryAD.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the 'Query AD' button action.  This routine queries AD and populates the main list view with data.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        DomainArray = Me.MWFunc.GetDomainList()

        RecordLabel.Text = "Working..." ' Set the status label to show that the query is running.
        bttnQueryAD.Enabled = False     ' Gray the 'Query AD' button out to prevent additional presses while executing. 
        Me.Refresh()

        'Grab todays date and subtract 45 days from it, then convert that into file time for use with an AD Filter
        Dim PwdDate2 As DateTime = (DateAdd(DateInterval.Day, -45, Now()))
        Dim Stamp As Long = PwdDate2.ToFileTime()

        'Clear any items already in the list view and reset the sorting flag.
        ListView_MW.Items.Clear()
        ListView_MW.Sorting = SortOrder.None

        For Each DomainPath As String In DomainArray
            'Define varables to hold all of the data brought back from the AD Query
            Dim myDirectory As New DirectoryEntry(DomainPath)
            Dim mySearchResultColl As SearchResultCollection
            Dim mySearchResult As SearchResult
            Dim myResultPropColl As ResultPropertyCollection
            Dim vCN As ResultPropertyValueCollection
            Dim vsAMAccountName As ResultPropertyValueCollection
            Dim vpwdLastSet As ResultPropertyValueCollection
            Dim vlastLogonTimestamp As ResultPropertyValueCollection
            Dim vOS As ResultPropertyValueCollection
            Dim vMemberOf As ResultPropertyValueCollection
            Dim vServerDescription As ResultPropertyValueCollection
            Dim vDepartment As ResultPropertyValueCollection
            Dim search As New DirectorySearcher(myDirectory)

            'The search filter returns all computer objects that have 'server' in the OS name that has a lastlogontimestamp in the last 45 days.
            Dim myFilter As String
            myFilter = "(&(objectClass=computer)(operatingsystem=*server*)(pwdlastSet>=" & Stamp & "))"

            search.Filter = myFilter
            search.PageSize = 6000 'This property limits the number of objects returned by the query.  May need to be increased at some point.
            search.PropertiesToLoad.Add("cn")
            search.PropertiesToLoad.Add("sAMAccountName")
            search.PropertiesToLoad.Add("pwdLastSet")
            search.PropertiesToLoad.Add("lastLogonTimestamp")
            search.PropertiesToLoad.Add("operatingsystem")
            search.PropertiesToLoad.Add("memberOf")
            search.PropertiesToLoad.Add("description")
            search.PropertiesToLoad.Add("department")

            mySearchResultColl = search.FindAll() 'Execute the query

            'This for loop, interates through the results returned by the AD query and populates the ListView on the interface.
            For I = 0 To mySearchResultColl.Count - 1
                mySearchResult = mySearchResultColl.Item(I)
                myResultPropColl = mySearchResult.Properties
                vCN = myResultPropColl.Item("cn")
                vsAMAccountName = myResultPropColl.Item("sAMAccountName")
                vpwdLastSet = myResultPropColl.Item("pwdLastSet")
                vlastLogonTimestamp = myResultPropColl.Item("lastLogonTimestamp")
                vOS = myResultPropColl.Item("operatingsystem")
                vMemberOf = myResultPropColl.Item("memberOf")
                vServerDescription = myResultPropColl.Item("description")
                vDepartment = myResultPropColl.Item("department")

                'Create a temp ListViewItem, set all of its properties and then finally add the item to the main ListView.
                Dim TempItem As New ListViewItem()
                Dim x As Integer
                TempItem.Text = vCN.Item(0).ToString
                For x = 1 To ColCount
                    TempItem.SubItems.Add("")
                Next
                'Check each varable to see if it has a value, if it does populate the tempitem with it, if not populate the temp item with an empty string.
                ' Repeat for each varable.
                If vServerDescription.Count() > 0 Then
                    TempItem.SubItems(ColServerDescription).Text = vServerDescription.Item(0).ToString
                End If

                If vpwdLastSet.Count() > 0 Then
                    Dim PwdDate As DateTime
                    Dim pwdvalue As Long = vpwdLastSet.Item(0).ToString
                    PwdDate = DateTime.FromFileTimeUtc(pwdvalue) 'Covert AD Timestamp to something human readable
                    TempItem.SubItems(ColPasswordLastChanged).Text = PwdDate
                End If

                If vlastLogonTimestamp.Count() > 0 Then
                    Dim PwdDate As DateTime
                    Dim logonvalue As Long = vlastLogonTimestamp.Item(0).ToString
                    PwdDate = DateTime.FromFileTimeUtc(logonvalue) 'Covert AD Timestamp to something human readable
                    TempItem.SubItems(ColLastLogin).Text = PwdDate
                End If

                If vOS.Count() > 0 Then
                    TempItem.SubItems(ColOS).Text = vOS.Item(0).ToString
                End If

                If vDepartment.Count() > 0 Then
                    TempItem.SubItems(ColDepartment).Text = vDepartment.Item(0).ToString
                End If

                'This section handles formatting the memberOf attribute as well as discarding all groups not pre-fixed with 'MW'.
                ' This section also queries AD for the group description for each group where a server is only a member of one MW group.
                If vMemberOf.Count() > 0 Then
                    Dim Groups As String = ""
                    Dim GroupName As String = ""
                    Dim GroupArray() As String
                    Dim GroupPrefix As String
                    Dim First As Boolean = True
                    Dim GroupCount As Integer = 0

                    Dim j As Integer
                    For j = 0 To vMemberOf.Count - 1
                        GroupArray = vMemberOf.Item(j).ToString.Split(",")
                        GroupName = GroupArray(0).Substring(3, GroupArray(0).Length() - 3)
                        GroupPrefix = GroupName.Substring(0, 2)
                        If GroupPrefix = "MW" Then
                            If First Then
                                Groups = GroupName
                                First = False
                            Else
                                Groups = Groups & "|" & GroupName
                            End If
                            GroupCount += 1
                        End If
                    Next
                    TempItem.SubItems(ColGroupCount).Text = GroupCount
                    TempItem.SubItems(ColMemberOf).Text = Groups
                    If GroupCount = 1 Then
                        TempItem.SubItems(ColGroupDescription).Text = MWFunc.HandleGroupDescription(Groups, DomainPath)
                    End If
                Else
                    TempItem.SubItems(ColGroupCount).Text = "0"
                End If

                TempItem.SubItems(ColDomain).Text = DomainPath
                ListView_MW.Items.Add(TempItem) ' Finally adding the Temp ListView Item to the main ListView.
            Next
            myDirectory.Close() 'Clean up AD connection
        Next
        ColorGroupCounts()  'Color any rows that have more than one MW group memberships.
        RecordLabel.Text = ListView_MW.Items.Count & " Servers Returned."
        bttnQueryAD.Enabled = True 'Reenable the 'Query AD' button.
        ListView_MW.Sort()
        CreateCacheSet() 'Create a cache of all of the data displayed in the ListView.  This will be used for refreshing the ListView between filters.
        RemoveAJKK_Button.Enabled = True

    End Sub

    Private Sub ColorGroupCounts()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles looping through the ListView_MW and highlighting rows that have more than 1 MW group memberships.  The Backcolor 
        '  for the rows is changed to RED.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim X As Integer
        Dim Value As String
        For X = 0 To ListView_MW.Items.Count - 1
            Value = ListView_MW.Items(X).SubItems(ColMemberOf).Text
            If Value <> Nothing Then
                If CDbl(ListView_MW.Items(X).SubItems(ColGroupCount).Text) > 1 Then ListView_MW.Items(X).BackColor = Color.Red
            End If
        Next
        ListView_MW.Update()
    End Sub

    Private Sub CreateCacheSet()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles caching a copy of the current ListView.  This cache is used to repoplulate the listview after filters changes are applied.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        originalListItems.Clear()
        Dim X As Integer
        For X = 0 To ListView_MW.Items.Count - 1
            originalListItems.Add(ListView_MW.Items(X))
        Next
    End Sub

    Private Sub txtFilter_TextChanged2(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilter.TextChanged, CheckBox_HideServers.CheckedChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles rebuilding the listview_MW based on text typed into the txtFilter text box.  As text is typed in this sub will 
        '  clear the items in the listview and then go through the cached copy and compare fields 1,2,7, and 8, if any matches are found in any
        '  of those fields then the row is included in the listview.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Hide_Check As Boolean = CheckBox_HideServers.Checked
        Dim Filter_Text As String = txtFilter.Text.ToUpper
        ListView_MW.BeginUpdate()
        ListView_MW.Sorting = SortOrder.None
        ListView_MW.Items.Clear()
        If Filter_Text.Trim().Length = 0 And Hide_Check = False Then
            For Each item In originalListItems
                ListView_MW.Items.Add(item)
            Next
        Else
            If Filter_Text.Trim().Length = 0 And Hide_Check = True Then
                For Each item In originalListItems 'Searching in originalListItems which is the cached list created by the CreateCacheSet in the beginning.
                    If item.SubItems(ColMemberOf).Text.ToUpper = "" Then
                        ListView_MW.Items.Add(item)
                    End If
                Next
            Else
                If Filter_Text.Trim().Length > 0 And Hide_Check = False Then
                    ListView_MW.Items.Clear()
                    For Each item In originalListItems 'Searching in originalListItems which is the cached list created by the CreateCacheSet in the beginning.
                        ' The if statement is looking at field indexes 0, 1, 2, 7, and 8 to match the filter text against.  This maps to columns ServerName, Server Description, Division, MemberOf, and Group Description.
                        If item.Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColServerDescription).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColOS).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColMemberOf).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColGroupDescription).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColDepartment).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColDomain).Text.ToUpper.Contains(Filter_Text) Then
                            ListView_MW.Items.Add(item)
                        End If
                    Next
                Else
                    ListView_MW.Items.Clear()
                    For Each item In originalListItems 'Searching in originalListItems which is the cached list created by the CreateCacheSet in the beginning.
                        ' The if statement is looking at field indexes 0, 1, 2, 7, and 8 to match the filter text against.  This maps to columns ServerName, Server Description, Division, MemberOf, and Group Description.
                        If (item.Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColServerDescription).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColOS).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColMemberOf).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColGroupDescription).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColDepartment).Text.ToUpper.Contains(Filter_Text) _
                            Or item.SubItems(ColDomain).Text.ToUpper.Contains(Filter_Text)
                            ) And item.SubItems(ColMemberOf).Text.ToUpper = "" Then
                            ListView_MW.Items.Add(item)
                        End If
                    Next
                End If
            End If
        End If
        ListView_MW.EndUpdate()
        RecordLabel.Text = ListView_MW.Items.Count & " Servers Returned."
        SelectedLabel.Text = ""
    End Sub

    Private Sub ListView_MW_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles ListView_MW.ItemSelectionChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub simply updates the status label for how many items are selected.  This action fires everytime the Item selection in 
        '  the main ListView is changed.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        SelectedLabel.Text = ListView_MW.SelectedItems.Count & " Servers currently Selected."
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Copy.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles copying ALL the data in the main ListView to the Clipboard including the Header names.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim buffer As New StringBuilder
        For i As Integer = 0 To ListView_MW.Columns.Count - 1
            buffer.Append(ListView_MW.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_MW.Items.Count - 1
            For j As Integer = 0 To ListView_MW.Columns.Count - 1
                buffer.Append(ListView_MW.Items(i).SubItems(j).Text)
                buffer.Append(vbTab)
            Next
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Sub CopySelectedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem1.Click, Button_CopySelected.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles copying the SELECTED data in the main ListView to the Clipboard including the Header names.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim buffer As New StringBuilder
        For i As Integer = 0 To ListView_MW.Columns.Count - 1
            buffer.Append(ListView_MW.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_MW.SelectedItems.Count - 1
            For j As Integer = 0 To ListView_MW.Columns.Count - 1
                buffer.Append(ListView_MW.SelectedItems(i).SubItems(j).Text)
                buffer.Append(vbTab)
            Next
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Sub CopyHostnameOnlyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyHostnamesOnlyToolStripMenuItem.Click, Button_CopyServer.Click
        Dim buffer As New StringBuilder
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_MW.SelectedItems.Count - 1
            buffer.Append(ListView_MW.SelectedItems(i).Text)
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Function RemoveCurrentGroups(ByVal ServerName As String, ByVal CurrGroups As String, ByVal LDAPPath As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is used to remove a server AD object from a group.  In this case it would be the MW group.  This function is only 
        '  called when updating which MW group a server is a member of since servers should only be a member of one MW group.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            Dim SAMAccountName As String = MWFunc.GetADSAMAccountName(ServerName, LDAPPath)
            Dim I As Integer
            Dim GroupArray() As String
            GroupArray = CurrGroups.Split("|")
            For I = 0 To GroupArray.Count - 1
                Dim GroupName As String = GroupArray(I)
                Dim DNSName As String = MWFunc.ConvertLDAP2DNS(LDAPPath)
                Dim nbDN As String = MWFunc.GetNetbiosDomainName(DNSName)
                Dim pc As PrincipalContext = New PrincipalContext(ContextType.Domain, nbDN)
                Dim Group As GroupPrincipal = GroupPrincipal.FindByIdentity(pc, GroupName)
                Group.Members.Remove(pc, IdentityType.SamAccountName, SAMAccountName)
                Group.Save()
            Next
            Return True
        Catch e As Exception
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function Add2Group(ByVal ServerName As String, ByVal GroupName As String, ByVal LDAPPath As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub adds a given server to a given group.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            Dim SAMAccountName As String = MWFunc.GetADSAMAccountName(ServerName, LDAPPath)

            Dim DNSName As String = MWFunc.ConvertLDAP2DNS(LDAPPath)
            Dim nbDN As String = MWFunc.GetNetbiosDomainName(DNSName)

            Dim pc As PrincipalContext = New PrincipalContext(ContextType.Domain, nbDN)
            Dim Group As GroupPrincipal = GroupPrincipal.FindByIdentity(pc, GroupName)
            Group.Members.Add(pc, IdentityType.SamAccountName, SAMAccountName)
            Group.Save()
            Return True
        Catch e As Exception
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Function UpdateCache(ByVal ServerName As String, ByVal Index As Integer, ByVal Value As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to find and update a value based on field index in the saved cache.  This is required to keep the cache
        '  consistant with changes made to the listview when changing server description or group membership.  The cache has to be kept
        '  current so when changes are made to the text filter and the listview is repopulated from cache all of the changes that have
        '  occured since the application started are still reflected in the listview after refresh.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Index2 As Integer = originalListItems.FindIndex(Function(value2 As ListViewItem) value2.Text = ServerName)
        originalListItems(Index2).SubItems(Index).Text = Value
        Return Index2
    End Function

    Private Sub UpdateGroup(ByVal GroupName As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles all actions required to update a group.  This includes cleaning up the previous group membership if needed as
        '  well as updating the Division Field, Adding the New group and updating the cache for both field changes.
        '  This function will work even when multiple servers are selected, it just loops through the list.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer
        Dim ServerName As String
        Dim CurrGroups As String
        Dim LDAPPath As String
        If ListView_MW.SelectedItems.Count > 0 Then 'Make sure at least 1 server was selected
            For I = 0 To ListView_MW.SelectedItems.Count - 1 'Loop through the selected servers list.
                ServerName = ListView_MW.SelectedItems.Item(I).Text
                CurrGroups = ListView_MW.SelectedItems.Item(I).SubItems(ColMemberOf).Text
                LDAPPath = ListView_MW.SelectedItems.Item(I).SubItems(ColDomain).Text
                If Not ServerName Is Nothing Then
                    Dim Prompt As Boolean = PromptStatus.CheckState
                    Dim Result
                    If Prompt Then
                        'Prompt user to change the <groupname> on <servername>.  If yes proceed.
                        Result = MessageBox.Show("Servername: " & ServerName & " GroupName: " & CurrGroups & vbCrLf & "and added to " & GroupName, "Confirm", MessageBoxButtons.YesNo)
                    Else
                        Result = True
                    End If
                    If Result Then
                        If Not (CurrGroups Is Nothing Or CurrGroups = "") Then
                            RemoveCurrentGroups(ServerName, CurrGroups, LDAPPath) 'If the group is not null or and empty string then remove the server from it.
                        End If
                        If Add2Group(ServerName, GroupName, LDAPPath) Then 'Add server to new AD group.
                            ListView_MW.SelectedItems(I).SubItems(ColGroupCount).Text = "1" 'Update group count to 1 in UI.
                            ListView_MW.SelectedItems(I).SubItems(ColMemberOf).Text = GroupName 'Update memberof field in UI.
                            ListView_MW.SelectedItems(I).SubItems(ColGroupDescription).Text = MWFunc.HandleGroupDescription(GroupName, LDAPPath) 'Update group description in UI.
                            UpdateCache(ServerName, ColGroupCount, "1") 'Update group count to 1 in Cache.
                            UpdateCache(ServerName, ColMemberOf, GroupName) 'Update memberof field in Cache.
                            UpdateCache(ServerName, ColGroupDescription, MWFunc.HandleGroupDescription(GroupName, LDAPPath)) 'Update group description in Cache.
                            ListView_MW.SelectedItems(I).BackColor = Color.White 'Update back ground color for the row in UI.
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub UpdateServerDescriptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateServerDescriptionToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the action that is executed when the 'Update Server Description' option is selected from the context menu.
        '  A Text Box will be displayed and the AD value for Description will be updated as well as the Listview_MW item and the cache as well.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer
        Dim ServerName As String
        Dim LDAPPath As String
        Dim CurrentValue As String = ListView_MW.SelectedItems(0).SubItems(ColServerDescription).Text
        Dim NewValue = InputBox("Server Description", "Server Description", CurrentValue)
        If ListView_MW.SelectedItems.Count > 0 Then 'Make sure at least 1 server was selected
            For I = 0 To ListView_MW.SelectedItems.Count - 1 'Loop through the selected servers list.
                ServerName = ListView_MW.SelectedItems.Item(I).Text
                LDAPPath = ListView_MW.SelectedItems.Item(I).SubItems(ColDomain).Text
                If Not ServerName Is Nothing Then
                    If NewValue = "" Then NewValue = CurrentValue
                    If NewValue <> CurrentValue Then
                        If MWFunc.UpdateServerDescription(ServerName, NewValue, LDAPPath) Then
                            ListView_MW.SelectedItems(I).SubItems(ColServerDescription).Text = NewValue
                            UpdateCache(ServerName, ColServerDescription, NewValue)
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub UpdateServerDepartmentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateServerDepartmentToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the action that is executed when the 'Update Server Department' option is selected from the context menu.
        '  A Text Box will be displayed and the AD value for Department will be updated as well as the Listview_MW item and the cache as well.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer
        Dim ServerName As String
        Dim LDAPPath As String
        Dim CurrentValue As String = ListView_MW.SelectedItems(0).SubItems(ColDepartment).Text
        Dim NewValue = InputBox("Server Department", "Server Department", CurrentValue)
        If ListView_MW.SelectedItems.Count > 0 Then 'Make sure at least 1 server was selected
            For I = 0 To ListView_MW.SelectedItems.Count - 1 'Loop through the selected servers list.
                ServerName = ListView_MW.SelectedItems.Item(I).Text
                LDAPPath = ListView_MW.SelectedItems.Item(I).SubItems(ColDomain).Text
                If Not ServerName Is Nothing Then
                    If NewValue = "" Then NewValue = CurrentValue
                    If NewValue <> CurrentValue Then
                        If MWFunc.UpdateServerDepartment(ServerName, NewValue, LDAPPath) Then
                            ListView_MW.SelectedItems(I).SubItems(ColDepartment).Text = NewValue
                            UpdateCache(ServerName, ColDepartment, NewValue)
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub GetUptimeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetUptimeToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the 'Get Uptime' option from the context menu.  This sub will loop through the selected servers from the ListView.
        '  It will update the UI with a 'Working...' message and then will create a seperate background worker to get the uptime for each server.
        '  This sub will also perform a ping before spinning it out to the uptime thread.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer
        Dim ServerName As String
        If ListView_MW.SelectedItems.Count > 0 Then 'Make sure at least 1 server was selected.
            For I = 0 To ListView_MW.SelectedItems.Count - 1 'Loop through the selected servers list.
                ServerName = ListView_MW.SelectedItems.Item(I).Text
                ListView_MW.SelectedItems(I).SubItems(ColUptime).Text = "Working..."
                Dim Ping = MWFunc.PingHost(ServerName)
                ListView_MW.SelectedItems(I).SubItems(ColPingStatus).Text = Ping
                ListView_MW.SelectedItems(I).UseItemStyleForSubItems = False
                If Ping Then
                    ListView_MW.SelectedItems(I).SubItems(ColPingStatus).BackColor = Color.Green
                Else
                    ListView_MW.SelectedItems(I).SubItems(ColPingStatus).BackColor = Color.Red
                End If
                UpdateCache(ServerName, ColPingStatus, Ping) 'Update Cache with ping status.
                NumWorkers = NumWorkers + 1 'This is the index trackers for the number of BackgroundWorker threads in the Workers thread array.
                ReDim Workers(NumWorkers) 'Add a worker thread entry.
                Workers(NumWorkers) = New BackgroundWorker 'Create the new thread.
                Workers(NumWorkers).WorkerReportsProgress = True 'Set the report progress property.
                Workers(NumWorkers).WorkerSupportsCancellation = True 'Set the supports cancellation property.
                AddHandler Workers(NumWorkers).DoWork, AddressOf WorkerDoWork 'Create a reference for the dowork event for this worker.
                AddHandler Workers(NumWorkers).RunWorkerCompleted, AddressOf WorkerCompleted ' Create a reference for the work completed event.
                Workers(NumWorkers).RunWorkerAsync(ServerName) 'Run the thread async.
            Next
            ListView_MW.Refresh()
        End If
    End Sub

    Private Sub WorkerDoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is the work completed by the BackgroundWorker threads for getting system uptime.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim ServerName As String
        ServerName = e.Argument.ToString 'Get the servername out of the passed Argument object.
        Try
            Dim TS As TimeSpan = MWFunc.GetUptime(ServerName) 'Attempt to get the server uptime.
            e.Result = ServerName & "|" & TS.ToString
        Catch
            e.Result = ServerName & "|Access Denied" 'If the GetUptime function throws an error it will be due to access.
        End Try
    End Sub

    Private Sub WorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is run as an Event Handler for when the server uptime backgroundworker completes.  This sub will handle updating the cache
        '  and UI with the uptime results.  This sub will call Update_ListViewUptime sub to update the UI.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim ServerName As String = (e.Result.ToString.Split("|"))(0)
        Dim TSResult As String = (e.Result.ToString.Split("|"))(1)
        UpdateCache(ServerName, ColUptime, TSResult)
        Update_ListViewUptime(ServerName, TSResult)
    End Sub

    Private Sub Update_ListViewUptime(ByVal ServerName, ByVal TSResult)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is called by the thread completion event for getting server uptime.  This sub handles updating the UI with the uptime results.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If ListView_MW.Items.Count > 0 Then
            Dim Index2 As Integer = (ListView_MW.FindItemWithText(ServerName, False, 0)).Index
            ListView_MW.Items(Index2).SubItems(ColUptime).Text = TSResult
        End If
    End Sub

    Private Sub PingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PingToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the ping option from the context menu.  It will loop through the selected servers and perform a ping operation and 
        '  then update the UI with the results.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer
        Dim ServerName As String
        If ListView_MW.SelectedItems.Count > 0 Then 'Make sure at least 1 server was selected
            For I = 0 To ListView_MW.SelectedItems.Count - 1 'Loop through the selected servers list.                
                ServerName = ListView_MW.SelectedItems.Item(I).Text
                Dim Ping = MWFunc.PingHost(ServerName) 'Ping the server
                ListView_MW.SelectedItems(I).SubItems(ColPingStatus).Text = Ping 'Update the UI.
                ListView_MW.SelectedItems(I).UseItemStyleForSubItems = False ' Allow coloring just the ping field backcolor rather than the whole line.
                If Ping Then
                    ListView_MW.SelectedItems(I).SubItems(ColPingStatus).BackColor = Color.Green
                Else
                    ListView_MW.SelectedItems(I).SubItems(ColPingStatus).BackColor = Color.Red
                End If
                ListView_MW.Update()
                UpdateCache(ServerName, ColPingStatus, Ping) 'Update the cache.
            Next
        End If
    End Sub

    Private Sub SetNoMWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetNoMWToolStripMenuItem1.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the Set NO MW option in the context menu.  This will loop through all of the selected servers and will remove them
        '  from any current MW_ group memberships.  It will then blank out the Division attribute in AD.  It will also update the cache.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer
        Dim ServerName As String
        Dim CurrGroups As String
        Dim LDAPPath As String
        If ListView_MW.SelectedItems.Count > 0 Then 'Make sure at least 1 server was selected.
            For I = 0 To ListView_MW.SelectedItems.Count - 1 'Loop through the selected servers list.
                ServerName = ListView_MW.SelectedItems.Item(I).Text
                CurrGroups = ListView_MW.SelectedItems.Item(I).SubItems(ColMemberOf).Text
                LDAPPath = ListView_MW.SelectedItems.Item(I).SubItems(ColDomain).Text
                If Not ServerName Is Nothing Then
                    Dim Prompt As Boolean = PromptStatus.CheckState
                    Dim Result
                    If Prompt Then
                        'Prompt user to change the <groupname> on <servername>.  If yes proceed.
                        Result = MessageBox.Show("Remove Servername: " & ServerName & "from GroupName: " & CurrGroups & vbCrLf, "Confirm", MessageBoxButtons.YesNo)
                    Else
                        Result = True
                    End If
                    If Result Then
                        If Not (CurrGroups Is Nothing Or CurrGroups = "") Then
                            If RemoveCurrentGroups(ServerName, CurrGroups, LDAPPath) Then 'If the group is not null or and empty string then remove the server from it.
                                ListView_MW.SelectedItems(I).SubItems(ColGroupCount).Text = "0" 'Update group count to 1 in UI.
                                ListView_MW.SelectedItems(I).SubItems(ColMemberOf).Text = "" 'Update memberof field in UI.
                                ListView_MW.SelectedItems(I).SubItems(ColGroupDescription).Text = "" 'Update group description in UI.
                                UpdateCache(ServerName, ColGroupCount, "0") 'Update group count to 1 in Cache.
                                UpdateCache(ServerName, ColMemberOf, "") 'Update memberof field in Cache.
                                UpdateCache(ServerName, ColGroupDescription, "") 'Update group description in Cache.
                                ListView_MW.SelectedItems(I).BackColor = Color.White 'Update back ground color for the row in UI.
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveAJKK_Button.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub will remove the AJKK servers from the Listview and then drop and recreate the cache when the 'Remove AJKK' button is pushed.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ListView_MW.BeginUpdate()
        ListView_MW.Items.Clear() 'Clear the current Listview contents.
        ListView_MW.Sorting = SortOrder.None
        For Each item In originalListItems 'Searching in originalListItems which is the cached list created by the CreateCacheSet in the beginning.
            'Re-add everything from the current cache back into the Listview but filter out servers that start with AJKK prefixes.
            If Not (item.Text.ToUpper.Contains("JPK") Or item.Text.ToUpper.Contains("JP2") Or item.Text.ToUpper.Contains("JPC")) Then
                ListView_MW.Items.Add(item)
            End If
        Next
        CreateCacheSet() 'Recreate the cache set.  This will clear the cache and then copy the current contents of the Listview into the cache
        ListView_MW.EndUpdate()
        RecordLabel.Text = ListView_MW.Items.Count & " Servers Returned."
        RemoveAJKK_Button.Enabled = False 'Once this operation is done disable the button.
    End Sub

    Private Sub GetHotfixesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetHotfixesToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub Launches the HotfixDialog form.  The HotfixDialog form connects to the first server that was selected from the ListView
        '  and pulls a list of all applied patches using a WMI query.  See the comments in the HotfixDialog.vb for more info.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim PatchForm As New HotfixDialog
        PatchForm.Show()
    End Sub

    Private Sub GetUpdateStatusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetUpdateStatusToolStripMenuItem.Click
        Dim UpdateStatus As New UpdateStates
        UpdateStatus.ServerName = ListView_MW.SelectedItems(0).Text
        UpdateStatus.Show()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub closes the application when 'Exit' option is clicked from the menu strip.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub calls the 'About' screen when that option is clicked from the menu strip.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim About As New AboutBox1
        About.Show()
    End Sub

    Private Sub Button_Export2Excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Export2Excel.Click
        Dim ExcelExport As New ExcelExport
        ExcelExport.Show()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' All of the below Subs execute when the respective MW Group membership is changed by clicking on the 'Set/Change MW Group' option 
    '  from the context menu for that particular MW group.
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub AnyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        MWNewBuildsToolStripMenuItem1.Click

        Dim Group As String = sender.Text
        UpdateGroup(Group)
    End Sub

    Private Sub SpotChcekHotfixToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SpotCheckHotfixToolStripMenuItem.Click
        Dim SpotCheck As New HotfixSpotCheck
        SpotCheck.Show()
    End Sub

    Public Sub TabPage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage2.Enter
        MWSet.Dock = DockStyle.Fill
        TabPage2.Controls.Add(MWSet)
    End Sub

    Private Sub TabPage3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage3.Enter
        CreateDep.Dock = DockStyle.Fill
        TabPage3.Controls.Add(CreateDep)
    End Sub

    Private Sub TabPage4_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage4.Enter
        AddSup.Dock = DockStyle.Fill
        TabPage4.Controls.Add(AddSup)
    End Sub

    Private Sub TabPage5_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage5.Enter
        Dists.Dock = DockStyle.Fill
        TabPage5.Controls.Add(Dists)
    End Sub

    Private Sub ListView_MW_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView_MW.ColumnClick
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub calls the correct sorter class when a column header is clicked on the ListView_MW.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Select Case e.Column
            Case 0
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListGroupSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListGroupDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColPasswordLastChanged
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListPwdLastSetSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListPwdLastSetDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColLastLogin
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListLastLoginSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListLastLoginDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColOS
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListOSSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListOSDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColGroupCount
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListGroupCountSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListGroupCountDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColMemberOf
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListmemberofSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListmemberofDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColGroupDescription
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListDescriptionSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListDescriptionDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColDomain
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListDomainSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListDomainDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColServerDescription
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListServerDescriptionSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListServerDescriptionDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColDepartment
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListDepartmentSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListDepartmentDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColPingStatus
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListPingStatusSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListPingStatusDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
            Case ColUptime
                If ListView_MW.Sorting = SortOrder.Ascending Then
                    ListView_MW.ListViewItemSorter = New ListUptimeSorter
                    ListView_MW.Sorting = SortOrder.Descending
                Else
                    ListView_MW.ListViewItemSorter = New ListUptimeDESorter
                    ListView_MW.Sorting = SortOrder.Ascending
                End If
        End Select
    End Sub


    Private Sub ChangeConfigurationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeConfigurationToolStripMenuItem.Click
        Dim AppConfig As New AppConfigure()
        AppConfig.Show()
    End Sub

End Class

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' The remainder of this file contains Class definitions that implement IComparer classes to sort the columns in the ListView_MW.
'  Each column has both a sort ascending and a sort descending class defined.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Class ListGroupSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.ToString.ToUpper > item2.ToString.ToUpper Then
            Return 1
        Else
            If item1.ToString.ToUpper < item2.ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function

End Class

Class ListGroupDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.ToString.ToUpper < item2.ToString.ToUpper Then
            Return 1
        Else
            If item1.ToString.ToUpper > item2.ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListPwdLastSetSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper > item2.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper < item2.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListPwdLastSetDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper < item2.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper > item2.SubItems(Main.ColPasswordLastChanged).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListLastLoginSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColLastLogin).ToString.ToUpper > item2.SubItems(Main.ColLastLogin).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColLastLogin).ToString.ToUpper < item2.SubItems(Main.ColLastLogin).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListLastLoginDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColLastLogin).ToString.ToUpper < item2.SubItems(Main.ColLastLogin).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColLastLogin).ToString.ToUpper > item2.SubItems(Main.ColLastLogin).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListOSSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColOS).ToString.ToUpper > item2.SubItems(Main.ColOS).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColOS).ToString.ToUpper < item2.SubItems(Main.ColOS).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListOSDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColOS).ToString.ToUpper < item2.SubItems(Main.ColOS).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColOS).ToString.ToUpper > item2.SubItems(Main.ColOS).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListGroupCountSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColGroupCount).ToString.ToUpper > item2.SubItems(Main.ColGroupCount).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColGroupCount).ToString.ToUpper < item2.SubItems(Main.ColGroupCount).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListGroupCountDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColGroupCount).ToString.ToUpper < item2.SubItems(Main.ColGroupCount).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColGroupCount).ToString.ToUpper > item2.SubItems(Main.ColGroupCount).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListmemberofSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColMemberOf).ToString.ToUpper > item2.SubItems(Main.ColMemberOf).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColMemberOf).ToString.ToUpper < item2.SubItems(Main.ColMemberOf).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListmemberofDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColMemberOf).ToString.ToUpper < item2.SubItems(Main.ColMemberOf).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColMemberOf).ToString.ToUpper > item2.SubItems(Main.ColMemberOf).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDescriptionSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColGroupDescription).ToString.ToUpper > item2.SubItems(Main.ColGroupDescription).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColGroupDescription).ToString.ToUpper < item2.SubItems(Main.ColGroupDescription).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDescriptionDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColGroupDescription).ToString.ToUpper < item2.SubItems(Main.ColGroupDescription).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColGroupDescription).ToString.ToUpper > item2.SubItems(Main.ColGroupDescription).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDepartmentSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColDepartment).ToString.ToUpper > item2.SubItems(Main.ColDepartment).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColDepartment).ToString.ToUpper < item2.SubItems(Main.ColDepartment).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDepartmentDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColDepartment).ToString.ToUpper < item2.SubItems(Main.ColDepartment).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColDepartment).ToString.ToUpper > item2.SubItems(Main.ColDepartment).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListServerDescriptionSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColServerDescription).ToString.ToUpper > item2.SubItems(Main.ColServerDescription).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColServerDescription).ToString.ToUpper < item2.SubItems(Main.ColServerDescription).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListServerDescriptionDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColServerDescription).ToString.ToUpper < item2.SubItems(Main.ColServerDescription).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColServerDescription).ToString.ToUpper > item2.SubItems(Main.ColServerDescription).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListPingStatusSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColPingStatus).ToString.ToUpper > item2.SubItems(Main.ColPingStatus).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColPingStatus).ToString.ToUpper < item2.SubItems(Main.ColPingStatus).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListPingStatusDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColPingStatus).ToString.ToUpper < item2.SubItems(Main.ColPingStatus).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColPingStatus).ToString.ToUpper > item2.SubItems(Main.ColPingStatus).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDomainSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColDomain).ToString.ToUpper > item2.SubItems(Main.ColDomain).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColDomain).ToString.ToUpper < item2.SubItems(Main.ColDomain).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDomainDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.ColDomain).ToString.ToUpper < item2.SubItems(Main.ColDomain).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.ColDomain).ToString.ToUpper > item2.SubItems(Main.ColDomain).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListUptimeSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim stritem1 As String = item1.SubItems(Main.ColUptime).Text.ToString
        Dim stritem2 As String = item2.SubItems(Main.ColUptime).Text.ToString
        If item1.SubItems(Main.ColUptime).Text.ToString <> "" Then
            If stritem1 = "Working..." Or stritem1 = "00:00:00" Or stritem1 = "Access Denied" Or stritem1 = "" Then
                stritem1 = "0.00:00:00"
            End If
            If stritem2 = "Working..." Or stritem2 = "00:00:00" Or stritem2 = "Access Denied" Or stritem2 = "" Then
                stritem2 = "0.00:00:00"
            End If
            Dim TS1 As TimeSpan = TimeSpan.Parse(stritem1)
            Dim TS2 As TimeSpan = TimeSpan.Parse(stritem2)
            If TS1 > TS2 Then
                Return 1
            Else
                If TS1 < TS2 Then
                    Return -1
                Else
                    Return 0
                End If
            End If
        End If
        Return 0
    End Function
End Class

Class ListUptimeDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim stritem1 As String = item1.SubItems(Main.ColUptime).Text.ToString
        Dim stritem2 As String = item2.SubItems(Main.ColUptime).Text.ToString
        If stritem1 <> "" Then
            If stritem1 = "Working..." Or stritem1 = "00:00:00" Or stritem1 = "Access Denied" Or stritem1 = "" Then
                stritem1 = "0.00:00:00"
            End If
            If stritem2 = "Working..." Or stritem2 = "00:00:00" Or stritem2 = "Access Denied" Or stritem2 = "" Then
                stritem2 = "0.00:00:00"
            End If
            Dim TS1 As TimeSpan = TimeSpan.Parse(stritem1)
            Dim TS2 As TimeSpan = TimeSpan.Parse(stritem2)
            If TS1 < TS2 Then
                Return 1
            Else
                If TS1 > TS2 Then
                    Return -1
                Else
                    Return 0
                End If
            End If
        End If
        Return 0
    End Function
End Class

