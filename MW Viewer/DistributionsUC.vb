Imports Microsoft.ConfigurationManagement.ManagementProvider
Imports Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the DistributionsUC form.  This form is used to return distribution data about Software Update
'  Deployment Packages.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class DistributionsUC

    Private Sub DistributionsUC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the page load.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' Populate the Package drop down in the GUI.
        PopulatePackageDropDown()
    End Sub

    Private Sub PopulatePackageDropDown()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub performs a lookup to get a list of Software Udpate Packages and then populate the Package Dropdown.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Connect to the CM Site server.
        Dim MyConnection As New WqlConnectionManager
        MyConnection.Connect(Main.MWFunc.GetVarValue("CMServerFQDN"))
        Dim listOfPackages As IResultObject
        ' Query for a list of packages.
        Try
            Dim query As String = "SELECT Name,PackageID FROM SMS_SoftwareUpdatesPackage Where Name like 'Server Updates%' And PackageType = 5 Order By Name Desc"
            listOfPackages = MyConnection.QueryProcessor.ExecuteQuery(query)
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try

        ' Populate the Dropdown items with the data returned from the query.
        For Each Item As IResultObject In listOfPackages
            ComboBox_Packagename.Items.Add(Item("Name").StringValue)
        Next
    End Sub

    Private Sub ComboBox_Packagename_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Packagename.SelectedIndexChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub keeps the Package ID text box updated when the Package name dropdown is changed.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim PackageName As String = ComboBox_Packagename.Text
        Dim PackageID As String = ""
        If PackageName <> "" Then
            TextBox_PackageID.Text = Main.MWFunc.GetPackageID(PackageName)
        End If
    End Sub

    Private Sub Button_GetStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_GetStatus.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles getting distribution status when the 'Get Status' button is clicked.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' Clear any previous data.
        DataGridView_PkgDetails.Rows.Clear()
        DataGridView_DPStatus.Rows.Clear()

        ' Get the PackageID from the UI.
        Dim PackageID As String = TextBox_PackageID.Text

        Dim PackageName, PackageSize, LastRefreshTime, PkgSourcePath, Priority, SourceDate, SourceSite, SourceVersion, StoredPkgVersion As String

        PackageName = ""
        PackageSize = ""
        LastRefreshTime = ""
        PkgSourcePath = ""
        Priority = ""
        SourceDate = ""
        SourceSite = ""
        SourceVersion = ""
        StoredPkgVersion = ""

        ' Connect and query the CM Site Server for package distribution details.
        Dim MyConnection As New WqlConnectionManager
        MyConnection.Connect(Main.MWFunc.GetVarValue("CMServerFQDN"))
        Dim PackageDetails As IResultObject
        Try
            Dim query As String = "SELECT PackageID,Name,PackageSize,LastRefreshTime,PkgSourcePath,Priority,SourceDate,SourceSite,SourceVersion,StoredPkgVersion FROM SMS_SoftwareUpdatesPackage Where PackageID ='" & PackageID & "'"
            PackageDetails = MyConnection.QueryProcessor.ExecuteQuery(query)

            For Each Item As IResultObject In PackageDetails
                PackageName = Item("Name").StringValue
                PackageSize = Item("PackageSize").StringValue
                LastRefreshTime = Item("LastRefreshTime").DateTimeValue.ToString()
                PkgSourcePath = Item("PkgSourcePath").StringValue
                Priority = Item("Priority").StringValue
                SourceDate = Item("SourceDate").DateTimeValue.ToString()
                SourceSite = Item("SourceSite").StringValue
                SourceVersion = Item("SourceVersion").StringValue
                StoredPkgVersion = Item("StoredPkgVersion").StringValue
            Next

        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try

        ' Fill out the Datagrid rows with the detailed info.
        Dim n As Integer = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Package ID"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = PackageID
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Package Name"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = PackageName
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Package Size (MB)"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = PackageSize / 1024
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Last Refresh Time"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = LastRefreshTime
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Package Source Path"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = PkgSourcePath
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Priority"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = Priority
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Source Date"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = SourceDate
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Source Site"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = SourceSite
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Source Version"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = SourceVersion
        n = DataGridView_PkgDetails.Rows.Add()
        DataGridView_PkgDetails.Rows.Item(n).Cells(0).Value = "Stored Package Version"
        DataGridView_PkgDetails.Rows.Item(n).Cells(1).Value = StoredPkgVersion

        ' Query and populate the distribution status data.  This is the status of the selected package and each targeted DP.
        Dim DPStatus As IResultObject
        Try
            Dim query As String = "SELECT * FROM SMS_DistributionPoint Where PackageID ='" & PackageID & "'"
            DPStatus = MyConnection.QueryProcessor.ExecuteQuery(query)

            For Each DP As IResultObject In DPStatus
                Dim x As Integer = DataGridView_DPStatus.Rows.Add()
                Dim ServerNALPath As String = DP("ServerNALPath").StringValue
                Dim Server As String = ServerNALPath.Split("\")(2)
                DataGridView_DPStatus.Rows.Item(x).Cells(0).Value = Server
                Dim Status As Integer = DP("Status").IntegerValue
                DataGridView_DPStatus.Rows.Item(x).Cells(1).Value = Status
                ' Color the cell back color based on the status data.
                If Status = 0 Then
                    DataGridView_DPStatus.Rows.Item(x).Cells(1).Style.BackColor = Color.Green
                Else
                    DataGridView_DPStatus.Rows.Item(x).Cells(1).Style.BackColor = Color.Red
                End If
                DataGridView_DPStatus.Rows.Item(x).Cells(2).Value = DP("RefreshNow").BooleanValue
                DataGridView_DPStatus.Rows.Item(x).Cells(3).Value = DP("SiteCode").StringValue
                DataGridView_DPStatus.Rows.Item(x).Cells(4).Value = DP("SourceSite").StringValue
                DataGridView_DPStatus.Rows.Item(x).Cells(5).Value = DP("LastRefreshTime").DateTimeValue
                DataGridView_DPStatus.Rows.Item(x).Cells(6).Value = DP("LastRefreshTime").DateTimeValue.ToLocalTime()
            Next

        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub
End Class
