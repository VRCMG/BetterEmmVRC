using System;
using System.Collections;
using System.Collections.Generic;
using emmVRC.Hacks;
using Il2CppSystem.IO;
using MelonLoader;
using UnityEngine;
using VRC.Core;

namespace emmVRC.Libraries
{
    public class AvatarUtilities
    {
        public static IEnumerator FavoriteAvatars(List<ApiAvatar> avatars, bool errored)
        {
            foreach (ApiAvatar apiAvatar in avatars)
            {
                if (apiAvatar.releaseStatus == "public" || apiAvatar.authorId == APIUser.CurrentUser.id)
                {
                    yield return CustomAvatarFavorites.FavoriteAvatar(apiAvatar);
                }
                else
                {
                    errored = true;
                }
                yield return new WaitForSeconds(1f);
            }
            List<ApiAvatar>.Enumerator enumerator = default(List<ApiAvatar>.Enumerator);
            CustomAvatarFavorites.MigrateButton.SetActive(false);
            File.Move(Path.Combine(Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.json"), Path.Combine(Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.old.json"));
            yield break;
            yield break;
        }

        public static IEnumerator fetchAvatars(List<string> avatars, Action<List<ApiAvatar>, bool> callBack)
        {
            AvatarUtilities.processingList = new List<ApiAvatar>();
            AvatarUtilities.requestFinished = true;
            AvatarUtilities.errored = false;
            foreach (string avatarId in avatars)
            {
                while (!AvatarUtilities.requestFinished)
                {
                    yield return new WaitForEndOfFrame();
                }
                AvatarUtilities.requestFinished = false;
                API.Fetch<ApiAvatar>(avatarId, (Action<ApiContainer>)delegate (ApiContainer container)
                {
                    AvatarUtilities.processingList.Add(container.Model.Cast<ApiAvatar>());
                    MelonCoroutines.Start(AvatarUtilities.Delay());
                }, (Action<ApiContainer>)delegate (ApiContainer container)
                {
                    AvatarUtilities.errored = true;
                    MelonCoroutines.Start(AvatarUtilities.Delay());
                }, false);
            }
            List<string>.Enumerator enumerator = default(List<string>.Enumerator);
            while (!AvatarUtilities.requestFinished)
            {
                yield return new WaitForEndOfFrame();
            }
            callBack(AvatarUtilities.processingList, AvatarUtilities.errored);
            yield break;
            yield break;
        }

        public static IEnumerator Delay()
        {
            yield return new WaitForSeconds(2.5f);
            AvatarUtilities.requestFinished = true;
            yield break;
        }

        public static List<ApiAvatar> processingList;

        public static bool requestFinished = true;

        public static bool errored = false;
    }
}
