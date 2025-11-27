# 自动设置工具使用指南

## 🎯 功能说明

`AutoSetup.cs` 是一个综合自动化工具，可以一键完成所有项目设置：

1. ✅ **设置标签和层级**（Ball, Goal, Checkpoint, Hazard）
2. ✅ **创建所有文件夹**（Materials, UI, Textures等）
3. ✅ **生成所有资源**（提示使用ResourceGenerator）
4. ✅ **创建所有材质**（齿轮、杠杆、弹簧、小球、地面）
5. ✅ **创建所有场景**（MainMenu, Gameplay, LevelEditor）
6. ✅ **配置场景对象**（自动添加所有必要的GameObject和组件）

---

## 🚀 快速使用

### 方法1：一键完成所有设置（推荐）

1. **打开工具**
   ```
   Tools -> 自动设置工具 -> 一键完成所有设置
   ```

2. **点击按钮**
   - 点击窗口中的 **"开始自动设置"** 按钮

3. **等待完成**
   - 工具会自动执行所有步骤
   - 显示进度条
   - 完成后会弹出提示

4. **检查结果**
   - 检查Project窗口中的资源
   - 检查Scenes文件夹中的场景
   - 检查Materials文件夹中的材质

---

### 方法2：单独执行某个步骤

如果只需要执行某个特定步骤：

1. **打开工具**
   ```
   Tools -> 自动设置工具 -> 一键完成所有设置
   ```

2. **点击对应的按钮**
   - **仅创建场景** - 只创建场景
   - **仅生成资源** - 只生成资源（需要手动运行ResourceGenerator）
   - **仅设置标签** - 只设置标签
   - **仅创建材质** - 只创建材质

---

## 📋 详细步骤说明

### 步骤1：打开工具

1. **在Unity顶部菜单栏**
   - 点击 `Tools`
   - 选择 `自动设置工具`
   - 点击 `一键完成所有设置`

2. **工具窗口会打开**
   - 显示工具界面
   - 有主要按钮和单独选项

### 步骤2：执行自动设置

1. **点击"开始自动设置"按钮**

2. **工具会自动执行**：
   - 设置标签和层级（10%）
   - 创建文件夹（20%）
   - 生成资源（40%）
   - 创建材质（60%）
   - 创建场景（80%）
   - 刷新资源（90%）

3. **完成后**
   - 弹出提示对话框
   - 显示完成信息

### 步骤3：验证结果

1. **检查标签**
   - `Edit -> Project Settings -> Tags and Layers`
   - 应该能看到：Ball, Goal, Checkpoint, Hazard

2. **检查文件夹**
   - `Assets/Materials/` - 应该存在
   - `Assets/UI/` - 应该存在
   - `Assets/Scenes/` - 应该存在

3. **检查材质**
   - `Assets/Materials/GearMaterial.mat`
   - `Assets/Materials/LeverMaterial.mat`
   - `Assets/Materials/SpringMaterial.mat`
   - `Assets/Materials/BallMaterial.mat`
   - `Assets/Materials/GroundMaterial.mat`

4. **检查场景**
   - `Assets/Scenes/MainMenu.unity`
   - `Assets/Scenes/Gameplay.unity`
   - `Assets/Scenes/LevelEditor.unity`

---

## 🎨 自动创建的内容

### 1. 标签和层级

自动添加以下标签：
- **Ball** - 小球标签
- **Goal** - 终点标签
- **Checkpoint** - 检查点标签
- **Hazard** - 危险区域标签

### 2. 文件夹结构

自动创建以下文件夹：
- `Assets/Materials/` - 材质文件夹
- `Assets/UI/` - UI资源文件夹
- `Assets/UI/Sprites/` - UI图片文件夹
- `Assets/UI/Fonts/` - 字体文件夹
- `Assets/Textures/` - 纹理文件夹

### 3. 材质

自动创建以下材质：
- **GearMaterial** - 齿轮材质（金属灰色）
- **LeverMaterial** - 杠杆材质（木材棕色）
- **SpringMaterial** - 弹簧材质（金属银色）
- **BallMaterial** - 小球材质（红色）
- **GroundMaterial** - 地面材质（浅灰色）

### 4. 场景

自动创建以下场景：
- **MainMenu** - 主菜单场景
- **Gameplay** - 游戏场景
- **LevelEditor** - 关卡编辑器场景

每个场景都包含：
- 必要的GameObject
- 必要的组件
- Canvas和UI结构
- 光源和地面

---

## ⚠️ 注意事项

### 1. 资源生成

工具会提示你手动运行ResourceGenerator来生成资源：
- 齿轮预制体
- 杠杆预制体
- 弹簧预制体
- 小球预制体

**操作**：
```
Tools -> 资源生成器 -> 生成齿轮/杠杆/弹簧/小球
```

### 2. 场景覆盖

如果场景已存在，工具会创建新场景并覆盖旧场景。

**建议**：
- 如果场景已存在且已修改，先备份
- 或使用单独选项只创建缺失的场景

### 3. 材质覆盖

如果材质已存在，工具会跳过创建。

**如果需要重新创建**：
- 手动删除现有材质
- 或修改材质名称

---

## 🔧 故障排除

### 问题1：工具窗口打不开

**解决**：
1. 检查脚本是否编译成功
2. 查看Console窗口是否有错误
3. 确保脚本在 `Assets/Scripts/Editor/` 文件夹中

### 问题2：某些步骤失败

**解决**：
1. 查看Console窗口的错误信息
2. 尝试单独执行失败的步骤
3. 检查Unity版本（需要2021.3+）

### 问题3：场景创建失败

**解决**：
1. 确保有写入权限
2. 检查Scenes文件夹是否存在
3. 尝试手动创建Scenes文件夹

---

## 📝 使用建议

### 首次使用

1. **运行一键设置**
   - 完成所有基础设置

2. **手动生成资源**
   - 运行ResourceGenerator生成预制体

3. **配置GameManager**
   - 在Gameplay场景中配置Ball预制体引用

4. **测试游戏**
   - 运行游戏测试功能

### 后续使用

- 如果需要重新设置，可以再次运行工具
- 如果只需要某个部分，使用单独选项
- 场景和材质会自动应用

---

## 🎯 快速操作清单

- [ ] 打开工具：`Tools -> 自动设置工具 -> 一键完成所有设置`
- [ ] 点击"开始自动设置"按钮
- [ ] 等待完成
- [ ] 检查标签是否已设置
- [ ] 检查材质是否已创建
- [ ] 检查场景是否已创建
- [ ] 运行ResourceGenerator生成资源
- [ ] 配置GameManager的预制体引用
- [ ] 测试游戏

---

**使用自动设置工具，可以快速完成所有基础设置！** 🚀

