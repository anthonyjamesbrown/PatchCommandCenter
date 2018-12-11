Public Class CustomMW
    Public HasChanged As Boolean = False

    Private Sub CustomMW_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TextBox_CollectionName.Text = Main.MWSet.ListView1.SelectedItems(0).Text
        TextBox_Timezone.Text = Main.MWSet.Label_Timezone.Text
        TextBox_Day.Text = Main.MWSet.ListView1.SelectedItems(0).SubItems(2).Text
        TextBox_LocalStartTime.Text = Main.MWSet.ListView1.SelectedItems(0).SubItems(3).Text
        TextBox_LocalEndTime.Text = Main.MWSet.ListView1.SelectedItems(0).SubItems(4).Text
        TextBox_UTCStartTime.Text = Main.MWSet.ListView1.SelectedItems(0).SubItems(5).Text
        TextBox_UTCEndTime.Text = Main.MWSet.ListView1.SelectedItems(0).SubItems(6).Text
        TextBox_Duration.Text = Main.MWSet.ListView1.SelectedItems(0).SubItems(7).Text

        DateTimePicker_Adjusted.Text = TextBox_UTCStartTime.Text
        NumericUpDown_NewDuration.Value = CInt(Main.MWSet.ListView1.SelectedItems(0).SubItems(7).Text)
        HasChanged = False
    End Sub
    Private Sub Bttn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bttn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub NumericUpDown_NewDuration_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NumericUpDown_NewDuration.ValueChanged, DateTimePicker_Adjusted.ValueChanged
        Dim Day As String
        Dim UTCStart As Date
        Dim UTCEnd As Date
        Dim LocalStart As Date
        Dim LocalEnd As Date

        UTCStart = DateTimePicker_Adjusted.Value
        UTCEnd = UTCStart.AddHours(NumericUpDown_NewDuration.Value)
        Day = UTCStart.DayOfWeek.ToString
        LocalStart = UTCStart.ToLocalTime()
        LocalEnd = UTCEnd.ToLocalTime()

        TextBox_Day.Text = Day
        TextBox_LocalStartTime.Text = LocalStart
        TextBox_LocalEndTime.Text = LocalEnd
        TextBox_UTCStartTime.Text = UTCStart
        TextBox_UTCEndTime.Text = UTCEnd
        TextBox_Duration.Text = NumericUpDown_NewDuration.Value
        HasChanged = True
    End Sub

    Private Sub Bttn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bttn_OK.Click
        If HasChanged = True Then
            Main.MWSet.ListView1.SelectedItems(0).SubItems(2).Text = TextBox_Day.Text
            Main.MWSet.ListView1.SelectedItems(0).SubItems(5).Text = TextBox_LocalStartTime.Text
            Main.MWSet.ListView1.SelectedItems(0).SubItems(6).Text = TextBox_LocalEndTime.Text
            Main.MWSet.ListView1.SelectedItems(0).SubItems(3).Text = TextBox_UTCStartTime.Text
            Main.MWSet.ListView1.SelectedItems(0).SubItems(4).Text = TextBox_UTCEndTime.Text
            Main.MWSet.ListView1.SelectedItems(0).SubItems(7).Text = TextBox_Duration.Text
            Me.Close()
        End If
    End Sub
End Class