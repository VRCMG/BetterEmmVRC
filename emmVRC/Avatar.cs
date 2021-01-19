using System;
using System.Text;
using Il2CppSystem.Collections.Generic;
using VRC.Core;

namespace emmVRC.Network.Objects
{
    public class Avatar : SerializedObject
    {
        public Avatar()
        {
        }

        public Avatar(ApiAvatar vrcAvatar)
        {
            this.avatar_id = vrcAvatar.id;
            this.avatar_name = vrcAvatar.name;
            this.avatar_asset_url = vrcAvatar.assetUrl;
            this.avatar_author_id = vrcAvatar.authorId;
            this.avatar_author_name = vrcAvatar.authorName;
            this.avatar_category = "";
            this.avatar_thumbnail_image_url = vrcAvatar.thumbnailImageUrl;
            this.avatar_supported_platforms = (int)vrcAvatar.supportedPlatforms;
            this.avatar_public = ((vrcAvatar.releaseStatus == "private") ? 0 : ((vrcAvatar.releaseStatus == "public") ? 1 : 255));
        }

        public List<string> avatar_tags
        {
            get
            {
                List<string> list = new List<string>();
                list.Add("avatar");
                return list;
            }
        }

        public ApiAvatar apiAvatar()
        {
            return new ApiAvatar
            {
                name = Encoding.UTF8.GetString(Convert.FromBase64String(this.avatar_name)),
                id = this.avatar_id,
                assetUrl = this.avatar_asset_url,
                thumbnailImageUrl = this.avatar_thumbnail_image_url,
                authorId = this.avatar_author_id,
                authorName = Encoding.UTF8.GetString(Convert.FromBase64String(this.avatar_author_name)),
                description = Encoding.UTF8.GetString(Convert.FromBase64String(this.avatar_name)),
                releaseStatus = "public",
                unityVersion = "2018.4.20f1",
                version = 1,
                apiVersion = 1,
                Endpoint = "avatars",
                Populated = false,
                assetVersion = new AssetVersion("2018.4.20f1", 0),
                tags = this.avatar_tags
            };
        }

        public string avatar_name = "";

        public string avatar_id = "";

        public string avatar_asset_url = "";

        public string avatar_thumbnail_image_url = "";

        public string avatar_author_id = "";

        public string avatar_category = "";

        public string avatar_author_name = "";

        public int avatar_public = 1;

        public int avatar_supported_platforms = 3;
    }
}
