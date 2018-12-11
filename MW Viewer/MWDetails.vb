Imports Microsoft.ConfigurationManagement.ManagementProvider

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the MWDetails UI.  This UI gets detailed information about all of the Maintenance Windows assigned to 
'  the selected collection.  The calling form will set the properties for CollectionName and CollectionID before calling the .Show() 
'  method of this form.
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class MWDetails
    ' Both of these are set by the calling form.
    Public CollectionName As String
    Public CollectionID As String

    Private Sub MWDetails_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles initializing this form.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        TextBox_CollectionName.Text = CollectionName
        TextBox_CollectionID.Text = CollectionID

        ' Define an Object to hold the MW information.
        Dim MWs As New List(Of IResultObject)()
        ' Fill the object by calling the GetMWs function
        MWs = Main.MWFunc.GetMWs(CollectionID)

        ' Populate the Datagrid in the UI with the data from each item in the MWs list.
        For Each MW As IResultObject In MWs
            Dim n As Integer = DataGridView1.Rows.Add()
            Dim UTC As Date = (MW("StartTime").DateTimeValue).ToUniversalTime() ' Convert the Starttime from local to UTC.
            DataGridView1.Rows.Item(n).Cells(0).Value = MW("ServiceWindowID").StringValue
            DataGridView1.Rows.Item(n).Cells(1).Value = MW("Name").StringValue
            DataGridView1.Rows.Item(n).Cells(2).Value = MW("Description").StringValue
            DataGridView1.Rows.Item(n).Cells(3).Value = MW("IsEnabled").BooleanValue
            DataGridView1.Rows.Item(n).Cells(4).Value = MW("StartTime").DateTimeValue.ToString()
            DataGridView1.Rows.Item(n).Cells(5).Value = UTC
            DataGridView1.Rows.Item(n).Cells(6).Value = (MW("Duration").IntegerValue / 60) ' Convert the Duration value from minutes to hours.
            DataGridView1.Rows.Item(n).Cells(7).Value = MW("ServiceWindowSchedules").StringValue
            DataGridView1.Rows.Item(n).Cells(8).Value = GetMWType(MW("ServiceWindowType").IntegerValue)
            DataGridView1.Rows.Item(n).Cells(9).Value = MW("IsGMT").BooleanValue
            DataGridView1.Rows.Item(n).Cells(10).Value = GetRecurrenceType(MW("RecurrenceType").IntegerValue)
        Next
    End Sub

    Private Function GetRecurrenceType(ByVal ReccurrenceType As Integer) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the friendly Reccureence Type name from a Integer identifier
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Type As String = ""
        Select Case ReccurrenceType
            Case 1
                Type = "NONE"
            Case 2
                Type = "DAILY"
            Case 3
                Type = "WEEKLY"
            Case 4
                Type = "MONTHLYBYWEEKDAY"
            Case 5
                Type = "MONTHLYBYDATE"
        End Select
        Return Type
    End Function

    Private Function GetMWType(ByVal ServiceWindowType As Integer) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the friendly Service Window Type from a Integer identifier
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Type As String = ""
        Select Case ServiceWindowType
            Case 1
                Type = "All Deployments"
            Case 4
                Type = "Software Updates"
            Case 5
                Type = "Task Sequence"
        End Select
        Return Type
    End Function

    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub closes the form.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Me.Close()
    End Sub
End Class