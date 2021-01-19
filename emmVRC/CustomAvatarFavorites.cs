using emmVRC.Libraries;
using emmVRC.Network.Objects;
using MelonLoader;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BetterEmmVRC.Helpers_And_Extensions;
using TinyJSON;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Core;
using VRC.UI;
using HTTPRequest = emmVRC.Network.HTTPRequest;

namespace emmVRC.Hacks
{
    public static class CustomAvatarFavorites
    {
        internal static void RenderElement(this UiVRCList uivrclist, Il2CppSystem.Collections.Generic.List<ApiAvatar> AvatarList)
        {
            if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled)
            {
                return;
            }
            if (CustomAvatarFavorites.renderElementMethod == null)
            {
                CustomAvatarFavorites.renderElementMethod = typeof(UiVRCList).GetMethods().FirstOrDefault((MethodInfo a) => a.Name.Contains("Method_Protected_Void_List_1_T_Int32_Boolean")).MakeGenericMethod(new Type[]
                {
                    typeof(ApiAvatar)
                });
            }
            MethodBase methodBase = CustomAvatarFavorites.renderElementMethod;
            object[] array = new object[4];
            array[0] = AvatarList;
            array[1] = 0;
            array[2] = true;
            methodBase.Invoke(uivrclist, array);
        }

        internal static void Initialize()
        {
            CustomAvatarFavorites.pageAvatar = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").gameObject;
            CustomAvatarFavorites.FavoriteButton = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Favorite Button").gameObject;
            CustomAvatarFavorites.FavoriteButtonNew = UnityEngine.Object.Instantiate<GameObject>(CustomAvatarFavorites.FavoriteButton, QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/"));
            CustomAvatarFavorites.FavoriteButtonNewButton = CustomAvatarFavorites.FavoriteButtonNew.GetComponent<Button>();
            CustomAvatarFavorites.FavoriteButtonNewButton.onClick.RemoveAllListeners();
            CustomAvatarFavorites.FavoriteButtonNewButton.onClick.AddListener((Action)delegate ()
            {
                ApiAvatar apiAvatar = CustomAvatarFavorites.pageAvatar.GetComponent<PageAvatar>().avatar.field_Internal_ApiAvatar_0;
                bool flag = false;
                for (int i = 0; i < CustomAvatarFavorites.LoadedAvatars.Count; i++)
                {
                    if (CustomAvatarFavorites.LoadedAvatars[i].id == apiAvatar.id)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name + "\"?", "Yes", delegate ()
                    {
                        MelonCoroutines.Start(CustomAvatarFavorites.UnfavoriteAvatar(apiAvatar));
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, "No", delegate ()
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, null);
                    return;
                }
                if ((!(apiAvatar.releaseStatus == "public") && !(apiAvatar.authorId == APIUser.CurrentUser.id)) || apiAvatar.releaseStatus == null)
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot favorite this avatar (it is private!)", "Dismiss", delegate ()
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, null);
                    return;
                }
                if (CustomAvatarFavorites.LoadedAvatars.Count < 500)
                {
                    MelonCoroutines.Start(CustomAvatarFavorites.FavoriteAvatar(apiAvatar));
                    return;
                }
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You have reached the maximum BetterEmmVRC favorites size.", "Dismiss", delegate ()
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }, null);
            });
            CustomAvatarFavorites.FavoriteButtonNew.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0f, 165f);
            CustomAvatarFavorites.FavoriteButtonNewText = CustomAvatarFavorites.FavoriteButtonNew.GetComponentInChildren<Text>();
            CustomAvatarFavorites.FavoriteButtonNewText.supportRichText = true;
            try
            {
                CustomAvatarFavorites.FavoriteButtonNew.transform.Find("Horizontal/FavoritesCountSpacingText").gameObject.SetActive(false);
                CustomAvatarFavorites.FavoriteButtonNew.transform.Find("Horizontal/FavoritesCurrentCountText").gameObject.SetActive(false);
                CustomAvatarFavorites.FavoriteButtonNew.transform.Find("Horizontal/FavoritesCountDividerText").gameObject.SetActive(false);
                CustomAvatarFavorites.FavoriteButtonNew.transform.Find("Horizontal/FavoritesMaxAvailableText").gameObject.SetActive(false);
            }
            catch (Exception)
            {

            }
            CustomAvatarFavorites.MigrateButton = UnityEngine.Object.Instantiate<GameObject>(CustomAvatarFavorites.FavoriteButton, QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/"));
            CustomAvatarFavorites.MigrateButton.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0f, 765f);
            CustomAvatarFavorites.MigrateButton.GetComponentInChildren<Text>().text = "Migrate";
            CustomAvatarFavorites.MigrateButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            CustomAvatarFavorites.MigrateButton.GetComponentInChildren<Button>().onClick.AddListener((Action)delegate ()
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Do you want to migrate your AviFav+ avatars to emmVRC?", "Yes", delegate ()
                {
                    System.Collections.Generic.List<AviFavAvatar> list = Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.json"))).Make<System.Collections.Generic.List<AviFavAvatar>>();

                    System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();

                    foreach (AviFavAvatar aviFavAvatar in list)
                    {
                        list2.Add(aviFavAvatar.AvatarID);
                    }
                    if (list2.Count > 0)
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Your avatars are being migrated in the background. This may take a few minutes. Please do not close VRChat.", "Dismiss", delegate ()
                        {
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                        }, null);
                        CustomAvatarFavorites.MigrateButton.GetComponentInChildren<Button>().enabled = false;
                        CustomAvatarFavorites.MigrateButton.GetComponentInChildren<Text>().text = "Migrating...";
                        MelonCoroutines.Start(AvatarUtilities.fetchAvatars(list2, delegate (System.Collections.Generic.List<ApiAvatar> avatars, bool errored)
                        {
                            MelonCoroutines.Start(AvatarUtilities.FavoriteAvatars(avatars, errored));
                        }));
                    }
                }, "No", delegate ()
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }, null);
            });
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.json")))
            {
                CustomAvatarFavorites.MigrateButton.SetActive(true);
                CustomAvatarFavorites.MigrateButton.GetComponentInChildren<Button>().enabled = true;
            }
            else
            {
                CustomAvatarFavorites.MigrateButton.SetActive(false);
            }
            GameObject gameObject = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            CustomAvatarFavorites.PublicAvatarList = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
            CustomAvatarFavorites.PublicAvatarList.transform.SetAsFirstSibling();
            CustomAvatarFavorites.ChangeButton = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Change Button").gameObject;
            CustomAvatarFavorites.baseChooseEvent = CustomAvatarFavorites.ChangeButton.GetComponent<Button>().onClick;
            CustomAvatarFavorites.ChangeButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            CustomAvatarFavorites.ChangeButton.GetComponent<Button>().onClick.AddListener((Action)delegate ()
            {
                ApiAvatar selectedAvatar = CustomAvatarFavorites.pageAvatar.GetComponent<PageAvatar>().avatar.field_Internal_ApiAvatar_0;
                if (!selectedAvatar.id.Contains("local"))
                {
                    API.Fetch<ApiAvatar>(selectedAvatar.id, (Action<ApiContainer>)delegate (ApiContainer cont)
                    {
                        ApiAvatar apiAvatar = cont.Model.Cast<ApiAvatar>();

                        if (apiAvatar.releaseStatus == "private" && apiAvatar.authorId != APIUser.CurrentUser.id && apiAvatar.authorName != "tafi_licensed")
                        {
                            VRCUiPopupManager field_Private_Static_VRCUiPopupManager_ = VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0;
                            string title = "emmVRC";
                            string content = "Cannot switch into this avatar (it is private).\nDo you want to unfavorite it?";
                            string button1Text = "Yes";
                            Action button1Action;

                            button1Action = delegate ()
                            {
                                MelonCoroutines.Start(CustomAvatarFavorites.UnfavoriteAvatar(selectedAvatar));
                                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                            };

                            field_Private_Static_VRCUiPopupManager_.ShowStandardPopup(title, content, button1Text, button1Action, "No", delegate ()
                                    {
                                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                                    }, null);
                            return;
                        }
                        CustomAvatarFavorites.baseChooseEvent.Invoke();
                    }, (Action<ApiContainer>)delegate (ApiContainer cont)
                    {
                        VRCUiPopupManager field_Private_Static_VRCUiPopupManager_ = VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0;
                        string title = "emmVRC";
                        string content = "Cannot switch into this avatar (no longer available).\nDo you want to unfavorite it?";
                        string button1Text = "Yes";
                        Action button1Action;
                        button1Action = delegate ()
                        {
                            MelonCoroutines.Start(CustomAvatarFavorites.UnfavoriteAvatar(selectedAvatar));
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                        };
                        field_Private_Static_VRCUiPopupManager_.ShowStandardPopup(title, content, button1Text, button1Action, "No", delegate ()
                        {
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                        }, null);
                    }, false);
                    return;
                }
                if (selectedAvatar.releaseStatus == "private" && selectedAvatar.authorId != APIUser.CurrentUser.id && selectedAvatar.authorName != "tafi_licensed")
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (it is private).\nDo you want to unfavorite it?", "Yes", delegate ()
                    {
                        MelonCoroutines.Start(CustomAvatarFavorites.UnfavoriteAvatar(selectedAvatar));
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, "No", delegate ()
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, null);
                    return;
                }
                if (selectedAvatar.releaseStatus == "unavailable")
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (no longer available).\nDo you want to unfavorite it?", "Yes", delegate ()
                    {
                        MelonCoroutines.Start(CustomAvatarFavorites.UnfavoriteAvatar(selectedAvatar));
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, "No", delegate ()
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, null);
                    return;
                }
                CustomAvatarFavorites.baseChooseEvent.Invoke();
            });
            CustomAvatarFavorites.avText = CustomAvatarFavorites.PublicAvatarList.transform.Find("Button").gameObject;
            CustomAvatarFavorites.avTextText = CustomAvatarFavorites.avText.GetComponentInChildren<Text>();
            CustomAvatarFavorites.avTextText.text = "(0) BetterEmmVRC Favorites";
            CustomAvatarFavorites.currPageAvatar = CustomAvatarFavorites.pageAvatar.GetComponent<PageAvatar>();
            CustomAvatarFavorites.NewAvatarList = CustomAvatarFavorites.PublicAvatarList.GetComponent<UiAvatarList>();
            CustomAvatarFavorites.NewAvatarList.clearUnseenListOnCollapse = false;
            CustomAvatarFavorites.NewAvatarList.category = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLi9vUnique.SpecificList;
            CustomAvatarFavorites.SearchAvatarList = CustomAvatarFavorites.PublicAvatarList.GetComponent<UiAvatarList>();
            CustomAvatarFavorites.SearchAvatarList.clearUnseenListOnCollapse = false;
            CustomAvatarFavorites.currPageAvatar.avatar.avatarScale *= 0.85f;
            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(CustomAvatarFavorites.ChangeButton, CustomAvatarFavorites.avText.transform.parent);
            gameObject2.GetComponentInChildren<Text>().text = "↻";
            gameObject2.GetComponent<Button>().onClick.RemoveAllListeners();
            gameObject2.GetComponent<Button>().onClick.AddListener((Action)delegate ()
            {
                CustomAvatarFavorites.Searching = false;

                MelonCoroutines.Start(CustomAvatarFavorites.RefreshMenu(0.5f));

                CustomAvatarFavorites.avText.GetComponentInChildren<Text>().text = "(" + CustomAvatarFavorites.LoadedAvatars.Count.ToString() + ") BetterEmmVRC Favorites";
            });
            gameObject2.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            gameObject2.transform.SetParent(CustomAvatarFavorites.avText.transform, true);
            gameObject2.GetComponent<RectTransform>().anchoredPosition = CustomAvatarFavorites.avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(975f, 0f);
            CustomAvatarFavorites.pageAvatar.transform.Find("AvatarModel").transform.localPosition += new Vector3(0f, 60f, 0f);
            CustomAvatarFavorites.LoadedAvatars = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
            CustomAvatarFavorites.SearchedAvatars = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
        }

        public static IEnumerator FavoriteAvatar(ApiAvatar avtr)
        {
            if (CustomAvatarFavorites.LoadedAvatars.ToArray().ToList<ApiAvatar>().FindIndex((ApiAvatar a) => a.id == avtr.id) == -1)
            {
                CustomAvatarFavorites.LoadedAvatars.Insert(0, avtr);
                Avatar obj = new Avatar(avtr);
                Task<string> request = HTTPRequest.post(BetterEmmVRC.BetterEmmVRC.NetworkLib.baseURL + "/api/avatar", obj);
                while (!request.IsCompleted && !request.IsFaulted)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (!request.IsFaulted)
                {
                    if (!CustomAvatarFavorites.Searching)
                    {
                        CustomAvatarFavorites.avText.GetComponentInChildren<Text>().text = "(" + CustomAvatarFavorites.LoadedAvatars.Count.ToString() + ") BetterEmmVRC Favorites";
                        MelonCoroutines.Start(CustomAvatarFavorites.RefreshMenu(0.1f));
                    }
                }
                else
                {
                    AggregateException exception = request.Exception;
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Error occured while updating avatar list.", "Dismiss", delegate ()
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }, null);
                }
                request = null;
            }
            yield break;
        }

        public static IEnumerator UnfavoriteAvatar(ApiAvatar avtr)
        {
            if (CustomAvatarFavorites.LoadedAvatars.Contains(avtr))
            {
                CustomAvatarFavorites.LoadedAvatars.Remove(avtr);
            }
            Task<string> request = HTTPRequest.delete(BetterEmmVRC.BetterEmmVRC.NetworkLib.baseURL + "/api/avatar", new Avatar(avtr));
            while (!request.IsCompleted && !request.IsFaulted)
            {
                yield return new WaitForEndOfFrame();
            }
            if (!request.IsFaulted)
            {
                if (!CustomAvatarFavorites.Searching)
                {
                    CustomAvatarFavorites.avText.GetComponentInChildren<Text>().text = "(" + CustomAvatarFavorites.LoadedAvatars.Count.ToString() + ") BetterEmmVRC Favorites";
                    MelonCoroutines.Start(CustomAvatarFavorites.RefreshMenu(0.1f));
                }
            }
            else
            {
                AggregateException exception = request.Exception;
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Error occured while updating avatar list.", "Dismiss", delegate ()
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }, null);
            }
            yield break;
        }

        public static void AddEmptyFavorite()
        {
            ApiAvatar item = new ApiAvatar
            {
                releaseStatus = "unavailable",
                name = "Avatar not available",
                id = "null",
                assetUrl = "",
                thumbnailImageUrl = "http://img.thetrueyoshifan.com/AvatarUnavailable.png"
            };
            CustomAvatarFavorites.LoadedAvatars.Insert(0, item);
            MelonCoroutines.Start(CustomAvatarFavorites.RefreshMenu(0.125f));
        }

        public static IEnumerator PopulateList()
        {
            CustomAvatarFavorites.LoadedAvatars = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
            Task<string> request = HTTPRequest.get(BetterEmmVRC.BetterEmmVRC.NetworkLib.baseURL + "/api/avatar");
            while (!request.IsCompleted && !request.IsFaulted)
            {
                yield return new WaitForEndOfFrame();
            }
            if (!request.IsFaulted)
            {
                Avatar[] array = Decoder.Decode(request.Result).Make<Avatar[]>();
                if (array != null)
                {
                    foreach (Avatar avatar in array)
                    {
                        CustomAvatarFavorites.LoadedAvatars.Add(avatar.apiAvatar());
                    }
                    CustomAvatarFavorites.avText.GetComponentInChildren<Text>().text = "(" + CustomAvatarFavorites.LoadedAvatars.Count.ToString() + ") BetterEmmVRC Favorites";
                }
            }
            else
            {
                CustomAvatarFavorites.error = true;
                CustomAvatarFavorites.errorWarned = true;
            }
            yield break;
        }

        public static IEnumerator RefreshMenu(float delay)
        {
            if (CustomAvatarFavorites.NewAvatarList.scrollRect != null)
            {
                CustomAvatarFavorites.NewAvatarList.scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
                yield return new WaitForSeconds(delay);
                CustomAvatarFavorites.NewAvatarList.RenderElement(CustomAvatarFavorites.LoadedAvatars);
                CustomAvatarFavorites.NewAvatarList.scrollRect.movementType = ScrollRect.MovementType.Elastic;
            }
            yield break;
        }

        public static IEnumerator SearchAvatarsAfterDelay(string query)
        {
            yield return new WaitForSecondsRealtime(1f);
            MelonCoroutines.Start(CustomAvatarFavorites.SearchAvatars(query));
            yield break;
        }

        public static IEnumerator SearchAvatars(string query)
        {
            Helpers.Log("Starting Search For: " + query + "..");
            if (BetterEmmVRC.BetterEmmVRC.NetworkLib.authToken == null)
            {
                yield return new WaitForEndOfFrame();
            }
            CustomAvatarFavorites.SearchedAvatars.Clear();
            string url = BetterEmmVRC.BetterEmmVRC.NetworkLib.baseURL + "/api/avatar/search";
            System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
            dictionary["query"] = query;
            Helpers.Log("Making Request..");
            Task<string> request = HTTPRequest.post(url, dictionary);
            while (!request.IsCompleted && !request.IsFaulted)
            {
                yield return new WaitForEndOfFrame();
            }
            if (!request.IsFaulted)
            {
                Helpers.Log("Success!");
                Avatar[] array = Decoder.Decode(request.Result).Make<Avatar[]>();
                if (array != null)
                {
                    foreach (Avatar avatar in array)
                    {
                        CustomAvatarFavorites.SearchedAvatars.Add(avatar.apiAvatar());
                    }
                }
                CustomAvatarFavorites.avText.GetComponentInChildren<Text>().text = "(" + CustomAvatarFavorites.SearchedAvatars.Count.ToString() + ") Result(s): " + query;
            }
            else
            {
                string str = "Asynchronous net post failed: ";
                AggregateException exception = request.Exception;
                MelonLogger.LogError(str + ((exception != null) ? exception.ToString() : null));
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Your search could not be processed.", "Dismiss", delegate ()
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }, null);
            }
            CustomAvatarFavorites.SearchAvatarList.scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            CustomAvatarFavorites.SearchAvatarList.RenderElement(CustomAvatarFavorites.SearchedAvatars);
            CustomAvatarFavorites.SearchAvatarList.scrollRect.movementType = ScrollRect.MovementType.Elastic;
            if (CustomAvatarFavorites.SearchAvatarList.expandButton.gameObject.transform.Find("ToggleIcon").GetComponentInChildren<Image>().sprite == CustomAvatarFavorites.SearchAvatarList.expandSprite)
            {
                CustomAvatarFavorites.SearchAvatarList.ToggleExtend();
            }
            CustomAvatarFavorites.Searching = true;
            yield break;
        }

        internal static void OnUpdate()
        {
            if (CustomAvatarFavorites.PublicAvatarList == null || CustomAvatarFavorites.FavoriteButtonNew == null || RoomManager.field_Internal_Static_ApiWorld_0 == null)
            {
                return;
            }
            if (CustomAvatarFavorites.searchBox == null && CustomAvatarFavorites.NewAvatarList.gameObject.activeInHierarchy)
            {
                VRCUiPageHeader componentInChildren = QuickMenuUtils.GetVRCUiMInstance().GetComponentInChildren<VRCUiPageHeader>(true);
                if (componentInChildren != null)
                {
                    CustomAvatarFavorites.searchBox = componentInChildren.searchBar;
                }
            }
            if (CustomAvatarFavorites.searchBoxAction == null)
            {
                CustomAvatarFavorites.searchBoxAction = DelegateSupport.ConvertDelegate<UnityAction<string>>(new Action<string>(delegate (string searchTerm)
                {
                    if (searchTerm == "" || searchTerm.Length < 2)
                    {
                        return;
                    }

                    MelonCoroutines.Start(CustomAvatarFavorites.SearchAvatars(searchTerm));
                }));
            }
            if (CustomAvatarFavorites.searchBox != null && CustomAvatarFavorites.searchBox.editButton != null && !CustomAvatarFavorites.searchBox.editButton.interactable && CustomAvatarFavorites.PublicAvatarList.activeInHierarchy && BetterEmmVRC.BetterEmmVRC.NetworkLib.authToken != null && RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                CustomAvatarFavorites.searchBox.editButton.interactable = true;
                CustomAvatarFavorites.searchBox.onDoneInputting = CustomAvatarFavorites.searchBoxAction;
            }
            if (CustomAvatarFavorites.PublicAvatarList.activeSelf && BetterEmmVRC.BetterEmmVRC.NetworkLib.authToken != null)
            {
                CustomAvatarFavorites.NewAvatarList.collapsedCount = 500;
                CustomAvatarFavorites.NewAvatarList.expandedCount = 500;
                if (!CustomAvatarFavorites.menuJustActivated)
                {
                    CustomAvatarFavorites.Searching = false;
                    CustomAvatarFavorites.avTextText.text = "(" + CustomAvatarFavorites.LoadedAvatars.Count.ToString() + ") BetterEmmVRC Favorites";
                    MelonCoroutines.Start(CustomAvatarFavorites.RefreshMenu(1f));
                    CustomAvatarFavorites.menuJustActivated = true;
                }
                if (CustomAvatarFavorites.menuJustActivated && (CustomAvatarFavorites.NewAvatarList.pickers.Count < CustomAvatarFavorites.LoadedAvatars.Count || CustomAvatarFavorites.NewAvatarList.isOffScreen))
                {
                    CustomAvatarFavorites.menuJustActivated = false;
                }
                if (CustomAvatarFavorites.currPageAvatar != null && CustomAvatarFavorites.currPageAvatar.avatar != null && CustomAvatarFavorites.currPageAvatar.avatar.field_Internal_ApiAvatar_0 != null && CustomAvatarFavorites.LoadedAvatars != null && CustomAvatarFavorites.FavoriteButtonNew != null)
                {
                    bool flag = false;
                    for (int i = 0; i < CustomAvatarFavorites.LoadedAvatars.Count; i++)
                    {
                        if (CustomAvatarFavorites.LoadedAvatars[i].id == CustomAvatarFavorites.currPageAvatar.avatar.field_Internal_ApiAvatar_0.id)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        CustomAvatarFavorites.FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Favorite";
                    }
                    else
                    {
                        CustomAvatarFavorites.FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Unfavorite";
                    }
                }
            }
            if ((BetterEmmVRC.BetterEmmVRC.NetworkLib.authToken == null) && (CustomAvatarFavorites.PublicAvatarList.activeSelf || CustomAvatarFavorites.FavoriteButtonNew.activeSelf))
            {
                CustomAvatarFavorites.PublicAvatarList.SetActive(false);
                CustomAvatarFavorites.FavoriteButtonNew.SetActive(false);
            }
            else if ((!CustomAvatarFavorites.PublicAvatarList.activeSelf || !CustomAvatarFavorites.FavoriteButtonNew.activeSelf) && BetterEmmVRC.BetterEmmVRC.NetworkLib.authToken != null)
            {
                CustomAvatarFavorites.PublicAvatarList.SetActive(true);
                CustomAvatarFavorites.FavoriteButtonNew.SetActive(true);
            }
            if (CustomAvatarFavorites.error && !CustomAvatarFavorites.errorWarned)
            {
                CustomAvatarFavorites.errorWarned = true;
            }
        }

        private static IEnumerator SetAvatarListAfterDelay(UiAvatarList avatars, Il2CppSystem.Collections.Generic.List<ApiAvatar> models)
        {
            if (models.Count == 0)
            {
                yield break;
            }
            Il2CppSystem.Collections.Generic.List<ApiAvatar> list = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
            list.Add(models[0]);
            avatars.RenderElement(list);
            yield return new WaitForSeconds(1f);
            avatars.RenderElement(models);
            yield break;
        }

        internal static void Hide()
        {
            CustomAvatarFavorites.PublicAvatarList.SetActive(false);
            CustomAvatarFavorites.FavoriteButtonNew.SetActive(false);
        }

        internal static void Show()
        {
            if (!CustomAvatarFavorites.error)
            {
                CustomAvatarFavorites.PublicAvatarList.SetActive(true);
                CustomAvatarFavorites.FavoriteButtonNew.SetActive(true);
            }
        }

        internal static void Destroy()
        {
            UnityEngine.Object.Destroy(CustomAvatarFavorites.PublicAvatarList);
            UnityEngine.Object.Destroy(CustomAvatarFavorites.FavoriteButtonNew);
        }

        internal static GameObject PublicAvatarList;

        internal static UiAvatarList NewAvatarList;

        internal static UiAvatarList SearchAvatarList;

        private static GameObject avText;

        private static Text avTextText;

        private static GameObject ChangeButton;

        public static Button.ButtonClickedEvent baseChooseEvent;

        private static GameObject FavoriteButton;

        private static GameObject FavoriteButtonNew;

        public static GameObject MigrateButton;

        private static Button FavoriteButtonNewButton;

        private static Text FavoriteButtonNewText;

        public static GameObject pageAvatar;

        private static PageAvatar currPageAvatar;

        private static bool error;

        private static bool errorWarned;

        private static bool Searching;

        public static Il2CppSystem.Collections.Generic.List<ApiAvatar> LoadedAvatars;

        private static Il2CppSystem.Collections.Generic.List<ApiAvatar> SearchedAvatars;

        private static bool menuJustActivated;

        private static UiInputField searchBox;

        private static UnityAction<string> searchBoxAction;

        private static MethodInfo renderElementMethod;
    }
}
