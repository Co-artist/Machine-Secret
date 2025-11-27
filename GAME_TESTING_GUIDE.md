# 游戏测试指南

## 🎯 测试目标

验证游戏的基本功能是否正常工作，包括：
- 场景加载
- 物理系统
- UI系统
- 游戏逻辑

---

## 📋 测试前准备

### 1. 检查场景

确保以下场景已创建：
- ✅ `Assets/Scenes/MainMenu.unity`
- ✅ `Assets/Scenes/Gameplay.unity`
- ✅ `Assets/Scenes/LevelEditor.unity`

### 2. 检查资源

确保以下资源已生成：
- ✅ 预制体（Gear, Lever, Spring, Ball）
- ✅ 材质（GearMaterial, LeverMaterial, SpringMaterial, BallMaterial, GroundMaterial）

### 3. 检查标签

确保以下标签已设置：
- ✅ Ball
- ✅ Goal
- ✅ Checkpoint
- ✅ Hazard

---

## 🚀 第一步：配置GameManager

### 步骤1：打开游戏场景

1. **打开Gameplay场景**
   - 在Project窗口中，双击 `Assets/Scenes/Gameplay.unity`
   - 等待场景加载完成

### 步骤2：配置GameManager

1. **选择GameManager对象**
   - 在Hierarchy中，找到并点击 `GameManager` 对象

2. **查看GameManager脚本**
   - 在Inspector中，找到 `Game Manager (Script)` 组件
   - 展开组件（如果折叠了）

3. **配置Ball Prefab**
   - 找到 `Ball Prefab` 字段
   - 从Project窗口拖拽 `Assets/Prefabs/Generated/Ball.prefab` 到该字段
   - 或点击字段旁边的圆形图标选择预制体

4. **配置Ball Start Position**
   - 找到 `Ball Start Position` 字段
   - 从Hierarchy拖拽 `BallStartPoint` 对象到该字段

5. **检查其他设置**
   - `Game Time Limit`: 默认值即可（或设置为0表示无限制）
   - `Checkpoint Enabled`: 根据需要勾选

---

## 🎮 第二步：基本功能测试

### 测试1：场景加载

1. **运行游戏**
   - 点击Unity顶部工具栏的Play按钮（▶️）
   - 或按 `Ctrl+P`（Windows）或 `Cmd+P`（Mac）

2. **检查场景**
   - 应该能看到Gameplay场景
   - 应该能看到地面（Ground）
   - 应该能看到Goal对象（球体）

3. **检查Console**
   - 打开Console窗口（`Window -> General -> Console`）
   - 检查是否有错误或警告
   - 如果有错误，先解决错误

### 测试2：小球生成

1. **观察小球**
   - 游戏运行后，应该能看到小球生成
   - 小球应该在 `BallStartPoint` 位置（Y: 2）

2. **检查小球行为**
   - 小球应该受重力影响下落
   - 小球应该能碰撞地面

3. **如果小球没有生成**
   - 检查GameManager的Ball Prefab是否已配置
   - 检查Ball预制体是否存在
   - 检查Console是否有错误

### 测试3：物理系统

1. **测试重力**
   - 小球应该下落
   - 小球应该与地面碰撞

2. **测试碰撞**
   - 小球应该能碰撞其他物体
   - 碰撞应该有物理反应

3. **如果物理不正常**
   - 检查Rigidbody组件
   - 检查Collider组件
   - 检查物理设置（`Edit -> Project Settings -> Physics`）

---

## 🔧 第三步：创建测试关卡

### 步骤1：放置测试对象

1. **放置齿轮**
   - 从Project窗口拖拽 `Gear.prefab` 到Scene视图
   - 位置：X: 3, Y: 1, Z: 0
   - 在Inspector中：
     - 勾选 `Auto Rotate`（自动旋转）
     - 设置 `Rotation Speed`: 50
     - 设置 `Rotation Axis`: Z（水平旋转）

2. **放置杠杆**
   - 从Project窗口拖拽 `Lever.prefab` 到Scene视图
   - 位置：X: 6, Y: 1, Z: 0
   - 在Inspector中：
     - 设置 `Trigger Force`: 10
     - 设置 `Trigger Layer`: 选择 `Default`（包含Ball的层）
     - 在 `Connected Objects` 中，拖拽齿轮对象

3. **放置弹簧**
   - 从Project窗口拖拽 `Spring.prefab` 到Scene视图
   - 位置：X: 9, Y: 2, Z: 0
   - 在Inspector中：
     - 设置 `Spring Force`: 100
     - 创建空GameObject作为连接点，拖拽到 `Connected Point` 字段

4. **调整Goal位置**
   - 选择 `Goal` 对象
   - 调整位置，使小球能够到达（如 X: 12, Y: 1, Z: 0）

### 步骤2：配置齿轮固定点

1. **创建固定点**
   - 创建空GameObject，命名为 `GearAnchor`
   - 位置：与齿轮相同（X: 3, Y: 1, Z: 0）

2. **配置固定点**
   - 添加Rigidbody组件
   - 设置为 `Is Kinematic`（勾选）

3. **连接齿轮**
   - 选择齿轮对象
   - 在Hinge Joint组件中
   - 将 `GearAnchor` 拖到 `Connected Body` 字段

---

## 🎯 第四步：测试游戏流程

### 测试1：小球移动

1. **运行游戏**
   - 点击Play按钮

2. **观察小球**
   - 小球应该从起始位置生成
   - 小球应该下落
   - 小球应该能移动

3. **测试重置**
   - 如果小球掉出场景，应该自动重置
   - 或手动触发重置（如果有重置按钮）

### 测试2：到达终点

1. **调整Goal位置**
   - 确保Goal在小球可以到达的位置

2. **运行游戏**
   - 让小球到达Goal

3. **检查胜利**
   - 应该触发胜利逻辑
   - 应该显示胜利UI（如果有）
   - 应该停止计时

### 测试3：物理交互

1. **测试齿轮**
   - 如果设置了Auto Rotate，齿轮应该自动旋转
   - 检查旋转方向是否正确

2. **测试杠杆**
   - 让小球撞击杠杆
   - 杠杆应该被触发
   - 连接的齿轮应该被激活

3. **测试弹簧**
   - 让小球接触弹簧
   - 弹簧应该伸缩
   - 小球应该被弹开

---

## 🖥️ 第五步：测试UI系统

### 测试1：游戏UI

1. **检查UI显示**
   - 应该能看到时间文本
   - 应该能看到按钮（暂停、重置）

2. **测试按钮**
   - 点击暂停按钮，游戏应该暂停
   - 点击重置按钮，游戏应该重置

3. **如果UI不显示**
   - 检查Canvas是否正确设置
   - 检查UI元素是否已连接到脚本
   - 检查UI元素的Rect Transform设置

### 测试2：胜利UI

1. **触发胜利**
   - 让小球到达Goal

2. **检查UI**
   - 应该显示胜利UI
   - 应该显示完成时间
   - 应该显示最佳时间

---

## 🔍 第六步：检查清单

### 基本功能 ✅

- [ ] 场景可以正常加载
- [ ] 小球可以正常生成
- [ ] 小球可以正常下落
- [ ] 小球可以碰撞地面
- [ ] 小球可以到达Goal
- [ ] 胜利逻辑可以触发

### 物理系统 ✅

- [ ] 重力正常工作
- [ ] 碰撞检测正常
- [ ] 齿轮可以旋转
- [ ] 杠杆可以被触发
- [ ] 弹簧可以伸缩

### UI系统 ✅

- [ ] 时间显示正常
- [ ] 按钮可以点击
- [ ] 胜利UI可以显示
- [ ] UI布局正确

### 游戏逻辑 ✅

- [ ] GameManager正常工作
- [ ] 计时功能正常
- [ ] 重置功能正常
- [ ] 检查点功能正常（如果启用）

---

## 🆘 常见问题排查

### 问题1：小球不生成

**检查**：
1. GameManager的Ball Prefab是否已配置
2. Ball预制体是否存在
3. BallStartPoint是否存在
4. Console是否有错误

**解决**：
- 配置GameManager的Ball Prefab字段
- 确保Ball预制体在 `Assets/Prefabs/Generated/` 文件夹中

### 问题2：小球不下落

**检查**：
1. Ball预制体是否有Rigidbody组件
2. Rigidbody的Use Gravity是否勾选
3. 物理设置是否正确

**解决**：
- 检查Ball预制体的Rigidbody组件
- 确保Use Gravity已勾选

### 问题3：碰撞不正常

**检查**：
1. 对象是否有Collider组件
2. Collider是否正确配置
3. 物理层设置是否正确

**解决**：
- 添加或检查Collider组件
- 确保Collider大小合适

### 问题4：UI不显示

**检查**：
1. Canvas是否正确设置
2. UI元素是否已连接到脚本
3. UI元素的Rect Transform是否正确

**解决**：
- 检查Canvas的Render Mode
- 检查UI元素的连接
- 检查UI元素的位置和大小

### 问题5：齿轮不旋转

**检查**：
1. Auto Rotate是否勾选
2. Rotation Speed是否设置
3. Rotation Axis是否正确
4. HingeJoint是否正确配置

**解决**：
- 勾选Auto Rotate
- 设置正确的Rotation Axis
- 检查HingeJoint配置

---

## 📝 测试报告模板

### 测试结果记录

```
测试日期：___________
Unity版本：___________

基本功能：
- 场景加载：□ 通过 □ 失败
- 小球生成：□ 通过 □ 失败
- 物理系统：□ 通过 □ 失败
- UI系统：□ 通过 □ 失败

发现的问题：
1. ________________________________
2. ________________________________
3. ________________________________

备注：
________________________________
```

---

## 🎯 下一步

测试完成后，如果一切正常，可以：

1. **创建更多关卡**
   - 使用LevelEditor创建关卡
   - 或手动在场景中放置对象

2. **优化游戏**
   - 调整物理参数
   - 优化性能
   - 美化UI

3. **添加功能**
   - 添加音效
   - 添加特效
   - 添加更多关卡

---

**按照上述步骤测试，可以全面验证游戏的可行性！** ✅

