using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(PermissionsService))]
  public sealed class PermissionsService
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    /// <summary>
    /// Gets if a player has the specified permission.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="permission">The permission to query.</param>
    /// <returns>True if the player has the specified permission, otherwise false.</returns>
    public bool HasPermission(NwPlayer player, string permission)
    {
      PermissionSet permissionSet = ConfigService.GetPermissionsForPlayer(player);
      if (permissionSet.Permissions.Contains(permission))
      {
        return true;
      }

      foreach (string wildcardPermission in permissionSet.WildcardPermissions)
      {
        if (permission.StartsWith(wildcardPermission))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Gets a list of groups that the specified player is a member of.
    /// </summary>
    /// <param name="player">The player to query.</param>
    /// <param name="includeDefault">If true, includes groups that the player is a part of by default.</param>
    /// <returns></returns>
    public IEnumerable<string> GetGroups(NwPlayer player, bool includeDefault = true)
    {
      return ConfigService.GetGroupsForPlayer(player, includeDefault);
    }
  }
}
