# 修复失败的测试项

## 📊 当前测试结果

- ✅ **通过**: 28项
- ❌ **失败**: 4项

---

## 🔍 常见失败项及修复方法

### 失败项1：预制体不存在

**错误信息**：
```
✗ 预制体 Gear 不存在: Assets/Prefabs/Generated/Gear.prefab
✗ 预制体 Lever 不存在: Assets/Prefabs/Generated/Lever.prefab
✗ 预制体 Spring 不存在: Assets/Prefabs/Generated/Spring.prefab
✗ 预制体 Ball 不存在: Assets/Prefabs/Generated/Ball.prefab
```

**修复方法**：

1. **生成预制体**
   ```
   Tools -> 预制体生成器 -> 一键生成所有预制体
   ```

2. **验证生成**
   - 检查Project窗口中的 `Assets/Prefabs/Generated/` 文件夹
   - 应该能看到4个预制体文件

3. **重新运行测试**
   - 运行测试工具验证

---

### 失败项2：GameManager未配置

**错误信息**：
```
✗ GameManager Ball Prefab 未配置
✗ GameManager Ball Start Position 未配置
```

**修复方法**：

1. **打开Gameplay场景**
   - 双击 `Assets/Scenes/Gameplay.unity`

2. **配置GameManager**
   - 选择 `GameManager` 对象
   - 在Inspector中：
     - Ball Prefab: 拖拽 `Ball.prefab` 到该字段
     - Ball Start Position: 拖拽 `BallStartPoint` 到该字段

3. **保存场景**
   - `File -> Save Scene` 或 `Ctrl+S`

---

### 失败项3：标签未设置

**错误信息**：
```
✗ 标签 Ball 未设置
✗ 标签 Goal 未设置
✗ 标签 Checkpoint 未设置
✗ 标签 Hazard 未设置
```

**修复方法**：

1. **运行标签设置工具**
   ```
   Tools -> 设置项目标签和层级
   ```

2. **或手动添加标签**
   - `Edit -> Project Settings -> Tags and Layers`
   - 在Tags部分添加缺失的标签

---

### 失败项4：材质不存在

**错误信息**：
```
⚠ 材质 GearMaterial 不存在
⚠ 材质 LeverMaterial 不存在
```

**修复方法**：

1. **运行自动设置工具**
   ```
   Tools -> 自动设置工具 -> 一键完成所有设置
   ```
   - 工具会自动创建所有材质

2. **或单独创建材质**
   ```
   Tools -> 自动设置工具 -> 仅创建材质
   ```

---

## 🎯 快速修复流程

### 步骤1：生成预制体

```
Tools -> 预制体生成器 -> 一键生成所有预制体
```

### 步骤2：配置GameManager

1. 打开Gameplay场景
2. 选择GameManager
3. 配置Ball Prefab和Ball Start Position
4. 保存场景

### 步骤3：重新运行测试

```
Tools -> 游戏测试工具 -> 运行自动化测试
```

---

## 📋 完整修复检查清单

- [ ] 所有预制体已生成（Gear, Lever, Spring, Ball）
- [ ] GameManager的Ball Prefab已配置
- [ ] GameManager的Ball Start Position已配置
- [ ] 所有标签已设置（Ball, Goal, Checkpoint, Hazard）
- [ ] 所有材质已创建
- [ ] 场景已保存
- [ ] 重新运行测试验证

---

## 🔧 详细修复步骤

### 修复预制体问题

1. **打开预制体生成器**
   ```
   Tools -> 预制体生成器 -> 一键生成所有预制体
   ```

2. **点击生成按钮**
   - 等待生成完成

3. **检查结果**
   - 在Project窗口中查看 `Assets/Prefabs/Generated/` 文件夹
   - 应该能看到4个预制体

### 修复GameManager配置

1. **打开场景**
   - 双击 `Assets/Scenes/Gameplay.unity`

2. **选择GameManager**
   - 在Hierarchy中点击 `GameManager`

3. **配置字段**
   - Ball Prefab: 从Project拖拽 `Ball.prefab`
   - Ball Start Position: 从Hierarchy拖拽 `BallStartPoint`

4. **保存**
   - `Ctrl+S` 保存场景

---

## ✅ 验证修复

修复后，重新运行测试：

```
Tools -> 游戏测试工具 -> 运行自动化测试
```

所有测试应该通过，或者失败的测试项应该减少。

---

## 🆘 如果仍有问题

### 查看详细错误

1. **在测试工具窗口中**
   - 滚动到底部
   - 查看失败的测试项详情

2. **在Console窗口中**
   - 查看详细的错误信息
   - 根据错误信息修复

### 常见问题

- **预制体路径错误**：检查文件夹结构
- **组件缺失**：检查预制体的组件
- **标签未设置**：运行标签设置工具
- **场景未保存**：确保场景已保存

---

**按照上述步骤修复，所有测试应该都能通过！** ✅

