using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserListPermsCommand : IChatCommand
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Command => ConfigService.GetFullChatCommand("user listpermissions");
    public string[] Aliases => null;

    public Dictionary<string, object> UserData { get; } = new Dictionary<string, object>
    {
      [PermissionConstants.ChatUserDataKey] = PermissionConstants.List,
    };

    public int? ArgCount => 0;
    public string Description => "Lists all user permissions.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("List all permissions for the target user."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      caller.EnterPlayerTargetMode(ListPermissionsOfTarget);
    }

    private void ListPermissionsOfTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection)
    {
      NwPlayer caller = selection.Caller;
      NwPlayer target = selection.Target;

      PermissionSet userPermissions = ConfigService.GetPermissionsForPlayer(target);
      caller.SendServerMessage($"Target has {(userPermissions.Permissions.Count == 0 ? "no permissions." : "the following permissions:")}", ColorConstants.Orange);
      caller.SendServerMessage(string.Join("\n", userPermissions.Permissions), ColorConstants.Lime);
    }
  }
}
