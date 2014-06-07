Imports System.Drawing.Drawing2D

Public Class Form1
    ''' <summary>
    ''' 関数（y=f(x)）のデリゲート
    ''' </summary>
    ''' <param name="x">入力</param>
    ''' <returns>出力</returns>
    ''' <remarks></remarks>
    Public Delegate Function FuncHandler(ByVal x As Integer) As Integer

    ''' <summary>
    ''' </summary>
    ''' <param name="targetImage">描画対象のイメージ</param>
    ''' <param name="fnc">関数（y=f(x)）</param>
    ''' <param name="xFrom">Ｘ座標の最小値</param>
    ''' <param name="xTo">Ｘ座標の最大値</param>
    ''' <remarks></remarks>

    Public Shared Sub DrawFunction( _
        ByVal targetImage As Image, ByVal fnc As FuncHandler, _
        ByVal xFrom As Integer, ByVal xTo As Integer)

        Dim g As Graphics = Nothing

        Try
            g = Graphics.FromImage(targetImage)
            g.SmoothingMode = SmoothingMode.AntiAlias
            '描画領域を初期化（白で塗りつぶす）
            g.FillRectangle(Brushes.White, 0, 0, _
                targetImage.Width, targetImage.Height)

            '描画領域の原点を中央に移動
            Dim xMax As Integer = targetImage.Width * 0.5
            Dim xMin As Integer = -1 * xMax
            Dim yMax As Integer = targetImage.Height * 0.5
            Dim yMin As Integer = -1 * yMax
            g.TranslateTransform(xMax, yMax)
            '描画領域のＹ座標の向きを逆にする（Ｙ座標の上方向をプラスにする）
            g.ScaleTransform(1, -1)

            '格子を描画する（水平線）
            For y As Single = 0 To yMax Step 10
                g.DrawLine(Pens.Cyan, xMin, y, xMax, y)
            Next
            For y As Single = 0 To yMin Step -10
                g.DrawLine(Pens.Cyan, xMin, y, xMax, y)
            Next

            '格子を描画する（垂直線）
            For x As Single = 0 To xMax Step 10
                g.DrawLine(Pens.CadetBlue, x, yMin, x, yMax)
            Next
            For x As Single = 0 To xMin Step -10
                g.DrawLine(Pens.CadetBlue, x, yMin, x, yMax)
            Next

            'Ｘ軸を描画
            g.DrawLine(Pens.Blue, xMin, 0, xMax, 0)
            'Ｙ軸を描画
            g.DrawLine(Pens.Blue, 0, yMin, 0, yMax)

            '関数のグラフを描画
            For x As Integer = xFrom To xTo
                Try
                    g.DrawLine( _
                        Pens.Black, x - 1, _
                        fnc.Invoke(x - 1), x, fnc.Invoke(x))
                Catch ex As ArithmeticException
                    'ゼロ除算、数値演算のオーバーフロー、
                    '定義されていない演算エラーは無視する
                End Try
            Next
        Finally
            If Not (g Is Nothing) Then
                g.Dispose()
            End If
        End Try
    End Sub

    Public Function 一次関数(ByVal x As Integer) As Integer
        Return x ^ 2 - x - 1
    End Function

    Public Function 三角関数(ByVal x As Integer) As Integer
        Return 50 * Math.Sin(x / 15)
    End Function

    Public Function 平方根(ByVal x As Integer) As Integer
        Return Math.Sqrt(x)
    End Function

    Public Function 三次関数(ByVal x As Integer) As Integer
        Return (1 / 4) * (x ^ 4) - (3 / 2) * (x ^ 2) - 2 * x + 2
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim img As New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)

        '関数の参照をデリゲート変数に格納する
        '※関数を変更する場合は、デリゲート変数の中身を変更する。
        Dim f As New FuncHandler(AddressOf Me.一次関数)
        '関数のグラフを描画する。
        DrawFunction(img, f, img.Width * -0.5, img.Width * 0.5)

        Me.PictureBox1.Image = img
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim img As New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)

        '関数の参照をデリゲート変数に格納する
        '※関数を変更する場合は、デリゲート変数の中身を変更する。
        Dim f As New FuncHandler(AddressOf Me.三角関数)
        '関数のグラフを描画する。
        DrawFunction(img, f, img.Width * -0.5, img.Width * 0.5)

        Me.PictureBox1.Image = img
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim img As New Bitmap(Me.PictureBox1.Width, Me.PictureBox1.Height)

        '関数の参照をデリゲート変数に格納する
        '※関数を変更する場合は、デリゲート変数の中身を変更する。
        Dim f As New FuncHandler(AddressOf Me.平方根)
        '関数のグラフを描画する。
        DrawFunction(img, f, img.Width * -0.5, img.Width * 0.5)

        Me.PictureBox1.Image = img
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim ファイル名 As String
        Try
            SaveFileDialog1.ShowDialog()
            ファイル名 = IO.Path.GetExtension(SaveFileDialog1.FileName)
            Dim extension As String = IO.Path.GetExtension(SaveFileDialog1.FileName)
            PictureBox1.Image.Save(SaveFileDialog1.FileName, Imaging.ImageFormat.Png)
        Catch ex As Exception
            MsgBox("キャンセルが押されました。")
        End Try
        
    End Sub
End Class
