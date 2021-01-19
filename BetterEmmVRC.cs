using BetterEmmVRC.Helpers_And_Extensions;
using emmVRC.Hacks;
using emmVRC.Network;
using MelonLoader;
using VRC.Core;

//Metadata For MelonLoader To Read
[assembly: MelonInfo(typeof(BetterEmmVRC.BetterEmmVRC), "BetterEmmVRC", "", "𝒫𝓁𝒶𝑔𝓊𝑒")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace BetterEmmVRC
{
    internal class BetterEmmVRC : MelonMod
    {
        internal static EmmVRCNLib.EmmVRCNetworkLib NetworkLib = new EmmVRCNLib.EmmVRCNetworkLib();

        /// <summary>
        /// Occurs On Mod Load
        /// </summary>
        public override void OnApplicationStart()
        {

        }

        public override void OnApplicationQuit()
        {
            if (NetworkLib.authToken != null)
            {
                HTTPRequest.get(NetworkLib.baseURL + "/api/authentication/logout");
            }
        }

        private bool LoggedIn = false;

        /// <summary>
        /// Occurs Every Frame Rendered
        /// </summary>
        public override void OnUpdate()
        {
            if (RoomManager.prop_Boolean_3)
            {
                if (APIUser.CurrentUser != null && VRCPlayer.field_Internal_Static_VRCPlayer_0 != null && VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.prop_EnumNPublicSealedva9vUnique_0 == VRCAvatarManager.EnumNPublicSealedva9vUnique.EnumValue7/*Custom Avatar Loaded*/ && !LoggedIn)
                {
                    Helpers.Log("Logging In To emmVRCNetwork..");

                    if (NetworkLib.Login())
                    {
                        CustomAvatarFavorites.Initialize();
                        CustomAvatarFavorites.Show();
                        MelonCoroutines.Start(CustomAvatarFavorites.PopulateList());

                        Helpers.Log("Logged In!");
                    }

                    LoggedIn = true;
                }
            }

            if (LoggedIn)
            {
                CustomAvatarFavorites.OnUpdate();
            }
        }

        /// <summary>
        /// Occurs On VRChat Loading The VRCUIManager
        /// </summary>
        public override void VRChat_OnUiManagerInit()
        {

        }
    }
}
