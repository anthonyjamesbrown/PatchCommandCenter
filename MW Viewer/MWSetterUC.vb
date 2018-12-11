Imports System.ComponentModel
Imports System.Globalization
Imports Microsoft.Office.Interop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the MWSetterUC form.  This form allows users to manage setting Maintenance Windows on collections.
'  This form uses data from the Collection Data xml file to populate and calculate Maintenance Windows on each collection.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class MWSetterUC
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Define class varables 
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Workers() As BackgroundWorker
    Private NumWorkers = 0

    ' Define Col Index numbers, these are used to reference the column order in the ListView
    Public ColCollectionID As Integer = 1
    Public ColDay As Integer = 2
    Public ColLocalStart As Integer = 3
    Public ColLocalEnd As Integer = 4
    Public ColUTCStart As Integer = 5
    Public ColUTCEnd As Integer = 6
    Public ColDuration As Integer = 7
    Public ColStatus As Integer = 8
    Public ColComment As Integer = 9
    Public ColClass As Integer = 10
    Public ColCount As Integer = 10

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is run when the form loads  
        '  - Read in and populate the MWData.xml file
        '  - Create a connection to the SCCM Site Server
        '  - Display the timezone of the client running this application
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'dataSet.ReadXml(sPath & "MWData.xml")
        DataGridView1.DataSource = Main.MWFunc.dataSet
        DataGridView1.DataMember = "collectionitem"

        CheckBox_SelectAll.Enabled = False

        Label_Timezone.Text = TimeZone.CurrentTimeZone.StandardName & " (UTC: " & TimeZone.CurrentTimeZone.GetUtcOffset(Now()).ToString() & ")"
    End Sub
    Private Sub Button_CalculateMW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CalculateMW.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the 'Calculate MWs' button click.  This sub is the main meat of this application.        
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim IW_StartDate As Date = Main.DateTime_IWStart.Value

        Dim MW_Type As String = ToolStripComboBox_MWType.Text
        Dim MW_Type_Code As Integer = 4

        If MW_Type = "Software Updates" Then MW_Type_Code = 4
        If MW_Type = "All Deployments" Then MW_Type_Code = 1

        ' If the StartDate is toward the end of the month and will span into the next month then the MW nameing will reflect both months.
        If IW_StartDate.Day > 27 Then
            Dim MonthLabel As String
            Dim EndDate As DateTime
            EndDate = IW_StartDate.AddDays(7)
            If IW_StartDate.ToString("MMMM", New CultureInfo("en-US")) <> EndDate.ToString("MMMM", New CultureInfo("en-US")) Then
                MonthLabel = IW_StartDate.ToString("MMMM", New CultureInfo("en-US")) & "-" & EndDate.ToString("MMMM", New CultureInfo("en-US"))
                If MW_Type_Code = 1 Then
                    TextBox_MWName.Text = IW_StartDate.Year.ToString + " " + MonthLabel + " IW (App)"
                Else
                    TextBox_MWName.Text = IW_StartDate.Year.ToString + " " + MonthLabel + " IW"
                End If
            End If
        Else
            If MW_Type_Code = 1 Then
                TextBox_MWName.Text = IW_StartDate.Year.ToString + " " + IW_StartDate.ToString("MMMM", New CultureInfo("en-US")) + " IW (App)"
            Else
                TextBox_MWName.Text = IW_StartDate.Year.ToString + " " + IW_StartDate.ToString("MMMM", New CultureInfo("en-US")) + " IW"
            End If
        End If
        ListView1.Items.Clear()

        ' Make sure that the date picked falls on a Tuesday since that is the day that IW starts on.
        If IW_StartDate.DayOfWeek <> DayOfWeek.Tuesday Then
            Dim Result
            Result = MessageBox.Show("The Date you picked does not start on Tuesday.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Dim LocalDataView As DataView = New DataView
            Dim FillOption As String = Main.ToolStripComboBox_Class.Text

            If FillOption <> "All" Then
                LocalDataView = Main.MWFunc.dataSet.Tables(0).DefaultView
                LocalDataView.RowFilter = "Class='" & FillOption & "'"
            Else
                LocalDataView = Main.MWFunc.dataSet.Tables(0).DefaultView
                LocalDataView.RowFilter = String.Empty
            End If

            ' Begin main loop to populate the ListView
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
                Dim LocalEndDate As Date = LocalStartDate.AddHours(dsDuration)
                Dim UTCStartDate As Date = LocalStartDate.ToUniversalTime()
                Dim UTCEndDate As Date = UTCStartDate.AddHours(dsDuration)

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
                TempItem.SubItems(ColDay).Text = dsDay
                TempItem.SubItems(ColLocalStart).Text = LocalStartDate
                TempItem.SubItems(ColLocalStart).BackColor = Color.LightSkyBlue
                TempItem.SubItems(ColLocalEnd).Text = LocalEndDate
                TempItem.SubItems(ColLocalEnd).BackColor = Color.LightSkyBlue
                TempItem.SubItems(ColUTCStart).Text = UTCStartDate
                TempItem.SubItems(ColUTCStart).BackColor = Color.LightSalmon
                TempItem.SubItems(ColUTCEnd).Text = UTCEndDate
                TempItem.SubItems(ColUTCEnd).BackColor = Color.LightSalmon
                TempItem.SubItems(ColDuration).Text = dsDuration
                TempItem.SubItems(ColStatus).Text = "Working..."
                TempItem.SubItems(ColComment).Text = "Working..."
                TempItem.SubItems(ColClass).Text = dsClass

                ' Add the temp ListView item to the ListView
                ListView1.Items.Add(TempItem)
                ListView1.Sort()

                ' Create additional threads using background worker threads to go query the SCCM server to get the MW Description and to see if the 
                '  MW already exists
                NumWorkers = NumWorkers + 1 'This is the index trackers for the number of BackgroundWorker threads in the Workers thread array.
                ReDim Workers(NumWorkers) 'Add a worker thread entry.
                Workers(NumWorkers) = New BackgroundWorker 'Create the new thread.
                Workers(NumWorkers).WorkerReportsProgress = True 'Set the report progress property.
                Workers(NumWorkers).WorkerSupportsCancellation = True 'Set the supports cancellation property.
                AddHandler Workers(NumWorkers).DoWork, AddressOf WorkerDoWork 'Create a reference for the dowork event for this worker.
                AddHandler Workers(NumWorkers).RunWorkerCompleted, AddressOf WorkerCompleted ' Create a reference for the work completed event.
                Workers(NumWorkers).RunWorkerAsync(dsCollectionID) 'Run the thread async.
            Next
            CheckBox_SelectAll.Enabled = True
        End If
    End Sub
    Private Sub WorkerDoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is the work completed by the BackgroundWorker threads for getting system uptime.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim dsCollectionID As String

        dsCollectionID = e.Argument.ToString 'Get the servername out of the passed Argument object.
        Dim dsComment As String = Main.MWFunc.GetCollectionComments(dsCollectionID)
        Dim Exists As Boolean = Main.MWFunc.MW_Exists(dsCollectionID, TextBox_MWName.Text)

        e.Result = dsCollectionID & "|" & Exists & "|" & dsComment
    End Sub
    Private Sub WorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is run as an Event Handler for when the server uptime backgroundworker completes.  This sub will handle updating the cache
        '  and UI with the uptime results.  This sub will call Update_ListViewUptime sub to update the UI.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim CollectionID As String = (e.Result.ToString.Split("|"))(0)
        Dim MWExists As Boolean = (e.Result.ToString.Split("|"))(1)
        Dim CollComment As String = (e.Result.ToString.Split("|"))(2)
        Update_ListViewData(CollectionID, MWExists, CollComment)
    End Sub
    Private Sub Update_ListViewData(ByVal CollectionID As String, ByVal MWExists As Boolean, ByVal CollComment As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is called by the thread completion event for getting server uptime.  This sub handles updating the UI with the uptime results.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If ListView1.Items.Count > 0 Then
            Dim Index2 As Integer = (ListView1.FindItemWithText(CollectionID, True, 0)).Index
            ListView1.Items(Index2).SubItems(ColStatus).Text = MWExists
            ListView1.Items(Index2).SubItems(ColComment).Text = CollComment
        End If
    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Export2Excel.Click
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
        xlWorkSheet.Name = TextBox_MWName.Text & " Schedule"

        xlWorkSheet.Cells(2, 1) = TextBox_MWName.Text & " Infrastructure Weekend RFC schedule"
        xlWorkSheet.Range("A2:L2").Merge()

        Dim style As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle1")
        style.Font.Bold = True
        style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange)

        Dim style2 As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle2")
        style2.Font.Bold = True
        style2.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray)

        xlWorkSheet.Range("A2:L2").Style = "NewStyle1"

        xlWorkSheet.Cells(3, 1) = "Change Order #"
        xlWorkSheet.Cells(3, 2) = "Schedule Start Date"
        xlWorkSheet.Cells(3, 3) = "Schedule End"
        xlWorkSheet.Cells(3, 4) = "Schedule Start Date (UTC)"
        xlWorkSheet.Cells(3, 5) = "Schedule End (UTC)"
        xlWorkSheet.Cells(3, 6) = "Hours"
        xlWorkSheet.Cells(3, 7) = "Summary"
        xlWorkSheet.Cells(3, 8) = "Product Manager"
        xlWorkSheet.Cells(3, 9) = "Relationship Manager"
        xlWorkSheet.Cells(3, 10) = "Set MW"
        xlWorkSheet.Cells(3, 11) = "Included in Deployment"
        xlWorkSheet.Cells(3, 12) = "Class"

        xlWorkSheet.Range("A3:L3").Style = "NewStyle2"
        xlWorkSheet.Range("A3:L3").Borders.Weight = 2
        xlWorkSheet.Range("A3:L3").HorizontalAlignment = -4108

        xlWorkSheet.Range("A3:L3").RowHeight = 25
        xlWorkSheet.Columns("A:L").ColumnWidth = 25
        xlWorkSheet.Columns("B:E").ColumnWidth = 35
        xlWorkSheet.Columns("A").ColumnWidth = 20
        xlWorkSheet.Columns("F").ColumnWidth = 10
        xlWorkSheet.Columns("G").ColumnWidth = 110
        xlWorkSheet.Columns("J").ColumnWidth = 15


        Dim rCollectionName As String
        Dim rCollectionID As String
        Dim rDay As String
        Dim rLocalStart As DateTime
        Dim rLocalEnd As DateTime
        Dim rUTCStart As DateTime
        Dim rUTCEnd As DateTime
        Dim rDuration As String
        Dim rMWSet As String
        Dim rComment As String
        Dim Summary As String
        Dim rClass As String

        For i = 0 To ListView1.Items.Count - 1
            rCollectionName = ListView1.Items.Item(i).Text.ToString
            rCollectionID = ListView1.Items.Item(i).SubItems(1).Text
            rDay = ListView1.Items.Item(i).SubItems(2).Text
            rLocalStart = DateTime.Parse(ListView1.Items.Item(i).SubItems(3).Text)
            rLocalEnd = DateTime.Parse(ListView1.Items.Item(i).SubItems(4).Text)
            rUTCStart = DateTime.Parse(ListView1.Items.Item(i).SubItems(5).Text)
            rUTCEnd = DateTime.Parse(ListView1.Items.Item(i).SubItems(6).Text)
            rDuration = ListView1.Items.Item(i).SubItems(7).Text
            rMWSet = ListView1.Items.Item(i).SubItems(8).Text
            rComment = ListView1.Items.Item(i).SubItems(9).Text
            rClass = ListView1.Items.Item(i).SubItems(10).Text

            Summary = "Security Patching for " & rCollectionName & " " & rComment & " - " & TextBox_MWName.Text

            ' Col one is  blank
            xlWorkSheet.Cells(i + 4, 2) = rLocalStart.ToLongDateString() & " " & rLocalStart.ToLongTimeString()
            xlWorkSheet.Cells(i + 4, 3) = rLocalEnd.ToLongDateString() & " " & rLocalEnd.ToLongTimeString()
            xlWorkSheet.Cells(i + 4, 4) = rUTCStart.ToLongDateString() & " " & rUTCStart.ToLongTimeString()
            xlWorkSheet.Cells(i + 4, 5) = rUTCEnd.ToLongDateString() & " " & rUTCEnd.ToLongTimeString()
            xlWorkSheet.Cells(i + 4, 6) = rDuration
            xlWorkSheet.Cells(i + 4, 7) = Summary
            xlWorkSheet.Cells(i + 4, 8) = ""
            xlWorkSheet.Cells(i + 4, 9) = ""
            xlWorkSheet.Cells(i + 4, 10) = rMWSet
            xlWorkSheet.Cells(i + 4, 11) = ""
            xlWorkSheet.Cells(i + 4, 12) = rClass
        Next
        xlWorkSheet.Range("A4:L" & ListView1.Items.Count + 3).Borders.Weight = 2
        xlWorkSheet.Range("A4:F" & ListView1.Items.Count + 3).HorizontalAlignment = -4108

        Dim CurrRow As Integer = ListView1.Items.Count + 3
        Dim PreviousMonth As String = rLocalStart.AddMonths(-1).ToString("MMMM", New CultureInfo("en-US"))
        Dim Year As String = rLocalStart.Year.ToString

        xlWorkSheet.Cells(CurrRow + 2, 1) = PreviousMonth & " " & Year & " Post Infrastructure Weekend RFC schedule"
        xlWorkSheet.Range("A" & CurrRow + 2 & ":L" & CurrRow + 2).Merge()
        xlWorkSheet.Range("A" & CurrRow + 2 & ":L" & CurrRow + 2).Style = "NewStyle1"

        xlWorkSheet.Cells(CurrRow + 3, 1) = "Change Order #"
        xlWorkSheet.Cells(CurrRow + 3, 2) = "Schedule Start Date"
        xlWorkSheet.Cells(CurrRow + 3, 5) = "Schedule End"
        xlWorkSheet.Cells(CurrRow + 3, 7) = "POST - Infrastructure Weekend - " & TextBox_MWName.Text
        xlWorkSheet.Range("A" & CurrRow + 3 & ":L" & CurrRow + 3).Font.Bold = True
        xlWorkSheet.Range("A" & CurrRow + 2 & ":L" & CurrRow + 5).Borders.Weight = 2
        xlWorkSheet.Range("A" & CurrRow + 3 & ":F" & CurrRow + 5).HorizontalAlignment = -4108

        Dim dlg As New SaveFileDialog
        dlg.Filter = "Excel Files (*.xlsx)|*.xlsx"
        dlg.FilterIndex = 1
        dlg.InitialDirectory = Main.MWFunc.GetVarValue("ChangeDirectoryPath")
        dlg.FileName = TextBox_MWName.Text & " Schedule"
        Dim ExcelFile As String = """"
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            ExcelFile = dlg.FileName
            xlWorkSheet.SaveAs(ExcelFile)
        End If
        xlWorkBook.Close()
        xlApp.Quit()
    End Sub

    Private Sub Button_SetMWs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SetMWs.Click, AddMWToAllCheckedCollectionsToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles creating new Maintenance Windows on each of the checked collections when the 'Set Checked' button is clicked.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim I As Integer

        ' Pull data from the UI
        Dim MW_Type As String = ToolStripComboBox_MWType.Text
        Dim MW_Type_Code As Integer = 4

        If MW_Type = "Software Updates" Then MW_Type_Code = 4
        If MW_Type = "All Deployments" Then MW_Type_Code = 1

        ' Create a list to keep track of each MW that is added.  This is used later to build the message body of an email that will be sent as
        '  a summary of the MWs created.
        Dim AddedCollections As List(Of String) = New List(Of String)

        ' Loop through all of the selected (Checked) Collections and add a MW that collection with the date information from the UI.
        For I = 0 To ListView1.CheckedItems.Count - 1
            Dim CollectionName As String = ListView1.CheckedItems(I).Text
            Dim UTCStartTime As Date = Date.Parse(ListView1.CheckedItems(I).SubItems(ColUTCStart).Text)
            Dim LocalStartTime As Date = Date.Parse(ListView1.CheckedItems(I).SubItems(ColLocalStart).Text)
            Dim Duration As Integer = CInt(ListView1.CheckedItems(I).SubItems(ColDuration).Text)
            Dim Status As Boolean = ListView1.CheckedItems(I).SubItems(ColStatus).Text
            Dim CollID As String = ListView1.CheckedItems(I).SubItems(ColCollectionID).Text
            Dim MWName As String = TextBox_MWName.Text
            If MW_Type_Code = 1 Then MWName = TextBox_MWName.Text & " (App)"

            Dim CIMDate As String = Main.MWFunc.ConvertDatetoCIM(UTCStartTime)

            Dim MyServiceWindowSchedule As String = Main.MWFunc.CreateDailyRecurringScheduleToken(Duration, 0, CIMDate, True)

            ' The status column will hold a boolean value indicating whether or not a deployment for this collection for the selected MW already exists or not.
            '  If the MW does not exist then allow to proceed, otherwise skip it.
            If Status = False Then
                ' Check to see if the user should be prompted for each MW.
                Dim Prompt As Boolean = Main.PromptStatus.CheckState
                Dim Result As MsgBoxResult
                Dim PromptText As String = "Set MW Window for collection : " & CollectionName & " (" & CollID & ")" & vbCrLf & _
                                           "Create new MW called : " & MWName & vbCrLf & _
                                           "MW Start Date/Time (UTC) : " & UTCStartTime & vbCrLf & _
                                           "MW Start Date/Time (Local): " & LocalStartTime & vbCrLf & _
                                           "MW Duration : " & Duration & vbCrLf & _
                                           "MW Type : " & MW_Type
                If Prompt Then
                    Result = MessageBox.Show(PromptText, "Confirm", MessageBoxButtons.YesNo)
                Else
                    Result = MsgBoxResult.Yes
                End If
                ' If the user answered yes or if prompting was disabled proceed with the creating the new MW, otherwise skip it.
                If Result = MsgBoxResult.Yes Then
                    ' Call the Create Maintenance Windows function.
                    Main.MWFunc.CreateMaintenanceWindow(CollID, MWName, "Occurs on (UTC) " & UTCStartTime, MyServiceWindowSchedule, True, MW_Type_Code)
                    ' Update the UI
                    ListView1.CheckedItems(I).SubItems(ColStatus).Text = True

                    ' Add the MW info to the list.  This will be used in the next section to send a summary email.
                    AddedCollections.Add(PromptText)
                End If
            End If
        Next

        ' If a MW was added, IE the AddedCollections list has at least one item then send an email.
        If AddedCollections.Count > 0 Then
            Dim x As Integer

            ' Construct the Message Body.
            ' Start with the header info.
            Dim MessageBody As String
            MessageBody = "Operation performed by: " & Main.CurrentUser & vbCrLf & _
                          "Operation performed   : " & Now().ToString & vbCrLf & vbCrLf & _
                          "Maintenance Windows were set on the following Collections:" & vbCrLf & vbCrLf
            ' Loop through the added deployments list and append the message body with data for each.
            For x = 0 To AddedCollections.Count - 1
                MessageBody = MessageBody & AddedCollections.Item(x).ToString & vbCrLf & vbCrLf
            Next
            ' Call the Send Email function
            Main.MWFunc.SendEmail("MWs Creation operation", MessageBody)
        End If
    End Sub

    Private Sub CheckBox_SelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_SelectAll.CheckedChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim CheckStatus As Boolean = CheckBox_SelectAll.Checked
        Dim I As Integer
        If CheckStatus Then
            For I = 0 To ListView1.Items.Count - 1
                ListView1.Items(I).Checked = True
            Next
        Else
            For I = 0 To ListView1.Items.Count - 1
                ListView1.Items(I).Checked = False
            Next
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem1.Click, Button_Copy.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim buffer As New System.Text.StringBuilder
        For i As Integer = 0 To ListView1.Columns.Count - 1
            buffer.Append(ListView1.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView1.SelectedItems.Count - 1
            For j As Integer = 0 To ListView1.Columns.Count - 1
                buffer.Append(ListView1.SelectedItems(i).SubItems(j).Text)
                buffer.Append(vbTab)
            Next
            buffer.Append(vbCrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Sub AddCustomMWToCollectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCustomMWToCollectionToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim CustomFrm1 As New CustomMW
        CustomFrm1.Show()
    End Sub

    Private Sub CreateNewMWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttn_AddMW.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Frm_AddMW As New AddMW
        Frm_AddMW.Show()
    End Sub

    Private Sub GetMWDetailsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetMWDetailsToolStripMenuItem.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim MWInfo As New MWDetails
        MWInfo.CollectionName = ListView1.SelectedItems(0).Text
        MWInfo.CollectionID = ListView1.SelectedItems(0).SubItems(1).Text
        MWInfo.Show()
    End Sub

    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub calls the correct sorter class when a column header is clicked on the ListView_MW.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Select Case e.Column
            Case 0
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListCollectionSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListCollectionDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 1
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListCollectionIDSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListCollectionIDDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 2
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListDaySorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListDayDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 3
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListUTCStartSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListUTCStartDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 4
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListUTCEndSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListUTCEndDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 5
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListLocalStartSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListLocalStartDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 6
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListLocalEndSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListLocalEndDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 7
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListDurationSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListDurationDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 8
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListStatusSorter2
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListStatusDESorter2
                    ListView1.Sorting = SortOrder.Ascending
                End If
            Case 10
                If ListView1.Sorting = SortOrder.Ascending Then
                    ListView1.ListViewItemSorter = New ListClassSorter
                    ListView1.Sorting = SortOrder.Descending
                Else
                    ListView1.ListViewItemSorter = New ListClassDESorter
                    ListView1.Sorting = SortOrder.Ascending
                End If
        End Select
    End Sub

End Class

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' The remainder of this file contains Class definitions that implement IComparer classes to sort the columns in the ListView1.
'  Each column has both a sort ascending and a sort descending class defined.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Class ListCollectionSorter2
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
Class ListCollectionDESorter2
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
Class ListCollectionIDSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper > item2.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper < item2.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListCollectionIDDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper < item2.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper > item2.SubItems(Main.MWSet.ColCollectionID).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListDaySorter2
    Implements IComparer
    Function GetDayIndex(ByVal Day As String) As Integer
        Dim Index As Integer
        If Day = "Tuesday" Then Index = 0
        If Day = "Wednesday" Then Index = 1
        If Day = "Thursday" Then Index = 2
        If Day = "Friday" Then Index = 3
        If Day = "Saturday" Then Index = 4
        If Day = "Sunday" Then Index = 5
        If Day = "Monday" Then Index = 6
        If Day = "Tuesday2" Then Index = 7
        If Day = "Wednesday2" Then Index = 8
        If Day = "Thursday2" Then Index = 9
        If Day = "Friday2" Then Index = 10
        If Day = "Saturday2" Then Index = 11
        If Day = "Sunday2" Then Index = 12
        If Day = "Monday2" Then Index = 13
        Return Index
    End Function
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim v1 As Integer = GetDayIndex(item1.SubItems(Main.MWSet.ColDay).ToString)
        Dim v2 As String = GetDayIndex(item2.SubItems(Main.MWSet.ColDay).ToString)

        If v1 > v2 Then
            Return 1
        Else
            If v1 < v2 Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListDayDESorter2
    Implements IComparer
    Function GetDayIndex(ByVal Day As String) As Integer
        Dim Index As Integer
        If Day = "Tuesday" Then Index = 0
        If Day = "Wednesday" Then Index = 1
        If Day = "Thursday" Then Index = 2
        If Day = "Friday" Then Index = 3
        If Day = "Saturday" Then Index = 4
        If Day = "Sunday" Then Index = 5
        If Day = "Monday" Then Index = 6
        If Day = "Tuesday2" Then Index = 7
        If Day = "Wednesday2" Then Index = 8
        If Day = "Thursday2" Then Index = 9
        If Day = "Friday2" Then Index = 10
        If Day = "Saturday2" Then Index = 11
        If Day = "Sunday2" Then Index = 12
        If Day = "Monday2" Then Index = 13

        Return Index
    End Function
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)

        Dim v1 As Integer = GetDayIndex(item1.SubItems(Main.MWSet.ColDay).ToString)
        Dim v2 As String = GetDayIndex(item2.SubItems(Main.MWSet.ColDay).ToString)

        If v1 < v2 Then
            Return 1
        Else
            If v1 > v2 Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListUTCStartSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColUTCStart).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColUTCStart).Text)
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
Class ListUTCStartDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColUTCStart).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColUTCStart).Text)
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

Class ListUTCEndSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColUTCEnd).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColUTCEnd).Text)
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
Class ListUTCEndDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColUTCEnd).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColUTCEnd).Text)
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
Class ListLocalStartSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColLocalStart).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColLocalStart).Text)
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
Class ListLocalStartDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColLocalStart).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColLocalStart).Text)
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
Class ListLocalEndSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColLocalEnd).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColLocalEnd).Text)
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
Class ListLocalEndDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        Dim d1 As Date = Date.Parse(item1.SubItems(Main.MWSet.ColLocalEnd).Text)
        Dim d2 As Date = Date.Parse(item2.SubItems(Main.MWSet.ColLocalEnd).Text)
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
Class ListDurationSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If CInt(item1.SubItems(Main.MWSet.ColDuration).ToString) > CInt(item2.SubItems(Main.MWSet.ColDuration).ToString) Then
            Return 1
        Else
            If CInt(item1.SubItems(Main.MWSet.ColDuration).ToString) < CInt(item2.SubItems(Main.MWSet.ColDuration).ToString) Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListDurationDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If CInt(item1.SubItems(Main.MWSet.ColDuration).ToString) < CInt(item2.SubItems(Main.MWSet.ColDuration).ToString) Then
            Return 1
        Else
            If CInt(item1.SubItems(Main.MWSet.ColDuration).ToString) > CInt(item2.SubItems(Main.MWSet.ColDuration).ToString) Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListStatusSorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.MWSet.ColStatus).ToString.ToUpper > item2.SubItems(Main.MWSet.ColStatus).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.MWSet.ColStatus).ToString.ToUpper < item2.SubItems(Main.MWSet.ColStatus).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListStatusDESorter2
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.MWSet.ColStatus).ToString.ToUpper < item2.SubItems(Main.MWSet.ColStatus).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.MWSet.ColStatus).ToString.ToUpper > item2.SubItems(Main.MWSet.ColStatus).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListClassSorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.MWSet.ColClass).ToString.ToUpper > item2.SubItems(Main.MWSet.ColClass).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.MWSet.ColClass).ToString.ToUpper < item2.SubItems(Main.MWSet.ColClass).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class
Class ListClassDESorter
    Implements IComparer
    Public Function CompareTo(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item1, item2 As ListViewItem
        item1 = CType(o1, ListViewItem)
        item2 = CType(o2, ListViewItem)
        If item1.SubItems(Main.MWSet.ColClass).ToString.ToUpper < item2.SubItems(Main.MWSet.ColClass).ToString.ToUpper Then
            Return 1
        Else
            If item1.SubItems(Main.MWSet.ColClass).ToString.ToUpper > item2.SubItems(Main.MWSet.ColClass).ToString.ToUpper Then
                Return -1
            Else
                Return 0
            End If
        End If
    End Function
End Class