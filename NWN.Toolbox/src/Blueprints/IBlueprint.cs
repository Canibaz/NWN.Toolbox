using Anvil.API;

namespace Jorteck.Toolbox
{
  public interface IBlueprint
  {
    public string FullName { get; }

    public string Name { get; }

    public string Category { get; }

    public BlueprintObjectType ObjectType { get; }

    public NwObject Create(Location location);
  }
}
