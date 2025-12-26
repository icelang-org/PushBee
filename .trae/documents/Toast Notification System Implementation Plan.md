Based on the analysis of `docs/ui.txt` and `docs/todolist.txt`, here is the implementation plan for the Toast notification system in WPF.

## 1. Define Data Models & Enums
Create a `NotificationType` enum to support the 4 required types:
- `Success`
- `Error`
- `Info`
- `Warning`

## 2. Implement ToastWindow Component
Create a new WPF Window `ToastWindow.xaml` designed as a borderless, transparent window.
- **UI Styling**: Translate the Tailwind/CSS styles from `ui.txt` into WPF `Resources` (SolidColorBrush).
    - Map colors (Emerald, Red, Blue, Amber) for Background, Border, Text, and Icon.
- **Icons**: Convert the SVG path data provided in `ui.txt` into XAML `Path` geometries.
- **Layout**: 
    - Container with rounded corners.
    - Icon on the left, Message text on the right.
    - Close button (optional but recommended).
- **Animations**: Implement smooth `Loaded` (Fade In + Slide Up) and `Unloading` (Fade Out) animations using WPF Storyboards to ensure bug-free performance.

## 3. Implement ToastManager
Create a `ToastManager` class to handle the creation and lifecycle of toasts.
- **Positioning**: Calculate screen coordinates to display toasts (e.g., bottom-right or top-right of the primary screen).
- **Stacking**: Handle displaying multiple toasts simultaneously (stacking them vertically).
- **Auto-close**: Implement a timer to automatically close the toast after a set duration (e.g., 3 seconds).

## 4. Update MainWindow for Testing
Modify `MainWindow.xaml` to include 4 buttons corresponding to the test cases in `todolist.txt`:
- **Success**: "操作成功！数据已保存到服务器"
- **Error**: "操作失败！请检查网络连接"
- **Info**: "系统将在今晚进行维护，预计持续2小时"
- **Warning**: "您的密码即将过期，请及时修改"

## 5. Verification
- Run the application.
- Click each button to verify the correct color, icon, and message appears.
- Verify animations are smooth and the window closes automatically.
