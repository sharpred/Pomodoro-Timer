Public Class TimerForm
    Private alarmtime As Date
    Private status As Integer
    Private inc As Integer
    Private timer As Double
    Private type As Double
    Private txtMessage As String

    Private Sub TimerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblTimer.Text = "Timer Stopped"
        With Me
            .Opacity = 0
            .ShowInTaskbar = False
            .WindowState = FormWindowState.Minimized
        End With

        txtFrequency.Text = My.Settings.set_frequency
        txtLongBreak.Text = My.Settings.set_longbreak
        txtShortBreak.Text = My.Settings.set_shortbreak
        txtWorkTime.Text = My.Settings.set_pomodorotime

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        StartTimer()
    End Sub
    Private Sub ShowBalloon()
        With PomodoroNI
            .BalloonTipText = txtMessage
            .ShowBalloonTip(My.Settings.set_balloontime)
        End With
        Beep()
    End Sub


    Private Sub StartTimer()
        Me.alarmtime = Date.Now.AddMinutes(My.Settings.set_pomodorotime)
        status = 1 ' short or long break
        inc = 1 ' first increment
        type = 1 'work period
        txtMessage = "Start Work"
        ShowBalloon()
        Timer1.Enabled = True
    End Sub

    Private Sub StopTimer()
        txtMessage = "Timer Stopped"
        lblTimer.Text = txtMessage
        ShowBalloon()
        Me.Timer1.Stop()
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        StopTimer()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        With Me
            .Opacity = 0
            .ShowInTaskbar = False
            .WindowState = FormWindowState.Minimized
        End With
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If alarmtime < Date.Now Then
            Me.Timer1.Stop()
            If type = 1 Then
                'work period has finished
                Select Case inc
                    Case Is = txtFrequency.Text
                        'reset inc to 1
                        inc = 1
                        'set break interval
                        Me.alarmtime = Date.Now.AddMinutes(My.Settings.set_longbreak)
                        'display message status
                        txtMessage = "Time for a long break"
                        type = 2
                    Case Else
                        ' add 1 to inc counter
                        inc = inc + 1
                        'set break interval
                        Me.alarmtime = Date.Now.AddMinutes(My.Settings.set_shortbreak)
                        'display message status
                        txtMessage = "time for a short break"
                        type = 2
                End Select
                'set type to break
                'start break timer
            Else
                'break period has finished
                'set work status
                'set work interval
                Me.alarmtime = Date.Now.AddMinutes(My.Settings.set_pomodorotime)
                'display message status
                txtMessage = "Back to Work"
                type = 1
            End If
            ShowBalloon()
            Me.Timer1.Start()
        Else
            Dim remaining As TimeSpan = Me.alarmtime.Subtract(Date.Now)
            lblTimer.Text = String.Format("{0:d2}:{1:d2}", remaining.Minutes, remaining.Seconds)
        End If
    End Sub

    Private Sub StartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartToolStripMenuItem.Click
        StartTimer()
    End Sub

    Private Sub StopToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopToolStripMenuItem.Click
        StopTimer()
    End Sub

    Private Sub PomodoroNI_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PomodoroNI.MouseDoubleClick
        With Me
            .Opacity = 1
            .ShowInTaskbar = True
            .WindowState = FormWindowState.Normal
        End With
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        With Me
            .Opacity = 1
            .ShowInTaskbar = True
            .WindowState = FormWindowState.Normal
        End With
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            With My.Settings
                .set_pomodorotime = txtWorkTime.Text
                .set_frequency = txtFrequency.Text
                .set_longbreak = txtLongBreak.Text
                .set_shortbreak = txtShortBreak.Text
            End With
            lblUpdate.Text = "settings updated"
        Catch ex As Exception
            lblUpdate.Text = ex.Message
        End Try
    End Sub
End Class
