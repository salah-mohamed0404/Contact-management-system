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
