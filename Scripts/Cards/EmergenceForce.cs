// 卡名:{破土伟力}
// 效果:{造成4点伤害。生长:此牌增加2点伤害}
// 类型:{攻击牌}
// 牌效偏向:{过渡终端}
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using Summitbell.Scripts.KeyWords;
using Summitbell.Scripts.Pools;

namespace Summitbell.Scripts.Cards;

// 注册卡牌。如果要写自定义池看添加人物的开头
[Pool(typeof(SummitBellCardPool))]
public class EmergenceForce : CustomCardModel, IGrowableCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];
    
    // 生长关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SummitBellKeyWords.Grow];

    public override string PortraitPath => $"res://summitbell/images/cards/{nameof(EmergenceForce)}.png";

    public EmergenceForce() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }

    // 实现IGrowableCard接口 - 当被培育触发时
    public async Task OnCultivateTriggered(PlayerChoiceContext ctx, CardPlay originalCardPlay)
    {
        // 生长效果：增加2点伤害
        DynamicVars.Damage.BaseValue += 2;
        
        // 可以在这里添加卡牌特定的视觉效果
        // 例如：卡牌闪一下光
    }
}