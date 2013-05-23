﻿@model TopicModel
@using $rootnamespace$.Models.Topics;
@{
    var isPopup = ViewBag.IsPopup;
    if (isPopup == null || isPopup == false)
    {
        Layout = "~/Views/Shared/_Root.cshtml";
    }
    else
    {
        /*pop-up windows*/
        Layout = "~/Views/Shared/_RootPopup.cshtml";
    }

    if (!Model.IsPasswordProtected)
    {
        Html.AddTitleParts(!String.IsNullOrEmpty(Model.MetaTitle) ? Model.MetaTitle : Model.Title);
        Html.AddMetaDescriptionParts(Model.MetaDescription);
        Html.AddMetaKeywordParts(Model.MetaKeywords);
    }
}
@if (Model.IsPasswordProtected)
{
    // For popup topics, use a minimal layout that includes the Ajax and jQuery scripts
    if (isPopup != null && isPopup == true)
    {
        Layout = "~/Views/Shared/_RootPopup.cshtml";
    }    
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ph-topic').hide();
            $('#ph-password #password').select().focus();
        });
        function OnAuthenticateSuccess(context) {
            if (context.Authenticated) {
                $('#ph-title .page-title h1').html(context.Title);
                if ($('#ph-title .page-title h1').text().length == 0) {
                    $('#ph-title').hide();
                }
                $('#ph-topic .page-body').html(context.Body);
                $('#ph-password').hide();
                $('#ph-topic').show();
            }
            else {
                $('#password-error').text(context.Error);
                $('#ph-password #password').select().focus();
            }
        }
    </script>
    <div id="ph-password">
        @using (Ajax.BeginRouteForm("TopicAuthenticate", new AjaxOptions
        {
            HttpMethod = "Post",
            OnSuccess = "OnAuthenticateSuccess",
            LoadingElementId = "authenticate-progress"
        }))
        {
            @Html.HiddenFor(model => model.Id)
            <div class="enter-password-title">
                Nhập mật khẩu
            </div>
            <div class="enter-password-form">
                @Html.Password("password")
                <input type="submit" value="Chấp nhận" class="button-1 topic-password-button"/>
                <span id="authenticate-progress" style="display: none;" class="please-wait">Vui lòng chờ...</span>
            </div>
            <div class="password-error">
                <span id="password-error"></span>
            </div>
        }
    </div>
}
<div class="page topic-page" id="ph-topic">
    <div id="ph-title">
        <div class="page-title">
            <h1>
                @Model.Title</h1>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="page-body">
        @Html.Raw(Model.Body)
    </div>
</div>