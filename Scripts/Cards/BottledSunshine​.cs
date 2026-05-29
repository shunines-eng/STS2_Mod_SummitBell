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
public class BottledSunshine​ : CustomCardModel, IGrowableCard
{
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Common;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.None;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 1点能量
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SummitBellKeyWords.Grow];
// 添加这一行，指定卡牌立绘路径
    public override string PortraitPath => $"res://summitbell/images/cards/{nameof(TestCard)}.png";

    public BottledSunshine​() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner); //回复能量
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
       AddKeyword(CardKeyword.Innate);
    }

    public async Task OnCultivateTriggered(PlayerChoiceContext ctx)
    {
        // 生长效果：增加1点能量
        DynamicVars.Energy.BaseValue += 1;
    }
}