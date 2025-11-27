# 详细操作步骤指南

本文档提供完成《机械之谜》项目的每一步详细操作说明。

---

## 第一步：使用AI工具生成基础资源

### 1.1 打开Unity编辑器

#### 操作步骤：
1. **启动Unity Hub**
   - 双击桌面上的Unity Hub图标
   - 如果没有安装，从 https://unity.com/download 下载

2. **打开项目**
   - 在Unity Hub中点击"打开"或"Add"
   - 浏览到项目文件夹：`C:\Users\zzh66\Desktop\Game`
   - 选择该文件夹并打开

3. **等待项目加载**
   - Unity会自动编译所有脚本
   - 等待Console窗口显示"Compiling..."完成
   - 如果有错误，先解决编译错误

#### 预期结果：
- Unity编辑器成功打开
- 项目窗口显示Assets文件夹结构
- Hierarchy窗口为空（新项目）

---

### 1.2 运行资源生成器

#### 操作步骤：

1. **打开资源生成器窗口**
   - 在Unity顶部菜单栏点击 `Tools`
   - 在下拉菜单中找到 `资源生成器`
   - 点击打开窗口

2. **如果菜单中没有"资源生成器"**
   - 检查脚本是否已编译成功
   - 在Project窗口中找到 `Assets/Scripts/Editor/ResourceGenerator.cs`
   - 右键点击 → `Reimport`
   - 等待Unity重新编译
   - 再次尝试打开 `Tools -> 资源生成器`

3. **配置资源生成器**
   - 窗口打开后，你会看到：
     - "保存路径"输入框（默认：`Assets/GeneratedMeshes`）
     - 齿轮生成参数（齿数、半径、厚度）
     - 生成按钮

4. **设置保存路径（可选）**
   - 在"保存路径"输入框中输入：`Assets/Prefabs/Generated`
   - 或者保持默认路径

#### 预期结果：
- 资源生成器窗口成功打开
- 可以看到所有生成选项

---

### 1.3 生成所有基础模型

#### 操作步骤：

#### A. 生成齿轮
1. **配置齿轮参数**
   - 齿数：`12`（默认，可以调整）
   - 半径：`1`（默认）
   - 厚度：`0.3`（默认）

2. **生成齿轮**
   - 点击"生成齿轮"按钮
   - 等待Unity处理（几秒钟）

3. **检查生成结果**
   - 在Project窗口中，找到 `Assets/Prefabs/Generated` 文件夹
   - 应该能看到 `Gear.prefab` 文件
   - 双击打开预制体，检查是否有以下组件：
     - Mesh Filter
     - Mesh Renderer
     - Rigidbody
     - HingeJoint
     - Gear脚本

4. **测试齿轮预制体**
   - 将Gear预制体拖到Scene窗口中
   - 点击Play按钮
   - 检查齿轮是否正确显示

#### B. 生成杠杆
1. **生成杠杆**
   - 点击"生成杠杆"按钮
   - 等待Unity处理

2. **检查生成结果**
   - 在 `Assets/Prefabs/Generated` 文件夹中
   - 应该能看到 `Lever.prefab` 文件
   - 检查组件：
     - Mesh Filter
     - Mesh Renderer
     - Rigidbody
     - Lever脚本

#### C. 生成弹簧
1. **生成弹簧**
   - 点击"生成弹簧"按钮
   - 等待Unity处理（可能需要更长时间）

2. **检查生成结果**
   - 应该能看到 `Spring.prefab` 文件
   - 检查组件：
     - Mesh Filter
     - Mesh Renderer
     - Rigidbody
     - SpringJoint
     - Spring脚本
     - LineRenderer

#### D. 生成小球
1. **生成小球**
   - 点击"生成小球"按钮
   - 等待Unity处理

2. **检查生成结果**
   - 应该能看到 `Ball.prefab` 文件
   - 检查组件：
     - Mesh Filter
     - Mesh Renderer
     - Rigidbody
     - SphereCollider
     - BallController脚本
   - **重要**：确认标签（Tag）设置为"Ball"
     - 选择预制体
     - 在Inspector窗口顶部，找到Tag下拉菜单
     - 如果没有"Ball"标签：
       - 点击下拉菜单 → `Add Tag...`
       - 点击"+"号添加新标签
       - 输入名称：`Ball`
       - 点击"Save"
       - 返回预制体，选择"Ball"标签

#### 预期结果：
- 所有4个预制体都已生成
- 每个预制体都有正确的组件
- 预制体可以正常显示

#### 常见问题解决：

**问题1：生成按钮点击无反应**
- 解决：检查Console窗口是否有错误
- 确保所有脚本已成功编译

**问题2：预制体没有物理组件**
- 解决：关闭资源生成器窗口，重新打开
- 重新生成预制体

**问题3：预制体显示为紫色（缺少材质）**
- 解决：这是正常的，Unity使用默认材质
- 稍后可以添加自定义材质

---

### 1.4 检查生成的预制体

#### 操作步骤：

1. **创建测试场景**
   - 在Hierarchy窗口中，右键点击 → `Create Empty`
   - 命名为 `TestScene`
   - 删除默认的Main Camera和Directional Light（如果存在）

2. **添加地面**
   - 右键点击 `TestScene` → `3D Object -> Plane`
   - 在Inspector中设置：
     - Position: (0, 0, 0)
     - Scale: (10, 1, 10)

3. **测试齿轮**
   - 从Project窗口拖拽 `Gear.prefab` 到Scene窗口
   - 设置Position: (0, 2, 0)
   - 点击Play
   - 检查齿轮是否显示正确

4. **测试杠杆**
   - 拖拽 `Lever.prefab` 到Scene窗口
   - 设置Position: (3, 2, 0)
   - 检查杠杆是否显示正确

5. **测试弹簧**
   - 拖拽 `Spring.prefab` 到Scene窗口
   - 设置Position: (6, 2, 0)
   - 检查弹簧是否显示正确

6. **测试小球**
   - 拖拽 `Ball.prefab` 到Scene窗口
   - 设置Position: (0, 5, 0)
   - 点击Play
   - 小球应该下落

7. **保存测试场景**
   - `File -> Save Scene`
   - 保存为 `Assets/Scenes/TestScene.unity`

#### 预期结果：
- 所有预制体都能正常显示
- 物理组件正常工作
- 可以保存场景

---

## 第二步：使用AI工具配置场景

### 2.1 运行场景配置器

#### 操作步骤：

1. **打开场景配置器窗口**
   - 在Unity顶部菜单栏点击 `Tools`
   - 找到 `场景配置器`
   - 点击打开窗口

2. **如果菜单中没有"场景配置器"**
   - 检查 `Assets/Scripts/Editor/SceneConfigurator.cs` 是否存在
   - 右键点击 → `Reimport`
   - 重新打开 `Tools -> 场景配置器`

3. **查看配置选项**
   - 窗口中有3个按钮：
     - "配置游戏场景"
     - "配置编辑器场景"
     - "配置主菜单场景"
   - 还有一个"创建所有场景"按钮

#### 预期结果：
- 场景配置器窗口成功打开

---

### 2.2 创建所有场景

#### 操作步骤：

1. **创建场景文件夹（如果不存在）**
   - 在Project窗口中，右键点击 `Assets`
   - `Create -> Folder`
   - 命名为 `Scenes`

2. **运行场景配置器**
   - 点击"创建所有场景"按钮
   - 等待Unity处理（可能需要几分钟）
   - 注意观察Console窗口的提示信息

3. **检查生成结果**
   - 在Project窗口中，找到 `Assets/Scenes` 文件夹
   - 应该能看到3个场景文件：
     - `MainMenu.unity`
     - `Gameplay.unity`
     - `LevelEditor.unity`

#### 预期结果：
- 3个场景文件都已创建
- Console窗口显示成功消息

#### 常见问题解决：

**问题1：场景创建失败**
- 解决：检查Console窗口的错误信息
- 确保之前的脚本都已编译成功

**问题2：场景文件存在但无法打开**
- 解决：右键点击场景文件 → `Reimport`
- 然后双击打开场景

---

### 2.3 检查自动生成的场景

#### 操作步骤：

#### A. 检查主菜单场景

1. **打开主菜单场景**
   - 双击 `Assets/Scenes/MainMenu.unity`
   - 等待场景加载

2. **检查Hierarchy窗口**
   - 应该看到以下对象：
     - `Directional Light` - 光源
     - `NetworkManager` - 网络管理器
     - `SceneLoader` - 场景加载器
     - `AudioManager` - 音频管理器
     - `Canvas` - UI画布
     - `LevelSelector` - 关卡选择器（在Canvas下）

3. **检查组件**
   - 选择 `NetworkManager`
   - 在Inspector中确认有 `NetworkManager` 脚本
   - 选择 `Canvas`
   - 确认有 `Canvas`、`CanvasScaler`、`GraphicRaycaster` 组件

#### B. 检查游戏场景

1. **打开游戏场景**
   - 双击 `Assets/Scenes/Gameplay.unity`

2. **检查Hierarchy窗口**
   - 应该看到：
     - `Ground` - 地面
     - `Directional Light` - 光源
     - `Main Camera` - 摄像机
     - `GameManager` - 游戏管理器
     - `LevelManager` - 关卡管理器
     - `NetworkManager` - 网络管理器
     - `PerformanceOptimizer` - 性能优化器
     - `ObjectPool` - 对象池
     - `AudioManager` - 音频管理器
     - `SceneLoader` - 场景加载器
     - `BallStartPoint` - 小球起始点
     - `Goal` - 终点
     - `Canvas` - UI画布
     - `GameUI` - 游戏UI（在Canvas下）
     - `VictoryUI` - 胜利UI（在Canvas下）
     - `LeaderboardUI` - 排行榜UI（在Canvas下）

3. **检查GameManager配置**
   - 选择 `GameManager`
   - 在Inspector中找到 `Ball Prefab` 字段
   - 如果没有赋值：
     - 从Project窗口拖拽 `Assets/Prefabs/Generated/Ball.prefab` 到该字段
   - 找到 `Ball Start Position` 字段
   - 如果没有赋值：
     - 从Hierarchy拖拽 `BallStartPoint` 到该字段

4. **检查摄像机位置**
   - 选择 `Main Camera`
   - 调整Position和Rotation，确保能看到场景
   - 建议设置：
     - Position: (0, 5, -10)
     - Rotation: (20, 0, 0)

#### C. 检查编辑器场景

1. **打开编辑器场景**
   - 双击 `Assets/Scenes/LevelEditor.unity`

2. **检查Hierarchy窗口**
   - 应该看到：
     - `Ground` - 地面
     - `Directional Light` - 光源
     - `Main Camera` - 摄像机
     - `LevelEditor` - 关卡编辑器
     - `ComponentConnector` - 部件连接器
     - `LevelManager` - 关卡管理器
     - `Canvas` - UI画布
     - `EditorUI` - 编辑器UI（在Canvas下）

3. **配置LevelEditor**
   - 选择 `LevelEditor`
   - 在Inspector中找到 `Component Prefabs` 数组
   - 设置数组大小为4
   - 依次添加：
     - Element 0: `Assets/Prefabs/Generated/Gear.prefab`
     - Element 1: `Assets/Prefabs/Generated/Lever.prefab`
     - Element 2: `Assets/Prefabs/Generated/Spring.prefab`
     - Element 3: `Assets/Prefabs/Generated/Ball.prefab`

#### 预期结果：
- 所有场景都已正确创建
- 所有对象都已正确配置
- 场景可以正常运行

---

### 2.4 手动调整UI布局

#### 操作步骤：

#### A. 调整游戏场景UI

1. **打开游戏场景**
   - 双击 `Assets/Scenes/Gameplay.unity`

2. **创建GameUI面板**
   - 在Hierarchy中选择 `Canvas -> GameUI`
   - 如果没有，创建：
     - 右键点击 `Canvas` → `UI -> Panel`
     - 命名为 `GameUI`
     - 添加 `GameUI` 脚本组件

3. **创建时间显示文本**
   - 右键点击 `GameUI` → `UI -> Text - TextMeshPro`
   - 如果提示导入TMP，点击"Import TMP Essentials"
   - 命名为 `TimeText`
   - 在Inspector中设置：
     - Text: "00:00.00"
     - Font Size: 24
     - Color: 白色
     - Alignment: 左上
   - 在Rect Transform中设置：
     - Anchor: 左上角
     - Pos X: 20
     - Pos Y: -20

4. **创建最佳时间文本**
   - 复制 `TimeText`（Ctrl+D）
   - 命名为 `BestTimeText`
   - 设置：
     - Text: "最佳: --:--"
     - Pos Y: -50

5. **创建暂停按钮**
   - 右键点击 `GameUI` → `UI -> Button - TextMeshPro`
   - 命名为 `PauseButton`
   - 设置：
     - Anchor: 右上角
     - Pos X: -20
     - Pos Y: -20
     - 按钮文本: "暂停"

6. **创建重置按钮**
   - 复制 `PauseButton`
   - 命名为 `ResetButton`
   - 设置：
     - Pos Y: -60
     - 按钮文本: "重置"

7. **配置GameUI脚本引用**
   - 选择 `GameUI` 对象
   - 在Inspector中的GameUI脚本中：
     - 拖拽 `TimeText` 到 `Time Text` 字段
     - 拖拽 `BestTimeText` 到 `Best Time Text` 字段
     - 拖拽 `PauseButton` 到 `Pause Button` 字段
     - 拖拽 `ResetButton` 到 `Reset Button` 字段

8. **创建胜利UI面板**
   - 右键点击 `Canvas` → `UI -> Panel`
   - 命名为 `VictoryPanel`
   - 添加 `VictoryUI` 脚本组件
   - 设置初始状态为隐藏：
     - 取消勾选Panel组件（禁用）

9. **在VictoryPanel下创建UI元素**
   - 创建标题文本："关卡完成！"
   - 创建时间文本：`TimeText`
   - 创建最佳时间文本：`BestTimeText`
   - 创建排名文本：`RankText`
   - 创建按钮：
     - `NextLevelButton` - "下一关"
     - `RetryButton` - "重试"
     - `ShareButton` - "分享"
     - `LeaderboardButton` - "排行榜"

10. **配置VictoryUI脚本引用**
    - 选择 `VictoryPanel`
    - 在Inspector中配置所有字段引用

#### B. 调整编辑器场景UI

1. **打开编辑器场景**
   - 双击 `Assets/Scenes/LevelEditor.unity`

2. **创建编辑器面板**
   - 在Canvas下创建Panel，命名为 `EditorPanel`
   - 添加 `EditorUI` 脚本

3. **创建部件库面板**
   - 创建Panel，命名为 `ComponentLibrary`
   - 设置：
     - Anchor: 左侧
     - Width: 200
     - 背景色: 半透明黑色

4. **创建部件按钮**
   - 在ComponentLibrary下创建按钮
   - 为每个部件创建一个按钮
   - 按钮文本：齿轮、杠杆、弹簧、小球

5. **创建工具栏**
   - 创建Panel，命名为 `Toolbar`
   - 添加按钮：
     - 保存
     - 加载
     - 播放
     - 撤销
     - 重做

6. **配置EditorUI脚本引用**
    - 配置所有按钮和输入框的引用

#### 预期结果：
- UI布局美观合理
- 所有UI元素都已正确连接
- 可以在Game视图中看到UI

---

## 第三步：手动完善资源

### 3.1 使用Blender优化模型（可选）

#### 操作步骤：

1. **下载并安装Blender**
   - 访问 https://www.blender.org/download/
   - 下载最新版本（免费）
   - 安装到电脑

2. **导入Unity生成的模型**
   - 打开Blender
   - `File -> Import -> FBX`（如果Unity导出了FBX）
   - 或者手动创建模型：
     - 删除默认立方体（按X键）
     - 创建新模型

3. **优化齿轮模型**
   - 添加细节
   - 优化面数
   - 添加平滑

4. **导出模型**
   - `File -> Export -> FBX`
   - 保存到 `Assets/Models` 文件夹

5. **在Unity中导入**
   - Unity会自动检测新文件
   - 在Project窗口中找到导出的模型
   - 配置Import Settings：
     - Scale Factor: 1
     - 勾选"Generate Colliders"（如果需要）

6. **替换预制体**
   - 打开 `Gear.prefab`
   - 将Mesh Filter中的Mesh替换为新模型

#### 预期结果：
- 模型更精细
- 面数优化
- 视觉效果更好

---

### 3.2 添加纹理和材质

#### 操作步骤：

#### A. 创建材质文件夹
1. 在Project窗口中，右键点击 `Assets`
2. `Create -> Folder`
3. 命名为 `Materials`

#### B. 创建齿轮材质
1. **创建材质**
   - 右键点击 `Materials` 文件夹
   - `Create -> Material`
   - 命名为 `GearMaterial`

2. **配置材质**
   - 选择材质
   - 在Inspector中：
     - Albedo: 点击颜色选择器，选择金属灰色
     - Metallic: 0.8（金属质感）
     - Smoothness: 0.5

3. **应用材质到预制体**
   - 打开 `Gear.prefab`
   - 选择Mesh Renderer组件
   - 将 `GearMaterial` 拖到Materials列表的Element 0

#### C. 创建其他材质
重复上述步骤创建：
- `LeverMaterial` - 棕色木材
- `SpringMaterial` - 银色金属
- `BallMaterial` - 红色或蓝色
- `GroundMaterial` - 灰色或绿色

#### D. 添加纹理（可选）
1. **下载纹理**
   - 访问 https://opengameart.org/
   - 搜索"metal texture"或"wood texture"
   - 下载免费纹理

2. **导入纹理**
   - 将下载的图片文件复制到 `Assets/Textures` 文件夹
   - Unity会自动导入

3. **应用纹理到材质**
   - 选择材质
   - 将纹理拖到Albedo贴图槽
   - 调整Tiling和Offset

#### 预期结果：
- 所有模型都有材质
- 视觉效果更好
- 纹理正确显示

---

### 3.3 添加音效文件

#### 操作步骤：

#### A. 创建音效文件夹
1. 在Project窗口中创建文件夹 `Assets/Audio/SFX`

#### B. 下载音效
1. **访问免费音效库**
   - https://freesound.org/
   - 或 https://opengameart.org/

2. **下载需要的音效**
   - 碰撞音效：搜索"collision"、"hit"
   - 胜利音效：搜索"victory"、"success"、"win"
   - 背景音乐：搜索"background music"、"ambient"

3. **下载的文件类型**
   - 优先选择WAV或OGG格式
   - MP3也可以，但文件较大

#### C. 导入音效到Unity
1. **复制文件**
   - 将下载的音效文件复制到 `Assets/Audio/SFX` 文件夹

2. **Unity自动导入**
   - Unity会自动检测并导入音频文件
   - 等待导入完成

3. **配置音频导入设置**
   - 选择音频文件
   - 在Inspector中：
     - Load Type: Decompress On Load（小文件）或 Streaming（大文件）
     - Compression Format: Vorbis
     - Quality: 70-80

#### D. 配置BallController音效
1. **打开Ball预制体**
   - 选择 `Ball.prefab`

2. **配置音效字段**
   - 找到 `BallController` 脚本
   - 找到 `Collision Sound` 字段
   - 拖拽碰撞音效文件到该字段
   - 找到 `Goal Sound` 字段
   - 拖拽胜利音效文件到该字段

3. **添加AudioSource组件（如果没有）**
   - `Add Component -> Audio -> Audio Source`
   - 配置：
     - Play On Awake: false
     - Spatial Blend: 0（2D音效）

#### E. 配置背景音乐
1. **创建背景音乐对象**
   - 在游戏场景中，选择 `AudioManager`
   - 在Inspector中找到 `Music Source`
   - 如果没有，创建：
     - 右键点击 `AudioManager` → `Create Empty`
     - 命名为 `MusicSource`
     - 添加 `Audio Source` 组件
     - 拖拽到AudioManager的Music Source字段

2. **添加背景音乐**
   - 创建ScriptableObject或直接在代码中引用
   - 在游戏开始时调用：
     ```csharp
     AudioManager.Instance.PlayMusic(backgroundMusicClip);
     ```

#### 预期结果：
- 音效文件已导入
- 音效正确播放
- 背景音乐正常播放

---

### 3.4 完善UI视觉效果

#### 操作步骤：

#### A. 创建UI样式资源
1. **创建UI资源文件夹**
   - `Assets/UI/Sprites`
   - `Assets/UI/Fonts`

2. **添加UI背景图片**
   - 可以使用纯色图片或渐变图片
   - 创建Sprite：
     - 右键点击 `Sprites` 文件夹
     - `Create -> Sprite`（或在外部创建图片后导入）

3. **导入字体（可选）**
   - 下载中文字体（如果需要）
   - 导入到 `Fonts` 文件夹

#### B. 美化按钮
1. **选择按钮**
   - 在Hierarchy中选择任意按钮

2. **添加按钮图片**
   - 在Image组件中：
     - Source Image: 拖拽按钮背景图片
     - Image Type: Sliced（如果图片支持9-slice）

3. **设置按钮颜色**
   - 在Button组件中：
     - Normal Color: 默认颜色
     - Highlighted Color: 高亮颜色
     - Pressed Color: 按下颜色
     - Selected Color: 选中颜色
     - Disabled Color: 禁用颜色

#### C. 添加UI动画（可选）
1. **创建动画控制器**
   - 右键点击 `Assets` → `Create -> Animator Controller`
   - 命名为 `UIAnimator`

2. **添加动画**
   - 选择UI对象
   - `Window -> Animation -> Animation`
   - 创建动画：
     - 淡入淡出
     - 缩放动画
     - 滑入滑出

3. **应用动画**
   - 添加 `Animator` 组件到UI对象
   - 拖拽动画控制器到Animator组件

#### 预期结果：
- UI视觉效果更好
- 按钮有交互反馈
- 动画流畅自然

---

## 第四步：集成微信SDK

### 4.1 注册微信小程序账号

#### 操作步骤：

1. **访问微信公众平台**
   - 打开浏览器，访问 https://mp.weixin.qq.com/

2. **注册账号**
   - 点击"立即注册"
   - 选择"小程序"
   - 填写邮箱和密码
   - 验证邮箱
   - 填写账号信息

3. **获取AppID**
   - 登录小程序后台
   - 左侧菜单 → `开发 -> 开发管理 -> 开发设置`
   - 找到"AppID(小程序ID)"
   - 复制AppID（稍后使用）

4. **配置服务器域名（如果需要）**
   - `开发 -> 开发管理 -> 开发设置 -> 服务器域名`
   - 添加你的服务器域名

#### 预期结果：
- 小程序账号注册成功
- 获得AppID
- 服务器域名配置完成

---

### 4.2 下载SDK文件

#### 操作步骤：

1. **下载微信小游戏SDK**
   - 访问 https://developers.weixin.qq.com/minigame/dev/guide/
   - 下载最新的微信小游戏SDK
   - 或者搜索"微信小游戏 Unity SDK"

2. **解压SDK文件**
   - 解压到临时文件夹
   - 查看文件结构

3. **查找必要的JS文件**
   - 通常包含：
     - `weapp-adapter.js` - 适配器
     - 其他API文件

#### 预期结果：
- SDK文件已下载
- 了解SDK结构

---

### 4.3 配置WeChatBridge.js

#### 操作步骤：

1. **创建StreamingAssets文件夹**
   - 在Project窗口中，右键点击 `Assets`
   - `Create -> Folder`
   - 命名为 `StreamingAssets`

2. **复制WeChatBridge.js**
   - 项目中的 `Assets/Scripts/Network/WeChatBridge.js` 已经创建
   - 需要将其复制到WebGL构建的StreamingAssets目录

3. **修改WeChatBridge.js（如果需要）**
   - 打开 `Assets/Scripts/Network/WeChatBridge.js`
   - 检查所有API调用是否正确
   - 确保与你的微信SDK版本兼容

4. **创建小程序入口文件**
   - 在StreamingAssets中创建 `game.js`
   - 添加代码：
     ```javascript
     // 引入微信SDK
     require('./weapp-adapter.js');
     
     // Unity实例
     var unityInstance = null;
     
     // 设置Unity实例
     function SetUnityInstance(instance) {
         unityInstance = instance;
         // 初始化WeChatBridge
         if (typeof SetUnityInstance === 'function') {
             SetUnityInstance(instance);
         }
     }
     
     // 导出
     module.exports = {
         SetUnityInstance: SetUnityInstance
     };
     ```

5. **配置小程序的game.json**
   - 在小程序项目中创建 `game.json`：
     ```json
     {
         "deviceOrientation": "portrait",
         "showStatusBar": false
     }
     ```

#### 预期结果：
- WeChatBridge.js配置完成
- 小程序入口文件创建完成

---

### 4.4 测试登录和分享

#### 操作步骤：

#### A. 配置Unity构建设置

1. **切换到WebGL平台**
   - `File -> Build Settings`
   - 选择 `WebGL`
   - 点击"Switch Platform"
   - 等待平台切换完成

2. **配置Player Settings**
   - 点击"Player Settings"
   - 在Inspector中：
     - Company Name: 你的公司名
     - Product Name: 机械之谜
     - WebGL Template: 选择默认模板

3. **配置压缩格式**
   - Compression Format: Gzip
   - Code Optimization: Size

#### B. 构建WebGL版本

1. **构建项目**
   - `File -> Build Settings`
   - 点击"Build"
   - 选择输出文件夹（建议创建 `Build/WebGL`）
   - 等待构建完成

2. **检查构建结果**
   - 输出文件夹应该包含：
     - `index.html`
     - `Build` 文件夹
     - `TemplateData` 文件夹

#### C. 集成到微信小程序

1. **创建小程序项目**
   - 打开微信开发者工具
   - 新建项目
   - 填写项目信息：
     - AppID: 使用之前获得的AppID
     - 项目名称: 机械之谜
     - 项目目录: 选择小程序项目文件夹

2. **复制Unity构建文件**
   - 将Unity构建的文件复制到小程序项目的 `webgl` 文件夹

3. **修改小程序页面**
   - 创建 `pages/game/game.js`:
     ```javascript
     Page({
         onLoad: function() {
             // 加载Unity游戏
             this.loadUnityGame();
         },
         loadUnityGame: function() {
             // 使用微信小游戏API加载Unity
             // 具体实现根据Unity版本和微信SDK版本
         }
     });
     ```

#### D. 测试功能

1. **测试登录**
   - 在小程序中运行游戏
   - 点击登录按钮
   - 检查是否成功调用微信登录API
   - 查看Console日志

2. **测试分享**
   - 点击分享按钮
   - 检查是否弹出分享菜单
   - 尝试分享给好友
   - 检查分享内容是否正确

3. **调试问题**
   - 使用微信开发者工具的调试功能
   - 查看Console日志
   - 检查网络请求
   - 查看Unity日志

#### 预期结果：
- 登录功能正常
- 分享功能正常
- 没有错误信息

#### 常见问题解决：

**问题1：登录失败**
- 检查AppID是否正确
- 检查网络请求是否正常
- 查看微信开发者工具的错误信息

**问题2：分享不显示**
- 检查WeChatBridge.js是否正确加载
- 检查API调用是否正确
- 确认小程序权限设置

**问题3：Unity无法加载**
- 检查文件路径是否正确
- 检查文件大小是否超过限制
- 查看Unity构建日志

---

## 第五步：测试和优化

### 5.1 在Unity编辑器中测试

#### 操作步骤：

#### A. 测试游戏场景

1. **打开游戏场景**
   - 双击 `Assets/Scenes/Gameplay.unity`

2. **配置测试环境**
   - 确保所有预制体都已正确配置
   - 检查GameManager的引用

3. **运行测试**
   - 点击Play按钮
   - 测试以下功能：
     - 小球生成和下落
     - 齿轮旋转
     - 杠杆触发
     - 弹簧伸缩
     - 到达终点
     - UI显示和交互

4. **记录问题**
   - 在Console窗口中查看错误和警告
   - 记录需要修复的问题

#### B. 测试编辑器场景

1. **打开编辑器场景**
   - 双击 `Assets/Scenes/LevelEditor.unity`

2. **测试编辑器功能**
   - 测试部件放置
   - 测试撤销/重做
   - 测试保存/加载
   - 测试连接功能

#### C. 测试主菜单场景

1. **打开主菜单场景**
   - 双击 `Assets/Scenes/MainMenu.unity`

2. **测试菜单功能**
   - 测试关卡选择
   - 测试按钮交互
   - 测试场景切换

#### 预期结果：
- 所有功能正常工作
- 没有严重错误
- 性能可以接受

---

### 5.2 构建WebGL版本测试

#### 操作步骤：

1. **构建WebGL**
   - `File -> Build Settings`
   - 选择 `WebGL`
   - 点击"Build"
   - 等待构建完成

2. **本地测试**
   - 使用本地服务器运行构建结果
   - 或者使用Unity内置的"Build And Run"
   - 在浏览器中测试

3. **测试功能**
   - 测试所有游戏功能
   - 检查性能
   - 测试不同浏览器兼容性

4. **优化构建**
   - 如果包体太大，优化资源
   - 压缩纹理
   - 减少代码大小

#### 预期结果：
- WebGL版本可以正常运行
- 性能可以接受
- 没有明显错误

---

### 5.3 在微信小程序中测试

#### 操作步骤：

1. **上传小程序代码**
   - 在微信开发者工具中
   - 点击"上传"
   - 填写版本号和备注

2. **提交审核（可选）**
   - 如果需要正式发布
   - 在小程序后台提交审核

3. **真机测试**
   - 使用微信开发者工具的真机调试功能
   - 或者分享给测试用户

4. **测试所有功能**
   - 登录功能
   - 游戏玩法
   - 分享功能
   - 排行榜功能
   - 性能测试

#### 预期结果：
- 小程序可以正常运行
- 所有功能正常
- 性能可以接受

---

### 5.4 根据结果优化

#### 操作步骤：

#### A. 性能优化

1. **使用Unity Profiler**
   - `Window -> Analysis -> Profiler`
   - 运行游戏并记录性能数据
   - 分析瓶颈

2. **优化建议**
   - 减少Draw Call
   - 优化物理计算
   - 减少内存分配
   - 优化纹理大小

#### B. 代码优化

1. **修复bug**
   - 根据测试结果修复问题
   - 优化代码逻辑

2. **添加功能**
   - 根据测试反馈添加新功能
   - 改进用户体验

#### C. 资源优化

1. **压缩资源**
   - 压缩纹理
   - 压缩音频
   - 优化模型

2. **移除无用资源**
   - 删除未使用的资源
   - 清理项目

#### 预期结果：
- 性能提升
- 包体减小
- 用户体验改善

---

## 完成检查清单

使用以下清单确认所有步骤都已完成：

### 资源制作 ✅
- [ ] 所有预制体已生成
- [ ] 材质已创建并应用
- [ ] 纹理已导入和应用
- [ ] 音效已导入和配置

### 场景配置 ✅
- [ ] 所有场景已创建
- [ ] 场景对象已正确配置
- [ ] UI已创建和配置
- [ ] 脚本引用已连接

### 微信集成 ✅
- [ ] 小程序账号已注册
- [ ] AppID已获取
- [ ] SDK已集成
- [ ] 登录功能正常
- [ ] 分享功能正常

### 测试完成 ✅
- [ ] 编辑器测试通过
- [ ] WebGL构建测试通过
- [ ] 微信小程序测试通过
- [ ] 性能优化完成

---

## 遇到问题？

如果遇到问题，请：
1. 检查Console窗口的错误信息
2. 查看相关文档
3. 搜索Unity官方文档
4. 查看微信开发者文档

祝开发顺利！🎮

