using System;
using System.Linq;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace emmVRC.Libraries
{
    public class QuickMenuUtils
    {
        public static BoxCollider QuickMenuBackground()
        {
            if (QuickMenuUtils.QuickMenuBackgroundReference == null)
            {
                QuickMenuUtils.QuickMenuBackgroundReference = QuickMenuUtils.GetQuickMenuInstance().GetComponent<BoxCollider>();
            }
            return QuickMenuUtils.QuickMenuBackgroundReference;
        }

        public static void ResizeQuickMenuCollider()
        {
            if (QuickMenuUtils.QuickMenuColliderPositionNormal == Vector3.zero && QuickMenuUtils.QuickMenuColliderSizeNormal == Vector3.zero)
            {
                QuickMenuUtils.QuickMenuColliderSizeNormal = QuickMenuUtils.QuickMenuBackground().size;
                QuickMenuUtils.QuickMenuColliderPositionNormal = QuickMenuUtils.QuickMenuBackground().center;
                QuickMenuUtils.QuickMenuBackground().size = new Vector3(QuickMenuUtils.QuickMenuColliderSizeNormal.x, QuickMenuUtils.QuickMenuColliderSizeNormal.y + QuickMenuUtils.QuickMenuColliderSizeNormal.y / 4f, QuickMenuUtils.QuickMenuColliderSizeNormal.z);
                QuickMenuUtils.QuickMenuBackground().center = new Vector3(QuickMenuUtils.QuickMenuColliderPositionNormal.x, QuickMenuUtils.QuickMenuColliderPositionNormal.y + QuickMenuUtils.QuickMenuColliderPositionNormal.y / 8f, QuickMenuUtils.QuickMenuColliderPositionNormal.z);
                return;
            }
            QuickMenuUtils.QuickMenuBackground().size = QuickMenuUtils.QuickMenuColliderSizeNormal;
            QuickMenuUtils.QuickMenuBackground().center = QuickMenuUtils.QuickMenuColliderPositionNormal;
            QuickMenuUtils.QuickMenuColliderSizeNormal = Vector3.zero;
            QuickMenuUtils.QuickMenuColliderPositionNormal = Vector3.zero;
        }

        public static GameObject SingleButtonTemplate()
        {
            if (QuickMenuUtils.SingleButtonReference == null)
            {
                QuickMenuUtils.SingleButtonReference = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/WorldsButton").gameObject;
            }
            return QuickMenuUtils.SingleButtonReference;
        }

        public static GameObject ToggleButtonTemplate()
        {
            if (QuickMenuUtils.ToggleButtonReference == null)
            {
                QuickMenuUtils.ToggleButtonReference = QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/BlockButton").gameObject;
            }
            return QuickMenuUtils.ToggleButtonReference;
        }

        public static Transform NestedMenuTemplate()
        {
            if (QuickMenuUtils.NestedButtonReference == null)
            {
                QuickMenuUtils.NestedButtonReference = QuickMenuUtils.GetQuickMenuInstance().transform.Find("CameraMenu");
            }
            return QuickMenuUtils.NestedButtonReference;
        }

        public static QuickMenu GetQuickMenuInstance()
        {
            return QuickMenu.prop_QuickMenu_0;
        }

        public static VRCUiManager GetVRCUiMInstance()
        {
            return VRCUiManager.prop_VRCUiManager_0;
        }

        public static void ShowQuickmenuPage(string pagename)
        {
            QuickMenu quickMenuInstance = QuickMenuUtils.GetQuickMenuInstance();
            Transform transform = (quickMenuInstance != null) ? quickMenuInstance.transform.Find(pagename) : null;
            if (transform == null)
            {
            }
            if (QuickMenuUtils.currentPageGetter == null)
            {
                GameObject gameObject = quickMenuInstance.transform.Find("ShortcutMenu").gameObject;
                if (!gameObject.activeInHierarchy)
                {
                    gameObject = quickMenuInstance.transform.Find("UserInteractMenu").gameObject;
                }
                FieldInfo[] array = (from fi in Il2CppType.Of<QuickMenu>().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                                     where fi.FieldType == Il2CppType.Of<GameObject>()
                                     select fi).ToArray<FieldInfo>();
                int num = 0;
                foreach (FieldInfo fieldInfo in array)
                {
                    Il2CppSystem.Object value = fieldInfo.GetValue(quickMenuInstance);
                    if (((value != null) ? value.TryCast<GameObject>() : null) == gameObject && ++num == 2)
                    {
                        QuickMenuUtils.currentPageGetter = fieldInfo;
                        break;
                    }
                }
                if (QuickMenuUtils.currentPageGetter == null)
                {
                    return;
                }
            }
            Il2CppSystem.Object value2 = QuickMenuUtils.currentPageGetter.GetValue(quickMenuInstance);
            if (value2 != null)
            {
                value2.Cast<GameObject>().SetActive(false);
            }
            QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar").gameObject.SetActive(pagename == "ShortcutMenu");
            QuickMenuUtils.GetQuickMenuInstance().field_Private_QuickMenuContextualDisplay_0.Method_Public_Void_EnumNPublicSealedvaUnNoToUs7vUsNoUnique_0(QuickMenuContextualDisplay.EnumNPublicSealedvaUnNoToUs7vUsNoUnique.NoSelection);
            transform.gameObject.SetActive(true);
            QuickMenuUtils.currentPageGetter.SetValue(quickMenuInstance, transform.gameObject);
            if (pagename == "ShortcutMenu")
            {
                QuickMenuUtils.SetIndex(0);
                return;
            }
            if (pagename == "UserInteractMenu")
            {
                QuickMenuUtils.SetIndex(3);
                return;
            }
            QuickMenuUtils.SetIndex(-1);
        }

        public static void SetIndex(int index)
        {
            QuickMenuUtils.GetQuickMenuInstance().field_Private_Int32_0 = index;
        }

        private static BoxCollider QuickMenuBackgroundReference;

        private static GameObject SingleButtonReference;

        private static GameObject ToggleButtonReference;

        private static Transform NestedButtonReference;

        private static QuickMenu quickmenuInstance;

        private static VRCUiManager vrcuimInstance;

        private static Vector3 QuickMenuColliderSizeNormal = Vector3.zero;

        private static Vector3 QuickMenuColliderPositionNormal = Vector3.zero;

        private static FieldInfo currentPageGetter;
    }
}
