# 手动添加标签指南

## 🔍 问题

在Tag下拉菜单中看不到Goal、Ball等自定义标签。

## ✅ 解决方案

### 方法1：使用改进的工具（推荐）

1. **运行手动设置工具**
   ```
   Tools -> 手动设置标签（可靠方法）
   ```

2. **重新启动Unity**
   - 关闭Unity
   - 重新打开项目
   - 等待编译完成

3. **检查标签**
   - 选择任意GameObject
   - 查看Tag下拉菜单
   - 应该能看到所有标签

---

### 方法2：完全手动添加（最可靠）

如果工具不工作，完全手动添加：

#### 步骤1：打开Project Settings

1. 在Unity顶部菜单栏
2. 点击 `Edit`
3. 选择 `Project Settings...`
4. 窗口会打开

#### 步骤2：找到Tags and Layers

1. 在Project Settings窗口左侧
2. 找到并点击 `Tags and Layers`
3. 右侧会显示标签设置

#### 步骤3：添加标签

1. **找到Tags部分**
   - 在窗口右侧，找到 `Tags` 部分
   - 应该能看到一个列表，显示现有标签

2. **找到空槽位**
   - 标签列表通常有多个槽位（Element 0, Element 1, ...）
   - 找到第一个空的槽位（显示为空或"Untagged"）

3. **添加Ball标签**
   - 点击空槽位
   - 输入：`Ball`
   - 按回车确认

4. **添加Goal标签**
   - 找到下一个空槽位
   - 输入：`Goal`
   - 按回车确认

5. **添加Checkpoint标签**
   - 找到下一个空槽位
   - 输入：`Checkpoint`
   - 按回车确认

6. **添加Hazard标签**
   - 找到下一个空槽位
   - 输入：`Hazard`
   - 按回车确认

#### 步骤4：保存设置

1. **设置会自动保存**
   - Unity会自动保存Project Settings
   - 不需要手动保存

2. **关闭窗口**
   - 关闭Project Settings窗口

#### 步骤5：验证

1. **选择GameObject**
   - 在Hierarchy中选择任意对象（如Goal）

2. **查看Tag下拉菜单**
   - 在Inspector顶部，点击Tag下拉菜单
   - 应该能看到：
     - Ball
     - Goal
     - Checkpoint
     - Hazard

3. **设置Goal标签**
   - 选择 `Goal` 对象
   - 在Tag下拉菜单中选择 `Goal`

---

## 📋 详细操作步骤（带截图说明）

### 步骤1：打开Project Settings

```
Edit -> Project Settings...
```

### 步骤2：选择Tags and Layers

在左侧列表中找到并点击：
```
Tags and Layers
```

### 步骤3：找到Tags列表

在右侧窗口，找到：
```
Tags
  Size: [数字]
  Element 0: [现有标签]
  Element 1: [现有标签]
  ...
  Element X: [空]
```

### 步骤4：添加标签

对于每个空槽位：

1. **点击槽位**
   - 点击 `Element X` 输入框

2. **输入标签名**
   - 输入：`Ball`（或Goal、Checkpoint、Hazard）
   - **注意**：标签名区分大小写，必须完全一致

3. **确认**
   - 按回车键
   - 或点击其他地方

4. **重复**
   - 为每个标签重复上述步骤

### 步骤5：增加Size（如果需要）

如果所有槽位都满了：

1. **增加Size**
   - 找到 `Size` 字段
   - 增加数字（例如从10改为14）
   - 按回车

2. **添加标签**
   - 在新的空槽位中添加标签

---

## ⚠️ 重要提示

### 标签命名规则

- **区分大小写**：`Goal` 和 `goal` 是不同的标签
- **必须完全一致**：代码中使用 `"Goal"`，标签也必须叫 `Goal`
- **不能有空格**：不能使用 `"My Tag"`，应该用 `"MyTag"`

### 标签顺序

- 标签在列表中的顺序不重要
- 只要标签存在，代码就能找到

### 如果标签添加失败

1. **检查标签名**
   - 确保拼写正确
   - 确保大小写正确

2. **检查槽位**
   - 确保选择了正确的槽位
   - 确保输入框可以编辑

3. **重新尝试**
   - 关闭Project Settings
   - 重新打开
   - 再次尝试添加

---

## 🎯 快速检查清单

添加标签后，检查：

- [ ] Ball标签已添加
- [ ] Goal标签已添加
- [ ] Checkpoint标签已添加
- [ ] Hazard标签已添加
- [ ] 在Tag下拉菜单中能看到所有标签
- [ ] 可以为GameObject设置这些标签

---

## ✅ 验证标签已添加

### 测试方法1：查看Tag下拉菜单

1. 选择任意GameObject
2. 在Inspector中点击Tag下拉菜单
3. 应该能看到所有4个标签

### 测试方法2：设置标签

1. 选择 `Goal` 对象
2. 在Tag下拉菜单中选择 `Goal`
3. 如果成功，说明标签已添加

### 测试方法3：查看Project Settings

1. `Edit -> Project Settings -> Tags and Layers`
2. 在Tags列表中应该能看到所有标签

---

## 🆘 如果仍然看不到标签

### 问题1：标签添加了但下拉菜单中没有

**解决**：
1. 关闭Unity
2. 重新打开Unity
3. 等待编译完成
4. 再次检查

### 问题2：无法在Project Settings中添加标签

**解决**：
1. 检查Unity版本（需要Unity 2019.3+）
2. 尝试重启Unity
3. 检查文件权限

### 问题3：标签添加后立即消失

**解决**：
1. 确保Project Settings窗口保持打开
2. 添加标签后不要立即关闭
3. 等待几秒钟让Unity保存

---

## 📝 标签使用示例

添加标签后，可以这样使用：

### 设置Goal对象的标签

1. 在Hierarchy中选择 `Goal` 对象
2. 在Inspector顶部，点击Tag下拉菜单
3. 选择 `Goal`
4. 完成！

### 在代码中使用

代码已经准备好了，标签添加后会自动工作：

```csharp
// BallController.cs 中已经使用了这些标签
if (collision.gameObject.CompareTag("Hazard"))
{
    ResetBall();
}

if (other.CompareTag("Goal"))
{
    ReachGoal();
}
```

---

**按照上述步骤手动添加标签，这是最可靠的方法！** ✅

