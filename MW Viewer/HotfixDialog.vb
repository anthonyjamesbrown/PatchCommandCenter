Imports System.Management
Imports System.Text
Public Class HotfixDialog
    Private groupTables() As Hashtable
    Private groupColumn As Integer = 0
    Private Function Get_InstalledPatches(ByVal ComputerName As String)
        Try
            Dim scope As ManagementScope
            scope = New ManagementScope("\\" & ComputerName & "\root\cimv2")
            scope.Connect()

            Dim query As ObjectQuery
            query = New ObjectQuery("SELECT HotfixID, Caption, Description, InstalledOn, InstalledBy FROM Win32_QuickFixEngineering")
            Dim searcher As ManagementObjectSearcher
            searcher = New ManagementObjectSearcher(scope, query)

            Dim queryCollection As ManagementObjectCollection
            queryCollection = searcher.Get()

            Dim m As ManagementObject
            For Each m In queryCollection
                Dim HotfixID As String = m("HotfixID")
                Dim Caption As String = m("Caption")
                Dim Description As String = m("Description")
                Dim InsalledOn As String = m("InstalledOn")
                Dim InstalledBy As String = m("InstalledBy")

                Dim TempItem As New ListViewItem()

                TempItem.Text = HotfixID
                TempItem.SubItems.Add(Description)
                TempItem.SubItems.Add(InsalledOn)
                TempItem.SubItems.Add(InstalledBy)
                TempItem.SubItems.Add(Caption)

                ListView_Hotfixes.Items.Add(TempItem)
            Next
        Catch
            ListView_Hotfixes.Items.Add("Can't connect to the remote system.  This operation requires local admin access to the remote computer.")
        End Try
        Return 0
    End Function

    Private Sub Form2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ComputerName As String = Main.ListView_MW.SelectedItems(0).Text
        ServerName_Label.Text = ComputerName
        Get_InstalledPatches(ComputerName)
        If ListView_Hotfixes.Items(0).Text <> "Can't connect to the remote system.  This operation requires local admin access to the remote computer." Then
            groupTables = New Hashtable(ListView_Hotfixes.Columns.Count) {}
            Dim column As Integer
            For column = 0 To ListView_Hotfixes.Columns.Count - 1
                groupTables(column) = CreateGroupsTable(column)
            Next column
            SetGroups(2)
        End If
    End Sub
    Private Sub myListView_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) ' Handles ListView_Hotfixes.ColumnClick
        If ListView_Hotfixes.Sorting = SortOrder.Descending OrElse _
            e.Column <> groupColumn Then
            ListView_Hotfixes.Sorting = SortOrder.Ascending
        Else
            ListView_Hotfixes.Sorting = SortOrder.Descending
        End If
        groupColumn = e.Column
        SetGroups(e.Column)

    End Sub
    Private Sub SetGroups(ByVal column As Integer)
        ListView_Hotfixes.Groups.Clear()

        Dim groups As Hashtable = CType(groupTables(column), Hashtable)

        Dim groupsArray(groups.Count - 1) As ListViewGroup
        groups.Values.CopyTo(groupsArray, 0)

        Array.Sort(groupsArray, New ListViewGroupSorter(ListView_Hotfixes.Sorting))
        ListView_Hotfixes.Groups.AddRange(groupsArray)

        Dim item As ListViewItem
        For Each item In ListView_Hotfixes.Items
            Dim subItemText As String = item.SubItems(column).Text

            If column = 0 Then
                subItemText = subItemText.Substring(0, 1)
            End If
            item.Group = CType(groups(subItemText), ListViewGroup)
        Next item

    End Sub

    Private Function CreateGroupsTable(ByVal column As Integer) As Hashtable
        Dim groups As New Hashtable()

        Dim item As ListViewItem
        For Each item In ListView_Hotfixes.Items
            Dim subItemText As String = item.SubItems(column).Text

            If column = 0 Then
                subItemText = subItemText.Substring(0, 1)
            End If
            If Not groups.Contains(subItemText) Then
                groups.Add(subItemText, New ListViewGroup(subItemText, HorizontalAlignment.Left))
            End If
        Next item
        Return groups
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub CopySelectedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopySelectedToolStripMenuItem.Click
        Dim buffer As New StringBuilder
        For i As Integer = 0 To ListView_Hotfixes.Columns.Count - 1
            buffer.Append(ListView_Hotfixes.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_Hotfixes.SelectedItems.Count - 1
            For j As Integer = 0 To ListView_Hotfixes.Columns.Count - 1
                buffer.Append(ListView_Hotfixes.SelectedItems(i).SubItems(j).Text)
                buffer.Append(vbTab)
            Next
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Dim buffer As New StringBuilder
        For i As Integer = 0 To ListView_Hotfixes.Columns.Count - 1
            buffer.Append(ListView_Hotfixes.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_Hotfixes.Items.Count - 1
            For j As Integer = 0 To ListView_Hotfixes.Columns.Count - 1
                buffer.Append(ListView_Hotfixes.Items(i).SubItems(j).Text)
                buffer.Append(vbTab)
            Next
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub
End Class
Class ListViewGroupSorter
    Implements IComparer
    Private order As SortOrder
    Public Sub New(ByVal theOrder As SortOrder)
        order = theOrder
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements IComparer.Compare
        Dim result As Integer = Date.Compare(CType(x, ListViewGroup).Header, CType(y, ListViewGroup).Header)
        If order = SortOrder.Ascending Then
            Return result
        Else
            Return -result
        End If
    End Function
End Class