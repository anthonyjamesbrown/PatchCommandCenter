Imports System.Management.ManagementDateTimeConverter
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class is the code for the Add MW form.  This form is used to add all of the needed items to use a new MW_ collection.
' This form will handle creating all of the following activities:
'   - Create AD Group
'   - Create CM Collection
'   - Create Dynamic membership rule on the new collection to query for membership in the new AD Group
'   - Create a new entry in the Collection Data xml file for the new collection
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class AddMW

    Private Sub ComboBox_MWType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_MWType.SelectedValueChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This section determines the path where the new collections should be created.  This code is useless at the moment since the
        '  SDK does not allow you to specify a folder location when new collections are created nor does it have a method to move the collection
        '  once it is created.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Label_Type.Text = ComboBox_MWType.Text
        TextBox_FullName.Text = "MW_" & TextBox_Name.Text & " " & ComboBox_MWType.Text
        If ComboBox_MWType.Text = "(Auto Reboot)" Then
            TextBox_FullPath.Text = "Overview\Device Collections\Server Patching\Maintenance Groups\Auto Reboot Groups"
        Else
            TextBox_FullPath.Text = "Overview\Device Collections\Server Patching\Maintenance Groups\Manual Reboot Groups"
        End If
    End Sub

    Private Sub TextBox_Name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_Name.TextChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub builds and populates the full name of the new collection.  It basically prepends the name with MW_.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        TextBox_FullName.Text = "MW_" & TextBox_Name.Text & " " & ComboBox_MWType.Text
    End Sub

    Private Sub TextBox_CollComment_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_CollComment.TextChanged, ComboBox_Day.TextChanged, TextBox_FormatTime.TextChanged, TextBox_Reboot.TextChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub builds the collection comment string based on UI selections.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        TextBox_ADDesc.Text = TextBox_CollComment.Text & " | " & ComboBox_Day.Text & " " & TextBox_FormatTime.Text & "," & NumericUpDown_Duration.Value.ToString & " | " & TextBox_Reboot.Text
    End Sub

    Private Sub NumericUpDown_Hour_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown_Hour.ValueChanged, NumericUpDown_Minutes.ValueChanged, ComboBox_AMPM.TextChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub builds the time string based on UI selectiongs.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Minutes As Integer = NumericUpDown_Minutes.Value
        TextBox_FormatTime.Text = NumericUpDown_Hour.Value.ToString & ":" & Minutes.ToString("D2") & " " & ComboBox_AMPM.Text
    End Sub

    Private Sub CheckBox_Citrix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Citrix.CheckedChanged
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub builds the collection path string based on UI selections.  As previously stated this is currently useless.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If CheckBox_Citrix.CheckState = CheckState.Checked Then
            TextBox_FullPath.Text = "Overview\Device Collections\Server Patching\Maintenance Groups\Citrix Patching Groups"
        Else
            If ComboBox_MWType.Text = "(Auto Reboot)" Then
                TextBox_FullPath.Text = "Overview\Device Collections\Server Patching\Maintenance Groups\Auto Reboot Groups"
            Else
                TextBox_FullPath.Text = "Overview\Device Collections\Server Patching\Maintenance Groups\Manual Reboot Groups"
            End If
        End If
    End Sub

    Private Sub Button_Create_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Create.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub handles the following actions:
        '   - Create AD Group
        '   - Create CM Collection
        '   - Create Dynamic membership rule on the new collection to query for membership in the new AD Group
        '   - Create a new entry in the Collection Data xml file for the new collection
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim GroupName As String = TextBox_FullName.Text
        Dim GroupDesc As String = TextBox_ADDesc.Text
        Dim CollComment As String = TextBox_CollComment.Text
        Dim tClass As String = ComboBox_Class.Text
        Dim LC As String = "PRI0057B" ' This is the CollectionID for 'ADM CS All Servers - New' this is used as the limiting collection for the new collection

        ' Build the query string for the dynamic membership rule on the new collection.
        Dim Query As String = "select SMS_R_SYSTEM.ResourceID,SMS_R_SYSTEM.ResourceType,SMS_R_SYSTEM.Name,SMS_R_SYSTEM.SMSUniqueIdentifier,SMS_R_SYSTEM.ResourceDomainORWorkgroup,SMS_R_SYSTEM.Client from SMS_R_System where SMS_R_System.SystemGroupName = ""HQDOMAIN\\" & GroupName & """"

        Dim CollectionID As String = ""

        ' Create the AD Group.
        If Main.MWFunc.CreateADGroup(GroupName, GroupDesc) Then
            ' If the AD Group creation was successful then check if the collection already exists
            If Main.MWFunc.Collection_Exists(GroupName) = False Then
                ' If the collection did not exist then create it.
                CollectionID = Main.MWFunc.CreateDynamicCollection(GroupName, CollComment, True, Query, GroupName, LC)
                If CollectionID <> "" Then
                    ' If the collection was created then add an entry in the Collection Data xml file.
                    Dim Day As String = ComboBox_Day.Text
                    Dim StartTime As String = TextBox_FormatTime.Text
                    Dim Duration As Integer = NumericUpDown_Duration.Value

                    Main.MWFunc.AddMWtoXML(GroupName, CollectionID, Day, StartTime, Duration, tClass)
                    MessageBox.Show("Operation Completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub closes this form.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Me.Close()
    End Sub
End Class