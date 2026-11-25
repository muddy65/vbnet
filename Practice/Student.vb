Public Class Student
    Public Property Name As String
    Public Property Age As Integer
    Public Property AdministrationNo As String
    Public Property School As String

    Public Sub DisplayStudentDetails()
        Console.WriteLine("Student Name: " & Name)
        Console.WriteLine("Student Age: " & Age)
        Console.WriteLine("Registration Number: " & AdministrationNo)
        Console.WriteLine("Institution: " & School)
        Console.WriteLine(DateTime.Now.ToString("HH:MM:SS"))
        Threading.Thread.Sleep(1000)
    End Sub
End Class
