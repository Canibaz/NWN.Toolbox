using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Toolbox.src.Windows.ItemTools
{
  public sealed class ItemPropertiesGeneralWindowController : WindowController<ItemPropertiesGeneralWindowView>
  {
    private NuiBind<bool>[] widgetEnabledBinds;
    private NwItem selectedItem;

    public override void Init()
    {
      widgetEnabledBinds = new[]
      {
        View.ChargesEnabled,
        View.AddCostEnabled,
        View.StackSizeEnabled,
        View.StolenEnabled,
        View.PlotEnabled,
        View.TagEnabled,
        View.NameEnabled,
        View.BaseTypeNameEnabled,
        View.SaveEnabled,
      };

      Update();
    }

    public override void ProcessEvent(ModuleEvents.OnNuiEvent eventData)
    {
      switch (eventData.EventType)
      {
        case NuiEventType.Click:
          HandleButtonClick(eventData);
          break;
        case NuiEventType.Open:
          Update();
          break;
      }
    }

    protected override void OnClose()
    {
      selectedItem = null;
    }

    private void Update()
    {
      if (selectedItem == null)
      {
        SetElementsEnabled(false);

        RefreshTopContainer();
        RefreshBottomContainer();
        return;
      }

      ApplyPermissionBindings(widgetEnabledBinds);

      SetBindValue(View.TotalCost, selectedItem.GoldValue.ToString());
      SetBindValue(View.AdditionalCost, selectedItem.AddGoldValue.ToString());
      SetBindValue(View.Weight, selectedItem.Weight.ToString());
      SetBindValue(View.AC, selectedItem.ACValue.ToString());
      //SetBindValue(View.ACType, );
      //SetBindValue(View.ArcaneSpellFailure, );
      //SetBindValue(View.ArmorCheckPenalty, );
      //SetBindValue(View.MaxDexBonus, );
      SetBindValue(View.Charges, selectedItem.ItemCharges.ToString());
      //SetBindValue(View.Damage, );
      //SetBindValue(View.Critical, );
      //SetBindValue(View.DamageType, );
      SetBindValue(View.StackSize, selectedItem.StackSize.ToString());
      //SetBindValue(View.RequiredLore, );
      SetBindValue(View.RequiredLevel, selectedItem.MinEquipLevel.ToString());
      //SetBindValue(View.Category, );
      SetBindValue(View.Plot, selectedItem.PlotFlag);
      SetBindValue(View.Stolen, selectedItem.Stolen);
      SetBindValue(View.Name, selectedItem.Name);
      SetBindValue(View.Tag, selectedItem.Tag);
      SetBindValue(View.BaseTypeName, (int)selectedItem.BaseItemType);
      SetBindValue(View.ResRef, selectedItem.ResRef);

      // Only enable the stack if the item is stackable otherwise it crashes the items
      SetBindValue(View.StackSizeEnabled, selectedItem.CanStack);

      RefreshTopContainer();
      RefreshBottomContainer();
    }

    private void HandleButtonClick(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventData.ElementId == View.SelectItemButton.Id)
      {
        Player.TryEnterTargetMode(OnItemSelected, ObjectTypes.Item);
      }
      else if (eventData.ElementId == View.SaveChangesButton.Id)
      {
        SaveChanges();
      }
      else if (eventData.ElementId == View.DiscardChangesButton.Id)
      {
        Update();
      }
    }

    private void SaveChanges()
    {
      if (selectedItem == null || !selectedItem.IsValid)
      {
        return;
      }

      if (int.TryParse(GetBindValue(View.AdditionalCost), out int addCost))
        selectedItem.AddGoldValue = addCost;

      // NOTE: Seems to break the item (maybe because the item cannot support the charges?)
      //if (int.TryParse(GetBindValue(View.Charges), out int charges))
      //  selectedItem.ItemCharges = charges;

      if (selectedItem.CanStack && int.TryParse(GetBindValue(View.StackSize), out int stackSize))
        selectedItem.StackSize = stackSize;

      // NOTE: Even if the change is working, for it to apply really you need to put it in your inventory for it to apply.
      // WARNING: You shouldn't try to make changes to an item in your inventory!
      selectedItem.BaseItemType = (BaseItemType)GetBindValue(View.BaseTypeName);
      selectedItem.PlotFlag = GetBindValue(View.Plot);
      selectedItem.Stolen = GetBindValue(View.Stolen);
      selectedItem.Name = GetBindValue(View.Name);
      selectedItem.Tag = GetBindValue(View.Tag);

      Update();
    }

    private void OnItemSelected(ModuleEvents.OnPlayerTarget eventData)
    {
      if (eventData.TargetObject == null || eventData.TargetObject is not NwItem item)
      {
        return;
      }

      selectedItem = item;

      Update();
    }

    private void SetElementsEnabled(bool enabled)
    {
      foreach (NuiBind<bool> bind in widgetEnabledBinds)
      {
        SetBindValue(bind, enabled);
      }
    }

    private void RefreshTopContainer()
    {
      NuiColumn col = new NuiColumn();

      col.Children.Add(new NuiRow
      {
        Children = new List<NuiElement>
        {
          new NuiColumn
          {
            Children = new List<NuiElement>
            {
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Total Cost") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.TotalCost, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Base Weight") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.Weight, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Armor Class") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.AC, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Armor Type") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.ACType, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Arcane Spell Failure") { Width = 200f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.ArcaneSpellFailure, ushort.MaxValue, false) { Width = 50f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Armor Check Penalty") { Width = 200f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.ArmorCheckPenalty, ushort.MaxValue, false) { Width = 50f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Max Dex Bonus") { Width = 200f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.MaxDexBonus, ushort.MaxValue, false) { Width = 50f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Charges") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.Charges, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false /*View.ChargesEnabled*/ },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Additional Cost") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.AdditionalCost, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = View.AddCostEnabled },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Stolen") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiCheck(string.Empty, View.Stolen) { Width = 100f, Height = 40f, Enabled = View.StolenEnabled },
                },
              },
            },
          },
          new NuiColumn
          {
            Children = new List<NuiElement>
            {
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Damage") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.Damage, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Critical") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.Critical, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Damage Type") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.DamageType, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("") { Width = 250f, Height = 40f },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Stack Size") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.StackSize, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = View.StackSizeEnabled },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Required Lore") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.RequiredLore, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Required Level") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.RequiredLevel, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("") { Width = 250f, Height = 40f },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Category") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiTextEdit(string.Empty, View.Category, ushort.MaxValue, false) { Width = 100f, Height = 40f, Enabled = false },
                },
              },
              new NuiRow
              {
                Children = new List<NuiElement>
                {
                  new NuiLabel("Plot") { Width = 150f, Height = 40f, VerticalAlign = NuiVAlign.Middle },
                  new NuiCheck(string.Empty, View.Plot) { Width = 100f, Height = 40f, Enabled = View.PlotEnabled },
                },
              },
            },
          },
        },
      });

      SetGroupLayout(View.TopContainer, col);
    }

    private void RefreshBottomContainer()
    {
      NuiColumn col = new NuiColumn();

      NuiCombo baseTypeNameCombo = NuiUtils.CreateComboForEnum<BaseItemType>(View.BaseTypeName);
      baseTypeNameCombo.Width = 260f;
      baseTypeNameCombo.Height = 40f;
      baseTypeNameCombo.Enabled = View.BaseTypeNameEnabled;

      col.Children.Add(new NuiRow
      {
        Children = new List<NuiElement>
        {
          new NuiColumn
          {
            Children = new List<NuiElement>
            {
              new NuiLabel("Item Name") { Width = 260f, Height = 20f },
              new NuiTextEdit(string.Empty, View.Name, ushort.MaxValue, false) { Width = 260f, Height = 40f, Enabled = View.NameEnabled },
              new NuiLabel("Base Type Name") { Width = 260f, Height = 20f },
              baseTypeNameCombo,
            },
          },
          new NuiColumn
          {
            Children = new List<NuiElement>
            {
              new NuiLabel("Tag") { Width = 260f, Height = 20f },
              new NuiTextEdit(string.Empty, View.Tag, ushort.MaxValue, false) { Width = 260f, Height = 40f, Enabled = View.TagEnabled },
              new NuiLabel("Blueprint ResRef") { Width = 260f, Height = 20f },
              new NuiTextEdit(string.Empty, View.ResRef, ushort.MaxValue, false) { Width = 260f, Height = 40f, Enabled = false },
            },
          },
        }
      });

      SetGroupLayout(View.BottomContainer, col);
    }
  }
}
