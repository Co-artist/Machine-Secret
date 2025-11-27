# 机械之谜 - 3D物理谜题游戏

## 项目简介
《机械之谜》是一款基于Unity开发的3D物理谜题游戏，玩家需要通过设计机械装置（齿轮、杠杆、弹簧等）引导小球到达终点。游戏支持关卡编辑器、竞技模式、排行榜和好友挑战等功能。

## 项目结构
```
Assets/
├── Scripts/
│   ├── Core/              # 核心机械部件
│   │   ├── Gear.cs        # 齿轮系统
│   │   ├── Lever.cs       # 杠杆系统
│   │   └── Spring.cs      # 弹簧系统
│   ├── Editor/            # 关卡编辑器
│   │   ├── LevelEditor.cs # 编辑器核心
│   │   └── ComponentConnector.cs # 部件连接
│   ├── Gameplay/          # 游戏玩法逻辑
│   │   ├── BallController.cs # 小球控制
│   │   ├── GameManager.cs    # 游戏管理
│   │   ├── Checkpoint.cs     # 检查点
│   │   ├── ChallengeManager.cs # 挑战管理
│   │   └── LevelSelector.cs   # 关卡选择
│   ├── UI/                # 用户界面
│   │   ├── GameUI.cs      # 游戏UI
│   │   ├── VictoryUI.cs   # 胜利UI
│   │   ├── EditorUI.cs    # 编辑器UI
│   │   └── LeaderboardUI.cs # 排行榜UI
│   ├── Network/           # 网络和社交功能
│   │   └── NetworkManager.cs # 网络管理
│   └── Utils/             # 工具类
│       ├── LevelData.cs   # 关卡数据
│       ├── LevelManager.cs # 关卡管理
│       ├── ObjectPool.cs  # 对象池
│       ├── PerformanceOptimizer.cs # 性能优化
│       ├── ResourceManager.cs # 资源管理
│       ├── SceneLoader.cs # 场景加载
│       ├── AudioManager.cs # 音频管理
│       ├── GameConfig.cs  # 游戏配置
│       └── JsonHelper.cs  # JSON辅助
├── Prefabs/               # 预制体（需创建）
├── Materials/             # 材质（需创建）
├── Textures/              # 纹理（需创建）
└── Scenes/                # 场景文件（需创建）
```

## 核心功能

### ✅ 已实现
- ✅ 3D物理谜题系统（齿轮、杠杆、弹簧）
- ✅ 小球引导和重置系统
- ✅ 可视化关卡编辑器（放置、旋转、撤销/重做）
- ✅ 关卡保存/加载（JSON + Base64编码）
- ✅ 竞技模式和排行榜系统
- ✅ 好友挑战机制
- ✅ 性能优化（对象池、资源管理）
- ✅ 完整的UI系统
- ✅ 音频管理系统

### 🚧 待完成
- [ ] 3D模型和材质制作
- [ ] 场景配置和设置
- [ ] 微信小程序SDK集成
- [ ] 音效和背景音乐
- [ ] UI预制体制作

## 技术栈
- **引擎**: Unity 2021.3 LTS+
- **语言**: C#
- **物理**: Unity PhysX
- **平台**: 微信小程序（WebGL）
- **数据格式**: JSON

## 快速开始

### 1. 环境要求
- Unity 2021.3 LTS 或更高版本
- Visual Studio 或 Rider（推荐）

### 2. 项目设置
详细设置步骤请参考 [PROJECT_SETUP.md](PROJECT_SETUP.md)

### 3. 使用指南
详细使用说明请参考 [USAGE.md](USAGE.md)

### 4. 实现总结
完整功能列表请参考 [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)

## 核心系统说明

### 机械部件系统
- **齿轮**: 使用HingeJoint实现旋转，支持齿轮间动力传递
- **杠杆**: 使用FixedJoint固定支点，通过力矩计算触发
- **弹簧**: 使用SpringJoint实现弹性连接，可配置力和阻尼

### 关卡编辑器
- 可视化部件放置
- 网格对齐和旋转
- 撤销/重做功能
- JSON格式保存/加载
- Base64编码分享

### 竞技模式
- 排行榜系统（好友和全球）
- 挑战机制
- 成绩上传和比较
- 微信分享集成

## 开发进度

### 核心系统 ✅
- [x] 项目结构搭建
- [x] 核心机械部件实现
- [x] 小球引导逻辑
- [x] 关卡编辑器开发
- [x] 数据管理系统
- [x] 竞技模式实现
- [x] 性能优化系统
- [x] UI系统

### 资源制作 🚧
- [ ] 3D模型制作
- [ ] 材质和纹理
- [ ] 音效和音乐
- [ ] UI资源

### 集成和测试 🚧
- [ ] 微信SDK集成
- [ ] 场景配置
- [ ] 功能测试
- [ ] 性能测试

## 文档

- [PROJECT_SETUP.md](PROJECT_SETUP.md) - 项目设置指南
- [USAGE.md](USAGE.md) - 使用指南
- [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) - 实现总结

## 许可证
本项目仅供学习和开发使用。

## 贡献
欢迎提交Issue和Pull Request！

