# 快速参考指南

## 核心类快速索引

### 机械部件
- `Gear` - 齿轮组件，实现旋转和联动
- `Lever` - 杠杆组件，实现触发机制
- `Spring` - 弹簧组件，实现弹性连接

### 游戏玩法
- `BallController` - 小球控制器
- `GameManager` - 游戏管理器
- `Checkpoint` - 检查点系统

### 编辑器
- `LevelEditor` - 关卡编辑器核心
- `ComponentConnector` - 部件连接器
- `EditorUI` - 编辑器UI

### 数据管理
- `LevelManager` - 关卡管理器（单例）
- `LevelData` - 关卡数据结构
- `ResourceManager` - 资源管理器（单例）

### 网络和社交
- `NetworkManager` - 网络管理器（单例）
- `ChallengeManager` - 挑战管理器（单例）
- `LeaderboardUI` - 排行榜UI

### 工具类
- `ObjectPool` - 对象池（单例）
- `PerformanceOptimizer` - 性能优化器
- `AudioManager` - 音频管理器（单例）
- `SceneLoader` - 场景加载器

## 常用API

### 关卡管理
```csharp
// 保存关卡
LevelManager.Instance.SaveLevel(levelData, levelId);

// 加载关卡
LevelData data = LevelManager.Instance.LoadLevel(levelId);

// 获取关卡代码
string code = LevelManager.Instance.GetLevelCode(levelData);

// 从代码加载
LevelData data = LevelManager.Instance.LoadLevelFromCode(code);
```

### 对象池
```csharp
// 获取对象
GameObject obj = ObjectPool.Instance.SpawnFromPool("Ball", pos, rot);

// 归还对象
ObjectPool.Instance.ReturnToPool("Ball", obj);
```

### 网络功能
```csharp
// 登录
NetworkManager.Instance.WeChatLogin((success) => { });

// 上传成绩
NetworkManager.Instance.UploadScore(levelId, time);

// 获取排行榜
NetworkManager.Instance.GetLeaderboard(levelId, (data) => { });

// 分享关卡
NetworkManager.Instance.ShareLevel(levelCode);
```

### 音频管理
```csharp
// 播放音乐
AudioManager.Instance.PlayMusic(clip);

// 播放音效
AudioManager.Instance.PlaySFX(clip);
AudioManager.Instance.PlaySFX("clipName");

// 设置音量
AudioManager.Instance.SetMasterVolume(0.8f);
```

## 标签和层级

### 标签（Tags）
- `Ball` - 小球
- `Goal` - 终点
- `Checkpoint` - 检查点
- `Hazard` - 危险区域

### 层级（Layers）
- 设置小球专用层用于触发检测
- 设置放置层用于编辑器

## 关键参数

### 齿轮参数
- `rotationSpeed` - 旋转速度（度/秒）
- `motorForce` - 电机力
- `autoRotate` - 自动旋转

### 杠杆参数
- `triggerForce` - 触发所需力
- `maxAngle` - 最大旋转角度
- `leverArmLength` - 杠杆臂长度

### 弹簧参数
- `springForce` - 弹簧力
- `damper` - 阻尼
- `minDistance` / `maxDistance` - 距离范围

### 小球参数
- `mass` - 质量
- `drag` - 阻力
- `resetDelay` - 重置延迟

## 文件路径

### 关卡存储
```
Application.persistentDataPath/Levels/level_{id}.json
```

### 资源路径
```
Resources/Audio/SFX/{clipName}
Resources/GameConfig
```

## 事件系统

### GameManager事件
```csharp
gameManager.OnTimeUpdate += UpdateTime;
gameManager.OnGameComplete += OnComplete;
gameManager.OnGameStart += OnStart;
```

## 调试技巧

### 物理调试
- 使用Unity Scene视图查看物理连接
- 启用Gizmos查看碰撞器
- 使用Physics Debug窗口

### 性能调试
- 使用Unity Profiler分析性能
- 检查对象池使用情况
- 监控内存分配

### 关卡调试
- 使用关卡代码快速测试
- 检查JSON序列化是否正确
- 验证部件连接关系

## 常见问题

### Q: 齿轮不旋转？
A: 检查HingeJoint配置，确认电机已启用，检查连接关系。

### Q: 杠杆不触发？
A: 检查触发层设置，确认小球有足够力，检查碰撞器。

### Q: 关卡保存失败？
A: 检查文件权限，确认LevelManager已初始化。

### Q: 性能问题？
A: 使用PerformanceOptimizer，减少物理对象，使用对象池。

## 扩展开发

### 添加新部件
1. 创建脚本继承MonoBehaviour
2. 实现物理组件
3. 添加到部件库
4. 配置参数

### 添加新功能
1. 创建新的管理器类
2. 实现单例模式（如需要）
3. 添加UI支持
4. 集成到现有系统

