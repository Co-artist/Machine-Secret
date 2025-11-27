# 《机械之谜》使用指南

## 快速开始

### 1. 打开项目
1. 使用Unity 2021.3 LTS或更高版本打开项目
2. 等待Unity导入所有资源

### 2. 创建基础场景
按照 `PROJECT_SETUP.md` 中的步骤创建场景和预制体

### 3. 运行游戏
1. 打开 `Scenes/MainMenu.unity` 场景
2. 点击Play按钮开始游戏

## 核心功能使用

### 齿轮系统

#### 创建齿轮
1. 在场景中放置齿轮预制体
2. 在Inspector中配置：
   - `Rotation Speed`: 旋转速度（度/秒）
   - `Motor Force`: 电机力
   - `Auto Rotate`: 是否自动旋转

#### 连接齿轮
1. 在关卡编辑器中，选择"连接"模式
2. 点击第一个齿轮，再点击第二个齿轮
3. 两个齿轮会自动建立连接关系

### 杠杆系统

#### 创建杠杆
1. 放置杠杆预制体
2. 设置支点位置（Pivot Point）
3. 配置触发参数：
   - `Trigger Force`: 触发所需力
   - `Max Angle`: 最大旋转角度
   - `Trigger Layer`: 触发层（通常是小球层）

#### 连接杠杆到其他部件
1. 在Inspector中，将目标对象拖入 `Connected Objects` 数组
2. 杠杆被触发后会自动激活连接的对象

### 弹簧系统

#### 创建弹簧
1. 放置弹簧预制体
2. 设置连接点（Connected Point）
3. 配置参数：
   - `Spring Force`: 弹簧力
   - `Damper`: 阻尼
   - `Min/Max Distance`: 距离范围

### 关卡编辑器

#### 放置部件
1. 打开关卡编辑器场景
2. 在部件库中选择要放置的部件
3. 在场景中点击放置
4. 使用鼠标滚轮旋转部件

#### 保存关卡
1. 输入关卡名称
2. 点击"保存"按钮
3. 关卡数据会保存到 `Application.persistentDataPath/Levels/`

#### 加载关卡
1. 点击"加载"按钮
2. 选择要加载的关卡
3. 关卡会从文件加载并显示在场景中

#### 撤销/重做
- `Ctrl + Z`: 撤销
- `Ctrl + Y`: 重做

### 游戏玩法

#### 开始游戏
1. 从主菜单选择关卡
2. 点击"开始"按钮
3. 小球会自动生成并开始移动

#### 重置小球
- 点击"重置"按钮
- 小球会回到起始位置

#### 暂停游戏
- 点击"暂停"按钮
- 或按 `ESC` 键

### 竞技模式

#### 查看排行榜
1. 完成关卡后，点击"排行榜"按钮
2. 查看好友和全球排名
3. 显示最佳时间和排名

#### 分享关卡
1. 在胜利界面点击"分享"按钮
2. 生成关卡代码
3. 通过微信分享给好友

#### 接受挑战
1. 收到好友挑战后，点击通知
2. 加载挑战关卡
3. 完成后比较成绩

## 代码示例

### 创建自定义机械部件

```csharp
using UnityEngine;

public class CustomComponent : MonoBehaviour
{
    [Header("自定义参数")]
    public float customValue = 10f;
    
    void Start()
    {
        // 初始化代码
    }
    
    public void Activate()
    {
        // 激活逻辑
    }
}
```

### 在代码中加载关卡

```csharp
// 加载关卡
LevelData levelData = LevelManager.Instance.LoadLevel(levelId);

// 从代码加载
string levelCode = "base64编码的关卡数据";
LevelData levelData = LevelManager.Instance.LoadLevelFromCode(levelCode);
```

### 使用对象池

```csharp
// 从对象池获取对象
GameObject ball = ObjectPool.Instance.SpawnFromPool("Ball", position, rotation);

// 归还对象
ObjectPool.Instance.ReturnToPool("Ball", ball);
```

## 性能优化建议

### 物理优化
1. 使用 `PerformanceOptimizer` 调整物理参数
2. 启用刚体睡眠模式
3. 限制物理计算频率

### 渲染优化
1. 使用LOD系统
2. 压缩纹理
3. 减少Draw Call

### 内存优化
1. 使用对象池管理频繁创建的对象
2. 及时卸载未使用的资源
3. 使用AssetBundle按需加载

## 常见问题解决

### 问题：齿轮不旋转
**解决方案：**
1. 检查HingeJoint是否正确配置
2. 确认电机已启用
3. 检查连接关系是否正确

### 问题：杠杆不触发
**解决方案：**
1. 检查触发层设置
2. 确认小球有足够的力
3. 检查碰撞器设置

### 问题：关卡保存失败
**解决方案：**
1. 检查文件写入权限
2. 确认LevelManager已初始化
3. 检查JSON序列化是否正确

### 问题：性能问题
**解决方案：**
1. 使用PerformanceOptimizer优化设置
2. 减少同时活动的物理对象
3. 使用对象池减少内存分配

## 扩展开发

### 添加新机械部件
1. 创建新的脚本继承MonoBehaviour
2. 实现必要的物理组件
3. 添加到部件库预制体数组
4. 在编辑器中配置参数

### 添加新关卡类型
1. 创建新的关卡数据结构
2. 实现加载和保存逻辑
3. 添加UI支持

### 集成第三方服务
1. 在NetworkManager中添加API调用
2. 实现数据序列化
3. 处理网络错误

## 技术支持

如有问题，请参考：
- `PROJECT_SETUP.md` - 项目设置指南
- Unity官方文档
- 代码注释

