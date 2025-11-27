# 修复齿轮旋转方向

## 🔧 问题

齿轮旋转方向不正确。

## ✅ 解决方案

已更新代码，添加了 `reverseDirection` 参数来控制旋转方向。

---

## 🎯 快速修复方法

### 方法1：在Inspector中反转方向（推荐）

1. **选择齿轮对象**
   - 在Hierarchy中选择齿轮对象

2. **找到Gear脚本**
   - 在Inspector中找到 `Gear (Script)` 组件

3. **勾选Reverse Direction**
   - 在 `齿轮参数` 部分
   - 找到 `Reverse Direction` 复选框
   - **勾选它**（反转旋转方向）
   - 或**取消勾选**（恢复正常方向）

4. **测试**
   - 运行游戏
   - 检查旋转方向是否正确

---

### 方法2：修改Rotation Speed符号

1. **选择齿轮对象**

2. **找到Rotation Speed字段**
   - 在Gear脚本的 `齿轮参数` 部分
   - 找到 `Rotation Speed` 字段

3. **修改数值**
   - 如果当前是正数（如 100），改为负数（-100）
   - 如果当前是负数（如 -100），改为正数（100）

4. **测试**
   - 运行游戏检查

---

## 📋 详细操作步骤

### 使用Reverse Direction参数（最简单）

#### 步骤1：选择齿轮

1. **在Hierarchy中找到齿轮**
   - 展开场景对象
   - 找到齿轮对象（名称可能是 `Gear` 或自定义名称）

2. **选择齿轮**
   - 点击选择齿轮对象

#### 步骤2：查看Gear脚本

1. **在Inspector中查看**
   - 应该能看到 `Gear (Script)` 组件

2. **展开齿轮参数**
   - 在 `齿轮参数` 部分
   - 应该能看到：
     - Rotation Speed
     - Motor Force
     - Auto Rotate
     - **Reverse Direction** ← 新添加的参数

#### 步骤3：反转方向

1. **勾选Reverse Direction**
   - 点击 `Reverse Direction` 复选框
   - 勾选 = 反转方向
   - 取消勾选 = 正常方向

2. **运行游戏测试**
   - 点击Play按钮
   - 观察齿轮旋转方向
   - 如果方向正确，完成！
   - 如果方向仍然不对，尝试取消勾选

---

## 🔍 检查HingeJoint轴方向

如果使用Reverse Direction仍然不对，可能需要检查HingeJoint的轴方向。

### 步骤1：检查HingeJoint

1. **选择齿轮对象**

2. **找到Hinge Joint组件**
   - 在Inspector中，找到 `Hinge Joint` 组件

3. **查看Axis设置**
   - 找到 `Axis` 字段
   - 默认应该是 (0, 0, 1) 或 (0, 1, 0)
   - 这决定了旋转轴

### 步骤2：调整Axis（如果需要）

1. **修改Axis值**
   - 如果Axis是 (0, 0, 1)，尝试改为 (0, 0, -1)
   - 如果Axis是 (0, 1, 0)，尝试改为 (0, -1, 0)
   - 如果Axis是 (1, 0, 0)，尝试改为 (-1, 0, 0)

2. **测试**
   - 运行游戏检查旋转方向

---

## 🎯 常见问题

### 问题1：齿轮不旋转

**检查**：
1. Auto Rotate是否勾选
2. 齿轮是否被激活（通过杠杆或其他方式）
3. Motor Force是否足够大

### 问题2：旋转方向仍然不对

**解决**：
1. 尝试勾选/取消勾选Reverse Direction
2. 尝试修改Rotation Speed的符号
3. 检查HingeJoint的Axis设置

### 问题3：多个齿轮方向不一致

**解决**：
1. 为每个齿轮单独设置Reverse Direction
2. 或调整Rotation Speed的符号
3. 连接的齿轮会自动反向（这是正确的，因为齿轮啮合）

---

## 📝 代码说明

### 新增功能

代码中添加了 `reverseDirection` 参数：

```csharp
[SerializeField] private bool reverseDirection = false; // 反转旋转方向
```

### 工作原理

- 如果 `reverseDirection = false`：使用正常的 `rotationSpeed`
- 如果 `reverseDirection = true`：使用 `-rotationSpeed`（反转）

### 齿轮联动

当齿轮被其他齿轮驱动时，会自动反向旋转（这是正确的物理行为）：
```csharp
rotationSpeed = -sourceGear.rotationSpeed; // 反向旋转
```

---

## ✅ 快速修复清单

- [ ] 选择齿轮对象
- [ ] 在Gear脚本中找到 `Reverse Direction` 复选框
- [ ] 勾选或取消勾选（根据需要的方向）
- [ ] 运行游戏测试
- [ ] 如果方向正确，完成！
- [ ] 如果方向仍然不对，尝试修改Rotation Speed符号

---

## 🎮 测试方法

1. **运行游戏**
   - 点击Play按钮

2. **观察齿轮**
   - 如果Auto Rotate已勾选，齿轮应该自动旋转
   - 检查旋转方向是否符合预期

3. **调整方向**
   - 如果方向不对，勾选Reverse Direction
   - 再次测试

4. **确认**
   - 方向正确后，保存场景

---

**按照上述步骤操作，齿轮旋转方向就能修复了！** ✅

