using System;
using System.Linq;
using System.Reflection;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Libraries
{
    public static class PopupManagerUtils
    {
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager)
        {
            VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP");
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, System.Action<VRCUiPopup> onCreated = null)
        {
            PopupManagerUtils.ShowUiStandardPopup1(title, content, onCreated);
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, System.Action buttonAction, System.Action<VRCUiPopup> onCreated = null)
        {
            PopupManagerUtils.ShowUiStandardPopup2(title, content, buttonText, buttonAction, onCreated);
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, System.Action button1Action, string button2Text, System.Action button2Action, System.Action<VRCUiPopup> onCreated = null)
        {
            PopupManagerUtils.ShowUiStandardPopup3(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);
        }

        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, System.Action buttonAction, System.Action<VRCUiPopup> onCreated = null)
        {
            PopupManagerUtils.ShowUiStandardPopupV21(title, content, buttonText, buttonAction, onCreated);
        }

        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, System.Action button1Action, string button2Text, System.Action button2Action, System.Action<VRCUiPopup> onCreated = null)
        {
            PopupManagerUtils.ShowUiStandardPopupV22(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);
        }

        public static void ShowInputPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string preFilledText, InputField.InputType inputType, bool keypad, string buttonText, Il2CppSystem.Action<string, List<KeyCode>, Text> buttonAction, Il2CppSystem.Action cancelAction, string boxText = "Enter text....", bool closeOnAccept = true, System.Action<VRCUiPopup> onCreated = null)
        {
            PopupManagerUtils.ShowUiInputPopup(title, preFilledText, inputType, keypad, buttonText, buttonAction, cancelAction, boxText, closeOnAccept, onCreated);
        }

        public static void ShowAlert(this VRCUiPopupManager vrcUiPopupManager, string title, string content, float timeout)
        {
            PopupManagerUtils.ShowUiAlertPopup(title, content, timeout);
        }

        public static PopupManagerUtils.ShowUiInputPopupAction ShowUiInputPopup
        {
            get
            {
                if (PopupManagerUtils.ourShowUiInputPopupAction != null)
                {
                    return PopupManagerUtils.ourShowUiInputPopupAction;
                }
                MethodInfo method = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(delegate (MethodInfo it)
                {
                    if (it.GetParameters().Length == 10)
                    {
                        return XrefScanner.XrefScan(it).Any(delegate (XrefInstance jt)
                        {
                            if (jt.Type == XrefType.Global)
                            {
                                Il2CppSystem.Object @object = jt.ReadAsObject();
                                return ((@object != null) ? @object.ToString() : null) == "UserInterface/MenuContent/Popups/InputPopup";
                            }
                            return false;
                        });
                    }
                    return false;
                });
                PopupManagerUtils.ourShowUiInputPopupAction = (PopupManagerUtils.ShowUiInputPopupAction)System.Delegate.CreateDelegate(typeof(PopupManagerUtils.ShowUiInputPopupAction), VRCUiPopupManager.prop_VRCUiPopupManager_0, method);
                return PopupManagerUtils.ourShowUiInputPopupAction;
            }
        }

        public static PopupManagerUtils.ShowUiStandardPopup1Action ShowUiStandardPopup1
        {
            get
            {
                if (PopupManagerUtils.ourShowUiStandardPopup1Action != null)
                {
                    return PopupManagerUtils.ourShowUiStandardPopup1Action;
                }
                MethodInfo method = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(delegate (MethodInfo it)
                {
                    if (it.GetParameters().Length == 3)
                    {
                        return XrefScanner.XrefScan(it).Any(delegate (XrefInstance jt)
                        {
                            if (jt.Type == XrefType.Global)
                            {
                                Il2CppSystem.Object @object = jt.ReadAsObject();
                                return ((@object != null) ? @object.ToString() : null) == "UserInterface/MenuContent/Popups/StandardPopup";
                            }
                            return false;
                        });
                    }
                    return false;
                });
                PopupManagerUtils.ourShowUiStandardPopup1Action = (PopupManagerUtils.ShowUiStandardPopup1Action)System.Delegate.CreateDelegate(typeof(PopupManagerUtils.ShowUiStandardPopup1Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, method);
                return PopupManagerUtils.ourShowUiStandardPopup1Action;
            }
        }

        public static PopupManagerUtils.ShowUiStandardPopup2Action ShowUiStandardPopup2
        {
            get
            {
                if (PopupManagerUtils.ourShowUiStandardPopup2Action != null)
                {
                    return PopupManagerUtils.ourShowUiStandardPopup2Action;
                }
                MethodInfo method = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(delegate (MethodInfo it)
                {
                    if (it.GetParameters().Length == 5)
                    {
                        return XrefScanner.XrefScan(it).Any(delegate (XrefInstance jt)
                        {
                            if (jt.Type == XrefType.Global)
                            {
                                Il2CppSystem.Object @object = jt.ReadAsObject();
                                return ((@object != null) ? @object.ToString() : null) == "UserInterface/MenuContent/Popups/StandardPopup";
                            }
                            return false;
                        });
                    }
                    return false;
                });
                PopupManagerUtils.ourShowUiStandardPopup2Action = (PopupManagerUtils.ShowUiStandardPopup2Action)System.Delegate.CreateDelegate(typeof(PopupManagerUtils.ShowUiStandardPopup2Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, method);
                return PopupManagerUtils.ourShowUiStandardPopup2Action;
            }
        }

        public static PopupManagerUtils.ShowUiStandardPopup3Action ShowUiStandardPopup3
        {
            get
            {
                if (PopupManagerUtils.ourShowUiStandardPopup3Action != null)
                {
                    return PopupManagerUtils.ourShowUiStandardPopup3Action;
                }
                MethodInfo method = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(delegate (MethodInfo it)
                {
                    if (it.GetParameters().Length == 7)
                    {
                        return XrefScanner.XrefScan(it).Any(delegate (XrefInstance jt)
                        {
                            if (jt.Type == XrefType.Global)
                            {
                                Il2CppSystem.Object @object = jt.ReadAsObject();
                                return ((@object != null) ? @object.ToString() : null) == "UserInterface/MenuContent/Popups/StandardPopup";
                            }
                            return false;
                        });
                    }
                    return false;
                });
                PopupManagerUtils.ourShowUiStandardPopup3Action = (PopupManagerUtils.ShowUiStandardPopup3Action)System.Delegate.CreateDelegate(typeof(PopupManagerUtils.ShowUiStandardPopup3Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, method);
                return PopupManagerUtils.ourShowUiStandardPopup3Action;
            }
        }

        public static PopupManagerUtils.ShowUiStandardPopupV21Action ShowUiStandardPopupV21
        {
            get
            {
                if (PopupManagerUtils.ourShowUiStandardPopupV21Action != null)
                {
                    return PopupManagerUtils.ourShowUiStandardPopupV21Action;
                }
                MethodInfo method = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(delegate (MethodInfo it)
                {
                    if (it.GetParameters().Length == 5)
                    {
                        return XrefScanner.XrefScan(it).Any(delegate (XrefInstance jt)
                        {
                            if (jt.Type == XrefType.Global)
                            {
                                Il2CppSystem.Object @object = jt.ReadAsObject();
                                return ((@object != null) ? @object.ToString() : null) == "UserInterface/MenuContent/Popups/StandardPopupV2";
                            }
                            return false;
                        });
                    }
                    return false;
                });
                PopupManagerUtils.ourShowUiStandardPopupV21Action = (PopupManagerUtils.ShowUiStandardPopupV21Action)System.Delegate.CreateDelegate(typeof(PopupManagerUtils.ShowUiStandardPopupV21Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, method);
                return PopupManagerUtils.ourShowUiStandardPopupV21Action;
            }
        }

        public static PopupManagerUtils.ShowUiStandardPopupV22Action ShowUiStandardPopupV22
        {
            get
            {
                if (PopupManagerUtils.ourShowUiStandardPopupV22Action != null)
                {
                    return PopupManagerUtils.ourShowUiStandardPopupV22Action;
                }
                MethodInfo method = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(delegate (MethodInfo it)
                {
                    if (it.GetParameters().Length == 7)
                    {
                        return XrefScanner.XrefScan(it).Any(delegate (XrefInstance jt)
                        {
                            if (jt.Type == XrefType.Global)
                            {
                                Il2CppSystem.Object @object = jt.ReadAsObject();
                                return ((@object != null) ? @object.ToString() : null) == "UserInterface/MenuContent/Popups/StandardPopupV2";
                            }
                            return false;
                        });
                    }
                    return false;
                });
                PopupManagerUtils.ourShowUiStandardPopupV22Action = (PopupManagerUtils.ShowUiStandardPopupV22Action)System.Delegate.CreateDelegate(typeof(PopupManagerUtils.ShowUiStandardPopupV22Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, method);
                return PopupManagerUtils.ourShowUiStandardPopupV22Action;
            }
        }

        public static PopupManagerUtils.ShowUiAlertPopupAction ShowUiAlertPopup
        {
            get
            {
                if (PopupManagerUtils.ourShowUiAlertPopupAction != null)
                {
                    return PopupManagerUtils.ourShowUiAlertPopupAction;
                }
                MethodInfo method = typeof(VRCUiPopupManager).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(delegate (MethodInfo it)
                {
                    if (it.GetParameters().Length == 3)
                    {
                        return XrefScanner.XrefScan(it).Any(delegate (XrefInstance jt)
                        {
                            if (jt.Type == XrefType.Global)
                            {
                                Il2CppSystem.Object @object = jt.ReadAsObject();
                                return ((@object != null) ? @object.ToString() : null) == "UserInterface/MenuContent/Popups/AlertPopup";
                            }
                            return false;
                        });
                    }
                    return false;
                });
                PopupManagerUtils.ourShowUiAlertPopupAction = (PopupManagerUtils.ShowUiAlertPopupAction)System.Delegate.CreateDelegate(typeof(PopupManagerUtils.ShowUiAlertPopupAction), VRCUiPopupManager.prop_VRCUiPopupManager_0, method);
                return PopupManagerUtils.ourShowUiAlertPopupAction;
            }
        }

        private static PopupManagerUtils.ShowUiInputPopupAction ourShowUiInputPopupAction;

        private static PopupManagerUtils.ShowUiStandardPopup1Action ourShowUiStandardPopup1Action;

        private static PopupManagerUtils.ShowUiStandardPopup2Action ourShowUiStandardPopup2Action;

        private static PopupManagerUtils.ShowUiStandardPopup3Action ourShowUiStandardPopup3Action;

        private static PopupManagerUtils.ShowUiStandardPopupV21Action ourShowUiStandardPopupV21Action;

        private static PopupManagerUtils.ShowUiStandardPopupV22Action ourShowUiStandardPopupV22Action;

        private static PopupManagerUtils.ShowUiAlertPopupAction ourShowUiAlertPopupAction;

        public delegate void ShowUiInputPopupAction(string title, string initialText, InputField.InputType inputType, bool isNumeric, string confirmButtonText, Il2CppSystem.Action<string, List<KeyCode>, Text> onComplete, Il2CppSystem.Action onCancel, string placeholderText = "Enter text...", bool closeAfterInput = true, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        public delegate void ShowUiStandardPopup1Action(string title, string body, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        public delegate void ShowUiStandardPopup2Action(string title, string body, string middleButtonText, Il2CppSystem.Action middleButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        public delegate void ShowUiStandardPopup3Action(string title, string body, string leftButtonText, Il2CppSystem.Action leftButtonAction, string rightButtonText, Il2CppSystem.Action rightButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        public delegate void ShowUiStandardPopupV21Action(string title, string body, string middleButtonText, Il2CppSystem.Action middleButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        public delegate void ShowUiStandardPopupV22Action(string title, string body, string leftButtonText, Il2CppSystem.Action leftButtonAction, string rightButtonText, Il2CppSystem.Action rightButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        public delegate void ShowUiAlertPopupAction(string title, string body, float timeout);
    }
}
