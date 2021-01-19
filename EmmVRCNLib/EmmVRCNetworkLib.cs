using emmVRC.Libraries;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace BetterEmmVRC.EmmVRCNLib
{
    //You can really tell what I've coded by how neat it is vs Emilia's cancer, ey?
    internal class EmmVRCNetworkLib
    {
        /// <summary>
        /// Used To Send Requests
        /// </summary>
        internal HttpClient EmmVRCNetworkClient = new HttpClient();

        /// <summary>
        /// Ecks Dee
        /// </summary>
        internal readonly string VersionNumber = "2.5.0";

        internal string loginKey;

        /// <summary>
        /// No, This Is Not Your VRC AuthToken, Emilia Is Just Shit At Naming
        /// </summary>
        private string _authToken;
        internal string authToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                EmmVRCNetworkClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal EmmVRCNetworkLib()
        {
            EmmVRCNetworkClient.DefaultRequestHeaders.Accept.Clear();
            EmmVRCNetworkClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            EmmVRCNetworkClient.DefaultRequestHeaders.UserAgent.ParseAdd("emmVRC/1.0 (Client; emmVRCClient/" + VersionNumber + ")");
        }

        internal readonly string baseURL = "https://thetrueyoshifan.com:3000";

        /// <summary>
        /// Login Type
        /// </summary>
        private enum LoginType
        {
            UserID,
            Bearer,
            Password
        }

        internal string Response = "";

        /// <summary>
        /// Logs You Into The EmmVRCNetwork
        /// </summary>
        /// <param name="password">
        /// Password For Login, If Needed
        /// </param>
        internal bool Login(string password = "")
        {
            Response = "undefined";

            #region What To Login With?

            //Default
            var loginType = LoginType.UserID;

            var createFile = (Authentication.Exists(APIUser.CurrentUser.id) ? "0" : "1");

            if (!string.IsNullOrWhiteSpace(password))
            {
                loginType = LoginType.Password;
            }
            else if (Authentication.Exists(APIUser.CurrentUser.id))
            {
                loginType = LoginType.Bearer;
            }

            #endregion

            #region Login Phase

            var LoginURL = baseURL + "/api/authentication/login";

            var Content = new Dictionary<string, string>
            {
                ["username"] = APIUser.CurrentUser.id,
                ["name"] = APIUser.CurrentUser.displayName,
                ["password"] = password ?? "",
                ["loginKey"] = createFile
            };


            //Apply LoginType
            switch (loginType)
            {
                case LoginType.UserID:
                    Content["password"] = APIUser.CurrentUser.id;
                    loginKey = APIUser.CurrentUser.id;
                    break;
                case LoginType.Bearer:
                    Content["password"] = Authentication.ReadTokenFile(APIUser.CurrentUser.id);
                    loginKey = Authentication.ReadTokenFile(APIUser.CurrentUser.id);
                    break;
                case LoginType.Password:
                    loginKey = password ?? "";
                    break;
                default:
                    return false;
            }

            var JsonContent = TinyJSON.Encoder.Encode(Content);

            if (string.IsNullOrWhiteSpace(JsonContent))
            {
                MelonLogger.LogError("JsonContent Is Null!");
                return false;
            }

            //This Is Where Shit Goes Wee Woo Bois
            try
            {
                var Result = EmmVRCNetworkClient.PostAsync(LoginURL,
                    new StringContent(JsonContent, Encoding.UTF8, "application/json")).Result;

                if (Result == null)
                {
                    MelonLogger.LogError("Result Is Null!");
                    return false;
                }

                string ContentResult = Result.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrWhiteSpace(ContentResult))
                {
                    MelonLogger.LogError("ContentResult Is Null!");
                    return false;
                }

                Response = Result.StatusCode + ": " + Enum.GetName(typeof(HttpStatusCode), Result.StatusCode) + " -> " + ContentResult ?? "";

                var JsonResult = TinyJSON.Decoder.Decode(ContentResult.ToString());

                authToken = JsonResult["token"];

                if ((bool)JsonResult["reset"])
                {
                    RequestNewPin();
                }

                if (createFile == "1" && loginKey != APIUser.CurrentUser.id)
                {
                    try
                    {
                        Authentication.CreateTokenFile(APIUser.CurrentUser.id, JsonResult["loginKey"]);

                        loginKey = JsonResult["loginKey"];
                    }
                    catch (System.Exception ex)
                    {
                        MelonLogger.LogError(ex.ToString());
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError("Login Catch Hit! - " + Response + " -> " + ex.Message);

                if (Response.ToLower().Contains("unauthorized"))
                {
                    //Feck.
                    if (Response.ToLower().Contains("banned"))
                    {
                        //You Are Banned!
                    }

                    //Assume The Token Is Invalid
                    if (Authentication.Exists(APIUser.CurrentUser.id))
                    {
                        Authentication.DeleteTokenFile(APIUser.CurrentUser.id);
                    }
                }
                else if (Response.ToLower().Contains("forbidden"))
                {
                    //Tried To Log In Too Fast
                }
                else
                {
                    //Oof
                }
            }

            #endregion

            return false;
        }

        //Taken From emmVRC's Cancer-Code, Cba Fam
        private void RequestNewPin()
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Please enter your new pin", "", InputField.InputType.Standard, true, "Set Pin", (Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>)delegate (string pin, Il2CppSystem.Collections.Generic.List<KeyCode> keyk, Text tx)
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Please confirm your new pin", "", InputField.InputType.Standard, true, "Set Pin", (Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>)delegate (string pin2, Il2CppSystem.Collections.Generic.List<KeyCode> keyk2, Text tx2)
                {
                    if (pin == pin2)
                    {
                        Login(pin2);
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                        return;
                    }
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "The pins you entered did not match. Please try again.", "Okay", RequestNewPin, null);
                }, null, "Enter pin....", false, null);
            }, null, "Enter pin....", false, null);
        }
    }
}
