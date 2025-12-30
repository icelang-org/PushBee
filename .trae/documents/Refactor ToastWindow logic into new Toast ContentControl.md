I will create a new `Toast` custom control inheriting from `ContentControl` and migrate the logic from `ToastWindow` into it.

### **1. Create** **`Toast.cs`** **(New Custom Control)**

* **Inheritance**: Inherits from `System.Windows.Controls.ContentControl`.

* **Properties**:

  * `NotificationType`: DependencyProperty to handle theme switching (Success, Error, Info, Warning).

  * `IconData`, `IconFill`: Read-only DependencyProperties updated by `NotificationType`.

  * `CornerRadius`: For styling flexibility.

* **Events**: `Closed` event.

* **Logic (Moved from ToastWindow)**:

  * **Auto-Close Timer**: Logic moved to `OnApplyTemplate` or `Loaded`.

  * **Theme Logic**: `ApplyTheme` method moves here, updating properties instead of direct element manipulation.

  * **Animations**: Logic to trigger `SlideIn` (on load) and `FadeOut` (on close).

  * **Close Command**: Handles the close button click and timer expiration.

### **2. Create** **`Themes/Generic.xaml`**

* **Purpose**: Define the default `Style` and `ControlTemplate` for the `Toast` control.

* **Content**:

  * Port the XAML UI from `ToastWindow.xaml` (Border, Grid, Path, TextBlock, Button).

  * Bind visual elements to the new `Toast` properties (`Background`, `IconData`, etc.).

  * Define `SlideIn` and `FadeOut` storyboards within the Style Resources.

### **3. Refactor** **`ToastWindow`** **(Wrapper)**

* **XAML**: Remove all UI code. Replace with a single `<local:Toast ... />` element.

* **Code-behind**:

  * Remove all logic (Timer, Theme, Animations).

  * Constructor simply passes the message and type to the `Toast` control.

  * Listen for `Toast.Closed` event to close the `ToastWindow`.

### **4. File Structure Changes**

* New: `Toast.cs`

* New: `Themes/Generic.xaml`

* Modify: `ToastWindow.xaml`, `ToastWindow.xaml.cs`

