Imports System

Module Program
    Sub Main(args As String())
        Dim student1 As New Student()

        Console.Write("Enter Student Name: ")
        student1.Name = Console.ReadLine()

        Console.Write("Enter Student Age: ")
        Integer.TryParse(Console.ReadLine(), student1.Age)

        Console.Write("Enter Student Reg No: ")
        student1.AdministrationNo = Console.ReadLine()

        Console.Write("Enter Institution Name: ")
        student1.School = Console.ReadLine()

        Console.WriteLine()
        student1.DisplayStudentDetails()
    End Sub
End Module
