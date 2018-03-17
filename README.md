# ARdestructibleTerrain demo 说明文档



---
## 功能
+ 在 AR 模式下通过射击炸弹，实现对场景内建筑与地形的物理破坏效果

## 使用
+ Unity Editor:
    - 进入 Assets -> Test -> Scene -> Test.scene
    - Play mode 下，直接点击 **"Confirm"**按钮，生成游戏场景，以可旋转的第一人称视角俯瞰整个场景，按下 **空格键** 射击

+ iOS Device:
    - 对应包名为 “Bomb”
    - 点击 **"StartSession"** 开启会话，平面出现后点击 **"Confirm"**，生成游戏场景
    - 左下角的白色圆形按钮为射击键

NOTE: 业务层代码直接在编辑器环境下测试即可

## 脚本
+ ARPreSetting.cs
    > 进入游戏前对 AR 环境的配置，主要包括开启会话与平面确认

+ BombExplosion.cs
    > 控制所有炸弹的爆炸效果，主要包括：
    - 弹坑的生成(对 Terrain 高度与纹理的改变)
    - 对建筑的击碎效果(借助插件实现爆炸物理效果)
    - 爆炸粒子效果
    - 炸弹本身击飞的物理效果

+ CreateBomb.cs
    > 炸弹的射击功能，包括：
    - 编辑器下：空格
    - 移动设备下：射击 button

+ PlayerLookAround.cs
    > 编辑器下，通过移动鼠标旋转视角

+ TerrainInsurance.cs
    > 保证 Terrain 地形数据在退出 Playe mode 后恢复至初始状态




