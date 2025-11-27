# 使用Legacy Text替代TextMeshPro

## 🔍 当前情况

在右键菜单中看到了：
- `UI -> Legacy -> Text` ✅
- 但没有 `Text - TextMeshPro` ❌

## ✅ 解决方案

有两种方法：
1. **使用Legacy Text**（简单快速）
2. **安装TextMeshPro**（推荐，但需要额外步骤）

---

## 方案1：使用Legacy Text（推荐，快速）

### 步骤1：创建Legacy Text

1. **右键点击GameUIPanel**
   - 在Hierarchy中，右键点击 `GameUIPanel`

2. **选择UI菜单**
   - `UI -> Legacy -> Text`
   - 点击 `Text`

3. **重命名**
   - 新对象默认名称是 `Text`
   - 重命名为 `TimeText`

### 步骤2：配置Legacy Text

1. **选择TimeText对象**
   - 在Hierarchy中点击 `TimeText`

2. **配置Rect Transform**
   - Anchor: 左上角
   - Pos X: 20
   - Pos Y: -20
   - Width: 200
   - Height: 30

3. **配置Text组件**
   - 在Inspector中找到 `Text` 组件
   - **Text字段**：输入 `00:00.00`
   - **Font Size**：`24`
   - **Color**：白色 (255, 255, 255, 255)
   - **Alignment**：左上对齐

### 步骤3：更新GameUI脚本以支持Legacy Text

需要修改GameUI脚本，使其同时支持Legacy Text和TextMeshPro。

---

## 方案2：安装TextMeshPro（推荐，功能更强大）

### 步骤1：打开Package Manager

1. **打开Package Manager**
   ```
   Window -> Package Manager
   ```

2. **切换到Unity Registry**
   - 点击左上角的下拉菜单
   - 选择 `Unity Registry`

### 步骤2：安装TextMeshPro

1. **搜索TextMeshPro**
   - 在搜索框中输入：`textmeshpro`
   - 或 `TextMeshPro`

2. **找到TextMeshPro包**
   - 应该能看到 `TextMeshPro` 包
   - 显示版本号（如 3.0.6）

3. **安装**
   - 点击 `Install` 按钮
   - 等待安装完成

4. **导入资源**
   - 安装完成后，会弹出窗口
   - 点击 `Import TMP Essential Resources`
   - 等待导入完成

### 步骤3：使用TextMeshPro

安装完成后，右键菜单中就会出现 `Text - TextMeshPro` 选项。

---

## 🔧 修改代码以支持两种Text系统

为了兼容性，可以修改GameUI脚本，使其同时支持Legacy Text和TextMeshPro。

### 修改GameUI.cs

需要将 `Text` 类型改为同时支持两种：

```csharp
// 使用UnityEngine.UI.Text（Legacy）或TMPro.TextMeshProUGUI
[SerializeField] private UnityEngine.UI.Text timeText;  // Legacy Text
// 或者
[SerializeField] private TMPro.TextMeshProUGUI timeText;  // TextMeshPro
```

但实际上，最好的方法是使用接口或基类。不过为了简单，我们可以：

1. **使用Legacy Text**：直接使用 `UnityEngine.UI.Text`
2. **或安装TextMeshPro**：使用 `TMPro.TextMeshProUGUI`

---

## 🎯 推荐操作流程

### 快速方案（使用Legacy Text）

1. **创建Text**
   ```
   Hierarchy → 右键点击 GameUIPanel
   → UI -> Legacy -> Text
   ```

2. **配置Text**
   - 重命名为 `TimeText`
   - 设置位置、大小、文本内容

3. **连接脚本**
   - 拖拽到GameUI脚本的Time Text字段

4. **修改脚本类型**（如果需要）
   - 如果GameUI脚本使用的是TextMeshPro类型
   - 需要改为Legacy Text类型

### 完整方案（安装TextMeshPro）

1. **安装TextMeshPro**
   ```
   Window -> Package Manager
   → 搜索 TextMeshPro
   → 点击 Install
   → 导入资源
   ```

2. **创建Text**
   ```
   Hierarchy → 右键点击 GameUIPanel
   → UI -> Text - TextMeshPro
   ```

3. **配置和连接**
   - 按照之前的步骤操作

---

## 📝 修改GameUI脚本以支持Legacy Text

如果需要使用Legacy Text，需要修改GameUI脚本：

### 方法1：直接修改字段类型

将GameUI.cs中的字段类型从TextMeshPro改为Legacy Text：

```csharp
// 原来的（TextMeshPro）
[SerializeField] private Text timeText;

// 改为（Legacy Text）
[SerializeField] private UnityEngine.UI.Text timeText;
```

### 方法2：使用条件编译（支持两种）

可以同时支持两种Text系统，但这需要更复杂的代码。

---

## ✅ 立即操作

**推荐**：先使用Legacy Text快速完成，稍后再安装TextMeshPro。

### 使用Legacy Text的步骤：

1. **创建Text**
   - 右键点击 `GameUIPanel`
   - `UI -> Legacy -> Text`

2. **重命名**
   - 命名为 `TimeText`

3. **配置**
   - Position: X: 20, Y: -20
   - Text: "00:00.00"
   - Font Size: 24
   - Color: 白色

4. **连接**
   - 拖拽到GameUI脚本的Time Text字段

5. **如果字段类型不匹配**
   - 需要修改GameUI脚本的字段类型
   - 或安装TextMeshPro

---

## 🔍 检查当前GameUI脚本使用的类型

让我检查一下GameUI脚本当前使用的Text类型，然后提供相应的解决方案。

