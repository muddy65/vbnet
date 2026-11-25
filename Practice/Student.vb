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
    End Sub
End Class
