using System.Collections.Generic;
using Anvil.API;

namespace Jorteck.Toolbox.src.Windows.ItemTools
{
  public sealed class ItemPropertiesGeneralWindowView : WindowView<ItemPropertiesGeneralWindowView>
  {
    public override string Id => "item.general";
    public override string Title => "Item Properties: General";
    public override NuiWindow WindowTemplate { get; }

    public override IWindowController CreateDefaultController(NwPlayer player)
    {
      return CreateController<ItemPropertiesGeneralWindowController>(player);
    }

    // Sub-views
    public readonly NuiGroup TopContainer = new NuiGroup
    {
      Id = "top_container",
      Scrollbars = NuiScrollbars.None,
      Height = 490f,
      Width = 550f,
    };

    public readonly NuiGroup BottomContainer = new NuiGroup
    {
      Id = "bottom_container",
      Scrollbars = NuiScrollbars.None,
      Height = 160f,
      Width = 550f,
    };

    // Permission Binds

    public readonly NuiBind<bool> ChargesEnabled = new NuiBind<bool>("charges");
    public readonly NuiBind<bool> AddCostEnabled = new NuiBind<bool>("add_cost");
    public readonly NuiBind<bool> StackSizeEnabled = new NuiBind<bool>("stack_size");
    public readonly NuiBind<bool> StolenEnabled = new NuiBind<bool>("stolen");
    public readonly NuiBind<bool> PlotEnabled = new NuiBind<bool>("plot");
    public readonly NuiBind<bool> TagEnabled = new NuiBind<bool>("tag");
    public readonly NuiBind<bool> NameEnabled = new NuiBind<bool>("name");
    public readonly NuiBind<bool> BaseTypeNameEnabled = new NuiBind<bool>("base_type_name");

    public readonly NuiBind<bool> SaveEnabled = new NuiBind<bool>("save");

    // Value Binds

    public readonly NuiBind<string> TotalCost = new NuiBind<string>("total_cost_val");
    public readonly NuiBind<string> Weight = new NuiBind<string>("weight_val");
    public readonly NuiBind<string> AC = new NuiBind<string>("ac_val");
    public readonly NuiBind<string> ACType = new NuiBind<string>("ac_type_val");
    public readonly NuiBind<string> ArcaneSpellFailure = new NuiBind<string>("arcane_spell_failure_val");
    public readonly NuiBind<string> ArmorCheckPenalty = new NuiBind<string>("armor_check_penalty_val");
    public readonly NuiBind<string> MaxDexBonus = new NuiBind<string>("max_dex_bonus_val");
    public readonly NuiBind<string> Charges = new NuiBind<string>("charges_val");
    public readonly NuiBind<string> AdditionalCost = new NuiBind<string>("additionnal_cost_val");
    public readonly NuiBind<string> Damage = new NuiBind<string>("damage_val");
    public readonly NuiBind<string> Critical = new NuiBind<string>("critical_val");
    public readonly NuiBind<string> DamageType = new NuiBind<string>("damage_type_val");
    public readonly NuiBind<string> StackSize = new NuiBind<string>("stack_size_val");
    public readonly NuiBind<string> RequiredLore = new NuiBind<string>("required_lore_val");
    public readonly NuiBind<string> RequiredLevel = new NuiBind<string>("required_level_val");
    public readonly NuiBind<string> Category = new NuiBind<string>("category_val");
    public readonly NuiBind<bool> Plot = new NuiBind<bool>("plot_val");
    public readonly NuiBind<bool> Stolen = new NuiBind<bool>("stolen_val");
    public readonly NuiBind<string> Name = new NuiBind<string>("name_val");
    public readonly NuiBind<string> Tag = new NuiBind<string>("tag_val");
    public readonly NuiBind<int> BaseTypeName = new NuiBind<int>("base_type_name_val");
    public readonly NuiBind<string> ResRef = new NuiBind<string>("res_ref_val");

    // Buttons
    public readonly NuiButton SelectItemButton;
    public readonly NuiButton SaveChangesButton;
    public readonly NuiButton DiscardChangesButton;

    public ItemPropertiesGeneralWindowView()
    {
      SelectItemButton = new NuiButton("Select Item")
      {
        Id = "btn_crt_sel",
      };

      SaveChangesButton = new NuiButton("Save")
      {
        Id = "btn_save",
        Enabled = SaveEnabled,
      };

      DiscardChangesButton = new NuiButton("Discard")
      {
        Id = "btn_discard",
        Enabled = SaveEnabled,
      };

      NuiColumn root = new NuiColumn
      {
        Children = new List<NuiElement>
        {
          TopContainer,
          BottomContainer,
          new NuiRow
          {
            Height = 40f,
            Children = new List<NuiElement>
            {
              SelectItemButton,
              new NuiSpacer(),
              SaveChangesButton,
              DiscardChangesButton,
            },
          },
        }
      };

      WindowTemplate = new NuiWindow(root, Title)
      {
        Geometry = new NuiRect(500f, 100f, 600f, 720f),
      };
    }
  }
}
