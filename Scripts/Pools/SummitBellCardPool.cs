using BaseLib.Abstracts;
using Godot;

namespace Summitbell.Scripts.Pools;


public class SummitBellCardPool : CustomCardPoolModel
{
    // 卡池的ID。必须唯一防撞车。
    public override string Title => "summitbell";

    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://summitbell/images/energy/energy_test.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://summitbell/images/energy/energy_test_big.png";

    // 卡池的主题色。
    public override Color DeckEntryCardColor => new(0.5f, 0.5f, 1f);

    // 如果你使用默认的卡框，可以使用这个颜色来修改卡框的颜色。
    public override Color ShaderColor => new(0.5f, 0.5f, 1f);


    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;
}