Imports System.Security.Cryptography

Class MainWindow
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        UpdateBlockStatus()
    End Sub

    Private Sub BlockButton_Click(sender As Object, e As RoutedEventArgs) Handles BlockButton.Click
        If BlockButton.Content.ToString.StartsWith("Block") Then
            Try
                For Each Resource As DictionaryEntry In GetResources()
                    Dim ClothTechName = Resource.Key.ToString.Remove(0, 1)
                    Dim ClothRevName = ClothTechName.Remove(ClothTechName.IndexOf("_"))
                    ClothTechName = ClothTechName.Remove(0, ClothTechName.IndexOf("_") + 1)
                    Dim ClothPath As String = IO.Path.GetTempPath & "\com.sulake.habbo\avatar\" & ClothTechName & "\" & ClothRevName
                    IO.Directory.CreateDirectory(ClothPath)
                    IO.File.WriteAllBytes(ClothPath & "\asset.swf", Resource.Value)
                Next
            Catch
                MsgBox("Error while blocking content.", MsgBoxStyle.Critical, "Error")
            End Try
        Else
            Try
                For Each Resource As DictionaryEntry In GetResources()
                    Dim ClothTechName = Resource.Key.ToString.Remove(0, 1)
                    Dim ClothRevName = ClothTechName.Remove(ClothTechName.IndexOf("_"))
                    ClothTechName = ClothTechName.Remove(0, ClothTechName.IndexOf("_") + 1)
                    Dim ClothPath As String = IO.Path.GetTempPath & "\com.sulake.habbo\avatar\" & ClothTechName & "\" & ClothRevName & "\asset.swf"
                    If IO.File.Exists(ClothPath) Then
                        IO.File.Delete(ClothPath)
                    End If
                Next
            Catch
                MsgBox("Error while unblocking content.", MsgBoxStyle.Critical, "Error")
            End Try
        End If
        UpdateBlockStatus()
    End Sub

    Private Sub AboutButton_Click(sender As Object, e As RoutedEventArgs) Handles AboutButton.Click
        MsgBox("This utility was designed to locally block the visualization of clothing with oppressive association.", MsgBoxStyle.Information, "About")
    End Sub

    Sub UpdateBlockStatus()
        If GetLocalCacheMD5() = GetResourcesMD5() Then
            BlockButton.Background = Brushes.DarkRed
            BlockButton.Content = "Unblock oppressive content"
        Else
            BlockButton.Background = Brushes.Green
            BlockButton.Content = "Block oppressive content"
        End If
    End Sub

    Function GetResources() As IEnumerable(Of Object)
        Dim ResourceSet As Resources.ResourceSet = My.Resources.ResourceManager.GetResourceSet(Globalization.CultureInfo.CurrentCulture, True, True)
        Return ResourceSet.OfType(Of Object)()
    End Function

    Function GetLocalCacheMD5() As String
        Dim FinalMD5 = ""
        For Each Resource As DictionaryEntry In GetResources()
            Dim ClothTechName = Resource.Key.ToString.Remove(0, 1)
            Dim ClothRevName = ClothTechName.Remove(ClothTechName.IndexOf("_"))
            ClothTechName = ClothTechName.Remove(0, ClothTechName.IndexOf("_") + 1)
            Dim ClothPath As String = IO.Path.GetTempPath & "\com.sulake.habbo\avatar\" & ClothTechName & "\" & ClothRevName & "\asset.swf"
            If IO.File.Exists(ClothPath) Then
                FinalMD5 += GetFileMD5(ClothPath)
            End If
        Next
        Return FinalMD5
    End Function

    Function GetResourcesMD5() As String
        Dim FinalMD5 = ""
        For Each Resource As DictionaryEntry In GetResources()
            FinalMD5 += GetBytesMD5(Resource.Value)
        Next
        Return FinalMD5
    End Function

    Private Function GetFileMD5(ByVal filename As String) As String
        Using md5 As MD5 = MD5.Create()
            Using stream = IO.File.OpenRead(filename)
                Return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty)
            End Using
        End Using
    End Function

    Private Function GetBytesMD5(ByVal data As Byte()) As String
        Using md5 As MD5 = MD5.Create()
            Return BitConverter.ToString(md5.ComputeHash(data)).Replace("-", String.Empty)
        End Using
    End Function

End Class
