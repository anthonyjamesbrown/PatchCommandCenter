Imports System.Management
Imports System.ComponentModel
Public Class HotfixSpotCheck
    Public MWFunc As New MWFunctions()
    Private Workers() As BackgroundWorker
    Private NumWorkers = 0

    Private Sub HotfixSpotCheck_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim I As Integer
        Dim ServerName As String
        If Main.ListView_MW.SelectedItems.Count > 0 Then 'Make sure at least 1 server was selected
            For I = 0 To Main.ListView_MW.SelectedItems.Count - 1 'Loop through the selected servers list.
                ServerName = Main.ListView_MW.SelectedItems.Item(I).Text
                If Not ServerName Is Nothing Then
                    Dim tempItem As New ListViewItem()
                    tempItem.Text = ServerName
                    tempItem.SubItems.Add("")
                    ListView_Serverlist.Items.Add(tempItem)
                End If
            Next
        Else
            Me.Close()
        End If
    End Sub
    Private Function Get_HotfixStatus(ByVal ComputerName As String, ByVal strHotFixID As String)
        Try
            Dim scope As ManagementScope
            scope = New ManagementScope("\\" & ComputerName & "\root\cimv2")
            scope.Connect()

            Dim query As ObjectQuery
            query = New ObjectQuery("SELECT HotfixID FROM Win32_QuickFixEngineering WHERE HotfixID='" & strHotFixID & "'")
            Dim searcher As ManagementObjectSearcher
            searcher = New ManagementObjectSearcher(scope, query)

            Dim queryCollection As ManagementObjectCollection
            queryCollection = searcher.Get()

            If queryCollection.Count = 0 Then
                Return False
            Else
                Return True
            End If
        Catch
            Return "Access Denied"
        End Try
    End Function
    Private Sub Bttn_HotfixQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bttn_HotfixQuery.Click
        ClearCurrentResults()
        Bttn_HotfixQuery.Enabled = False
        Dim HotfixID As String = Hotfix_TextBox.Text
        If Not HotfixID = "" Then
            Dim I As Integer
            Dim ServerName As String
            If ListView_Serverlist.Items.Count > 0 Then 'Make sure at least 1 server was selected.
                For I = 0 To ListView_Serverlist.Items.Count - 1 'Loop through the selected servers list.
                    ServerName = ListView_Serverlist.Items.Item(I).Text
                    ListView_Serverlist.Items(I).SubItems(1).Text = "Working..."
                    Dim Ping = MWFunc.PingHost(ServerName)
                    If Ping Then
                        Dim Value As String = ServerName & "|" & HotfixID
                        NumWorkers = NumWorkers + 1 'This is the index trackers for the number of BackgroundWorker threads in the Workers thread array.
                        ReDim Workers(NumWorkers) 'Add a worker thread entry.
                        Workers(NumWorkers) = New BackgroundWorker 'Create the new thread.
                        Workers(NumWorkers).WorkerReportsProgress = True 'Set the report progress property.
                        Workers(NumWorkers).WorkerSupportsCancellation = True 'Set the supports cancellation property.
                        AddHandler Workers(NumWorkers).DoWork, AddressOf WorkerDoWork 'Create a reference for the dowork event for this worker.
                        AddHandler Workers(NumWorkers).RunWorkerCompleted, AddressOf WorkerCompleted ' Create a reference for the work completed event.
                        Workers(NumWorkers).RunWorkerAsync(Value) 'Run the thread async.
                    End If
                Next
                ListView_Serverlist.Refresh()
            End If
        End If
    End Sub
    Private Sub WorkerDoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is the work completed by the BackgroundWorker threads for getting system uptime.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim ServerName As String
        Dim HotfixID As String
        ServerName = ((e.Argument.ToString).Split("|"))(0) 'Get the servername out of the passed Argument object.
        HotfixID = ((e.Argument.ToString).Split("|"))(1)
        Dim rslt As Boolean = Get_HotfixStatus(ServerName, HotfixID) 'Attempt to get the server uptime.
        e.Result = ServerName & "|" & rslt.ToString
    End Sub
    Private Sub WorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is run as an Event Handler for when the server uptime backgroundworker completes.  This sub will handle updating the cache
        '  and UI with the uptime results.  This sub will call Update_ListViewUptime sub to update the UI.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim ServerName As String = (e.Result.ToString.Split("|"))(0)
        Dim TSResult As String = (e.Result.ToString.Split("|"))(1)
        Dim Index2 As Integer = (ListView_Serverlist.FindItemWithText(ServerName, False, 0)).Index
        ListView_Serverlist.Items(Index2).SubItems(1).Text = TSResult
        CheckCompleted()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim I As Integer
        For I = 1 To NumWorkers - 1
            If Workers(I).IsBusy Then
                Workers(I).CancelAsync()
            End If
        Next
    End Sub
    Private Sub CheckCompleted()
        Dim Checked As Boolean = True
        Dim I As Integer
        Dim Status As String
        For I = 0 To ListView_Serverlist.Items.Count - 1
            Status = ListView_Serverlist.Items(I).SubItems(1).Text
            If Status = "Working..." Then
                Checked = False
                Exit For
            End If
        Next
        If Checked Then
            Bttn_HotfixQuery.Enabled = True
        End If
    End Sub
    Private Sub ClearCurrentResults()
        Dim I As Integer
        For I = 0 To ListView_Serverlist.Items.Count - 1
            ListView_Serverlist.Items(I).SubItems(1).Text = ""
        Next
    End Sub
End Class