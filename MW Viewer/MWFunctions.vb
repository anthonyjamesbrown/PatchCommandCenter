Imports System.Net
Imports System.Net.Mail
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports System.Runtime.InteropServices
Imports Microsoft.ConfigurationManagement.ManagementProvider
Imports Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine
Imports Microsoft.Win32

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' This class holds all of the sharded functions and subs for this project.  The Main form initializes a new instance of this
'  class called MWFunc.  All functions in other forms are referenced by calling Main.MWFunc.<function or sub name>(<params>)
'
' Instead of hard-coding Global Variables in this class most of the configuration data is stored in the Windows registry.
' Accessing this data can be achieved by using the getVarValue(<"Value Name">) function.
'
' Values are stored in 'HKCU\Software\ab utilities\Patch Command Center' of the HKEY_CURRENT_USER registry hive.
'  
' The registry values used are:
'   - CMServerFQDN              : The FQDN of the current primary Config Mgr site server.
'   - ChangeDirectoryPath       : The UNC path to the directory where the Collection Data xml file is located, also the default location where Excel Exports go.
'   - XMLFileName               : The Name of the Collection Data xml file.
'   - UpdatesDownloadPath       : The UNC path where the source for new Software Update packages is going to go.  This is also the temp download location for updates.
'   - SMTPServer                : The FQDN of a valid SMTP Relay server.
'   - SMTPMailTo                : The email address of the recipents that will recieve email from this tool.
'   - SMTPMailFrom              : The email address that emails will come from.
'   - LDAPPath                  : The LDAP path for the main domain where MW security groups reside. e.g.: DC=int,DC=asurion,DC=com.
'   - DomainNetBIOSName         : The NetBIOS (Short Name) Name of the main domain. e.g.: HQDomain.
'   - BaseOU                    : The Distinguished Name of the OU where new MW Security groups will be created.
'   - DomainList                : The List of Base OUs to scan for each domain.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class MWFunctions

    Public MyConnection As New WqlConnectionManager
    Public GroupDescription As New Dictionary(Of String, String)
    Public dataSet As DataSet = New DataSet

    Public Sub Initialize()
        ' Check to see if one of the registry values has been set, if not set the defaults.
        If GetVarValue("CMServerFQDN") = "" Or GetVarValue("CMServerFQDN") = "1" Then
            SetDefaults()
        End If

        ' Populate the Collection XML dataset by reading in MWData.xml file
        dataSet.ReadXml(GetVarValue("ChangeDirectoryPath") & GetVarValue("XMLFileName"))

        ' Check if in Admin mode then connect to the CM Site Server if you are.  MyConnection is used by many other functions in this class.
        If Main.CurrentUserRole = "Admin" Then
            MyConnection.Connect(GetVarValue("CMServerFQDN"))
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ' CM Functions
    '
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Public Function Connect(ByVal serverName As String, ByVal userName As String, ByVal userPassword As String) As WqlConnectionManager
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function handles making a connection to the SCCM management server
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            Dim namedValues As New SmsNamedValuesDictionary()
            Dim connection As New WqlConnectionManager(namedValues)

            If System.Net.Dns.GetHostName().ToUpper() = serverName.ToUpper() Then
                ' Connect to local computer.
                connection.Connect(serverName)
            Else
                ' Connect to remote computer.
                connection.Connect(serverName, userName, userPassword)
            End If

            Return connection
        Catch e As SmsException
            MessageBox.Show("Failed to Connect. Error: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Catch e As UnauthorizedAccessException
            MessageBox.Show("Failed to authenticate. Error: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function
    Public Function CreateDailyRecurringScheduleToken(ByVal hourDuration As Integer, ByVal daySpan As Integer, ByVal startTime As String, ByVal isGmt As Boolean) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns a string formatted as a Recurring Schedule Token.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            ' Connect to the Primary Site server
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            Dim recurInterval As IResultObject = MyConnection.CreateEmbeddedObjectInstance("SMS_ST_NonRecurring")

            ' Populate the schedule properties.
            recurInterval("DayDuration").IntegerValue = 0
            recurInterval("HourDuration").IntegerValue = hourDuration
            recurInterval("MinuteDuration").IntegerValue = 0
            recurInterval("StartTime").StringValue = startTime
            recurInterval("IsGMT").BooleanValue = isGmt

            ' Creating array to use as a parameters for the WriteToString method.
            Dim scheduleTokens As New List(Of IResultObject)()
            scheduleTokens.Add(recurInterval)

            ' Creating dictionary object to pass parameters to the WriteToString method.
            Dim inParams As New Dictionary(Of String, Object)()
            inParams("TokenData") = scheduleTokens

            ' Initialize the outParams object.
            Dim outParams As IResultObject = Nothing

            ' Call WriteToString method to decode the schedule token.
            outParams = MyConnection.ExecuteMethod("SMS_ScheduleMethods", "WriteToString", inParams)

            ' Output schedule token as an interval string.
            ' Note: The return value for this method is always 0, so this check is just best practice.
            If outParams("ReturnValue").IntegerValue = 0 Then
                Return outParams("StringData").StringValue
            End If
        Catch ex As SmsException
            Return ("Failed. Error: " + ex.InnerException.Message)
        End Try
        Return "0"
    End Function

    Public Function SUG_Exists(ByVal SUG_Name As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function checks to see is the passed Software Update Group exists or not.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim SUG_Result As IResultObject
        SUG_Result = MyConnection.QueryProcessor.ExecuteQuery("Select CI_ID, LocalizedDisplayName From SMS_AuthorizationList Where LocalizedDisplayName = '" & SUG_Name & "'")
        For Each Item As IResultObject In SUG_Result
            Dim Name As String = Item("LocalizedDisplayName").StringValue
            If Name = SUG_Name Then Return True
        Next
        Return False
    End Function

    Public Function Collection_Exists(ByVal Collection_Name As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function checks to see is the passed Collection Name exists or not.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        MyConnection.Connect(GetVarValue("CMServerFQDN"))
        Dim Collection_Result As IResultObject
        Collection_Result = MyConnection.QueryProcessor.ExecuteQuery("Select * From SMS_Collection Where Name = '" & Collection_Name & "'")
        For Each Item As IResultObject In Collection_Result
            Dim Name As String = Item("Name").StringValue
            If Name = Collection_Name Then Return True
        Next
        Return False
    End Function

    Public Function UpdatePackage_Exists(ByVal PackageName As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function checks to see is the passed Software Deployment Package exists or not.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim UpdatePackage_Result As IResultObject
        UpdatePackage_Result = MyConnection.QueryProcessor.ExecuteQuery("Select PackageID, Name From SMS_SoftwareUpdatesPackage Where Name = '" & PackageName & "'")
        For Each Item As IResultObject In UpdatePackage_Result
            Dim Name As String = Item("Name").StringValue
            If Name = PackageName Then Return True
        Next
        Return False
    End Function

    Public Function Deployment_Exists(ByVal CollectionID As String, ByVal SUGName As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function checks to see is the passed Software Update Deployment exists or not.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Query As String = "Select * From SMS_UpdatesAssignment Where TargetCollectionID = '" & CollectionID & "' and AssignmentName like '" & SUGName & "%'"
        Dim collectionSettingsInstance As IResultObject = Nothing

        ' Get the collection settings instance for the targetCollectionID.
        Dim allCollectionSettings As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(Query)

        ' Enumerate the allCollectionSettings collection (there should be just one item) and save the instance.
        For Each collectionSetting As IResultObject In allCollectionSettings
            collectionSettingsInstance = collectionSetting
        Next

        If collectionSettingsInstance Is Nothing Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function Deployment_Enabled(ByVal CollectionID As String, ByVal SUGName As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function checks to see is the passed Software Update Deployment is set to enabled or disabled.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Query As String = "Select * From SMS_UpdatesAssignment Where TargetCollectionID = '" & CollectionID & "' and AssignmentName like '" & SUGName & "%'"
        Dim collectionSettingsInstance As IResultObject = Nothing

        ' Get the collection settings instance for the targetCollectionID.
        Dim allCollectionSettings As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(Query)

        ' Enumerate the allCollectionSettings collection (there should be just one item) and save the instance.
        For Each collectionSetting As IResultObject In allCollectionSettings
            Return collectionSetting("Enabled").BooleanValue
        Next
        Return False
    End Function

    Public Function MW_Exists(ByVal targetCollectionID As String, ByVal MWName As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function checks to see is the passed Maintenance Window exists or not.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            ' Create an object to hold the collection settings instance (used to check whether a collection settings instance exists). 
            Dim collectionSettingsInstance As IResultObject = Nothing

            ' Get the collection settings instance for the targetCollectionID.
            Dim allCollectionSettings As IResultObject = MyConnection.QueryProcessor.ExecuteQuery("Select * from SMS_CollectionSettings where CollectionID='" & targetCollectionID & "'")

            ' Enumerate the allCollectionSettings collection (there should be just one item) and save the instance.
            For Each collectionSetting As IResultObject In allCollectionSettings
                collectionSettingsInstance = collectionSetting
            Next

            ' If a collection settings instance, output message that there are no maintenance windows.
            If collectionSettingsInstance Is Nothing Then
                Return False
            Else
                ' Create a new array list to hold the service window objects.
                Dim maintenanceWindowArray As New List(Of IResultObject)()

                ' Establish connection to collection settings instance associated with the Collection ID.
                Dim collectionSettings As IResultObject = MyConnection.GetInstance("SMS_CollectionSettings.CollectionID='" & targetCollectionID & "'")

                ' Populate the array list with the existing service window objects (from the target collection).
                maintenanceWindowArray = collectionSettings.GetArrayItems("ServiceWindows")

                ' Enumerate through the array list to access each maintenance window object and output specific properties for each object.
                For Each maintenanceWindow As IResultObject In maintenanceWindowArray
                    If maintenanceWindow("Name").StringValue = MWName Then Return True
                Next
            End If
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
        Return False
    End Function

    Public Function MW_Count(ByVal targetCollectionID As String) As Integer
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the count of how many Maintenance Windows are listed under a give Collection ID.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            ' Create an object to hold the collection settings instance (used to check whether a collection settings instance exists). 
            Dim collectionSettingsInstance As IResultObject = Nothing

            ' Get the collection settings instance for the targetCollectionID.
            Dim allCollectionSettings As IResultObject = MyConnection.QueryProcessor.ExecuteQuery("Select * from SMS_CollectionSettings where CollectionID='" & targetCollectionID & "'")

            ' Enumerate the allCollectionSettings collection (there should be just one item) and save the instance.
            For Each collectionSetting As IResultObject In allCollectionSettings
                collectionSettingsInstance = collectionSetting
            Next

            ' If a collection settings instance, output message that there are no maintenance windows.
            If collectionSettingsInstance Is Nothing Then
                Return False
            Else
                ' Create a new array list to hold the service window objects.
                Dim maintenanceWindowArray As New List(Of IResultObject)()

                ' Establish connection to collection settings instance associated with the Collection ID.
                Dim collectionSettings As IResultObject = MyConnection.GetInstance("SMS_CollectionSettings.CollectionID='" & targetCollectionID & "'")

                ' Populate the array list with the existing service window objects (from the target collection).
                maintenanceWindowArray = collectionSettings.GetArrayItems("ServiceWindows")

                Return maintenanceWindowArray.Count
            End If
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
        Return False
    End Function

    Public Function Last_MW_Set(ByVal targetCollectionID As String) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the name of the Maintenance Windows with a start time farest in the future from the list of MWs targeted to that Collection ID.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            ' Create an object to hold the collection settings instance (used to check whether a collection settings instance exists). 
            Dim collectionSettingsInstance As IResultObject = Nothing

            ' Get the collection settings instance for the targetCollectionID.
            Dim allCollectionSettings As IResultObject = MyConnection.QueryProcessor.ExecuteQuery("Select * from SMS_CollectionSettings where CollectionID='" & targetCollectionID & "'")

            ' Enumerate the allCollectionSettings collection (there should be just one item) and save the instance.
            For Each collectionSetting As IResultObject In allCollectionSettings
                collectionSettingsInstance = collectionSetting
            Next

            ' If a collection settings instance, output message that there are no maintenance windows.
            If collectionSettingsInstance Is Nothing Then
                Return False
            Else
                ' Create a new array list to hold the service window objects.
                Dim maintenanceWindowArray As New List(Of IResultObject)()

                ' Establish connection to collection settings instance associated with the Collection ID.
                Dim collectionSettings As IResultObject = MyConnection.GetInstance("SMS_CollectionSettings.CollectionID='" & targetCollectionID & "'")

                ' Populate the array list with the existing service window objects (from the target collection).
                maintenanceWindowArray = collectionSettings.GetArrayItems("ServiceWindows")

                ' Enumerate through the array list to access each maintenance window object and output specific properties for each object.
                Dim LastDate As Date = Date.Parse("1/1/1900")
                Dim NewestMW As String = ""
                For Each maintenanceWindow As IResultObject In maintenanceWindowArray
                    Dim MWName As String = maintenanceWindow("Name").StringValue
                    Dim StartTime As Date = maintenanceWindow("StartTime").DateTimeValue
                    If StartTime > LastDate Then
                        NewestMW = MWName
                        LastDate = StartTime
                    End If
                Next
                Return NewestMW
            End If
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
        Return False
    End Function

    Public Function GetMWs(ByVal targetCollectionID As String) As List(Of IResultObject)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns a list of all Maintenacne Windows defined for a given Collection ID.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            ' Create an object to hold the collection settings instance (used to check whether a collection settings instance exists). 
            Dim collectionSettingsInstance As IResultObject = Nothing

            ' Get the collection settings instance for the targetCollectionID.
            Dim allCollectionSettings As IResultObject = MyConnection.QueryProcessor.ExecuteQuery("Select * from SMS_CollectionSettings where CollectionID='" & targetCollectionID & "'")

            ' Enumerate the allCollectionSettings collection (there should be just one item) and save the instance.
            For Each collectionSetting As IResultObject In allCollectionSettings
                collectionSettingsInstance = collectionSetting
            Next

            ' If a collection settings instance, output message that there are no maintenance windows.
            If Not collectionSettingsInstance Is Nothing Then
                ' Create a new array list to hold the service window objects.
                Dim maintenanceWindowArray As New List(Of IResultObject)()

                ' Establish connection to collection settings instance associated with the Collection ID.
                Dim collectionSettings As IResultObject = MyConnection.GetInstance("SMS_CollectionSettings.CollectionID='" & targetCollectionID & "'")

                ' Populate the array list with the existing service window objects (from the target collection).
                maintenanceWindowArray = collectionSettings.GetArrayItems("ServiceWindows")

                Return maintenanceWindowArray
            End If
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
        Return Nothing
    End Function

    Public Function CreateDynamicCollection(ByVal newCollectionName As String, _
                                       ByVal newCollectionComment As String, _
                                       ByVal ownedByThisSite As Boolean, _
                                       ByVal query As String, _
                                       ByVal ruleName As String, _
                                       ByVal LimitToCollectionID As String) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function creates a new Collection using the properties passed.  Collections created by this function are dynamic and will need to be
        '  passed a valid WQL query as one of the parameters.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            ' Create new SMS_Collection object.
            Dim newCollection As IResultObject = MyConnection.CreateInstance("SMS_Collection")

            ' Populate the new collection object properties.
            newCollection("Name").StringValue = newCollectionName
            newCollection("Comment").StringValue = newCollectionComment
            newCollection("OwnedByThisSite").BooleanValue = ownedByThisSite
            newCollection("LimitToCollectionID").StringValue = LimitToCollectionID

            ' Save the new collection object and properties.
            ' In this case, it seems necessary to 'get' the object again to access the properties.
            newCollection.Put()
            newCollection.Get()

            ' Validate the query.
            Dim validateQueryParameters As New Dictionary(Of String, Object)
            validateQueryParameters.Add("WQLQuery", query)
            Dim result As IResultObject = MyConnection.ExecuteMethod("SMS_CollectionRuleQuery", "ValidateQuery", validateQueryParameters)

            ' Create query rule.        
            Dim newQueryRule As IResultObject = MyConnection.CreateInstance("SMS_CollectionRuleQuery")
            newQueryRule("QueryExpression").StringValue = query
            newQueryRule("RuleName").StringValue = ruleName

            ' Add the rule. Although not used in this sample, QueryID contains the query identifier.
            Dim addMembershipRuleParameters As New Dictionary(Of String, Object)
            addMembershipRuleParameters.Add("collectionRule", newQueryRule)
            'newCollection.ExecuteMethod("", newQueryRule)
            Dim queryID As IResultObject = newCollection.ExecuteMethod("AddMembershipRule", addMembershipRuleParameters)

            ' Start collection evaluator.
            newCollection.ExecuteMethod("RequestRefresh", Nothing)

            Return newCollection("CollectionID").StringValue

        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Function

    Public Sub CreateMaintenanceWindow(ByVal targetCollectionID As String, _
                                       ByVal newMaintenanceWindowName As String, _
                                       ByVal newMaintenanceWindowDescription As String, _
                                       ByVal newMaintenanceWindowServiceWindowSchedules As String, _
                                       ByVal newMaintenanceWindowIsEnabled As Boolean, _
                                       ByVal newMaintenanceWindowServiceWindowType As Integer)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub creates a new Maintenance Window on the given Collection ID using the passed parameters.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            ' Create an object to hold the collection settings instance (used to check whether a collection settings instance exists). 
            Dim collectionSettingsInstance As IResultObject = Nothing

            Dim allCollectionSettings As IResultObject = MyConnection.GetInstance("SMS_CollectionSettings.CollectionID='" & targetCollectionID & "'")
            ' Enumerate the allCollectionSettings collection (there should be just one item) and save the instance.
            For Each collectionSetting As IResultObject In allCollectionSettings
                collectionSettingsInstance = collectionSetting
            Next

            ' If a collection settings instance does not exist for the target collection, create one.
            If collectionSettingsInstance Is Nothing Then
                collectionSettingsInstance = MyConnection.CreateInstance("SMS_CollectionSettings")
                collectionSettingsInstance("CollectionID").StringValue = targetCollectionID
                collectionSettingsInstance.Put()
                collectionSettingsInstance.[Get]()
            End If

            ' Create a new array list to hold the service window object.
            Dim tempServiceWindowArray As New List(Of IResultObject)()

            ' Create and populate a temporary SMS_ServiceWindow object with the new maintenance window values.
            Dim tempServiceWindowObject As IResultObject = MyConnection.CreateEmbeddedObjectInstance("SMS_ServiceWindow")

            ' Populate temporary SMS_ServiceWindow object with the new maintenance window values.
            tempServiceWindowObject("Name").StringValue = newMaintenanceWindowName
            tempServiceWindowObject("Description").StringValue = newMaintenanceWindowDescription
            tempServiceWindowObject("ServiceWindowSchedules").StringValue = newMaintenanceWindowServiceWindowSchedules
            tempServiceWindowObject("IsEnabled").BooleanValue = newMaintenanceWindowIsEnabled
            tempServiceWindowObject("ServiceWindowType").IntegerValue = newMaintenanceWindowServiceWindowType

            ' Populate the local array list with the existing service window objects (from the target collection).
            tempServiceWindowArray = collectionSettingsInstance.GetArrayItems("ServiceWindows")

            ' Add the newly created service window object to the local array list.
            tempServiceWindowArray.Add(tempServiceWindowObject)

            ' Replace the existing service window objects from the target collection with the temporary array that includes the new service window.
            collectionSettingsInstance.SetArrayItems("ServiceWindows", tempServiceWindowArray)

            ' Save the new values in the collection settings instance associated with the target collection.
            collectionSettingsInstance.Put()
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub

    Public Sub CreateUpdateDeployment(ByVal newSoftwareUpdateGroupName As String, ByVal newTargetCollectionName As String, ByVal newTargetCollectionID As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub creates a new Software Update Deployment targeting the given collection and software update group name.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            Dim newNotifyUser As Boolean
            Dim newSuppressReboot As Integer
            Dim newUserUIExperience As Boolean

            Dim newStartTime As String = ConvertDatetoCIM(Date.Now())
            Dim newEnforcementDeadline As String = ConvertDatetoCIM(Date.Now().AddMinutes(5))

            If newTargetCollectionName.Contains("Auto Reboot") Then
                newSuppressReboot = 0
            Else
                newSuppressReboot = 2
            End If

            If newTargetCollectionName.Contains("Citrix") Then
                newNotifyUser = False
                newUserUIExperience = False
            Else
                newNotifyUser = True
                newUserUIExperience = True
            End If

            'Get array of Update AssignedCIs from Software Update Group Name
            Dim newArrayAssignedCIs As Integer() = GetAssignmentIDsFromSUG(newSoftwareUpdateGroupName)

            Dim newAssignedUpdateGroup As Integer = GetSUG_CI_ID(newSoftwareUpdateGroupName)

            ' Create the deployment object.
            Dim newSUMUpdatesAssignment As IResultObject = MyConnection.CreateInstance("SMS_UpdateGroupAssignment")

            ' Populate new deployment properties.
            newSUMUpdatesAssignment("AssignedCIs").IntegerArrayValue = newArrayAssignedCIs
            newSUMUpdatesAssignment("AssignedUpdateGroup").IntegerValue = newAssignedUpdateGroup
            newSUMUpdatesAssignment("AssignmentName").StringValue = newSoftwareUpdateGroupName & " - " & newTargetCollectionName
            newSUMUpdatesAssignment("EnforcementDeadline").StringValue = newEnforcementDeadline
            newSUMUpdatesAssignment("NotifyUser").BooleanValue = newNotifyUser
            newSUMUpdatesAssignment("StartTime").StringValue = newStartTime
            newSUMUpdatesAssignment("SuppressReboot").IntegerValue = newSuppressReboot
            newSUMUpdatesAssignment("TargetCollectionID").StringValue = newTargetCollectionID
            newSUMUpdatesAssignment("UserUIExperience").BooleanValue = newUserUIExperience

            newSUMUpdatesAssignment("ApplyToSubTargets").BooleanValue = False
            newSUMUpdatesAssignment("AssignmentAction").IntegerValue = 2
            newSUMUpdatesAssignment("AssignmentDescription").StringValue = ""
            newSUMUpdatesAssignment("AssignmentType").IntegerValue = 5
            newSUMUpdatesAssignment("DesiredConfigType").IntegerValue = 1
            newSUMUpdatesAssignment("DPLocality").IntegerValue = 131072
            newSUMUpdatesAssignment("Enabled").BooleanValue = False
            newSUMUpdatesAssignment("LocaleID").IntegerValue = 1033
            newSUMUpdatesAssignment("LogComplianceToWinEvent").BooleanValue = False
            newSUMUpdatesAssignment("OverrideServiceWindows").BooleanValue = False
            newSUMUpdatesAssignment("RaiseMomAlertsOnFailure").BooleanValue = False
            newSUMUpdatesAssignment("RebootOutsideOfServiceWindows").BooleanValue = False
            newSUMUpdatesAssignment("SendDetailedNonComplianceStatus").BooleanValue = False
            newSUMUpdatesAssignment("StateMessageVerbosity").IntegerValue = 10
            newSUMUpdatesAssignment("UseGMTTimes").BooleanValue = True
            newSUMUpdatesAssignment("WoLEnabled").BooleanValue = False

            ' Save new deployment and new deployment properties.
            newSUMUpdatesAssignment.Put()

        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub

    Public Function GetAssignmentIDsFromSUG(ByVal SoftwareUpdateGroupName As String) As Integer()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns an Array of Integers containing the CI_IDs of each update associated with the given Software Update Group Name.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            Dim CI_ID As Integer = GetSUG_CI_ID(SoftwareUpdateGroupName)

            Dim Updates As Integer()

            Dim SUGInstance As IResultObject = MyConnection.GetInstance((Convert.ToString("SMS_AuthorizationList.CI_ID=") & CI_ID))
            Updates = SUGInstance("Updates").IntegerArrayValue
            Return Updates

        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Function

    Public Function GetSUG_CI_ID(ByVal SoftwareUpdateGroupName As String) As Integer
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the CI_ID of a given Software Update Group Name.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))
            ' Get the specific SUM Deployment Package to change.
            Dim Query As String = "Select * from SMS_AuthorizationList Where LocalizedDisplayName = '" & SoftwareUpdateGroupName & "'"
            Dim SUGList As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(Query)

            Dim CI_ID As Integer

            ' This query should only return 1 result object
            For Each SUG As IResultObject In SUGList
                CI_ID = SUG("CI_ID").IntegerValue
                Return CI_ID
            Next

            Return 0
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try

    End Function

    Public Function GetSoftwareUpdateGroups() As IResultObject
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns a list of all Software Udpate Group Names.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))
            Dim query As String = "Select * from SMS_AuthorizationList"

            Dim listOfSUGs As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(query)

            Return listOfSUGs
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Function

    Public Function GetSoftwareUpdates(ByVal Days As Integer) As IResultObject
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns all of the Software Updates that meet the following criteria:
        '   - DateRevised in last <Days> Default is 14
        '   - Product IDs for:
        '       - Windows Server 2003
        '       - Windows Server 2003 DataCenter
        '       - Windows Server 2008 R2
        '       - Windows Server 2012 R2
        '       - SilverLight
        '   - Update Classification
        '       - Security Updates
        '       - Critical Updates
        '   - Custom Serverity not equal to 2, this translates to low in the CM Console.  This is used to mark updates as excluded
        '   - Update Name does not contain 'Itanium' or '.Net'
        '   - Update is not Superseded
        '   - update is not Expired
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            Dim DateValue As Date = Now().AddDays(Days * -1)

            Dim DateQuery As String = ConvertDatetoCIM(DateValue)

            Dim query As String = "Select * from SMS_SoftwareUpdate where DateRevised > '" & DateQuery & "'" & _
                                  " and " & _
                                  "((CategoryInstance_UniqueIDs like '%Product:9f3dd20a-1004-470e-ba65-3dc62d982958%' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:7f44c2a7-bc36-470b-be3b-c01b6dc5dd4e%' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:dbf57a08-0d5a-46ff-b30c-7715eb9498e9%' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:fdfe8200-9d98-44ba-a12a-772282bf60ef%' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:d31bd4c3-d872-41c9-a2e7-231f372588cb%')" & _
                                  " and " & _
                                  "(LocalizedCategoryInstanceNames like '%Critical Updates%' or LocalizedCategoryInstanceNames like '%Security Updates%'))" & _
                                  " and " & _
                                  "CustomSeverity <> 2 and not LocalizedDisplayName like '%Itanium-based%' and not LocalizedDisplayName like '%.NET%' and " & _
                                  "IsSuperseded = 0 and IsExpired = 0"

            ' Run query.
            Dim listOfResources1 As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(query)

            Return listOfResources1
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Function

    Public Function GetSoftwareUpdatesWorkstations(ByVal Days As Integer) As IResultObject
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns all of the Software Updates that meet the following criteria:
        '   - DateRevised in last <Days> Default is 14
        '   - Product IDs for:
        '       - Microsoft Lync 2010
        '       - Office 2003
        '       - Office 2007
        '       - Office 2010
        '       - Office 2013
        '       - Office 2016
        '       - SilverLight
        '       - Windows 7
        '       - Windows 8.1
        '       - Windows 10
        '   - Update Classification
        '       - Security Updates
        '   - Update is not Superseded
        '   - update is not Expired
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))

            Dim DateValue As Date = Now().AddDays(Days * -1)

            Dim DateQuery As String = ConvertDatetoCIM(DateValue)

            Dim query As String = "Select * from SMS_SoftwareUpdate where DateRevised > '" & DateQuery & "'" & _
                                  " and " & _
                                  "(CategoryInstance_UniqueIDs like '%Product:5e870422-bd8f-4fd2-96d3-9c5d9aafda22' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:1403f223-a63f-f572-82ba-c92391218055' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:041e4f9f-3a3d-4f58-8b2f-5e6fe95c4591' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:84f5f325-30d7-41c4-81d1-87a0e6535b66' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:704a0a4a-518f-4d69-9e03-10ba44198bd5' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:25aed893-7c2d-4a31-ae22-28ff8ac150ed' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:9f3dd20a-1004-470e-ba65-3dc62d982958' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:a3c2375d-0c8a-42f9-bce0-28333e198407' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:bfe5b177-a086-47a0-b102-097e4fa1f807' or " & _
                                  "CategoryInstance_UniqueIDs like '%Product:6407468e-edc7-4ecd-8c32-521f64cee65e')" & _
                                  " and " & _
                                  "LocalizedCategoryInstanceNames like '%Security Updates%')" & _
                                  " and IsSuperseded = 0 and IsExpired = 0"

            ' Run query.
            Dim listOfResources1 As IResultObject = MyConnection.QueryProcessor.ExecuteQuery(query)

            Return listOfResources1
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Function

    Public Sub CreateSUMUpdateList(ByVal newUpdates As Integer(), ByVal newDescriptionInfo As List(Of IResultObject))
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub creates a new Software Update Group.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))
            ' Create the new SMS_AuthorizationList object.
            Dim newUpdateList As IResultObject = MyConnection.CreateInstance("SMS_AuthorizationList")

            ' Populate the new SMS_AuthorizationList object properties.
            ' Updates is an int32 array that maps to the CI_ID in SMS_SoftwareUpdate.
            newUpdateList("Updates").IntegerArrayValue = newUpdates
            ' Pass embedded properties (LocalizedInformation) here.
            newUpdateList.SetArrayItems("LocalizedInformation", newDescriptionInfo)

            ' Save changes.
            newUpdateList.Put()

        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub

    Public Function CreateSUMDeploymentPackage(ByVal newPackageName As String, ByVal newPackageDescription As String, ByVal newPackageSourceFlag As Integer, ByVal newPackageSourcePath As String) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function creates a new Software Update Deployment Package.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))
            ' Create the new SUM package object.
            Dim newSUMDeploymentPackage As IResultObject = MyConnection.CreateInstance("SMS_SoftwareUpdatesPackage")

            ' Populate the new SUM package properties.
            newSUMDeploymentPackage("Name").StringValue = newPackageName
            newSUMDeploymentPackage("Description").StringValue = newPackageDescription
            newSUMDeploymentPackage("PkgSourceFlag").IntegerValue = newPackageSourceFlag
            newSUMDeploymentPackage("PkgSourcePath").StringValue = newPackageSourcePath

            ' Save the new SUM package and new package properties.
            newSUMDeploymentPackage.Put()
            newSUMDeploymentPackage.Get()
            ' Return the ID of the new package.
            Dim ID As String = newSUMDeploymentPackage("PackageID").StringValue
            Return ID
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Function

    Public Sub DownloadUpdate(ByVal CI_ID As Integer, ByVal Source As String, ByVal Destination As String, ByVal FolderPath As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub downloads a Software Update.  The Update URL (download location) is passed in as well as the destination.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If (Not System.IO.Directory.Exists(FolderPath)) Then
            System.IO.Directory.CreateDirectory(FolderPath)
        End If

        Dim wc As New WebClient()
        wc.DownloadFile(Source, Destination)

    End Sub

    Public Sub AddUpdatestoSUMDeploymentPackage(ByVal existingSUMPackageID As String, ByVal addUpdateContentParameters As Dictionary(Of String, Object))
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub adds Updates to an existing Software Update Deployment Package.  The update package is created in another step with no content.
        '  Then this sub is called once for each update to add to that new package.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            MyConnection.Connect(GetVarValue("CMServerFQDN"))
            ' Get the specific SUM Deployment Package to change.
            Dim existingSUMDeploymentPackage As IResultObject = MyConnection.GetInstance((Convert.ToString("SMS_SoftwareUpdatesPackage.PackageID='") & existingSUMPackageID) + "'")

            ' Add updates to the existing SUM Deployment Package using the AddUpdateContent method.
            ' Note: The method will throw an exception, if the method is not able to add the content.
            existingSUMDeploymentPackage.ExecuteMethod("AddUpdateContent", addUpdateContentParameters)
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub

    Public Function GetPackageID(ByVal PackageName As String) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the PackageID for a given package name.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        MyConnection.Connect(GetVarValue("CMServerFQDN"))
        Dim listOfPackages As IResultObject
        Try
            Dim query As String = "SELECT PackageID FROM SMS_SoftwareUpdatesPackage Where Name = '" & PackageName & "' And PackageType = 5"
            listOfPackages = MyConnection.QueryProcessor.ExecuteQuery(query)
        Catch ex As SmsException
            MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try

        For Each Item As IResultObject In listOfPackages
            Return Item("PackageID").StringValue
        Next
        Return Nothing
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ' AD Functions
    '
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Function UpdateServerDescription(ByVal ServerName As String, ByVal Value As String, ByVal LDAPPath As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to update the Descrition attribute in Active Directory for a server computer object.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            Dim myDirectory As New DirectoryEntry(LDAPPath)
            Dim mySearchResult As SearchResult

            Dim search As New DirectorySearcher(myDirectory)
            Dim myFilter As String = "(&(objectClass=computer)(name=" & ServerName & "))"
            search.Filter = myFilter
            search.PropertiesToLoad.Add("description")

            mySearchResult = search.FindOne()

            If Not mySearchResult Is Nothing Then
                Dim dirEntryResults As New DirectoryEntry(mySearchResult.Path)
                SetADProperty(dirEntryResults, "description", Value)
                dirEntryResults.CommitChanges()
                dirEntryResults.Close()
            End If
            myDirectory.Close()
        Catch e As Exception
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        Return True
    End Function

    Function UpdateServerDepartment(ByVal ServerName As String, ByVal Value As String, ByVal LDAPPath As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to update the Department attribute in Active Directory for a server computer object.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            Dim myDirectory As New DirectoryEntry(LDAPPath)
            Dim mySearchResult As SearchResult

            Dim search As New DirectorySearcher(myDirectory)
            Dim myFilter As String = "(&(objectClass=computer)(name=" & ServerName & "))"
            search.Filter = myFilter
            search.PropertiesToLoad.Add("department")

            mySearchResult = search.FindOne()

            If Not mySearchResult Is Nothing Then
                Dim dirEntryResults As New DirectoryEntry(mySearchResult.Path)
                SetADProperty(dirEntryResults, "department", Value)
                dirEntryResults.CommitChanges()
                dirEntryResults.Close()
            End If
            myDirectory.Close()
        Catch e As Exception
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        Return True
    End Function

    Public Function ConvertLDAP2DNS(ByVal LDAPPath As String) As String
        Dim DNS As String
        DNS = ""
        Dim MyArray() = LDAPPath.Split(",")
        For Each item As String In MyArray
            DNS += item.Split("=")(1).Trim() & "."
        Next
        DNS = DNS.Substring(0, DNS.Length - 1)
        Return DNS
    End Function

    Public Function GetNetbiosDomainName(ByVal dnsDomainName As String) As String
        Dim netbiosDomainName As String = String.Empty

        Dim rootDSE As New DirectoryEntry(String.Format("LDAP://{0}/RootDSE", dnsDomainName))

        Dim configurationNamingContext As String = rootDSE.Properties("configurationNamingContext")(0).ToString()

        Dim searchRoot As New DirectoryEntry(Convert.ToString("LDAP://cn=Partitions,") & configurationNamingContext)

        Dim searcher As New DirectorySearcher(searchRoot)
        searcher.SearchScope = SearchScope.OneLevel
        searcher.PropertiesToLoad.Add("netbiosname")
        searcher.Filter = String.Format("(&(objectcategory=Crossref)(dnsRoot={0})(netBIOSName=*))", dnsDomainName)

        Dim result As SearchResult = searcher.FindOne()

        If result IsNot Nothing Then
            netbiosDomainName = result.Properties("netbiosname")(0).ToString()
        End If

        Return netbiosDomainName
    End Function

    Public Function CreateADGroup(ByVal GroupName As String, ByVal GroupDesc As String) As Boolean
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to create a new AD security group.  The AddMW form calls this function when adding a new group.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim newGroupPrincipal As GroupPrincipal
        Using ouPrincipalContext As PrincipalContext = New PrincipalContext(ContextType.Domain, GetVarValue("DomainNetBIOSName"), GetVarValue("BaseOU"))
            Try
                newGroupPrincipal = New GroupPrincipal(ouPrincipalContext)
                With newGroupPrincipal
                    .Name = GroupName
                    .IsSecurityGroup = True
                    .GroupScope = GroupScope.Global
                    .SamAccountName = GroupName
                    .Description = GroupDesc
                    .Save()
                    Return True
                End With
            Catch ex As Exception
                MessageBox.Show("Failed. Error: " + ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Using
    End Function

    Function GetADSAMAccountName(ByVal ServerName As String, ByVal LDAPPath As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the sAMAccountName of a server based on its CN name.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim myDirectory As New DirectoryEntry(LDAPPath)
        Dim mySearchResult As SearchResult

        Dim search As New DirectorySearcher(myDirectory)
        Dim myFilter As String = "(&(objectClass=computer)(name=" & ServerName & "))"
        search.Filter = myFilter
        search.PropertiesToLoad.Add("sAMAccountName")

        mySearchResult = search.FindOne()
        Dim myResultPropColl As ResultPropertyCollection
        Dim vsAMAccountName As ResultPropertyValueCollection
        myResultPropColl = mySearchResult.Properties
        vsAMAccountName = myResultPropColl.Item("sAMAccountName")
        myDirectory.Close()
        Return vsAMAccountName.Item(0).ToString
    End Function

    Function GetADGroupDescription(ByVal GroupName As String, ByVal LDAPPath As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the group object Description field for a given Active Directory group object.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim myDirectory As New DirectoryEntry(LDAPPath)
        Dim mySearchResult As SearchResult

        Dim search As New DirectorySearcher(myDirectory)
        Dim myFilter As String = "(&(objectClass=group)(name=" & GroupName & "))"
        search.Filter = myFilter
        search.PropertiesToLoad.Add("description")

        Dim myResultPropColl As ResultPropertyCollection
        Dim vsAMAccountName As ResultPropertyValueCollection
        Dim strGroupDesc As String = ""
        mySearchResult = search.FindOne()
        If Not mySearchResult Is Nothing Then
            myResultPropColl = mySearchResult.Properties
            vsAMAccountName = myResultPropColl.Item("description")
            strGroupDesc = vsAMAccountName.Item(0).ToString
        End If
        myDirectory.Close()
        Return strGroupDesc
    End Function

    Function HandleGroupDescription(ByVal GroupName As String, ByVal LDAPPath As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to call GetADGroupDescription as well as keep a running cache of lookups.  When this function is called,
        '  it will lookup the groupname in the GroupDescription dictionary list.  If it finds the GroupName in the dictionary it will return
        '  the description value stored in the cache.  If a match cannot be found in the dictionary then this function will call the 
        '  GetADGroupDescription and return that value as well as add it to the cache for future lookups.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If GroupDescription.ContainsKey(GroupName) Then
            Return GroupDescription.Item(GroupName)
        Else
            Dim value As String = GetADGroupDescription(GroupName, LDAPPath)
            GroupDescription.Add(GroupName, value)
            Return value
        End If
    End Function

    Public Shared Sub SetADProperty(ByVal de As DirectoryEntry, ByVal pName As String, ByVal pValue As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub simply sets the given value of a given attribute on a given AD Object.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If Not pValue Is Nothing Then
            If de.Properties.Contains(pName) Then
                de.Properties(pName)(0) = pValue
            Else
                de.Properties(pName).Add(pValue)
            End If
        End If
    End Sub

    Public Function GetUptime(ByVal ServerName As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function connects to a server by mapping a drive to the IPC$ with the credentials that were collected at application launch.
        '  After the drive is mapped to establish an administrative connection to the remote system a new PerformanceCounter object is created
        '  to determine system UpTime.  The IPC$ is then unmapped and the Uptime Value is returned as a time span object.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim ts As TimeSpan
        If PingHost(ServerName) Then
            Dim pc As New PerformanceCounter("System", "System Up Time", "", ServerName)
            pc.NextValue()
            ts = TimeSpan.FromSeconds(pc.NextValue)
        End If

        Return ts
    End Function

    Public Sub SendEmail(ByVal Subject As String, ByVal MessageBody As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub is used to send an email.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mail As New MailMessage()

        'set the addresses
        mail.From = New MailAddress(GetVarValue("SMTPMailFrom"))
        mail.[To].Add(GetVarValue("SMTPMailTo"))

        'set the content
        mail.Subject = Subject
        mail.Body = MessageBody

        'set the server
        Dim smtp As New SmtpClient(GetVarValue("SMTPServer"))

        'send the message
        Try
            smtp.Send(mail)
        Catch exc As Exception
            MessageBox.Show("Send failure: " & exc.ToString())
        End Try
    End Sub

    Public Function PingHost(ByVal ServerName As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This entry simply performs a DNS lookup and ping of a system.  This does both functions so that an accurate message can be displayed if
        '  a system cannot be reached.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim sIP As String = ""
        Dim IPHEntry As IPHostEntry
        Dim IPAdd() As IPAddress
        Dim Result As Boolean = False
        Dim j As Integer
        Try
            IPHEntry = Dns.GetHostEntry(ServerName)
        Catch
            IPHEntry = Nothing
        End Try
        If Not IPHEntry Is Nothing Then
            IPAdd = IPHEntry.AddressList
            For j = 0 To IPAdd.GetUpperBound(0)
                sIP = IPAdd(j).ToString
            Next
            If My.Computer.Network.Ping(sIP, 30) Then
                Result = True
            Else
                If My.Computer.Network.Ping(sIP, 60) Then
                    Result = True
                End If
            End If
        End If
        Return Result
    End Function

    Public Function CovertCIMtoDate(ByVal CIM_DateString As String) As Date
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to convert the date format used by CIM to a Date Datatype value
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim newDate As DateTime = System.Management.ManagementDateTimeConverter.ToDateTime(CIM_DateString)
        Return newDate
    End Function
    Public Function ConvertDatetoCIM(ByVal DateData As Date) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to convert from a date datatype value to the format used by CIM
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim CIM As String = DateData.Year & DateData.ToString("MM") & DateData.ToString("dd") & DateData.ToString("HH") & DateData.ToString("mm") & DateData.ToString("ss") & ".000000+***"
        Return CIM
    End Function

    Public Function GetCollectionComments(ByVal collectionID As String) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to get the Collection Comments by passing in a collectionID
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        MyConnection.Connect(GetVarValue("CMServerFQDN"))
        Dim collection As IResultObject = MyConnection.GetInstance(String.Format("SMS_Collection.CollectionID='{0}'", collectionID))
        Dim commentText As String = collection("Comment").StringValue
        Return commentText
    End Function

    Public Function Calc2ndTuesday(ByVal StartDate As Date) As Date
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function is used to determine what date the 2nd Tuesday of the given month falls on.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Year As Integer = StartDate.Year
        Dim Month As Integer = StartDate.Month
        Dim NewDate As Date

        Dim FirstDayofMonth As Date = Date.Parse(Month & "/1/" & Year)
        Do While FirstDayofMonth.DayOfWeek <> DayOfWeek.Tuesday
            FirstDayofMonth = FirstDayofMonth.AddDays(1)
        Loop
        NewDate = FirstDayofMonth.AddDays(7)
        Return NewDate

    End Function

    Public Function CalcDate(ByVal Day As String, ByVal StartDate As Date) As Date
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns a date adjusted from the StartDate (Patch Tuesday) by the number of days assigned by the day name.
        '  The days that end in 0 are adjusted relative to Patch Tuesday
        '  The other days are adjusted from the Start Date.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim NewDate As Date
        Dim DayIndex As Integer
        If Day.EndsWith("0") Then
            If Day = "Tuesday0" Then DayIndex = 0
            If Day = "Wednesday0" Then DayIndex = 1
            If Day = "Thursday0" Then DayIndex = 2
            If Day = "Friday0" Then DayIndex = 3
            If Day = "Saturday0" Then DayIndex = 4
            If Day = "Sunday0" Then DayIndex = 5
            If Day = "Monday0" Then DayIndex = 6

            Dim PatchTuesday As Date = Calc2ndTuesday(StartDate)
            NewDate = PatchTuesday.AddDays(DayIndex)
        Else
            If Day = "Tuesday" Then DayIndex = 0
            If Day = "Wednesday" Then DayIndex = 1
            If Day = "Thursday" Then DayIndex = 2
            If Day = "Friday" Then DayIndex = 3
            If Day = "Saturday" Then DayIndex = 4
            If Day = "Sunday" Then DayIndex = 5
            If Day = "Monday" Then DayIndex = 6
            If Day = "Tuesday2" Then DayIndex = 7
            If Day = "Wednesday2" Then DayIndex = 8
            If Day = "Thursday2" Then DayIndex = 9
            If Day = "Friday2" Then DayIndex = 10
            If Day = "Saturday2" Then DayIndex = 11
            If Day = "Sunday2" Then DayIndex = 12
            If Day = "Monday2" Then DayIndex = 13

            NewDate = StartDate.AddDays(DayIndex)
        End If
        Return NewDate
    End Function

    Function GetInt64FromLargeInteger(ByVal largeInteger As Object) As Int64
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function converts the long integer that is stored in AD to something that is easier to work with.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim low As Int32
        Dim high As Int32
        Dim valBytes(7) As Byte
        Dim longInt As IADsLargeInteger = CType(largeInteger, IADsLargeInteger)
        low = longInt.LowPart
        high = longInt.HighPart
        BitConverter.GetBytes(low).CopyTo(valBytes, 0)
        BitConverter.GetBytes(high).CopyTo(valBytes, 4)
        Return BitConverter.ToInt64(valBytes, 0)
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This is a special interface for IADsLargeIntegers, this interface is needed for working with long dates stored in AD in attributes like
    '  lastlogin and pwdLastSet.
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    <ComImport(), Guid("9068270b-0939-11D1-8be1-00c04fd8d503"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)> _
    Public Interface IADsLargeInteger
        Property HighPart As Int32
        Property LowPart As Int32
    End Interface

    Public Function GetDomainList() As String()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function pulls an Array of Strings containing the names of each DN listed in the DomainList registry key.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim DomainList() As String = {}

        Dim MyRegPath As String = "SOFTWARE\AB Utilities\Patch Command Center"

        Dim registryKey As RegistryKey
        registryKey = Registry.CurrentUser

        Dim registrySubKey As RegistryKey

        registrySubKey = registryKey.OpenSubKey(MyRegPath, True)
        If Not registrySubKey Is Nothing Then

            DomainList = registrySubKey.GetValue("DomainList", RegistryValueKind.MultiString)

            registrySubKey.Close()
        End If
        Return DomainList
    End Function

    Public Function GetVarValue(ByVal VarName As String) As String
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This function returns the value for the passed value name in the registry.  This is where the configuration is stored.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Value As String = ""

        Dim MyRegPath As String = "SOFTWARE\AB Utilities\Patch Command Center"

        Dim registryKey As RegistryKey
        registryKey = Registry.CurrentUser

        Dim registrySubKey As RegistryKey

        registrySubKey = registryKey.OpenSubKey(MyRegPath, True)
        If Not registrySubKey Is Nothing Then

            Value = registrySubKey.GetValue(VarName, RegistryValueKind.String)

            registrySubKey.Close()
        End If
        Return Value
    End Function

    Public Sub SetDefaults()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub creates and sets the Registry Keys and values.  This is called the first time the application is run or if the CMServerFQDN key does not exist.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim MyRegPath As String = "SOFTWARE\AB Utilities\Patch Command Center"

        Dim registryKey As RegistryKey
        registryKey = Registry.CurrentUser

        Dim registrySubKey As RegistryKey

        registrySubKey = registryKey.OpenSubKey(MyRegPath, True)
        If registrySubKey Is Nothing Then
            registrySubKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\AB Utilities")
            registrySubKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\AB Utilities\Patch Command Center")
        End If

        Dim DefaultDomains() As String = {"LDAP://DC=icmasu,DC=icm", "LDAP://DC=int,DC=asurion,DC=com", "LDAP://DC=asurion,DC=org"}

        If registrySubKey.GetValue("CMServerFQDN") Is Nothing Then registrySubKey.SetValue("CMServerFQDN", "NDCINFCMMP701.int.asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("ChangeDirectoryPath") Is Nothing Then registrySubKey.SetValue("ChangeDirectoryPath", "\\nasappfp01\budfar\changeControl\Security Patching\", RegistryValueKind.String)
        If registrySubKey.GetValue("XMLFileName") Is Nothing Then registrySubKey.SetValue("XMLFileName", "MWData.xml", RegistryValueKind.String)
        If registrySubKey.GetValue("UpdatesDownloadPath") Is Nothing Then registrySubKey.SetValue("UpdatesDownloadPath", "\\NDCINFCMMP701\MS Packages\WindowsUpdates\", RegistryValueKind.String)
        If registrySubKey.GetValue("SMTPServer") Is Nothing Then registrySubKey.SetValue("SMTPServer", "GLBSMTP.int.asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("SMTPMailTo") Is Nothing Then registrySubKey.SetValue("SMTPMailTo", "IT-SCCMSupport@asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("SMTPMailFrom") Is Nothing Then registrySubKey.SetValue("SMTPMailFrom", "sccm@asurion.com", RegistryValueKind.String)
        If registrySubKey.GetValue("LDAPPath") Is Nothing Then registrySubKey.SetValue("LDAPPath", "LDAP://DC=int,DC=asurion,DC=com", RegistryValueKind.String)
        If registrySubKey.GetValue("DomainNetBIOSName") Is Nothing Then registrySubKey.SetValue("DomainNetBIOSName", "HQDomain", RegistryValueKind.String)
        If registrySubKey.GetValue("BaseOU") Is Nothing Then registrySubKey.SetValue("BaseOU", "OU=Maintenance Windows,OU=Groups,OU=AsurionRoot,DC=int,DC=asurion,DC=com", RegistryValueKind.String)
        If registrySubKey.GetValue("DomainList") Is Nothing Then registrySubKey.SetValue("DomainList", DefaultDomains, RegistryValueKind.MultiString)

        registrySubKey.Close()

    End Sub

    Public Sub AddMWtoXML(ByVal CollectionName As String, ByVal CollectionID As String, ByVal Day As String, ByVal StartTime As String, ByVal Duration As String, ByVal tClass As String)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' This sub adds a collection item element to the Collection Data xml file.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim XMLFilePath As String = GetVarValue("ChangeDirectoryPath") & GetVarValue("XMLFileName")

        Dim doc As New Xml.XmlDocument()
        doc.Load(XMLFilePath)

        ' Create the xml elements that are going to be added.
        Dim Collection As Xml.XmlElement = doc.CreateElement("collectionitem")
        Dim collectionnameEL As Xml.XmlElement = doc.CreateElement("collectionname")
        Dim CollectionIDEL As Xml.XmlElement = doc.CreateElement("CollectionID")
        Dim DayEL As Xml.XmlElement = doc.CreateElement("Day")
        Dim StartTimeEL As Xml.XmlElement = doc.CreateElement("StartTime")
        Dim DurationEL As Xml.XmlElement = doc.CreateElement("Duration")
        Dim ClassEL As Xml.XmlElement = doc.CreateElement("Class")

        ' Populate the data for each element
        collectionnameEL.InnerText = CollectionName
        CollectionIDEL.InnerText = CollectionID
        DayEL.InnerText = Day
        StartTimeEL.InnerText = StartTime
        DurationEL.InnerText = Duration
        ClassEL.InnerText = tClass

        ' Add the elements into the new collectionitem element
        Collection.AppendChild(collectionnameEL)
        Collection.AppendChild(CollectionIDEL)
        Collection.AppendChild(DayEL)
        Collection.AppendChild(StartTimeEL)
        Collection.AppendChild(DurationEL)
        Collection.AppendChild(ClassEL)

        ' Add the new collectionitem element to the xml document
        doc.DocumentElement.AppendChild(Collection)

        'Persist the xml doc back to disk.
        doc.Save(XMLFilePath)
    End Sub


End Class

