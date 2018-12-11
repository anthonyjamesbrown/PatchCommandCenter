Imports Microsoft.Office.Interop
Imports System.Globalization
Public Class ExcelExport

    Private Sub ExcelExport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        ProgressBar1.Value = 0
        Label2.Text = ""
        Label3.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sPath = "\\nasappfp01\budfar\changeControl\Security Patching\"
        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim i As Integer

        Dim IW_StartDate As Date = Now()
        Dim FileName As String = IW_StartDate.Year.ToString + " " + IW_StartDate.ToString("MMMM", New CultureInfo("en-US")) + " IW ServerList"

        xlApp = New Excel.Application
        xlWorkBook = xlApp.Workbooks.Add(misValue)
        xlWorkSheet = xlWorkBook.Sheets("sheet1")
        xlWorkSheet.Name = "IW Server List"

        xlWorkSheet.Cells(1, 1) = "Out of Patching"
        xlWorkSheet.Cells(1, 1).Interior.Color = System.Drawing.Color.FromArgb(255, 255, 204)
        xlWorkSheet.Cells(2, 1) = "Auto Reboot"
        xlWorkSheet.Cells(2, 1).Interior.Color = System.Drawing.Color.FromArgb(196, 215, 155)
        xlWorkSheet.Cells(3, 1) = "Manual Reboot"
        xlWorkSheet.Cells(3, 1).Interior.Color = System.Drawing.Color.FromArgb(141, 180, 226)
        xlWorkSheet.Range("A1:A3").Font.Bold = True
        xlWorkSheet.Range("A1:A3").Font.Size = 10
        xlWorkSheet.Range("A1:A3").Borders.Weight = 2

        xlWorkSheet.Range("A4:J4").RowHeight = 5
        xlWorkSheet.Range("A4:J4").Interior.Color = Color.Black

        xlWorkSheet.Cells(5, 1) = "Server Name"
        xlWorkSheet.Cells(5, 2) = "Server Description"
        xlWorkSheet.Cells(5, 3) = "OS"
        xlWorkSheet.Cells(5, 4) = "MW Description"
        xlWorkSheet.Cells(5, 5) = "Schedule (In CST)"
        xlWorkSheet.Cells(5, 6) = "SCCM Reboot Option"
        xlWorkSheet.Cells(5, 7) = "Manual Reboots Handled By"
        xlWorkSheet.Cells(5, 8) = "Ping Status"
        xlWorkSheet.Cells(5, 9) = "Uptime"
        xlWorkSheet.Cells(5, 10) = "Comments"

        xlWorkSheet.Range("A5:J5").RowHeight = 15
        xlWorkSheet.Range("A5:J5").Interior.Color = Color.Yellow
        xlWorkSheet.Range("A5:J5").Font.Bold = True
        xlWorkSheet.Range("A5:J5").Borders.Weight = 2

        xlWorkSheet.Columns("A:J").ColumnWidth = 25
        xlWorkSheet.Columns("B").ColumnWidth = 35
        xlWorkSheet.Columns("C").ColumnWidth = 30
        xlWorkSheet.Columns("D").ColumnWidth = 35
        xlWorkSheet.Columns("E:F").ColumnWidth = 30
        xlWorkSheet.Columns("J").ColumnWidth = 50

        Dim sServerName, sServerDesc, sOS, sMWDescData, sPatchingGroup, sPingStatus, sUptime As String
        Dim sMWDesc, sSchedule, sRebootTeam, ColorStatus As String
        Dim Index, N As Integer

        N = Main.ListView_MW.Items.Count - 1
        xlWorkSheet.Range("A" & i + 6 & ":J" & N + 4).Interior.Color = System.Drawing.Color.FromArgb(196, 215, 155)

        For i = 0 To N

            ProgressBar1.Value = (i / N) * 100
            Label3.Text = i & " of " & N & " (" & ((i / N) * 100).ToString("N2") & "%)"
            Me.Refresh()

            sServerName = Main.ListView_MW.Items.Item(i).Text.ToString
            sServerDesc = Main.ListView_MW.Items.Item(i).SubItems(Main.ColServerDescription).Text
            sOS = Main.ListView_MW.Items.Item(i).SubItems(Main.ColOS).Text
            sMWDescData = Main.ListView_MW.Items.Item(i).SubItems(Main.ColGroupDescription).Text
            If sMWDescData = "" Then sMWDescData = "||"
            sPatchingGroup = Main.ListView_MW.Items.Item(i).SubItems(Main.ColMemberOf).Text
            sPingStatus = Main.ListView_MW.Items.Item(i).SubItems(Main.ColPingStatus).Text
            sUptime = Main.ListView_MW.Items.Item(i).SubItems(Main.ColUptime).Text
            sMWDesc = sMWDescData.Split("|")(0).Trim()
            sSchedule = sMWDescData.Split("|")(1).Trim()
            sRebootTeam = Main.ListView_MW.Items.Item(i).SubItems(Main.ColDepartment).Text

            Label2.Text = sServerName

            ColorStatus = "Out of Patching"
            If sPatchingGroup.Contains("(Auto Reboot)") Then ColorStatus = "Auto Reboot"
            If sPatchingGroup.Contains("(Manual Reboot)") Then ColorStatus = "Manual Reboot"

            Index = i + 6

            Dim MyArray() As String
            MyArray = {sServerName, sServerDesc, sOS, sMWDesc, sSchedule, sPatchingGroup, sRebootTeam, sPingStatus, sUptime}
            xlWorkSheet.Range("A" & Index & ":I" & Index).Value = MyArray

            If ColorStatus = "Manual Reboot" Then xlWorkSheet.Range("A" & Index & ":J" & Index).Interior.Color = System.Drawing.Color.FromArgb(141, 180, 226)
            If ColorStatus = "Out of Patching" Then xlWorkSheet.Range("A" & Index & ":J" & Index).Interior.Color = System.Drawing.Color.FromArgb(255, 255, 204)

        Next
        xlWorkSheet.Range("A6:J" & N + 6).Borders.Weight = 2
        xlWorkSheet.Range("A6:J" & N + 6).Font.Size = 10
        xlWorkSheet.EnableAutoFilter = True
        xlWorkSheet.Range("A5:J" & N + 6).AutoFilter(Field:=1, [Operator]:=Excel.XlAutoFilterOperator.xlFilterValues)


        Dim dlg As New SaveFileDialog
        dlg.Filter = "Excel Files (*.xlsx)|*.xlsx"
        dlg.FilterIndex = 1
        dlg.InitialDirectory = sPath
        dlg.FileName = FileName
        Dim ExcelFile As String = """"
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            ExcelFile = dlg.FileName
            xlWorkSheet.SaveAs(ExcelFile)
        End If
        xlWorkBook.Close()
        xlApp.Quit()
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub
End Class