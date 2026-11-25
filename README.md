# VB.NET Projects

This folder contains two small VB.NET projects: a Windows Forms student registration GUI in `Login` and a console-based student details example in `Practice`.

## Projects

### `Login` — Student Registration GUI

`Login` targets `net8.0-windows` and uses Windows Forms. Run this project to open a form where a user can enter a student's name, course, age, gender, and newsletter preference. Students are added to a list, can be removed, and can be saved as a text file.

The project name is `Login`, but its form is a student registration screen; it does not implement authentication.

#### Form setup and controls

`Form1` inherits from `Form`, the base class for a Windows Forms window. In `Form1_Load`, the code sets the title, size, and screen position, calls `BuildUi`, and starts the clock timer. `BuildUi` creates and arranges controls at runtime rather than relying on the visual designer.

- `TextBox` (`txtName`) accepts a name. `KeyPress` rejects digit characters, while validation also trims whitespace and requires a nonempty value.
- `ComboBox` (`cboCourse`) offers a fixed list of courses. `DropDownList` prevents arbitrary text entry; a course must be selected before adding a student.
- `NumericUpDown` (`nudAge`) restricts age to 16–60 and starts at 18. Numeric controls avoid invalid text input for a numeric value.
- `RadioButton` controls choose one gender. Putting them in the same `FlowLayoutPanel` makes them mutually exclusive as a group.
- `CheckBox` (`chkNews`) captures an independent yes/no newsletter choice.
- `Button` controls perform Add, Remove, and Save actions.
- `ListBox` (`lstStudents`) displays the entries currently held in memory.
- `MenuStrip` and `ToolStripMenuItem` provide a File > Exit command.
- `StatusStrip` and `ToolStripStatusLabel` display the current time.
- `ToolTip` displays a short hint when the pointer rests on the Add button.
- `ErrorProvider` associates validation messages with the control that needs attention.

`AddRow` is a helper method that creates a caption and positions it beside a control. `SetBounds` sets a control's position and size. This is a simple fixed-position layout; a larger or resizable interface often benefits from layout containers such as `TableLayoutPanel` or `FlowLayoutPanel`.

#### Events and behavior

Windows Forms is event-driven: the application waits for user actions and runs event-handler methods in response.

- `Handles btnAdd.Click` connects the Add button's click event to `btnAdd_Click`. After `ValidateInput` succeeds, it builds a display string, adds it to the list, then clears and refocuses the name field.
- `Handles btnRemove.Click` checks for a selected list item and asks for confirmation before removing it.
- `Handles btnSave.Click` opens a `SaveFileDialog`; when the user confirms, `File.WriteAllLines` writes the list entries to the chosen text file.
- `AddHandler lstStudents.DoubleClick, AddressOf lstStudents_DoubleClick` connects an event at runtime. This is an alternative to a `Handles` clause and is useful when wiring events dynamically.
- `Handles cboCourse.SelectedIndexChanged` updates the window title when the selected course changes.
- `Handles tmrClock.Tick` updates the status bar once per second. A Windows Forms `Timer` raises its tick event on the UI thread, so the handler can update controls directly.
- `KeyPreview = True` lets the form receive keyboard events before its child controls. `Form1_KeyDown` maps Ctrl+S to the Save button and suppresses the key's default behavior.
- `Form1_FormClosing` can set `e.Cancel = True` to keep the window open if the user declines the exit confirmation.

Event-handler parameters such as `sender As Object` and `e As EventArgs` follow the .NET event pattern. `sender` identifies the object that raised the event; `e` carries event-specific information.

#### Validation and data

`ValidateInput` returns a `Boolean`: `True` means the required fields are acceptable, and `False` prevents the Add action. `String.Trim` removes surrounding whitespace. `SelectedIndex = -1` means no course has been selected. `Return False` exits the function immediately.

The current GUI stores each student as a formatted `String` in the list box. The age is constrained by the control, and the gender is stored as `M` or `F`. This is convenient for a small demonstration, but a real application would usually use a `Student` class or record and separate data from its display format.

#### Saving and resource cleanup

The `Using` block ensures the `SaveFileDialog` is disposed of when the block ends, including if an error occurs. `ShowDialog` returns a `DialogResult`; writing happens only when the result is `OK`. `File.WriteAllLines` replaces the selected file's contents with the current entries. The list itself is only in memory, so entries are not retained between runs unless the user saves them.

`Imports System.IO` makes file APIs such as `File.WriteAllLines` available without qualifying them with the full namespace.

#### Startup and project configuration

`Program.vb` defines `Sub Main`, the application's entry point. `<STAThread()>` configures the UI thread for Windows desktop components. The startup code configures high-DPI behavior and visual styles, then `Application.Run(New Form1)` opens the main form and starts the message loop that dispatches UI events.

`Login.vbproj` is an SDK-style project file. `OutputType` is `WinExe` for a Windows GUI application, `TargetFramework` is `net8.0-windows`, and `UseWindowsForms` enables Windows Forms. `StartupObject` selects `Sub Main` as the entry point.

`Form1.Designer.vb` is the partial-class companion normally managed by the Windows Forms designer. `Partial` lets one class be split across files. In this project, most controls are instead declared and created in `Form1.vb`; avoid editing designer-managed files by hand if the visual designer is being used.

### `Practice` — Console Student Example

`Practice` targets `net8.0` and runs as a console application. `Program.Main` creates a `Student`, prompts for values using `Console.Write` and `Console.ReadLine`, and calls `DisplayStudentDetails` to print them.

`Student.vb` demonstrates a class with auto-implemented properties (`Name`, `Age`, `AdministrationNo`, and `School`) and a method. `Public` makes these members accessible outside the class. `New Student()` constructs an object, and the dot operator accesses its properties and methods.

`Integer.TryParse` attempts to convert the age input and returns whether conversion succeeded without throwing an exception. The example currently ignores that Boolean result, so invalid age text leaves the age at its default value; a more complete console program would check the result and ask again or report an error.

## General VB.NET concepts in these examples

- **Types and declarations:** `Dim` declares a local variable, while `As String`, `As Integer`, and `As Boolean` specify its type. `New` creates an object.
- **Procedures:** `Sub` performs actions and does not return a value. `Function` returns a value, declared here with `As Boolean`.
- **Properties:** Properties provide a named interface for reading or changing an object's data. Auto-implemented properties are concise VB.NET properties backed by compiler-generated storage.
- **Conditional logic:** `If ... Then` chooses a path based on a condition. `If(condition, value1, value2)` is VB.NET's conditional operator. `AndAlso` short-circuits: the second check runs only when the first is true.
- **Strings:** `$"..."` is string interpolation; `&` concatenates strings. `String()` is an array of strings.
- **Collections and LINQ:** `Items` is a collection. `Cast(Of String)()` treats its entries as strings so they can be passed to `WriteAllLines`; LINQ extension methods are available through VB.NET's standard imports in this project.
- **Namespaces and imports:** Namespaces organize .NET APIs. `Imports` lets code refer to imported types more briefly.
- **Access modifiers:** `Public` exposes a member, `Private` limits it to its containing type, and `Friend` makes it available within the same assembly.
- **Object initializers:** `New Label() With {.Text = caption, ...}` creates an object and sets properties in one expression.
- **Comments:** A single apostrophe (`'`) begins a comment that explains code for readers and is ignored by the compiler.

## Running the projects

Open the `.vbproj` file for the project you want to run in Visual Studio, or use the .NET CLI from that project's directory:

```powershell
dotnet run --project Login/Login.vbproj
dotnet run --project Practice/Practice.vbproj
```

The `Login` Windows Forms app requires Windows. The `Practice` console app can run on platforms supported by .NET 8.
