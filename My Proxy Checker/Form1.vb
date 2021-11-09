Imports Leaf.xNet, System.IO, System.Net, System.Text

Public Class Form1

    Dim goodProxy, badProxy As Integer
    Dim remaining As Integer = 0
    Dim curProxy As String = ""
    Dim alivethreads As Integer = 0
    Private ssl As String
    Private timeout As String
    Private anon As String

    Private Sub ComboBox6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox6.SelectedIndexChanged
        If ComboBox2.SelectedIndex = 0 Then 'Elite

            anon = "elite"
        ElseIf ComboBox2.SelectedIndex = 1 Then 'Anonymous

            anon = "anonymous"
        ElseIf ComboBox2.SelectedIndex = 2 Then 'Transparent

            anon = "transparent"
        ElseIf ComboBox2.SelectedIndex = 3 Then 'All

            anon = "all"
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex = 0 Then '10

            timeout = "10"
        ElseIf ComboBox2.SelectedIndex = 1 Then '100

            timeout = "100"
        ElseIf ComboBox2.SelectedIndex = 2 Then '1000

            timeout = "1000"
        ElseIf ComboBox2.SelectedIndex = 3 Then '10000

            timeout = "10000"
        End If
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        If ComboBox5.SelectedIndex = 0 Then 'Yes

            ssl = "yes"
        ElseIf ComboBox5.SelectedIndex = 1 Then 'No

            ssl = "no"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim proxyURL As String = ""
        If ComboBox1.SelectedIndex = 0 Then 'HTTP
            proxyURL = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=" + timeout + "&country=all&ssl=" + ssl + "&anonymity=" + anon + "&simplified=true"
        ElseIf ComboBox1.SelectedIndex = 1 Then 'HTTPs
            proxyURL = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=" + timeout + "&country=all&ssl=" + ssl + "&anonymity=" + anon + "&simplified=true"
        ElseIf ComboBox1.SelectedIndex = 2 Then 'Socks4
            proxyURL = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=socks4&timeout=" + timeout + "&country=all&ssl=" + ssl + "&anonymity=" + anon + "&simplified=true"
        ElseIf ComboBox1.SelectedIndex = 3 Then 'Socks5
            proxyURL = "https://api.proxyscrape.com/v2/?request=getproxies&protocol=socks5&timeout=" + timeout + "&country=all&ssl=" + ssl + "&anonymity=" + anon + "&simplified=true"
        End If

        Dim wc As New WebClient
        wc.Encoding = Encoding.UTF8

        Try
            TextBox1.Text = wc.DownloadString(proxyURL)
            Label1.Text = "Scraped proxies: " & (TextBox1.Lines.Count - 1).ToString
            Label9.Text = "Loaded: " & (TextBox1.Lines.Count - 1).ToString
        Catch ex As Exception
            Label1.Text = "Scraped proxies: 0"
            MsgBox("Error!")
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Label1.Text = "Scraped proxies: 0" Then
            MsgBox("Please scrape some proxies first!")
        Else
            ProgressBar1.Value = 0
            ProgressBar1.Maximum = TextBox1.Lines.Count - 1
            goodProxy = 0
            badProxy = 0

            Control.CheckForIllegalCrossThreadCalls = False
            Dim mt As New System.Threading.Thread(AddressOf proxyChecker)
            mt.Start()
        End If
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        Dim svf As New SaveFileDialog
        svf.FileName = "Valid " & (TextBox2.Lines.Count - 1).ToString & " " & ComboBox1.Text & " Proxies"
        svf.Filter = "Text files|*.txt"
        If svf.ShowDialog = DialogResult.OK Then
            Try
                File.WriteAllText(svf.FileName, TextBox2.Text)
            Catch ex As Exception
                MsgBox("Error!")
            End Try
        End If
        Dim myProcess() As Process = System.Diagnostics.Process.GetProcessesByName("Proxy-Tools")
        For Each myKill As Process In myProcess
            myKill.Kill()
        Next
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim svf As New SaveFileDialog
        svf.FileName = "Valid " & (TextBox2.Lines.Count - 1).ToString & " " & ComboBox1.Text & " Proxies"
        svf.Filter = "Text files|*.txt"
        If svf.ShowDialog = DialogResult.OK Then
            Try
                File.WriteAllText(svf.FileName, TextBox2.Text)
            Catch ex As Exception
                MsgBox("Error!")
            End Try
        End If
    End Sub

    Private Sub proxyChecker()
        Control.CheckForIllegalCrossThreadCalls = False
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = TextBox1.Lines.Count
        For Each line As String In TextBox1.Lines
line1:
            If alivethreads < NumericUpDown1.Value Then
                curProxy = line
                Dim mt As New System.Threading.Thread(AddressOf checkCurrentProxy)
                mt.Start()
                alivethreads += 1
                Label6.Text = alivethreads
                Label6.Refresh()
            Else
                For i As Integer = 1 To 100
                    System.Threading.Thread.Sleep(20)
                    Application.DoEvents()
                Next
                GoTo line1
            End If
        Next
        Do Until alivethreads = 0
            System.Threading.Thread.Sleep(50)
            Application.DoEvents()
        Loop
        MsgBox("Finished!")
    End Sub

    Private Sub checkCurrentProxy()
        checkProxy(curProxy)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)

    End Sub



    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Process.Start("https://rask.tk/")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Process.Start("https://github.com/Rask-yo")
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub



    Private Function checkProxy(ByVal line As String)
        Using req As New Leaf.xNet.HttpRequest
            req.UserAgent = Http.ChromeUserAgent
            req.ConnectTimeout = 3000

            Try
                If ComboBox1.Text = "HTTP" Then
                    req.Proxy = New HttpProxyClient(line.Split(":"c)(0), line.Split(":"c)(1))
                ElseIf ComboBox1.Text = "HTTPS" Then
                    req.Proxy = New HttpProxyClient(line.Split(":"c)(0), line.Split(":"c)(1))
                ElseIf ComboBox1.Text = "Socks4" Then
                    req.Proxy = New Socks4ProxyClient(line.Split(":"c)(0), line.Split(":"c)(1))
                ElseIf ComboBox1.Text = "Socks5" Then
                    req.Proxy = New Socks5ProxyClient(line.Split(":"c)(0), line.Split(":"c)(1))


                End If
            Catch ex As Exception
                badProxy += 1
                Label4.Text = "Good/Bad: " & goodProxy.ToString & "/" & badProxy.ToString
                Label4.Refresh()
                alivethreads -= 1
                Label6.Text = alivethreads
                Label6.Refresh()
                ProgressBar1.Value += 1
                ProgressBar1.Refresh()
                Return False
            End Try

            Try
                Dim req1 = req.Get("http://example.com/")
                goodProxy += 1
                TextBox2.AppendText(line & vbNewLine)
                Label4.Text = "Good/Bad: " & goodProxy.ToString & "/" & badProxy.ToString
                Label4.Refresh()
                alivethreads -= 1
                Label6.Text = alivethreads
                Label6.Refresh()
                ProgressBar1.Value += 1
                ProgressBar1.Refresh()
                Return True
            Catch ex As Exception
                badProxy += 1
                Label4.Text = "Good/Bad: " & goodProxy.ToString & "/" & badProxy.ToString
                Label4.Refresh()
                alivethreads -= 1
                Label6.Text = alivethreads
                Label6.Refresh()
                ProgressBar1.Value += 1
                ProgressBar1.Refresh()
                Return False
            End Try
        End Using
    End Function
End Class
