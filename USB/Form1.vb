Imports System.Management
Imports System.Runtime.InteropServices

Public Class Form1

    Inherits System.Windows.Forms.Form

    ' Shell Events Constants
    Public Enum ShellEvents
        HSHELL_WINDOWCREATED = 1
        HSHELL_WINDOWDESTROYED = 2
        HSHELL_ACTIVATESHELLWINDOW = 3
        HSHELL_WINDOWACTIVATED = 4
        HSHELL_GETMINRECT = 5
        HSHELL_REDRAW = 6
        HSHELL_TASKMAN = 7
        HSHELL_LANGUAGE = 8
        HSHELL_ACCESSIBILITYSTATE = 11
    End Enum

    ' API Declares
    Public Declare Function RegisterWindowMessage Lib "user32.dll" Alias "RegisterWindowMessageA" (ByVal lpString As String) As Integer
    Public Declare Function DeregisterShellHookWindow Lib "user32" (ByVal hWnd As IntPtr) As Integer
    Public Declare Function RegisterShellHookWindow Lib "user32" (ByVal hWnd As IntPtr) As Integer

    Private WithEvents w As ManagementEventWatcher
    Private q As WqlEventQuery
    Private uMsgNotify As Integer

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        q = New WqlEventQuery("Select * from Win32_DeviceChangeEvent")
        w = New ManagementEventWatcher(q)

        w.Start()
        GetDevices()

        ' This will register the ShellHook event messages between the shell and our application.
        ' The uMsgNotify is then used to communicate between the shell and our application whenever any
        ' of the shell events are fired such as an app starting up, shutting down, activating, minimizing, etc
        uMsgNotify = RegisterWindowMessage("SHELLHOOK")
        ' This basically registers our window to receive the shell events
        Call RegisterShellHookWindow(Me.Handle)

    End Sub

    Private Sub w_EventArrived(ByVal sender As Object, ByVal e As System.Management.EventArrivedEventArgs) Handles w.EventArrived
        Dim mbo As ManagementBaseObject

        ' the first thing we have to do is figure out if this is a creation or deletion event
        mbo = CType(e.NewEvent, ManagementBaseObject)

        Console.WriteLine(mbo.ClassPath.ClassName)

        Select Case mbo.ClassPath.ClassName
            
            Case "Win32_VolumeChangeEvent"
                GetDevices()
        End Select

    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        w.Stop()

    End Sub

    Private Sub GetDevices()

        Dim moReturn As Management.ManagementObjectCollection
        Dim moSearch As Management.ManagementObjectSearcher
        Dim mo As Management.ManagementObject

        moSearch = New Management.ManagementObjectSearcher("Select * from Win32_PnPEntity")
        moReturn = moSearch.Get

        For Each mo In moReturn
            Dim strOut As String = ""

            Try
                strOut = String.Format("{0}, PNP DeviceID {1}", mo("Name").ToString, mo("PNPDeviceID").ToString)

                ' http://msdn2.microsoft.com/en-US/library/ms791134.aspx
                Select Case mo("ClassGUID").ToString.ToLower
                    Case "{72631e54-78a4-11d0-bcf7-00aa00b7b32a}" ' Battery
                        Console.WriteLine(strOut)
                    Case "{53D29EF7-377C-4D14-864B-EB3A85769359}" ' Biometric
                    Case "{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}" ' Bluetooth
                    Case "{4d36e965-e325-11ce-bfc1-08002be10318}" ' CDROM
                    Case "{4d36e967-e325-11ce-bfc1-08002be10318}" ' DiskDrive
                    Case "{4d36e968-e325-11ce-bfc1-08002be10318}" ' Display
                    Case "{4d36e969-e325-11ce-bfc1-08002be10318}" ' FDC
                    Case "{4d36e980-e325-11ce-bfc1-08002be10318}" ' FloppyDisk
                    Case "{4d36e96a-e325-11ce-bfc1-08002be10318}" ' HDC
                    Case "{745a17a0-74d3-11d0-b6fe-00a0c90f57da}" ' HIDClass
                    Case "{48721b56-6795-11d2-b1a8-0080c72e74a2}" ' Dot4
                    Case "{49ce6ac8-6f86-11d2-ble5-0080c72e74a2}" ' Dot4Print
                    Case "{7ebefbc0-3200-11d2-b4c2-00a0C9697d07}" ' 61883
                    Case "{c06ff265-ae09-48f0-812c-16753d7cba83}" ' AVC

                    Case Else
                        'Console.WriteLine(strOut)
                End Select
            Catch ex As Exception

            End Try
        Next

    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = uMsgNotify Then
            Select Case m.WParam.ToInt32
                Case ShellEvents.HSHELL_WINDOWCREATED
                    Console.WriteLine("window created")
                Case ShellEvents.HSHELL_WINDOWDESTROYED
                    Console.WriteLine("window destroyed")
                Case ShellEvents.HSHELL_ACTIVATESHELLWINDOW
                    Console.WriteLine("shell activated")
                Case ShellEvents.HSHELL_WINDOWACTIVATED
                    Console.WriteLine("Window activated")
                Case ShellEvents.HSHELL_GETMINRECT
                    Console.WriteLine("GETMINRECT")
                Case ShellEvents.HSHELL_REDRAW
                    Console.WriteLine("Redraw")
                Case ShellEvents.HSHELL_TASKMAN
                    Console.WriteLine("Task manager")
                Case ShellEvents.HSHELL_LANGUAGE
                    Console.WriteLine("Language")
                Case ShellEvents.HSHELL_ACCESSIBILITYSTATE
                    Console.WriteLine("Accessibility")
            End Select
        End If
        MyBase.WndProc(m)
    End Sub


End Class
