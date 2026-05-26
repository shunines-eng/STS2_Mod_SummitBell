# 山铃花之诗（sts2 ver.0.103.4）
## 卡牌思路
 暂无
##  mod学习
### 项目基础内容
本mod使用BaseLib作为框架辅助开发，学习阶段跟随教程**https://tutorials.sts2modding.com/docs/** 开发。感谢所有先行者的努力付出!
#### 项目解析
推荐项目目录如下:  
```
<modId>
|--Script
|--|--Cards
|--|--Character
|--|--Pools
|--|--Potion
|--|--Relics
|--|--Entry.cs
|--<modID>
|--|--images
|--|--localization
|--|--scenes
|--|--svg
|--|--background.tscn
<modId>.json
```
**注意**：
* 这些文件是在你实现一个角色时，除去项目初始化外文件夹。但实际上你可以对于除了modID以外文件随意放置，但不利于你的后续开发。初学还是建议按照格式生成。
* 关于modID：这是你json文件中id:后的值,也是你json文件本身的名字，大小写敏感!一定要大小写完全一致，不然本地化（翻译）会出问题而且不报错(BaseLib>=3.1.2版本特性)。
---
#### 阶段性学习问题
##### 项目立项阶段|2026.4：
> **模组不能被游戏识别**  
主要在于json文件中BaseLib依赖版本的处理,版本更新有时候会修改注册逻辑，以sts2 ver0.99.3和ver0.103.4版本为对比  

ver.0.99.3
```json
"dependencies": [{ "id": "BaseLib", "min_version": "3.1.2" }],
# 新版本
```
ver 0.103.4

```json
"dependencies": [{"BaseLib"}],
# 旧版本
```
往后的版本以新版本描述为首，但也不排除以后会继续修改，模组加载出问题时可以从此方面入手  
##### 基础内容学习阶段（2026.5）   
- **尝试学习制作所有内容的测试**  
包括：角色、卡牌、遗物、药水  
按照上述超链接教程，实现一个最基础的可运行的项目。我针对上述项目作一些最小化补充（2026/5/25，之后教程作者可能会自己补充），帮助你在根据超链接教程的学习时能做出一个最小化可以运作，而不是在游戏中玩到一半黑屏的测试模组。  
  
但首先，你要先学会看日志。原版提供了游戏中**波浪键**打开控制台，然后输入log open来查阅，但BaseLib提供的日志更加直观,从**游戏首页-模组配置-勾选启动时打开日志窗口**可以打开。
下面只附加一些我遭遇过的问题，这些问题都是由我查看日志信息，以及比对其他作者mod的反编译学习而来。这里感谢模组STEVE的作者，（MOD链接）**https://www.nexusmods.com/slaythespire2/mods/960**
* 问题1: 战斗胜利后黑屏： 看日志就能知道你的卡池中卡片不够多，要在教程为你实现了一张卡的基础上多设置几张，最后每个稀有度都有3张牌，能解决大部分事件，当然测试时最好不要走事件，避免走商店，但部分情况下你完全可以通过控制台来实现卡牌测试。  
* 问题2：欧洛巴斯（大眼）提供的欧洛巴斯之触/牙锁定，提示无初始遗物/手牌。
```
（附控制台命令）
relic add TOUCH_OF_OROBAS
relic add relic add TOUCH_OF_OROBAS
#分别是获取欧洛巴斯之触和先古之牙

```
在教程教程作者提供的代码，如果你完全实现，你仍然会出现两个问题。  
在百科大全中你能看到卡牌，但他不是先古牌。在遇到大眼，或者通过控制台调用**牙**和**触**，发现没变化。  
参考我的项目：  
**先古卡**：Scripts/Cards/TestCard.cs和其先古版本Scripts/Cards/TestCardITranscendece.cs  
1-你要在初始版本里上接口ITranscendenceCard而不是其先古版本上实现。  
```c#
public class TestCard : CustomCardModel ,ITranscendenceCard
```
2-修改参数rarity = CardRarity.Basic,将其归类为你的初始卡牌（其他所有基础卡也是），这样才不会在卡牌奖励卡池中抓到角色初始卡。
```c#
    private const CardRarity rarity = CardRarity.Basic;
```
3- 绑定其对应先古卡
```c#
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<TestCardITranscendence>();
```
4- 对应先古卡也要修改参数rarity = CardRarity.Ancient  
```c#
    private const CardRarity rarity = CardRarity.Ancient;
```

**先古遗物**：Scripts/Relics/TestRelics.cs和其先古版本Scripts/Cards/TestRelicsUpdate.cs  
问题是相似的，修改变量    
```c#
    public override RelicRarity Rarity => RelicRarity.Starter;
```
和初始遗物绑定先古遗物即可，不再过多解释
```c#
    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<TestRelicUpgrade>();
```
3） 如果你和我一样不够严谨，你可能还会遇到一个特别的问题。卡面无中文描述，显示xxcard.title等。  
这个状况则是上文所说的id不一致，按说明修改即可。

###### 代码学习阶段(2026.5-Forever)

**常用代码参数详解**  
1. 卡牌稀有度
>     private const CardRarity rarity = CardRarity.?;    

| 稀有度   | ? |
| ------- | -----|
| 初始卡牌|Basic|
| 先古卡| Ancient|
| 普通牌| Common|
| 罕见牌| Uncommon |
| 稀有牌| Rare |
|状态牌|Status|
|诅咒牌|Curse|
|衍生牌(类似骨妹的灵魂)|Token(未证实)|

2. 遗物稀有度
>    public override RelicRarity Rarity => RelicRarity.Starter;  

| 稀有度   | ? |
| ------- | -----|
| 初始遗物|Starter|
| 普通（白）| Common|
| 罕见（蓝）| Uncommon|
| 稀有（金）| Rare |
|事件牌|Event|
|商店专属|Shop|
|问号专属|Quest|
|诅咒|Curse|

3.卡牌类型
技能牌 Skill

4.目标类型
指向形 Targert
无目标 None

5. 注意事项,教程中说过改变代码,但是不动资源可以不更新pck.  
但实践下来,如果你改过与游戏内语言有关的json文件,也是要更新pck的,因为这些代码也被作为资源存放在了pck,这就是为什么反编译学习代码时找不到与localization有关的任何代码.这导致了学习汉化难度变高了一些.

6. 关于打出卡牌时的效果逻辑：  
以对所有敌人施加虚弱、易伤、降力为例子  

```c#
		foreach (Creature enemy in base.CombatState.HittableEnemies)
		{
			await PowerCmd.Apply<VulnerablePower>(ctx,enemy, enemyVulnerable, base.Owner.Creature, this);
            await PowerCmd.Apply<WeakPower>(ctx,enemy, enemyWeak, base.Owner.Creature, this);
            await PowerCmd.Apply<StrengthPower>(ctx,enemy, selfStrength, base.Owner.Creature, this);
        }
```
>     protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
这一句在测试服是这样写的，正式服（v.103）要把ctx去掉，建议是在观察源码时尽可能的查看你写的那个版本的sts2.dll  

7. 教程没有提供效果类的json翻译格式。我解决的方式是这样的。
> 随意加一个{}进入你的描述，看日志报错，报错时会顺带告诉你你可以获取到的数值对应的效果。依据这个效果回到代码查看，发现是DynamicVar里面自己的命名直接调用，不加diff()即可，不过这样似乎没有特效了，我以后看看是什么问题  
以我设计的空谷回响([空谷回响](Scripts/Cards/ValeEcho​.cs))为例子
```c# 
   protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("EnemyVulnerable", 5m),    // 敌人的易伤
        new DynamicVar("EnemyWeak", 5m),         // 敌人的虚弱
        new DynamicVar("EnemyStrength", -1m)      // 敌人力量变化
    ];
```
对应如下(负数还未处理，后续检查)
```json
    [{
        "SUMMITBELL-VALE_ECHO.title": "空谷回响",
        "SUMMITBELL-VALE_ECHO.description": "对自身造成1点虚弱，1点易伤。对所有敌人造成{EnemyWeak}点虚弱，造成{EnemyVulnerable}点易伤，并失去1点力量"
    }]
```