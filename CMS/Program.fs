open System
open System.Windows.Forms
open MySql.Data.MySqlClient

[<EntryPoint>]
let main argv =
    // إنشاء النافذة الرئيسية
    let form = new Form(Text = "F# CRUD App", Width = 600, Height = 400)

    // إنشاء الحقول
    let nameLabel = new Label(Text = "Name:", AutoSize = true, Top = 20, Left = 20)
    let nameTextBox = new TextBox(Width = 200, Top = 20, Left = 80)

    let numberLabel = new Label(Text = "Number:", AutoSize = true, Top = 60, Left = 20)
    let numberTextBox = new TextBox(Width = 200, Top = 60, Left = 80)

    let emailLabel = new Label(Text = "Email:", AutoSize = true, Top = 100, Left = 20)
    let emailTextBox = new TextBox(Width = 200, Top = 100, Left = 80)

    // زر "Search"
    let searchLabel = new Label(Text = "Search:", AutoSize = true, Top = 200, Left = 20)
    let searchTextBox = new TextBox(Width = 200, Top = 200, Left = 80)

    let searchButton = new Button(Text = "Search", Top = 200, Left = 300, Width = 100)
    searchButton.Click.Add(fun _ ->
        try
            let searchValue = searchTextBox.Text
            if String.IsNullOrWhiteSpace(searchValue) then
                MessageBox.Show("Please enter a number to search.") |> ignore
            else
                let connectionString = "Server=localhost;Database=tester;User Id=root;Password=;"
                use connection = new MySqlConnection(connectionString)
                connection.Open()

                let query = "SELECT name, number, email FROM data_of_a7 WHERE number = @number"
                use command = new MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@number", Int32.Parse(searchValue)) |> ignore

                use reader = command.ExecuteReader()

                if reader.Read() then
                    nameTextBox.Text <- reader.GetString(0)
                    numberTextBox.Text <- reader.GetInt32(1).ToString()
                    emailTextBox.Text <- reader.GetString(2)
                else
                    MessageBox.Show("Not found any data about this input.") |> ignore

                reader.Close()
        with
        | ex -> MessageBox.Show($"Error: {ex.Message}") |> ignore
    )

   

   

    // إضافة كل العناصر للنافذة
    form.Controls.AddRange(
        [| nameLabel; nameTextBox;
           numberLabel; numberTextBox;
           emailLabel; emailTextBox;
           searchLabel; searchTextBox; searchButton;
           addButton; editButton; deleteButton; clearButton |]
    )

    // تشغيل التطبيق
    Application.Run(form)
    0
