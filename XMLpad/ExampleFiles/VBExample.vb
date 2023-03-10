Module HelloWorld
    Sub Main()
        Console.WriteLine("What is your name?")
        Dim name As String = Console.ReadLine()
        Console.WriteLine("Hello, " & name & "!")
        Console.ReadLine()
    End Sub
End Module
