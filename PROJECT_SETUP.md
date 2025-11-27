# 《机械之谜》项目设置指南

## Unity版本要求
- Unity 2021.3 LTS 或更高版本

## 项目结构说明

### 核心脚本
- `Assets/Scripts/Core/` - 核心机械部件（齿轮、杠杆、弹簧）
- `Assets/Scripts/Gameplay/` - 游戏玩法逻辑（小球控制、游戏管理）
- `Assets/Scripts/Editor/` - 关卡编辑器
- `Assets/Scripts/UI/` - 用户界面
- `Assets/Scripts/Network/` - 网络和社交功能
- `Assets/Scripts/Utils/` - 工具类

## 设置步骤

### 1. 创建场景

#### 主菜单场景
1. 创建新场景 `Scenes/MainMenu.unity`
2. 添加 `NetworkManager` 和 `SceneLoader` 对象

#### 游戏场景
1. 创建新场景 `Scenes/Gameplay.unity`
2. 设置场景：
   - 添加地面（Plane）
   - 添加光源（Directional Light）
   - 添加主摄像机
   - 添加 `GameManager` 对象
   - 添加 `LevelManager` 对象
   - 添加 `PerformanceOptimizer` 对象
   - 添加 `ObjectPool` 对象

#### 编辑器场景
1. 创建新场景 `Scenes/LevelEditor.unity`
2. 设置场景：
   - 添加 `LevelEditor` 对象
   - 添加 `EditorUI` 对象
   - 添加 `ComponentConnector` 对象

### 2. 创建预制体

#### 小球预制体
1. 创建球体（Sphere）
2. 添加组件：
   - `Rigidbody`（质量：1，阻力：0.5）
   - `SphereCollider`（半径：0.5）
   - `BallController`
3. 设置标签为 "Ball"
4. 保存为 `Prefabs/Ball.prefab`

#### 齿轮预制体
1. 创建圆柱体（Cylinder）或导入齿轮模型
2. 添加组件：
   - `Rigidbody`
   - `HingeJoint`
   - `Gear`
   - `Collider`（Mesh Collider 或 Capsule Collider）
3. 保存为 `Prefabs/Gear.prefab`

#### 杠杆预制体
1. 创建立方体（Cube）或导入杠杆模型
2. 添加组件：
   - `Rigidbody`
   - `Lever`
   - `BoxCollider`（设置为Trigger）
3. 保存为 `Prefabs/Lever.prefab`

#### 弹簧预制体
1. 创建球体或导入弹簧模型
2. 添加组件：
   - `Rigidbody`
   - `SpringJoint`
   - `Spring`
   - `Collider`
   - `LineRenderer`（用于可视化）
3. 保存为 `Prefabs/Spring.prefab`

#### 终点预制体
1. 创建球体或特殊模型
2. 添加组件：
   - `Collider`（设置为Trigger）
   - 设置标签为 "Goal"
3. 保存为 `Prefabs/Goal.prefab`

#### 检查点预制体
1. 创建标记模型
2. 添加组件：
   - `Collider`（设置为Trigger）
   - `Checkpoint`
   - 设置标签为 "Checkpoint"
3. 保存为 `Prefabs/Checkpoint.prefab`

### 3. 配置对象池

在 `ObjectPool` 对象上配置：
- 添加小球到对象池（tag: "Ball", size: 5）
- 根据需要添加其他对象

### 4. 设置物理参数

在 `PerformanceOptimizer` 对象上配置：
- Max Solver Iterations: 6
- Max Depenetration Velocity: 10
- Enable Sleeping: true
- Target Frame Rate: 60

### 5. 配置关卡编辑器

在 `LevelEditor` 对象上配置：
- 将创建的预制体添加到 `Component Prefabs` 数组
- 设置 `Grid Size`: 1
- 设置 `Snap To Grid`: true

### 6. UI设置

#### 游戏UI
1. 创建Canvas
2. 添加 `GameUI` 组件
3. 创建UI元素：
   - 时间文本
   - 最佳时间文本
   - 暂停按钮
   - 重置按钮

#### 胜利UI
1. 在Canvas下创建胜利面板
2. 添加 `VictoryUI` 组件
3. 创建UI元素：
   - 完成时间文本
   - 最佳时间文本
   - 排名文本
   - 下一关按钮
   - 重试按钮
   - 分享按钮
   - 排行榜按钮

#### 编辑器UI
1. 在编辑器场景创建Canvas
2. 添加 `EditorUI` 组件
3. 创建UI元素：
   - 部件库容器
   - 保存/加载按钮
   - 播放按钮
   - 撤销/重做按钮
   - 网格设置

### 7. 微信小程序集成

#### 在NetworkManager中配置：
- 设置服务器URL
- 实现微信登录回调
- 实现分享功能

#### 微信API集成代码示例：
```javascript
// 在微信小程序环境中
wx.login({
  success: function(res) {
    if (res.code) {
      // 发送到Unity
      SendMessageToUnity('NetworkManager', 'OnWeChatLogin', res.code);
    }
  }
});

wx.showShareMenu({
  withShareTicket: true
});

wx.onShareAppMessage(function() {
  return {
    title: "挑战我的机械谜题",
    path: `/pages/play/play?code=${levelCode}`,
    imageUrl: "生成的谜题预览图"
  };
});
```

### 8. 构建设置

#### WebGL构建（微信小程序）
1. File -> Build Settings
2. 选择 WebGL 平台
3. Player Settings:
   - Compression Format: Gzip
   - Code Optimization: Size
   - Strip Engine Code: true
4. 发布到微信小程序目录

#### 性能优化
1. 纹理压缩：
   - Android: ETC1/ATC
   - iOS: PVRTC
2. 模型优化：
   - 使用LOD
   - 减少多边形数量
3. 代码优化：
   - 启用IL2CPP
   - 代码剥离

## 测试清单

- [ ] 齿轮可以正常旋转
- [ ] 杠杆可以被小球触发
- [ ] 弹簧可以正常伸缩
- [ ] 小球可以到达终点
- [ ] 关卡编辑器可以保存和加载
- [ ] 排行榜可以显示
- [ ] 分享功能正常
- [ ] 性能优化生效

## 常见问题

### 物理模拟不稳定
- 检查Time.fixedDeltaTime设置
- 增加Solver Iterations
- 检查碰撞器设置

### 对象池不工作
- 确保ObjectPool在场景中存在
- 检查tag是否正确
- 确保预制体已配置

### 关卡保存失败
- 检查文件写入权限
- 确保LevelManager已初始化
- 检查JSON序列化是否正确

## 下一步开发

1. 添加更多机械部件（传送带、滑轮等）
2. 实现关卡难度系统
3. 添加音效和背景音乐
4. 实现成就系统
5. 优化移动端性能

