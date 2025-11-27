# 详细任务完成指南

本文档提供完成后续开发任务的详细步骤。

---

## 任务1：完善UI布局

### 1.1 调整游戏场景UI

#### 步骤1：打开游戏场景

1. **打开场景**
   - 在Project窗口中，导航到 `Assets/Scenes/`
   - 双击 `Gameplay.unity` 打开场景
   - 等待场景加载完成

#### 步骤2：检查Canvas结构

1. **查看Hierarchy**
   - 在Hierarchy窗口中，找到 `Canvas` 对象
   - 展开Canvas，应该能看到：
     - `GameUI`
     - `VictoryUI`
     - `LeaderboardUI`

2. **如果Canvas不存在**
   - 右键点击Hierarchy空白处
   - `UI -> Canvas`
   - 会自动创建Canvas和EventSystem

#### 步骤3：创建GameUI面板

1. **创建Panel**
   - 在Hierarchy中选择 `Canvas`
   - 右键点击 `Canvas` → `UI -> Panel`
   - 命名为 `GameUIPanel`

2. **配置Panel**
   - 选择 `GameUIPanel`
   - 在Inspector中：
     - **Rect Transform**：
       - Anchor: 拉伸到全屏（点击左上角的方框图标，选择"stretch-stretch"）
       - 或者手动设置：
         - Left: 0, Right: 0, Top: 0, Bottom: 0
     - **Image组件**：
       - Color: 设置为透明（Alpha: 0）
       - 或者删除Image组件（右键点击Image → Remove Component）

3. **添加GameUI脚本**
   - 选择 `GameUIPanel`
   - 点击 `Add Component`
   - 搜索 `GameUI`
   - 添加 `GameUI` 脚本            进行到这一步












#### 步骤4：创建时间显示文本

1. **创建Text组件**
   - 右键点击 `GameUIPanel` → `UI -> Text - TextMeshPro`
   - 如果提示导入TMP，点击 `Import TMP Essentials`
   - 命名为 `TimeText`

2. **配置Text组件**
   - 选择 `TimeText`
   - 在Inspector中：
     - **Rect Transform**：
       - Anchor: 左上角
       - Pos X: 20
       - Pos Y: -20
       - Width: 200
       - Height: 30
     - **TextMeshProUGUI组件**：
       - Text: `00:00.00`
       - Font Size: 24
       - Color: 白色 (255, 255, 255, 255)
       - Alignment: 左上对齐
       - Font Style: Normal

3. **连接到GameUI脚本**
   - 选择 `GameUIPanel`（有GameUI脚本的对象）
   - 在Inspector的GameUI脚本中
   - 找到 `Time Text` 字段
   - 从Hierarchy拖拽 `TimeText` 到该字段

#### 步骤5：创建最佳时间文本

1. **复制TimeText**
   - 选择 `TimeText`
   - 按 `Ctrl+D`（Windows）或 `Cmd+D`（Mac）复制
   - 命名为 `BestTimeText`

2. **调整位置**
   - 选择 `BestTimeText`
   - 在Inspector中：
     - Pos Y: -50（在TimeText下方）

3. **修改文本**
   - Text: `最佳: --:--`

4. **连接到GameUI脚本**
   - 在GameUI脚本的 `Best Time Text` 字段
   - 拖拽 `BestTimeText` 到该字段

#### 步骤6：创建暂停按钮

1. **创建Button**
   - 右键点击 `GameUIPanel` → `UI -> Button - TextMeshPro`
   - 命名为 `PauseButton`

2. **配置Button位置**
   - 选择 `PauseButton`
   - 在Inspector中：
     - **Rect Transform**：
       - Anchor: 右上角
       - Pos X: -20
       - Pos Y: -20
       - Width: 100
       - Height: 40

3. **修改按钮文本**
   - 在Hierarchy中展开 `PauseButton`
   - 选择 `Text (TMP)` 子对象
   - 在Inspector中：
     - Text: `暂停`
     - Font Size: 18

4. **连接到GameUI脚本**
   - 在GameUI脚本的 `Pause Button` 字段
   - 拖拽 `PauseButton` 到该字段

#### 步骤7：创建重置按钮

1. **复制PauseButton**
   - 选择 `PauseButton`
   - 按 `Ctrl+D` 复制
   - 命名为 `ResetButton`

2. **调整位置**
   - Pos Y: -70（在PauseButton下方）

3. **修改文本**
   - 选择 `ResetButton -> Text (TMP)`
   - Text: `重置`

4. **连接到GameUI脚本**
   - 在GameUI脚本的 `Reset Button` 字段
   - 拖拽 `ResetButton` 到该字段

#### 步骤8：美化按钮样式

1. **选择按钮**
   - 选择 `PauseButton`

2. **配置按钮颜色**
   - 在Inspector的Button组件中：
     - **Normal Color**: 蓝色 (0, 100, 200, 255)
     - **Highlighted Color**: 浅蓝色 (100, 150, 255, 255)
     - **Pressed Color**: 深蓝色 (0, 50, 150, 255)
     - **Selected Color**: 蓝色
     - **Disabled Color**: 灰色

3. **应用到其他按钮**
   - 选择 `ResetButton`
   - 重复上述步骤
   - 或复制Button组件设置

#### 步骤9：创建VictoryUI面板

1. **创建Panel**
   - 右键点击 `Canvas` → `UI -> Panel`
   - 命名为 `VictoryPanel`

2. **配置Panel**
   - **Rect Transform**：  
     - Anchor: 拉伸到全屏
     - Left: 0, Right: 0, Top: 0, Bottom: 0
   - **Image组件**：
     - Color: 半透明黑色 (0, 0, 0, 200)

3. **添加VictoryUI脚本**
   - 添加 `VictoryUI` 组件

4. **初始状态设为隐藏**
   - 取消勾选Panel组件（禁用）
   - 或取消勾选GameObject的激活复选框

#### 步骤10：创建VictoryUI内容

1. **创建标题文本**  
   - 在 `VictoryPanel` 下创建Text
   - 命名为 `TitleText`
   - 设置：
     - Text: `关卡完成！`
     - Font Size: 36
     - Color: 金色 (255, 215, 0, 255)
     - Anchor: 居中
     - Pos Y: 100

2. **创建时间文本**
   - 创建Text，命名为 `TimeText`
   - 设置：
     - Text: `完成时间: 00:00.00`
     - Font Size: 24
     - Anchor: 居中
     - Pos Y: 50

3. **创建最佳时间文本**
   - 创建Text，命名为 `BestTimeText`
   - 设置：
     - Text: `最佳时间: --:--`
     - Font Size: 20
     - Anchor: 居中
     - Pos Y: 10

4. **创建排名文本**
   - 创建Text，命名为 `RankText`
   - 设置：
     - Text: `排名: 未上榜`
     - Font Size: 20
     - Anchor: 居中
     - Pos Y: -30

5. **创建按钮组**
   - 创建空GameObject，命名为 `ButtonGroup`
   - 在ButtonGroup下创建4个按钮：
     - `NextLevelButton` - "下一关"
     - `RetryButton` - "重试"
     - `ShareButton` - "分享"
     - `LeaderboardButton` - "排行榜"

6. **配置按钮布局**
   - 使用Horizontal Layout Group或Grid Layout Group
   - 或手动排列按钮

7. **连接到VictoryUI脚本**
   - 选择 `VictoryPanel`
   - 在VictoryUI脚本中配置所有字段引用

#### 步骤11：测试UI布局

1. **进入Game视图**
   - 点击Scene视图上方的 `Game` 标签

2. **检查UI显示**
   - 应该能看到时间文本和按钮
   - 调整分辨率测试不同屏幕尺寸

3. **调整Canvas Scaler**
   - 选择 `Canvas`
   - 在Canvas Scaler组件中：
     - UI Scale Mode: `Scale With Screen Size`
     - Reference Resolution: 1920 x 1080
     - Match: 0.5

---

### 1.2 美化UI样式

#### 步骤1：创建UI样式资源

1. **创建文件夹**
   - 在Project窗口中，右键点击 `Assets`
   - `Create -> Folder`
   - 命名为 `UI`

2. **创建子文件夹**
   - 在UI文件夹下创建：
     - `Sprites` - 存放UI图片
     - `Materials` - 存放UI材质
     - `Fonts` - 存放字体

#### 步骤2：添加按钮背景图片

1. **创建简单图片（可选）**
   - 可以使用纯色图片
   - 或从网上下载免费UI资源

2. **导入图片**
   - 将图片文件复制到 `Assets/UI/Sprites/`
   - Unity会自动导入

3. **配置图片导入设置**
   - 选择图片
   - 在Inspector中：
     - Texture Type: `Sprite (2D and UI)`
     - 点击 `Apply`

4. **应用到按钮**
   - 选择按钮
   - 在Image组件中：
     - Source Image: 拖拽图片到该字段
     - Image Type: `Sliced`（如果图片支持9-slice）

#### 步骤3：添加字体（可选）  

1. **下载中文字体**
   - 从网上下载免费字体（如思源黑体）
   - 或使用系统字体

2. **导入字体**
   - 将字体文件复制到 `Assets/UI/Fonts/`
   - Unity会自动导入

3. **应用到Text组件**
   - 选择Text组件
   - 在Font Asset字段中：
     - 如果使用TextMeshPro，需要创建Font Asset
     - 右键点击字体文件 → `Create -> TextMeshPro -> Font Asset`

---

## 任务2：添加材质和纹理

### 2.1 创建材质文件夹

1. **创建文件夹**
   - 在Project窗口中，右键点击 `Assets`
   - `Create -> Folder`
   - 命名为 `Materials`

### 2.2 创建齿轮材质

1. **创建材质**
   - 右键点击 `Materials` 文件夹
   - `Create -> Material`
   - 命名为 `GearMaterial`

2. **配置材质属性**
   - 选择 `GearMaterial`
   - 在Inspector中：
     - **Albedo**: 点击颜色选择器
       - 选择金属灰色：R: 150, G: 150, B: 160
     - **Metallic**: 0.8（金属质感）
     - **Smoothness**: 0.5（光滑度）
     - **Emission**: 关闭（不发光）

3. **应用材质到预制体**
   - 在Project窗口中，找到 `Assets/Prefabs/Generated/Gear.prefab`
   - 双击打开预制体
   - 在Hierarchy中选择齿轮对象
   - 在Inspector的Mesh Renderer组件中
   - 找到 `Materials` 列表
   - 将 `GearMaterial` 拖到 `Element 0` 字段
   - 点击 `Apply` 保存预制体

### 2.3 创建杠杆材质

1. **创建材质**
   - 创建新材质，命名为 `LeverMaterial`

2. **配置为木材效果**
   - **Albedo**: 棕色 (139, 90, 43)
   - **Metallic**: 0.0（非金属）
   - **Smoothness**: 0.3（稍微粗糙）
   - **Normal Map**: 可以添加木纹法线贴图（可选）

3. **应用材质**
   - 打开 `Lever.prefab`
   - 应用 `LeverMaterial`
   - 保存预制体

### 2.4 创建弹簧材质

1. **创建材质**
   - 创建新材质，命名为 `SpringMaterial`

2. **配置为金属效果**
   - **Albedo**: 银色 (200, 200, 210)
   - **Metallic**: 0.9（高金属度）
   - **Smoothness**: 0.7（高光滑度）

3. **应用材质**
   - 打开 `Spring.prefab`
   - 应用 `SpringMaterial`
   - 保存预制体

### 2.5 创建小球材质

1. **创建材质**
   - 创建新材质，命名为 `BallMaterial`

2. **配置颜色**
   - **Albedo**: 红色 (255, 0, 0) 或蓝色 (0, 100, 255)
   - **Metallic**: 0.0
   - **Smoothness**: 0.8（光滑球体）

3. **应用材质**
   - 打开 `Ball.prefab`
   - 应用 `BallMaterial`
   - 保存预制体

### 2.6 创建地面材质

1. **创建材质**
   - 创建新材质，命名为 `GroundMaterial`

2. **配置地面效果**
   - **Albedo**: 浅灰色 (200, 200, 200) 或绿色 (100, 150, 100)
   - **Metallic**: 0.0
   - **Smoothness**: 0.2（粗糙地面）

3. **应用到场景**
   - 在游戏场景中，选择 `Ground` 对象
   - 应用 `GroundMaterial`

### 2.7 添加纹理贴图（可选）

1. **下载纹理**
   - 访问 https://opengameart.org/
   - 搜索 "metal texture" 或 "wood texture"
   - 下载免费纹理（PNG或JPG格式）

2. **导入纹理**
   - 创建 `Assets/Textures/` 文件夹
   - 将下载的纹理文件复制到该文件夹
   - Unity会自动导入

3. **配置纹理导入设置**
   - 选择纹理文件
   - 在Inspector中：
     - **Texture Type**: `Default`
     - **Max Size**: 1024（根据需要调整）
     - **Format**: `Automatic`
     - 点击 `Apply`

4. **应用纹理到材质**
   - 选择材质（如 `GearMaterial`）
   - 在Albedo旁边，点击小圆圈图标
   - 选择纹理
   - 或直接拖拽纹理到Albedo槽位

5. **调整纹理平铺**
   - 在材质Inspector中：
     - **Tiling**: X: 2, Y: 2（根据需要调整）
     - **Offset**: X: 0, Y: 0

---





## 任务3：测试物理系统

### 3.1 在场景中放置机械部件

#### 步骤1：打开游戏场景

1. **打开场景**
   - 双击 `Assets/Scenes/Gameplay.unity`

#### 步骤2：放置齿轮

1. **从Project窗口拖拽**
   - 找到 `Assets/Prefabs/Generated/Gear.prefab`
   - 拖拽到Scene视图或Hierarchy窗口

2. **调整位置**
   - 在Hierarchy中选择 `Gear`
   - 在Inspector的Transform中：
     - Position: X: 3, Y: 1, Z: 0
     - Rotation: X: 0, Y: 0, Z: 0
     - Scale: X: 1, Y: 1, Z: 1

3. **配置齿轮参数**
   - 在Inspector的Gear脚本中：
     - Rotation Speed: 50
     - Motor Force: 1000
     - Auto Rotate: true（勾选）

4. **设置固定点**
   - 齿轮需要固定在某个位置
   - 创建空GameObject，命名为 `GearAnchor`
   - Position: 与齿轮相同位置
   - 添加Rigidbody组件，设置为Kinematic
   - 在齿轮的HingeJoint组件中：
     - Connected Body: 拖拽 `GearAnchor` 到该字段

#### 步骤3：放置杠杆

1. **拖拽杠杆预制体**
   - 从Project窗口拖拽 `Lever.prefab` 到场景

2. **调整位置和旋转**
   - Position: X: 6, Y: 1, Z: 0
   - Rotation: X: 0, Y: 0, Z: 0
   - Scale: X: 3, Y: 0.2, Z: 0.5

3. **配置杠杆参数**
   - 在Lever脚本中：
     - Trigger Force: 10
     - Max Angle: 45
     - Trigger Layer: 选择包含Ball的层

4. **设置支点**
   - 创建空GameObject，命名为 `LeverPivot`
   - Position: X: 6, Y: 1, Z: 0（杠杆中心）
   - 在Lever脚本的 `Pivot Point` 字段中：
     - 拖拽 `LeverPivot` 到该字段

5. **连接齿轮**
   - 在Lever脚本的 `Connected Objects` 数组中： 
     - Size: 1
     - Element 0: 拖拽齿轮对象到该字段

#### 步骤4：放置弹簧

1. **拖拽弹簧预制体**
   - 从Project窗口拖拽 `Spring.prefab` 到场景

2. **调整位置**
   - Position: X: 9, Y: 2, Z: 0

3. **配置弹簧参数**
   - 在Spring脚本中：
     - Spring Force: 100
     - Damper: 5
     - Min Distance: 1
     - Max Distance: 3

4. **设置连接点**
   - 创建空GameObject，命名为 `SpringAnchor`
   - Position: X: 9, Y: 1, Z: 0
   - 在Spring脚本的 `Connected Point` 字段中：
     - 拖拽 `SpringAnchor` 到该字段

#### 步骤5：配置小球

1. **检查BallStartPoint位置**
   - 选择 `BallStartPoint`
   - Position应该是: X: 0, Y: 2, Z: 0

2. **配置GameManager**
   - 选择 `GameManager`
   - 在Inspector的GameManager脚本中：
     - Ball Prefab: 拖拽 `Ball.prefab` 到该字段
     - Ball Start Position: 拖拽 `BallStartPoint` 到该字段

---

### 3.2 测试物理交互

#### 步骤1：运行游戏

1. **点击Play按钮**
   - 点击Unity顶部工具栏的Play按钮（▶️）

2. **观察小球**
   - 小球应该从起始位置生成
   - 受重力影响下落

#### 步骤2：测试齿轮

1. **观察齿轮旋转**
   - 如果设置了Auto Rotate，齿轮应该自动旋转
   - 检查旋转方向是否正确

2. **测试齿轮联动**
   - 放置第二个齿轮
   - 调整位置使两个齿轮接触
   - 观察是否能够联动

#### 步骤3：测试杠杆

1. **观察杠杆状态**
   - 杠杆应该保持水平
   - 支点应该固定

2. **测试触发**
   - 让小球撞击杠杆
   - 观察杠杆是否翘起
   - 检查连接的齿轮是否被激活

3. **调整触发力**
   - 如果杠杆不触发，增加 `Trigger Force`
   - 如果太容易触发，减少 `Trigger Force`

#### 步骤4：测试弹簧

1. **观察弹簧状态**
   - 弹簧应该显示连接线（如果有LineRenderer）
   - 弹簧应该在连接点之间

2. **测试弹性**
   - 让小球撞击弹簧
   - 观察弹簧是否伸缩
   - 检查弹性是否合理

3. **调整参数**
   - 如果弹性太强，减少 `Spring Force`
   - 如果弹性太弱，增加 `Spring Force`
   - 调整 `Damper` 控制阻尼

#### 步骤5：测试小球到达终点

1. **检查Goal对象**
   - 选择 `Goal` 对象
   - 确认Tag设置为 `Goal`
   - 确认Collider的 `Is Trigger` 已勾选

2. **测试到达终点**
   - 调整Goal位置，使小球能够到达
   - 运行游戏
   - 观察小球到达Goal时是否触发胜利

---

### 3.3 调整物理参数

#### 步骤1：调整物理全局设置

1. **打开Project Settings**
   - `Edit -> Project Settings...`
   - 选择 `Physics`

2. **调整物理参数**
   - **Gravity**: Y: -9.81（默认值，可以调整）
   - **Default Solver Iterations**: 6
   - **Default Solver Velocity Iterations**: 1

#### 步骤2：调整小球物理参数

1. **打开Ball预制体**
   - 双击 `Ball.prefab`

2. **调整Rigidbody**
   - **Mass**: 1（默认，可以调整）
   - **Drag**: 0.5（空气阻力）
   - **Angular Drag**: 0.5（旋转阻力）

3. **测试不同参数**
   - 增加Mass：小球更重，惯性更大
   - 增加Drag：小球更快停止
   - 调整后测试效果

#### 步骤3：调整齿轮参数

1. **选择场景中的齿轮**
   - 或打开Gear预制体

2. **调整Gear脚本参数**
   - **Rotation Speed**: 调整旋转速度
     - 值越大，旋转越快
   - **Motor Force**: 调整电机力
     - 值越大，旋转力越大

3. **测试效果**
   - 运行游戏
   - 观察旋转速度是否合适

#### 步骤4：调整杠杆参数

1. **选择场景中的杠杆**

2. **调整Lever脚本参数**
   - **Trigger Force**: 触发所需力
     - 值越大，需要更大的力才能触发
   - **Max Angle**: 最大旋转角度
     - 值越大，杠杆可以翘得越高

3. **测试效果**
   - 运行游戏
   - 让小球以不同速度撞击杠杆
   - 观察触发效果

#### 步骤5：调整弹簧参数

1. **选择场景中的弹簧**

2. **调整Spring脚本参数**
   - **Spring Force**: 弹簧力
     - 值越大，弹性越强
   - **Damper**: 阻尼
     - 值越大，振动衰减越快
   - **Min/Max Distance**: 距离范围
     - 控制弹簧的伸缩范围

3. **测试效果**
   - 运行游戏
   - 观察弹簧的弹性效果
   - 调整参数直到满意

---

### 3.4 创建测试关卡

#### 步骤1：设计简单测试关卡

1. **规划布局**
   - 起点：X: 0, Y: 2
   - 齿轮：X: 3, Y: 1
   - 杠杆：X: 6, Y: 1
   - 弹簧：X: 9, Y: 2
   - 终点：X: 12, Y: 1

#### 步骤2：放置所有部件

1. **按照规划放置**
   - 使用上述步骤放置所有部件
   - 调整位置使路径连贯

2. **调整摄像机**
   - 选择 `Main Camera`
   - 调整Position和Rotation
   - 确保能看到整个关卡

#### 步骤3：测试完整流程

1. **运行游戏**
   - 点击Play按钮

2. **观察完整流程**
   - 小球生成 → 下落 → 接触齿轮 → 触发杠杆 → 经过弹簧 → 到达终点

3. **记录问题**
   - 哪些部件工作不正常
   - 哪些参数需要调整
   - 哪些位置需要修改

#### 步骤4：优化和调整

1. **根据测试结果调整**
   - 调整部件位置
   - 调整物理参数
   - 添加或移除部件

2. **重复测试**
   - 多次运行游戏
   - 确保流程顺畅
   - 确保所有功能正常

---

## 📋 完成检查清单

### UI布局 ✅
- [ ] GameUI面板已创建
- [ ] 时间文本已创建并连接
- [ ] 按钮已创建并连接
- [ ] VictoryUI面板已创建
- [ ] UI在不同分辨率下正常显示
- [ ] Canvas Scaler已配置

### 材质和纹理 ✅
- [ ] 所有材质已创建
- [ ] 材质已应用到预制体
- [ ] 纹理已导入（如果使用）
- [ ] 材质效果满意

### 物理系统测试 ✅
- [ ] 齿轮可以正常旋转
- [ ] 杠杆可以被触发
- [ ] 弹簧可以正常伸缩
- [ ] 小球可以到达终点
- [ ] 所有物理参数已调整
- [ ] 测试关卡已创建

---

## 🎯 快速参考

### 常用操作快捷键

- `W` - 移动工具
- `E` - 旋转工具
- `R` - 缩放工具
- `Ctrl+D` - 复制对象
- `Delete` - 删除对象
- `F` - 聚焦到选中对象

### UI创建快捷方式

- 右键点击Canvas → `UI -> Text` - 创建文本
- 右键点击Canvas → `UI -> Button` - 创建按钮
- 右键点击Canvas → `UI -> Panel` - 创建面板

### 材质创建快捷方式

- 右键点击文件夹 → `Create -> Material` - 创建材质

---

**按照上述步骤操作，可以完成所有任务！** 🚀

