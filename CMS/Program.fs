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

    
// Function to load data into DataGridView
    let loadData () =
        try
            let connectionString = "Server=localhost;Database=tester;User Id=root;Password=;"
            use connection = new MySqlConnection(connectionString)
            connection.Open()

            let query = "SELECT number, name, email FROM data_of_a7"
            use adapter = new MySqlDataAdapter(query, connection)
            let table = new DataTable()
            adapter.Fill(table) |> ignore
            dataGridView.DataSource <- table
        with
        | ex -> MessageBox.Show($"Error loading data: {ex.Message}") |> ignore

    // Refresh button
    let refreshButton = new Button(Text = "Refresh", Top = 230, Left = 620, Width = 100)
    refreshButton.Click.Add(fun _ ->
        loadData() // Reload data after adding
    )

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

    let addButton = new Button(Text = "Add", Top = 260, Left = 260, Width = 100)
    addButton.Click.Add(fun _ ->
        try
            let name = nameTextBox.Text
            let email = emailTextBox.Text
            let number = numberTextBox.Text

            if String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(number) then
                MessageBox.Show("Please fill all fields.") |> ignore
            else
                let connectionString = "Server=localhost;Database=tester;User Id=root;Password=;"
                use connection = new MySqlConnection(connectionString)
                connection.Open()

                let query = "INSERT INTO data_of_a7 (name, number, email) VALUES (@name, @number, @email)"
                use command = new MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@name", name) |> ignore
                command.Parameters.AddWithValue("@number", Int32.Parse(number)) |> ignore
                command.Parameters.AddWithValue("@email", email) |> ignore

                let rowsAffected = command.ExecuteNonQuery()
                if rowsAffected > 0 then
                    MessageBox.Show("Data added successfully!") |> ignore
                else
                    MessageBox.Show("Failed to add data.") |> ignore
        with
        | ex -> MessageBox.Show($"Error: {ex.Message}") |> ignore
    )

    let editButton = new Button(Text = "Edit", Top = 260, Left = 140, Width = 100)
    editButton.Click.Add(fun _ ->
        try
            let name = nameTextBox.Text
            let email = emailTextBox.Text
            let number = numberTextBox.Text
            let searchValue = searchTextBox.Text

            if String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(number) then
                MessageBox.Show("Please fill all fields to update.") |> ignore
            else
                let connectionString = "Server=localhost;Database=tester;User Id=root;Password=;"
                use connection = new MySqlConnection(connectionString)
                connection.Open()

                let queryCheckNumber = "SELECT number FROM data_of_a7 WHERE number = @searchNumber"
                use commandCheckNumber = new MySqlCommand(queryCheckNumber, connection)
                commandCheckNumber.Parameters.AddWithValue("@searchNumber", Int32.Parse(searchValue)) |> ignore
                use readerCheck = commandCheckNumber.ExecuteReader()

                if readerCheck.Read() && readerCheck.GetInt32(0) <> Int32.Parse(number) then
                    MessageBox.Show("Cannot edit the ID.") |> ignore
                else
                    readerCheck.Close()

                    let updateQuery = "UPDATE data_of_a7 SET name = @name, email = @email WHERE number = @searchNumber"
                    use updateCommand = new MySqlCommand(updateQuery, connection)
                    updateCommand.Parameters.AddWithValue("@name", name) |> ignore
                    updateCommand.Parameters.AddWithValue("@email", email) |> ignore
                    updateCommand.Parameters.AddWithValue("@searchNumber", Int32.Parse(searchValue)) |> ignore

                    let rowsAffected = updateCommand.ExecuteNonQuery()
                    if rowsAffected > 0 then
                        MessageBox.Show("Data updated successfully!") |> ignore
                    else
                        MessageBox.Show("Failed to update data.") |> ignore
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
