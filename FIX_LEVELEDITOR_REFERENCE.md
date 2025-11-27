# 修复 LevelEditor 类型引用错误

## 🔍 问题原因

`LevelEditor.cs` 文件位于 `Assets/Scripts/Editor/` 文件夹中，但它继承自 `MonoBehaviour`，是一个**运行时脚本**。

**Unity的规则**：
- `Editor` 文件夹中的脚本**只在编辑器模式下编译**
- 运行时脚本（如 `EditorUI.cs`）无法访问 `Editor` 文件夹中的类型
- 这就是为什么 `EditorUI.cs` 找不到 `LevelEditor` 类型

---

## ✅ 解决方案：移动文件

**重要**：需要移动两个文件：
- `LevelEditor.cs` - 继承自 `MonoBehaviour`，运行时脚本
- `ComponentConnector.cs` - 继承自 `MonoBehaviour`，运行时脚本

### 方法1：在Unity中移动（推荐）

#### 步骤1：打开Unity编辑器
1. 确保Unity编辑器已打开项目

#### 步骤2：在Project窗口中移动文件

**移动 LevelEditor.cs：**
1. **找到文件**
   - 在Project窗口中，导航到 `Assets/Scripts/Editor/`
   - 找到 `LevelEditor.cs`

2. **移动文件**
   - **拖拽方式**：
     - 将 `LevelEditor.cs` 拖拽到 `Assets/Scripts/` 文件夹
     - 或者拖拽到 `Assets/Scripts/Gameplay/` 文件夹
   
   - **或者使用菜单**：
     - 右键点击 `LevelEditor.cs`
     - 选择 `Cut`（剪切）
     - 导航到目标文件夹（如 `Assets/Scripts/`）
     - 右键点击目标文件夹
     - 选择 `Paste`（粘贴）

**移动 ComponentConnector.cs：**
1. **找到文件**
   - 在Project窗口中，导航到 `Assets/Scripts/Editor/`
   - 找到 `ComponentConnector.cs`

2. **移动文件**
   - 使用相同的方法，将 `ComponentConnector.cs` 移动到相同的位置
   - 建议和 `LevelEditor.cs` 放在同一个文件夹

3. **等待Unity重新编译**
   - Unity会自动检测文件变化
   - 等待编译完成（查看Console窗口）

#### 步骤3：验证修复
1. **检查Console窗口**
   - 应该没有关于 `LevelEditor` 的错误
   - 所有脚本应该正常编译

2. **检查脚本引用**
   - 打开 `EditorUI.cs`
   - 在Inspector中应该能正常看到 `LevelEditor` 字段

---

### 方法2：手动移动文件（如果Unity方法不工作）

#### 步骤1：关闭Unity
1. 完全关闭Unity编辑器

#### 步骤2：在文件系统中移动

**移动 LevelEditor.cs：**
1. **找到文件**
   - 导航到：`C:\Users\zzh66\Desktop\Game\Assets\Scripts\Editor\`
   - 找到 `LevelEditor.cs` 和 `LevelEditor.cs.meta`

2. **移动文件**
   - 将两个文件都移动到：`C:\Users\zzh66\Desktop\Game\Assets\Scripts\`
   - 或者移动到：`C:\Users\zzh66\Desktop\Game\Assets\Scripts\Gameplay\`

**移动 ComponentConnector.cs：**
1. **找到文件**
   - 在相同位置找到 `ComponentConnector.cs` 和 `ComponentConnector.cs.meta`

2. **移动文件**
   - 将两个文件移动到相同的位置（和 `LevelEditor.cs` 一起）

3. **重新打开Unity**
   - 打开Unity编辑器
   - Unity会自动检测文件变化并重新导入

---

## 📋 文件组织建议

### 推荐的文件结构

```
Assets/Scripts/
├── Core/              # 核心机械部件（运行时）
├── Gameplay/          # 游戏玩法逻辑（运行时）
│   └── LevelEditor.cs # ← 移动到这里（如果用于游戏运行时）
├── Editor/            # 编辑器工具（仅编辑器）
│   ├── ResourceGenerator.cs    # 编辑器工具
│   └── SceneConfigurator.cs     # 编辑器工具
├── UI/                # UI脚本（运行时）
├── Network/           # 网络功能（运行时）
└── Utils/             # 工具类（运行时）
```

### 或者创建新文件夹

如果 `LevelEditor` 主要用于编辑器场景（但需要在运行时使用），可以：

```
Assets/Scripts/
├── EditorTools/       # 编辑器工具（运行时）
│   ├── LevelEditor.cs
│   └── ComponentConnector.cs
└── Editor/            # 纯编辑器工具（仅编辑器）
    ├── ResourceGenerator.cs
    └── SceneConfigurator.cs
```

---

## ⚠️ 注意事项

### Editor文件夹 vs 运行时脚本

**Editor文件夹中的脚本**：
- ✅ 只在Unity编辑器中编译
- ✅ 不会包含在最终构建中
- ✅ 用于编辑器工具和扩展
- ❌ 运行时脚本无法访问

**运行时脚本**：
- ✅ 在编辑器和运行时都编译
- ✅ 包含在最终构建中
- ✅ 可以在场景中使用
- ✅ 可以被其他运行时脚本访问

### LevelEditor 的性质

`LevelEditor` 继承自 `MonoBehaviour`，这意味着：
- 它需要在场景中使用
- 它需要在运行时可用（即使只在编辑器场景中）
- **必须移出 `Editor` 文件夹**

---

## 🔧 如果移动后仍有问题

### 问题1：文件移动后Unity没有检测到
**解决**：
1. 在Unity中：`Assets -> Refresh`（或按 `Ctrl+R`）
2. 或者关闭并重新打开Unity

### 问题2：移动后出现其他错误
**解决**：
1. 确保 `ComponentConnector.cs` 也已经移动（它也需要移出 `Editor` 文件夹）
2. 检查是否有其他运行时脚本在 `Editor` 文件夹中

### 问题3：场景中的引用丢失
**解决**：
1. 打开使用 `LevelEditor` 的场景
2. 重新添加 `LevelEditor` 组件到GameObject
3. 重新配置Inspector中的引用

---

## ✅ 验证修复成功

修复后，应该能够：

1. ✅ `EditorUI.cs` 能正常识别 `LevelEditor` 类型
2. ✅ Console窗口没有类型引用错误
3. ✅ 可以在Inspector中正常拖拽 `LevelEditor` 组件
4. ✅ 代码提示（IntelliSense）正常工作
5. ✅ 场景可以正常运行

---

## 📝 快速操作总结

**最简单的修复方法**：

1. 在Unity Project窗口中
2. 找到 `Assets/Scripts/Editor/LevelEditor.cs`
3. 将其拖拽到 `Assets/Scripts/` 文件夹
4. 等待Unity重新编译
5. 检查错误是否消失

**原因**：
- `LevelEditor` 是运行时脚本（继承自 `MonoBehaviour`）
- 不应该放在 `Editor` 文件夹中
- `Editor` 文件夹中的脚本无法被运行时脚本访问

---

**提示**：`ComponentConnector.cs` 也继承自 `MonoBehaviour`，必须和 `LevelEditor.cs` 一起移动。

**需要移动的文件**：
- ✅ `LevelEditor.cs` - 运行时脚本
- ✅ `ComponentConnector.cs` - 运行时脚本

**保留在Editor文件夹的文件**：
- ✅ `ResourceGenerator.cs` - 纯编辑器工具
- ✅ `SceneConfigurator.cs` - 纯编辑器工具

