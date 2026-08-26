' Commands:
' 1: Start
' 2: Stop
' 3: Set Delay
' 5: Output temperature
' 8: Output to LED's
' 9: Echo

Option Strict Off
Option Explicit On
Friend Class MainForm
    Inherits System.Windows.Forms.Form

    ' vendor and product IDs
    Private Const VendorID As Short = 4660
    Private Const ProductID As Short = 1

    ' read and write buffers
    Private Const BufferInSize As Short = 7
    Private Const BufferOutSize As Short = 7
    Dim BufferIn(BufferInSize) As Byte
    Dim BufferOut(BufferOutSize) As Byte

    ' ****************************************************************
    ' when the form loads, connect to the HID controller - pass
    ' the form window handle so that you can receive notification
    ' events...
    '*****************************************************************
    Private Sub MainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        ' do not remove!
        ConnectToHID(Me.Handle.ToInt32)
    End Sub

    '*****************************************************************
    ' disconnect from the HID controller...
    '*****************************************************************
    Private Sub MainForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        DisconnectFromHID()
    End Sub

    '*****************************************************************
    ' a HID device has been plugged in...
    '*****************************************************************
    Public Sub OnPlugged(ByVal pHandle As Integer)
        If hidGetVendorID(pHandle) = VendorID And hidGetProductID(pHandle) = ProductID Then
            ' ** YOUR CODE HERE **
            txtTemperature.Text = "plugged"
        End If
    End Sub

    '*****************************************************************
    ' a HID device has been unplugged...
    '*****************************************************************
    Public Sub OnUnplugged(ByVal pHandle As Integer)
        If hidGetVendorID(pHandle) = VendorID And hidGetProductID(pHandle) = ProductID Then
            hidSetReadNotify(hidGetHandle(VendorID, ProductID), False)
            txtTemperature.Text = "unplugged"
        End If
    End Sub

    '*****************************************************************
    ' controller changed notification - called
    ' after ALL HID devices are plugged or unplugged
    '*****************************************************************
    Public Sub OnChanged()
        ' get the handle of the device we are interested in, then set
        ' its read notify flag to true - this ensures you get a read
        ' notification message when there is some data to read...
        Dim pHandle As Integer
        pHandle = hidGetHandle(VendorID, ProductID)
        hidSetReadNotify(hidGetHandle(VendorID, ProductID), True)
    End Sub

    '*****************************************************************
    ' on read event...
    '*****************************************************************
    Public Sub OnRead(ByVal pHandle As Integer)
        ' read the data (don't forget, pass the whole array)...
        If hidRead(pHandle, BufferIn(0)) Then
            ' first byte is the report ID, e.g. BufferIn(0)
            ' the other bytes are the data from the microcontroller...
            txtTemperature.Text = Chr(BufferIn(1)) + Chr(BufferIn(2)) + Chr(BufferIn(3)) + Chr(BufferIn(4)) + Chr(BufferIn(5)) + Chr(BufferIn(6)) + Chr(BufferIn(7))
            If lblTemperature.ForeColor = Color.Red Then
                lblTemperature.ForeColor = Color.Black
            Else
                lblTemperature.ForeColor = Color.Red
            End If
        End If
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        BufferOut(0) = 0
        BufferOut(1) = Microsoft.VisualBasic.Asc("1")
        BufferOut(2) = 0
        hidWriteEx(VendorID, ProductID, BufferOut(0))
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        BufferOut(0) = 0
        BufferOut(1) = Microsoft.VisualBasic.Asc("2")
        BufferOut(2) = 0
        hidWriteEx(VendorID, ProductID, BufferOut(0))
    End Sub

    Private Sub btnEcho_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEcho.Click
        BufferOut(0) = 0
        Dim i As Integer
        For i = 0 To 6
            BufferOut(i) = 0
        Next
        BufferOut(1) = Microsoft.VisualBasic.Asc("9")
        For i = 0 To txtEcho.Text.Length - 1
            BufferOut(i + 2) = Microsoft.VisualBasic.Asc(txtEcho.Text(i))
        Next
        hidWriteEx(VendorID, ProductID, BufferOut(0))
    End Sub

    Private Sub btnSetLED_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetLED.Click
        BufferOut(0) = 0
        Dim outputbyte As Byte
        BufferOut(1) = Microsoft.VisualBasic.Asc("8")
        outputbyte = 0
        If CheckBox1.Checked Then
            outputbyte = outputbyte + 1
        End If
        If CheckBox2.Checked Then
            outputbyte = outputbyte + 2
        End If
        If CheckBox3.Checked Then
            outputbyte = outputbyte + 4
        End If
        If CheckBox4.Checked Then
            outputbyte = outputbyte + 8
        End If
        If CheckBox5.Checked Then
            outputbyte = outputbyte + 16
        End If
        If CheckBox6.Checked Then
            outputbyte = outputbyte + 32
        End If
        If CheckBox7.Checked Then
            outputbyte = outputbyte + 64
        End If
        If CheckBox8.Checked Then
            outputbyte = outputbyte + 128
        End If
        BufferOut(2) = outputbyte
        hidWriteEx(VendorID, ProductID, BufferOut(0))
    End Sub

    Private Sub btnDelay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelay.Click
        BufferOut(0) = 0
        BufferOut(1) = Microsoft.VisualBasic.Asc("3")
        BufferOut(2) = txtDelay.Text
        hidWriteEx(VendorID, ProductID, BufferOut(0))

    End Sub

    Private Sub btnGet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGet.Click
        BufferOut(0) = 0
        BufferOut(1) = Microsoft.VisualBasic.Asc("5")
        hidWriteEx(VendorID, ProductID, BufferOut(0))
    End Sub

End Class