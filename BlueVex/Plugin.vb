Imports System.Runtime.InteropServices

Public Class Plugin
    Private Declare Function SetParent Lib "user32" (ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As Integer

    Public Sub New()
    End Sub

    Public Sub InitPlugin(ByVal Funcs As IntPtr)
        Dim info As RedVexInfo = Nothing
        Try
            If Funcs <> IntPtr.Zero Then
                Dim handle As IntPtr = Funcs
                info = Marshal.PtrToStructure(handle, GetType(RedVexInfo))
                If Not info.Equals(Nothing) Then
                    MyRedVexInfo = info
                    Dim RedVexHandle As IntPtr = info.GetWindowHandle.Invoke()
                    Main = New frmMain
                    SetParent(Main.Handle, RedVexHandle)
                    Main.SetDesktopLocation(640 - Main.Width, 0)
                    Main.Show()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub DestroyPlugin()

        'MsgBox("Destroy Plugins")
    End Sub

End Class

Public Module MainModule

    Public MyRedVexInfo As RedVexInfo

    Public Delegate Sub WriteLogType(ByVal Text As String)
    Public Delegate Function GetWindowHandleType() As IntPtr

    <Serializable(), StructLayout(LayoutKind.Sequential)> _
     Public Structure RedVexInfo
        Public WriteLog As WriteLogType
        Public GetWindowHandle As GetWindowHandleType
        Public Controler As IntPtr
    End Structure

    Public Delegate Sub RelayDelegate(ByVal bytes() As Byte, ByVal length As Integer, ByVal ProxyPointer As IntPtr, ByVal ModulePointer As IntPtr)
    
    <Serializable(), StructLayout(LayoutKind.Sequential)> _
    Public Structure FunctionInfo
        Public RelayToClientDelegate As RelayDelegate
        Public RelayToServerDelegate As RelayDelegate
        Public ProxyPointer As IntPtr
        Public ModulePointer As IntPtr
    End Structure

    Delegate Sub SetFlagDelegate(ByVal PacketPointer As IntPtr, ByVal flag As Integer)
    <Serializable(), StructLayout(LayoutKind.Sequential)> _
    Public Structure PacketFunctionInfo
        Public SetFlag As SetFlagDelegate
    End Structure

End Module
