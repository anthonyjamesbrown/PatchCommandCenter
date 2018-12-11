Imports Microsoft.Office.Interop
Imports System.ComponentModel
Imports Microsoft.ConfigurationManagement.ManagementProvider
Imports Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the Create Deployments user interface.  That interface is used to display information about deployments
'   as well as create new Software Update Group deployments.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CreateDeployments

    Private Workers() As BackgroundWorker
    Private NumWorkers = 0

    Public ColCollectionID As Integer = 1
    Public ColMWName As Integer = 2
    Public ColNbrMWs As Integer = 3
    Public ColDeployed As Integer = 4
    Public ColEnabled As Integer = 5
    Public ColComment As Integer = 6
    Public ColClass As Integer = 7
    Public ColStartTime As Integer = 8
    Public ColCount As Integer = 8

    Private Sub CreateDeployments_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' When the page loads populate the Dropdown list that contains a list of Software Update Groups.
        Populate_SUG_Dropdown()
    End Sub

    Private Sub Populate_SUG_Dropdown()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim MyConnection As New WqlConnectionManager
        MyConnection.Connect(Main.MWFunc.GetVarValue("CMServerFQDN"))
        Dim listOfSUGs As IResultObject
        Try
            Dim query As String = "Select LocalizedDisplayName from SMS_AuthorizationList Where CIType_ID = 9 And LocalizedDisplayName like 'Server%' Order By DateLastModified DESC"
            listOfSUGs = MyConnection.QueryProcessor.ExecuteQuery(query)
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try

        For Each Item As IResultObject In listOfSUGs
            ComboBox_SUGList.Items.Add(Item("LocalizedDisplayName").StringValue)
        Next
    End Sub

    Private Sub Button_GetDeployments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_GetDeployments.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ListView_Deployments.Items.Clear()

        Dim IW_StartDate As Date = Main.DateTime_IWStart.Value

        Dim SoftwareUpdateGroupName As String = ComboBox_SUGList.Text
        Dim LocalDataView As DataView = New DataView
        If SoftwareUpdateGroupName <> "" Then
            If IW_StartDate.DayOfWeek <> DayOfWeek.Tuesday Then
                Dim Result
                Result = MessageBox.Show("The Date you picked does not start on Tuesday.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Dim FillOption As String = Main.ToolStripComboBox_Class.Text

                If FillOption <> "All" Then
                    LocalDataView = Main.MWFunc.dataSet.Tables(0).DefaultView
                    LocalDataView.RowFilter = "Class='" & FillOption & "'"
                Else
                    LocalDataView = Main.MWFunc.dataSet.Tables(0).DefaultView
                    LocalDataView.RowFilter = String.Empty
                End If

                For Each RowView As DataRowView In LocalDataView
                    Dim DataRow As DataRow = RowView.Row

                    ' Get values ready for use in the new ListView item
                    Dim dsCollectionName As String = DataRow("collectionname").ToString
                    Dim dsCollectionID As String = DataRow("CollectionID").ToString
                    Dim dsDay As String = DataRow("Day").ToString
                    Dim dsStartTime As String = DataRow("StartTime").ToString
                    Dim dsDuration As String = DataRow("Duration").ToString
                    Dim dsClass As String = DataRow("Class").ToString

                    Dim BaseStartDate As Date = Main.MWFunc.CalcDate(dsDay, IW_StartDate)
                    Dim StartTime As Date = Date.Parse(dsStartTime)

                    Dim LocalStartDate As New DateTime(BaseStartDate.Year, BaseStartDate.Month, BaseStartDate.Day, StartTime.Hour, StartTime.Minute, 1)

                    ' Create a new temp ListView item and add column place holders
                    Dim TempItem As New ListViewItem()
                    Dim x As Integer
                    TempItem.Text = dsCollectionName
                    For x = 1 To ColCount
                        TempItem.SubItems.Add("")
                    Next

                    ' Populate the temp ListView item with data
                    TempItem.UseItemStyleForSubItems = False
                    TempItem.SubItems(ColCollectionID).Text = dsCollectionID
                    TempItem.SubItems(ColMWName).Text = "Working..."
                    TempItem.SubItems(ColNbrMWs).Text = "Working..."
                    TempItem.SubItems(ColDeployed).Text = "Working..."
                    TempItem.SubItems(ColEnabled).Text = "Working..."
                    TempItem.SubItems(ColComment).Text = "Working..."
                    TempItem.SubItems(ColClass).Text = dsClass
                    TempItem.SubItems(ColStartTime).Text = LocalStartDate

                    ' Add the temp ListView item to the ListView
                    ListView_Deployments.Items.Add(TempItem)
                    ListView_Deployments.Sort()

                    ' Create additional threads using background worker threads to go query the SCCM server to get the MW Description and to see if the 
                    '  MW already exists
                    NumWorkers = NumWorkers + 1 'This is the index trackers for the number of BackgroundWorker threads in the Workers thread array.
                    ReDim Workers(NumWorkers) 'Add a worker thread entry.
                    Workers(NumWorkers) = New BackgroundWorker 'Create the new thread.
                    Workers(NumWorkers).WorkerReportsProgress = True 'Set the report progress property.
                    Workers(NumWorkers).WorkerSupportsCancellation = True 'Set the supports cancellation property.
                    AddHandler Workers(NumWorkers).DoWork, AddressOf WorkerDoWork 'Create a reference for the dowork event for this worker.
                    AddHandler Workers(NumWorkers).RunWorkerCompleted, AddressOf WorkerCompleted ' Create a reference for the work completed event.
                    Workers(NumWorkers).RunWorkerAsync(dsCollectionID & "|" & SoftwareUpdateGroupName) 'Run the thread async.

                Next
                Button_Set.BackColor = Color.Green
                Button_Set.Enabled = True
            End If
        End If
    End Sub
    Private Sub WorkerDoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is the work completed by the BackgroundWorker threads for getting system uptime.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim dsCollectionID, SUGName As String

        dsCollectionID = (e.Argument.ToString.Split("|")(0)) 'Get the servername out of the passed Argument object.
        SUGName = (e.Argument.ToString.Split("|")(1))
        Dim dsComment As String = Main.MWFunc.GetCollectionComments(dsCollectionID)
        Dim MWCount As Integer = Main.MWFunc.MW_Count(dsCollectionID)
        Dim MWName As String = Main.MWFunc.Last_MW_Set(dsCollectionID)
        Dim Deployed As Boolean = Main.MWFunc.Deployment_Exists(dsCollectionID, SUGName)
        Dim Enabled As Boolean = Main.MWFunc.Deployment_Enabled(dsCollectionID, SUGName)

        e.Result = dsCollectionID & "|" & MWCount & "|" & MWName & "|" & dsComment & "|" & Deployed & "|" & Enabled
    End Sub
    Private Sub WorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is run as an Event Handler for when the server uptime backgroundworker completes.  This sub will handle updating the cache
        '  and UI with the uptime results.  This sub will call Update_ListViewUptime sub to update the UI.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim CollectionID As String = (e.Result.ToString.Split("|"))(0)
        Dim MWCount As Integer = (e.Result.ToString.Split("|"))(1)
        Dim MWName As String = (e.Result.ToString.Split("|"))(2)
        Dim CollComment As String = (e.Result.ToString.Split("|"))(3)
        Dim Deployed As Boolean = (e.Result.ToString.Split("|"))(4)
        Dim Enabled As Boolean = (e.Result.ToString.Split("|"))(5)
        Update_ListViewData(CollectionID, MWCount, MWName, CollComment, Deployed, Enabled)
    End Sub
    Private Sub Update_ListViewData(ByVal CollectionID As String, ByVal MWCount As String, ByVal MWName As String, ByVal CollComment As String, ByVal Deployed As Boolean, ByVal Enabled As Boolean)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is called by the thread completion event for getting server uptime.  This sub handles updating the UI with the uptime results.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If ListView_Deployments.Items.Count > 0 Then
            Dim Index2 As Integer = (ListView_Deployments.FindItemWithText(CollectionID, True, 0)).Index
            ListView_Deployments.Items(Index2).SubItems(ColNbrMWs).Text = MWCount
            ListView_Deployments.Items(Index2).SubItems(ColMWName).Text = MWName
            ListView_Deployments.Items(Index2).SubItems(ColComment).Text = CollComment
            ListView_Deployments.Items(Index2).SubItems(ColDeployed).Text = Deployed
            ListView_Deployments.Items(Index2).SubItems(ColEnabled).Text = Enabled

            If Deployed = True Then
                ListView_Deployments.Items(Index2).SubItems(ColDeployed).ForeColor = Color.Green
            Else
                ListView_Deployments.Items(Index2).SubItems(ColDeployed).ForeColor = Color.Red
            End If

            If Enabled = True Then
                ListView_Deployments.Items(Index2).SubItems(ColEnabled).ForeColor = Color.Green
            Else
                ListView_Deployments.Items(Index2).SubItems(ColEnabled).ForeColor = Color.Red
            End If
        End If
    End Sub

    Private Sub CheckBox_CheckAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_CheckAll.CheckedChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles checking or unchecking all items in the listview for this form.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim CheckStatus As Boolean = CheckBox_CheckAll.Checked
        Dim I As Integer
        If CheckStatus Then
            For I = 0 To ListView_Deployments.Items.Count - 1
                ListView_Deployments.Items(I).Checked = True
            Next
        Else
            For I = 0 To ListView_Deployments.Items.Count - 1
                ListView_Deployments.Items(I).Checked = False
            Next
        End If
    End Sub

    Private Sub Button_Set_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Set.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles creating deployments for all 'checked' collections when the Set button is clicked.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer

        ' Pull the Software Udpate Group name from what is selected in the SUGList dropdown box.
        Dim SoftwareUpdateGroupName As String = ComboBox_SUGList.Text
        GroupBox1.Enabled = False

        ' Create a list to keep track of each deployment that is added.  This is used later to build the message body of an email that will be sent as
        '  a summary of the deployments created.
        Dim AddedDeployments As List(Of String) = New List(Of String)

        ' Loop through all of the selected (Checked) Collections and add a deployment for the  software update group targeting that collection.
        For I = 0 To ListView_Deployments.CheckedItems.Count - 1
            Dim CollectionName As String = ListView_Deployments.CheckedItems(I).Text
            Dim CollID As String = ListView_Deployments.CheckedItems(I).SubItems(ColCollectionID).Text
            Dim Status As Boolean = ListView_Deployments.CheckedItems(I).SubItems(ColDeployed).Text

            ' The status column will hold a boolean value indicating whether or not a deployment for this collection for the selected SUG already exists or not.
            '  If the deployment does not exist then allow to proceed, otherwise skip it.
            If Status = False Then
                ' Check to see if the user should be prompted for each deployment.
                Dim Prompt As Boolean = Main.PromptStatus.CheckState
                Dim Result As MsgBoxResult
                Dim PromptText As String = "Set Deployment for collection : " & CollectionName & " (" & CollID & ")" & vbCrLf & _
                                               "Software Update Group : " & SoftwareUpdateGroupName
                If Prompt Then
                    Result = MessageBox.Show(PromptText, "Confirm", MessageBoxButtons.YesNo)
                Else
                    Result = MsgBoxResult.Yes
                End If
                ' If the user answered yes or if prompting was disabled proceed with the deployment, otherwise skip it.
                If Result = MsgBoxResult.Yes Then
                    ' Call Function to Create Deployment
                    Main.MWFunc.CreateUpdateDeployment(SoftwareUpdateGroupName, CollectionName, CollID)
                    ' Update the UI
                    ListView_Deployments.CheckedItems(I).SubItems(ColDeployed).Text = True
                    ListView_Deployments.CheckedItems(I).SubItems(ColDeployed).ForeColor = Color.Green
                    ' Add the deployment info to the list.  This will be used in the next section to send a summary email.
                    AddedDeployments.Add(PromptText)
                End If
            End If
        Next

        ' If a deployment was added, IE the AddedDeployment list has at least one item then send an email.
        If AddedDeployments.Count > 0 Then
            Dim x As Integer
            Dim MessageBody As String

            ' Construct the Message Body.
            ' Start with the header info.
            MessageBody = "Operation performed by: " & Main.CurrentUser & vbCrLf & _
                          "Operation performed   : " & Now().ToString & vbCrLf & vbCrLf & _
                          "Deployments were created on the following Collections:" & vbCrLf & vbCrLf
            ' Loop through the added deployments list and append the message body with data for each.
            For x = 0 To AddedDeployments.Count - 1
                MessageBody = MessageBody & AddedDeployments.Item(x).ToString & vbCrLf & vbCrLf
            Next
            ' Call the function to send the email.
            Main.MWFunc.SendEmail("Deployment Creation operation", MessageBody)
        End If
        Button_Set.BackColor = Color.LightGray
        Button_GetDeployments.Enabled = True
    End Sub

    Private Sub GetMWDetailsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetMWDetailsToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function calls the form that shows detailed info about Maintenance Windows assigned to the selected Collection.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim MWInfo As New MWDetails
        MWInfo.CollectionName = ListView_Deployments.SelectedItems(0).Text
        MWInfo.CollectionID = ListView_Deployments.SelectedItems(0).SubItems(1).Text
        MWInfo.Show()
    End Sub

    Private Sub ListView_MW_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView_Deployments.ColumnClick
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub calls the correct sorter class when a column header is clicked on the ListView_MW.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Select Case e.Column
            Case 0
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListCollectionNameSorter
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListCollectionNameDESorter
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColCollectionID
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListCollectionIDSorter
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListCollectionIDDESorter
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColMWName
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListMWNameSorter
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListMWNameDESorter
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColNbrMWs
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListNbrMWsSorter
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListNbrMWsDESorter
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColDeployed
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListDeployedSorter
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListDeployedDESorter
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColEnabled
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListEnabledSorter
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListEnabledDESorter
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColComment
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListCommentSorter
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListCommentDESorter
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColClass
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListClassSorter2
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListClassDESorter2
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
            Case ColStartTime
                If ListView_Deployments.Sorting = SortOrder.Ascending Then
                    ListView_Deployments.ListViewItemSorter = New ListLocalStartSorter3
                    ListView_Deployments.Sorting = SortOrder.Descending
                Else
                    ListView_Deployments.ListViewItemSorter = New ListLocalStartDESorter3
                    ListView_Deployments.Sorting = SortOrder.Ascending
                End If
        End Select
    End Sub

    Private Sub Button_Copy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Copy.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles copying ALL the data in the main ListView to the Clipboard including the Header names.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim buffer As New System.Text.StringBuilder
        For i As Integer = 0 To ListView_Deployments.Columns.Count - 1
            buffer.Append(ListView_Deployments.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_Deployments.Items.Count - 1
            For j As Integer = 0 To ListView_Deployments.Columns.Count - 1
                buffer.Append(ListView_Deployments.Items(i).SubItems(j).Text)
                buffer.Append(vbTab)
            Next
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Sub Button_CopySelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CopySelected.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles copying the SELECTED data in the main ListView to the Clipboard including the Header names.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim buffer As New System.Text.StringBuilder
        For i As Integer = 0 To ListView_Deployments.Columns.Count - 1
            buffer.Append(ListView_Deployments.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_Deployments.SelectedItems.Count - 1
            For j As Integer = 0 To ListView_Deployments.Columns.Count - 1
                buffer.Append(ListView_Deployments.SelectedItems(i).SubItems(j).Text)
                buffer.Append(vbTab)
            Next
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Sub Button_Export2Excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Export2Excel.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles creating an Excel export of the information in the ListView.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim i As Integer

        xlApp = New Excel.Application
        xlWorkBook = xlApp.Workbooks.Add(misValue)
        xlWorkSheet = xlWorkBook.Sheets("sheet1")
        xlWorkSheet.Name = "Deployments"

        xlWorkSheet.Cells(2, 1) = ComboBox_SUGList.Text & " Deployments"
        xlWorkSheet.Range("A2:G2").Merge()

        Dim style As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle1")
        style.Font.Bold = True
        style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange)

        Dim style2 As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle2")
        style2.Font.Bold = True
        style2.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray)

        xlWorkSheet.Range("A2:G2").Style = "NewStyle1"

        xlWorkSheet.Cells(3, 1) = "Collection Name"
        xlWorkSheet.Cells(3, 2) = "Collection ID"
        xlWorkSheet.Cells(3, 3) = "Last MW Name"
        xlWorkSheet.Cells(3, 4) = "Nbr of MWs"
        xlWorkSheet.Cells(3, 5) = "Deployed"
        xlWorkSheet.Cells(3, 6) = "Enalbed"
        xlWorkSheet.Cells(3, 7) = "Collection Comments"

        xlWorkSheet.Range("A3:G3").Style = "NewStyle2"
        xlWorkSheet.Range("A3:G3").Borders.Weight = 2
        xlWorkSheet.Range("A3:G3").HorizontalAlignment = -4108

        xlWorkSheet.Range("A3:G3").RowHeight = 25
        xlWorkSheet.Columns("A:G").ColumnWidth = 35
        xlWorkSheet.Columns("G").ColumnWidth = 110

        Dim rCollectionName As String
        Dim rCollectionID As String
        Dim rLastMWSet As String
        Dim rNbrOfMWs As String
        Dim rDeployed As String
        Dim rEnabled As String
        Dim rComment As String

        For i = 0 To ListView_Deployments.Items.Count - 1
            rCollectionName = ListView_Deployments.Items.Item(i).Text.ToString
            rCollectionID = ListView_Deployments.Items.Item(i).SubItems(1).Text
            rLastMWSet = ListView_Deployments.Items.Item(i).SubItems(2).Text
            rNbrOfMWs = ListView_Deployments.Items.Item(i).SubItems(3).Text
            rDeployed = ListView_Deployments.Items.Item(i).SubItems(4).Text
            rEnabled = ListView_Deployments.Items.Item(i).SubItems(5).Text
            rComment = ListView_Deployments.Items.Item(i).SubItems(6).Text

            ' Col one is  blank
            xlWorkSheet.Cells(i + 4, 1) = rCollectionName
            xlWorkSheet.Cells(i + 4, 2) = rCollectionID
            xlWorkSheet.Cells(i + 4, 3) = rLastMWSet
            xlWorkSheet.Cells(i + 4, 4) = rNbrOfMWs
            xlWorkSheet.Cells(i + 4, 5) = rDeployed
            xlWorkSheet.Cells(i + 4, 6) = rEnabled
            xlWorkSheet.Cells(i + 4, 7) = rComment

        Next

        xlWorkSheet.Range("A4:G" & ListView_Deployments.Items.Count + 3).Borders.Weight = 2
        xlWorkSheet.Range("A4:G" & ListView_Deployments.Items.Count + 3).HorizontalAlignment = -4108

        Dim CurrRow As Integer = ListView_Deployments.Items.Count + 3

        Dim dlg As New SaveFileDialog
        dlg.Filter = "Excel Files (*.xlsx)|*.xlsx"
        dlg.FilterIndex = 1
        dlg.InitialDirectory = Main.MWFunc.GetVarValue("ChangeDirectoryPath")
        dlg.FileName = ComboBox_SUGList.Text & " Deployments"
        Dim ExcelFile As String = """"
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            ExcelFile = dlg.FileName
            xlWorkSheet.SaveAs(ExcelFile)
        End If
        xlWorkBook.Close()
        xlApp.Quit()
    End Sub
End Class

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' The remainder of this file contains Class definitions that implement IComparer classes to sort the columns in the ListView_MW.
'  Each column has both a sort ascending and a sort descending class defined.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Class ListCollectionNameSorter
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

Class ListCollectionNameDESorter
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

Class ListCollectionIDSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColCollectionID
        If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListCollectionIDDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColCollectionID
        If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListMWNameSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColMWName
        If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListMWNameDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColMWName
        If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListNbrMWsSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColNbrMWs
        If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListNbrMWsDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColNbrMWs
        If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDeployedSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColDeployed
        If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListDeployedDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColDeployed
        If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListEnabledSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColEnabled
        If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListEnabledDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColEnabled
        If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListCommentSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColComment
        If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListCommentDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColComment
        If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListClassSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColClass
        If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class

Class ListClassDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim Index As Integer = Main.CreateDep.ColClass
        If item1.SubItems(Index).ToString.ToUpper < item2.SubItems(Index).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Index).ToString.ToUpper > item2.SubItems(Index).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListLocalStartSorter3
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.CreateDep.ColStartTime).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.CreateDep.ColStartTime).Text)
        If d1 > d2 Then
            Return 1
        Else
            If d1 < d2 Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListLocalStartDESorter3
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.CreateDep.ColStartTime).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.CreateDep.ColStartTime).Text)
        If d1 > d2 Then
            Return 1
        Else
            If d1 < d2 Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class