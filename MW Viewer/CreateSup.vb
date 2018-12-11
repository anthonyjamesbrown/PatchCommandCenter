Imports System.Globalization
Imports Microsoft.ConfigurationManagement.ManagementProvider
Imports Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine
Imports Microsoft.Office.Interop

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the Create Software Update Group UI.
'
' This form handles several actions.  First, it will return a group of updates based on the normal criteria for a specified time period.
'  the default is all new updates in the last 14 days but this is selectable through a dropdown on the UI.  This is intended to show
'  which updates would be in scope each month.
' 
' After the updates have been scoped the user can click the 'Create SUG' button to perform the following actions:
'   - Create a new Software Update Package that contains the Updates that were populated by the 'Get Updates' sub.
'   - Create a new directory to hold the package source.
'   - Download the update content for each update into a new child directory for each in the parent directory that was created in the previous step
'   - Create a new Software Update Package.
'   - Add content for each downloaded update to the new Software Update Package.
'   - Clean up the downloaded content directories after the content was added to the package.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class CreateSup

    Dim ColCI_ID As Integer = 0
    Dim ColArticleID As Integer = 1
    Dim ColBulletinID As Integer = 2
    Dim ColTitle As Integer = 3
    Dim ColDateRevised As Integer = 4
    Dim ColURL As Integer = 5
    Dim ColSeverity As Integer = 6
    Dim ColCount As Integer = 6

    Private Sub Button_GetUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_GetUpdates.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub queries the CM Site server for updates and populates the listview in the UI.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Button_GetUpdates.Enabled = False
        Main.RecordLabel.Text = "Working..."

        Dim CurrDate As Date = Now()
        Dim SupName As String = "Server Updates - " & CurrDate.ToString("MMMM", New CultureInfo("en-US")) & " " & CurrDate.Year.ToString & " (Automation Created)"
        Dim PackName As String = "ServerUpdatePackage-" & CurrDate.Year.ToString & "-" & CurrDate.ToString("MMMM", New CultureInfo("en-US"))

        TextBox_SupName.Text = SupName
        TextBox_PackageName.Text = PackName

        Dim Days As Integer = NumericUpDown_Days.Value

        ' Call function to get updates.
        Dim Updates As IResultObject = Main.MWFunc.GetSoftwareUpdates(Days)

        ' Populate the Listview in the UI from the query results.
        For Each update As IResultObject In Updates
            If update("IsSuperseded").BooleanValue = False And update("LocalizedDisplayName").StringValue.Contains(".NET") = False Then
                Dim TempItem As New ListViewItem()
                Dim x As Integer
                TempItem.Text = update("CI_ID").StringValue
                For x = 1 To ColCount
                    TempItem.SubItems.Add("")
                Next

                TempItem.SubItems(ColArticleID).Text = update("ArticleID").StringValue
                TempItem.SubItems(ColBulletinID).Text = update("BulletinID").StringValue
                TempItem.SubItems(ColTitle).Text = update("LocalizedDisplayName").StringValue
                TempItem.SubItems(ColDateRevised).Text = Main.MWFunc.CovertCIMtoDate(update("DateRevised").StringValue)
                TempItem.SubItems(ColURL).Text = update("LocalizedInformativeURL").StringValue
                TempItem.SubItems(ColSeverity).Text = update("SeverityName").StringValue

                ListView_Updates.Items.Add(TempItem)
            End If
        Next

        Main.RecordLabel.Text = ListView_Updates.Items.Count & " Updates Returned."
        Button_GetUpdates.Enabled = True
        Button_CreateSUG.Enabled = True
    End Sub

    Private Sub Button_CreateSUG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CreateSUG.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles all of the actions for the following process:
        '   - Create a new Software Update Package that contains the Updates that were populated by the 'Get Updates' sub.
        '   - Create a new directory to hold the package source.
        '   - Download the update content for each update into a new child directory for each in the parent directory that was created in the previous step
        '   - Create a new Software Update Package.
        '   - Add content for each downloaded update to the new Software Update Package.
        '   - Clean up the downloaded content directories after the content was added to the package.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' Get the Software update group name and package name from the text boxes in the UI.
        Dim SUGName As String = TextBox_SupName.Text
        Dim PackageName As String = TextBox_PackageName.Text

        ' Query the download path from the registry.
        Dim DLDownLoadPath As String = Main.MWFunc.GetVarValue("UpdatesDownloadPath") & PackageName

        Dim SUGExists, PackageExists, DirExists As Boolean

        ' Check to see if the Software Update Group, Package, and Download Directory all do not already exist.
        SUGExists = Main.MWFunc.SUG_Exists(SUGName)
        PackageExists = Main.MWFunc.UpdatePackage_Exists(SUGName)
        DirExists = System.IO.Directory.Exists(DLDownLoadPath)

        If (SUGExists = False And PackageExists = False And DirExists = False) Then

            GroupBox2.Enabled = False
            Button_CreateSUG.Enabled = False

            Dim Description As String = "Created by AB Automation"

            ' Create the software Update Group
            ' Update the UI Status
            Label_status.Text = "Operation 1 of 7 : Creating Software Update Group"
            Dim MyConnection As New WqlConnectionManager
            MyConnection.Connect(Main.MWFunc.GetVarValue("CMServerFQDN"))

            Dim newDescriptionInfo As New List(Of IResultObject)()
            Dim SMSCILocalizedProperties As IResultObject = MyConnection.CreateEmbeddedObjectInstance("SMS_CI_LocalizedProperties")

            ' Populate the initial array values (this could be a loop to added more localized info).
            SMSCILocalizedProperties("Description").StringValue = Description
            SMSCILocalizedProperties("DisplayName").StringValue = SUGName
            SMSCILocalizedProperties("InformativeURL").StringValue = ""
            SMSCILocalizedProperties("LocaleID").StringValue = "1033"

            ' Add the 'embedded properties' to newDescriptionInfo.
            newDescriptionInfo.Add(SMSCILocalizedProperties)

            Dim Test As Integer = ListView_Updates.Items(0).Text.ToString

            ' Create the array of CI_IDs.
            Dim newCI_ID As Integer()
            Dim x As Integer
            Dim n As Integer = ListView_Updates.Items.Count

            ReDim newCI_ID(n - 1)

            For x = 0 To n - 1
                newCI_ID(x) = ListView_Updates.Items(x).Text.ToString
            Next

            ProgressBar_DL.Maximum = n
            ProgressBar_DL.Value = 0

            ' Build the file download list
            Label_status.Text = "Operation 2 of 7 : Building Download File List"
            Dim ContentFileList As New List(Of ListViewItem)
            For x = 0 To n - 1
                ProgressBar_DL.Increment(1)
                Label_Progress.Text = "Gathering Details: " & x + 1 & " of " & n
                Dim CI_ID As Integer = newCI_ID(x)

                Try
                    Dim query As String = "Select ContentID,ContentUniqueID,ContentLocales from SMS_CITOContent Where CI_ID='" & CI_ID & "'"

                    Dim ContentIDs As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(query)

                    For Each update As IResultObject In ContentIDs
                        Dim Locale = update("ContentLocales").StringArrayValue

                        If Locale.Contains("Locale:0") Or Locale.Contains("Locale:9") Then
                            Dim query2 As String = "Select FileName,SourceURL from SMS_CIContentfiles WHERE ContentID='" & update("ContentID").IntegerValue & "'"
                            Dim ContentFiles As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(query2)

                            For Each File As IResultObject In ContentFiles
                                Dim tempItem As New ListViewItem
                                Dim ContentID As Integer = update("ContentID").IntegerValue
                                Dim FolderPath As String = DLDownLoadPath & "\" & ContentID.ToString
                                Dim Filename As String = File("FileName").StringValue
                                Dim SourceURL As String = File("SourceURL").StringValue
                                Dim Destination As String = FolderPath & "\" & Filename

                                tempItem.Text = ContentID.ToString
                                tempItem.SubItems.Add(Filename)
                                tempItem.SubItems.Add(Destination)
                                tempItem.SubItems.Add(SourceURL)
                                tempItem.SubItems.Add(FolderPath)

                                ContentFileList.Add(tempItem)
                            Next
                        End If
                    Next

                Catch ex As SmsException
                    MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Throw
                End Try
            Next

            ' Call the CreateSUMUpdateList function.
            Main.MWFunc.CreateSUMUpdateList(newCI_ID, newDescriptionInfo)

            ' If the Downloadpath directory does not exist create it.
            If (Not System.IO.Directory.Exists(DLDownLoadPath)) Then
                System.IO.Directory.CreateDirectory(DLDownLoadPath)
            End If

            ' Download each update.
            n = ContentFileList.Count
            ProgressBar_DL.Maximum = n
            ProgressBar_DL.Value = 0
            Label_status.Text = "Operation 3 of 7 : Downloading Updates"
            Label_Progress.Text = "..."
            Me.GroupBox1.Refresh()
            For x = 0 To n - 1
                Label_Progress.Text = "Downloading: " & ContentFileList.Item(x).SubItems(1).Text & "  (" & x + 1 & " of " & n & ")"
                ProgressBar_DL.Increment(1)
                Me.GroupBox1.Refresh()
                Dim Current_CI_ID As Integer = ContentFileList.Item(x).Text
                Dim Source As String = ContentFileList.Item(x).SubItems(3).Text
                Dim Destination As String = ContentFileList.Item(x).SubItems(2).Text
                Dim SourcePath As String = DLDownLoadPath & "\" & Current_CI_ID

                Main.MWFunc.DownloadUpdate(Current_CI_ID, Source, Destination, SourcePath)
            Next
            Label_status.Text = "Operation 3 of 7 : Downloading Updates Completed"
            Label_Progress.Text = "..."
            Me.GroupBox1.Refresh()

            ' Create the Deployment Package.
            ProgressBar_DL.Value = 0
            Label_status.Text = "Operation 4 of 7 : Creating Deployment Package"
            Dim PackageID As String = Main.MWFunc.CreateSUMDeploymentPackage(SUGName, Description, 2, DLDownLoadPath)
            ProgressBar_DL.Increment(n)
            Me.GroupBox1.Refresh()

            ' Define the array of source paths (these must be UNC) to load into addUpdateContentParameters.
            Dim newArrayContentSourcePath As String()

            Dim newContentIDs As Integer()

            n = ContentFileList.Count

            ReDim newContentIDs(n - 1)
            ReDim newArrayContentSourcePath(n - 1)

            ' Create the deployment package content list.
            ProgressBar_DL.Value = 0
            Label_status.Text = "Operation 5 of 7 : Creating Deployment Package Content List"
            Label_Progress.Text = "..."
            For x = 0 To n - 1
                Label_Progress.Text = "Processing: " & "  (" & x + 1 & " of " & n & ")"
                Me.GroupBox1.Refresh()
                ProgressBar_DL.Increment(1)
                newContentIDs(x) = ContentFileList.Item(x).Text
                newArrayContentSourcePath(x) = ContentFileList.Item(x).SubItems(4).Text
            Next

            ' Load the update content parameters into an object to pass to the method.
            Dim addUpdateContentParameters As New Dictionary(Of String, Object)()
            addUpdateContentParameters.Add("ContentIds", newContentIDs)
            addUpdateContentParameters.Add("ContentSourcePath", newArrayContentSourcePath)
            addUpdateContentParameters.Add("bRefreshDPs", True)

            ' Add content the deployment package
            ProgressBar_DL.Value = 0
            Label_status.Text = "Operation 6 of 7 : Adding content to the Deployment Package"
            Label_Progress.Text = "..."
            Me.GroupBox1.Refresh()

            Main.MWFunc.AddUpdatestoSUMDeploymentPackage(PackageID, addUpdateContentParameters)
            ProgressBar_DL.Increment(n)

            ' Clean up the temporary directories that were created when they were downloaded.
            ProgressBar_DL.Value = 0
            Label_status.Text = "Operation 7 of 7 : Cleaning up download directories"
            Label_Progress.Text = "..."
            For x = 0 To n - 1
                Dim Path As String = newArrayContentSourcePath(x)
                Label_Progress.Text = "Deleting " & Path & "  (" & x + 1 & " of " & n & ")"
                ProgressBar_DL.Increment(1)
                Me.GroupBox1.Refresh()
                System.IO.Directory.Delete(Path, True)
            Next

            Label_status.Text = "All operations completed!"
            Label_Progress.Text = ""
            Me.GroupBox1.Refresh()

            Dim MsgText As String = "Software Update Group: " & SUGName & " has been created. " & vbCrLf & _
                                    "Software Deployment Package: " & PackageName & " (" & PackageID & ") has been created."
            MessageBox.Show(MsgText, "Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim MsgText As String = "The specified Software Update Group name or Update Package name " & _
                                    "or download directory already exists." & vbCrLf & vbCrLf & _
                                    " Software Udpate Group Exists   : " & SUGExists & vbCrLf & _
                                    " Deployment Package Exists      : " & PackageExists & vbCrLf & _
                                    " WindowsUpdate Directory Exists : " & DirExists

            MessageBox.Show(MsgText, "Warning...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub Button_Copy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Copy.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles copying ALL the data in the main ListView to the Clipboard including the Header names.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim buffer As New System.Text.StringBuilder
        For i As Integer = 0 To ListView_Updates.Columns.Count - 1
            buffer.Append(ListView_Updates.Columns(i).Text)
            buffer.Append(vbTab)
        Next
        buffer.Append(vbCrLf)
        For i As Integer = 0 To ListView_Updates.Items.Count - 1
            For j As Integer = 0 To ListView_Updates.Columns.Count - 1
                buffer.Append(ListView_Updates.Items(i).SubItems(j).Text)
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
        xlWorkSheet.Name = "Updates"

        xlWorkSheet.Cells(2, 1) = TextBox_SupName.Text & " Updates"
        xlWorkSheet.Range("A2:G2").Merge()

        Dim style As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle1")
        style.Font.Bold = True
        style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange)

        Dim style2 As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("NewStyle2")
        style2.Font.Bold = True
        style2.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray)

        xlWorkSheet.Range("A2:G2").Style = "NewStyle1"

        xlWorkSheet.Cells(3, 1) = "CI ID"
        xlWorkSheet.Cells(3, 2) = "Article ID"
        xlWorkSheet.Cells(3, 3) = "Bulletin ID"
        xlWorkSheet.Cells(3, 4) = "Title"
        xlWorkSheet.Cells(3, 5) = "Date Revised"
        xlWorkSheet.Cells(3, 6) = "URL"
        xlWorkSheet.Cells(3, 7) = "Severity"

        xlWorkSheet.Range("A3:G3").Style = "NewStyle2"
        xlWorkSheet.Range("A3:G3").Borders.Weight = 2
        xlWorkSheet.Range("A3:G3").HorizontalAlignment = -4108

        xlWorkSheet.Range("A3:G3").RowHeight = 25
        xlWorkSheet.Columns("A:G").ColumnWidth = 35
        xlWorkSheet.Columns("G").ColumnWidth = 110

        For i = 0 To ListView_Updates.Items.Count - 1

            xlWorkSheet.Cells(i + 4, 1) = ListView_Updates.Items.Item(i).Text.ToString
            xlWorkSheet.Cells(i + 4, 2) = ListView_Updates.Items.Item(i).SubItems(1).Text
            xlWorkSheet.Cells(i + 4, 3) = ListView_Updates.Items.Item(i).SubItems(2).Text
            xlWorkSheet.Cells(i + 4, 4) = ListView_Updates.Items.Item(i).SubItems(3).Text
            xlWorkSheet.Cells(i + 4, 5) = ListView_Updates.Items.Item(i).SubItems(4).Text
            xlWorkSheet.Cells(i + 4, 6) = ListView_Updates.Items.Item(i).SubItems(5).Text
            xlWorkSheet.Cells(i + 4, 7) = ListView_Updates.Items.Item(i).SubItems(6).Text

        Next

        xlWorkSheet.Range("A4:G" & ListView_Updates.Items.Count + 3).Borders.Weight = 2
        xlWorkSheet.Range("A4:G" & ListView_Updates.Items.Count + 3).HorizontalAlignment = -4108

        Dim CurrRow As Integer = ListView_Updates.Items.Count + 3

        Dim dlg As New SaveFileDialog
        dlg.Filter = "Excel Files (*.xlsx)|*.xlsx"
        dlg.FilterIndex = 1
        dlg.InitialDirectory = Main.MWFunc.GetVarValue("ChangeDirectoryPath")
        dlg.FileName = TextBox_SupName.Text & " Updates"
        Dim ExcelFile As String = """"
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            ExcelFile = dlg.FileName
            xlWorkSheet.SaveAs(ExcelFile)
        End If
        xlWorkBook.Close()
        xlApp.Quit()
    End Sub
End Class
