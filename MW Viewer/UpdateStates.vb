Imports System.Management
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the Get Update States in the user interface.  That interface is used to display information about updates
'   that have been targeted to a server.  This is a real time call to the server using WMI.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class UpdateStates
    Public ServerName As String

    Private Sub UpdateStates_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This handles the load event.  Currently it only sets the Servername in the UI.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        TextBox_ServerName.Text = ServerName
    End Sub

    Private Sub UpdateStates_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the UI Shown event.  This sub populates the datagrid with the update information.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        DataGridView_Updates.Rows.Clear()
        GetUpdatesStatus(ServerName)
    End Sub

    Public Sub GetUpdatesStatus(ByVal ServerName As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles connecting to the remote server over WMI and pulling a list of targeted updates with thier current state.  This
        '  data is then used to populate the gridview.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        StripLabel1.Text = "Working..."
        Try
            'Connect to the remote computer
            Dim ms As New ManagementScope("\\" & ServerName & "\ROOT\CCM\ClientSDK")
            'Query remote computer across the connection
            Dim oq As New ObjectQuery("SELECT * FROM CCM_SoftwareUpdate")
            Dim query1 As New ManagementObjectSearcher(ms, oq)
            Dim queryCollection1 As ManagementObjectCollection = query1.[Get]()

            For Each mo As ManagementObject In queryCollection1
                Dim ArticleID As String = mo("ArticleID")
                Dim BulletinID As String = mo("BulletinID")
                Dim Deadline As Date = Main.MWFunc.CovertCIMtoDate(mo("Deadline"))
                Dim Description As String = mo("Description")
                Dim ErrorCode As String = mo("ErrorCode").ToString()
                Dim EvaluationState As Integer = mo("EvaluationState")
                Dim Name As String = mo("Name")
                Dim StartTime As Date = Main.MWFunc.CovertCIMtoDate(mo("StartTime"))

                Dim n As Integer = DataGridView_Updates.Rows.Add()
                DataGridView_Updates.Rows.Item(n).Cells(0).Value = ArticleID
                DataGridView_Updates.Rows.Item(n).Cells(1).Value = BulletinID
                DataGridView_Updates.Rows.Item(n).Cells(2).Value = Name
                DataGridView_Updates.Rows.Item(n).Cells(3).Value = StartTime
                DataGridView_Updates.Rows.Item(n).Cells(4).Value = Deadline
                DataGridView_Updates.Rows.Item(n).Cells(5).Value = Description
                DataGridView_Updates.Rows.Item(n).Cells(6).Value = GetEvaluationState(EvaluationState)
                DataGridView_Updates.Rows.Item(n).Cells(7).Value = GetErrorMessageText(ErrorCode)

                DataGridView_Updates.Rows.Item(n).Cells(6).Style.BackColor = GetStateColorCode(EvaluationState)
            Next
            StripLabel1.Text = "(" & queryCollection1.Count & ") updates found."
        Catch
            Dim n As Integer = DataGridView_Updates.Rows.Add()
            StripLabel1.Text = "Can't connect to the remote system.  This operation requires local admin access to the remote computer."
        End Try

    End Sub

    Private Function GetEvaluationState(ByVal EvaluationState As Integer) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the friendly Evaluation State from a Integer identifier
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Type As String = ""
        Select Case EvaluationState
            Case 0
                Type = "Past Due, will be installed."
            Case 1
                Type = "Available"
            Case 2
                Type = "Submitted"
            Case 3
                Type = "Detecting"
            Case 4
                Type = "Pre download"
            Case 5
                Type = "Downloading"
            Case 6
                Type = "Wait Install"
            Case 7
                Type = "Installing"
            Case 8
                Type = "Pending soft reboot"
            Case 9
                Type = "Pending hard reboot"
            Case 10
                Type = "Wait reboot"
            Case 11
                Type = "Verifying"
            Case 12
                Type = "Install complete"
            Case 13
                Type = "Error"
            Case 14
                Type = "Wait Service Window"
            Case 15
                Type = "Wait user logon"
            Case 16
                Type = "Wait user logoff"
            Case 17
                Type = "Wait job user logon"
            Case 18
                Type = "Wait user reconnect"
            Case 19
                Type = "Pending user logoff"
            Case 20
                Type = "Pending update"
            Case 21
                Type = "Waiting retry"
            Case 22
                Type = "Wait pres mode off"
            Case 23
                Type = "Wait for orchestration"
        End Select
        Return Type
    End Function

    Function GetStateColorCode(ByVal EvalState As Integer) As Color
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns color to use in the datagrid cell for Evaluation State.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Value As Color
        Select Case EvalState
            Case 0
                Value = Color.LightBlue
            Case 1, 2, 3, 4, 5, 6, 7
                Value = Color.Green
            Case 8, 9
                Value = Color.Yellow
            Case 13
                Value = Color.Red
            Case Else
                Value = Color.White
        End Select
        Return Value
    End Function

    Function GetErrorMessageText(ByVal ErrorCode As Long) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the friendly Error Message from a Integer identifier
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Msg As String
        Select Case ErrorCode
            Case 2147942512, 3355445008
                Msg = "Not enough disk space"
            Case 2278557265
                Msg = "Post install scan failed"
            Case 2278557288
                Msg = "Software update still detected as actionable after apply"
            Case 2147943860
                Msg = "This operation returned because the timeout period expired."
            Case Else
                Msg = ErrorCode
        End Select
        Return Msg
    End Function

    Private Sub Button_Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Refresh.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles refreshing the data in the datagrid.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        DataGridView_Updates.Rows.Clear()
        GetUpdatesStatus(ServerName)
    End Sub

End Class