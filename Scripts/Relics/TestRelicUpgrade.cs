using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using Summitbell.Scripts.Pools;

namespace Summitbell.Scripts.Relics;

// 注册遗物。如果要写自定义池看添加人物的开头
[Pool(typeof(SummitBellRelicPool))]
public class TestRelicUpgrade : CustomRelicModel
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Ancient;

    // 遗物的数值。替换本地化中的{Cards}。
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    // 小图标（原版85x85）
    public override string PackedIconPath => $"res://summitbell/images/relics/{nameof(TestRelic)}.png";
    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => $"res://summitbell/images/relics/{nameof(TestRelic)}.png";
    // 大图标（原版256x256）
    protected override string BigIconPath => $"res://summitbell/images/relics/{nameof(TestRelic)}.png";

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        // 这里的DynamicVars.Cards.IntValue为上面设置的CardsVar的数值。
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, player);
    }
    // 初始遗物的升级可以写这里
    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<TestRelicUpgrade>(); // 实现方法。自己更改类型
    
}